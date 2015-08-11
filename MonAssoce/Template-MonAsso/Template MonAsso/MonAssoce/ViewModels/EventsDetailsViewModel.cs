using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.ViewModels.Items;

namespace MonAssoce.ViewModels
{
    public class EventsDetailsViewModel : PageDetailsViewModel<EventItemViewModel>
    {
        public EventsDetailsViewModel()
        {
            this.Items = new ObservableCollection<EventItemViewModel>();
        }

        public async Task LoadData()
        {
            if (App.EventsViewModel.Items.Count == 0)
            {
                await App.EventsViewModel.LoadData();
            }
            this.Items = App.EventsViewModel.Items;
        }
    }
}
