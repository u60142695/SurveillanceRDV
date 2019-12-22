using SurveillanceRDV.Models;
using SurveillanceRDV.Requestors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SurveillanceRDV.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if(propertyName != "")
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Prefecture> _prefectures = new ObservableCollection<Prefecture>();
        public ObservableCollection<Prefecture> Prefectures
        {
            get { return _prefectures; }
            set
            {
                _prefectures = value;
                RaisePropertyChanged("Prefectures");
            }
        }

        private string _statusText = "";
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                RaisePropertyChanged("StatusText");
            }
        }

        private ObservableCollection<string> _errorMessages = new ObservableCollection<string>();
        public ObservableCollection<string> ErrorMessages
        {
            get { return _errorMessages; }
            set
            {
                _errorMessages = value;
                RaisePropertyChanged("ErrorMessages");
            }
        }

        private Thread _prefectureQueryRoutine = null;

        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance { get { return _instance; } }

        public MainViewModel()
        {
            AllocConsole();

            _statusText = "Logiciel pret.";

            PopulatePrefectures();
            
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                _prefectureQueryRoutine = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(2500);
                    PrefectureQueryRoutine();
                }));

                _prefectureQueryRoutine.Start();
            }
        }

        ~MainViewModel()
        {

        }

        private void PopulatePrefectures()
        {
            // 1. Sarcelles
            Prefectures.Add(new Prefecture("95_SARCELLES",
                                           "95 - Sarcelles", 
                                           @"http://www.val-doise.gouv.fr/booking/create/5260/0", 
                                           new GenericPrefectureRequestor() { TargetURL = @"http://www.val-doise.gouv.fr/booking/create/5260/0" },
                                           61));

            // 2. Bobigny
            Prefectures.Add(new Prefecture("93_BOBIGNY",
                                           "93 - Bobigny", 
                                           @"http://www.seine-saint-denis.gouv.fr/booking/create/9829/0",
                                           new GenericPrefectureRequestor() { TargetURL = @"http://www.seine-saint-denis.gouv.fr/booking/create/9829/0" },
                                           62));

            // 3. Le Raincy
            Prefectures.Add(new Prefecture("93_RAINCY",
                                           "93 - Le Raincy",
                                           @"http://www.seine-saint-denis.gouv.fr/booking/create/10317/0",
                                           new GenericPrefectureRequestor() { TargetURL = @"http://www.seine-saint-denis.gouv.fr/booking/create/10317/0" },
                                           63));

            // 4. Nanterre
            Prefectures.Add(new Prefecture("92_NANTERRE",
                                           "92 - Nanterre", 
                                           @"http://www.hauts-de-seine.gouv.fr/booking/create/12491",
                                           new GenericPrefectureRequestor() { TargetURL = @"http://www.hauts-de-seine.gouv.fr/booking/create/12491" },
                                           64));

            // 5. Creteil
            Prefectures.Add(new Prefecture("94_CRETEIL",
                                           "94 - Creteil",
                                           @"https://rdv-etrangers-94.interieur.gouv.fr/eAppointmentpref94/element/jsp/specific/pref94.jsp",
                                           new CreteilPrefectureRequestor(),
                                           65));
        }
        
        private void PrefectureQueryRoutine()
        {
            while(true)
            {
                foreach (Prefecture pPrefecture in Prefectures)
                {
                    if (pPrefecture.Enabled)
                    {
                        if (pPrefecture.SecondsUntilQuery == 0)
                        {
                            pPrefecture.Status = ePrefectureStatus.Querying;

                            StatusText = "Interrogation de " + pPrefecture.Name;

                            bool? bResult = pPrefecture.Requestor.Request();

                            pPrefecture.Status = bResult == null ? ePrefectureStatus.Error : (bResult == true ? ePrefectureStatus.AppointmentAvailable : ePrefectureStatus.AppointmentUnavailable);

                            if (pPrefecture.Status == ePrefectureStatus.AppointmentAvailable)
                            {
                                SystemSounds.Asterisk.Play();
                            }

                            pPrefecture.SecondsUntilQuery = pPrefecture.QueryTime;
                        }
                        else
                        {
                            pPrefecture.SecondsUntilQuery--;
                        }
                    }

                    StatusText = "Inactif.";
                }
                
                Thread.Sleep(1000);
            }
        }
    }
}
