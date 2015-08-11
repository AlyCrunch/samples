using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace MonAssoce.Data.Libs
{
    class Helpers
    {
        /// <summary>
        /// Checks whether the URI is a remote URI or loads data locally
        /// </summary>
        /// <param name="URI"></param>
        /// <returns></returns>
        public bool IsRemoteURI(string URI)
        {
            try
            {
                if (URI.Trim().StartsWith("http://") || URI.Trim().StartsWith("https://"))
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public string GetBingStaticMapUrl(double latitude, double longitude, string bingMapsKey)
        {
            return string.Format("http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{0},{1}/15?mapSize=376,376&pp={0},{1};21&mapVersion=v1&key={2}", latitude, longitude, bingMapsKey);
        }
    }
}
