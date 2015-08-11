using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace MonAssoce.ViewModels.Items
{
    public class MainItemViewModel : MonAssoce.Common.BindableBase
    {
        private const string EVENT_DEFAULT_IMAGE_PATH = "/Content/Images/Events/default.png";
        private const string NEWS_DEFAULT_IMAGE_PATH = "/Content/Images/News/default.png";
        private const string PROJECT_DEFAULT_IMAGE_PATH = "/Content/Images/Projects/default.png";

        private int _ID;

        private string _placeholder;
        private string _bigPhoto;
        private string _photo;
        private string _photo1;
        private string _photo2;
        private string _photo3;

        private string _title;
        private string _subtitle;
        private string _description;

        private Visibility _imageOnlyV;
        private Visibility _OtherV;
        private Visibility _descriptionV;

        private bool _isDescription;
        private string _labelImage;

        private bool _isProject;
        private bool _isNews;
        private bool _isEvent;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Placeholder
        {
            get 
            {
                if (this.IsNews)
                {
                    this._placeholder = NEWS_DEFAULT_IMAGE_PATH;
                }
                else if (this.IsEvent)
                {
                    this._placeholder = EVENT_DEFAULT_IMAGE_PATH;
                }
                else if (this.IsProject)
                {
                    this._placeholder = PROJECT_DEFAULT_IMAGE_PATH;
                }
                else
                {
                    this._placeholder = string.Empty;
                }
                return this._placeholder; 
            }
            set { this.SetProperty(ref this._placeholder, value); }
        }

        public string LabelImage
        {
            get { return this._labelImage; }
            set { this.SetProperty(ref this._labelImage, value); }
        }

        public string BigPhoto
        {
            get { return this._bigPhoto; }
            set { this.SetProperty(ref this._bigPhoto, value); }
        }

        public string Photo
        {
            get { return this._photo; }
            set { this.SetProperty(ref this._photo, value); }
        }

        public string Photo1
        {
            get { return this._photo1; }
            set { this.SetProperty(ref this._photo1, value); }
        }

        public string Photo2
        {
            get { return this._photo2; }
            set { this.SetProperty(ref this._photo2, value); }
        }

        public string Photo3
        {
            get { return this._photo3; }
            set { this.SetProperty(ref this._photo3, value); }
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

        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        public Visibility OtherVisibility
        {
            get { return this._OtherV; }
            set { this.SetProperty(ref this._OtherV, value); }
        }

        public Visibility ImageOnlyVisibility
        {
            get { return this._imageOnlyV; }
            set { this.SetProperty(ref this._imageOnlyV, value); }
        }

        public Visibility DescriptionVisibility
        {
            get { return this._descriptionV; }
            set { this.SetProperty(ref this._descriptionV, value); }
        }

        public bool IsDescription
        {
            get { return this._isDescription; }
            set { this.SetProperty(ref this._isDescription, value); }
        }

        public bool IsEvent
        {
            get { return this._isEvent; }
            set { this.SetProperty(ref this._isEvent, value); }
        }

        public bool IsNews
        {
            get { return this._isNews; }
            set { this.SetProperty(ref this._isNews, value); }
        }

        public bool IsProject
        {
            get { return this._isProject; }
            set { this.SetProperty(ref this._isProject, value); }
        }
    }
}
