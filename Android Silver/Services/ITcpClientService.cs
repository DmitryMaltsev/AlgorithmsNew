namespace Android_Silver.Services
{
    public interface ITcpClientService
    {
        bool IsConnecting { get;}
        bool IsSending { get; }
        int ResieveCounter { get; set; }

        public  Task Connect();
        void Disconnect();
        public void SendRecieveTask(string val);
        public  void SendData(string data);
    }
}