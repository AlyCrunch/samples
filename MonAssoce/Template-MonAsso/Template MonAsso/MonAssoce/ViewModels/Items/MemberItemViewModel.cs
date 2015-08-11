using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Libs.Helpers;
using Windows.ApplicationModel.Resources;

namespace MonAssoce.ViewModels.Items
{
    public class MemberItemViewModel : MonAssoce.Common.BindableBase
    {
        private const string DEFAULT_IMAGE_PATH = "/Content/Images/Members/default.png";
        private const string MEMBER_SINCE_KEY = "MemberSince";
        private DateToStringConverter dateToStringConverter;
        private ResourceLoader resourceLoader;

        private string _image;
        private string _title;
        private string _subtitle;

        private string _email;
        private string _phoneNumber;
        private string _websiteURL;

        public void ModelToItem(MonAssoce.Data.Models.Member member)
        {
            dateToStringConverter = new DateToStringConverter();
            resourceLoader = new ResourceLoader();
            if (member.PictureURI.Equals(string.Empty))
            {
                this.Image = DEFAULT_IMAGE_PATH;
            }
            else
            {
                this.Image = member.PictureURI;
            }
            this.Title = member.FirstName + " " + member.LastName;
            this.Subtitle = resourceLoader.GetString(MEMBER_SINCE_KEY) + " " + dateToStringConverter.ConvertDateToString(member.MemberSince, false, false);
            this.Email = member.Email;
            this.PhoneNumber = member.PhoneNumber;
            this.WebsiteURL = member.WebSiteURL;
        }

        public void ModelToItem(MonAssoce.Data.Models.OfficeMember officeMember)
        {
            if (officeMember.PictureURI.Equals(string.Empty))
            {
                this.Image = DEFAULT_IMAGE_PATH;
            }
            else
            {
                this.Image = officeMember.PictureURI;
            }
            this.Title = officeMember.FirstName + " " + officeMember.LastName;
            this.Subtitle = officeMember.Title;

            this.Email = officeMember.Email;
            this.PhoneNumber = officeMember.PhoneNumber;
            this.WebsiteURL = officeMember.WebSiteURL;
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

        public string Email
        {
            get { return this._email; }
            set { this.SetProperty(ref this._email, value); }
        }

        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { this.SetProperty(ref this._phoneNumber, value); }
        }

        public string WebsiteURL
        {
            get { return this._websiteURL; }
            set { this.SetProperty(ref this._websiteURL, value); }
        }
    }
}
