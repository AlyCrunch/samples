using System;

namespace MonAssoce.Data.Models
{
    public class Event
    {

        public int ID { get; set; }

        public string BigPictureURI { get; set; }

        public string RemotePictureURI { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string PictureURI { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string WebSiteURL { get; set; }

        public string Address { get; set; }

        public bool Schedule { get; set; }

        //public string LocationStr { get; set; }

        //public GeoCoordinate Location { get; set; }

    }
}
