using Android_Silver.Entities.ValuesEntities;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class Updater:BindableBase
    {
		private byte _isUpdate;

		public byte IsUpdate
		{
			get { return _isUpdate; }
			set { 
				_isUpdate = value; 
				OnPropertyChanged(nameof(IsUpdate));
			}
		}

		private IntValue _packetsCount;

		public IntValue PacketsCount
		{
			get { return _packetsCount; }
			set { 
				_packetsCount = value;
				OnPropertyChanged(nameof(PacketsCount));
			}
		}

		private ushort _currnetPacket;

		public ushort CurrentPacket
		{
			get { return _currnetPacket; }
			set { 
				_currnetPacket = value; 
				OnPropertyChanged($"{nameof(CurrentPacket)}");
			}
		}

		private int _resendCounter;

		public int ResendCounter
		{
			get { return _resendCounter; }
			set {
				_resendCounter = value;
				OnPropertyChanged(nameof(ResendCounter));
					}
		}

		private string _resultPackets = "0/0";

        public string ResultPackets
		{
			get { return _resultPackets; }
			set { 
				_resultPackets = value;
				OnPropertyChanged(nameof(ResultPackets));
			}
		}

		public string[] SplittedPacket;
		public byte[] BinaryData;
		public char[,] UseCharData;
		public StringBuilder FileContent;
		public List<StringBuilder> FileContentList;
        public int DataSize = 2048;
        public Updater()
        {
			PacketsCount = new IntValue(0, 100000);
            CurrentPacket = 1;
			FileContent= new StringBuilder();
			FileContentList = new List<StringBuilder>();
        }

    }
}
