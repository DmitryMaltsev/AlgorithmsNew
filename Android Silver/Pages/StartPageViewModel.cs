using Android_Silver.Entities;

using System.ComponentModel;

namespace Android_Silver.Pages;

public class StartPageViewModel : INotifyPropertyChanged
{
    public IEthernetEntities EthernetEntities { get; set; }
    public StartPageViewModel()
	{
        EthernetEntities = DIContainer.Resolve<IEthernetEntities>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
}