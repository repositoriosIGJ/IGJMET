using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Timers
    {
        private static DateTime _unixTimestamp;
        public static DateTime UnixTimestamp
        {
            get { return _unixTimestamp != null ? _unixTimestamp : (_unixTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0)); }
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            var diff = date - UnixTimestamp;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConverFormUnixTimestamp(double timestamp)
        {
            var dateUnix = UnixTimestamp;
            return dateUnix.AddSeconds(timestamp);
        }
    }
}
