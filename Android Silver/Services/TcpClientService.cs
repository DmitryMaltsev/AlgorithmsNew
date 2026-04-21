using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;


using System;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace Android_Silver.Services
{
    public class TcpClientService
    {
        private NetworkStream _stream;
        private EthernetEntities _ethernetEntities { get; set; }
        private ModesEntities _modesEntities { get; set; }
        private FBs _fbs { get; set; }
        private PicturesSet _pictureSet { get; set; }

        private FileSystemService _fileSystemService { get; set; }

        public bool IsConnecting { get; private set; } = false;
        public bool IsSending { get; set; } = false;
        public bool IsRecieving { get; private set; } = false;

        private string _messageToServer = String.Empty;
        public string MessageToServer
        {
            get { return _messageToServer; }
            set { _messageToServer = value; }
        }
        public int ResieveCounter { get; set; }

        public Action<int> SetMode1Action { get; set; }

        private ActivePagesEntities _activePageEntities { get; set; }

        MenusEntities _menusEntities { get; set; }

        MathService _mathService { get; set; }

        private ServiceActivePagesEntities _servActivePageEntities { get; set; }

        public Action ClientDisconnected { get; set; }

        private FilesEntities _filesEntities;

        private ReadWriteService _readWriteService;

        public List<ushort> WriteValuesList = new List<ushort>();
        private byte[] _readValuesArr;

        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            _activePageEntities = DIContainer.Resolve<ActivePagesEntities>();
            _menusEntities = DIContainer.Resolve<MenusEntities>();
            _fbs = DIContainer.Resolve<FBs>();
            _fileSystemService = DIContainer.Resolve<FileSystemService>();
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            _servActivePageEntities = DIContainer.Resolve<ServiceActivePagesEntities>();
            _mathService = DIContainer.Resolve<MathService>();
            _filesEntities = DIContainer.Resolve<FilesEntities>();
            _fbs.OtherSettings.SpecModeAction += SpecModeCallback;
            _readWriteService = DIContainer.Resolve<ReadWriteService>();
            //isConnected=TryConnect(tcpClient, ip, port, ref _systemMessage);
            //RecieveData(100,8);
        }

        public async Task Connect()
        {
            try
            {
                _ethernetEntities.Client = new TcpClient();
                _ethernetEntities.Client.ReceiveTimeout = 1000;
                _ethernetEntities.Client.SendTimeout = 1000;
                _ethernetEntities.IsConnected = false;
                IsConnecting = true;
                _ethernetEntities.CanTryToConnect = !IsConnecting;
                Task connectTask = _ethernetEntities.Client.ConnectAsync(_ethernetEntities.ConnectIP, _ethernetEntities.ConnectPort);
                if (await Task.WhenAny(connectTask, Task.Delay(3000)) != connectTask)
                {
                    _ethernetEntities.IsConnected = false;
                    _ethernetEntities.Client.Close();
                    IsConnecting = false;
                    _ethernetEntities.CanTryToConnect = !IsConnecting;
                    _ethernetEntities.EthernetMessage = "Не удалось подключиться модулю WI-FI";
                    if (connectTask.Status == TaskStatus.RanToCompletion || connectTask.Status == TaskStatus.Faulted
                        || connectTask.Status == TaskStatus.Canceled)
                    {
                        connectTask.Dispose();
                    }
                    throw new Exception("Не удалось подключиться модулю WI-FI");
                }
                else
                {
                    if (_ethernetEntities.PagesTab == 1)
                    {
                        _servActivePageEntities.SetActivePageState(SActivePageState.LoadingPage);
                    }
                    _ethernetEntities.EthernetMessage = "Успешное подключение";
                    _ethernetEntities.SystemMessage = "Успешное подключение";
                    _ethernetEntities.IsConnected = true;
                    IsConnecting = false;
                    _ethernetEntities.CanTryToConnect = !IsConnecting;
                }
                //await Task.Run(() =>
                //{
                //    Task.Delay(3000);
                //   _ethernetEntities.CanTryToConnect = true;
                //});
            }
            catch (Exception ex)
            {
                _ethernetEntities.Client.Close();
                _ethernetEntities.Client.Dispose();
                _ethernetEntities.SystemMessage = ex.Message;
                _ethernetEntities.IsConnected = false;
                IsConnecting = false;
                _ethernetEntities.CanTryToConnect = !IsConnecting;
            }
        }

        StringBuilder sbResult;

        public void SendRecieveTask(string val)
        {
            Task.Run(() =>
             {
                 while (_ethernetEntities.IsConnected)
                 {
                     GetMessagesState();
                     string messToClient = GetMessageToServer();
                     if (!IsSending)
                     {
                         //Процесс отправки/получения байт
                         if (SendCommand(messToClient))
                         {
                             _ethernetEntities.SystemMessage = "Успешная передача данных";
                         }
                         else
                         {
                             _ethernetEntities.SystemMessage = "Данные уже передаются";
                         }
                         Thread.Sleep(100);
                     }
                 }
             });
        }

        bool IsModbusDataRight(byte[] data, int length)
        {
            if (length < 3)
                return false;
            ushort crc = GetCRC(data, (ushort)(length - 2));
            byte lb = (byte)(crc & 0xff);
            byte hb = (byte)(crc >> 8);
            if (lb != data[length - 2] || hb != data[length - 1])
                return false;
            return true;
        }

        ushort GetCRC(byte[] data, ushort length)
        {
            ushort crc = 0XFFFF;
            for (ushort i = 0; i < length; i++)
            {
                crc ^= (ushort)data[i];
                for (ushort j = 0; j < 8; j++)
                {
                    bool lbSet = (crc & 0x0001) != 0;
                    crc >>= 1;
                    if (lbSet)
                        crc ^= 0xA001;
                }
            }
            return crc;
        }



        private void GetMessagesState()
        {
            //Условие при котором мы определям, что мы находимся на странице календаря или переходим на нее.
            bool isReadingShedTable = (_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage)
                && _modesEntities.CTimeModeValues.Count > 0 && _modesEntities.CTimeModeValues[0].Mode2Num == 3;
            if (_fbs.CUpdater.IsUpdate == 1)
            {
                _ethernetEntities.CMessageState = MessageStates.UpdaterMessage;
            }
            else
            if (_ethernetEntities.PagesTab == 0)
            {
                _ethernetEntities.CMessageState = MessageStates.UserMessage;
            }
            else
            if (_ethernetEntities.PagesTab == 1)
            {
                if (_ethernetEntities.CMessageState != MessageStates.ServiceMessage1)
                {
                    _ethernetEntities.CMessageState = MessageStates.ServiceMessage1;
                }
                else
                if (_ethernetEntities.CMessageState == MessageStates.ServiceMessage1)
                {
                    _ethernetEntities.CMessageState = MessageStates.ServiceMessage2;
                }
            }
        }

        private string GetMessageToServer()
        {
            string messToClient = MessageToServer;

            if (String.IsNullOrEmpty(messToClient) || _ethernetEntities.CMessageState == MessageStates.UpdaterMessage)
            {
                switch (_ethernetEntities.CMessageState)
                {
                    case MessageStates.UserMessage:
                        {
                            _readValuesArr = new byte[] { 1, 3, 0, 100, 0, 116 };
                            // messToClient = "0100,058\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage1:
                        {
                            //1 44 126
                            _readValuesArr = new byte[] { 1, 3, 1, 44, 0, 241 };
                            //messToClient = "0300,126\r\n";
                            //messToClient = "300,050\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage2:
                        {
                            //1 170 121
                            _readValuesArr = new byte[] { 1, 3, 1, 44, 0, 241 };
                            //messToClient = "0426,137\r\n";
                            //messToClient = "300,050\r\n";
                        }
                        break;
                    case MessageStates.UpdaterMessage:
                        {
                            messToClient = "";
                            for (int i = 0; i < _fbs.CUpdater.UseCharData.GetLength(1); i++)
                            {
                                messToClient += _fbs.CUpdater.UseCharData[_fbs.CUpdater.CurrentPacket - 1, i];
                            }
                            messToClient += "\r\n";
                        }
                        break;
                }
            }
            return messToClient;
        }

        private int _trySendcounter = 0;
        private bool SendCommand(string command)
        {
            IsSending = true;
            sbResult = new StringBuilder();
            _trySendcounter = 0;
            do
            {
                try
                {
                    _stream = _ethernetEntities.Client.GetStream();
                    ushort startAddress = 300;
                    //Работа прошивки
                    if (command.Length > 0 && _ethernetEntities.CMessageState == MessageStates.UpdaterMessage)
                    {
                        byte[] buffer = new byte[2200];
                        StreamWriter writer = new StreamWriter(_stream, Encoding.ASCII);
                        writer.WriteLine(command);
                        writer.Flush();
                        byte[] respData = new byte[2200];

                        int sBytes = _stream.Read(respData, 0, respData.Length);

                        string result = string.Empty;
                        do
                        {
                            result += (Encoding.ASCII.GetString(respData, 0, sBytes));
                        }
                        while (_stream.DataAvailable);

                        //Парсинг ответа
                        var result2 = result.Split(",");
                        byte id0 = 0;
                        byte id1 = 0;
                        ushort id = 0;
                        if (result2.Length == 2)
                        {
                            id0 = _mathService.GetByteFromHexChar(result2[0][0], result2[0][1]);
                            id1 = _mathService.GetByteFromHexChar(result2[0][2], result2[0][3]);
                            id = (ushort)((id0 << 8) | id1);
                        }

                        bool isRightPacket = false;
                        bool isAllDataSender = false;
                        if (result2[1].Contains("OK") && id == _fbs.CUpdater.CurrentPacket)
                        {
                            if (_fbs.CUpdater.CurrentPacket < _fbs.CUpdater.PacketsCount.Value)
                            {
                                isRightPacket = true;
                            }
                            else
                            {
                                isAllDataSender = true;
                            }
                        }
                        if (isAllDataSender)
                        {
                            _fbs.CUpdater.PacketsCount.Value = 0;
                            _fbs.CUpdater.IsUpdate = 0;
                            _fbs.CUpdater.CurrentPacket = 0;
                            _fbs.CUpdater.ResultPackets = "Успешно скопровано";
                        }
                        else
                        if (isRightPacket)
                        {
                            if (_fbs.CUpdater.CurrentPacket < _fbs.CUpdater.PacketsCount.Value)
                            {
                                _fbs.CUpdater.CurrentPacket += 1;
                                _fbs.CUpdater.ResendCounter = 0;
                                _fbs.CUpdater.ResultPackets = _fbs.CUpdater.CurrentPacket + "/" + _fbs.CUpdater.PacketsCount.Value;
                            }
                        }
                        else
                        {
                            _fbs.CUpdater.ResendCounter += 1;
                            if (_fbs.CUpdater.ResendCounter > 100)
                            {
                                _fbs.CUpdater.PacketsCount.Value = 0;
                                _fbs.CUpdater.IsUpdate = 0;
                                _fbs.CUpdater.CurrentPacket = 0;
                            }
                        }
                        _ethernetEntities.SystemMessage = result;
                    }
                    else
                    {
                        //Запись в контроллер
                        if (WriteValuesList.Count > 0)
                        {
                            byte[] buffer = new byte[8 + (WriteValuesList.Count - 1) * 2];

                            buffer[0] = 1;
                            buffer[1] = 16;
                            //Начальный регистр
                            buffer[2] = (byte)(WriteValuesList[0] >> 8);
                            buffer[3] = (byte)(WriteValuesList[0]);
                            //Количество регистров
                            buffer[4] = (byte)((WriteValuesList.Count - 1) >> 8);
                            buffer[5] = (byte)(WriteValuesList.Count - 1);
                            byte addressIndex = 6;
                            for (int i = 1; i < WriteValuesList.Count; i++)
                            {
                                buffer[addressIndex++] = (byte)(WriteValuesList[i] >> 8);
                                buffer[addressIndex++] = (byte)(WriteValuesList[i]);
                            }
                            ushort crc = GetCRC(buffer, (ushort)(buffer.Length - 2));
                            buffer[buffer.Length - 2] = (byte)(crc & 0xff);
                            buffer[buffer.Length - 1] = (byte)(crc >> 8);
                            _stream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            //Чтение из контроллера
                            ushort crc = GetCRC(_readValuesArr, (ushort)_readValuesArr.Length);
                            byte[] sendData = new byte[_readValuesArr.Length + 2];
                            for (int i = 0; i < _readValuesArr.Length; i++)
                            {
                                sendData[i] = _readValuesArr[i];
                            }
                            sendData[sendData.Length - 2] = (byte)(crc & 0xff);
                            sendData[sendData.Length - 1] = (byte)(crc >> 8);
                            _stream.Write(sendData, 0, sendData.Length);
                        }
                        //Принятие после отправленного пакета
                        byte[] data = new byte[2200];
                        int bytes = _stream.Read(data, 0, data.Length);
                        while (_stream.DataAvailable) { }
                    ;
                        if (IsModbusDataRight(data, (ushort)(bytes)))
                        {
                            byte func = data[1];
                            ushort addr1 = (ushort)(data[2] << 8);
                            ushort addr2 = data[3];
                            startAddress = (ushort)(data[2] << 8 | data[3]); //(ushort)(addr1 | addr2);//(ushort)(data[2] << 8) | (ushort)(data[3]);
                            ushort regsCount = (ushort)(data[4] << 8 | data[5]);
                            if (func == 3)
                            {
                                ushort bufferLength = 6;
                                for (ushort i = startAddress; i < startAddress + regsCount; i++)
                                {
                                    bufferLength = _readWriteService.EthernetData_Read(data, i, bufferLength, func);
                                }
                            }
                            else if (func == 16)
                            {
                                int addrCount = WriteValuesList.Count - 1;
                                data = new byte[addrCount * 2];
                                int index = 0;
                                for (int j = 1; j < WriteValuesList.Count; j++)
                                {
                                    data[index++] = (byte)(WriteValuesList[j] >> 8);
                                    data[index++] = (byte)WriteValuesList[j];
                                }
                                ushort startIndex = 0;
                                for (ushort i = startAddress; i < startAddress + addrCount; i++)
                                {
                                    startIndex = _readWriteService.EthernetData_Read(data, i+_menusEntities.WriteOffset, startIndex, func);
                                }
                                WriteValuesList.Clear();
                            }
                        }
                    }
                    IsSending = false;
                    ResieveCounter += 1;
                    return true;
                }
                catch
                {
                    // _stream?.Close();
                    _trySendcounter += 1;
                    //Thread.Sleep(200);
                    _ethernetEntities.SystemMessage = $"количество попыток {_trySendcounter}";

                }
                // && _ethernetEntities.MessageToServer==String.Empty
            }
            while (IsSending && _trySendcounter < 20 && MessageToServer == String.Empty && !_filesEntities.FileIsReading);
            IsSending = false;
            if (_trySendcounter == 20)
            {
                _pictureSet.SetPicureSetIfNeed(_pictureSet.LinkHeader, _pictureSet.LinkHeader.Default);
                if (_ethernetEntities.IsConnected == true)
                {
                    _ethernetEntities.SystemMessage = "Превышено максимальное количество попыток - 20";
                    _ethernetEntities.EthernetMessage = "Превышено максимальное количество попыток передачи данных. Подключитесь повторно";
                    ClientDisconnected?.Invoke();
                    _activePageEntities.SetActivePageState(ActivePageState.StartPage);
                    Disconnect();
                }
                else
                {
                    Disconnect();
                }
            }
            return false;
        }




        public void Disconnect()
        {
            _fbs.CUpdater.IsUpdate = 0;
            _fbs.CUpdater.CurrentPacket = 0;
            _fbs.CUpdater.PacketsCount.Value = 0;
            _ethernetEntities.IsConnected = false;
            //SystemMessage = "Соединение разорвано";
            _ethernetEntities.Client.Close();
            _ethernetEntities.Client.Dispose();
            _stream.Close();
            _stream.Dispose();
        }

    
        private bool StringToFloat(string val, int precision, ref float result)
        {

            if (float.TryParse(val, out float buffer))
            {
                result = buffer / 10;
                return true;
            }
            else
            {
                _ethernetEntities.SystemMessage = "Неверный формат сообщения";
                return false;
            }
        }

        /// <summary>
        /// Основной метод передачи данных на сервер
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        public void SetCommandToServer(int address, int[] values)
        {
            if (_ethernetEntities.CMessageState != MessageStates.UpdaterMessage)
            {
                WriteValuesList.Clear();
                WriteValuesList.Add((ushort)address);
                for (int i = 0; i < values.Length; i++)
                {
                    WriteValuesList.Add((ushort)values[i]);
                }
            }
        }

        #region Получение временнных режимов
        private void GetTModeDay(int m2Num, int tModeNum, string valueString)
        {
            if (int.TryParse(valueString, out int val))
            {
                if (val >= 0 && val <= 8)
                {
                    _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].DayNum = val;
                }
            }
        }

        private void GetTModeHours(int m2Num, int tModeNum, string valueString)
        {
            if (int.TryParse(valueString, out int val))
            {
                if (val >= 0 && val <= 23)
                {
                    _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].Hour = val;
                }
            }
        }

        private void GetTModeMinutes(int m2Num, int tModeNum, string valueString)
        {
            if (int.TryParse(valueString, out int val))
            {
                if (val >= 0 && val <= 60)
                {
                    _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].Minute = val;
                }
            }
        }

        private void GetTModeCMode1(int m2Num, int tModeNum, string valueString)
        {
            if (int.TryParse(valueString, out int val))
            {
                if (val >= 0 && val <= 5 && _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].CMode1.Num != val)
                {

                    _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].CMode1 = _modesEntities.Mode1ValuesList[val];
                }
            }
        }
        #endregion

        #region OtherSettings callbacks
        private void SpecModeCallback(bool val)
        {
            int specActive = val ? 1 : 0;
            int[] vals = { specActive };
            SetCommandToServer(166, vals);
        }
        #endregion
    }
}
