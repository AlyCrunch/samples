using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Data;
using MonAssoce.Data.Models;
using MonAssoce.ViewModels.Items;
using Windows.ApplicationModel.Resources;

namespace MonAssoce.ViewModels
{
    public class MembersViewModel : MonAssoce.Common.BindableBase
    {
        private const string OFFICE_RESOURCE_KEY = "OfficeMembers";
        private const string MEMBERS_RESOURCE_KEY = "MembersMainPage";
        private ObservableCollection<MembersHubViewModel> _hubs;
        public ObservableCollection<MembersHubViewModel> Hubs
        {
            get { return this._hubs; }
            set { this.SetProperty(ref this._hubs, value); }
        }

        private MemberItemViewModel _itemToScrollTo;
        public MemberItemViewModel ItemToScrollTo
        {
            get { return this._itemToScrollTo; }
            set { this.SetProperty(ref this._itemToScrollTo, value); }
        }

        public MembersViewModel()
        {
        }

        public async void LoadData()
        {
            this.Hubs = new ObservableCollection<MembersHubViewModel>();
            //this.LoadSampleData();
            await this.LoadProcessedData();
        }

        public async Task LoadProcessedData()
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            await ProcessData.LoadSettings();

            MembersHubViewModel OfficeMembers = new MembersHubViewModel();
            OfficeMembers.HubName = resourceLoader.GetString(OFFICE_RESOURCE_KEY);
            List<OfficeMember> OfficeMembersModelsList = ProcessData.GetOfficeMembers();

            foreach (OfficeMember officeMemberModel in OfficeMembersModelsList)
            {
                MemberItemViewModel officeMemberItem = new MemberItemViewModel();
                officeMemberItem.ModelToItem(officeMemberModel);
                OfficeMembers.Add(officeMemberItem);
            }
            this.Hubs.Add(OfficeMembers);

            MembersHubViewModel Members = new MembersHubViewModel();
            Members.HubName = resourceLoader.GetString(MEMBERS_RESOURCE_KEY);
            List<Member> MembersModelsList = ProcessData.GetMembers();

            foreach (Member MemberModel in MembersModelsList)
            {
                MemberItemViewModel MemberItem = new MemberItemViewModel();
                MemberItem.ModelToItem(MemberModel);
                Members.Add(MemberItem);
            }
            this.Hubs.Add(Members);
        }

        public void LoadSampleData()
        {
            string[] tempHubNames = { "Office", "Members" };
            for (int i = 0; i < 2; i++)
            {
                MembersHubViewModel tempHub = new MembersHubViewModel();
                tempHub.HubName = tempHubNames[i];
                for (int j = 0; j < 4; j++)
                {
                    MemberItemViewModel tempItem = new MemberItemViewModel();
                    tempItem.Image = "/Content/Images/Members/default.png";
                    tempItem.Title = "John Doe " + j;
                    tempItem.Subtitle = " Joined on 22/12/2012";
                    tempHub.Add(tempItem);
                }
                this.Hubs.Add(tempHub);
            }
        }

        public async void RefreshData()
        {
            this.Hubs = new ObservableCollection<MembersHubViewModel>();
            ProcessData.AllMembers = null;
            await ProcessData.LoadMembersImages();
        }

        public void GetItemToScrollTo(MainItemViewModel item)
        {
            foreach (MembersHubViewModel membersHub in Hubs)
            {
                foreach (MemberItemViewModel member in membersHub)
                {
                    if (item.Title.Equals(member.Title) && item.Subtitle.Equals(member.Subtitle))
                    {
                        this.ItemToScrollTo = member;
                    }
                }
            }
        }
    }
}
