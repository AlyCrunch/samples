using MonAssoce.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MonAssoce.Views;
using MonAssoce.ViewModels;
using MonAssoce.Data;
using Windows.ApplicationModel.Search;
using MonAssoce.ViewModels.Items;
using System.Diagnostics;
using MonAssoce.BackgroundTasks;
using Windows.ApplicationModel.Background;
using MonAssoce.Libs.Helpers.BackgroundTask;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace MonAssoce
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }
            

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            SearchPane.GetForCurrentView().SuggestionsRequested += OnSuggestionsRequested;

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            // Register handler for SuggestionsRequested events from the search pane
            SearchPane.GetForCurrentView().SuggestionsRequested += OnSuggestionsRequested;
            this.StartBackgroundTask();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        #region ViewModels
        private static MainPageViewModel _mainPageViewModel = null;
        public static MainPageViewModel MainPageViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_mainPageViewModel == null)
                {
                    _mainPageViewModel = new MainPageViewModel();
                }
                return _mainPageViewModel;
            }
        }

        private static SearchPageViewModel _searchPageViewModel = null;
        public static SearchPageViewModel SearchPageViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_searchPageViewModel == null)
                {
                    _searchPageViewModel = new SearchPageViewModel();
                }
                return _searchPageViewModel;
            }
        }

        private static NewsViewModel _newsViewModel = null;
        public static NewsViewModel NewsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_newsViewModel == null)
                {
                    _newsViewModel = new NewsViewModel();
                }
                return _newsViewModel;
            }
        }

        private static NewsDetailsViewModel _newsDetailsViewModel = null;
        public static NewsDetailsViewModel NewsDetailsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_newsDetailsViewModel == null)
                {
                    _newsDetailsViewModel = new NewsDetailsViewModel();
                }
                return _newsDetailsViewModel;
            }
        }

        private static EventsViewModel _eventsViewModel = null;
        public static EventsViewModel EventsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_eventsViewModel == null)
                {
                    _eventsViewModel = new EventsViewModel();
                }
                return _eventsViewModel;
            }
        }

        private static EventsDetailsViewModel _eventsDetailsViewModel = null;
        public static EventsDetailsViewModel EventsDetailsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_eventsDetailsViewModel == null)
                {
                    _eventsDetailsViewModel = new EventsDetailsViewModel();
                }
                return _eventsDetailsViewModel;
            }
        }

        private static ProjectsViewModel _projectsViewModel = null;
        public static ProjectsViewModel ProjectsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_projectsViewModel == null)
                {
                    _projectsViewModel = new ProjectsViewModel();
                }
                return _projectsViewModel;
            }
        }

        private static ProjectsDetailsViewModel _projectsDetailsViewModel = null;
        public static ProjectsDetailsViewModel ProjectsDetailsViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_projectsDetailsViewModel == null)
                {
                    _projectsDetailsViewModel = new ProjectsDetailsViewModel();
                }
                return _projectsDetailsViewModel;
            }
        }

        private static MembersViewModel _membersViewModel = null;
        public static MembersViewModel MembersViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (_membersViewModel == null)
                {
                    _membersViewModel = new MembersViewModel();
                }
                return _membersViewModel;
            }
        }
        #endregion

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {

            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            Window.Current.Content = rootFrame;

            SearchPage.Activate(args.QueryText, args.PreviousExecutionState);
        }
        
        public async void OnSuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            string query = args.QueryText.ToLower();
            List<string> terms = new List<string>();

            if (!App.SearchPageViewModel.IsDataLoaded)
            {
                await App.SearchPageViewModel.LoadData();
            }
            
            foreach (MainHubViewModel hub in App.SearchPageViewModel.Hubs)
            {
                foreach (MainItemViewModel item in hub)
                {
                    terms.Add(item.Title);
                }
            }


            foreach (var term in terms)
            {
                if (term.ToLower().StartsWith(query.ToLower()))
                {
                    try
                    {
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestion(term);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }

        #region BackgroundTask
        private async void StartBackgroundTask()
        {
            bool isTime = false;
            bool isOnLauch = false;
            bool internetConnected = false;

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTaskConstant.TimeTriggeredTaskName)
                {
                    BackgroundTaskConstant.UpdateBackgroundTaskStatus(BackgroundTaskConstant.TimeTriggeredTaskName, true);
                    isTime = true;
                }

                if (task.Value.Name == BackgroundTaskConstant.SampleBackgroundTaskName)
                {
                    BackgroundTaskConstant.UpdateBackgroundTaskStatus(BackgroundTaskConstant.SampleBackgroundTaskName, true);
                    isOnLauch = true;
                }

                if (task.Value.Name == BackgroundTaskConstant.InternetBackgroundTaskName)
                {
                    BackgroundTaskConstant.UpdateBackgroundTaskStatus(BackgroundTaskConstant.InternetBackgroundTaskName, true);
                    internetConnected = true;
                }
            }

            try
            {
                if (!isTime && !isOnLauch)
                    await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch { }

            if (!isTime)
                this.RegisterBackgroundTaskTime();

            if (!isOnLauch)
                this.RegisterBackgroundTaskPresent();

            if (!internetConnected)
                this.RegisterBackgroundTaskInternet();
        }

        private void RegisterBackgroundTaskTime()
        {
            var task = BackgroundTaskConstant.RegisterBackgroundTask(BackgroundTaskConstant.SampleBackgroundTaskEntryPoint,
                                                                   BackgroundTaskConstant.TimeTriggeredTaskName,
                                                                   new TimeTrigger(15, false),
                                                                   null);
        }

        private void RegisterBackgroundTaskPresent()
        {

            var task = BackgroundTaskConstant.RegisterBackgroundTask(BackgroundTaskConstant.SampleBackgroundTaskEntryPoint,
                                                                   BackgroundTaskConstant.SampleBackgroundTaskName,
                                                                   new SystemTrigger(SystemTriggerType.SessionConnected, false),
                                                                   null);
        }

        private void RegisterBackgroundTaskInternet()
        {

            var task = BackgroundTaskConstant.RegisterBackgroundTask(BackgroundTaskConstant.InternetBackgroundTaskEntryPoint,
                                                                   BackgroundTaskConstant.InternetBackgroundTaskName,
                                                                   new SystemTrigger(SystemTriggerType.InternetAvailable, false),
                                                                   null);
        }

        private void UnregisterBackgroundTask(string NameTask)
        {
            BackgroundTaskConstant.UnregisterBackgroundTasks(NameTask);
        }
        #endregion
    }
}
