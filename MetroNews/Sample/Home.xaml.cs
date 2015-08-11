using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MetroFrance.ViewModels.Items;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;
using MetroFrance.Views.UserControls;
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using MetroFrance.Libs;
using System.Threading;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace MetroFrance.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class Home : MetroFrance.Common.LayoutAwarePage
    {
        #region const

        public const string MAJ = "Mis à jour ";

        #endregion
        private Rect _windowBounds;
        private double _settingsWidth = 346;
        private Popup _settingsPopup;
        private bool _isArrangeItems = false;

        // Orientation changed (fix issue when width / height are not well detected).
        //DispatcherTimer CheckWidthHeightTimer = new DispatcherTimer();
        private static readonly SemaphoreSlim _buildView = new SemaphoreSlim(initialCount: 1);

        public Home()
        {
            this.DataContext = App.HomeViewModel;

            this.InitializeComponent();

            if (!App.HomeViewModel.AlreadySet)
            {
                App.HomeViewModel.AlreadySet = true;

                _windowBounds = Window.Current.Bounds;

                Window.Current.SizeChanged += OnWindowSizeChanged;

                SettingsPane.GetForCurrentView().CommandsRequested += Home_CommandsRequested;
            }
        }

        #region Settings

        void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            _windowBounds = Window.Current.Bounds;
        }

        void Home_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("town", "Ma Ville", (x) =>
            {
                _settingsPopup = new Popup();
                _settingsPopup.Closed += OnPopupClosed;
                Window.Current.Activated += OnWindowActivated;
                _settingsPopup.IsLightDismissEnabled = true;
                _settingsPopup.Width = _settingsWidth;
                _settingsPopup.Height = _windowBounds.Height;

                SettingsCharm mypane = new SettingsCharm();
                mypane.Width = _settingsWidth;
                mypane.Height = _windowBounds.Height;

                _settingsPopup.Child = mypane;
                _settingsPopup.SetValue(Canvas.LeftProperty, _windowBounds.Width - _settingsWidth);
                _settingsPopup.SetValue(Canvas.TopProperty, 0);
                _settingsPopup.IsOpen = true;
            });

            SettingsCommand cmdAbout = new SettingsCommand("about", "À propos", (x) =>
            {
                _settingsPopup = new Popup();
                _settingsPopup.Closed += OnPopupClosed;
                Window.Current.Activated += OnWindowActivated;
                _settingsPopup.IsLightDismissEnabled = true;
                _settingsPopup.Width = _settingsWidth;
                _settingsPopup.Height = _windowBounds.Height;

                AboutCharm mypane = new AboutCharm();
                mypane.Width = _settingsWidth;
                mypane.Height = _windowBounds.Height;

                _settingsPopup.Child = mypane;
                _settingsPopup.SetValue(Canvas.LeftProperty, _windowBounds.Width - _settingsWidth);
                _settingsPopup.SetValue(Canvas.TopProperty, 0);
                _settingsPopup.IsOpen = true;
            });

            SettingsCommand cmdPersonal = new SettingsCommand("personal", "Données personelles", (x) =>
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri(App.PERSONAL_DATA_URL));
            });

            args.Request.ApplicationCommands.Add(cmd);
            args.Request.ApplicationCommands.Add(cmdAbout);
            args.Request.ApplicationCommands.Add(cmdPersonal);
        }

        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                _settingsPopup.IsOpen = false;
            }
        }

        void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }

        #endregion

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            App.LastCat = new ArticleItemViewModel();
            App.LastMedia = new ArticleItemViewModel();
        }

        #region Orientation

        private async void FullScreenCompleted(object sender, object e)
        {
            try
            {
                _buildView.Release();
                if (App.HomeViewModel.IsLandscape == false || App.IsLaunched || App.HomeViewModel.Hubs[0].Count == 0)
                {
                    //App.HomeViewModel.Hubs = new System.Collections.ObjectModel.ObservableCollection<HubHomeViewModel>();
                    App.HomeViewModel.IsLandscape = true;
                    App.HomeViewModel.IsSnapped = false;
                    await this.ArrangeItems(false, true);
                }
                else
                {
                    App.HomeViewModel.IsLandscape = true;
                    App.HomeViewModel.IsSnapped = false;
                    await this.ArrangeItems(false, false);
                }
            }
            catch (Exception) { }

            //this.ScrollOnGrid();
        }

        private async void PortraitCompleted(object sender, object e)
        {
            try
            {
                _buildView.Release();
                if (App.HomeViewModel.IsLandscape == true || App.IsLaunched || App.HomeViewModel.Hubs[0].Count == 0)
                {
                    //App.HomeViewModel.Hubs = new System.Collections.ObjectModel.ObservableCollection<HubHomeViewModel>();
                    App.HomeViewModel.IsLandscape = false;
                    App.HomeViewModel.IsSnapped = false;
                    /*if (App.HomeViewModel.Hubs != null && App.HomeViewModel.Hubs.Count > 0) 
                        App.LastHubHome = App.HomeViewModel.Hubs[0]; // Reinitialize scrolling to left.*/
                    await this.ArrangeItems(false, true);
                }
                else
                {
                    App.HomeViewModel.IsLandscape = false;
                    App.HomeViewModel.IsSnapped = false;
                    await this.ArrangeItems(false, false);
                }
            }
            catch (Exception) { }

            //this.ScrollOnGrid();
        }

        private async void SnappedCompleted(object sender, object e)
        {
            try
            {
                //_buildView.Release();
                if (App.HomeViewModel.IsSnapped == false || App.IsLaunched || App.HomeViewModel.Hubs[0].Count == 0)
                {
                    //App.HomeViewModel.Hubs = new System.Collections.ObjectModel.ObservableCollection<HubHomeViewModel>();
                    App.HomeViewModel.IsLandscape = true;
                    App.HomeViewModel.IsSnapped = true;
                    if (App.IsLaunched || App.HomeViewModel.Hubs[0].Count == 0)
                    {
                        await this.ArrangeItems(false, true);
                    }
                    else await this.ArrangeItems(false, false);
                }
                else await this.ArrangeItems(false, false);
            }
            catch (Exception) { }
        }

        #endregion

        private async Task ArrangeItems(bool refresh, bool loadData)
        {
            /*if (!refresh)
            {
                this.ScrollOnGrid();
            }*/

            await _buildView.WaitAsync();

            this.RefreshButton.IsEnabled = false;
            this.CalculateItems();
            this.ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if (App.IsLaunched)
            {
                // CHECK IF THERE IS NO DATA (WE CONSIDER THERE IS NO DATA WHEN GetCategories SENDS AN EXCEPTION (quick check)
                try
                {
                    //await App.DataAccess.GetCategoriesAsync(refresh, true);
                    //await App.HomeViewModel.LoadData(refresh, true);
                    if (!NetworkInterface.GetIsNetworkAvailable())
                    {
                        await App.DataAccess.GetCategoriesAsync(false, true);
                        await App.HomeViewModel.LoadData(false, true);
                    }
                    else
                    {
                        //await App.DataAccess.GetCategoriesAsync(true, false);
                        await App.HomeViewModel.LoadData(true, false);
                    }
                }
                catch
                {
                    // Do nothing.
                    Debug.WriteLine("Catch - No connection");
                }
            }
            else
            {
                if (loadData)
                {
                    await App.HomeViewModel.LoadData(refresh, false);
                    if (App.HomeViewModel.Hubs.Count == 0) await MetroFrance.Libs.ConvertHelpers.Alerte(); // CRITICAL ERROR, CANNOT GET CATEGORIES.
                }
                else DataContext = App.HomeViewModel; // Refresh page to avoid bad displays.
            }

            this.ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            itemGridViewOut.ItemsSource = groupedItemsViewSource.View.CollectionGroups;

            /*if (!refresh)
            {
                foreach (HubHomeViewModel hub in App.HomeViewModel.Hubs)
                {
                    if (hub.HubName == App.LastHubHome.HubName)
                    {
                        await Dispatcher.RunIdleAsync((source) =>
                        {
                            itemGridViewIn.ScrollIntoView(hub);
                        });
                        break;
                    }
                }
            }*/

            //_isArrangeItems = false;

            // App has just been launched.
            if (App.IsLaunched && App.HomeViewModel.Hubs.Count > 0)
            {
                this.ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                App.IsLaunched = false;
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //_buildView.Release();
                    await this.ReloadAllItems(); // Update last update date.
                }
                this.ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else if (App.HomeViewModel.Hubs.Count == 0)
            {
                this.RefreshButton.IsEnabled = true;
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    await MetroFrance.Libs.ConvertHelpers.Alerte();
                    App.Current.Exit();
                }
            }
            //}
            if (!refresh)
            {
                this.ScrollOnGrid();
            }
            this.RefreshButton.IsEnabled = true;
            _buildView.Release();
        }

        private async void ScrollOnGrid()
        {
            try
            {
                if (App.HomeViewModel.Hubs != null)
                {
                    foreach (HubHomeViewModel hub in App.HomeViewModel.Hubs)
                    {
                        if (hub.HubName == App.LastHubHome.HubName)
                        {
                            itemGridViewIn.ScrollIntoView(hub);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void CalculateItems()
        {
            try
            {
                _isArrangeItems = true;
                if (!App.HomeViewModel.IsSnapped)
                {
                    /*if (App.HomeViewModel.IsLandscape)
                    {
                        if (this.itemGridViewIn.ActualHeight < this.itemGridViewIn.ActualWidth)
                            App.HomeViewModel.NbItemsHeight = ((int)this.ActualHeight - 200) / 85;
                        else
                            App.HomeViewModel.NbItemsHeight = ((int)this.ActualWidth - 200) / 85;
                        //App.HomeViewModel.NbItemsHeight = ((int)Window.Current.Bounds.Height - 200) / 85;
                    }
                    else
                    {
                        if (this.itemGridViewIn.ActualHeight < this.itemGridViewIn.ActualWidth)
                            App.HomeViewModel.NbItemsHeight = ((int)this.ActualWidth - 200) / 85;
                        else
                            App.HomeViewModel.NbItemsHeight = ((int)this.ActualHeight - 200) / 85;
                        //App.HomeViewModel.NbItemsHeight = ((int)Window.Current.Bounds.Width - 200) / 85;
                    }*/

                    App.HomeViewModel.NbItemsHeight = ((int)Window.Current.Bounds.Height - 200) / 85;

                    if (App.HomeViewModel.NbItemsHeight % 2 != 0)
                        App.HomeViewModel.NbItemsHeight = App.HomeViewModel.NbItemsHeight - 1;
                }
            }
            catch (Exception) { }
        }

        private void HeaderHubName_Click(object sender, RoutedEventArgs e)
        {
            Button hubClik = e.OriginalSource as Button;
            foreach (HubHomeViewModel hub in App.HomeViewModel.Hubs)
            {
                if (hub.HubName == hubClik.Content.ToString())
                    App.LastHubHome = hub;
            }

            if (hubClik.Content.ToString().ToLower() != App.ResourceLoader.GetString("NewsImages").ToLower())
                this.Frame.Navigate(typeof(Category), App.HomeViewModel.GetIDHubName(hubClik.Content.ToString()) + "hub_title" + hubClik.Content);
            else
                this.Frame.Navigate(typeof(Media));
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock hubClik = e.OriginalSource as TextBlock;
            foreach (HubHomeViewModel hub in App.HomeViewModel.Hubs)
            {
                if (hub.HubName == hubClik.Tag.ToString())
                    App.LastHubHome = hub;
            }

            if (hubClik.Tag.ToString().ToLower() != App.ResourceLoader.GetString("NewsImages").ToLower())
                this.Frame.Navigate(typeof(Category), App.HomeViewModel.GetIDHubName(hubClik.Tag.ToString()) + "hub_title" + hubClik.Tag);
            else
                this.Frame.Navigate(typeof(Media));
        }

        private void itemGridViewIn_ItemClick(object sender, ItemClickEventArgs e)
        {
            ArticleItemViewModel article = e.ClickedItem as ArticleItemViewModel;
            if (article.ErrorVisibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                foreach (HubHomeViewModel hub in App.HomeViewModel.Hubs)
                {
                    if (hub.HubName == article.HubName)
                        App.LastHubHome = hub;
                }

                if (article.ImageHub)
                {
                    if (article.IsVideo)
                    {
                        App.MediaViewModel.PlayVideo = true;
                        this.Frame.Navigate(typeof(Media), article);
                    }
                    else
                    {
                        this.Frame.Navigate(typeof(Diaporama), article);
                    }
                }
                else
                {
                    this.Frame.Navigate(typeof(Article), article);
                }
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _buildView.Release();
            // Reinitialize items to refresh... and avoid clicks...
            App.HomeViewModel.Hubs = new System.Collections.ObjectModel.ObservableCollection<HubHomeViewModel>();

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await MetroFrance.Libs.ConvertHelpers.Alerte();
            }
            else
            {
                await this.ArrangeItems(true, true);

            }
        }

        private async Task ReloadAllItems()
        {
            // Force refresh...
            //await this.ArrangeItems(true, true);
            App.HomeViewModel.LastUpdateDate = MAJ + MetroFrance.Libs.ConvertHelpers.DateTimeToString(App.DataAccess.LastRefreshDate).ToLower();
        }


        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
