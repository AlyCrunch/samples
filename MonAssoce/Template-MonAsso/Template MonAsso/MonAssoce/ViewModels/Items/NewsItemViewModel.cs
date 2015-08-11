using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Libs.Helpers;
namespace MonAssoce.ViewModels.Items
{
    public class NewsItemViewModel : MonAssoce.Common.BindableBase
    {

        private const string DEFAULT_IMAGE_PATH = "/Content/Images/News/default.png";
        private DateToStringConverter dateToStringConverter;

        private int _ID;
        private string _image;
        private string _title;
        private string _subtitle;
        private string _shortSubtitle;
        private string _content;

        private string _bigPictureURI;
        private string _remotePictureURI;
        private string _link;

        public void ModelToItem(MonAssoce.Data.Models.News newsModel)
        {
            dateToStringConverter = new DateToStringConverter();
            this.ID = newsModel.ID;
            if (newsModel.ImageURL.Equals(string.Empty))
            {
                this.Image = DEFAULT_IMAGE_PATH;
            }
            else
            {
            this.Image = newsModel.ImageURL;
            }
            this.BigPictureURI = newsModel.BigPictureURI;
            this.RemotePictureURI = newsModel.RemotePictureURI;
            this.Title = newsModel.Title;
            this.Subtitle = dateToStringConverter.ConvertDateToString(newsModel.PubDate, newsModel.Schedule, true);
            this.ShortSubtitle = dateToStringConverter.ConvertDateToString(newsModel.PubDate, newsModel.Schedule, false);
            this.Content = newsModel.Content;
        }

        public int ID
        {
            get { return this._ID; }
            set { this.SetProperty(ref this._ID, value); }
        }

        public string Image
        {
            get { return this._image; }
            set { this.SetProperty(ref this._image, value); }
        }

        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        public string ShortSubtitle
        {
            get { return this._shortSubtitle; }
            set { this.SetProperty(ref this._shortSubtitle, value); }
        }

        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        public string BigPictureURI
        {
            get { return this._bigPictureURI; }
            set { this.SetProperty(ref this._bigPictureURI, value); }
        }

        public string RemotePictureURI
        {
            get { return this._remotePictureURI; }
            set { this.SetProperty(ref this._remotePictureURI, value); }
        }

        public string Link
        {
            get { return this._link; }
            set { this.SetProperty(ref this._link, value); }
        }
    }
}
