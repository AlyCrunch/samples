using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonAssoce.ViewModels.Items;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace MonAssoce.Views
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchPage : MonAssoce.Common.LayoutAwarePage
    {
        private ResourceLoader res = new ResourceLoader();
        private UIElement _previousContent;
        private ApplicationExecutionState _previousExecutionState;
        private Dictionary<string, List<MainItemViewModel>> _results = new Dictionary<string, List<MainItemViewModel>>();
        private List<MainItemViewModel> all = new List<MainItemViewModel>();
        private List<MainItemViewModel> news = new List<MainItemViewModel>();
        private List<MainItemViewModel> projects = new List<MainItemViewModel>();
        private List<MainItemViewModel> events = new List<MainItemViewModel>();
        private List<MainItemViewModel> members = new List<MainItemViewModel>();

        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Determines how best to support navigation back to the previous application state.
        /// </summary>
        public static void Activate(String queryText, ApplicationExecutionState previousExecutionState)
        {
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame != null)
            {
                // If the app is already running and uses top-level frame navigation we can just
                // navigate to the search results
                frame.Navigate(typeof(SearchPage), queryText);
            }
            else
            {
                // Otherwise bypass navigation and provide the tools needed to emulate the back stack
                SearchPage page = new SearchPage();
                page._previousContent = previousContent;
                page._previousExecutionState = previousExecutionState;
                page.LoadState(queryText, null);
                Window.Current.Content = page;
            }

            // Either way, active the window
            Window.Current.Activate();
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
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            await App.SearchPageViewModel.LoadData();

            var queryText = (navigationParameter as String);
            var queryLower = (navigationParameter as String).ToLower();

            string[] queryWords = queryLower.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            var filterList = new List<Filter>();

            int cpt = 0;
            foreach (MainHubViewModel hub in App.SearchPageViewModel.Hubs)
            {
                var items = new List<MainItemViewModel>();

                foreach (var task in hub)
                {
                    try
                    {
                        foreach (string query in queryWords)
                        {
                            if (task.Title.ToLower().Contains(query) && query != "")
                            {
                                int ind = -1;
                                int i = 0;

                                foreach (MainItemViewModel t in all)
                                {
                                    if (t == task)
                                        ind = i;
                                    i++;
                                }
                                if (ind > 0)
                                {
                                    all.RemoveAt(ind);
                                    all.Insert(ind - 1, task);
                                }
                                else if (ind == -1)
                                {
                                    all.Add(task);
                                }

                                ind = -1;
                                foreach (MainItemViewModel t in items)
                                {
                                    if (t == task)
                                        ind = i;
                                    i++;
                                }
                                if (ind > 0)
                                {
                                    items.RemoveAt(ind);
                                    items.Insert(ind - 1, task);
                                }
                                else if (ind == -1)
                                {
                                    items.Add(task);
                                }
                            }

                            if (task.Description.ToLower().Contains(query) && query != "")
                            {
                                int ind = -1;
                                int i = 0;

                                foreach (MainItemViewModel t in all)
                                {
                                    if (t == task)
                                        ind = i;
                                    i++;
                                }
                                if (ind > 0)
                                {
                                    all.RemoveAt(ind);
                                    all.Insert(ind - 1, task);
                                }
                                else if (ind == -1)
                                {
                                    all.Add(task);
                                }

                                ind = -1;
                                foreach (MainItemViewModel t in items)
                                {
                                    if (t == task)
                                        ind = i;
                                    i++;
                                }
                                if (ind > 0)
                                {
                                    items.RemoveAt(ind);
                                    items.Insert(ind - 1, task);
                                }
                                else if (ind == -1)
                                {
                                    items.Add(task);
                                }
                            }
                        }
                    }
                    catch { }
                }

                if (cpt == 0)
                    news = items;
                if (cpt == 1)
                    events = items;
                if (cpt == 2)
                    projects = items;
                if (cpt == 3)
                    members = items;

                cpt++;
            }
            if (all.Count > 0)
            {
                this.DefaultViewModel["ResultStates"] = "ResultsFound";
            }


            filterList.Add(new Filter(res.GetString("allQuery"), news.Count + events.Count + projects.Count + members.Count, true));
            if (news.Count != 0)
                filterList.Add(new Filter(App.SearchPageViewModel.Hubs[0].HubName, news.Count, false));
            if (events.Count != 0)
                filterList.Add(new Filter(App.SearchPageViewModel.Hubs[1].HubName, events.Count, false));
            if (projects.Count != 0)
                filterList.Add(new Filter(App.SearchPageViewModel.Hubs[2].HubName, projects.Count, false));
            if (members.Count != 0)
                filterList.Add(new Filter(App.SearchPageViewModel.Hubs[3].HubName, members.Count, false));

            // Communicate results through the view model
            this.DefaultViewModel["Results"] = all;
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["CanGoBack"] = this._previousContent != null;
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// Invoked when the back button is pressed.
        /// </summary>
        /// <param name="sender">The Button instance representing the back button.</param>
        /// <param name="e">Event data describing how the button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            // Return the application to the state it was in before search results were requested
            if (this.Frame != null && this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else if (this._previousContent != null)
            {
                Window.Current.Content = this._previousContent;
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        /// <summary>
        /// Invoked when a filter is selected using the ComboBox in snapped view state.
        /// </summary>
        /// <param name="sender">The ComboBox instance.</param>
        /// <param name="e">Event data describing how the selected filter was changed.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceLoader res = new ResourceLoader();
            // Determine what filter was selected
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                selectedFilter.Active = true;

                if (selectedFilter.Name.Contains(res.GetString("MembersMainPage")))
                    this.DefaultViewModel["Results"] = members;

                if (selectedFilter.Name.Contains(res.GetString("allQuery")))
                    this.DefaultViewModel["Results"] = all;

                if (selectedFilter.Name.Contains(res.GetString("ProjectsPageTitleMain")))
                    this.DefaultViewModel["Results"] = projects;

                if (selectedFilter.Name.Contains(res.GetString("NewsPageTitleMain")))
                    this.DefaultViewModel["Results"] = news;

                if (selectedFilter.Name.Contains(res.GetString("EventsPageTitleMain")))
                    this.DefaultViewModel["Results"] = events;

                // Ensure results are found
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // Display informational text when there are no search results.
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// Invoked when a filter is selected using a RadioButton when not snapped.
        /// </summary>
        /// <param name="sender">The selected RadioButton instance.</param>
        /// <param name="e">Event data describing how the RadioButton was selected.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // Mirror the change into the CollectionViewSource used by the corresponding ComboBox
            // to ensure that the change is reflected when snapped
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        /// <summary>
        /// View model describing one of the filters available for viewing search results.
        /// </summary>
        private sealed class Filter : MonAssoce.Common.BindableBase
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
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
            else
            {
                this.Frame.Navigate(typeof(Members), item);
            }
        }
    }
}
