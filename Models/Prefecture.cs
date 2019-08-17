using SurveillanceRDV.Requestors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SurveillanceRDV.Models
{
    public class Prefecture : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (propertyName != "")
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Prefecture(string name, IPrefectureRequestor requestor)
        {
            _name = name;
            _requestor = requestor;
            _requestor.Owner = this;
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private bool _result = false;
        public bool Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        private ePrefectureStatus _status = ePrefectureStatus.AppointmentUnavailable;
        public ePrefectureStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        private IPrefectureRequestor _requestor = null;
        public IPrefectureRequestor Requestor
        {
            get { return _requestor; }
            set
            {
                _requestor = value;
                RaisePropertyChanged("Requestor");
            }
        }
    }
}
