using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Timers;
using IntraDayReport.Models;
using System.CodeDom;

namespace IntraDayReport
{
    public partial class DayAheadPPSvc : ServiceBase
    {
        IFileLogger _fileLogger = new FileLogger();

        private string _fileLocation;
        private int _interval;

        private System.Timers.Timer _timer;
        private DateTime _startTime;
        IPowerTrades _powerTrades = null;
        IPowerTradesExtract _powerTradesExtract = null;
        public DayAheadPPSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (FetchSettings())
                {
                    // Run extract on service start
                    _powerTrades = new PowerTrades();
                    _powerTradesExtract = new PowerTradesExtract(_fileLocation);
                    timer_Elapsed(this, null);
                    _startTime = DateTime.Now;
                    _timer = new System.Timers.Timer(60000 *  _interval);
                    _timer.Elapsed += timer_Elapsed;
                    _timer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.Message + ex.StackTrace);
            }
        }



        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                IEnumerable<HourlyVolume> hourlyVolumes = _powerTrades.GetPowerTrades();

                if (hourlyVolumes != null)
                {
                   _powerTradesExtract.ProduceExtractFile(hourlyVolumes);
                }
                else
                {
                   _fileLogger.Log("No hourly volumes found");
                }                
            }
            catch  (Exception ex)
            {
                _fileLogger.Log(ex.Message + ex.StackTrace);
            }
            
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();          
        }

        private bool FetchSettings()
        {
            _fileLocation = ConfigurationManager.AppSettings["PowerPositionFileLocation"];
            if (int.TryParse(ConfigurationManager.AppSettings["ExtractInterval"], out _interval))
            {
                if (Directory.Exists(_fileLocation))
                {
                    return true;
                }
                else
                    _fileLogger.Log("Error : The configured directory does not exist : " + _fileLocation);
            }
            else
                _fileLogger.Log("Error : The configured interval is not valid");

            return false;
            
        }

    }
}
