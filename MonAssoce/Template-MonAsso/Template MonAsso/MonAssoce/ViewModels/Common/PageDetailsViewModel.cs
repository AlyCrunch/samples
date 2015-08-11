using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonAssoce.ViewModels
{
    public class PageDetailsViewModel<T> : MonAssoce.Common.BindableBase
    {
        #region Variables
        private ObservableCollection<T> _items = null;
        private string _pageTitle = string.Empty;
        #endregion

        #region Getters/Setters
        public ObservableCollection<T> Items
        {
            get { return this._items; }
            set { this.SetProperty(ref this._items, value); }
        }

        public string PageTitle
        {
            get { return this._pageTitle; }
            set { this.SetProperty(ref this._pageTitle, value); }
        }
        #endregion

        /// <summary>
        /// Format DateTime like "9 March 2008 @ 15:00" or "Sunday 9 March 2008", ... 
        /// </summary>
        /// <param name="date">DateTime you want to format</param>
        /// <param name="schedule">DateTime contains an hour or not</param>
        /// <param name="longDate">Add the day (monday,...) before the date</param>
        /// <returns></returns>
        public string FormattedDateTime(DateTime date, bool schedule, bool longDate)
        {
            if (longDate)
            {
                if (schedule)
                {
                    return (date.ToString("dddd dd MMMM yyyy @ t")).Substring(0, 1).ToUpper() + (date.ToString("dddd dd MMMM yyyy @ t")).Substring(1, (date.ToString("dddd dd MMMM yyyy @ t")).Length - 1);
                }
                else
                {
                    return (date.ToString("dddd dd MMMM yyyy")).Substring(0, 1).ToUpper() + (date.ToString("dddd dd MMMM yyyy")).Substring(1, (date.ToString("dddd dd MMMM yyyy")).Length - 1);
                }
            }
            else
            {
                if (schedule)
                {
                    return date.ToString("dd MMMM yyyy @ t");
                }
                else
                {
                    return date.ToString("dd MMMM yyyy");
                }
            }
        }
    }
}
