using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraDayReport
{
    public class FileLogger : IFileLogger
    {
        public string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\DayAheadPPSvc_Log";
        public  void Log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath ))
            {
                streamWriter.WriteLine(DateTime.Now.ToString() + " " + message);
                streamWriter.Close();
            }
        }
    }

}
