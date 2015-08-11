using MonAssoce.Data.Libs;
using MonAssoce.ViewModels.Items;
using MonAssoce.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace MonAssoce.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : MonAssoce.Common.LayoutAwarePage
    {
        private bool _isSnapped;
        private bool _isLandscape;
        private bool _isLoaded;
        private DispatcherTimer _timer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();
            if (!App.MainPageViewModel.IsAboutPaneAdded)
            {
                this.AddAboutPane();
                App.MainPageViewModel.IsAboutPaneAdded = true;
            }
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
            DataContext = App.MainPageViewModel;
            if (!App.MainPageViewModel.IsDataLoaded)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    await this.RefreshData();
                    App.MainPageViewModel.LoadHubsFromOnlineData();
                    this.ArrangeElementHubs();
                }
                else
                {
                    await App.MainPageViewModel.LoadHubs();
                    if (App.MainPageViewModel.Hubs.Count != 0)
                    {
                        this.ArrangeElementHubs();
                    }
                    else
                    {
                        this.LoadingBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                this._isLoaded = true;
            }

            await LiveTilesHelper.GetLiveTiles();
        }

        private async Task RefreshData()
        {
            this._isLoaded = false;
            this.LoadingBar.Visibility = Visibility.Visible;
            await App.MainPageViewModel.LoadData();
            this.CheckDataLoaded();
        }

        private void CheckDataLoaded()
        {
            this._timer.Tick += timer_Tick;
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Start();
        }

        void timer_Tick(object sender, object e)
        {
            if (App.MainPageViewModel.IsDataLoaded)
            {
                this._timer.Tick -= timer_Tick;
                this._timer.Stop();
                this.ArrangeElementHubs();
                this._isLoaded = true;
                this.LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        public void ArrangeElementHubs()
        {
            int NbItems = 0;
            this.MemberButtonTitle.Content = App.MainPageViewModel.NbMembers;

            if (!this._isSnapped)
            {
                if (this._isLandscape)
                {
                    if (this.itemGridViewIn.ActualHeight < this.itemGridViewIn.ActualWidth)
                        NbItems = (int)(itemGridViewIn.ActualHeight - 10) / 80;
                    else
                        NbItems = (int)(itemGridViewIn.ActualWidth - 140) / 80;
                }
                else
                {
                    if (this.itemGridViewIn.ActualHeight < this.itemGridViewIn.ActualWidth)
                        NbItems = (int)(itemGridViewIn.ActualWidth - 10) / 80;
                    else
                        NbItems = (int)(itemGridViewIn.ActualHeight - 10) / 80;
                    if (NbItems % 2 == 0)
                        NbItems -= 1;
                }
            }
            else
            {
                NbItems = (int)(itemGridViewIn.ActualHeight - 10) / 80;
            }


            int descript = (NbItems - 6) * 3 / 2;
            int other = NbItems / 3 * 2;

            if (App.MainPageViewModel.Hubs[0].HubName.Equals(new ResourceLoader().GetString("Presentation")))
            {
                for (int i = App.MainPageViewModel.Hubs[0].Count - 1; i >= descript; i--)
                {
                    // Si la connection échoue, i est négatif
                    if (i > 0)
                        App.MainPageViewModel.Hubs[0].RemoveAt(i);
                    //else

                        //NotifyUser("La connection a échoué, assurez-vous d'être connecté à internet et cliquez sur rafraichir.");
                }
            }

            for (int i = 1; i <= App.MainPageViewModel.Hubs.Count - 1; i++)
            {
                for (int j = App.MainPageViewModel.Hubs[i].Count - 1; j >= other; j--)
                {
                    App.MainPageViewModel.Hubs[i].RemoveAt(j);
                }
            }

            if (this._isSnapped && App.MainPageViewModel.Hubs[0].HubName.Equals(new ResourceLoader().GetString("Presentation")))
            {
                App.MainPageViewModel.Hubs.RemoveAt(0);
            }
            this.itemsViewSource.Source = App.MainPageViewModel.Hubs;
            this.itemGridViewOut.ItemsSource = this.itemsViewSource.View.CollectionGroups;
        }

        private void Header_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader _res = new ResourceLoader();

            if ((e.OriginalSource as Button).Content.ToString().Contains(_res.GetString("NewsPageTitleMain")))
            {
                this.Frame.Navigate(typeof(News));
            }

            if ((e.OriginalSource as Button).Content.ToString().Contains(_res.GetString("EventsPageTitleMain")))
            {
                this.Frame.Navigate(typeof(Events));
            }

            if ((e.OriginalSource as Button).Content.ToString().Contains(_res.GetString("ProjectsPageTitleMain")))
            {
                this.Frame.Navigate(typeof(Projects));
            }
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainItemViewModel item = (e.ClickedItem as MainItemViewModel);
            if (item.IsEvent)
            {
                this.Frame.Navigate(typeof(EventsDetails), item.ID);
            }
            else if (item.IsNews)
            {
                this.Frame.Navigate(typeof(NewsDetails), item.ID);
            }
            else if (item.IsProject)
            {
                this.Frame.Navigate(typeof(ProjectsDetails), item.ID);
            }
        }

        private void MemberButtonTitle_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Members));
        }

        private async void Portrait_Completed(object sender, object e)
        {
            this._isLandscape = false;
            this._isSnapped = false;
            if (this._isLoaded)
            {
                await App.MainPageViewModel.LoadHubs();
                this.ArrangeElementHubs();
            }
        }

        private async void Filled_Completed(object sender, object e)
        {
            this._isLandscape = true;
            this._isSnapped = false;
            if (this._isLoaded)
            {
                await App.MainPageViewModel.LoadHubs();
                this.ArrangeElementHubs();
            }
        }

        private async void Landscape_Completed(object sender, object e)
        {
            this._isLandscape = true;
            this._isSnapped = false;
            if (this._isLoaded)
            {
                await App.MainPageViewModel.LoadHubs();
                this.ArrangeElementHubs();
            }
        }

        private async void Snapped_Completed(object sender, object e)
        {
            this._isLandscape = false;
            this._isSnapped = true;
            if (this._isLoaded)
            {
                await App.MainPageViewModel.LoadHubs();
                this.ArrangeElementHubs();
            }
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Members));
        }

        private async void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            XDocument settingsDoc = XDocument.Load("Data/Settings.xml");
            Uri uri = new Uri(settingsDoc.Root.Element("facebookURI").Value);
            bool success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void TwitterButton_Click(object sender, RoutedEventArgs e)
        {
            XDocument settingsDoc = XDocument.Load("Data/Settings.xml");
            if (settingsDoc.Root.Element("twitterURI").Value != "")
            {
                Uri uri = new Uri(settingsDoc.Root.Element("twitterURI").Value);
                bool success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await this.RefreshData();
        }

        private void AddAboutPane()
        {
            SettingsPane.GetForCurrentView().CommandsRequested += new TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs>(AddToSettingsPane);
        }

        private void AddToSettingsPane(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            ResourceLoader resources = new ResourceLoader();
            SettingsCommand AboutPageCommand = new SettingsCommand("AboutPage", resources.GetString("About"), new UICommandInvokedHandler(onSettingsCommand));
            args.Request.ApplicationCommands.Add(AboutPageCommand);
        }

        public void onSettingsCommand(IUICommand command)
        {
            SettingsCommand settingsCommand = (SettingsCommand)command;
            if (settingsCommand.Id.ToString() == "AboutPage")
            {
                AboutPane();
            }
        }

        public void AboutPane()
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            StackPanel s = new StackPanel();
            s.Orientation = Orientation.Vertical;
            HyperlinkButton aheadLink = new HyperlinkButton();
            HyperlinkButton policy = new HyperlinkButton();

            Image aheadLogo = new Image();
            aheadLogo.Source = new BitmapImage(new Uri("ms-appx:/Content/Images/logo_ahead.png", UriKind.RelativeOrAbsolute));
            aheadLogo.Width = 160;


            TextBlock t1 = new TextBlock() { TextWrapping = TextWrapping.Wrap };
            TextBlock t2 = new TextBlock() { TextWrapping = TextWrapping.Wrap };
            TextBlock t2_click = new TextBlock() { TextWrapping = TextWrapping.Wrap };
            TextBlock t3 = new TextBlock() { TextWrapping = TextWrapping.Wrap };


            t1.Text = resourceLoader.GetString("TitleMyThings");
            t1.Margin = new Thickness(4);
            t1.FontSize = 16;
            t1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            t1.Foreground = new SolidColorBrush(Colors.Black);

            StackPanel sText = new StackPanel();
            sText.Orientation = Orientation.Horizontal;
            sText.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            sText.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;



            t2.Text = "© 2012";
            t2.FontSize = 16;
            t2.Foreground = new SolidColorBrush(Colors.Black);
            t2.TextAlignment = TextAlignment.Right;
            t2.Foreground = new SolidColorBrush(Colors.Black);
            t2.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

            aheadLink.Foreground = new SolidColorBrush(Colors.Black);
            aheadLink.Click += new RoutedEventHandler(GoToAheadSolutions);
            aheadLink.BorderBrush = new SolidColorBrush(Colors.Transparent);
            aheadLink.Content = "Ahead Solutions";
            aheadLink.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            aheadLink.Margin = new Thickness(0, 0, 0, 3);



            StackPanel sText2 = new StackPanel();
            sText2.Orientation = Orientation.Horizontal;
            sText2.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            sText2.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            policy.Foreground = new SolidColorBrush(Colors.Black);
            policy.Click += new RoutedEventHandler(GoToPolicy);
            policy.BorderBrush = new SolidColorBrush(Colors.Transparent);
            policy.Content = "Politique de confidentialité";
            policy.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            policy.Margin = new Thickness(0, 0, 0, 3);


            sText.Children.Add(t2);
            sText.Children.Add(aheadLink);

            sText2.Children.Add(policy);

            t3.Text = resourceLoader.GetString("TextDescriMyThings");
            t3.Margin = new Thickness(10);
            t3.FontSize = 16;
            t3.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            t3.Foreground = new SolidColorBrush(Colors.Black);

            s.Children.Add(aheadLogo);
            s.Children.Add(t1);
            s.Children.Add(sText);
            s.Children.Add(sText2);
            s.Children.Add(t3);


            FlyoutCharm f = new FlyoutCharm(
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(Colors.White),
                "About",
                FlyoutDimension.Narrow,
                s);
            f.Show();
        }

        private async void GoToAheadSolutions(object sender, RoutedEventArgs e)
        {
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.ahead-solutions.com"));
        }

        private async void GoToPolicy(object sender, RoutedEventArgs e)
        {
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("http://ma.ms.giz.fr/?name=" + Uri.EscapeDataString(Application.Current.Resources["AppName"].ToString())));
        }
    }
}