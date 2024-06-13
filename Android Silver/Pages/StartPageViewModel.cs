using Android_Silver.Entities;
using Android_Silver.Services;

using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Android_Silver.Pages;

public class StartPageViewModel : INotifyPropertyChanged
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
               TcpClientService.RecieveData("100,05");
            }
        }
        else
        {
            EthernetEntities.SystemMessage = "В данный момент подключаемся";
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}