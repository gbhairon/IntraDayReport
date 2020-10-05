using IntraDayReport.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IntraDayReport
{
    public class PowerTrades : IPowerTrades
    {
        public IEnumerable<HourlyVolume> GetPowerTrades()
        {
            PowerService powerService = new PowerService();
            var powerTrades = powerService.GetTrades(DateTime.Today);

            var hourlyVolume = powerTrades.SelectMany(x => x.Periods).Select(y => new HourlyVolume
            {
                Hour = y.Period.ToString(),
                Volume = Decimal.Parse(y.Volume.ToString())
            }).ToList();


            var totalVolumesByPeriod =
                from volume in hourlyVolume
                group volume by volume.Hour into volumeGroup
                select new HourlyVolume
                {
                    Hour = volumeGroup.Key,
                    Volume = Math.Round(volumeGroup.Sum(x => x.Volume), 0)
                };


            return totalVolumesByPeriod;
        }
    }
}
