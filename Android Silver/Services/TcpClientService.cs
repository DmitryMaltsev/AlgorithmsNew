using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;

using System.Collections;
using System.ComponentModel.Design;
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

        private ServiceActivePagesEntities _servActivePageEntities { get; set; }

        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            _activePageEntities = DIContainer.Resolve<ActivePagesEntities>();
            _menusEntities = DIContainer.Resolve<MenusEntities>();
            _fbs = DIContainer.Resolve<FBs>();
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            _servActivePageEntities = DIContainer.Resolve<ServiceActivePagesEntities>();
            _fbs.OtherSettings.MFloorAction += MFloorCallback;
            _fbs.OtherSettings.SpecModeAction += SpecModeCallback;
            //isConnected=TryConnect(tcpClient, ip, port, ref _systemMessage);
            //RecieveData(100,8);
        }

        public async Task Connect()
        {
            try
            {
                IsConnecting = true;
                _ethernetEntities.Client = new TcpClient();
                _ethernetEntities.Client.ReceiveTimeout = 600;
                _ethernetEntities.Client.SendTimeout = 600;
                _ethernetEntities.IsConnected = false;
                Task connectTask = _ethernetEntities.Client.ConnectAsync(_ethernetEntities.ConnectIP, _ethernetEntities.ConnectPort);
                if (_ethernetEntities.PagesTab == 1)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.LoadingPage);
                }
                if (await Task.WhenAny(connectTask, Task.Delay(3000)) != connectTask)
                {
                    _ethernetEntities.IsConnected = false;
                    IsConnecting = false;
                    _ethernetEntities.Client.Close();
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
                    _ethernetEntities.EthernetMessage = "Успешное подключение";
                    _ethernetEntities.SystemMessage = "Успешное подключение";
                    _ethernetEntities.IsConnected = true;
                    IsConnecting = false;
                }
            }
            catch (Exception ex)
            {
                _ethernetEntities.Client.Close();
                _ethernetEntities.Client.Dispose();
                _ethernetEntities.SystemMessage = ex.Message;
                _ethernetEntities.IsConnected = false;
                IsConnecting = false;
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
                     Task.Delay(200);
                 }
             });
        }

        private void GetMessagesState()
        {
            //Условие при котором мы определям, что мы находимся на странице календаря или переходим на нее.
            bool isReadingShedTable = (_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage)
                && _modesEntities.CTimeModeValues.Count > 0 && _modesEntities.CTimeModeValues[0].Mode2Num == 3;

            if (_activePageEntities.IsLoadingPage && _modesEntities.CTimeModeValues.Count > 0 &&
                                                           _modesEntities.CTimeModeValues[0].Mode2Num == 2)
            {
                _ethernetEntities.CMessageState = MessageStates.VacMessage;
            }
            else
                if (isReadingShedTable && _ethernetEntities.CMessageState != MessageStates.ShedMessage1)
            {
                _ethernetEntities.CMessageState = MessageStates.ShedMessage1;
            }
            else
                if (isReadingShedTable && _ethernetEntities.CMessageState == MessageStates.ShedMessage1)
            {
                _ethernetEntities.CMessageState = MessageStates.ShedMessage2;
                _activePageEntities.QueryStep = 0;
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
                            messToClient = "103,050\r\n";
                        }
                        break;
                    case MessageStates.VacMessage:
                        {
                            messToClient = "167,016\r\n";
                        }
                        break;
                    case MessageStates.ShedMessage1:
                        {
                            messToClient = "183,056\r\n";
                        }
                        break;
                    case MessageStates.ShedMessage2:
                        {
                            messToClient = "239,056\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage1:
                        {
                            //  messToClient = "299,87\r\n";
                            //Термоанемометры
                            //  messToClient = "379,27\r\n";
                            messToClient = "299,60\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage2:
                        {
                            //  messToClient = "299,87\r\n";
                            //Термоанемометры
                            //  messToClient = "379,27\r\n";
                            messToClient = "359,59\r\n";
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
                    byte[] data = new byte[300];
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
                    Task.Delay(50);
                    _ethernetEntities.SystemMessage = $"количество попыток {_trySendcounter}";
                }
                // && _ethernetEntities.MessageToServer==String.Empty
            }
            while (IsSending && _trySendcounter < 10 && MessageToServer == String.Empty);
            IsSending = false;
            if (_trySendcounter == 10)
            {
                _pictureSet.SetPicureSetIfNeed(_pictureSet.LinkHeader, _pictureSet.LinkHeader.Default);
                if (_ethernetEntities.IsConnected == true)
                {
                    _ethernetEntities.SystemMessage = "Превышено максимальное количество попыток - 10";
                    _ethernetEntities.EthernetMessage = "Превышено максимальное количество попыток передачи данных. Подключитесь повторно";
                    _activePageEntities.SetActivePageState(ActivePageState.StartPage);
                    _ethernetEntities.IsConnected = false;
                }
                else
                {
                    _ethernetEntities.IsConnected = false;
                }
            }
            return sbResult;
        }

        public void Disconnect()
        {
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
                if (int.TryParse(resultVals[1], out int valsCount))
                {
                    ushort startTag = ushort.Parse(resultVals[0]);
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
                        //_ethernetEntities.IP = resp.ValueString;
                    }
                    break;
                case 101:
                    {
                        //  _ethernetEntities.Subnet = resp.ValueString;
                    }
                    break;
                case 102:
                    {
                        // _ethernetEntities.GateWay = resp.ValueString;
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
                            _modesEntities.Mode1ValuesList[8].SypplySP = Val;
                        }
                    }
                    break;
                case 131:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].ExhaustSP = Val;
                        }
                    }
                    break;
                case 132:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].TempSP = Val;
                        }
                    }
                    break;
                case 133:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].PowerLimitSP = Val;
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
                                BitArray bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms1);
                                _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 0);
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
                                BitArray bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms2);
                                _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 1);
                            }
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
                                _fbs.CFilterVals.FilterPolPercent = val;
                                if (_fbs.CFilterVals.FilterPolPercent >= 70)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter70Header, _pictureSet.Filter70Header.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter70Header, _pictureSet.Filter70Header.Default);
                                }
                                if (_fbs.CFilterVals.FilterPolPercent == 100)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter100MainIcon, _pictureSet.Filter100MainIcon.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.Filter100MainIcon, _pictureSet.Filter100MainIcon.Default);
                                }
                            }
                        }
                    }
                    break;
                //Работа рекуператора
                case 149:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (val >= 0 && val <= 100)
                            {
                                if (val > 0)
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.RecuperatorHeader, _pictureSet.RecuperatorHeader.Selected);
                                }
                                else
                                {
                                    _pictureSet.SetPicureSetIfNeed(_pictureSet.RecuperatorHeader, _pictureSet.RecuperatorHeader.Default);
                                }
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
                        if (_activePageEntities.IsLoadingPage)
                        {
                            _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                            _modesEntities.TTitle = "Расписание";
                        }
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
                            if (_activePageEntities.IsLoadingPage)
                            {
                                _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                                _modesEntities.TTitle = "Расписание";
                            }

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
                        }
                    }
                    break;
                case 294:
                    {
                        GetTModeCMode1(3, 27, resp.ValueString);

                    }
                    break;
                //Проверка того, что данные записаны
                case 500:
                    {
                        //_fbs.SP1Count += 1;
                    }
                    break;
                case 501:
                    {
                        // _fbs.SP2Count += 1;
                    }
                    break;
                case 502:
                    {
                        // _fbs.SP3Count += 1;
                    }
                    break;
                case 503:
                    {
                        //_fbs.SPFCount += 1;
                    }
                    break;
                case 508:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode1ValuesByIndex(Val);
                            _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 509:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode2ValuesByIndex(Val);
                            // _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                #region Минимальный режим
                case 510:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].SypplySP = Val;
                        }
                    }
                    break;
                case 511:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].ExhaustSP = Val;
                        }
                    }
                    break;
                case 512:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].TempSP = Val;
                        }
                    }
                    break;
                case 513:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Нормальный режим
                case 514:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].SypplySP = Val;
                        }
                    }
                    break;
                case 515:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].ExhaustSP = Val;
                        }
                    }
                    break;
                case 516:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].TempSP = Val;
                        }
                    }
                    break;
                case 517:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Максимальный режим
                case 518:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].SypplySP = Val;
                        }
                    }
                    break;
                case 519:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].ExhaustSP = Val;
                        }
                    }
                    break;
                case 520:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].TempSP = Val;
                        }
                    }
                    break;
                case 521:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим кухни
                case 522:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].SypplySP = Val;
                        }
                    }
                    break;
                case 523:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].ExhaustSP = Val;
                        }
                    }
                    break;
                case 524:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].TempSP = Val;
                        }
                    }
                    break;
                case 525:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим отпуска
                case 526:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].SypplySP = Val;
                        }
                    }
                    break;
                case 527:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].ExhaustSP = Val;
                        }
                    }
                    break;
                case 528:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].TempSP = Val;
                        }
                    }
                    break;
                case 529:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Специальный режим
                case 530:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].SypplySP = Val;
                        }
                    }
                    break;
                case 531:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].ExhaustSP = Val;
                        }
                    }
                    break;
                case 532:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].TempSP = Val;
                        }
                    }
                    break;
                case 533:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                //Режим кухни.
                case 534:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            //_modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = val;
                            //_activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 537:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                        }
                    }
                    break;
                //Получение режима 1
                case 538:
                    {
                        GetTModeDay(2, 0, resp.ValueString);
                    }
                    break;
                case 539:
                    {
                        GetTModeHours(2, 0, resp.ValueString);
                    }
                    break;
                case 540:
                    {
                        GetTModeMinutes(2, 0, resp.ValueString);
                    }
                    break;
                case 541:
                    {
                        GetTModeCMode1(2, 0, resp.ValueString);
                    }
                    break;
                //Получение режима 2
                case 542:
                    {
                        GetTModeDay(2, 1, resp.ValueString);
                    }
                    break;
                case 543:
                    {
                        GetTModeHours(2, 1, resp.ValueString);
                    }
                    break;
                case 544:
                    {
                        GetTModeMinutes(2, 1, resp.ValueString);
                    }
                    break;
                case 545:
                    {
                        GetTModeCMode1(2, 1, resp.ValueString);
                    }
                    break;
                //Получение режима 3
                case 546:
                    {
                        GetTModeDay(2, 2, resp.ValueString);
                    }
                    break;
                case 547:
                    {
                        GetTModeHours(2, 2, resp.ValueString);
                    }
                    break;
                case 548:
                    {
                        GetTModeMinutes(2, 2, resp.ValueString);
                    }
                    break;
                case 549:
                    {
                        GetTModeCMode1(2, 2, resp.ValueString);
                    }
                    break;
                //Получение режима 4
                case 550:
                    {
                        GetTModeDay(2, 3, resp.ValueString);
                    }
                    break;
                case 551:
                    {
                        GetTModeHours(2, 3, resp.ValueString);
                    }
                    break;
                case 552:
                    {
                        GetTModeMinutes(2, 3, resp.ValueString);
                    }
                    break;
                case 553:
                    {
                        GetTModeCMode1(2, 3, resp.ValueString);
                    }
                    break;
                case 554:
                    {
                        GetTModeCMode1(4, 0, resp.ValueString);
                    }
                    break;
                case 555:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _fbs.CHumiditySPS.HumiditySP = val;
                        }
                    }
                    break;
                case 556:
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
                case 557:
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
                case 558:
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
                case 559:
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
                case 560:
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
                case 561:
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
                case 562:
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
                case 563:
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
                case 567:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 568:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 569:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 570:
                    {
                        GetTModeCMode1(2, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 571:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 572:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 573:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 574:
                    {
                        GetTModeCMode1(2, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 575:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 576:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 577:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 578:
                    {
                        GetTModeCMode1(2, 2, resp.ValueString);

                    }
                    break;
                //Строка 4
                case 579:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 580:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 581:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 582:
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
                case 583:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = val;
                        }
                    }
                    break;
                case 584:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 585:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 586:
                    {
                        GetTModeCMode1(3, 0, resp.ValueString);
                    }
                    break;
                //Строка 2
                case 587:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 588:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 589:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 590:
                    {
                        GetTModeCMode1(3, 1, resp.ValueString);
                    }
                    break;
                //Строка 3
                case 591:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 592:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 593:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 594:
                    {
                        GetTModeCMode1(3, 2, resp.ValueString);
                    }
                    break;
                //Строка 4
                case 595:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 596:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 597:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 598:
                    {
                        GetTModeCMode1(3, 3, resp.ValueString);
                    }
                    break;
                //Строка 5
                case 599:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = val;
                        }
                    }
                    break;
                case 600:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = val;
                        }
                    }
                    break;
                case 601:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = val;
                        }
                    }
                    break;
                case 602:
                    {
                        GetTModeCMode1(3, 4, resp.ValueString);
                    }
                    break;
                //Строка 6
                case 603:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = val;
                        }
                    }
                    break;
                case 604:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = val;
                        }
                    }
                    break;
                case 605:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = val;
                        }
                    }
                    break;
                case 606:
                    {
                        GetTModeCMode1(3, 5, resp.ValueString);
                    }
                    break;
                //Строка 7
                case 607:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = val;
                        }
                    }
                    break;
                case 608:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = val;
                        }
                    }
                    break;
                case 609:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = val;
                        }
                    }
                    break;
                case 610:
                    {
                        GetTModeCMode1(3, 6, resp.ValueString);
                    }
                    break;
                //Строка 8
                case 611:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].DayNum = val;
                        }
                    }
                    break;
                case 612:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Hour = val;
                        }
                    }
                    break;
                case 613:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Minute = val;
                        }
                    }
                    break;
                case 614:
                    {
                        GetTModeCMode1(3, 7, resp.ValueString);
                    }
                    break;
                //Строка 9
                case 615:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].DayNum = val;
                        }
                    }
                    break;
                case 616:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Hour = val;
                        }
                    }
                    break;
                case 617:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Minute = val;
                        }
                    }
                    break;
                case 618:
                    {
                        GetTModeCMode1(3, 8, resp.ValueString);
                    }
                    break;
                //Строка 10
                case 619:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].DayNum = val;
                        }
                    }
                    break;
                case 620:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Hour = val;
                        }
                    }
                    break;
                case 621:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Minute = val;
                        }
                    }
                    break;
                case 622:
                    {
                        GetTModeCMode1(3, 9, resp.ValueString);
                    }
                    break;
                //Строка 11
                case 623:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].DayNum = val;
                        }
                    }
                    break;
                case 624:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Hour = val;
                        }
                    }
                    break;
                case 625:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Minute = val;
                        }
                    }
                    break;
                case 626:
                    {
                        GetTModeCMode1(3, 10, resp.ValueString);
                    }
                    break;
                //Строка 12
                case 627:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].DayNum = val;
                        }
                    }
                    break;
                case 628:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Hour = val;
                        }
                    }
                    break;
                case 629:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Minute = val;
                        }
                    }
                    break;
                case 630:
                    {
                        GetTModeCMode1(3, 11, resp.ValueString);
                    }
                    break;
                //Строка 13
                case 631:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].DayNum = val;
                        }
                    }
                    break;
                case 632:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Hour = val;
                        }
                    }
                    break;
                case 633:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Minute = val;
                        }
                    }
                    break;
                case 634:
                    {
                        GetTModeCMode1(3, 12, resp.ValueString);
                    }
                    break;
                //Строка 14
                case 635:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].DayNum = val;
                        }
                    }
                    break;
                case 636:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Hour = val;
                        }
                    }
                    break;
                case 637:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Minute = val;
                        }
                    }
                    break;
                case 638:
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
                case 639:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].DayNum = val;
                        }
                    }
                    break;
                case 640:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Hour = val;
                        }
                    }
                    break;
                case 641:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[14].Minute = val;
                        }
                    }
                    break;
                case 642:
                    {
                        GetTModeCMode1(3, 14, resp.ValueString);
                    }
                    break;
                //Строка 16
                case 643:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].DayNum = val;
                        }
                    }
                    break;
                case 644:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Hour = val;
                        }
                    }
                    break;
                case 645:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[15].Minute = val;
                        }
                    }
                    break;
                case 646:
                    {
                        GetTModeCMode1(3, 15, resp.ValueString);
                    }
                    break;
                //Строка 17
                case 647:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].DayNum = val;
                        }
                    }
                    break;
                case 648:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Hour = val;
                        }
                    }
                    break;
                case 649:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[16].Minute = val;
                        }
                    }
                    break;
                case 650:
                    {
                        GetTModeCMode1(3, 16, resp.ValueString);
                    }
                    break;
                //Строка 18
                case 651:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].DayNum = val;
                        }
                    }
                    break;
                case 652:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Hour = val;
                        }
                    }
                    break;
                case 653:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[17].Minute = val;
                        }
                    }
                    break;
                case 654:
                    {
                        GetTModeCMode1(3, 17, resp.ValueString);
                    }
                    break;
                //Строка 19
                case 655:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].DayNum = val;
                        }
                    }
                    break;
                case 656:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Hour = val;
                        }
                    }
                    break;
                case 657:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[18].Minute = val;
                        }
                    }
                    break;
                case 658:
                    {
                        GetTModeCMode1(3, 18, resp.ValueString);
                    }
                    break;
                //Строка 20
                case 659:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].DayNum = val;
                        }
                    }
                    break;
                case 660:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Hour = val;
                        }
                    }
                    break;
                case 661:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[19].Minute = val;
                        }
                    }
                    break;
                case 662:
                    {
                        GetTModeCMode1(3, 19, resp.ValueString);
                    }
                    break;
                //Строка 21
                case 663:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].DayNum = val;
                        }
                    }
                    break;
                case 664:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Hour = val;
                        }
                    }
                    break;
                case 665:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[20].Minute = val;
                        }
                    }
                    break;
                case 666:
                    {
                        GetTModeCMode1(3, 20, resp.ValueString);
                    }
                    break;
                //Строка 22
                case 667:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].DayNum = val;
                        }
                    }
                    break;
                case 668:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Hour = val;
                        }
                    }
                    break;
                case 669:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[21].Minute = val;
                        }
                    }
                    break;
                case 670:
                    {
                        GetTModeCMode1(3, 21, resp.ValueString);
                    }
                    break;
                //Строка 23
                case 671:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].DayNum = val;
                        }
                    }
                    break;
                case 672:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Hour = val;
                        }
                    }
                    break;
                case 673:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[22].Minute = val;
                        }
                    }
                    break;
                case 674:
                    {
                        GetTModeCMode1(3, 22, resp.ValueString);
                    }
                    break;
                //Строка 24
                case 675:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].DayNum = val;
                        }
                    }
                    break;
                case 676:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Hour = val;
                        }
                    }
                    break;
                case 677:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[23].Minute = val;
                        }
                    }
                    break;
                case 678:
                    {
                        GetTModeCMode1(3, 23, resp.ValueString);
                    }
                    break;
                //Строка 25
                case 679:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].DayNum = val;
                        }
                    }
                    break;
                case 680:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Hour = val;
                        }
                    }
                    break;
                case 681:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[24].Minute = val;
                        }
                    }
                    break;
                case 682:
                    {
                        GetTModeCMode1(3, 24, resp.ValueString);
                    }
                    break;
                //Строка 26
                case 683:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].DayNum = val;
                        }
                    }
                    break;
                case 684:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Hour = val;
                        }
                    }
                    break;
                case 685:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[25].Minute = val;
                        }
                    }
                    break;
                case 686:
                    {
                        GetTModeCMode1(3, 25, resp.ValueString);
                    }
                    break;
                //Строка 27
                case 687:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].DayNum = val;
                        }
                    }
                    break;
                case 688:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Hour = val;
                        }
                    }
                    break;
                case 689:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[26].Minute = val;
                        }
                    }
                    break;
                case 690:
                    {
                        GetTModeCMode1(3, 26, resp.ValueString);
                    }
                    break;
                //Строка 28
                case 691:
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
                case 692:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Hour = val;

                        }
                    }
                    break;
                case 693:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[3].TimeModeValues[27].Minute = val;
                        }
                    }
                    break;
                case 694:
                    {
                        GetTModeCMode1(3, 27, resp.ValueString);

                    }
                    break;

            }
            //Общие уставки
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CCommonSetPoints.SPTempAlarm = (float)val / 10;
                    }
                }
                return;
            }

            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 1)
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
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 2)
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

            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 3)
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
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 4)
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
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 5)
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
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 6)
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
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 7)
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
            //Автосброс пожара
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 8)
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
            //Авторестарт
            if (resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 400 + 9)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1)
                    {
                        _fbs.CEConfig.AutoRestart = val;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Заслонка
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 1)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 2)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 3)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 4)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 5)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 6)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 7)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 8)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 9)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 10)
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
            if (resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 400 + 11)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CDamperSetPoints.ServoDampers[0].CalPos = val;
                        _fbs.CDamperSetPoints.ServoDampers[1].CalPos = val;
                        _fbs.CDamperSetPoints.ServoDampers[2].CalPos = val;
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CFans.Speed0v = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 100)
                    {
                        _fbs.CFans.Speed10v = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFans.PDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFans.IDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFans.DDecrFan = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.PWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.IWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.DWork = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.PRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.IRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CWHSetPoints.DRet = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 + 400)
            {
                return;
            }
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 + 400)
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
            if (resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 || resp.Tag == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 + 400)
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
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CEHSetPoints.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CEHSetPoints.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CEHSetPoints.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFreonCoolerSP.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFreonCoolerSP.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CFreonCoolerSP.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
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
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CHumiditySPS.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CHumiditySPS.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CHumiditySPS.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CHumiditySPS.Stage1OnS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CHumiditySPS.Stage1OffS = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
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
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 100)
                    {
                        _fbs.CRecup.EffSP = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 + 400)
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
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.TempA = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.TempB = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
                    {
                        _fbs.CRecup.TempC = (float)val / 10;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1000)
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
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 + 400)
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
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 + 400)
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
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 + 400)
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
            if (resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 2)
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
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val < 65535)
                    {
                        _fbs.ThmSps.SupTHmKoef = (float)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val < 65535)
                    {
                        _fbs.ThmSps.SupCurveKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val < 65535)
                    {
                        _fbs.ThmSps.ExhaustTHmKoef = (float)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val < 65535)
                    {
                        _fbs.ThmSps.ExhaustCurveKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 + 400)
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
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -1000 && val <= 1200)
                    {
                        _fbs.ThmSps.TempC2 = (float)val / 10;
                    }
                    if (_menusEntities.StartMenuCollection.Count > 8 && _servActivePageEntities.LastActivePageState == SActivePageState.TmhSettingsPage)
                    {
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.TempH1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[5].CVal = _fbs.ThmSps.TempC1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[6].CVal = _fbs.ThmSps.TempH2;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[7].CVal = _fbs.ThmSps.TempC2;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val <= 1000)
                    {
                        _fbs.ThmSps.PReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val <= 1000)
                    {
                        _fbs.ThmSps.IReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val >= 0 && val <= 1000)
                    {
                        _fbs.ThmSps.DReg = val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -9999 && val <= 9999)
                    {
                        _fbs.ThmSps.KPolKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -9999 && val <= 9999)
                    {
                        _fbs.ThmSps.BPolKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 13 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 13 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -9999 && val <= 9999)
                    {
                        _fbs.ThmSps.KClKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 14 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 14 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= -9999 && val <= 9999)
                    {
                        _fbs.ThmSps.BClKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 15 || resp.Tag == _menusEntities.ETH_THM_SETTINGS_ADDR + 15 + 400)
            {
                if (short.TryParse(resp.ValueString, out short val))
                {

                    if (val >= 0 && val <= 100)
                    {
                        _fbs.ThmSps.U = (float)val / 10;
                    }
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return;
            }
            //Работа рекуператора Modbus
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsRotTest = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {

                    if (val <= 1 && val >= 0)
                    {
                        _fbs.MbRecSPs.IsForward = (byte)val;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 + 400)
            {
                if (ushort.TryParse(resp.ValueString, out ushort val))
                {
                    if (val <= 1000 && val >= 0)
                    {
                        _fbs.MbRecSPs.ReductKoef = (float)val / 100;
                    }
                }
                return;
            }
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 + 400)
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
            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 + 400)
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

            if (resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 || resp.Tag == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 + 400)
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

        public void SendData(string val)
        {
            Task.Run(() =>
            {
                string messToClient = val;
                if (!String.IsNullOrEmpty(MessageToServer))
                {
                    messToClient = MessageToServer;
                }
                if (!IsSending)
                {
                    SendCommand(messToClient);
                    if (sbResult != null && sbResult.Length > 0)
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
                //   Task.Delay(10);

            });
        }

        /// <summary>
        /// Основной метод передачи данных на сервер
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        public void SetCommandToServer(int address, int[] values)
        {
            string bufLength = String.Empty;
            if (values.Length > 0 && values.Length < 10)
            {
                bufLength = "00" + values.Length.ToString();
            }
            else
            if (values.Length >= 10 && values.Length < 100)
            {
                bufLength = "0" + values.Length.ToString();
            }
            else
            if (values.Length >= 100 && values.Length < 1000)
            {
                bufLength = values.Length.ToString();
            }
            MessageToServer = $"{address},{bufLength},";
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
            SetCommandToServer(556, vals);
        }

        private void MFloorCallback(bool val)
        {
            int mfActive = val ? 1 : 0;
            int[] vals = { mfActive };
            SetCommandToServer(557, vals);
        }
        #endregion
    }
}
