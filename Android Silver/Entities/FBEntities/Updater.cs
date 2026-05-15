using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System.Text;

namespace Android_Silver.Entities.FBEntities
{
    enum AutoUpdateState
    {
        BOOTCTRL_AUTOUPD_OFF,
        BOOTCTRL_AUTOUPD_NEWFW,
        BOOTCTRL_AUTOUPD_BKPFW,
        VCHAL_BOOTCTRL_AUTOUPD_MAX,
        BOOTCTRL_AUTOUPD_ERROR=255
    }
    public class Updater : BindableBase
    {
    

        #region Rising properties
        private byte _isUpdate;

        public byte IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged(nameof(IsUpdate));
                NotIsUpdate = _isUpdate == 0 ? true : false;
            }
        }

        private bool _notIsUpdate = true;

        public bool NotIsUpdate
        {
            get { return _notIsUpdate; }
            set
            {

                _notIsUpdate = value;
                OnPropertyChanged(nameof(NotIsUpdate));
            }
        }

        private IntValue _packetsCount;

        public IntValue PacketsCount
        {
            get { return _packetsCount; }
            set
            {
                _packetsCount = value;
                OnPropertyChanged(nameof(PacketsCount));
            }
        }

        private ushort _currnetPacket;

        public ushort CurrentPacket
        {
            get { return _currnetPacket; }
            set
            {
                _currnetPacket = value;
                OnPropertyChanged($"{nameof(CurrentPacket)}");
            }
        }

        private int _resendCounter;

        public int ResendCounter
        {
            get { return _resendCounter; }
            set
            {
                _resendCounter = value;
                OnPropertyChanged(nameof(ResendCounter));
            }
        }

        private string _resultPackets = "0/0";

        public string ResultPackets
        {
            get { return _resultPackets; }
            set
            {
                _resultPackets = value;
                OnPropertyChanged(nameof(ResultPackets));
            }
        }

        private List<byte> _fwVerCur;

        public List<byte> FWVerCur
        {
            get { return _fwVerCur; }
            set
            {
                _fwVerCur = value;
            }
        }

        private List<byte> _fwVerNew;
        public List<byte> FWVerNew
        {
            get { return _fwVerNew; }
            set
            {
                _fwVerNew = value;
            }
        }

        private List<byte> _fwVerBkp;
        public List<byte> FWVerBkp
        {
            get { return _fwVerBkp; }
            set
            {
                _fwVerBkp = value;
            }
        }

        private string _fwVerСurWord=String.Empty;

        public string FWVerCurWord
        {
            get { return _fwVerСurWord; }
            set
            {
                _fwVerСurWord = value;
                OnPropertyChanged(nameof(FWVerCurWord));
            }
        }

        private string _fwVerNewWord = String.Empty;

        public string FWVerNewWord
        {
            get { return _fwVerNewWord; }
            set
            {
                _fwVerNewWord = value;
                OnPropertyChanged(nameof(FWVerNewWord));
            }
        }

        private string _fwVerBkpWord = String.Empty;

        public string FWVerBkpWord
        {
            get { return _fwVerBkpWord; }
            set
            {
                _fwVerBkpWord = value;
                OnPropertyChanged(nameof(FWVerBkpWord));
            }
        }

        private int _autoUpdIndex;

        public int AutoUpdIndex
        {
            get { return _autoUpdIndex; }
            set {
                _autoUpdIndex = value;
                OnPropertyChanged(nameof(AutoUpdIndex));
            }
        }
        private List<string> _autoUpdList= new List<string>() { "Отключено", "Новая прошивка", "Резервная прошивка" };

        public List<string> AutoUpdList
        {
            get { return _autoUpdList; }
            set { 
                _autoUpdList = value;
                OnPropertyChanged(nameof(AutoUpdList));
            }
        }


        #endregion
        public string[] SplittedPacket;
        public byte[] BinaryData;
        public char[,] UseCharData;
        public StringBuilder FileContent;
        public List<StringBuilder> FileContentList;
        public int HexSize = 512;//1024;
        public int BinSize;
        PicturesSet _pictureSet;
        public Updater()
        {
            PacketsCount = new IntValue(0, 100000);
            CurrentPacket = 1;
            FileContent = new StringBuilder();
            FileContentList = new List<StringBuilder>();
            BinSize = HexSize / 2;
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            FWVerCur = [0, 0, 0, 0];
            FWVerNew = [0, 0, 0, 0];
            FWVerBkp = [0, 0, 0, 0];
            SetFWCur(FWVerCur);
            SetFWNew(FWVerCur);
            SetFWBkp(FWVerCur);
        }

        public void SetFWCur(List<byte> fwVer)
        {
            FWVerCurWord=String.Empty;
            for (int i = 0; i < fwVer.Count; i++)
            {
                FWVerCurWord += fwVer[i];
                if (i < fwVer.Count - 1)
                {
                    FWVerCurWord += ".";
                }
            }
        }

        public void SetFWBkp(List<byte> fwVer)
        {
            FWVerBkpWord = String.Empty;
            for (int i = 0; i < fwVer.Count; i++)
            {
                FWVerBkpWord += fwVer[i];
                if (i < fwVer.Count - 1)
                {
                    FWVerBkpWord += ".";
                }
            }
        }

        public void SetFWNew(List<byte> fwVer)
        {
            FWVerNewWord = String.Empty;
            for (int i = 0; i < fwVer.Count; i++)
            {
                FWVerNewWord += fwVer[i];
                if (i < fwVer.Count - 1)
                {
                    FWVerNewWord += ".";
                }
            }
        }


    }
}
