using Android_Silver.Entities.Modes;

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

        private string _token = "gTKZKsSHJeTdbmEWOpNEF4Ntk81YJf4Hhtv2LaYUBxkTcocCzEWM4MQc";
        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                OnPropertyChanged(nameof(Token));
            }
        }

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

        public AsyncCommand ConnectCommand { get; private set; }

        public MQTTViewModel()
        {
          //  testData = new MyData();
            SendCommand = new Command(AsyncSend);
            SendPostCommand = new Command(ExecuteSendPost);
            //   ConnectCommand = new Command(ExecuteConnect);
        }

        private void ExecuteConnect(object obj)
        {

        }
        //MyData testData;
        private void ExecuteSend()
        {
            //MyData result = Task.Run(() => AsyncSend()).GetAwaiter().GetResult();
        }

        public void AsyncSend()
        {

            //if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            //    return;
            //using HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string? tokenString = Token.ToString();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
            //try
            //{
            //    SystemMessage = client.GetStringAsync("https://dash.wqtt.ru/api/devices/82116 ").GetAwaiter().GetResult();

            //    testData = client.GetFromJsonAsync<MyData>("https://dash.wqtt.ru/api/devices/82116 ").GetAwaiter().GetResult();
            //}
            //catch (Exception ex)
            //{
            //    SystemMessage = ex.Message;
            //}
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
            //Root root = new Root();
            //root.name = "Тестовый вентилятор";
            //root.type = 28;
            //root.room = "Общая";
            //root.on_off = new List<OnOff>();
            //root.on_off.Add(new OnOff() { cmd_on = "1", cmd_off = "0", topic_cmd = "topic/#" });
            //root.range = new List<Range>();
            //root.range.Add(new Range() { type = 4, topic_cmd = "topic/write", topic_state = "topic/read", max = 100, min = 0, precision = 10, multiplier = 1 });
            //root.sensors_float = new List<SensorFloat>();
            //root.sensors_float.Add(new SensorFloat() {multiplier=0.1f, type=0,topic="asd"});
            //root.mode= new List<Mode>();
            //root.mode.Add(new Mode());




            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string? tokenString = Token.ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
         //   string jsonPayload = JsonSerializer.Serialize(root, _serializerOptions);
         //   StringContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            try
            {
           //     HttpResponseMessage resp = client.PostAsync("https://dash.wqtt.ru/api/devices", content).GetAwaiter().GetResult();
           //     resp.EnsureSuccessStatusCode();
          //      SystemMessage = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SystemMessage = ex.Message;
            }
        }
    }



}
