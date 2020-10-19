using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SurveillanceRDV.Utilities
{
    public class StatusLogger
    {
        public static string FILEPATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SurveillanceRDV.log";

        static StatusLogger()
        {
            if (!File.Exists(FILEPATH))
                File.AppendAllText(FILEPATH, "----- Journal des réponses positives -----");
        }

        public static void WriteEvent(string strPrefName)
        {
            string contents = DateTime.Now.ToString() + " : " + strPrefName + "\r\n";

            File.AppendAllText(FILEPATH, contents);
        }
    }
}
