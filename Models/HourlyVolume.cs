using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraDayReport.Models
{
    public class HourlyVolume
    {
        public string Hour { get; set; }
        public Decimal Volume { get; set; }

        public string TwentyFourHour 
        {
            get { return new TimeSpan(1, Int32.Parse(Hour) == 1 ? 23 : Int32.Parse(Hour) - 2, 0, 1).ToString(@"hh\:mm"); }
        }

    }
}
