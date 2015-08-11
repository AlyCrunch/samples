using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonAssoce.Libs.Helpers;
using MonAssoce.ViewModels.Items;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace MonAssoce.Views
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class EventsDetails : MonAssoce.Common.LayoutAwarePage
    {
        public EventsDetails()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            DataContext = App.EventsDetailsViewModel;
            await App.EventsDetailsViewModel.LoadData();

            if (navigationParameter.GetType() == typeof(int))
            {
                foreach (EventItemViewModel events in App.EventsDetailsViewModel.Items)
                {
                    if (events.ID == (int)navigationParameter)
                    {
                        this.flipView.SelectedItem = events;
                    }
                }
            }
            else
            {
                this.flipView.SelectedItem = navigationParameter;
            }
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }
            DataTransferManager.GetForCurrentView().DataRequested += ShareCurrentPage_DataRequested;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = this.flipView.SelectedItem;
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as FlipView).SelectedItem != null)
            {
                this.pageTitle.Text = ((sender as FlipView).SelectedItem as EventItemViewModel).Title;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= ShareCurrentPage_DataRequested;
        }

        private void ShareCurrentPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ResourceLoader resources = new ResourceLoader();
            HTMLBuilder builder = new HTMLBuilder();
            EventItemViewModel item = this.flipView.SelectedItem as EventItemViewModel;
            DataPackage data = args.Request.Data;

            data.Properties.Title = resources.GetString("AssociationName") + " " + item.Title;
            data.Properties.Description = item.Subtitle;

            List<string> content = new List<string>();
            content.Add(item.Title);
            content.Add(item.Subtitle);
            content.Add(item.RemotePictureURI);
            content.Add(item.Content);
            content.Add(item.ContactName);
            content.Add(item.ContactEmail);
            content.Add(item.PhoneNumber);
            content.Add(item.Address);
            content.Add(item.WebSiteURL);

            // Sharing text
            data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(builder.ContentToHTML(content)));
        }
    }
}
