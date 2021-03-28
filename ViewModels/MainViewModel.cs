using SurveillanceRDV.Models;
using SurveillanceRDV.Requestors;
using SurveillanceRDV.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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

            PrefectureConfiguration.LoadConfiguration();

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

            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
            //ServicePointManager.SecurityProtocol = 
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        ~MainViewModel()
        {

        }

        private void PopulatePrefectures()
        {
            // 1. Sarcelles
            Prefectures.Add(new Prefecture("95_SARCELLES",
                                           "95 - Sarcelles",
                                           @"https://www.val-doise.gouv.fr/booking/create/5260/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://www.val-doise.gouv.fr/booking/create/5260/0" }));

            // 2. Bobigny
            Prefectures.Add(new Prefecture("93_BOBIGNY",
                                           "93 - Bobigny",
                                           @"https://www.seine-saint-denis.gouv.fr/booking/create/9829/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://www.seine-saint-denis.gouv.fr/booking/create/9829/0" }));

            // 3. Le Raincy
            Prefectures.Add(new Prefecture("93_RAINCY",
                                           "93 - Le Raincy",
                                           @"https://www.seine-saint-denis.gouv.fr/booking/create/10317/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://www.seine-saint-denis.gouv.fr/booking/create/10317/0" }));

            // 4. Nanterre
            Prefectures.Add(new Prefecture("92_NANTERRE",
                                           "92 - Nanterre", 
                                           @"https://www.hauts-de-seine.gouv.fr/booking/create/12491/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://www.hauts-de-seine.gouv.fr/booking/create/12491" }));

            // 5. Paris - VPF13
            Prefectures.Add(new Prefecture("75_VPF13",
                                           "75 - VPF-13",
                                           @"https://pprdv.interieur.gouv.fr/booking/create/948/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://pprdv.interieur.gouv.fr/booking/create/948/0" }));

            // 6. Paris - VPF17
            Prefectures.Add(new Prefecture("75_VPF17",
                                           "75 - VPF-17",
                                           @"https://pprdv.interieur.gouv.fr/booking/create/953/1",
                                           new GenericPrefectureRequestor() { TargetURL = @"https://pprdv.interieur.gouv.fr/booking/create/953/0" }));

            // 7. Paris - SAL13
            Prefectures.Add(new Prefecture("75_SAL13",
                                           "75 - SAL-13",
                                           @"https://pprdv.interieur.gouv.fr/booking/create/885/1",
                                           new ParisPrefectureRequestor() { TargetURL = @"https://pprdv.interieur.gouv.fr/booking/create/885/1" }));

            // 8. Paris - SAL17
            Prefectures.Add(new Prefecture("75_SAL17",
                                           "75 - SAL-17",
                                           @"https://pprdv.interieur.gouv.fr/booking/create/876/1",
                                           new ParisPrefectureRequestor() { TargetURL = @"https://pprdv.interieur.gouv.fr/booking/create/876/1" }));

            // 9. Paris - ALGR
            Prefectures.Add(new Prefecture("75_ALGR",
                                           "75 - ALGR",
                                           @"https://pprdv.interieur.gouv.fr/booking/create/957/1",
                                           new ParisPrefectureRequestor() { TargetURL = @"https://pprdv.interieur.gouv.fr/booking/create/957/1" }));

            // 5. Creteil
            /*Prefectures.Add(new Prefecture("94_CRETEIL",
                                           "94 - Creteil",
                                           @"https://rdv-etrangers-94.interieur.gouv.fr/eAppointmentpref94/element/jsp/specific/pref94.jsp",
                                           new CreteilPrefectureRequestor(),
                                           75));*/
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
                                StatusLogger.WriteEvent(pPrefecture.Id);

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
