using System.Net.Sockets;

namespace Android_Silver.Entities
{
    public interface IEthernetEntities
    {
        string GateWay { get; set; }
        string IP { get; set; }
        string Subnet { get; set; }
        Response ResponseValue { get; set; }
        string ConnectIP { get; set; }
        int ConnectPort { get; set; }
        TcpClient Client { get; set; }
    }
}