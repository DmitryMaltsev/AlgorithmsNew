using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;

using System.Collections;
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

        private ServiceActivePagesEntities _servActivePageEntities { get; set; }

        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            _activePageEntities = DIContainer.Resolve<ActivePagesEntities>();
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
                _ethernetEntities.Client.ReceiveTimeout = 300;
                _ethernetEntities.Client.SendTimeout = 300;
                _ethernetEntities.IsConnected = false;
                Task connectTask = _ethernetEntities.Client.ConnectAsync(_ethernetEntities.ConnectIP, _ethernetEntities.ConnectPort);
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
                     //string messToClient = val;
                     //if (!String.IsNullOrEmpty(MessageToServer))
                     //{
                     //    messToClient = MessageToServer;
                     //}
                     //else
                     ////????????????
                     //if (_activePageEntities.IsLoadingPage && _modesEntities.CTimeModeValues.Count > 0 &&
                     //                                               _modesEntities.CTimeModeValues[0].Mode2Num == 2)
                     //{
                     //    messToClient = "167,16\r\n";
                     //}
                     //else
                     //    if ((_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage) && _modesEntities.CTimeModeValues.Count > 0 &&
                     //                                               _modesEntities.CTimeModeValues[0].Mode2Num == 3 && _activePageEntities.QueryStep == 0)
                     //{
                     //    messToClient = "183,56\r\n";
                     //    _activePageEntities.QueryStep = 1;
                     //}
                     //else
                     //    if ((_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage) && _modesEntities.CTimeModeValues.Count > 0 &&
                     //                                               _modesEntities.CTimeModeValues[0].Mode2Num == 3 && _activePageEntities.QueryStep == 1)
                     //{
                     //    messToClient = "239,56\r\n";
                     //    _activePageEntities.QueryStep = 0;
                     //}
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
                     Task.Delay(50);
                 }
             });
        }

        private void GetMessagesState()
        {

            if (_activePageEntities.IsLoadingPage && _modesEntities.CTimeModeValues.Count > 0 &&
                                                           _modesEntities.CTimeModeValues[0].Mode2Num == 2)
            {
                _ethernetEntities.CMessageState = MessageStates.VacMessage;
            }
            else
                if ((_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage) && _modesEntities.CTimeModeValues.Count > 0 &&
                                                           _modesEntities.CTimeModeValues[0].Mode2Num == 3 && _activePageEntities.QueryStep == 0)
            {
                _ethernetEntities.CMessageState = MessageStates.ShedMessage1;
                _activePageEntities.QueryStep = 1;
            }
            else
                if ((_activePageEntities.IsLoadingPage || _activePageEntities.IsTSettingsPage) && _modesEntities.CTimeModeValues.Count > 0 &&
                                                           _modesEntities.CTimeModeValues[0].Mode2Num == 3 && _activePageEntities.QueryStep == 1)
            {
                _ethernetEntities.CMessageState = MessageStates.ShedMessage1;
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
                _ethernetEntities.CMessageState = MessageStates.ServiceMessage;
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
                            messToClient = "103,50\r\n";
                        }
                        break;
                    case MessageStates.VacMessage:
                        {
                            messToClient = "167,16\r\n";
                        }
                        break;
                    case MessageStates.ShedMessage1:
                        {
                            messToClient = "183,56\r\n";
                        }
                        break;
                    case MessageStates.ShedMessage2:
                        {
                            messToClient = "239,56\r\n";
                        }
                        break;
                    case MessageStates.ServiceMessage:
                        {
                            messToClient = "299,87\r\n";
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
                    byte[] data = new byte[350];
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
                //Общие уставки
                case 299:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempAlarm = val;
                            }
                        }
                    }
                    break;
                case 300:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.TregularCh_R = val;
                            }
                        }
                    }
                    break;
                case 301:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempMaxCh = val;
                            }

                        }
                    }
                    break;
                case 302:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempMinCh = val;
                            }

                        }
                    }
                    break;
                //Задержка авари по темп(пока 0)
                case 303:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 60000)
                            {
                                _fbs.CCommonSetPoints.TControlDelayS = val;
                            }
                        }
                    }
                    break;
                //Режим времени года
                case 304:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 2)
                            {
                                _fbs.CCommonSetPoints.SeasonMode = val;
                            }
                        }
                    }
                    break;
                //Уставка режима года
                case 305:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CCommonSetPoints.SPSeason = val;
                            }
                        }
                    }
                    break;
                //Гистерезис режима года
                case 306:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.HystSeason = val;
                            }
                        }
                    }
                    break;
                //Автосброс пожара
                case 307:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.AutoResetFire = val;
                            }

                        }
                    }
                    break;
                //Авторестарт
                case 308:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.AutoRestart = val;
                            }
                        }
                    }
                    break;
                //Заслонка
                case 309:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CDamperSetPoints.DamperOpenTime = val;
                            }

                        }
                    }
                    break;
                case 310:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CDamperSetPoints.DamperHeatingTime = val;
                            }

                        }
                    }
                    break;
                //Настройки вентилятора
                case 311:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100000)
                            {
                                _fbs.CFans.SFanNominalFlow = val;
                            }
                        }
                    }
                    break;
                case 312:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100000)
                            {
                                _fbs.CFans.EFanNominalFlow = val;
                            }
                        }
                    }
                    break;
                case 313:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.Speed0v = val;
                            }
                        }
                    }
                    break;
                case 314:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.Speed10v = val;
                            }
                        }
                    }
                    break;
                case 315:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFans.PressureFailureDelay = val;
                            }
                        }
                    }
                    break;
                case 316:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFans.FanFailureDelay = val;
                            }
                        }
                    }
                    break;
                case 317:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CFans.DecrFanConfig = val;
                            }
                        }
                    }
                    break;
                case 318:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.PDecrFan = val;
                            }
                        }
                    }
                    break;
                case 319:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.IDecrFan = val;
                            }
                        }
                    }
                    break;
                case 320:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.DDecrFan = val;
                            }
                        }
                    }
                    break;
                case 321:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.MinFanPercent = val;
                            }
                        }
                    }
                    break;
                //Водяной нагреватель
                case 322:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.PWork = val;
                            }
                        }
                    }
                    break;
                case 323:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.IWork = val;
                            }
                        }
                    }
                    break;
                case 324:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.DWork = val;
                            }
                        }
                    }
                    break;
                case 325:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.PRet = val;
                            }
                        }
                    }
                    break;
                case 326:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.IRet = val;
                            }
                        }
                    }
                    break;
                case 327:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.DRet = val;
                            }
                        }
                    }
                    break;
                case 328:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetMax = val;
                            }
                        }
                    }
                    break;
                case 329:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetMin = val;
                            }
                        }
                    }
                    break;
                case 330:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetStb = val;
                            }
                        }
                    }
                    break;
                case 331:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetF = val;
                            }
                        }
                    }
                    break;
                case 332:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetStart = val;
                            }
                        }
                    }
                    break;
                //2 раза, потом дополнить
                case 333:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                // _fbs.CWHSetPoints.TRetStart = val;
                            }
                        }
                    }
                    break;
                case 334:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CWHSetPoints.SSMaxIntervalS = val;
                            }
                        }
                    }
                    break;
                case 335:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.MinDamperPerc = val;
                            }
                        }
                    }
                    break;
                case 336:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.SPWinterProcess = val;
                            }
                        }
                    }
                    break;
                case 337:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CWHSetPoints.IsSummerTestPump = val;
                            }
                        }
                    }
                    break;
                //Электрический нагреватель.   
                case 338:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100_000)
                            {
                                _fbs.CEHSetPoints.NomPowerVT = val;
                            }
                        }
                    }
                    break;
                case 339:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.PReg = val;
                            }
                        }
                    }
                    break;
                case 340:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.IReg = val;
                            }
                        }
                    }
                    break;
                case 341:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.DReg = val;
                            }
                        }
                    }
                    break;
                case 342:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CEHSetPoints.BlowDownTime = val;
                            }
                        }
                    }
                    break;
                //Фреоновый охладитель
                case 343:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.PReg = val;
                            }
                        }
                    }
                    break;
                case 344:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.IReg = val;
                            }
                        }
                    }
                    break;
                case 345:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.DReg = val;
                            }
                        }
                    }
                    break;
                case 346:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFreonCoolerSP.Stage1OffS = val;
                            }
                        }
                    }
                    break;
                case 347:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFreonCoolerSP.Stage1OnS = val;
                            }
                        }
                    }
                    break;
                case 348:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.Hyst = val;
                            }
                        }
                    }
                    break;
                //Увлажнитель
                case 349:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.PReg = val;
                            }
                        }
                    }
                    break;
                case 350:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.IReg = val;
                            }
                        }
                    }
                    break;
                case 351:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.DReg = val;
                            }
                        }
                    }
                    break;
                case 352:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CHumiditySPS.Stage1OffS = val;
                            }
                        }
                    }
                    break;
                case 353:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CHumiditySPS.Stage1OnS = val;
                            }
                        }
                    }
                    break;
                case 354:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.Hyst = val;
                            }
                        }
                    }
                    break;
                case 355:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.PReg = val;
                            }
                        }
                    }
                    break;
                case 356:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.IReg = val;
                            }
                        }
                    }
                    break;
                case 357:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.DReg = val;
                            }
                        }
                    }
                    break;
                case 358:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.EffSP = val;
                            }
                        }
                    }
                    break;
                case 359:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.EffFailValue = val;
                            }
                        }
                    }
                    break;
                case 360:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CRecup.EffFailDelay = val;
                            }
                        }
                    }
                    break;
                case 361:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.HZMax = val;
                            }
                        }
                    }
                    break;
                case 362:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempA = val;
                            }
                        }
                    }
                    break;
                case 363:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempB = val;
                            }
                        }
                    }
                    break;
                case 364:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempC = val;
                            }
                        }
                    }
                    break;
                case 365:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempD = val;
                            }
                        }
                    }
                    break;
                //Датчики
                case 366:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.OutdoorTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 367:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.SupTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 368:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.ExhaustTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 369:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.RoomTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 370:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.ReturnTemp.Correction = val;
                            }
                        }

                    }
                    break;
                //Конфигурация
                case 371:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.ET1= val;
                            }
                        }
                    }
                    break;
                case 372:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.ET2 = val;
                            }
                        }
                    }
                    break;
                case 373:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.OUT1 = val;
                            }
                        }
                    }
                    break;
                case 374:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.OUT2 = val;
                            }
                        }
                    }
                    break;
                case 375:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.AR1 = val;
                            }
                        }
                    }
                    break;
                case 376:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.AR2 = val;
                            }
                        }
                    }
                    break;
                case 377:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 20)
                            {
                                _fbs.CEConfig.AR3 = val;
                            }
                        }
                    }
                    break;
                case 378:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= 0 && val <= 1)
                            {
                                _fbs.CEConfig.Recup = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 379:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 65535)
                            {
                                _fbs.ThmSps.SupTHmKoef = val;
                            }
                        }
                    }
                    break;
                case 380:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 65535)
                            {
                                _fbs.ThmSps.SupCurveKoef = val;
                            }
                        }
                    }
                    break;
                case 381:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 65535)
                            {
                                _fbs.ThmSps.ExhaustTHmKoef = val;
                            }
                        }
                    }
                    break;
                case 382:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 65535)
                            {
                                _fbs.ThmSps.ExhaustCurveKoef = val;
                            }
                        }
                    }
                    break;
                case 383:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 1000)
                            {
                                _fbs.ThmSps.PReg = val;
                            }
                        }
                    }
                    break;
                case 384:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 1000)
                            {
                                _fbs.ThmSps.IReg = val;
                            }
                        }
                    }
                    break;
                case 385:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val >= 0 && val < 1000)
                            {
                                _fbs.ThmSps.DReg = val;
                            }
                        }
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
                //Общие уставки
                case 699:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempAlarm = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 700:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.TregularCh_R = val;
                            }
                        }
                    }
                    break;
                case 701:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempMaxCh = val;
                            }

                        }
                    }
                    break;
                case 702:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.SPTempMinCh = val;
                            }

                        }
                    }
                    break;
                //Задержка авари по темп(пока 0)
                case 703:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 60000)
                            {
                                _fbs.CCommonSetPoints.TControlDelayS = val;
                            }
                        }
                    }
                    break;
                //Режим времени года
                case 704:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 2)
                            {
                                _fbs.CCommonSetPoints.SeasonMode = val;
                            }
                        }
                    }
                    break;
                //Уставка режима года
                case 705:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CCommonSetPoints.SPSeason = val;
                            }
                        }
                    }
                    break;
                //Гистерезис режима года
                case 706:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CCommonSetPoints.HystSeason = val;
                            }
                        }
                    }
                    break;
                //Автосброс пожара
                case 707:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.AutoResetFire = val;
                            }

                        }
                    }
                    break;
                //Авторестарт
                case 708:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.AutoRestart = val;
                            }
                        }
                    }
                    break;
                //Заслонка
                case 709:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CDamperSetPoints.DamperOpenTime = val;
                            }

                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 710:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CDamperSetPoints.DamperHeatingTime = val;
                            }

                        }
                    }
                    break;
                //Настройки вентилятора
                case 711:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100000)
                            {
                                _fbs.CFans.SFanNominalFlow = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 712:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100000)
                            {
                                _fbs.CFans.EFanNominalFlow = val;
                            }
                        }
                    }
                    break;
                case 713:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.Speed0v = val;
                            }
                        }
                    }
                    break;
                case 714:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.Speed10v = val;
                            }
                        }
                    }
                    break;
                case 715:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFans.PressureFailureDelay = val;
                            }
                        }
                    }
                    break;
                case 716:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFans.FanFailureDelay = val;
                            }
                        }
                    }
                    break;
                case 717:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CFans.DecrFanConfig = val;
                            }
                        }
                    }
                    break;
                case 718:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.PDecrFan = val;
                            }
                        }
                    }
                    break;
                case 719:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.IDecrFan = val;
                            }
                        }
                    }
                    break;
                case 720:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFans.DDecrFan = val;
                            }
                        }
                    }
                    break;
                case 721:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CFans.MinFanPercent = val;
                            }
                        }
                    }
                    break;
                //Водяной нагреватель
                case 722:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.PWork = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 723:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.IWork = val;
                            }
                        }
                    }
                    break;
                case 724:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.DWork = val;
                            }
                        }
                    }
                    break;
                case 725:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.PRet = val;
                            }
                        }
                    }
                    break;
                case 726:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.IRet = val;
                            }
                        }
                    }
                    break;
                case 727:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CWHSetPoints.DRet = val;
                            }
                        }
                    }
                    break;
                case 728:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetMax = val;
                            }
                        }
                    }
                    break;
                case 729:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetMin = val;
                            }
                        }
                    }
                    break;
                case 730:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetStb = val;
                            }
                        }
                    }
                    break;
                case 731:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetF = val;
                            }
                        }
                    }
                    break;
                case 732:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.TRetStart = val;
                            }
                        }
                    }
                    break;
                //2 раза, потом дополнить
                case 733:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                // _fbs.CWHSetPoints.TRetStart = val;
                            }
                        }
                    }
                    break;
                case 734:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CWHSetPoints.SSMaxIntervalS = val;
                            }
                        }
                    }
                    break;
                case 735:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.MinDamperPerc = val;
                            }
                        }
                    }
                    break;
                case 736:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CWHSetPoints.SPWinterProcess = val;
                            }
                        }
                    }
                    break;
                case 737:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CWHSetPoints.IsSummerTestPump = val;
                            }
                        }
                    }
                    break;
                //Электрический нагреватель.   
                case 738:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

                            if (val <= 100_000)
                            {
                                _fbs.CEHSetPoints.NomPowerVT = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 739:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.PReg = val;
                            }
                        }
                    }
                    break;
                case 740:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.IReg = val;
                            }
                        }
                    }
                    break;
                case 741:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CEHSetPoints.DReg = val;
                            }
                        }
                    }
                    break;
                case 742:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CEHSetPoints.BlowDownTime = val;
                            }
                        }
                    }
                    break;
                //Фреоновый охладитель
                case 743:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.PReg = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 744:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.IReg = val;
                            }
                        }
                    }
                    break;
                case 745:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.DReg = val;
                            }
                        }
                    }
                    break;
                case 746:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFreonCoolerSP.Stage1OffS = val;
                            }
                        }
                    }
                    break;
                case 747:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CFreonCoolerSP.Stage1OnS = val;
                            }
                        }
                    }
                    break;
                case 748:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CFreonCoolerSP.Hyst = val;
                            }
                        }
                    }
                    break;
                //Увлажнитель
                case 749:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.PReg = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 750:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.IReg = val;
                            }
                        }
                    }
                    break;
                case 751:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.DReg = val;
                            }
                        }
                    }
                    break;
                case 752:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CHumiditySPS.Stage1OffS = val;
                            }
                        }
                    }
                    break;
                case 753:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CHumiditySPS.Stage1OnS = val;
                            }
                        }
                    }
                    break;
                case 754:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CHumiditySPS.Hyst = val;
                            }
                        }
                    }
                    break;
                case 755:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.PReg = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                case 756:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.IReg = val;
                            }
                        }
                    }
                    break;
                case 757:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1000)
                            {
                                _fbs.CRecup.DReg = val;
                            }
                        }
                    }
                    break;
                case 758:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.EffSP = val;
                            }
                        }
                    }
                    break;
                case 759:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.EffFailValue = val;
                            }
                        }
                    }
                    break;
                case 760:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 65000)
                            {
                                _fbs.CRecup.EffFailDelay = val;
                            }
                        }
                    }
                    break;
                case 761:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.HZMax = val;
                            }
                        }
                    }
                    break;
                case 762:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempA = val;
                            }
                        }
                    }
                    break;
                case 763:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempB = val;
                            }
                        }
                    }
                    break;
                case 764:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempC = val;
                            }
                        }
                    }
                    break;
                case 765:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 100)
                            {
                                _fbs.CRecup.TempD = val;
                            }
                        }
                    }
                    break;
                //Коррекция датчиков
                case 766:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.OutdoorTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 767:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.SupTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 768:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.ExhaustTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 769:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.RoomTemp.Correction = val;
                            }
                        }
                    }
                    break;
                case 770:
                    {
                        if (short.TryParse(resp.ValueString, out short val))
                        {

                            if (val >= -100 && val <= 100)
                            {
                                _fbs.CSensors.ReturnTemp.Correction = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
                //Конфигурация датчиков
                case 771:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.ET1 = val;
                            }
                        }
                    }
                    break;
                case 772:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.ET2 = val;
                            }
                        }
                    }
                    break;
                case 773:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.OUT1 = val;
                            }
                        }
                    }
                    break;
                case 774:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.OUT2 = val;
                            }
                        }
                    }
                    break;

                case 775:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.AR1 = val;
                            }
                        }
                    }
                    break;
                case 776:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.AR2 = val;
                            }
                        }
                    }
                    break;
                case 777:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 20)
                            {
                                _fbs.CEConfig.AR3 = val;
                            }
                        }
                    }
                    break;
                case 778:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            if (val <= 1)
                            {
                                _fbs.CEConfig.Recup = val;
                            }
                        }
                        if (_servActivePageEntities.IsLoadingPage)
                        {
                            _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                        }
                    }
                    break;
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

        public void SetCommandToServer(int address, int[] values)
        {
            string bufLength = values.Length > 9 ? values.Length.ToString() : "0" + values.Length.ToString();
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
