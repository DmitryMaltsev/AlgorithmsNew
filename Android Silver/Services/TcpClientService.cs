using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;

using System.Collections;
using System.IO;
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
        public bool IsSending { get; private set; } = false;
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
            _mathService= DIContainer.Resolve<MathService>();
            _fbs.OtherSettings.SpecModeAction += SpecModeCallback;
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
                         SendCommand(messToClient);
                         if (sbResult != null && sbResult.Length > 0)
                         {
                             if (_ethernetEntities.CMessageState == MessageStates.UpdaterMessage)
                             {
                                 string result = sbResult.ToString();
                                 var result2 = result.Split(",");
                                 byte id0 = 0;
                                 byte id1 = 0;
                                 ushort id = 0;
                                 if (result2.Length == 2)
                                 {
                                     id0 = _mathService.GetByteFromHexChar(result2[0][0], result2[0][1]);
                                     id1= _mathService.GetByteFromHexChar(result2[0][2], result2[0][3]);
                                     id=(ushort)((id0 << 8) | id1);
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
                                 }
                                 else
                                 if (isRightPacket)
                                 {
                                     if (_fbs.CUpdater.CurrentPacket < _fbs.CUpdater.PacketsCount.Value)
                                     {
                                         _fbs.CUpdater.CurrentPacket += 1;
                                         _fbs.CUpdater.ResendCounter = 0;
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
                                 _fbs.CUpdater.ResultPackets = _fbs.CUpdater.CurrentPacket + "/" + _fbs.CUpdater.PacketsCount.Value;
                                 _ethernetEntities.SystemMessage = result;
                             }
                             else
                             {
                                 List<Response> responseList = new();
                                 if (GetResponseData(sbResult, responseList))
                                 {
                                     foreach (var response in responseList)
                                     {
                                         GetValueByTag(response);
                                     }
                                     if (String.Compare(messToClient, MessageToServer, true) == 0)
                                         MessageToServer = String.Empty;
                                 }
                                 else
                                 {
                                     _ethernetEntities.SystemMessage = $"Данные не получены";
                                 }
                             }
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
            if (_activePageEntities.IsLoadingPage && _modesEntities.CTimeModeValues.Count > 0 &&
                                                           _modesEntities.CTimeModeValues[0].Mode2Num == 2)
            {
                _ethernetEntities.CMessageState = MessageStates.VacMessage;
            }
            else
                if (isReadingShedTable && _ethernetEntities.CMessageState != MessageStates.ShedMessage)
            {
                _ethernetEntities.CMessageState = MessageStates.ShedMessage;
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

            if (String.IsNullOrEmpty(messToClient))
            {
                switch (_ethernetEntities.CMessageState)
                {
                    case MessageStates.UserMessage:
                        {
                            messToClient = "0100,058\r\n";
                        }
                        break;
                    case MessageStates.VacMessage:
                        {
                            messToClient = "0167,016\r\n";
                        }
                        break;
                    case MessageStates.ShedMessage:
                        {
                            messToClient = "0183,112\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage1:
                        {
                            messToClient = "0300,121\r\n";
                            //messToClient = "300,050\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage2:
                        {
                            messToClient = "0421,121\r\n";
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
        private StringBuilder SendCommand(string command)
        {

            IsSending = true;
            sbResult = new StringBuilder();
            _trySendcounter = 0;
            do
            {
                try
                {
                    _stream = _ethernetEntities.Client.GetStream();
                    StreamWriter writer = new StreamWriter(_stream, Encoding.ASCII);
                    writer.WriteLine(command);
                    writer.Flush();
                    byte[] data = new byte[2200];
                    int bytes = _stream.Read(data, 0, data.Length);
                    do
                    {
                        sbResult.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    IsSending = false;
                    ResieveCounter += 1;
                }
                catch (Exception ex)
                {
                    _trySendcounter += 1;
                    Thread.Sleep(200);
                    _ethernetEntities.SystemMessage = $"количество попыток {_trySendcounter}";
                }
                // && _ethernetEntities.MessageToServer==String.Empty
            }
            while (IsSending && _trySendcounter < 20 && MessageToServer == String.Empty);
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
            return sbResult;
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
        }

        private bool GetResponseData(StringBuilder rSB, List<Response> response)
        {

            //Дописать проверку исключения, если ответ пришел в неверном формате.
            bool isRightResponse = true;
            string[] resultVals = rSB.ToString().Split(",");
            if (resultVals.Length >= 2)
            {
                if (int.TryParse(resultVals[1], out int valsCount) && int.TryParse(resultVals[0], out int startTag))
                {
                    for (ushort i = 0; i < valsCount; i++)
                    {
                        string valBuf = (i + 2) < resultVals.Length ? resultVals[i + 2] : String.Empty;
                        response.Add(new Response() { Tag = (ushort)(startTag + i), ValueString = valBuf });
                    }
                }
                else
                {
                    isRightResponse = false;
                    _ethernetEntities.SystemMessage = "Пришло неверное значение длиной меньше 2 слов";
                }
            }
            else
            {
                isRightResponse = false;
                _ethernetEntities.SystemMessage = "2 слово не является количеством адресов";
            }
            return isRightResponse;
        }

        private void GetValueByTag(Response resp)
        {
            int floatPrec = 1;
            float buff = 0;
            switch (resp.Tag)
            {
                case 100:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _fbs.CRecup.FreqHZ = val;
                        }
                    }
                    break;
                case 101:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _fbs.CEHSetPoints.CPower = val;
                        }
                    }
                    break;
                case 102:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CCommonSetPoints.SPTempR = buff;
                        }
                    }
                    break;
                case 103:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CSensors.OutdoorTemp.Value = buff;
                        }
                    }
                    break;
                case 104:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CSensors.SupTemp.Value = buff;
                        }
                    }
                    break;
                case 105:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CSensors.ExhaustTemp.Value = buff;
                        }
                    }
                    break;
                case 106:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CSensors.RoomTemp.Value = buff;
                        }
                    }
                    break;
                case 107:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref buff))
                        {
                            _fbs.CSensors.ReturnTemp.Value = buff;
                        }
                    }
                    break;
                //Режим 1
                case 108:
                    {

                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            if (val != _modesEntities.CMode1.Num)
                            {
                                _modesEntities.SetMode1ValuesByIndex(val);
                            }
                        }
                    }
                    break;
                //Режим 2
                case 109:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            _modesEntities.SetMode2ValuesByIndex(val);
                            // _ethernetEntities.Loaded = true;
                        }
                    }
                    break;
                #region Минимальный режим
                case 110:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].SypplySP = Val;
                        }
                    }
                    break;
                case 111:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].ExhaustSP = Val;
                        }
                    }
                    break;
                case 112:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].TempSP = Val;
                        }
                    }
                    break;
                case 113:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Нормальный режим
                case 114:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].SypplySP = Val;
                        }
                    }
                    break;
                case 115:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].ExhaustSP = Val;
                        }
                    }
                    break;
                case 116:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].TempSP = Val;
                        }
                    }
                    break;
                case 117:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Максимальный режим
                case 118:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].SypplySP = Val;
                        }
                    }
                    break;
                case 119:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].ExhaustSP = Val;
                        }
                    }
                    break;
                case 120:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].TempSP = Val;
                        }
                    }
                    break;
                case 121:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим кухни
                case 122:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].SypplySP = Val;
                        }
                    }
                    break;
                case 123:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].ExhaustSP = Val;
                        }
                    }
                    break;
                case 124:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].TempSP = Val;
                        }
                    }
                    break;
                case 125:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим отпуска
                case 126:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].SypplySP = Val;
                        }
                    }
                    break;
                case 127:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].ExhaustSP = Val;
                        }
                    }
                    break;
                case 128:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].TempSP = Val;
                        }
                    }
                    break;
                case 129:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Специальный режим
                case 130:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[6].SypplySP = Val;
                        }
                    }
                    break;
                case 131:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[6].ExhaustSP = Val;
                        }
                    }
                    break;
                case 132:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[6].TempSP = Val;
                        }
                    }
                    break;
                case 133:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[6].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                case 134:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 135:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (_fbs.CAlarms.Alarms1 != val)
                            {
                                _fbs.CAlarms.Alarms1 = val;
                                _fbs.CAlarms.Alarms1Changed = true;
                            }
                        }
                    }
                    break;
                case 136:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (_fbs.CAlarms.Alarms2 != val)
                            {
                                _fbs.CAlarms.Alarms2 = val;
                                _fbs.CAlarms.Alarms2Changed = true;
                            }
                        }
                        if (_fbs.CAlarms.Alarms1Changed || _fbs.CAlarms.Alarms2Changed)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                _fbs.CAlarms.AlarmsCollection.Clear();
                            });
                            BitArray bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms1);
                            _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 0);
                            bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms2);
                            _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 1);
                            _fbs.CAlarms.Alarms1Changed = false;
                            _fbs.CAlarms.Alarms2Changed = false;
                        }
                    }
                    break;
                //Режим по контакту
                case 137:
                    {
                        GetTModeCMode1(4, 0, resp.ValueString);
                    }
                    break;
                case 138:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (_fbs.CHumiditySPS.HumiditySP != val)
                            {
                                _fbs.CHumiditySPS.HumiditySP = val;
                            }
                        }
                    }
                    break;
                //Включен ли спец режим
                case 139:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            bool isSpec = val > 0 ? true : false;
                            if (_fbs.OtherSettings.IsSpecMode != isSpec)
                            {
                                _fbs.OtherSettings.IsSpecMode = isSpec;
                            }
                        }
                    }
                    break;
                //Включен ли режим многоэтажки
                case 140:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            bool isMF = val > 0 ? true : false;
                            if (_fbs.OtherSettings.IsMF != isMF)
                            {
                                _fbs.OtherSettings.IsMF = isMF;
                            }
                        }
                    }
                    break;
                //Текущий год
                case 141:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 99)
                            {
                                _fbs.CTime.Year = val;
                            }
                        }
                    }
                    break;
                //Текущий месяц
                case 142:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 12)
                            {
                                _fbs.CTime.Month = val;
                            }
                        }
                    }
                    break;
                //Текущий день
                case 143:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 31)
                            {
                                _fbs.CTime.Day = val;
                            }
                        }
                    }
                    break;
                //Текущий час
                case 144:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 60)
                            {
                                _fbs.CTime.Hour = val;
                            }
                        }
                    }
                    break;
                //Текущая минута
                case 145:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 60)
                            {
                                _fbs.CTime.Minute = val;
                                _fbs.CTime.SetTimerInterface();
                            }
                        }
                    }
                    break;
                //День недели
                case 146:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 6)
                            {
                                _fbs.CTime.DayOfWeek = val;
                            }
                        }
                    }
                    break;
                //День недели
                case 147:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 100)
                            {
                                _fbs.CRecup.Efficiency = val;
                            }
                        }
                    }
                    break;
                //Степень загрзнения фильтра
                case 148:
                    {

                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val <= 100)
                            {
                                _fbs.CFilterVals.FilterClearPercent = val;
                                if (_fbs.CFilterVals.FilterClearPercent >= 0 && _fbs.CFilterVals.FilterClearPercent <= 20)
                                {
                                    if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter0Header)
                                    {
                                        _pictureSet.FilterCurrentHeader = _pictureSet.Filter0Header;
                                    }
                                }
                                else
                                if (_fbs.CFilterVals.FilterClearPercent >= 21 && _fbs.CFilterVals.FilterClearPercent <= 40)
                                {
                                    if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter20Header)
                                    {
                                        _pictureSet.FilterCurrentHeader = _pictureSet.Filter20Header;
                                    }

                                }
                                else
                                if (_fbs.CFilterVals.FilterClearPercent >= 41 && _fbs.CFilterVals.FilterClearPercent <= 60)
                                {
                                    if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter40Header)
                                    {
                                        _pictureSet.FilterCurrentHeader = _pictureSet.Filter40Header;
                                    }

                                }
                                if (_fbs.CFilterVals.FilterClearPercent >= 61 && _fbs.CFilterVals.FilterClearPercent <= 80)
                                {
                                    if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter60Header)
                                    {
                                        _pictureSet.FilterCurrentHeader = _pictureSet.Filter60Header;
                                    }

                                }
                                else
                                  if (_fbs.CFilterVals.FilterClearPercent >= 81 && _fbs.CFilterVals.FilterClearPercent <= 100)
                                {
                                    if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter80Header)
                                    {
                                        _pictureSet.FilterCurrentHeader = _pictureSet.Filter80Header;
                                    }

                                }
                                //if (_fbs.CFilterVals.FilterClearPercent >= 0 && _fbs.CFilterVals.FilterClearPercent <= 20)
                                //{
                                //    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter100MainIcon, _pictureSet.Filter100MainIcon.Selected);
                                //}
                                //else
                                //{
                                //    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter100MainIcon, _pictureSet.Filter100MainIcon.Default);
                                //}
                            }
                        }
                    }
                    break;
                //Работа рекуператора
                case 149:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (_fbs.CAlarms.IsRecupLowTurns)
                            {
                                _pictureSet.RecuperatorHeaderCurrent = _pictureSet.RecuperatorHeaderAlarm;
                            }
                            else
                           if (val > 0)
                            {
                                _pictureSet.RecuperatorHeaderCurrent = _pictureSet.RecuperatorHeaderWork;
                            }
                            else
                            {
                                _pictureSet.RecuperatorHeaderCurrent = "";
                            }
                        }

                    }
                    break;
                //Работа электрокалорифера
                case 150:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 100)
                            {
                                if (val > 0)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.EHeaterHeader, _pictureSet.EHeaterHeader.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.EHeaterHeader, _pictureSet.EHeaterHeader.Default);
                                }
                            }
                        }
                    }
                    break;
                //Напряжение вентиляторов
                case 151:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 10)
                            {
                                if (val > 0)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.FanHeader, _pictureSet.FanHeader.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.FanHeader, _pictureSet.FanHeader.Default);
                                }
                            }
                        }
                    }
                    break;
                //Предуреждение активно
                case 152:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 1)
                            {
                                if (val > 0)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.AlarmMainIcon, _pictureSet.AlarmMainIcon.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.AlarmMainIcon, _pictureSet.AlarmMainIcon.Default);
                                }
                            }
                        }
                    }
                    break;
                case 153:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 65535)
                            {
                                _fbs.CFans.SFlow = val;
                            }
                        }
                    }
                    break;
                case 154:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 65535)
                            {
                                _fbs.CFans.EFlow = val;
                            }
                        }
                    }
                    break;
                case 155:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 100)
                            {
                                _fbs.CFans.SPercent = val;
                            }
                        }
                    }
                    break;
                case 156:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 100)
                            {
                                _fbs.CFans.EPercent = val;
                            }
                        }
                    }
                    break;
                case 157:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 1)
                            {
                                _fbs.CUpdater.IsUpdate = (byte)val;
                            }
                        }
                    }
                    break;
                //Данные о режиме отпуска
                case 167:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 168:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 169:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 170:
                    {
                        GetTModeCMode1(2, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 171:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 172:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 173:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 174:
                    {
                        GetTModeCMode1(2, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 175:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 176:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 177:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 178:
                    {
                        GetTModeCMode1(2, 2, resp.ValueString);

                    }
                    break;
                //Строка 4
                case 179:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 180:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 181:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 182:
                    {
                        GetTModeCMode1(2, 3, resp.ValueString);
                        if (_activePageEntities.IsLoadingPage)
                        {
                            _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                            _modesEntities.TTitle = "Расписание для отпуска";
                        }
                    }
                    break;
                //Расписание
                //Строка 1
                case 183:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 184:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 185:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 186:
                    {
                        GetTModeCMode1(3, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 187:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 188:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 189:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 190:
                    {
                        GetTModeCMode1(3, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 191:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 192:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 193:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 194:
                    {
                        GetTModeCMode1(3, 2, resp.ValueString);
                    }
                    break;
                //Строка 4
                case 195:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 196:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 197:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 198:
                    {
                        GetTModeCMode1(3, 3, resp.ValueString);
                    }
                    break;
                //Строка 5
                case 199:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = val;
                        }
                    }
                    break;
                case 200:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = val;
                        }
                    }
                    break;
                case 201:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = val;
                        }
                    }
                    break;
                case 202:
                    {
                        GetTModeCMode1(3, 4, resp.ValueString);
                    }
                    break;
                //Строка 6
                case 203:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = val;
                        }
                    }
                    break;
                case 204:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = val;
                        }
                    }
                    break;
                case 205:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = val;
                        }
                    }
                    break;
                case 206:
                    {
                        GetTModeCMode1(3, 5, resp.ValueString);
                    }
                    break;
                //Строка 7
                case 207:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = val;
                        }
                    }
                    break;
                case 208:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = val;
                        }
                    }
                    break;
                case 209:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = val;
                        }
                    }
                    break;
                case 210:
                    {
                        GetTModeCMode1(3, 6, resp.ValueString);
                    }
                    break;
                //Строка 8
                case 211:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].DayNum = val;
                        }
                    }
                    break;
                case 212:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Hour = val;
                        }
                    }
                    break;
                case 213:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Minute = val;
                        }
                    }
                    break;
                case 214:
                    {
                        GetTModeCMode1(3, 7, resp.ValueString);
                    }
                    break;
                //Строка 9
                case 215:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].DayNum = val;
                        }
                    }
                    break;
                case 216:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Hour = val;
                        }
                    }
                    break;
                case 217:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Minute = val;
                        }
                    }
                    break;
                case 218:
                    {
                        GetTModeCMode1(3, 8, resp.ValueString);
                    }
                    break;
                //Строка 10
                case 219:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].DayNum = val;
                        }
                    }
                    break;
                case 220:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Hour = val;
                        }
                    }
                    break;
                case 221:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Minute = val;
                        }
                    }
                    break;
                case 222:
                    {
                        GetTModeCMode1(3, 9, resp.ValueString);
                    }
                    break;
                //Строка 11
                case 223:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].DayNum = val;
                        }
                    }
                    break;
                case 224:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Hour = val;
                        }
                    }
                    break;
                case 225:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Minute = val;
                        }
                    }
                    break;
                case 226:
                    {
                        GetTModeCMode1(3, 10, resp.ValueString);
                    }
                    break;
                //Строка 12
                case 227:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].DayNum = val;
                        }
                    }
                    break;
                case 228:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Hour = val;
                        }
                    }
                    break;
                case 229:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Minute = val;
                        }
                    }
                    break;
                case 230:
                    {
                        GetTModeCMode1(3, 11, resp.ValueString);
                    }
                    break;
                //Строка 13
                case 231:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].DayNum = val;
                        }
                    }
                    break;
                case 232:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Hour = val;
                        }
                    }
                    break;
                case 233:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Minute = val;
                        }
                    }
                    break;
                case 234:
                    {
                        GetTModeCMode1(3, 12, resp.ValueString);
                    }
                    break;
                //Строка 14
                case 235:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].DayNum = val;
                        }
                    }
                    break;
                case 236:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Hour = val;
                        }
                    }
                    break;
                case 237:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Minute = val;
                        }
                    }
                    break;
                case 238:
                    {
                        GetTModeCMode1(3, 13, resp.ValueString);

                    }
                    break;
                //Строка 15
                case 239:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].DayNum = val;
                        }
                    }
                    break;
                case 240:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Hour = val;
                        }
                    }
                    break;
                case 241:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Minute = val;
                        }
                    }
                    break;
                case 242:
                    {
                        GetTModeCMode1(3, 14, resp.ValueString);
                    }
                    break;
                //Строка 16
                case 243:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].DayNum = val;
                        }
                    }
                    break;
                case 244:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Hour = val;
                        }
                    }
                    break;
                case 245:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Minute = val;
                        }
                    }
                    break;
                case 246:
                    {
                        GetTModeCMode1(3, 15, resp.ValueString);
                    }
                    break;
                //Строка 17
                case 247:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].DayNum = val;
                        }
                    }
                    break;
                case 248:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Hour = val;
                        }
                    }
                    break;
                case 249:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Minute = val;
                        }
                    }
                    break;
                case 250:
                    {
                        GetTModeCMode1(3, 16, resp.ValueString);
                    }
                    break;
                //Строка 18
                case 251:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].DayNum = val;
                        }
                    }
                    break;
                case 252:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Hour = val;
                        }
                    }
                    break;
                case 253:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Minute = val;
                        }
                    }
                    break;
                case 254:
                    {
                        GetTModeCMode1(3, 17, resp.ValueString);
                    }
                    break;
                //Строка 19
                case 255:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].DayNum = val;
                        }
                    }
                    break;
                case 256:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Hour = val;
                        }
                    }
                    break;
                case 257:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Minute = val;
                        }
                    }
                    break;
                case 258:
                    {
                        GetTModeCMode1(3, 18, resp.ValueString);
                    }
                    break;
                //Строка 20
                case 259:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].DayNum = val;
                        }
                    }
                    break;
                case 260:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Hour = val;
                        }
                    }
                    break;
                case 261:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Minute = val;
                        }
                    }
                    break;
                case 262:
                    {
                        GetTModeCMode1(3, 19, resp.ValueString);
                    }
                    break;
                //Строка 21
                case 263:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].DayNum = val;
                        }
                    }
                    break;
                case 264:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Hour = val;
                        }
                    }
                    break;
                case 265:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Minute = val;
                        }
                    }
                    break;
                case 266:
                    {
                        GetTModeCMode1(3, 20, resp.ValueString);
                    }
                    break;
                //Строка 22
                case 267:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].DayNum = val;
                        }
                    }
                    break;
                case 268:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Hour = val;
                        }
                    }
                    break;
                case 269:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Minute = val;
                        }
                    }
                    break;
                case 270:
                    {
                        GetTModeCMode1(3, 21, resp.ValueString);
                    }
                    break;
                //Строка 23
                case 271:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].DayNum = val;
                        }
                    }
                    break;
                case 272:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Hour = val;
                        }
                    }
                    break;
                case 273:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Minute = val;
                        }
                    }
                    break;
                case 274:
                    {
                        GetTModeCMode1(3, 22, resp.ValueString);
                    }
                    break;
                //Строка 24
                case 275:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].DayNum = val;
                        }
                    }
                    break;
                case 276:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Hour = val;
                        }
                    }
                    break;
                case 277:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Minute = val;
                        }
                    }
                    break;
                case 278:
                    {
                        GetTModeCMode1(3, 23, resp.ValueString);
                    }
                    break;
                //Строка 25
                case 279:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].DayNum = val;
                        }
                    }
                    break;
                case 280:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Hour = val;
                        }
                    }
                    break;
                case 281:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Minute = val;
                        }
                    }
                    break;
                case 282:
                    {
                        GetTModeCMode1(3, 24, resp.ValueString);
                    }
                    break;
                //Строка 26
                case 283:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].DayNum = val;
                        }
                    }
                    break;
                case 284:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Hour = val;
                        }
                    }
                    break;
                case 285:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Minute = val;
                        }
                    }
                    break;
                case 286:
                    {
                        GetTModeCMode1(3, 25, resp.ValueString);
                    }
                    break;
                //Строка 27
                case 287:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].DayNum = val;
                        }
                    }
                    break;
                case 288:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Hour = val;
                        }
                    }
                    break;
                case 289:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Minute = val;
                        }
                    }
                    break;
                case 290:
                    {
                        GetTModeCMode1(3, 26, resp.ValueString);
                    }
                    break;
                //Строка 28
                case 291:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].DayNum = val;

                        }
                    }
                    break;
                case 292:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Hour = val;

                        }
                    }
                    break;
                case 293:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Minute = val;
                            if (_activePageEntities.IsLoadingPage)
                            {
                                _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                                _modesEntities.TTitle = "Расписание";
                            }
                        }
                    }
                    break;
                case 294:
                    {
                        GetTModeCMode1(3, 27, resp.ValueString);
                    }
                    break;
                //Проверка того, что данные записаны
                case 5100:
                    {
                        //_fbs.SP1Count += 1;
                    }
                    break;
                case 5101:
                    {
                        // _fbs.SP2Count += 1;
                    }
                    break;
                case 5102:
                    {
                        // _fbs.SP3Count += 1;
                    }
                    break;
                case 5103:
                    {
                        //_fbs.SPFCount += 1;
                    }
                    break;
                case 5108:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode1ValuesByIndex(Val);
                            _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 5109:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode2ValuesByIndex(Val);
                            // _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                #region Минимальный режим
                case 5110:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].SypplySP = Val;
                        }
                    }
                    break;
                case 5111:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5112:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].TempSP = Val;
                        }
                    }
                    break;
                case 5113:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Нормальный режим
                case 5114:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].SypplySP = Val;
                        }
                    }
                    break;
                case 5115:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5116:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].TempSP = Val;
                        }
                    }
                    break;
                case 5117:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Максимальный режим
                case 5118:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].SypplySP = Val;
                        }
                    }
                    break;
                case 5119:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5120:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].TempSP = Val;
                        }
                    }
                    break;
                case 5121:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим кухни
                case 5122:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].SypplySP = Val;
                        }
                    }
                    break;
                case 5123:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5124:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].TempSP = Val;
                        }
                    }
                    break;
                case 5125:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим отпуска
                case 5126:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].SypplySP = Val;
                        }
                    }
                    break;
                case 5127:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5128:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].TempSP = Val;
                        }
                    }
                    break;
                case 5129:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Специальный режим
                case 5130:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].SypplySP = Val;
                        }
                    }
                    break;
                case 5131:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].ExhaustSP = Val;
                        }
                    }
                    break;
                case 5132:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].TempSP = Val;
                        }
                    }
                    break;
                case 5133:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                //Режим кухни.
                case 5134:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            //_modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = val;
                            //_activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 5137:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                        }
                    }
                    break;
                //Получение режима 1
                case 5138:
                    {
                        GetTModeDay(2, 0, resp.ValueString);
                    }
                    break;
                case 5139:
                    {
                        GetTModeHours(2, 0, resp.ValueString);
                    }
                    break;
                case 5140:
                    {
                        GetTModeMinutes(2, 0, resp.ValueString);
                    }
                    break;
                case 5141:
                    {
                        GetTModeCMode1(2, 0, resp.ValueString);
                    }
                    break;
                //Получение режима 2
                case 5142:
                    {
                        GetTModeDay(2, 1, resp.ValueString);
                    }
                    break;
                case 5143:
                    {
                        GetTModeHours(2, 1, resp.ValueString);
                    }
                    break;
                case 5144:
                    {
                        GetTModeMinutes(2, 1, resp.ValueString);
                    }
                    break;
                case 5145:
                    {
                        GetTModeCMode1(2, 1, resp.ValueString);
                    }
                    break;
                //Получение режима 3
                case 5146:
                    {
                        GetTModeDay(2, 2, resp.ValueString);
                    }
                    break;
                case 5147:
                    {
                        GetTModeHours(2, 2, resp.ValueString);
                    }
                    break;
                case 5148:
                    {
                        GetTModeMinutes(2, 2, resp.ValueString);
                    }
                    break;
                case 5149:
                    {
                        GetTModeCMode1(2, 2, resp.ValueString);
                    }
                    break;
                //Получение режима 4
                case 5150:
                    {
                        GetTModeDay(2, 3, resp.ValueString);
                    }
                    break;
                case 5151:
                    {
                        GetTModeHours(2, 3, resp.ValueString);
                    }
                    break;
                case 5152:
                    {
                        GetTModeMinutes(2, 3, resp.ValueString);
                    }
                    break;
                case 5153:
                    {
                        GetTModeCMode1(2, 3, resp.ValueString);
                    }
                    break;
                case 5154:
                    {
                        GetTModeCMode1(4, 0, resp.ValueString);
                    }
                    break;
                case 5155:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _fbs.CHumiditySPS.HumiditySP = val;
                        }
                    }
                    break;
                case 5156:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            bool isSpecMode = val > 0 ? true : false;
                            if (_fbs.OtherSettings.IsSpecMode != isSpecMode)
                            {
                                _fbs.OtherSettings.IsSpecMode = isSpecMode;
                            }
                        }
                    }
                    break;
                case 5157:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val == _fbs.CUpdater.PacketsCount.Value)
                            {
                                _fbs.CUpdater.IsUpdate = 1;
                                _fbs.CUpdater.CurrentPacket = 1;
                              
                            }
                        }
                    }
                    break;
                //Текущий год
                case 5158:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 99)
                            {
                                _fbs.CTime.Year = val;
                            }
                        }
                    }
                    break;
                //Текущий месяц
                case 5159:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 12)
                            {
                                _fbs.CTime.Month = val;
                            }
                        }
                    }
                    break;
                //Текущий день
                case 5160:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 31)
                            {
                                _fbs.CTime.Day = val;
                            }
                        }
                    }
                    break;
                //Текущий час
                case 5161:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 60)
                            {
                                _fbs.CTime.Hour = val;
                            }
                        }
                    }
                    break;
                //Текущая минута
                case 5162:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 60)
                            {
                                _fbs.CTime.Minute = val;
                                _fbs.CTime.SetTimerInterface();
                            }
                        }
                    }
                    break;
                //День недели
                case 5163:
                    {
                        //День недели77777
                        //if (ushort.TryParse(resp.ValueString, out ushort val))
                        //{
                        //    if (val >= 0 && val <= 6)
                        //    {
                        //        _fbs.CTime.DayOfWeek = val;
                        //    }
                        //}
                    }
                    break;
                //Данные о режиме отпуска
                case 5167:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 5168:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 5169:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 5170:
                    {
                        GetTModeCMode1(2, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 5171:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 5172:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 5173:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 5174:
                    {
                        GetTModeCMode1(2, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 5175:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 5176:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 5177:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 5178:
                    {
                        GetTModeCMode1(2, 2, resp.ValueString);

                    }
                    break;
                //Строка 4
                case 5179:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 5180:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 5181:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 5182:
                    {
                        GetTModeCMode1(2, 3, resp.ValueString);
                        if (_activePageEntities.IsLoadingPage)
                        {
                            _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                            _modesEntities.TTitle = "Расписание для отпуска";
                        }
                    }
                    break;
                //Расписание
                //Строка 1
                case 5183:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 5184:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 5185:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 5186:
                    {
                        GetTModeCMode1(3, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 5187:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 5188:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 5189:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 5190:
                    {
                        GetTModeCMode1(3, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 5191:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 5192:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 5193:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 5194:
                    {
                        GetTModeCMode1(3, 2, resp.ValueString);
                    }
                    break;
                //Строка 4
                case 5195:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 5196:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 5197:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 5198:
                    {
                        GetTModeCMode1(3, 3, resp.ValueString);
                    }
                    break;
                //Строка 5
                case 5199:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = val;
                        }
                    }
                    break;
                case 5200:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = val;
                        }
                    }
                    break;
                case 5201:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = val;
                        }
                    }
                    break;
                case 5202:
                    {
                        GetTModeCMode1(3, 4, resp.ValueString);
                    }
                    break;
                //Строка 6
                case 5203:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = val;
                        }
                    }
                    break;
                case 5204:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = val;
                        }
                    }
                    break;
                case 5205:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = val;
                        }
                    }
                    break;
                case 5206:
                    {
                        GetTModeCMode1(3, 5, resp.ValueString);
                    }
                    break;
                //Строка 7
                case 5207:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = val;
                        }
                    }
                    break;
                case 5208:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = val;
                        }
                    }
                    break;
                case 5209:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = val;
                        }
                    }
                    break;
                case 5210:
                    {
                        GetTModeCMode1(3, 6, resp.ValueString);
                    }
                    break;
                //Строка 8
                case 5211:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].DayNum = val;
                        }
                    }
                    break;
                case 5212:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Hour = val;
                        }
                    }
                    break;
                case 5213:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Minute = val;
                        }
                    }
                    break;
                case 5214:
                    {
                        GetTModeCMode1(3, 7, resp.ValueString);
                    }
                    break;
                //Строка 9
                case 5215:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].DayNum = val;
                        }
                    }
                    break;
                case 5216:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Hour = val;
                        }
                    }
                    break;
                case 5217:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Minute = val;
                        }
                    }
                    break;
                case 5218:
                    {
                        GetTModeCMode1(3, 8, resp.ValueString);
                    }
                    break;
                //Строка 10
                case 5219:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].DayNum = val;
                        }
                    }
                    break;
                case 5220:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Hour = val;
                        }
                    }
                    break;
                case 5221:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Minute = val;
                        }
                    }
                    break;
                case 5222:
                    {
                        GetTModeCMode1(3, 9, resp.ValueString);
                    }
                    break;
                //Строка 11
                case 5223:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].DayNum = val;
                        }
                    }
                    break;
                case 5224:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Hour = val;
                        }
                    }
                    break;
                case 5225:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Minute = val;
                        }
                    }
                    break;
                case 5226:
                    {
                        GetTModeCMode1(3, 10, resp.ValueString);
                    }
                    break;
                //Строка 12
                case 5227:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].DayNum = val;
                        }
                    }
                    break;
                case 5228:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Hour = val;
                        }
                    }
                    break;
                case 5229:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Minute = val;
                        }
                    }
                    break;
                case 5230:
                    {
                        GetTModeCMode1(3, 11, resp.ValueString);
                    }
                    break;
                //Строка 13
                case 5231:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].DayNum = val;
                        }
                    }
                    break;
                case 5232:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Hour = val;
                        }
                    }
                    break;
                case 5233:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Minute = val;
                        }
                    }
                    break;
                case 5234:
                    {
                        GetTModeCMode1(3, 12, resp.ValueString);
                    }
                    break;
                //Строка 14
                case 5235:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].DayNum = val;
                        }
                    }
                    break;
                case 5236:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Hour = val;
                        }
                    }
                    break;
                case 5237:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Minute = val;
                        }
                    }
                    break;
                case 5238:
                    {
                        GetTModeCMode1(3, 13, resp.ValueString);
                        if (_activePageEntities.IsLoadingPage)
                        {
                            _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                            _modesEntities.TTitle = "Расписание";
                        }
                    }
                    break;
                //Строка 15
                case 5239:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].DayNum = val;
                        }
                    }
                    break;
                case 5240:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Hour = val;
                        }
                    }
                    break;
                case 5241:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Minute = val;
                        }
                    }
                    break;
                case 5242:
                    {
                        GetTModeCMode1(3, 14, resp.ValueString);
                    }
                    break;
                //Строка 16
                case 5243:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].DayNum = val;
                        }
                    }
                    break;
                case 5244:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Hour = val;
                        }
                    }
                    break;
                case 5245:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Minute = val;
                        }
                    }
                    break;
                case 5246:
                    {
                        GetTModeCMode1(3, 15, resp.ValueString);
                    }
                    break;
                //Строка 17
                case 5247:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].DayNum = val;
                        }
                    }
                    break;
                case 5248:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Hour = val;
                        }
                    }
                    break;
                case 5249:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Minute = val;
                        }
                    }
                    break;
                case 5250:
                    {
                        GetTModeCMode1(3, 16, resp.ValueString);
                    }
                    break;
                //Строка 18
                case 5251:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].DayNum = val;
                        }
                    }
                    break;
                case 5252:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Hour = val;
                        }
                    }
                    break;
                case 5253:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Minute = val;
                        }
                    }
                    break;
                case 5254:
                    {
                        GetTModeCMode1(3, 17, resp.ValueString);
                    }
                    break;
                //Строка 19
                case 5255:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].DayNum = val;
                        }
                    }
                    break;
                case 5256:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Hour = val;
                        }
                    }
                    break;
                case 5257:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Minute = val;
                        }
                    }
                    break;
                case 5258:
                    {
                        GetTModeCMode1(3, 18, resp.ValueString);
                    }
                    break;
                //Строка 20
                case 5259:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].DayNum = val;
                        }
                    }
                    break;
                case 5260:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Hour = val;
                        }
                    }
                    break;
                case 5261:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Minute = val;
                        }
                    }
                    break;
                case 5262:
                    {
                        GetTModeCMode1(3, 19, resp.ValueString);
                    }
                    break;
                //Строка 21
                case 5263:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].DayNum = val;
                        }
                    }
                    break;
                case 5264:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Hour = val;
                        }
                    }
                    break;
                case 5265:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Minute = val;
                        }
                    }
                    break;
                case 5266:
                    {
                        GetTModeCMode1(3, 20, resp.ValueString);
                    }
                    break;
                //Строка 22
                case 5267:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].DayNum = val;
                        }
                    }
                    break;
                case 5268:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Hour = val;
                        }
                    }
                    break;
                case 5269:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Minute = val;
                        }
                    }
                    break;
                case 5270:
                    {
                        GetTModeCMode1(3, 21, resp.ValueString);
                    }
                    break;
                //Строка 23
                case 5271:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].DayNum = val;
                        }
                    }
                    break;
                case 5272:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Hour = val;
                        }
                    }
                    break;
                case 5273:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Minute = val;
                        }
                    }
                    break;
                case 5274:
                    {
                        GetTModeCMode1(3, 22, resp.ValueString);
                    }
                    break;
                //Строка 24
                case 5275:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].DayNum = val;
                        }
                    }
                    break;
                case 5276:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Hour = val;
                        }
                    }
                    break;
                case 5277:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Minute = val;
                        }
                    }
                    break;
                case 5278:
                    {
                        GetTModeCMode1(3, 23, resp.ValueString);
                    }
                    break;
                //Строка 25
                case 5279:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].DayNum = val;
                        }
                    }
                    break;
                case 5280:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Hour = val;
                        }
                    }
                    break;
                case 5281:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Minute = val;
                        }
                    }
                    break;
                case 5282:
                    {
                        GetTModeCMode1(3, 24, resp.ValueString);
                    }
                    break;
                //Строка 26
                case 5283:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].DayNum = val;
                        }
                    }
                    break;
                case 5284:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Hour = val;
                        }
                    }
                    break;
                case 5285:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Minute = val;
                        }
                    }
                    break;
                case 5286:
                    {
                        GetTModeCMode1(3, 25, resp.ValueString);
                    }
                    break;
                //Строка 27
                case 5287:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].DayNum = val;
                        }
                    }
                    break;
                case 5288:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Hour = val;
                        }
                    }
                    break;
                case 5289:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Minute = val;
                        }
                    }
                    break;
                case 5290:
                    {
                        GetTModeCMode1(3, 26, resp.ValueString);
                    }
                    break;
                //Строка 28
                case 5291:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].DayNum = val;
                            if (_activePageEntities.IsLoadingPage)
                            {
                                _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                                _modesEntities.TTitle = "Расписание";
                            }

                        }
                    }
                    break;
                case 5292:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Hour = val;

                        }
                    }
                    break;
                case 5293:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Minute = val;
                        }
                    }
                    break;
                case 5294:
                    {
                        GetTModeCMode1(3, 27, resp.ValueString);

                    }
                    break;

            }
            //Общие уставки
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -100 && val <= 1000)
                    {
                        _fbs.CCommonSetPoints.SPTempAlarm = (float)val / 10;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 1)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CEConfig.TregularCh_R = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 2)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CCommonSetPoints.SPTempMaxCh = (float)val / 10;
                    }

                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 3)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CCommonSetPoints.SPTempMinCh = (float)val / 10;
                    }
                }
                return;
            }
            //Задержка авари по темп(пока 0)
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 4)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65535)
                    {
                        _fbs.CCommonSetPoints.TControlDelayS = val;
                    }
                }
                return;
            }
            //Режим времени года
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 5)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 2)
                    {
                        _fbs.CCommonSetPoints.SeasonMode = val;
                    }
                }
                return;
            }
            //Уставка режима года
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 6)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 300)
                    {
                        _fbs.CCommonSetPoints.SPSeason = (float)val / 10; ;
                    }
                }
                return;
            }
            //Гистерезис режима года
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 7)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 200)
                    {
                        _fbs.CCommonSetPoints.HystSeason = (float)val / 10;
                    }
                }
                return;
            }
            //Авторестарт
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 8)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CEConfig.AutoRestart = val;
                    }
                }

                return;
            }
            //Автосброс пожара
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 9)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CEConfig.AutoResetFire = val;
                    }

                }
                return;
            }
            //Сила тока уф светодиодов
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset + 10)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 70)
                    {
                        _fbs.UFLeds.LEDsI = (float)val / 100;
                    }

                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 1)
                    {
                        _fbs.CEConfig.IsDemoConfig = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= _fbs.CCommonSetPoints.RoomSPPReg.Min && val <= _fbs.CCommonSetPoints.RoomSPPReg.Max)
                    {
                        _fbs.CCommonSetPoints.RoomSPPReg.Value = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= _fbs.CCommonSetPoints.RoomSPIReg.Min && val <= _fbs.CCommonSetPoints.RoomSPIReg.Max)
                    {
                        _fbs.CCommonSetPoints.RoomSPIReg.Value = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= _fbs.CCommonSetPoints.RoomSPDReg.Min && val <= _fbs.CCommonSetPoints.RoomSPDReg.Max)
                    {
                        _fbs.CCommonSetPoints.RoomSPDReg.Value = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Заслонка
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 65535)
                    {
                        _fbs.CDamperSetPoints.DamperOpenTime = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 1)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65535)
                    {
                        _fbs.CDamperSetPoints.DamperHeatingTime = val;
                    }

                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 2)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].StartPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 3)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].EndPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 4)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[1].StartPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 5)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[1].EndPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 6)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[2].StartPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 7)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[2].EndPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 8)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[3].StartPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 9)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[3].EndPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 10)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].CloseAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 11)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].OpenAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 12)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[1].CloseAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 13)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[1].OpenAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 14)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[2].CloseAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 15)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[2].OpenAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 16)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[3].CloseAngle = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 17)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 90)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[3].OpenAngle = val;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 18)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 1)
                    {
                        _fbs.CDamperSetPoints.isTest = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 19 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 19)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].CalPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 20 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 20)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[1].CalPos = val;
                    }
                }

                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 21 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 21)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[2].CalPos = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 22 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 22)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[3].CalPos = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Настройки вентиляторов
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (val <= 65535)
                    {
                        _fbs.CFans.SFanNominalFlow = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (val <= 65535)
                    {
                        _fbs.CFans.EFanNominalFlow = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CFans.LowLimitBan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CFans.HighLimitBan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CFans.PressureFailureDelay = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CFans.FanFailureDelay = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CFans.DecrFanConfig = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFans.PDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFans.IDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFans.DDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CFans.MinFanPercent = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Водяной нагреватель
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.PWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.IWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.DWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.PRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.IRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CWHSetPoints.DRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.TRetMax = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.TRetMin = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.TRetStb = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.TRetF = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.TRetStart = (float)val / 10;
                    }
                }
                return;
            }
            //2 раза, потом дополнить
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CWHSetPoints.SSMaxIntervalS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CWHSetPoints.MinDamperPerc = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.SPWinterProcess = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CWHSetPoints.IsSummerTestPump = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Электрический нагреватель.  
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {

                    if (val <= 65535)
                    {
                        _fbs.CEHSetPoints.NomPowerVT = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CEHSetPoints.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CEHSetPoints.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CEHSetPoints.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CEHSetPoints.BlowDownTime = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Фреоновый охладитель
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFreonCoolerSP.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFreonCoolerSP.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFreonCoolerSP.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CFreonCoolerSP.Stage1OnS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CFreonCoolerSP.Stage1OffS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CFreonCoolerSP.Hyst = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Увлажнитель
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CHumiditySPS.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CHumiditySPS.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CHumiditySPS.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65535)
                    {
                        _fbs.CHumiditySPS.Stage1OnS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65535)
                    {
                        _fbs.CHumiditySPS.Stage1OffS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CHumiditySPS.Hyst = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Рекуператор
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CRecup.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CRecup.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000)
                    {
                        _fbs.CRecup.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -500 && val <= 500)
                    {
                        _fbs.CRecup.TEffSP = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100)
                    {
                        _fbs.CRecup.EffFailValue = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 65000)
                    {
                        _fbs.CRecup.EffFailDelay = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100)
                    {
                        _fbs.CRecup.HZMax = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -500 && val <= 500)
                    {
                        _fbs.CRecup.TempA = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -500 && val <= 500)
                    {
                        _fbs.CRecup.TempB = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -500 && val <= 500)
                    {
                        _fbs.CRecup.TempC = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -500 && val <= 500)
                    {
                        _fbs.CRecup.TempD = (float)val / 10;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Датчики
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1000)
                    {
                        _fbs.CSensors.OutdoorTemp.Correction = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1000)
                    {
                        _fbs.CSensors.SupTemp.Correction = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1000)
                    {
                        _fbs.CSensors.ExhaustTemp.Correction = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1000)
                    {
                        _fbs.CSensors.RoomTemp.Correction = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1000)
                    {
                        _fbs.CSensors.ReturnTemp.Correction = (float)val / 10;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Конфигурация
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.ET1 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.ET2 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.OUT1 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.OUT2 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.AR1 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.AR2 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 20)
                    {
                        _fbs.CEConfig.AR3 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 3)
                    {
                        _fbs.CEConfig.Recup = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }

            //Термоанемометры
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetIntValueResult(val, _fbs.SupCalibrateThm.FanControlType);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.FanControlType);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + _menusEntities.WriteOffset + 1)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {
                    if (val >= -1000 && val <= 1200)
                    {
                        _fbs.ThmSps.TempH1 = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1200)
                    {
                        _fbs.ThmSps.TempC1 = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1200)
                    {
                        _fbs.ThmSps.TempH2 = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1200)
                    {
                        _fbs.ThmSps.TempC2 = (float)val / 10;
                    }
                    if (_menusEntities.StartMenuCollection.Count > 8 && _servActivePageEntities.LastActivePageState == SActivePageState.TmhSettingsPage)
                    {
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.ThmSps.TempH1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.ThmSps.TempC1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.ThmSps.TempH2;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.TempC2;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    GetIntValueResult(val, _fbs.ThmSps.PReg);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    GetIntValueResult(val, _fbs.ThmSps.IReg);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    GetIntValueResult(val, _fbs.ThmSps.DReg);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ThmSps.KPolKoef);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ThmSps.BPolKoef);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ThmSps.KClKoef);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ThmSps.BClKoef);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ThmSps.PThmSup);
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Работа рекуператора Modbus
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 2 && val >= 0)
                    {
                        _fbs.MbRecSPs.MBRecMode = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsRotTest1 = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsRotTest2 = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsForward1 = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsForward2 = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsGrindingMode = (byte)val;
                    }
                    if (_menusEntities.StartMenuCollection.Count > 3 && _servActivePageEntities.LastActivePageState == SActivePageState.RecupSettingsPage)
                    {
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CPickVal = _fbs.MbRecSPs.IsGrindingMode;

                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000 && val >= 0)
                    {
                        _fbs.MbRecSPs.NominalCurrent = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val >= 0 && val <= 10_000)
                    {
                        _fbs.MbRecSPs.ReductKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100 && val >= 0)
                    {
                        _fbs.MbRecSPs.NominalTurns1 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100 && val >= 0)
                    {
                        _fbs.MbRecSPs.NominalTurns2 = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val <= 700 && val >= -700)
                    {
                        _fbs.MbRecSPs.NominalTemp1 = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val <= 700 && val >= -700)
                    {
                        _fbs.MbRecSPs.NominalTemp2 = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 10_000 && val >= 0)
                    {
                        _fbs.MbRecSPs.GrindingCurrent = val;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100 && val >= 0)
                    {
                        _fbs.MbRecSPs.GrindingTurns = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].SupMinVal = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].SypplySP = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].SupMaxVal = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + _menusEntities.WriteOffset + 3)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].ExhaustMinVal = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].ExhaustSP = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].ExhaustMaxVal = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 40)
                    {
                        _modesEntities.Mode1ValuesList[6].TempSP = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (byte.TryParse(resp.ValueString, out byte val))
                {
                    if (val >= 0 && val <= 100)
                    {
                        _modesEntities.Mode1ValuesList[6].PowerLimitSP = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Калибровка термоанемометров.
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateMode);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.CalibrateMode);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaThm);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaThm);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetFloatValueResult(val, _fbs.ThmSps.PThmSupValue);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetFloatValueResult(val, _fbs.ThmSps.PThmExhaustValue);
                }
                if (_menusEntities.StartMenuCollection.Count > 13 && _servActivePageEntities.LastActivePageState == SActivePageState.ThmCalibratePage)
                {
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[1].CVal = _fbs.SupCalibrateThm.DeltaThm.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[2].CVal = _fbs.ExhaustCalibrateThm.DeltaThm.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[3].CVal = _fbs.ThmSps.PThmSupValue.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[4].CVal = _fbs.ThmSps.PThmExhaustValue.Value;
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetIntValueResult(val, _fbs.SupCalibrateThm.CavType);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.CavType);
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateTimeS);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.CalibrateTimeS);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    //val >= 0 && val <= 100
                    GetIntValueResult(val, _fbs.SupCalibrateThm.TestTimeS);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.TestTimeS);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.SupCalibrateThm.LeakFlow);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.LeakFlow);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                    {
                        _fbs.SupCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                        _fbs.ExhaustCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                    {
                        _fbs.SupCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                        _fbs.ExhaustCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                    {
                        _fbs.SupCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                        _fbs.ExhaustCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                    {
                        _fbs.SupCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                        _fbs.ExhaustCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetIntValueResult(val, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                    {
                        _fbs.SupCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                        _fbs.ExhaustCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[0] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[1] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[2] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[3] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[4] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[5] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.DeltaTCalibrates[6] = _fbs.SupCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[0] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[1] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[2] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[3] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[4] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[5] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.FlowCalibrates[6] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            //***************************************************************************************************************************
            //Калибровка вытяжек.

            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[0] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[1] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[2] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[3] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[4] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 34 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 34 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[5] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 35 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 35 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.DeltaTCalibrates[6] = _fbs.ExhaustCalibrateThm.DeltaTCalibratesLimits.Value;
                    }
                }
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 36 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 36 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[0] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 37 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 37 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[1] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 38 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 38 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[2] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 39 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 39 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[3] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 40 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 40 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[4] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 41 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 41 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[5] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 42 || resp.Tag == _menusEntities.ETH_CALIBRATE_THM_ADDR + 42 + +_menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.FlowCalibrates[6] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }

            #region Настройки термоанемометров при постоянной температуре.

            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetFloatValueResult(val, _fbs.SupCalibrateThm.CalibrateDeltaT);
                    GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.CalibrateDeltaT);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 1 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + _menusEntities.WriteOffset + 1)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetIntValueResult(val, _fbs.SupCalibrateThm.PUReg);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.PUReg);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 2 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + _menusEntities.WriteOffset + 2)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetIntValueResult(val, _fbs.SupCalibrateThm.IUReg);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.IUReg);
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 3 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + _menusEntities.WriteOffset + 3)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    GetIntValueResult(val, _fbs.SupCalibrateThm.DUReg);
                    GetIntValueResult(val, _fbs.ExhaustCalibrateThm.DUReg);
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 4 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[0] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 5 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[1] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 6 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[2] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 7 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[3] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 8 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[4] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 9 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[5] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 10 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.SupCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.SupCalibrateThm.PCalibrates[6] = _fbs.SupCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }


            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 11 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[0] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 12 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[1] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 13 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[2] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 14 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[3] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 15 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[4] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 16 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 16 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[5] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 17 || resp.Tag == _menusEntities.ETH_TCONST_THM_ADDR + 17 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.ExhaustCalibrateThm.PCalibratesLimits))
                    {
                        _fbs.ExhaustCalibrateThm.PCalibrates[6] = _fbs.ExhaustCalibrateThm.PCalibratesLimits.Value;
                    }
                }
                return;
            }

            #endregion

            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[0].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[0].I_Start = _fbs.CRecup.RecProfiles[0].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[0].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[0].I_Cont = _fbs.CRecup.RecProfiles[0].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[0].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[0].Kp = _fbs.CRecup.RecProfiles[0].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[0].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[0].Ki = _fbs.CRecup.RecProfiles[0].KiLimits.Value;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[1].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[1].I_Start = _fbs.CRecup.RecProfiles[1].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[1].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[1].I_Cont = _fbs.CRecup.RecProfiles[1].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[1].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[1].Kp = _fbs.CRecup.RecProfiles[1].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[1].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[1].Ki = _fbs.CRecup.RecProfiles[1].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[2].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[2].I_Start = _fbs.CRecup.RecProfiles[2].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[2].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[2].I_Cont = _fbs.CRecup.RecProfiles[2].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[2].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[2].Kp = _fbs.CRecup.RecProfiles[2].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[2].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[2].Ki = _fbs.CRecup.RecProfiles[2].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[3].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[3].I_Start = _fbs.CRecup.RecProfiles[3].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[3].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[3].I_Cont = _fbs.CRecup.RecProfiles[3].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[3].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[3].Kp = _fbs.CRecup.RecProfiles[3].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 15 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[3].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[3].Ki = _fbs.CRecup.RecProfiles[3].KiLimits.Value;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 16 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 16 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[4].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[4].I_Start = _fbs.CRecup.RecProfiles[4].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 17 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 17 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[4].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[4].I_Cont = _fbs.CRecup.RecProfiles[4].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 18 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 18 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[4].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[4].Kp = _fbs.CRecup.RecProfiles[4].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 19 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 19 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[4].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[4].Ki = _fbs.CRecup.RecProfiles[4].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 20 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 20 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[5].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[5].I_Start = _fbs.CRecup.RecProfiles[5].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 21 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 21 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[5].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[5].I_Cont = _fbs.CRecup.RecProfiles[5].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 22 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 22 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[5].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[5].Kp = _fbs.CRecup.RecProfiles[5].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 23 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 23 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[5].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[5].Ki = _fbs.CRecup.RecProfiles[5].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 24 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 24 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[6].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[6].I_Start = _fbs.CRecup.RecProfiles[6].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 25 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 25 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[6].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[6].I_Cont = _fbs.CRecup.RecProfiles[6].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 26 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 26 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[6].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[6].Kp = _fbs.CRecup.RecProfiles[6].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 27 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 27 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[6].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[6].Ki = _fbs.CRecup.RecProfiles[6].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 28 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 28 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[7].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[7].I_Start = _fbs.CRecup.RecProfiles[7].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 29 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 29 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[7].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[7].I_Cont = _fbs.CRecup.RecProfiles[7].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 30 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 30 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[7].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[7].Kp = _fbs.CRecup.RecProfiles[7].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 31 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 31 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[7].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[7].Ki = _fbs.CRecup.RecProfiles[7].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 32 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 32 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[8].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[8].I_Start = _fbs.CRecup.RecProfiles[8].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 33 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 33 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[8].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[8].I_Cont = _fbs.CRecup.RecProfiles[8].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 34 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 34 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[8].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[8].Kp = _fbs.CRecup.RecProfiles[8].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 35 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 35 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[8].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[8].Ki = _fbs.CRecup.RecProfiles[8].KiLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 36 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 36 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[9].I_StartLimits))
                    {
                        _fbs.CRecup.RecProfiles[9].I_Start = _fbs.CRecup.RecProfiles[9].I_StartLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 37 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 37 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[9].I_ContLimits))
                    {
                        _fbs.CRecup.RecProfiles[9].I_Cont = _fbs.CRecup.RecProfiles[9].I_ContLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 38 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 38 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[9].KpLimits))
                    {
                        _fbs.CRecup.RecProfiles[9].Kp = _fbs.CRecup.RecProfiles[9].KpLimits.Value;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 39 || resp.Tag == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 39 + _menusEntities.WriteOffset)
            {
                if (int.TryParse(resp.ValueString, out int val))
                {
                    if (GetFloatValueResult(val, _fbs.CRecup.RecProfiles[9].KiLimits))
                    {
                        _fbs.CRecup.RecProfiles[9].Ki = _fbs.CRecup.RecProfiles[9].KiLimits.Value;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }

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
                string tagsCount = String.Empty;
                if (values.Length > 0 && values.Length < 10)
                {
                    tagsCount = "00" + values.Length.ToString();
                }
                else
                if (values.Length >= 10 && values.Length < 100)
                {
                    tagsCount = "0" + values.Length.ToString();
                }
                else
                if (values.Length >= 100 && values.Length < 1000)
                {
                    tagsCount = values.Length.ToString();
                }
                MessageToServer = $"{address},{tagsCount},";
                for (int i = 0; i < values.Length; i++)
                {
                    MessageToServer += values[i];
                    if (i < values.Length - 1)
                    {
                        MessageToServer += ",";
                    }
                }
                MessageToServer += "\r\n";
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
            SetCommandToServer(156 + _menusEntities.WriteOffset, vals);
        }

        private void MFloorCallback(bool val)
        {
            int[] vals = { 0 };
            SetCommandToServer(157 + _menusEntities.WriteOffset, vals);
        }
        #endregion

        private bool GetFloatValueResult(int inputVal, FloatValue floatVal)
        {

            var min = floatVal.Min * Math.Pow(10, floatVal.NumChr);
            var max = floatVal.Max * Math.Pow(10, floatVal.NumChr);
            if (inputVal >= min && inputVal <= max)
            {
                floatVal.Value = (float)inputVal / (float)Math.Pow(10, floatVal.NumChr);
                return true;
            }
            return false;
        }

        void GetCalibrateData(int val, SPCalibrateThm spCalibrateThm, int index)
        {
            if (spCalibrateThm.CavType.Value == 0)
            {
                if (GetFloatValueResult(val, spCalibrateThm.PCalibratesLimits))
                {
                    spCalibrateThm.PCalibrates[index] = spCalibrateThm.PCalibratesLimits.Value;
                }
            }
            else
            if (spCalibrateThm.CavType.Value == 1)
            {
                if (GetFloatValueResult(val, spCalibrateThm.DeltaTCalibratesLimits))
                {
                    spCalibrateThm.DeltaTCalibrates[index] = spCalibrateThm.DeltaTCalibratesLimits.Value;
                }
            }
        }

        private bool GetIntValueResult(int inputVal, IntValue intVal)
        {
            int min = intVal.Min; //* intVal.NumChr;
            int max = intVal.Max; //* intVal.NumChr;
            if (inputVal >= min && inputVal <= max)
            {
                intVal.Value = inputVal;
                return true;
            }
            return false;
        }
    }
}
