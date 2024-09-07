using Android_Silver.Entities;
using Android_Silver.Entities.Visual;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    class ServicePageViewModel:BindableBase
    {
        #region Rising properties

        private int _entryPass=0000;
        public int EntryPass
        {
            get { return _entryPass; }
            set { 
                _entryPass = value;
                OnPropertyChanged(nameof(EntryPass));
            }
        }

        private string _entryMessage="Введите пароль для входа";

        public string EntryMessage
        {
            get { return _entryMessage; }
            set {
                _entryMessage = value; 
                OnPropertyChanged(nameof(EntryMessage));
            }
        }


        public ICommand EntryPassedCommand { get; private set; }    
        #endregion
        public PicturesSet CPictureSet { get; set; }
        public ServicePageViewModel()
        {
           
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            EntryPassedCommand = new Command(ExecuteEntryPass);
        }




        #region Execute entry
        private void ExecuteEntryPass(object obj)
        {
            if (EntryPass == 4444)
            {
                EntryMessage = "Пароль введен верно";
            }
            else
            {
                EntryMessage = "Пароль введен неверно";
            }
        } 
        #endregion
    }
}
