using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveillanceRDV.Utilities
{
    public class RegistryUtility
    {
        public static bool GetPrefectureQueryEnabled(string prefectureId)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\SurveillanceRDV"))
            {
                object res = key.GetValue("QueryEnabled_" + prefectureId);

                if (res == null)
                {
                    key.SetValue("QueryEnabled_" + prefectureId, 1, RegistryValueKind.DWord);

                    return true;
                }
                else
                {
                    return int.Parse(res.ToString()) == 1;
                }
            }
        }

        public static void SetPrefectureQueryEnabled(string prefectureId, bool enabled)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\SurveillanceRDV"))
            {
                key.SetValue("QueryEnabled_" + prefectureId, enabled ? 1 : 0, RegistryValueKind.DWord);
            }
        }

        public static int GetPrefectureQueryTime(string prefectureId)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\SurveillanceRDV"))
            {
                object res = key.GetValue("QueryTime_" + prefectureId);

                if(res == null)
                {
                    key.SetValue("QueryTime_" + prefectureId, 180, RegistryValueKind.DWord);

                    return 180;
                }
                else
                {
                    return int.Parse(res.ToString());
                }
            }
        }

        public static void SetPrefectureQueryTime(string prefectureId, int time)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\SurveillanceRDV"))
            {
                key.SetValue("QueryTime_" + prefectureId, time, RegistryValueKind.DWord);
            }
        }
    }
}
