namespace SilverAndroid.Pages;

[QueryProperty(nameof(SendedText),"sendedText")]
[QueryProperty(nameof(Sended2Text), "sended2Text")]
public partial class PageWithText : ContentPage
{
    private PageWithClassViewModel _viewModel;
    public PageWithText()
	{
		InitializeComponent();
        _viewModel = new PageWithClassViewModel();
        BindingContext = _viewModel;
    }


	private string sendedText;

	public string SendedText
	{
		get { return sendedText; }
		set { 
			sendedText = value;
			UpdateValue(sendedText);
		}
	}


    private string sended2Text;

    public string Sended2Text
    {
        get { return sended2Text; }
        set
        {
            sended2Text = value;
            UpdateValue(sended2Text);
        }
    }

    private void UpdateValue(string sendedText)
    {
		//lTExt.Text = sendedText;
    }
}