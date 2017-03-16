using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace DawsWebApiService.Controllers
{
    public class NyGovQueryController : ApiController
    {
        public IHttpActionResult GetUser(String uid)
        {
            String user = "mjordan";



            //dawsSoap.dsmlQueryService daws = new dawsSoap.dsmlQueryService();
            //daws.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            //daws.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            //dawsSoap.BatchRequest batch = new dawsSoap.BatchRequest();

            //dawsSoap.SearchRequest search = new dawsSoap.SearchRequest();
            //search.dn = "o=ny,c=us";
            //search.scope = dawsSoap.SearchRequestScope.wholeSubtree;
            //search.derefAliases = dawsSoap.SearchRequestDerefAliases.neverDerefAliases;

            //dawsSoap.Filter filter = new dawsSoap.Filter();
            //object equalityMatchName = "uid";
            //filter.ItemElementName = dawsSoap.ItemChoiceType.equalityMatch;
            //filter.Item = equalityMatchName;
            //dawsSoap.AttributeValueAssertion ava = new dawsSoap.AttributeValueAssertion();
            //ava.name = "uid";
            //ava.value = "jjtester3";

            //dawsSoap.AttributeDescription uidAttr = new dawsSoap.AttributeDescription();
            //uidAttr.name = "uid";
            //dawsSoap.AttributeDescriptions attributes = new dawsSoap.AttributeDescriptions();
            //attributes.attribute = new dawsSoap.AttributeDescription[1] { uidAttr};
            //search.attributes = attributes;


            //batch.searchRequest = new dawsSoap.SearchRequest[1] { search };
            //dawsSoap.BatchResponse response =    daws.directoryRequest(batch);

            //dawsSoap.ErrorResponse[] eResponses = response.errorResponse;
            //dawsSoap.SearchResponse[] sResponses = response.searchResponse;
            //if(eResponses.Length > 0)
            //{
            //    System.Diagnostics.Debug.WriteLine("Poop");
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Search Response");
            //    dawsSoap.SearchResultEntry sre = sResponses[0].searchResultEntry[0];
            //}













            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery");
            ////WebProxy myproxy = new WebProxy("proxy-internet.cio.state.nyenet", 80);
            ////myproxy.BypassProxyOnLocal = false;
            //request.Method = "POST";
            ////request.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            //request.Headers.Add("SOAPAction", "");
            //request.ContentType = "text/xml;charset=\"utf-8\"";
            //request.Accept = "text/xml";
            //request.Method = "POST";
            //System.Diagnostics.Debug.WriteLine("Making soap env");
            //XmlDocument soapEnvelopeXml = CreateSoapEnv();
            //System.Diagnostics.Debug.WriteLine("inserting soap into request");
            //InsertSoapEnvIntoWebReq(soapEnvelopeXml, request);
            //System.Diagnostics.Debug.WriteLine("Making an async result");
            //System.Diagnostics.Debug.WriteLine("BEGIN get response");
            //IAsyncResult asyncResult = null;
            //try
            //{
            //    asyncResult = request.BeginGetResponse(null, null);
            //}
            //catch(Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine("doing async wait");
            //}

            //if(asyncResult != null)
            //{
            //    // suspend this thread until call is complete. You might want to
            //    // do something usefull here like update your UI.
            //    asyncResult.AsyncWaitHandle.WaitOne();
            //    System.Diagnostics.Debug.WriteLine("doing async wait");

            //}

            //try
            //{
            //    System.Diagnostics.Debug.WriteLine("End get response");
            //    using (WebResponse stream = request.EndGetResponse(asyncResult))
            //    {

            //        using (StreamReader stRead = new StreamReader(stream.GetResponseStream()))
            //        {
            //            System.Diagnostics.Debug.WriteLine("Reading stream for result");
            //            String result = stRead.ReadToEnd();
            //            System.Diagnostics.Debug.WriteLine("Result: " + result);
            //            user = result;
            //        }


            //        //using (WebResponse response = request.GetResponse())
            //        //{
            //        //    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            //        //    {
            //        //        string soapResult = rd.ReadToEnd();
            //        //        user = soapResult;
            //        //        System.Diagnostics.Debug.WriteLine(soapResult);
            //        //    }
            //        //}
            //    }
            //}
            //catch (WebException ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("issues with that damn request stream again");
            //    string message = ((HttpWebResponse)ex.Response).StatusDescription;
            //    System.Diagnostics.Debug.WriteLine(message);
            //}






            return Ok(user);
        }



        public static void CallWebServ()
        {
            var _url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            var _action = "";

            XmlDocument soapEnvelopeXml = CreateSoapEnv();
            HttpWebRequest webRequest = CreateWebReq(_url, _action);
            InsertSoapEnvIntoWebReq(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            //endGetResponse throws unauthorized if no credentials put on request
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                System.Diagnostics.Debug.WriteLine("Got web response");
                try
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        System.Diagnostics.Debug.WriteLine("Got stream reader from response");
                        soapResult = rd.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(soapResult);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("get response stream poop" + e);
                }

            }
        }

        private static HttpWebRequest CreateWebReq(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            return webRequest;
        }

        private static XmlDocument CreateSoapEnv()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"
<soap-env:Envelope
xmlns:xsd='http://www.w3.org/2001/XMLSchema'
xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
xmlns:soap-env='http://schemas.xmlsoap.org/soap/envelope/'>
<soap-env:Body>
<batchRequest xmlns='urn:oasis:names:tc:DSML:2:0:core'>
            <searchRequest dn='o = ny, c = us' scope='wholeSubtree' derefAliases='neverDerefAliases' timeLimit='0' sizeLimit='0'>
            	<filter>
            		<substrings name = 'mail'>
<initial>mark.mossman@its.ny.gov</initial>
</substrings>
</filter>
<attributes>
<attribute name = 'uid'/>
<attribute name = 'sn'/>
</attributes>
</searchRequest>
</batchRequest>
</soap-env:Body></soap-env:Envelope>
");
            return soapEnvelop;
        }

        private static void InsertSoapEnvIntoWebReq(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("get response stream poop:  " + e);
            }
        }
    }
}
