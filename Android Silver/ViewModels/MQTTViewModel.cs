using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            testData = new MyData();
            SendCommand = new Command(AsyncSend);
            SendPostCommand = new Command(ExecuteSendPost);
            //   ConnectCommand = new Command(ExecuteConnect);
        }

        private void ExecuteConnect(object obj)
        {

        }
        MyData testData;
        private void ExecuteSend()
        {
            //MyData result = Task.Run(() => AsyncSend()).GetAwaiter().GetResult();
        }

        public void AsyncSend()
        {

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string? tokenString = Token.ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
            try
            {
                SystemMessage = client.GetStringAsync("https://dash.wqtt.ru/api/broker").GetAwaiter().GetResult();
             
                testData = client.GetFromJsonAsync<MyData>("https://dash.wqtt.ru/api/broker").GetAwaiter().GetResult();
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
            Root root = new Root();
            root.name = "Название устройства";
            root.type = 2;
            root.room = "Комната";
            List<OnOff> onOff = new List<OnOff>();
            onOff.Add(new OnOff() {cmd_on="1",cmd_off="0",topic_cmd="topic"});
            root.on_off = onOff;

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string? tokenString = Token.ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenString);
            string jsonPayload = JsonSerializer.Serialize(root, _serializerOptions);


            try
            {
               // SystemMessage = client.PostAsync("")
                testData = client.GetFromJsonAsync<MyData>("https://dash.wqtt.ru/api/broker").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SystemMessage = ex.Message;
            }
        }
    }

    public class MyData
    {
        public string server { get; set; }
        public int port { get; set; }
        public int port_tls { get; set; }
        public int websocket { get; set; }
        public string user { get; set; }
        public string password { get; set; }
    }

    public class OnOff
    {
        public string topic_cmd { get; set; }
        public string cmd_on { get; set; }
        public string cmd_off { get; set; }
    }

    public class Root
    {
        public string name { get; set; }
        public int type { get; set; }
        public string room { get; set; }
        public List<OnOff> on_off { get; set; }
    }

}
