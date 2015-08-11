using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MonAssoce.Data.Models;
using MonAssoce.Data.Libs;
using Windows.ApplicationModel.Resources;
using System.IO;
using Windows.Storage;

namespace MonAssoce.Data
{
    public abstract class ProcessData
    {
        #region Constants

        public static string presentationNameFile = "Presentation";
        public static string eventsNameFile = "Events";
        public static string membersNameFile = "Members";
        public static string officeMembersNameFile = "OfficeMembers";
        public static string newsNameFile = "News";
        public static string projectFile = "Project";

        #endregion

        public static Settings CurrentSettings = null;
        public static Presentation AllPresentation = null;
        public static List<News> AllNews = null;
        public static List<Event> AllEvents = null;
        public static List<Event> UpcomingEvents = null;
        public static List<Member> AllMembers = null;
        public static List<Project> AllProjects = null;
        public static List<OfficeMember> AllOfficeMembers = null;
        private const string SettingsPath = "Data/Settings.xml";

        /// <summary>
        /// Load local app settings
        /// </summary>
        /// <returns></returns>
        public async static Task<Settings> LoadSettings()
        {

            if (CurrentSettings == null)
            {
                try
                {
                    CurrentSettings = new Settings();

                    Helpers help = new Helpers();

                    //StreamResourceInfo xml = Application.GetResourceStream(new Uri(SettingsPath, UriKind.RelativeOrAbsolute));
                    //XDocument settingsDoc = XDocument.Load(xml.Stream);

                    XDocument settingsDoc = XDocument.Load(SettingsPath);

                    CurrentSettings.ClubName = settingsDoc.Root.Element("clubName").Value;
                    CurrentSettings.ClubDescription = settingsDoc.Root.Element("description").Value;
                    CurrentSettings.ClubPictureURI = settingsDoc.Root.Element("pictureURI").Value;
                    CurrentSettings.ContactPhoneNumber = settingsDoc.Root.Element("contactPhoneNumber").Value;
                    CurrentSettings.ContactEmail = settingsDoc.Root.Element("contactEmail").Value;
                    CurrentSettings.EventSourceURI = settingsDoc.Root.Element("eventsSourceURI").Value;
                    CurrentSettings.FacebookURI = settingsDoc.Root.Element("facebookURI").Value;
                    CurrentSettings.PresentationSourceURI = settingsDoc.Root.Element("presentationSourceURI").Value;
                    CurrentSettings.MembersSourceURI = settingsDoc.Root.Element("membersSourceURI").Value;
                    CurrentSettings.NewsURI = settingsDoc.Root.Element("newsSourceURI").Value;
                    CurrentSettings.OfficeMembersSourceURI = settingsDoc.Root.Element("officeMembersSourceURI").Value;
                    CurrentSettings.ProjectsSourceURI = settingsDoc.Root.Element("projectsSourceURI").Value;
                    CurrentSettings.TwitterURI = settingsDoc.Root.Element("twitterURI").Value;

                    CurrentSettings.PresentationSourceURI = await LocalStorage.Instance.NewURI(presentationNameFile, CurrentSettings.PresentationSourceURI);
                    CurrentSettings.EventSourceURI = await LocalStorage.Instance.NewURI(eventsNameFile, CurrentSettings.EventSourceURI);
                    CurrentSettings.MembersSourceURI = await LocalStorage.Instance.NewURI(membersNameFile, CurrentSettings.MembersSourceURI);
                    CurrentSettings.OfficeMembersSourceURI = await LocalStorage.Instance.NewURI(officeMembersNameFile, CurrentSettings.OfficeMembersSourceURI);
                    CurrentSettings.ProjectsSourceURI = await LocalStorage.Instance.NewURI(projectFile, CurrentSettings.ProjectsSourceURI);
                    CurrentSettings.NewsURI = await LocalStorage.Instance.NewURI(newsNameFile, CurrentSettings.NewsURI);

                    return CurrentSettings;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in LoadSettings [MonAssoce.DataLayer.ProcessData]");
                    Debug.WriteLine(e.Message);
                }
            }
            return CurrentSettings;
        }

        public async static Task<Settings> LoadNewsImages()
        {
            try
            {
                XDocument settingsDoc = XDocument.Load(SettingsPath);
                CurrentSettings.NewsURI = settingsDoc.Root.Element("newsSourceURI").Value;
                CurrentSettings.NewsURI = await LocalStorage.Instance.NewURI(newsNameFile, CurrentSettings.NewsURI);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in LoadNewsImages [MonAssoce.DataLayer.ProcessData]");
                Debug.WriteLine(e.Message);
            }
            return CurrentSettings;
        }

        public async static Task<Settings> LoadEventsImages()
        {
            try
            {
                XDocument settingsDoc = XDocument.Load(SettingsPath);
                CurrentSettings.EventSourceURI = settingsDoc.Root.Element("eventsSourceURI").Value;
                CurrentSettings.EventSourceURI = await LocalStorage.Instance.NewURI(eventsNameFile, CurrentSettings.EventSourceURI);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in LoadEventsImages [MonAssoce.DataLayer.ProcessData]");
                Debug.WriteLine(e.Message);
            }
            return CurrentSettings;
        }

        public async static Task<Settings> LoadProjectsImages()
        {
            try
            {
                XDocument settingsDoc = XDocument.Load(SettingsPath);
                CurrentSettings.ProjectsSourceURI = settingsDoc.Root.Element("projectsSourceURI").Value;
                CurrentSettings.ProjectsSourceURI = await LocalStorage.Instance.NewURI(projectFile, CurrentSettings.ProjectsSourceURI);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in LoadProjectsImages [MonAssoce.DataLayer.ProcessData]");
                Debug.WriteLine(e.Message);
            }
            return CurrentSettings;
        }

        public async static Task<Settings> LoadMembersImages()
        {
            try
            {
                XDocument settingsDoc = XDocument.Load(SettingsPath);
                CurrentSettings.MembersSourceURI = settingsDoc.Root.Element("membersSourceURI").Value;
                CurrentSettings.MembersSourceURI = await LocalStorage.Instance.NewURI(membersNameFile, CurrentSettings.MembersSourceURI);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in LoadMembersImages [MonAssoce.DataLayer.ProcessData]");
                Debug.WriteLine(e.Message);
            }
            return CurrentSettings;
        }

        public static void Reset()
        {
            CurrentSettings = null;
            AllPresentation = null;
            AllNews = null;
            AllEvents = null;
            UpcomingEvents = null;
            AllMembers = null;
            AllProjects = null;
            AllOfficeMembers = null;
        }

        /// <summary>
        /// Get Presentation from XML Document
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Presentation GetPresentation()
        {
            if (AllPresentation == null)
            {
                try
                {
                    AllPresentation = new Presentation();
                    XDocument doc = XDocument.Load(CurrentSettings.PresentationSourceURI);
                    AllPresentation.BigPictureURI = doc.Root.Element("bigPictureURI").Value;
                    AllPresentation.Description = doc.Root.Element("description").Value;
                    foreach (var elem in doc.Root.Element("images").Descendants("image"))
                    {
                        AllPresentation.PicturesURI.Add(elem.Value);
                    }
                    return AllPresentation;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetPresentation [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }
            return AllPresentation;
        }

        /// <summary>
        /// Get Events from XML Document
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Event> GetEvents()
        {
            if (AllEvents == null)
            {
                try
                {
                    AllEvents = new List<Event>();
                    XDocument doc = XDocument.Load(CurrentSettings.EventSourceURI);
                    int i = 0;
                    foreach (var item in doc.Descendants("event"))
                    {
                        bool time = (item.Element("date").Value.ToString().Contains(" ")) ? true : false;
                        AllEvents.Add(new Event()
                        {
                            ID = i,
                            Title = item.Element("title").Value.ToString(),
                            Address = item.Element("address").Value.ToString(),
                            ContactEmail = item.Element("contactEmail").Value.ToString(),
                            ContactName = item.Element("contactName").Value.ToString(),
                            Date = DateTime.Parse(item.Element("date").Value.ToString()),
                            Schedule = time,
                            Description = item.Element("description").Value.ToString(),
                            PhoneNumber = item.Element("phoneNumber").Value.ToString(),
                            PictureURI = item.Element("pictureURI").Value.ToString(),
                            BigPictureURI = item.Element("bigPictureURI").Value.ToString(),
                            RemotePictureURI = item.Element("remotePictureURI").Value.ToString(),
                            WebSiteURL = item.Element("webSiteURL").Value.ToString(),
                        });
                        i++;
                    }
                    AllEvents.Sort((a, b) => a.Date.CompareTo(b.Date));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetEvents [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }
            return AllEvents;
        }

        /// <summary>
        /// Get Events from XML Document
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Event> GetUpcomingEvents()
        {
            if (UpcomingEvents == null)
            {
                try
                {
                    UpcomingEvents = new List<Event>();
                    XDocument doc = XDocument.Load(CurrentSettings.EventSourceURI);
                    int i = 0;
                    foreach (var item in doc.Descendants("event"))
                    {
                        bool time = (item.Element("date").Value.ToString().Contains(" ")) ? true : false;
                        UpcomingEvents.Add(new Event()
                        {
                            ID = i,
                            Title = item.Element("title").Value.ToString(),
                            Address = item.Element("address").Value.ToString(),
                            ContactEmail = item.Element("contactEmail").Value.ToString(),
                            ContactName = item.Element("contactName").Value.ToString(),
                            Date = DateTime.Parse(item.Element("date").Value.ToString()),
                            Schedule = time,
                            Description = item.Element("description").Value.ToString(),
                            PhoneNumber = item.Element("phoneNumber").Value.ToString(),
                            PictureURI = item.Element("pictureURI").Value.ToString(),
                            BigPictureURI = item.Element("bigPictureURI").Value.ToString(),
                            RemotePictureURI = item.Element("remotePictureURI").Value.ToString(),
                            WebSiteURL = item.Element("webSiteURL").Value.ToString(),
                        });
                        i++;
                    }
                    UpcomingEvents.Sort((a, b) => a.Date.CompareTo(b.Date));
                    return UpcomingEvents;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetUpcomingEvents [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }

            return UpcomingEvents;
        }

        /// <summary>
        /// Get Club Members from XML Document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Member> GetMembers()
        {
            if (AllMembers == null)
            {
                try
                {
                    AllMembers = new List<Member>();
                    XDocument doc = XDocument.Load(CurrentSettings.MembersSourceURI);
                    AllMembers = (from item in doc.Descendants("member")
                                  let LastName = item.Element("lastName").Value
                                  orderby LastName ascending
                                  select new Member
                                  {
                                      Email = item.Element("email").Value,
                                      FirstName = item.Element("firstName").Value,
                                      LastName = item.Element("lastName").Value,
                                      PhoneNumber = item.Element("phoneNumber").Value,
                                      PictureURI = item.Element("pictureURI").Value,
                                      MemberSince = DateTime.Parse(item.Element("memberSince").Value)
                                  }).ToList<Member>();

                    return AllMembers;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetMembers [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }

            return AllMembers;
        }

        /// <summary>
        /// Get Office Members from XML Document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<OfficeMember> GetOfficeMembers()
        {
            if (AllOfficeMembers == null)
            {
                try
                {
                    AllOfficeMembers = new List<OfficeMember>();
                    XDocument doc = XDocument.Load(CurrentSettings.OfficeMembersSourceURI);
                    AllOfficeMembers = (from item in doc.Descendants("member")
                                        select new OfficeMember
                                        {
                                            Title = item.Element("title").Value,
                                            Email = item.Element("email").Value,
                                            FirstName = item.Element("firstName").Value,
                                            LastName = item.Element("lastName").Value,
                                            PhoneNumber = item.Element("phoneNumber").Value,
                                            PictureURI = item.Element("pictureURI").Value,
                                            MemberSince = DateTime.Parse(item.Element("memberSince").Value)
                                        }).ToList<OfficeMember>();

                    return AllOfficeMembers;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetOfficeMembers [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }

            return AllOfficeMembers;
        }

        /// <summary>
        /// Get News from XML Document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<News> GetNews()
        {
            if (AllNews == null)
            {
                try
                {
                    int i = 0;
                    AllNews = new List<News>();
                    XDocument doc = XDocument.Load(CurrentSettings.NewsURI);
                    foreach (var item in doc.Descendants("item"))
                    {
                        bool time = (item.Element("date").Value.ToString().Contains(" ")) ? true : false;
                        AllNews.Add(new News()
                        {
                            ID = i,
                            Title = item.Element("title").Value,
                            ImageURL = item.Element("pictureURI").Value,
                            BigPictureURI = item.Element("bigPictureURI").Value.ToString(),
                            RemotePictureURI = item.Element("remotePictureURI").Value.ToString(),
                            Link = item.Element("link").Value,
                            Schedule = time,
                            PubDate = DateTime.Parse(item.Element("date").Value),
                            Content = item.Element("content").Value
                        });
                        i++;
                    }
                    return AllNews;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetNews [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }

            return AllNews;
        }

        /// <summary>
        /// Get Projects from XML Document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Project> GetProjects()
        {
            if (AllProjects == null)
            {
                try
                {
                    int i = 0;
                    AllProjects = new List<Project>();
                    XDocument doc = XDocument.Load(CurrentSettings.ProjectsSourceURI);
                    foreach (var item in doc.Descendants("project"))
                    {
                        bool time = (item.Element("deadline").Value.ToString().Contains(" ")) ? true : false;
                        AllProjects.Add(new Project()
                        {
                            ID = i,
                            Title = item.Element("title").Value.ToString(),
                            ContactPhone = item.Element("contactPhone").Value.ToString(),
                            ContactEmail = item.Element("contactEmail").Value.ToString(),
                            ContactName = item.Element("contactName").Value.ToString(),
                            Deadline = DateTime.Parse(item.Element("deadline").Value.ToString()),
                            Schedule = time,
                            Description = item.Element("description").Value.ToString(),
                            SubTitle = item.Element("subTitle").Value.ToString(),
                            PictureURI = item.Element("pictureURI").Value.ToString(),
                            BigPictureURI = item.Element("bigPictureURI").Value.ToString(),
                            RemotePictureURI = item.Element("remotePictureURI").Value.ToString()
                        });
                        i++;
                    }

                    return AllProjects;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in GetProjects [MonAssoce.DataLayer.ProcessData]\n");
                    Debug.WriteLine(e.Message);
                }
            }

            return AllProjects;
        }
    }
}
