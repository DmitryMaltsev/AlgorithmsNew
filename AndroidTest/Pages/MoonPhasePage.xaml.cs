namespace SilverAndroid.Pages;


public partial class MoonPhasePage : ContentPage
{
	public MoonPhasePage()
	{
		InitializeComponent();
		Btn.Clicked += async (s, e) => await Shell.Current.GoToAsync("pageWithText?sendedText=s123 Text?sended2Text=213");
	}
}