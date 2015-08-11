using Newtonsoft.Json;
using RemixJobsFlux.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace RemixJobsFlux.ViewModel
{
    public class MainViewModel
    {
        private List<MainJob> _jobs = new List<MainJob>();
        private RemixJob.RootObject _datas = null;

        

        public List<MainJob> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; NotifyPropertyChanged(); }
            
        }

        public MainViewModel()
        {
            
            _datas = GetJobs();
            Jobs = FormatDatas(_datas);
        }


        private RemixJob.RootObject GetJobs()
        {
            HttpClient client = new HttpClient();

            RemixJob.RootObject test = new RemixJob.RootObject();
            RemixJob.RootObject test2 = new RemixJob.RootObject();

            test2._links.next.href = @"/api/jobs?page=1";

            /*
            do
            {
            */

                string json = client.GetStringAsync("http://remixjobs.com" + test2._links.next.href).Result;

                try
                {
                    test2 = JsonConvert.DeserializeObject<RemixJob.RootObject>(json);
                }
                catch
                {
                }

                if (test2.jobs.Count > 0)
                {
                    test.jobs.AddRange(test2.jobs);
                    test._links.next.href = test2._links.next.href;
                }
            /*
            } while (test2.jobs.Count > 0);*/

            return test;
        }

        private List<MainJob> FormatDatas(RemixJob.RootObject datas)
        {
            List<MainJob> listJobs = new List<MainJob>();
            foreach (RemixJob.Job job in datas.jobs)
            {
                MainJob newJob = new MainJob();
                if (job.company_logo != null)
                    newJob.imageCompany = job.company_logo.url;
                else
                    newJob.imageCompany = @"\Assets\company-178-178.png";

                newJob.Title = job.title;
                newJob.ContractType = job.contract_type;

                foreach (object tag in job.tags)
                {

                    newJob.tags = newJob.tags + tag + ", ";
                }
                if (job.tags.Count > 0)
                    newJob.tags = newJob.tags.Remove(newJob.tags.Length - 2);


                foreach (RemixJob.Category cat in job.categories)
                {
                    newJob.JobType = newJob.JobType + cat.localized_menu_name + ", ";
                }
                if(job.categories.Count > 0)
                    newJob.JobType = newJob.JobType.Remove(newJob.JobType.Length - 2);

                newJob.Town = job.geolocation.short_formatted_address;
                newJob.date = job.validation_time;
                newJob.CompanyName = job.company_name;
                listJobs.Add(newJob);

            }
            return listJobs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UpdateCategories()
        {
            return String.Empty;
        }

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class MainJob
    {
        public string Title { get; set; }
        public string ContractType { get; set; }
        public string imageCompany { get; set; }
        public string tags { get; set; }
        public string date { get; set; }
        public string JobType { get; set; }
        public string Town { get; set; }

        public string CompanyName { get; set; }

    }
}
