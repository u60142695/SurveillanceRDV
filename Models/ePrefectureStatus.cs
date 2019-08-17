using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveillanceRDV.Models
{
    public enum ePrefectureStatus : int
    {
        AppointmentAvailable = 1,
        AppointmentUnavailable = 2,
        Querying = 3,
        Error = 4
    }
}
