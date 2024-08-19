

namespace Android_Silver.Entities.Visual
{
    public class CarouselViewData:ViewModels.BindableBase
    {
        private List<ViewData> _dataList;

        public List<ViewData> DataList
        {
            get { return _dataList; }
            set { _dataList = value;
                OnPropertyChanged(nameof(DataList));
            }
        }

        public CarouselViewData()
        {
            DataList = new List<ViewData>();
            DataList.Add(new ViewData() { TimeText = "1" });
            DataList.Add(new ViewData() { TimeText = "2" });
            DataList.Add(new ViewData() { TimeText = "2" });
        }


	}

	public class ViewData : ViewModels.BindableBase
    {
        private string _timeText;

        public string TimeText
        {
            get { return _timeText; }
            set
            {
                _timeText = value;
                OnPropertyChanged(nameof(TimeText));
            }
        }
    }

}
