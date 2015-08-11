using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonAssoce.Libs.Helpers
{
    class DateToStringConverter
    {
        private const string DATE_FORMAT_LONG_FR = "dddd dd MMMM yyyy @ H:mm";
        private const string DATE_FORMAT_LONG_NO_TIME_FR = "dddd dd MMMM yyyy";
        private const string DATE_FORMAT_SHORT_FR = "dd MMMM yyyy @ H:mm";
        private const string DATE_FORMAT_SHORT_NO_TIME_FR = "dd MMMM yyyy";

        private const string DATE_FORMAT_LONG_EN = "dddd, MMMM dd, yyyy @ H:mm";
        private const string DATE_FORMAT_LONG_NO_TIME_EN = "dddd, MMMM dd, yyyy";    
        private const string DATE_FORMAT_SHORT_EN = "MMMM dd, yyyy @ H:mm";
        private const string DATE_FORMAT_SHORT_NO_TIME_EN = "MMMM dd, yyyy";

        public DateToStringConverter() { }

        public string ConvertDateToString(DateTime dateToFormat, bool displayTime, bool isLongDate)
        {
            if (dateToFormat == null)
            {
                return string.Empty;
            }
            else if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("fr"))
            {
                if (isLongDate)
                {
                    if (displayTime)
                    {
                        return dateToFormat.ToString(DATE_FORMAT_LONG_FR);
                    }
                    else
                    {
                        return dateToFormat.ToString(DATE_FORMAT_LONG_NO_TIME_FR);
                    }
                }
                else
                {
                    if (displayTime)
                    {
                        return dateToFormat.ToString(DATE_FORMAT_SHORT_FR);
                    }
                    else
                    {
                        return dateToFormat.ToString(DATE_FORMAT_SHORT_NO_TIME_FR);
                    }
                }
            }
            else
            {
                if (isLongDate)
                {
                    if (displayTime)
                    {
                        return dateToFormat.ToString(DATE_FORMAT_LONG_EN);
                    }
                    else
                    {
                        return dateToFormat.ToString(DATE_FORMAT_LONG_NO_TIME_EN);
                    }
                }
                else
                {
                    if (displayTime)
                    {
                        return dateToFormat.ToString(DATE_FORMAT_SHORT_EN);
                    }
                    else
                    {
                        return dateToFormat.ToString(DATE_FORMAT_SHORT_NO_TIME_EN);
                    }
                }
            }
        }
    }
}
