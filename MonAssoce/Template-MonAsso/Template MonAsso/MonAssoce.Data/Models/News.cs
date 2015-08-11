using System;

namespace MonAssoce.Data.Models
{
    public class News
    {
        public int ID { get; set; }

        public string ImageURL { get; set; }

        public string Title { get; set; }

        public DateTime PubDate { get; set; }

        public string Content { get; set; }

        public string BigPictureURI { get; set; }

        public string RemotePictureURI { get; set; }

        public string Link { get; set; }

        public bool Schedule { get; set; }
    }
}
