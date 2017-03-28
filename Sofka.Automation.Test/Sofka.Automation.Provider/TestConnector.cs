using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.Provider
{
    public class TestConnector
    {

        public static HttpWebResponse getHttpWebRequest(string url, string usrname, string pwd, string method, string contentType,
                                       string[] headers, string body)
        {
            // Variables.
            HttpWebRequest Request;
            HttpWebResponse Response;
            //
            string strSrcURI = url.Trim();
            string strBody = body.Trim();
            try
            {
                // Create the HttpWebRequest object.
                Request = (HttpWebRequest)HttpWebRequest.Create(strSrcURI);

                // Add the network credentials to the request.
                Request.Credentials = new NetworkCredential(usrname.Trim(), pwd);

                // Specify the method.
                Request.Method = method.Trim();

                // request headers
                foreach (string s in headers)
                {
                    Request.Headers.Add(s);
                }

                // Set the content type header.
                Request.ContentType = contentType.Trim();

                // set the body of the request...
                Request.ContentLength = body.Length;
                using (Stream reqStream = Request.GetRequestStream())
                {
                    // Write the string to the destination as a text file.
                    reqStream.Write(Encoding.UTF8.GetBytes(body), 0, body.Length);
                    reqStream.Close();
                }

                // Send the method request and get the response from the server.
                Response = (HttpWebResponse)Request.GetResponse();

                // return the response to be handled by calling method...
                return Response;
            }
            catch (Exception e)
            {
                throw new Exception("Web API error: " + e.Message, e);
            }
        }
}
