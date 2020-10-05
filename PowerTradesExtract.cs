using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntraDayReport.Models;

namespace IntraDayReport
{
    public class PowerTradesExtract : IPowerTradesExtract
    {
        const string FILENAMEPREFIX = " PowerPosition_";
        const string FILENAMEDATEPOSTFIXFORMAT = "yyyyMMdd_HHmm";
        private string _filelocation;
        public PowerTradesExtract(string filelocation)
        {
            _filelocation = filelocation;
        }

        public void ProduceExtractFile(IEnumerable<HourlyVolume> hourlyVolumes)
        {
            string filename = _filelocation + "\\" + FILENAMEPREFIX + DateTime.Now.ToString(FILENAMEDATEPOSTFIXFORMAT) + ".csv";

            using (FileStream fs = File.Create(filename))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes("Local Time,Volume" + Environment.NewLine);

                fs.Write(title, 0, title.Length);
                foreach (var hourlyVolume in hourlyVolumes)
                {
                    byte[] volumeByHour = new UTF8Encoding(true).GetBytes(hourlyVolume.TwentyFourHour + "," + hourlyVolume.Volume.ToString() + Environment.NewLine);
                    fs.Write(volumeByHour, 0, volumeByHour.Length);
                }
            }
        }
    }
}
