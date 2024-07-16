using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;

using System;
using System.Collections;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Android_Silver.Services
{
    public class TcpClientService
    {
        private NetworkStream _stream;
        private EthernetEntities _ethernetEntities { get; set; }
        private SetPoints _setPoints { get; set; }
        private SensorsEntities _sensorsEntities { get; set; }
        private ModesEntities _modesEntities { get; set; }
        private Alarms _alarms { get; set; }
        public bool IsConnecting { get; private set; } = false;
        public bool IsSending { get; private set; } = false;
        public bool IsRecieving { get; private set; } = false;

        private string _messageToServer = String.Empty;
        public string MessageToServer
        {
            get { return _messageToServer; }
            private set
            { _messageToServer = value; }
        }
        public int ResieveCounter { get; set; }
        public Action<int> SetMode1Action { get; set; }
        private ActivePagesEntities _activePageEntities { get; set; }

        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _sensorsEntities = DIContainer.Resolve<SensorsEntities>();
            _setPoints = DIContainer.Resolve<SetPoints>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            _activePageEntities = DIContainer.Resolve<ActivePagesEntities>();
            _alarms = DIContainer.Resolve<Alarms>();
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
                Task connectTask = _ethernetEntities.Client.ConnectAsync(_ethernetEntities.ConnectIP, _ethernetEntities.ConnectPort);
                if (await Task.WhenAny(connectTask, Task.Delay(3000)) != connectTask)
                {
                    _ethernetEntities.IsConnected = false;
                    IsConnecting = false;
                    _ethernetEntities.Client.Close();
                    connectTask.Dispose();
                    throw new Exception("Ошибка: не удалось подключиться к серверу");
                }
                else
                {
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
                     Task.Delay(50);
                 }
             });
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
                    byte[] data = new byte[100];
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
                _ethernetEntities.IsConnected = false;
                _ethernetEntities.SystemMessage = "Превышено максимальное количество попыток - 10";
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
            float bufF = 0;
            switch (resp.Tag)
            {
                case 100:
                    {
                        _ethernetEntities.IP = resp.ValueString;
                    }
                    break;
                case 101:
                    {
                        _ethernetEntities.Subnet = resp.ValueString;
                    }
                    break;
                case 102:
                    {
                        _ethernetEntities.GateWay = resp.ValueString;
                    }
                    break;
                case 103:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref bufF))
                        {
                            _sensorsEntities.OutdoorTemp = bufF;
                        }
                    }
                    break;
                case 104:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref bufF))
                        {
                            _sensorsEntities.SupplyTemp = bufF;
                        }
                    }
                    break;
                case 105:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref bufF))
                        {
                            _sensorsEntities.ExhaustTemp = bufF;
                        }
                    }
                    break;
                case 106:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref bufF))
                        {
                            _sensorsEntities.RoomTemp = bufF;
                        }
                    }
                    break;
                case 107:
                    {
                        if (StringToFloat(resp.ValueString, floatPrec, ref bufF))
                        {
                            _sensorsEntities.ReturnWaterTemp = bufF;
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
                            if (_alarms.Alarms1 != val)
                            {
                                _alarms.Alarms1 = val;
                                BitArray bitArrrayBuf = _alarms.GetAlarmsByBits(_alarms.Alarms1);
                                _alarms.ConverBitArrayToAlarms(bitArrrayBuf, 0);
                            }
                        }
                    }
                    break;
                case 136:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            if (_alarms.Alarms2 != val)
                            {
                                _alarms.Alarms2 = val;
                                BitArray bitArrrayBuf = _alarms.GetAlarmsByBits(_alarms.Alarms2);
                                _alarms.ConverBitArrayToAlarms(bitArrrayBuf, 1);
                            }
                        }
                    }
                    break;
                //Данные о режиме отпуска
                case 137:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum=val;
                        }
                    }
                    break;
                case 138:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = val;
                        }
                    }
                    break;
                case 139:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = val;
                        }
                    }
                    break;
                case 140:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _modesEntities.Mode2ValuesList[2].TimeModeValues[0].CMode1Num = val;
                        }
                    }
                    break;
                //Строка 2
                case 141:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = val;
                        }
                    }
                    break;
                case 142:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = val;
                        }
                    }
                    break;
                case 143:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = val;
                        }
                    }
                    break;
                case 144:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _modesEntities.Mode2ValuesList[2].TimeModeValues[1].CMode1Num = val;
                        }
                    }
                    break;
                //Строка 3
                case 145:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = val;
                        }
                    }
                    break;
                case 146:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = val;
                        }
                    }
                    break;
                case 147:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = val;
                        }
                    }
                    break;
                case 148:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _modesEntities.Mode2ValuesList[2].TimeModeValues[2].CMode1Num = val;
                        }
                    }
                    break;
                //Строка 4
                case 149:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = val;
                        }
                    }
                    break;
                case 150:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = val;
                        }
                    }
                    break;
                case 151:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {

                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = val;
                        }
                    }
                    break;
                case 152:
                    {
                        if (ushort.TryParse(resp.ValueString, out ushort val))
                        {
                            _modesEntities.Mode2ValuesList[2].TimeModeValues[3].CMode1Num = val;
                        }
                    }
                    break;
                //Проверка того, что данные записаны
                case 300:
                    {
                        _setPoints.SP1Count += 1;
                    }
                    break;
                case 301:
                    {
                        _setPoints.SP2Count += 1;
                    }
                    break;
                case 302:
                    {
                        _setPoints.SP3Count += 1;
                    }
                    break;
                case 303:
                    {
                        _setPoints.SPFCount += 1;
                    }
                    break;
                case 308:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode1ValuesByIndex(Val);
                            _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 309:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.SetMode2ValuesByIndex(Val);
                            // _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                #region Минимальный режим
                case 310:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].SypplySP = Val;
                        }
                    }
                    break;
                case 311:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].ExhaustSP = Val;
                        }
                    }
                    break;
                case 312:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].TempSP = Val;
                        }
                    }
                    break;
                case 313:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[1].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Нормальный режим
                case 314:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].SypplySP = Val;
                        }
                    }
                    break;
                case 315:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].ExhaustSP = Val;
                        }
                    }
                    break;
                case 316:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].TempSP = Val;
                        }
                    }
                    break;
                case 317:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[2].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Максимальный режим
                case 318:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].SypplySP = Val;
                        }
                    }
                    break;
                case 319:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].ExhaustSP = Val;
                        }
                    }
                    break;
                case 320:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].TempSP = Val;
                        }
                    }
                    break;
                case 321:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[3].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим кухни
                case 322:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].SypplySP = Val;
                        }
                    }
                    break;
                case 323:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].ExhaustSP = Val;
                        }
                    }
                    break;
                case 324:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].TempSP = Val;
                        }
                    }
                    break;
                case 325:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[4].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Режим отпуска
                case 326:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].SypplySP = Val;
                        }
                    }
                    break;
                case 327:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].ExhaustSP = Val;
                        }
                    }
                    break;
                case 328:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].TempSP = Val;
                        }
                    }
                    break;
                case 329:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[5].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                #region Специальный режим
                case 330:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].SypplySP = Val;
                        }
                    }
                    break;
                case 331:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].ExhaustSP = Val;
                        }
                    }
                    break;
                case 332:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].TempSP = Val;
                        }
                    }
                    break;
                case 333:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[8].PowerLimitSP = Val;
                        }
                    }
                    break;
                #endregion
                //Режим кухни.
                case 334:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {
                            //_modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = val;
                            //_activePageEntities.SetActivePageState(ActivePageState.MainPage);
                        }
                    }
                    break;
                case 337:
                    {
                        if (int.TryParse(resp.ValueString, out int val))
                        {

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



    }
}
