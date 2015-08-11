using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Data;
using MonAssoce.Data.Models;
using MonAssoce.Libs.Helpers;
using MonAssoce.ViewModels.Items;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using MonAssoce.Data.Libs;

namespace MonAssoce.ViewModels
{
    public class MainPageViewModel : MonAssoce.Common.BindableBase
    {
        //private const string DFT_PRE_URI = "/Assets/splashscreen.png";
        private const string DFT_PRE_URI = "/Content/Images/Members/default.png";
        ResourceLoader _res = new ResourceLoader();

        private ObservableCollection<MainHubViewModel> _hubs;
        public ObservableCollection<MainHubViewModel> Hubs
        {
            get { return this._hubs; }
            set { this.SetProperty(ref this._hubs, value); }
        }

        private string _nbMembers;
        public string NbMembers
        {
            get { return _nbMembers; }
            set { _nbMembers = value; }
        }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { _isDataLoaded = value; }
        }

        private bool _isAboutPaneAdded;
        public bool IsAboutPaneAdded
        {
            get { return _isAboutPaneAdded; }
            set { _isAboutPaneAdded = value; }
        }

        private DispatcherTimer _timer = new DispatcherTimer();
        public DispatcherTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public MainPageViewModel()
        {
            this.Hubs = new ObservableCollection<MainHubViewModel>();
        }

        public async Task LoadData()
        {
            this.IsDataLoaded = false;
            ProcessData.Reset();
            await ProcessData.LoadSettings();
            this.CheckImages();
        }

        private ObservableCollection<MainHubViewModel> InitializeHubs(Presentation presentList, List<News> newsList, List<Event> eventList, List<Project> projectList)
        {
            DateToStringConverter dateToStringConverter = new DateToStringConverter();
            MainHubViewModel tempHub = new MainHubViewModel();
            ObservableCollection<MainHubViewModel> tempHubs = new ObservableCollection<MainHubViewModel>();

            #region Presentation formater
            tempHub.ID = tempHub.Count;
            tempHub.HubName = _res.GetString("Presentation");
            tempHub.NbItemsVisibility = Visibility.Collapsed;
            tempHub.Add(new MainItemViewModel()
            {
                BigPhoto = (presentList.BigPictureURI != null && presentList.BigPictureURI != string.Empty ? presentList.BigPictureURI : DFT_PRE_URI),
                IsDescription = true,
                DescriptionVisibility = Visibility.Visible,
                OtherVisibility = Visibility.Collapsed,
                ImageOnlyVisibility = Visibility.Collapsed,
                Description = presentList.Description,
                Photo1 = ((presentList.PicturesURI.Count >= 1 && presentList.PicturesURI[0] != string.Empty) ? presentList.PicturesURI[0] : DFT_PRE_URI),
                Photo2 = ((presentList.PicturesURI.Count >= 2 && presentList.PicturesURI[1] != string.Empty) ? presentList.PicturesURI[1] : DFT_PRE_URI),
                Photo3 = ((presentList.PicturesURI.Count >= 3 && presentList.PicturesURI[2] != string.Empty) ? presentList.PicturesURI[2] : DFT_PRE_URI)
            });
            for (int i = 3; i < presentList.PicturesURI.Count; i++)
            {
                tempHub.Add(new MainItemViewModel()
                {
                    Photo = ((presentList.PicturesURI[i] != string.Empty) ? presentList.PicturesURI[i] : DFT_PRE_URI),
                    IsDescription = true,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Collapsed,
                    ImageOnlyVisibility = Visibility.Visible
                });
            }
            tempHubs.Add(tempHub);
            #endregion

            #region News formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("NewsPageTitleMain");
            tempHub.NbItems = newsList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;
            int cpt = 0;
            foreach (News news in newsList)
            {
                string img;
                if (news.ImageURL == "")
                    img = "/Content/Images/News/default.png";
                else
                    img = news.ImageURL;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = news.ID,
                    Photo = img,
                    Title = news.Title,
                    Subtitle = dateToStringConverter.ConvertDateToString(news.PubDate, news.Schedule, false),
                    IsDescription = false,
                    LabelImage = "/Content/Images/News/label.png",
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    IsEvent = false,
                    IsNews = true,
                    IsProject = false
                });
            }

            tempHubs.Add(tempHub);
            #endregion

            #region Events formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("EventsPageTitleMain");
            tempHub.NbItems = eventList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;
            cpt = 0;
            foreach (Event events in eventList)
            {
                string img;
                if (events.PictureURI == "")
                    img = "/Content/Images/Events/default.png";
                else
                    img = events.PictureURI;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = events.ID,
                    Photo = img,
                    Title = events.Title,
                    Subtitle = dateToStringConverter.ConvertDateToString(events.Date, events.Schedule, false),
                    LabelImage = "/Content/Images/Events/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    IsEvent = true,
                    IsNews = false,
                    IsProject = false
                });

                cpt++;
            }

            tempHubs.Add(tempHub);

            #endregion

            #region Projects formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("ProjectsPageTitleMain");
            tempHub.NbItems = projectList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;

            foreach (Project project in projectList)
            {
                string img;
                if (project.PictureURI == "")
                    img = "/Content/Images/Projects/default.png";
                else
                    img = project.PictureURI;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = project.ID,
                    Photo = img,
                    Title = project.Title,
                    Subtitle = project.SubTitle,
                    LabelImage = "/Content/Images/Projects/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    IsEvent = false,
                    IsNews = false,
                    IsProject = true
                });

            }

            tempHubs.Add(tempHub);
            #endregion

            return tempHubs;
        }

        private ObservableCollection<MainHubViewModel> InitializeHubsFromOnlineData(Presentation presentList, List<News> newsList, List<Event> eventList, List<Project> projectList)
        {
            DateToStringConverter dateToStringConverter = new DateToStringConverter();
            MainHubViewModel tempHub = new MainHubViewModel();
            ObservableCollection<MainHubViewModel> tempHubs = new ObservableCollection<MainHubViewModel>();

            #region Presentation formater
            tempHub.ID = tempHub.Count;
            tempHub.HubName = _res.GetString("Presentation");
            tempHub.NbItemsVisibility = Visibility.Collapsed;
            tempHub.Add(new MainItemViewModel()
            {
                BigPhoto = (presentList.BigPictureURI != null && presentList.BigPictureURI != string.Empty ? presentList.BigPictureURI : DFT_PRE_URI),
                IsDescription = true,
                DescriptionVisibility = Visibility.Visible,
                OtherVisibility = Visibility.Collapsed,
                ImageOnlyVisibility = Visibility.Collapsed,
                Description = presentList.Description,
                Photo1 = ((presentList.PicturesURI.Count >= 1 && presentList.PicturesURI[0] != string.Empty) ? presentList.PicturesURI[0] : DFT_PRE_URI),
                Photo2 = ((presentList.PicturesURI.Count >= 2 && presentList.PicturesURI[1] != string.Empty) ? presentList.PicturesURI[1] : DFT_PRE_URI),
                Photo3 = ((presentList.PicturesURI.Count >= 3 && presentList.PicturesURI[2] != string.Empty) ? presentList.PicturesURI[2] : DFT_PRE_URI)
            });
            for (int i = 3; i < presentList.PicturesURI.Count; i++)
            {
                tempHub.Add(new MainItemViewModel()
                {
                    Photo = ((presentList.PicturesURI[i] != string.Empty) ? presentList.PicturesURI[i] : DFT_PRE_URI),
                    IsDescription = true,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Collapsed,
                    ImageOnlyVisibility = Visibility.Visible
                });
            }
            tempHubs.Add(tempHub);
            #endregion

            #region News formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("NewsPageTitleMain");
            tempHub.NbItems = newsList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;
            int cpt = 0;
            foreach (News news in newsList)
            {
                string img;
                if (news.ImageURL == "")
                    img = "/Content/Images/News/default.png";
                else
                    img = news.RemotePictureURI;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = news.ID,
                    Photo = img,
                    Title = news.Title,
                    Subtitle = dateToStringConverter.ConvertDateToString(news.PubDate, news.Schedule, false),
                    IsDescription = false,
                    LabelImage = "/Content/Images/News/label.png",
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    IsEvent = false,
                    IsNews = true,
                    IsProject = false
                });
            }

            tempHubs.Add(tempHub);
            #endregion

            #region Events formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("EventsPageTitleMain");
            tempHub.NbItems = eventList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;
            cpt = 0;
            foreach (Event events in eventList)
            {
                string img;
                if (events.PictureURI == "")
                    img = "/Content/Images/Events/default.png";
                else
                    img = events.RemotePictureURI;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = events.ID,
                    Photo = img,
                    Title = events.Title,
                    Subtitle = dateToStringConverter.ConvertDateToString(events.Date, events.Schedule, false),
                    LabelImage = "/Content/Images/Events/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    IsEvent = true,
                    IsNews = false,
                    IsProject = false
                });

                cpt++;
            }

            tempHubs.Add(tempHub);

            #endregion

            #region Projects formater
            tempHub = new MainHubViewModel();
            tempHub.ID = tempHubs.Count;
            tempHub.HubName = _res.GetString("ProjectsPageTitleMain");
            tempHub.NbItems = projectList.Count;
            tempHub.NbItemsVisibility = Visibility.Visible;

            foreach (Project project in projectList)
            {
                string img;
                if (project.PictureURI == "")
                    img = "/Content/Images/Projects/default.png";
                else
                    img = project.RemotePictureURI;

                tempHub.Add(new MainItemViewModel()
                {
                    ID = project.ID,
                    Photo = img,
                    Title = project.Title,
                    Subtitle = project.SubTitle,
                    LabelImage = "/Content/Images/Projects/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed,
                    IsEvent = false,
                    IsNews = false,
                    IsProject = true
                });

            }

            tempHubs.Add(tempHub);
            #endregion

            return tempHubs;
        }

        public void CheckImages()
        {
            this.Timer.Tick += timer_Tick;
            this.Timer.Interval = new TimeSpan(0, 0, 1);
            this.Timer.Start();
        }

        async void timer_Tick(object sender, object e)
        {
            if (LocalStorage.Instance.activeDownloads.Count == 0)
            {
                this.Timer.Stop();
                await this.LoadHubs();
            }
        }

        public async Task LoadHubs()
        {
            await ProcessData.LoadSettings();
            if (!ProcessData.CurrentSettings.PresentationSourceURI.Equals("Presentation"))
            {
                Presentation presentList = new Presentation();
                List<News> newsList = new List<News>();
                List<Event> eventList = new List<Event>();
                List<Project> projectList = new List<Project>();
                List<Member> memberList = new List<Member>();
                List<OfficeMember> officeMembersList = new List<OfficeMember>();
                presentList = ProcessData.GetPresentation();
                newsList = ProcessData.GetNews();
                eventList = ProcessData.GetEvents();
                projectList = ProcessData.GetProjects();
                memberList = ProcessData.GetMembers();
                officeMembersList = ProcessData.GetOfficeMembers();

                NbMembers = _res.GetString("MembersMainPage") + " (" + (memberList.Count + officeMembersList.Count) + ")";

                this.Hubs = this.InitializeHubs(presentList, newsList, eventList, projectList);
                this.IsDataLoaded = true;
            }
        }

        public void LoadHubsFromOnlineData()
        {
            Presentation presentList = new Presentation();
            List<News> newsList = new List<News>();
            List<Event> eventList = new List<Event>();
            List<Project> projectList = new List<Project>();
            List<Member> memberList = new List<Member>();
            List<OfficeMember> officeMembersList = new List<OfficeMember>();
            presentList = ProcessData.GetPresentation();
            newsList = ProcessData.GetNews();
            eventList = ProcessData.GetEvents();
            projectList = ProcessData.GetProjects();
            memberList = ProcessData.GetMembers();
            officeMembersList = ProcessData.GetOfficeMembers();

            NbMembers = _res.GetString("MembersMainPage") + " (" + (memberList.Count + officeMembersList.Count) + ")";

            this.Hubs = this.InitializeHubsFromOnlineData(presentList, newsList, eventList, projectList);
            this.IsDataLoaded = true;
        }

    }
}
