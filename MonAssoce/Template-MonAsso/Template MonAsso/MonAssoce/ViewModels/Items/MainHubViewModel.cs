using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MonAssoce.ViewModels.Items
{
    public class MainHubViewModel : ObservableCollection<MainItemViewModel>
    {
        private const string PRESENTATION_BG_PATH = "/Content/Images/zoom_out_presentation.png";
        private const string NEWS_BG_PATH = "/Content/Images/zoom_out_news.png";
        private const string EVENTS_BG_PATH = "/Content/Images/zoom_out_events.png";
        private const string PROJECTS_BG_PATH = "/Content/Images/zoom_out_projects.png";

        private int _ID;
        private string _hubName;
        private string _imagePath;
        private int _itemHeight = 100;
        private int _nbItems;
        private Visibility _nbItemsVisibility;
        private Visibility _descriptionVisibilty;

        public int ItemHeight
        {
            get { return this._itemHeight; }
            set { this._itemHeight = value; }
        }

        public int ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        public string HubName
        {
            get { return this._hubName; }
            set { this._hubName = value; }
        }

        public string HubTitle
        {
            get
            {
                ResourceLoader res = new ResourceLoader();
                if (HubName != res.GetString("Presentation"))
                {
                    return this._hubName + " (" + this.NbItems + ")";
                }
                else
                {
                    return HubName;
                }
            }
        }

        public string ImagePath
        {
            get 
            {
                switch (this.ID)
                {
                    case 0:
                        this._imagePath = PRESENTATION_BG_PATH;
                        break;
                    case 1:
                        this._imagePath = NEWS_BG_PATH;
                        break;
                    case 2:
                        this._imagePath = EVENTS_BG_PATH;
                        break;
                    case 3:
                        this._imagePath = PROJECTS_BG_PATH;
                        break;
                }
                    return this._imagePath; 
            }
            set { this._imagePath = value; }
        }

        public int NbItems
        {
            get { return this._nbItems; }
            set { this._nbItems = value; }
        }

        public Visibility NbItemsVisibility
        {
            get 
            {
                return this._nbItemsVisibility; 
            }
            set 
            {
                this._nbItemsVisibility = value;
                this._descriptionVisibilty = (this._nbItemsVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility DescriptionVisibilty
        {
            get { return _descriptionVisibilty; }
            set { _descriptionVisibilty = value; }
        }
        
        public new IEnumerator<MainItemViewModel> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<MainItemViewModel>)base.GetEnumerator();
        }
    }
}
