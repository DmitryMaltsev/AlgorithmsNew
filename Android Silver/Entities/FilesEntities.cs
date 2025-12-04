using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class FilesEntities:BindableBase
    {
		private string _systemMessage="Загрузить .bin файл для прошивки";
		public string SystemMessage
		{
			get { return _systemMessage; }
			set { 
				_systemMessage = value; 
				OnPropertyChanged(nameof(SystemMessage));
			}
		}

        public Task<FileResult> CFileResult { get; set; }
    }
}
