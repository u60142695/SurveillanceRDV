using SurveillanceRDV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SurveillanceRDV.Requestors
{
    public abstract class IPrefectureRequestor
    {
        public Prefecture Owner { get; set; }

        public abstract bool? Request();
    }
}
