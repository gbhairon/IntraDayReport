using IntraDayReport.Models;
using System.Collections.Generic;

namespace IntraDayReport
{
    public interface IPowerTrades
    {
        IEnumerable<HourlyVolume> GetPowerTrades();
    }
}