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
using Windows.UI.Xaml;

namespace MonAssoce.ViewModels
{
    public class SearchPageViewModel : MonAssoce.Common.BindableBase
    {
        private const string MEMBER_SINCE_KEY = "MemberSince";
        ResourceLoader _res = new ResourceLoader();

        private ObservableCollection<MainHubViewModel> _hubs;
        public ObservableCollection<MainHubViewModel> Hubs
        {
            get { return this._hubs; }
            set { this.SetProperty(ref this._hubs, value); }
        }

        private bool _isDataLoaded = false;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { _isDataLoaded = value; }
        }

        public SearchPageViewModel()
        {
            this.Hubs = new ObservableCollection<MainHubViewModel>();

        }

        public async Task LoadData()
        {
            IsDataLoaded = false;
            List<News> newsList = new List<News>();
            List<Event> eventList = new List<Event>();
            List<Project> projectList = new List<Project>();
            List<Member> memberList = new List<Member>();
            List<OfficeMember> officeMemberList = new List<OfficeMember>();

            await ProcessData.LoadSettings();

            newsList = ProcessData.GetNews();
            eventList = ProcessData.GetEvents();
            projectList = ProcessData.GetProjects();
            memberList = ProcessData.GetMembers();
            officeMemberList = ProcessData.GetOfficeMembers();

            this.Hubs = this.MakePresentationHubs(newsList, eventList, projectList, memberList, officeMemberList);
            
            IsDataLoaded = true;
        }

        private ObservableCollection<MainHubViewModel> MakePresentationHubs(List<News> newsList, List<Event> eventList, List<Project> projectList, List<Member> memberList, List<OfficeMember> officeMemberList)
        {
            DateToStringConverter dateToStringConverter = new DateToStringConverter();
            MainHubViewModel tempHub = new MainHubViewModel();
            ObservableCollection<MainHubViewModel> tempHubs = new ObservableCollection<MainHubViewModel>();
            
            #region News formater

            tempHub = new MainHubViewModel();

            tempHub.HubName = _res.GetString("NewsPageTitleMain");
            tempHub.NbItems = newsList.Count;
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

            tempHub.HubName = _res.GetString("EventsPageTitleMain");
            tempHub.NbItems = eventList.Count;
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

            tempHub.HubName = _res.GetString("ProjectsPageTitleMain");
            tempHub.NbItems = projectList.Count;

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

            #region Members formater
            tempHub = new MainHubViewModel();

            tempHub.HubName = _res.GetString("MembersMainPage");
            tempHub.NbItems = projectList.Count;

            foreach (Member member in memberList)
            {
                string img;
                if (member.PictureURI == "")
                    img = "/Content/Images/Members/default.png";
                else
                    img = member.PictureURI;

                tempHub.Add(new MainItemViewModel()
                {

                    Photo = img,
                    Title = member.FirstName + " " + member.LastName,
                    Subtitle = this._res.GetString(MEMBER_SINCE_KEY) + " " + dateToStringConverter.ConvertDateToString(member.MemberSince, false, false),
                    LabelImage = "/Content/Images/Members/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed
                });
            }

            tempHubs.Add(tempHub);
            #endregion

            #region OfficeMembers formater
            tempHub = new MainHubViewModel();

            tempHub.HubName = _res.GetString("MembersMainPage");
            tempHub.NbItems = projectList.Count;

            foreach (OfficeMember member in officeMemberList)
            {
                string img;
                if (member.PictureURI == "")
                    img = "/Content/Images/Members/default.png";
                else
                    img = member.PictureURI;

                tempHub.Add(new MainItemViewModel()
                {

                    Photo = img,
                    Title = member.FirstName + " " + member.LastName,
                    Subtitle = member.Title,
                    LabelImage = "/Content/Images/Members/label.png",
                    IsDescription = false,
                    DescriptionVisibility = Visibility.Collapsed,
                    OtherVisibility = Visibility.Visible,
                    ImageOnlyVisibility = Visibility.Collapsed
                });
            }

            tempHubs.Add(tempHub);
            #endregion

            return tempHubs;

        }
    }
}
