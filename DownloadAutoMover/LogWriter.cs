using System;
using System.IO;

namespace DownloadAutoMover
{
    public class LogWriter
    {
        public LogWriter(string path, string msg)
        {
            FileInfo fi = new FileInfo(path);
            if (!Directory.Exists(fi.Directory.ToString()))
            {
                Directory.CreateDirectory(fi.Directory.ToString());
            }

            string newMsg = DateTime.Now.ToShortDateString() + ":" + DateTime.Now.ToShortTimeString() + " - " + msg;
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(newMsg);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(newMsg);
                }
            }
        }
    }
}
