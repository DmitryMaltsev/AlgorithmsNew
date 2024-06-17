namespace Android_Silver.Pages;

public partial class IPPage : ContentPage
{
    private IPPageViewModel _ipPageViewModel { get; set; }
    public IPPage()
    {
        InitializeComponent();
        _ipPageViewModel = new IPPageViewModel();
        BindingContext = _ipPageViewModel;

    }
}