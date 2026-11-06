using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.MQTT;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Android_Silver.ViewModels
{
    public class MQTTViewModel : BindableBase
    {

        #region Riging proprty

        private string _systemMessage = "{\r\n    " +
            "\"server\": \"m8.wqtt.ru\",\r\n   " +
            " \"port\": 21710,\r\n    \"port_tls\": 21711,\r\n    " +
            "\"websocket\": 21713,\r\n    \"user\": \"u_I59LTD\",\r\n    " +
            "\"password\": \"HojKCWkv\"\r\n}";

        public string SystemMessage
        {
            get { return _systemMessage; }
            set
            {
                _systemMessage = value;
                OnPropertyChanged(nameof(SystemMessage));
            }
        }
        #endregion

        public ICommand SendCommand { get; private set; }
        public ICommand SendPostCommand { get; private set; }

        public MQTTEntity CMQTTEntity { get; set; }
        public EthernetEntities EthernetEntities { get; }
        public TcpClientService CTcpClientService { get; }
        public PicturesSet CPictureSet { get; }

        private FileSystemService _fileSystemService { get; set; }

        public MQTTViewModel()
        {
            //  testData = new MyData();
            SendCommand = new Command(AsyncSend);
            SendPostCommand = new Command(ExecuteSendPost);
            CMQTTEntity = DIContainer.Resolve<MQTTEntity>();
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CTcpClientService = DIContainer.Resolve<TcpClientService>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            _fileSystemService = DIContainer.Resolve<FileSystemService>();
            //   ConnectCommand = new Command(ExecuteConnect);
            CMQTTEntity.server = "https://dash.wqtt.ru";
            CMQTTEntity.port = 21710;
            CMQTTEntity.CRC = 23;
            CMQTTEntity.user = "u_I59LTD";
            CMQTTEntity.password = "HojKCWkv";
            CMQTTEntity.DeviceID = 83738;
            CMQTTEntity.Tag = 1;
        }

        private void ExecuteSend()
        {
            //MyData result = Task.Run(() => AsyncSend()).GetAwaiter().GetResult();
        }

        public async void AsyncSend()
        {
            CTcpClientService.Disconnect();
            await ExecuteConnect();
            if (EthernetEntities.IsConnected)
            {
                CMQTTEntity.MQTTInited = 1;
               // int[] index = { 1 };
               //  CTcpClientService.SetCommandToServer(149, index);
            }
            //MQTTSendGet("");
            ////MQTTSendGet("https://dash.wqtt.ru/api/devices/82116");
            //MQTTSendGet("https://dash.wqtt.ru/api/broker");
            //// MQTTSendGet("https://dash.wqtt.ru/api/devices");
        }

        async private Task ExecuteConnect()
        {
            EthernetEntities.ConnectIP =
                 $"{EthernetEntities.IP1}.{EthernetEntities.IP2}.{EthernetEntities.IP3}.{EthernetEntities.IP4}";

            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Selected);
                    await _fileSystemService.SaveToFileAsync("ConnectIP", EthernetEntities.ConnectIP);
                    CTcpClientService.SendRecieveTask();
                    SystemMessage = "Подключение успешно";
                    // TcpClientService.SendRecieveTask("137,4");ё
                }
                else
                {
                    SystemMessage = "Ошибка подключения";
                }
            }
            else
            {
                SystemMessage = "В данный момент подключаемся";
            }
        }

        private void MQTTSendGet(string address)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string? tokenString = CMQTTEntity.Token.ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
            try
            {
                SystemMessage = client.GetStringAsync(address).GetAwaiter().GetResult();
                CMQTTEntity = client.GetFromJsonAsync<MQTTEntity>(address).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SystemMessage = ex.Message;
            }
        }

        public void ExecuteSendPost()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;
            JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string? tokenString = CMQTTEntity.Token.ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
            string jsonPayload = JsonSerializer.Serialize(CMQTTEntity.Root, _serializerOptions);
            StringContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage resp = client.PostAsync("https://dash.wqtt.ru/api/devices", content).GetAwaiter().GetResult();
                resp.EnsureSuccessStatusCode();
                SystemMessage = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SystemMessage = ex.Message;
            }
        }
    }



}
