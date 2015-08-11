using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonAssoce.Data;
using MonAssoce.ViewModels.Items;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MonAssoce.Data.Libs;
using Windows.UI.Popups;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace MonAssoce.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class Members : MonAssoce.Common.LayoutAwarePage
    {
        private DispatcherTimer _timer;

        public Members()
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
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            DataContext = App.MembersViewModel;
            App.MembersViewModel.LoadData();
            if (navigationParameter != null)
            {
                App.MembersViewModel.GetItemToScrollTo((navigationParameter as MainItemViewModel));
            }
        }

        private void itemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MembersAppBar.IsOpen = this.MembersAppBar.IsSticky = ((GridView)sender).SelectedItems.Count > 0;
            if (((GridView)sender).SelectedItems.Count > 0)
            {
                this.SendEmailAppBarButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.SendEmailAppBarButton.Visibility = Visibility.Collapsed;
                this.BottomAppBar.IsOpen = false;
            }
        }

        private async void SendEmailAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            string MemberEmails = "";
            foreach (MemberItemViewModel member in itemGridView.SelectedItems)
            {
                if (i == 0)
                {
                    MemberEmails += member.Email + "?cc=";
                }
                else
                {
                    MemberEmails += member.Email + "; ";
                }
                i++;
            }
            //S'il y a eu au moins un membre selectionné, on supprime la fin de la chaine
            if (i > 0)
            {
                MemberEmails = MemberEmails.Substring(0, MemberEmails.Length - 2);
                var uri = new Uri("mailto:" + MemberEmails);
                bool success = await Windows.System.Launcher.LaunchUriAsync(uri);

            }
            else
                NotifyUser("Pas de membres sélectionnés");

            
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoadingBar.Visibility = Visibility.Visible;
            App.MembersViewModel.RefreshData();
            this.CheckDataLoaded();
        }

        private void CheckDataLoaded()
        {
            this._timer = new DispatcherTimer();
            this._timer.Tick += timer_Tick;
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Start();
        }

        public async void NotifyUser(string strMessage)
        {
            var md = new MessageDialog(strMessage);
            await md.ShowAsync();
        }

        private void timer_Tick(object sender, object e)
        {
            if (LocalStorage.Instance.activeDownloads.Count == 0)
            {
                this._timer.Tick -= timer_Tick;
                this._timer.Stop();
                App.MembersViewModel.LoadData();
                this.LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private void FullScreenLandscape_Completed(object sender, object e)
        {
            if (App.MembersViewModel.ItemToScrollTo != null)
            {
                this.itemGridView.ScrollIntoView(App.MembersViewModel.ItemToScrollTo);
            }
        }
    }
}
