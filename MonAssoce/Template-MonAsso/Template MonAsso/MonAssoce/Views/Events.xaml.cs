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
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using MonAssoce.Data.Libs;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace MonAssoce.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class Events : MonAssoce.Common.LayoutAwarePage
    {
        private DispatcherTimer _timer;

        public Events()
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
            DataContext = App.EventsViewModel;
            await App.EventsViewModel.LoadData();
        }

        private void ShowEvent_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(EventsDetails), (e.ClickedItem as EventItemViewModel));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoadingBar.Visibility = Visibility.Visible;
            App.EventsViewModel.RefreshData();
            this.CheckDataLoaded();
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Members));
        }

        private void CheckDataLoaded()
        {
            this._timer = new DispatcherTimer();
            this._timer.Tick += timer_Tick;
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Start();
        }

        private async void timer_Tick(object sender, object e)
        {
            if (LocalStorage.Instance.activeDownloads.Count == 0)
            {
                this._timer.Tick -= timer_Tick;
                this._timer.Stop();
                await App.EventsViewModel.LoadData();
                this.LoadingBar.Visibility = Visibility.Collapsed;
            }
        }


    }
}
