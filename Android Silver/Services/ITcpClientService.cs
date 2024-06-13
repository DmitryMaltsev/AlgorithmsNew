namespace Android_Silver.Services
{
    public interface ITcpClientService
    {
        bool IsConnecting { get;}
        bool IsSending { get; }

        public  Task Connect();
       public void RecieveData(string val);
        public Task SendData(string data);
    }
}