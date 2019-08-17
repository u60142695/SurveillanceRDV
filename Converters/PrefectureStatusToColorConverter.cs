using SurveillanceRDV.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SurveillanceRDV.Converters
{
    public class PrefectureStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ePrefectureStatus status = (ePrefectureStatus)value;

            switch(status)
            {
                case ePrefectureStatus.AppointmentAvailable:
                    return "Green";
                case ePrefectureStatus.AppointmentUnavailable:
                    return "Red";
                case ePrefectureStatus.Querying:
                    return "Yellow";
                case ePrefectureStatus.Error:
                    return "Orange";
                default:
                    return "Grey";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
