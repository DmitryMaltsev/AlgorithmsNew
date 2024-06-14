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

        private SensorsEntities _sensorsEntities { get; set; }

        public bool IsConnecting { get; private set; } = false;
        public bool IsSending { get; private set; } = false;
        public bool IsRecieving { get; private set; } = false;


        public int ResieveCounter { get; set; }
        public TcpClientService()
        {
            _ethernetEntities = DIContainer.Resolve<IEthernetEntities>();
            _sensorsEntities = DIContainer.Resolve<SensorsEntities>();
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
                        _ethernetEntities.SystemMessage = "длина возвр значения ==0";
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


        public void RecieveData(string val)
        {
            Task.Run(() =>
            {
                while (_ethernetEntities.IsConnected)
                {
                    if (!IsSending)
                    {
                        string messToClient = val;
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
                                //    _ethernetEntities.SystemMessage = $"Получены данные {sbResult} счетчик={ResieveCounter}";
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
                    Task.Delay(1000);
                }

            });
        }


        int counter = 0;

        private StringBuilder SendCommand(string command)
        {

            IsSending = true;
            sbResult = new StringBuilder();
            counter = 0;
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
                counter += 1;
                Task.Delay(300);
            }
            while (IsSending && counter < 5);
            IsSending = false;
            return sbResult;
        }





        private bool GetResponseData(StringBuilder rSB, List<Response> response)
        {
            bool isRightResponse = true;
            string[] resultVals = rSB.ToString().Split(",");
            ushort startTag = ushort.Parse(resultVals[0]);

            for (ushort i = 2; i < resultVals.Length; i++)
            {
                response.Add(new Response() { Tag = (ushort)(startTag + i - 2), ValueString = resultVals[i] });
            }
            return isRightResponse;
        }

        public void Disconnect()
        {
            _ethernetEntities.IsConnected = false;
            //SystemMessage = "Соединение разорвано";
            _ethernetEntities.Client.Close();
            _ethernetEntities.Client.Dispose();
        }

        void GetValueByTag(Response resp)
        {
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
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        _sensorsEntities.OutdoorTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 104:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        _sensorsEntities.SupplyTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 105:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        _sensorsEntities.ExhaustTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 106:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        _sensorsEntities.RoomTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 107:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        _sensorsEntities.ReturnWaterTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 300:
                    {

                    }
                    break;
                case 301:
                    {

                    }
                    break;
                case 302:
                    {

                    }
                    break;
            }
        }



        public void SendData()
        {

        }


    }
}
