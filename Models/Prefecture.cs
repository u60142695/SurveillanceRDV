using SurveillanceRDV.Requestors;
using SurveillanceRDV.Utilities;
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

        public Prefecture(string id, string name, string redirectURL, IPrefectureRequestor requestor, int queryTime)
        {
            _id = id;
            _name = name;
            _redirectURL = redirectURL;
            _requestor = requestor;
            _requestor.Owner = this;

            _enabled = RegistryUtility.GetPrefectureQueryEnabled(_id);

            _queryTime = queryTime;
            _secondsUntilQuery = 0;
        }

        private string _id = "";
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
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

        private int _queryTime = 0;
        public int QueryTime
        {
            get { return _queryTime; }
            set
            {
                _queryTime = value;

                RegistryUtility.SetPrefectureQueryTime(this.Id, value);

                RaisePropertyChanged("QueryTime");
            }
        }

        private int _secondsUntilQuery = 0;
        public int SecondsUntilQuery
        {
            get { return _secondsUntilQuery; }
            set
            {
                _secondsUntilQuery = value;
                                
                RaisePropertyChanged("SecondsUntilQuery");
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

        private string _redirectURL = "";
        public string RedirectURL
        {
            get { return _redirectURL; }
            set
            {
                _redirectURL = value;
                RaisePropertyChanged("RedirectURL");
            }
        }

        private bool _enabled = false;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;

                if(value == false)
                {
                    Status = ePrefectureStatus.AppointmentUnavailable;
                }
                else
                {
                    SecondsUntilQuery = 0;
                }

                RegistryUtility.SetPrefectureQueryEnabled(this.Id, value);

                RaisePropertyChanged("Enabled");
            }
        }
    }
}
