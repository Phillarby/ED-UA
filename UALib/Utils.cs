using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public static class Utils
    {

        //Convert seconds into DateTime representation
        public static DateTime TimeConvert(double Seconds)
        {
            DateTime dt = new DateTime(1, 1, 1);
            return dt.Add(TimeSpan.FromSeconds(Seconds));
        }
    }

     
}
