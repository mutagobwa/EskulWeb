using System.Diagnostics;

namespace Eskul.Custom
{
    public class LoggerErr : ILoggerErr
    {
        string _sPath;
        public LoggerErr(string logPath)
        {
            _sPath = logPath;
        }

        public void Info(string info)
        {
            try
            {
                var sPath = Path.Combine(_sPath, "WebEvents");
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                ClearOldLogs(sPath, 10);

                sPath = Path.Combine(sPath, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                sPath = Path.Combine(sPath, "Info-" + DateTime.Now.ToString("yyyyMMdd") + ".log");

                var eventmsg = DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss") + ": " + info;

                using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs)) { }
                }

                using (FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                        sw.WriteLine(eventmsg);
                }
            }
            catch
            {
            }
        }

        public void Request(string message, string msgId, bool xml = false)
        {
            try
            {

                var sPath = Path.Combine(_sPath, "Requests");
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                ClearOldLogs(sPath, 10);

                sPath = Path.Combine(sPath, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                if (!xml)
                {
                    sPath = Path.Combine(sPath, msgId + "-" + DateTime.Now.ToString("yyyyMMdd") + ".log");

                    var requestmsg = DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss") + ": " + message;

                    using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs)) { }
                    }

                    using (FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                            sw.WriteLine(requestmsg);
                    }
                }
                else
                {
                    //logic for xml
                }
            }
            catch
            {
            }
        }

        public void Response(string message, string msgId, bool xml = false)
        {
            var sPath = Path.Combine(_sPath, "Responses");
            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

            ClearOldLogs(sPath, 10);

            sPath = Path.Combine(sPath, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

            if (!xml)
            {
                sPath = Path.Combine(sPath, msgId + "-" + DateTime.Now.ToString("yyyyMMdd") + ".log");

                var responsemsg = DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss") + ": " + message;

                using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs)) { }
                }

                using (FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                        sw.WriteLine(responsemsg);
                }
            }
            else
            {
                //logic for xml
            }
        }

        public void Error(string message, Exception ex)
        {
            try
            {
                var sPath = Path.Combine(_sPath, "WebErrors");
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                ClearOldLogs(sPath, 10);

                sPath = Path.Combine(sPath, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                sPath = Path.Combine(sPath, "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log");

                var requestmsg = DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss") + ":" + message + Environment.NewLine
                    + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "") + Environment.NewLine + ex.StackTrace;

                using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs)) { }
                }

                using (FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                        sw.WriteLine(requestmsg);
                }
            }
            catch
            {
            }
        }
        private static void ClearOldLogs(string path, int days)
        {
            try
            {
                var di = new DirectoryInfo(path);
                var itms = di.GetFiles().Where(p => p.LastWriteTime.AddDays(days) < DateTime.Now);
                foreach (var itm in itms)
                {
                    File.Delete(itm.FullName);
                }
            }
            catch
            {
            }
        }

        
    }
}
