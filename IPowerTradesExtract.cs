using IntraDayReport.Models;
using System.Collections.Generic;

namespace IntraDayReport
{
    public interface IPowerTradesExtract
    {
        void ProduceExtractFile(IEnumerable<HourlyVolume> hourlyVolumes);
    }
}