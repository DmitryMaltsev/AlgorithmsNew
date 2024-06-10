using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SilverAndroid.Pages
{

    public class PageWithClassViewModel : INotifyPropertyChanged
    {

        string _name="abcdefg";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ICommand GiveBonus { get; private set; }

        public PageWithClassViewModel()
        {
            GiveBonus = new Command(GiveBonusExecute);
        }
        int counter = 0;
        #region Execute methods

        async private void GiveBonusExecute(object obj)
        {
            Name = counter.ToString();
            counter += 1;
            await Shell.Current.GoToAsync("moonPhasePage");
        } 
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
