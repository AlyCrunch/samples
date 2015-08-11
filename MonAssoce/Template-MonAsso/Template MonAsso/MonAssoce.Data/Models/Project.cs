using System;

namespace MonAssoce.Data.Models
{
    public class Project
    {
        public int ID { get; set; }

        public string BigPictureURI { get; set; }

        public string RemotePictureURI { get; set; }

        public string Title { get; set; }

        public DateTime Deadline { get; set; }

        public string SubTitle { get; set; }

        public string PictureURI { get; set; }

        public string Description { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public bool Schedule { get; set; }

        public Project()
        {
            Deadline = new DateTime();
        }
    }
}
