using Android_Silver.Entities;

using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace Android_Silver.Services
{

    public class TcpClientService : ITcpClientService
    {



        private NetworkStream _stream;
        private IEthernetEntities _ethernetEntities { get; set; }

        private SetPoints _setPoints { get; set; }

        private SensorsEntities _sensorsEntities { get; set; }

        private ModesEntities _modesEntities { get; set; }

        public bool IsConnecting { get; private set; } = false;
        public bool IsSending { get; private set; } = false;
        public bool IsRecieving { get; private set; } = false;


        public int ResieveCounter { get; set; }
        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<IEthernetEntities>();
            _sensorsEntities = DIContainer.Resolve<SensorsEntities>();
            _setPoints = DIContainer.Resolve<SetPoints>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            //isConnected=TryConnect(tcpClient, ip, port, ref _systemMessage);
            //RecieveData(100,8);
        }

        public async Task Connect()
        {
            try
            {
                IsConnecting = true;
                _ethernetEntities.Client = new TcpClient();
                _ethernetEntities.Client.ReceiveTimeout = 500;
                _ethernetEntities.Client.SendTimeout = 500;
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
                    string messToClient = String.IsNullOrEmpty(_ethernetEntities.MessageToSend) ? val : _ethernetEntities.MessageToSend;
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
                                if (String.Compare(messToClient, _ethernetEntities.MessageToSend, true) == 0)
                                    _ethernetEntities.MessageToSend = String.Empty;
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
                    Task.Delay(600);
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
                    //  _ethernetEntities.SystemMessage = ex.Message;
                }
                _trySendcounter += 1;
                Task.Delay(800);
            }
            while (IsSending && _trySendcounter < 4);
            IsSending = false;
            if (_trySendcounter == 4)
            {
                _ethernetEntities.IsConnected = false;
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

        void GetValueByTag(Response resp)
        {
            int floatPrec = 1;
            float bufF = 0;
            int bufInt = 0;
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
                case 108:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            if (_modesEntities.CMode1 != _modesEntities.Mode1ValuesList[Val])
                            {
                       //         _modesEntities.CMode1 = _modesEntities.Mode1ValuesList[Val];
                            }
                           
                        }
                    }
                    break;
                case 109:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.CMode2 = Val;
                        }
                    }
                    break;
                //Минимальный режим
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
                //Нормальный режим
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
                //Максимальный режим
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
                //Режим кухни
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
                //Режим отпуска
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
                //Специальный режим
                case 130:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[7].SypplySP = Val;
                        }
                    }
                    break;
                case 131:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[7].ExhaustSP = Val;
                        }
                    }
                    break;
                case 132:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[7].TempSP = Val;
                        }
                    }
                    break;
                case 133:
                    {
                        if (int.TryParse(resp.ValueString, out int Val))
                        {
                            _modesEntities.Mode1ValuesList[7].PowerLimitSP = Val;
                        }
                    }
                    break;
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
            if (_ethernetEntities.IsConnected)
            {
                if (!IsSending)
                {
                    Task.Run(() =>
                    {
                        string messToClient = val;
                        SendCommand(messToClient);
                        if (sbResult.Length > 0)
                        {
                            List<Response> responseList = new();
                            if (GetResponseData(sbResult, responseList))
                            {
                                foreach (var response in responseList)
                                {
                                    GetValueByTag(response);
                                }
                                _ethernetEntities.SystemMessage = $"Получены данные {sbResult}";
                            }
                            else
                            {
                                _ethernetEntities.SystemMessage = $"Данные не получены";
                            }
                        }
                        else
                        {
                            //      _ethernetEntities.SystemMessage = "длина возвр значения ==0";
                        }
                    });
                }
                else
                {
                    _ethernetEntities.SystemMessage = "Данные уже передаются";
                }
            }
            else
            {
                _ethernetEntities.SystemMessage = "Не подключен";
            }
        }


    }
}
