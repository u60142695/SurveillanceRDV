using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveillanceRDV
{
    public class PrefectureConfiguration
    {
        private static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SurveillanceRDV.conf";

        public static Dictionary<string, int> QueryTimes = new Dictionary<string, int>();

        public static void LoadConfiguration()
        {
            if(!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "95_SARCELLES=75\r\n93_BOBIGNY=75\r\n93_RAINCY=75\r\n92_NANTERRE=75\r\n75_VPF13=75\r\n75_VPF17=75\r\n75_SAL13=75\r\n75_SAL17=75");
            }

            // Load File
            string[] lines = File.ReadAllLines(filePath);
            foreach(string line in lines)
            {
                string[] data = line.Split(new char[] { '=' });

                QueryTimes.Add(data[0], int.Parse(data[1]));
            }
        }

        public static int GetPrefectureQueryTime(string prefId)
        {
            if (QueryTimes.ContainsKey(prefId))
                return QueryTimes[prefId];
            return 120;
        }
    }
}
