using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace SurveillanceRDV.Requestors
{
    public class CreteilPrefectureRequestor : IPrefectureRequestor
    {
        public override bool? Request()
        {
            string ajaxURL = @"https://rdv-etrangers-94.interieur.gouv.fr/eAppointmentpref94/dwr/call/plaincall/AjaxMotive.motiveMultiSiteSubmit.dwr";

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                var request = (HttpWebRequest)WebRequest.Create(ajaxURL);

                var postData = "callCount=1\npage=/eAppointmentpref94/element/jsp/appointment.jsp\nhttpSessionId=&scriptSessionId=D984FF2B4ACE2B438C6B3E7AE51DD0BF749\nc0-scriptName=AjaxMotive\nc0-methodName=motiveMultiSiteSubmit\nc0-id=0\nc0-param0=boolean:false\nc0-e1=number:1\nc0-param1=Object_Object:{70:reference:c0-e1}\nbatchId=2";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Accept = "*/*";
                request.Headers.Add("AcceptEncoding", "gzip, deflate, br");
                request.Headers.Add("AcceptLanguage", "en-US,en;q=0.9,es;q=0.8");
                request.KeepAlive = true;
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.ContentLength = data.Length;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";
                request.Referer = @"https://rdv-etrangers-94.interieur.gouv.fr/eAppointmentpref94/element/jsp/appointment.jsp";
                request.Headers.Add("Cookie", "JSESSIONID=2FB4321F69FB7EAC142E7A1E34B23771.worker_gfa1; visid_incap_651915=OcvsYOkgTmmdbVDrb3W+wbxm1lwAAAAAQUIPAAAAAAD9pd39BHYqNO4gsyOf9zX+; ID_ROUTE_GFA=.worker_gfa1");
                request.Headers.Add("Origin", "https://rdv-etrangers-94.interieur.gouv.fr");

                request.Timeout = 10000; // 10 sec timeout.

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Console.WriteLine(DateTime.Now.ToString() + " - Creteil Response: " + responseString);

                //return !responseString.Contains("Aucun rendez-vous");

                return false;
            }
            catch (Exception ex)
            {
                // Log this exception.
                //MainViewModel.Instance.ErrorMessages.Add(ex.Message);
                Console.WriteLine(DateTime.Now.ToString() + " - Erreur sur " + Owner.Name + ": " + ex.Message);

                return null;
            }
        }
    }
}
