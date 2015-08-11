using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.ViewModels.Items;

namespace MonAssoce.ViewModels
{
    public class NewsDetailsViewModel : PageDetailsViewModel<NewsItemViewModel>
    {
        public NewsDetailsViewModel()
        {
            this.Items = new ObservableCollection<NewsItemViewModel>();
        }

        public async Task LoadData()
        {
            if (App.NewsViewModel.Items.Count == 0)
            {
                await App.NewsViewModel.LoadData();
            }
            this.Items = App.NewsViewModel.Items;
        }
    }
}
