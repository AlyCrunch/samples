using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.ViewModels.Items;

namespace MonAssoce.ViewModels
{
    public class ProjectsDetailsViewModel : PageDetailsViewModel<ProjectItemViewModel>
    {
        public ProjectsDetailsViewModel()
        {
            this.Items = new ObservableCollection<ProjectItemViewModel>();
        }

        public async Task LoadData()
        {
            if (App.ProjectsViewModel.Items.Count == 0)
            {
                await App.ProjectsViewModel.LoadData();
            }
            this.Items = App.ProjectsViewModel.Items;
        }
    }
}
