using SurveillanceRDV.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SurveillanceRDV.Requestors
{
    public class ParisPrefectureRequestor : IPrefectureRequestor
    {
        public string TargetURL { get; set; }
        
        public override bool? Request()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(TargetURL);

                var postData = "planning=949&nextButton=Etape+suivante";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.152 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.KeepAlive = true;
                request.Referer = TargetURL;
                request.Headers.Add("Cookie", "eZSESSID=pedkbehc25ghp083fb3kuva2o4; _ga=GA1.3.965872204.1612609943");
                request.Headers.Add("Sec-Fetch-Dest", "document");
                request.Headers.Add("Sec-Fetch-Mode", "navigate");
                request.Headers.Add("Sec-Fetch-Site", "same-origin");
                request.Headers.Add("Sec-Fetch-User", "?1");
                request.Headers.Add("Sec-GPC", "1");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.AllowAutoRedirect = true;

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
