using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonAssoce.ViewModels.Items
{
    public class MembersHubViewModel : ObservableCollection<MemberItemViewModel>
    {
        private int _id;
        private string _hubName;

        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string HubName
        {
            get { return this._hubName; }
            set { this._hubName = value; }
        }

        public string HubTitle
        {
            get
            {
                return this.HubName + " (" + this.Count + ")";
            }
        }

        public new IEnumerator<MemberItemViewModel> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<MemberItemViewModel>)base.GetEnumerator();
        }
    }
}
