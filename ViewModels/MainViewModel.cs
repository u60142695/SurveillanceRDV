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

            _statusText = "Application ready.";

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
            Prefectures.Add(new Prefecture("95 - Sarcelles", new GenericPrefectureRequestor() { TargetURL = @"http://www.val-doise.gouv.fr/booking/create/5260/0" }));

            // 2. Bobigny
            Prefectures.Add(new Prefecture("93 - Bobigny", new GenericPrefectureRequestor() { TargetURL = @"http://www.seine-saint-denis.gouv.fr/booking/create/9829/0" }));

            // 3. Le Raincy
            Prefectures.Add(new Prefecture("93 - Le Raincy", new GenericPrefectureRequestor() { TargetURL = @"http://www.seine-saint-denis.gouv.fr/booking/create/10317/0" }));

            // 4. Nanterre
            Prefectures.Add(new Prefecture("92 - Nanterre", new GenericPrefectureRequestor() { TargetURL = @"http://www.hauts-de-seine.gouv.fr/booking/create/12491" }));

            // 5. Creteil
            Prefectures.Add(new Prefecture("94 - Creteil", new CreteilPrefectureRequestor()));
        }

        private int _secondsUntilQuery = 0;

        private void PrefectureQueryRoutine()
        {
            while(true)
            {
                if(_secondsUntilQuery == 0)
                {
                    foreach (Prefecture pPrefecture in Prefectures)
                    {
                        pPrefecture.Status = ePrefectureStatus.Querying;

                        StatusText = "Interrogation de " + pPrefecture.Name;

                        bool? bResult = pPrefecture.Requestor.Request();
                        
                        pPrefecture.Status = bResult == null ? ePrefectureStatus.Error : (bResult == true ? ePrefectureStatus.AppointmentAvailable : ePrefectureStatus.AppointmentUnavailable);
                        
                        if(pPrefecture.Status == ePrefectureStatus.AppointmentAvailable)
                        {
                            SystemSounds.Asterisk.Play();
                        }
                    }

                    StatusText = "Interrogation terminee.";

                    _secondsUntilQuery = 90;
                }
                else
                {
                    _secondsUntilQuery--;

                    StatusText = "Prochaine interrogation dans " + _secondsUntilQuery + " sec ...";
                }

                Thread.Sleep(1000);
            }
        }
    }
}
