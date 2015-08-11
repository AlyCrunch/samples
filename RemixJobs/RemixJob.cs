using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RemixJobsFlux.ViewModel.Model
{
    public class RemixJob
    {
        public class CompanyLogo
        {
            public CompanyLogo()
            {
                this.url = string.Empty;
                this.path = string.Empty;
            }

            [JsonProperty("url")]
            public string url { get; set; }
            [JsonProperty("path")]
            public string path { get; set; }
        }

        public class Category
        {
            public Category()
            {
                this.name = string.Empty;
                this.localized_name = string.Empty;
                this.localized_menu_name = string.Empty;
                this.rss = string.Empty;
                this.feedburner_id = string.Empty;
                this.twitter_url = string.Empty;
                this.twitter_screen_name = string.Empty;
            }

            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("localized_name")]
            public string localized_name { get; set; }
            [JsonProperty("localized_menu_name")]
            public string localized_menu_name { get; set; }
            [JsonProperty("rss")]
            public string rss { get; set; }
            [JsonProperty("feedburner_id")]
            public string feedburner_id { get; set; }
            [JsonProperty("twitter_url")]
            public string twitter_url { get; set; }
            [JsonProperty("twitter_screen_name")]
            public string twitter_screen_name { get; set; }
        }

        public class ShortUrl
        {
            public ShortUrl()
            {
                this.short_url = string.Empty;
            }

            [JsonProperty("short_url")]
            public string short_url { get; set; }
        }

        public class Geolocation
        {
            public Geolocation()
            {
                this.short_formatted_address = string.Empty;
                this.formatted_address = string.Empty;
                this.lat = 0;
                this.lng = 0;
            }

            [JsonProperty("short_formatted_address")]
            public string short_formatted_address { get; set; }
            [JsonProperty("formatted_address")]
            public string formatted_address { get; set; }
            [JsonProperty("lat")]
            public double lat { get; set; }
            [JsonProperty("lng")]
            public double lng { get; set; }
        }

        public class Location
        {
            public Location()
            {
                this.short_formatted_address = string.Empty;
                this.formatted_address = string.Empty;
                this.lat = 0;
                this.lng = 0;
            }

            [JsonProperty("short_formatted_address")]
            public string short_formatted_address { get; set; }
            [JsonProperty("formatted_address")]
            public string formatted_address { get; set; }
            [JsonProperty("lat")]
            public double lat { get; set; }
            [JsonProperty("lng")]
            public double lng { get; set; }
        }

        public class CompanyLocation
        {
            public CompanyLocation()
            {
                this.short_formatted_address = string.Empty;
                this.formatted_address = string.Empty;
                this.lat = 0;
                this.lng = 0;
            }

            [JsonProperty("short_formatted_address")]
            public string short_formatted_address { get; set; }
            [JsonProperty("formatted_address")]
            public string formatted_address { get; set; }
            [JsonProperty("lat")]
            public double lat { get; set; }
            [JsonProperty("lng")]
            public double lng { get; set; }
        }

        public class Logo
        {
            public Logo()
            {
                this.url = string.Empty;
                this.path = string.Empty;
            }

            [JsonProperty("url")]
            public string url { get; set; }
            [JsonProperty("path")]
            public string path { get; set; }
        }

        public class Www
        {
            [JsonProperty("href")]
            public string href { get; set; }
        }

        public class Links
        {
            [JsonProperty("www")]
            public Www www { get; set; }
        }

        public class Picture
        {
            public Picture()
            {
                this.url = string.Empty;
            }

            [JsonProperty("url")]
            public string url { get; set; }
        }

        public class Advantage
        {
            public Advantage()
            {
                this.name = string.Empty;
            }

            [JsonProperty("name")]
            public string name { get; set; }
        }

        public class Company
        {
            public Company()
            {
                this.advantages = new List<Advantage>();
                this.pictures = new List<Picture>();
                _links = new Links();
                this.logo = new Logo();
                company_description = string.Empty;
                company_address = string.Empty;
                company_location = new CompanyLocation();
                company_year_create = 0;
                company_name = string.Empty;
                company_logo = string.Empty;
                company_website = string.Empty;
                this.type = type;
                is_recruiting = false;
                this.description = string.Empty;
                this.address = string.Empty;
                this.location = new Location();
                year_create = 0;
                this.website = string.Empty;
                this.picture = string.Empty;
                this.name = string.Empty;
                this.id = 0;
            }

            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("picture")]
            public string picture { get; set; }
            [JsonProperty("website")]
            public string website { get; set; }
            [JsonProperty("year_create")]
            public int year_create { get; set; }
            [JsonProperty("location")]
            public Location location { get; set; }
            [JsonProperty("address")]
            public string address { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("is_recruiting")]
            public bool is_recruiting { get; set; }
            [JsonProperty("type")]
            public int type { get; set; }
            [JsonProperty("company_name")]
            public string company_name { get; set; }
            [JsonProperty("company_logo")]
            public string company_logo { get; set; }
            [JsonProperty("company_website")]
            public string company_website { get; set; }
            [JsonProperty("company_year_create")]
            public int company_year_create { get; set; }
            [JsonProperty("company_location")]
            public CompanyLocation company_location { get; set; }
            [JsonProperty("company_address")]
            public string company_address { get; set; }
            [JsonProperty("company_description")]
            public string company_description { get; set; }
            [JsonProperty("logo")]
            public Logo logo { get; set; }
            [JsonProperty("_links")]
            public Links _links { get; set; }
            [JsonProperty("pictures")]
            public List<Picture> pictures { get; set; }
            [JsonProperty("advantages")]
            public List<Advantage> advantages { get; set; }
        }
        
        public class Job
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("title")]
            public string title { get; set; }
            [JsonProperty("contract_type")]
            public string contract_type { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("experience")]
            public string experience { get; set; }
            [JsonProperty("study")]
            public string study { get; set; }
            [JsonProperty("company_website")]
            public string company_website { get; set; }
            [JsonProperty("company_name")]
            public string company_name { get; set; }
            [JsonProperty("company_logo")]
            public CompanyLogo company_logo { get; set; }
            [JsonProperty("tags")]
            public List<object> tags { get; set; }
            [JsonProperty("status")]
            public string status { get; set; }
            [JsonProperty("soldout")]
            public bool soldout { get; set; }
            [JsonProperty("validation_time")]
            public string validation_time { get; set; }
            [JsonProperty("creation_time")]
            public string creation_time { get; set; }
            [JsonProperty("telecommute")]
            public bool telecommute { get; set; }
            [JsonProperty("categories")]
            public List<Category> categories { get; set; }
            [JsonProperty("short_url")]
            public ShortUrl short_url { get; set; }
            [JsonProperty("geolocation")]
            public Geolocation geolocation { get; set; }
            [JsonProperty("highlight")]
            public bool highlight { get; set; }
            [JsonProperty("company")]
            public Company company { get; set; }
            [JsonProperty("_links")]
            public Links _links { get; set; }
        }

        public class Params
        {
            public Params()
            {
                this.page = 0;
            }

            [JsonProperty("page")]
            public int page { get; set; }
        }

        public class Next
        {
            public Next()
            {
                this.href = string.Empty;
                this.@params = new Params();
            }

            [JsonProperty("href")]
            public string href { get; set; }
            [JsonProperty("@params")]
            public Params @params { get; set; }
        }

        public class Rss
        {

            public Rss()
            {
                this.href = string.Empty;
                this.title = string.Empty;
            }

            [JsonProperty("href")]
            public string href { get; set; }
            [JsonProperty("title")]
            public string title { get; set; }
        }

        public class Nav
        {
            public Nav()
            {
                this.next = new Next();
                this.rss = new Rss();
            }

            [JsonProperty("next")]
            public Next next { get; set; }
            [JsonProperty("rss")]
            public Rss rss { get; set; }
        }

        public class RootObject
        {
            public RootObject()
            {
                this._links = new Nav();
                this.jobs = new List<Job>();
            }

            [JsonProperty("jobs")]
            public List<Job> jobs { get; set; }
            [JsonProperty("total_results_count")]
            public int total_results_count { get; set; }
            [JsonProperty("page_number")]
            public int page_number { get; set; }
            [JsonProperty("items_per_page")]
            public int items_per_page { get; set; }
            [JsonProperty("ordered_by")]
            public string ordered_by { get; set; }
            [JsonProperty("orderable_by")]
            public List<string> orderable_by { get; set; }
            [JsonProperty("_links")]
            public Nav _links { get; set; }
            [JsonProperty("feedburner_id")]
            public string feedburner_id { get; set; }
            [JsonProperty("twitter_screen_name")]
            public string twitter_screen_name { get; set; }
        }
    }
}
