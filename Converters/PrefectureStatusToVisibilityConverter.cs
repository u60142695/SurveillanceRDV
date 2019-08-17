using SurveillanceRDV.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SurveillanceRDV.Converters
{
    public class PrefectureStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ePrefectureStatus s1 = (ePrefectureStatus)value;
            ePrefectureStatus s2 = (ePrefectureStatus)parameter;

            return s1 == s2 ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
