using SilverAndroid.ViewModels;

using System.ComponentModel;

using Xamarin.Forms;

namespace SilverAndroid.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}