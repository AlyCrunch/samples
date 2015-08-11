
namespace MonAssoce.Data.Models
{
    public class Settings
    {

        public string ClubName { get; set; }
        public string ClubDescription { get; set; }
        public string ClubPictureURI { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }

        /// <summary>
        /// News may be loaded locally or remotly, they are always RSS 2.0 feeds.
        /// </summary>
        public string NewsURI { get; set; }

        /// <summary>
        /// Events may be stored in the App project or loaded from remote.
        /// </summary>
        public string EventSourceURI { get; set; }

        /// <summary>
        /// Presentation may be stored in the App project or loaded from remote.
        /// </summary>
        public string PresentationSourceURI { get; set; }

        /// <summary>
        /// OfficeMembers may be stored locally or loaded from remote.
        /// </summary>
        public string OfficeMembersSourceURI { get; set; }

        /// <summary>
        /// Club Members may be stored locally or loaded from remote.
        /// </summary>
        public string MembersSourceURI { get; set; }

        /// <summary>
        /// Club Projects may be stored locally or loaded from remote.
        /// </summary>
        public string ProjectsSourceURI { get; set; }

        // Facebook and Twitter pages
        public string FacebookURI { get; set; }
        public string TwitterURI { get; set; }

    }
}
