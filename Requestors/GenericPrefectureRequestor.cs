using SurveillanceRDV.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SurveillanceRDV.Requestors
{
    public class GenericPrefectureRequestor : IPrefectureRequestor
    {
        public string TargetURL { get; set; }
        
        public override bool? Request()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(TargetURL);

                var postData = "condition=on&nextButton=Effectuer une demande de rendez-vous";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                //request.KeepAlive = true;
                request.Referer = TargetURL;
                request.Headers.Add("Cookie", "xtvrn=$481979$; xtan481979=-; xtant481979=1; eZSESSID=3lv900erb8gllecqk65ekt1dt2");

                request.Timeout = 20000; // 20 sec timeout.

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Console.WriteLine(responseString);

                return !responseString.Contains("Il n'existe plus de plage horaire libre pour votre demande de rendez-vous");
            }
            catch(Exception ex)
            {
                // Log this exception.
                //MainViewModel.Instance.ErrorMessages.Add(ex.Message);
                Console.WriteLine(DateTime.Now.ToString() + " - Erreur sur " + Owner.Name + ": " + ex.ToString());

                return null;
            }
        }
    }
}
