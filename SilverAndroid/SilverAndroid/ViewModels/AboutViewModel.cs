using System;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SilverAndroid.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public void Test() 
        {
  

        }


        public ICommand OpenWebCommand { get; }
    }
}