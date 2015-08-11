using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Libs.Helpers;

namespace MonAssoce.ViewModels.Items
{
    public class ProjectItemViewModel : MonAssoce.Common.BindableBase
    {
        private const string DEFAULT_IMAGE_PATH = "/Content/Images/Projects/default.png";
        private DateToStringConverter dateToStringConverter;

        private int _ID;
        private string _image;
        private string _title;
        private string _subtitle;
        private string _content;

        private string _bigPictureURI;
        private string _remotePictureURI;
        private string _deadline;
        private string _contactName;
        private string _contactEmail;
        private string _contactPhone;


        public void ModelToItem(MonAssoce.Data.Models.Project projectModel)
        {
            dateToStringConverter = new DateToStringConverter();
            this.ID = projectModel.ID;
            if (projectModel.PictureURI.Equals(string.Empty))
            {
                this.Image = DEFAULT_IMAGE_PATH;
            }
            else
            {
                this.Image = projectModel.PictureURI;
            }
            this.Title = projectModel.Title;
            this.Subtitle = projectModel.SubTitle;
            this.Content = projectModel.Description;
            this.BigPictureURI = projectModel.BigPictureURI;
            this.RemotePictureURI = projectModel.RemotePictureURI;
            this.Deadline = dateToStringConverter.ConvertDateToString(projectModel.Deadline, projectModel.Schedule, true);
            this.ContactName = projectModel.ContactName;
            this.ContactEmail = projectModel.ContactEmail;
            this.ContactPhone = projectModel.ContactPhone;
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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

        public string Deadline
        {
            get { return this._deadline; }
            set { this.SetProperty(ref this._deadline, value); }
        }

        public string ContactName
        {
            get { return this._contactName; }
            set { this.SetProperty(ref this._contactName, value); }
        }

        public string ContactEmail
        {
            get { return this._contactEmail; }
            set { this.SetProperty(ref this._contactEmail, value); }
        }

        public string ContactPhone
        {
            get { return this._contactPhone; }
            set { this.SetProperty(ref this._contactPhone, value); }
        }
    }
}
