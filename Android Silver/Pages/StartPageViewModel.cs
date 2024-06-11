using Android_Silver.Entities;

using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Android_Silver.Pages;

public class StartPageViewModel : INotifyPropertyChanged
{
    public IEthernetEntities EthernetEntities { get; set; }

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
        ConnectCommand = new Command(ExecuteConnect);
    }


    async private void ExecuteConnect()
    {
        try
        {
            EthernetEntities.Client = new TcpClient();
            EthernetEntities.Client.ReceiveTimeout = 3000;
            EthernetEntities.Client.SendTimeout = 3000;

            SystemMessage = "Попытка подключения";
            EthernetEntities.Client.Connect(EthernetEntities.ConnectIP, EthernetEntities.ConnectPort);
            SystemMessage = "Подключение прошло успешно";
            //  SendIsActive = true;
            //  Connected = true;
            await Shell.Current.GoToAsync("mainPage");
        }
        catch (Exception ex)
        {
            //  SendIsActive = false;
            //  Connected = false;
            SystemMessage = ex.Message;
            EthernetEntities.Client.Close();
            EthernetEntities.Client.Dispose();
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