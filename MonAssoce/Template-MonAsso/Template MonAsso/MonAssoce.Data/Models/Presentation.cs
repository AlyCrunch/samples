using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonAssoce.Data.Models
{
    public class Presentation
    {
        public string BigPictureURI { get; set; }

        public string Description { get; set; }

        public List<string> PicturesURI { get; set; }

        public Presentation()
        {
            PicturesURI = new List<string>();
        }
    }
}
