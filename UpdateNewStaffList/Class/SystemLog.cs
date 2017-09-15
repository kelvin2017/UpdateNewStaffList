using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace UpdateNewStaffList.Class
{
    class SystemLog
    {
        private static string _log_file_path;

        public static string filePath
        {
            get { return SystemLog._log_file_path; }
            set { if (value.Length > 0) SystemLog._log_file_path = value; }
        }

        public static void flush()
        {
            File.WriteAllText(SystemLog.filePath, string.Empty);
        }

        public static void log(string msg)
        {
            if (msg.Length > 0)
            {
                using (StreamWriter sw = File.AppendText(SystemLog.filePath))
                {
                    sw.WriteLine("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), msg);
                    sw.Flush();
                }
            }
        }
    }
}
