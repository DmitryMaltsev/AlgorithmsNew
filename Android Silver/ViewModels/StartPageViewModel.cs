using Android_Silver.Entities;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Android_Silver.Pages;

public class StartPageViewModel : BindableBase
{
    public EthernetEntities EthernetEntities { get; set; }
    public TcpClientService TcpClientService { get; set; }

    private string _systemMessage;

    public string SystemMessage
    {
        get { return _systemMessage; }
        set
        {
            _systemMessage = value;
            OnPropertyChanged(nameof(SystemMessage));
        }
    }

    public ICommand ConnectCommand { get; private set; }
    public StartPageViewModel()
    {
        EthernetEntities = DIContainer.Resolve<EthernetEntities>();
        TcpClientService = DIContainer.Resolve<TcpClientService>();
        ConnectCommand = new Command(ExecuteConnect);

    }


    async private void ExecuteConnect()
    {
        EthernetEntities.SystemMessage = "Check";
        if (!TcpClientService.IsConnecting)
        {
            await TcpClientService.Connect();
            if (EthernetEntities.IsConnected)
            {
                await Shell.Current.GoToAsync("mainPage");
                TcpClientService.SendRecieveTask("100,08");
            }
        }
        else
        {
            EthernetEntities.SystemMessage = "В данный момент подключаемся";
        }
    }
}