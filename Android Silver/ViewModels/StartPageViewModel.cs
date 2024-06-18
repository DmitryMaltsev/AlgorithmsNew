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
    public IEthernetEntities EthernetEntities { get; set; }
    public ITcpClientService TcpClientService { get; set; }

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
        EthernetEntities = DIContainer.Resolve<IEthernetEntities>();
        TcpClientService = DIContainer.Resolve<ITcpClientService>();
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
            EthernetEntities.SystemMessage = "� ������ ������ ������������";
        }
    }
}