using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Libs.Helpers;

namespace MonAssoce.ViewModels.Items
{
    public class EventItemViewModel : MonAssoce.Common.BindableBase
    {
        private const string DEFAULT_IMAGE_PATH = "/Content/Images/Events/default.png";
        private DateToStringConverter dateToStringConverter;

        private int _ID;
        private string _image = DEFAULT_IMAGE_PATH;
        private string _title;
        private string _subtitle;
        private string _shortSubtitle;
        private string _content;

        private string _bigPictureURI;
        private string _remotePictureURI;
        private string _contactName;
        private string _contactEmail;
        private string _phoneNumber;
        private string _webSiteURL;
        private string _address;
        private bool _isEventPicVisible;
        
        public void ModelToItem(MonAssoce.Data.Models.Event eventModel)
        {
            dateToStringConverter = new DateToStringConverter();
            this.ID = eventModel.ID;
            if (eventModel.PictureURI.Equals(string.Empty))
            {
                this.Image = DEFAULT_IMAGE_PATH;
            }
            else
            {
                this.Image = eventModel.PictureURI;
            }
            this.Title = eventModel.Title;
            this.Subtitle = dateToStringConverter.ConvertDateToString(eventModel.Date, eventModel.Schedule, true);
            this.ShortSubtitle = dateToStringConverter.ConvertDateToString(eventModel.Date, eventModel.Schedule, false);
            this.Content = eventModel.Description;
            if (eventModel.BigPictureURI != "")
            {
                this.BigPictureURI = eventModel.BigPictureURI;
            }
            else
            {
                this.BigPictureURI = this.Image;
            }
            this.RemotePictureURI = eventModel.RemotePictureURI;
            this.ContactName = eventModel.ContactName;
            this.ContactEmail = eventModel.ContactEmail;
            this.PhoneNumber = eventModel.PhoneNumber;
            this.WebSiteURL = eventModel.WebSiteURL;
            this.Address = eventModel.Address;
        }

        public int ID
        {
            get { return _ID; }
            set { this.SetProperty(ref this._ID, value); }
        }

        public string Image
        {
            get { return this._image; }
            set { this.SetProperty(ref this._image, value); }
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

        public bool IsEventPicVisible
        {
            get { return _isEventPicVisible; }
            set
            {
                if (_isEventPicVisible == value) return;
                _isEventPicVisible = value;
                ////NotifyPropertyChanged("IsEventPicVisible");
            }
        }


        public string ContactName
        {
            get { return _contactName; }
            set
            {
                if (_contactName == value) return;
                _contactName = value;
                //_contactName = Localization.Contact + value;
                //NotifyPropertyChanged("ContactName");
            }
        }

        public string ContactEmail 
        {
            get { return _contactEmail; }
            set
            {
                if (_contactEmail == value) return;
                _contactEmail = value;
                //NotifyPropertyChanged("ContactEmail");
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber == value) return;
                _phoneNumber = value;
                //NotifyPropertyChanged("PhoneNumber");
            }
        }

        public string WebSiteURL 
        {
            get { return _webSiteURL; }
            set
            {
                if (_webSiteURL == value) return;
                _webSiteURL = value;
                //NotifyPropertyChanged("WebSiteURL");
            }
        }

        public string Address 
        {
            get { return _address; }
            set
            {
                if (_address == value) return;
                _address = value;
                //NotifyPropertyChanged("Address");
            }
        }



        /*public GeoCoordinate Location 
        {
            get { return _location; }
            set
            {
                if (_location == value) return;
                _location = value;
                //NotifyPropertyChanged("Location");
            }
        }*/
    }
}
