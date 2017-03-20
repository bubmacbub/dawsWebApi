

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace DawsWebApiService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //            DawsBusinessLogicLayer.Service.DawsBusinessCoordinator coordinator = new DawsBusinessCoordinator();
            DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            ViewBag.Title = "Home";
            //search.GetUserByUid("jjtester3");

            //GlobalProxySelection.Select = new WebProxy("127.0.0.1", 8888);
            //BatchRequest batch = new BatchRequest();
            //SearchRequest search = new SearchRequest();
            //daws.svc.ny.gov.Filter filter = new daws.svc.ny.gov.Filter();
            ////            dsmlQueryService client = new dsmlQueryService();
            ////daws.svc.ny.gov.dsmlSoapClient client = new dsmlSoapClient();
            //dsmlSoapClient client = new dsmlSoapClient();
            ////client.Endpoint = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            ////batch.searchRequest = new SearchRequest[1] { search };

            //client.ClientCredentials.UserName.UserName = "prxwsTL1HESC";
            //client.ClientCredentials.UserName.Password = "sfvwRMnB7N";
            //search.dn = "'o=ny, c=us'";

            //AttributeValueAssertion ava = new AttributeValueAssertion();
            //ava.name = "uid";
            //ava.value = "jjtester3";
            //filter.ItemElementName = ItemChoiceType.equalityMatch;
            //filter.Item = ava;
            //search.filter = filter;
            //search.scope = SearchRequestScope.wholeSubtree;


            ////client.PreAuthenticate = true;
            ////client.AllowAutoRedirect = true;

            //daws.svc.ny.gov.AuthRequest authReq = new AuthRequest();
            //authReq.principal = "";
            //batch.Items = new SearchRequest[1] { search };


            //BatchResponse response = null;
            //try
            //{
            //    //WebProxy myproxy = new WebProxy("proxy-internet.cio.state.nyenet", 80);
            //    //myproxy.BypassProxyOnLocal = false;
            //    //myproxy.Credentials = new NetworkCredential("mjordan", "fuckU023$6");
            //    //client.Proxy = myproxy;

            //    response = client.directoryRequest(batch);
            //    //Thread.Sleep(10000);
            //}
            //catch (Exception e)
            //{

            //    System.Diagnostics.Debug.WriteLine("Dang it.  probably a 502 from the server and even more probable about async.  " + e);
            //}
            //System.Diagnostics.Debug.WriteLine("just sent the request for a batch seach to directory request");
            //System.Diagnostics.Debug.WriteLine("Response: " + response);

            //if (response != null)
            //{
            //    Object[] objects = response.Items;
            //    if (objects is SearchResponse[])
            //    {
            //        SearchResponse[] sResponses = (SearchResponse[])objects;



            //        System.Diagnostics.Debug.WriteLine("Search Response: " + sResponses);
            //        if (sResponses != null)
            //        {
            //            System.Diagnostics.Debug.WriteLine("Got " + sResponses.Length + " responses");
            //            for (int i = 0; i < sResponses.Length; i++)
            //            {
            //                System.Diagnostics.Debug.WriteLine("Search Response #" + i + " requestID: " + sResponses[i].requestID);
            //                SearchResultEntry[] srEntries = sResponses[i].searchResultEntry;
            //                LDAPResult srd = sResponses[i].searchResultDone;
            //                if (srd != null)
            //                {
            //                    System.Diagnostics.Debug.WriteLine("LDAP Result AKA search result done");
            //                    System.Diagnostics.Debug.WriteLine(srd.resultCode.descr);
            //                }
            //                if (srEntries != null)
            //                {
            //                    System.Diagnostics.Debug.WriteLine("Search Result Entries Cycle");
            //                    for (int r = 0; r < srEntries.Length; r++)
            //                    {
            //                        System.Diagnostics.Debug.WriteLine(srEntries[r].dn);
            //                        System.Diagnostics.Debug.WriteLine(srEntries[r].attr);
            //                        DsmlAttr[] attributeList = srEntries[r].attr;
            //                        if (attributeList != null)
            //                        {
            //                            for (int a = 0; a < attributeList.Length; a++)
            //                            {
            //                                System.Diagnostics.Debug.WriteLine("name: " + attributeList[a].name);
            //                                for (int x = 0; x < attributeList[a].value.Length; x++)
            //                                {
            //                                    System.Diagnostics.Debug.WriteLine("value: " + attributeList[a].value[a]);
            //                                    ViewBag.Title = attributeList[a].value[a];
            //                                }


            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    System.Diagnostics.Debug.WriteLine("Search results list is null for some reason");
            //                }
            //            }
            //        }
            //    }
            //    else if (objects is ErrorResponse[])
            //    {
            //        ErrorResponse[] eResponses = (ErrorResponse[]) objects;
            //        System.Diagnostics.Debug.WriteLine("Checking out errors from the batch response");
            //        System.Diagnostics.Debug.WriteLine("Errors: " + eResponses);
            //        //After adding a attribute value assertion and fitler to the search the error response ends up null so make a check for that
            //        if (eResponses != null)
            //        {
            //            if (eResponses.Length > 0)
            //            {
            //                System.Diagnostics.Debug.WriteLine("Error Response");
            //                for (int i = 0; i < eResponses.Length; i++)
            //                {
            //                    ErrorResponse error = eResponses[i];
            //                    System.Diagnostics.Debug.WriteLine(error.message);
            //                    System.Diagnostics.Debug.WriteLine(error.detail);
            //                    System.Diagnostics.Debug.WriteLine(error.type);
            //                }
            //            }
            //        }
            //    }







            //}








            return View();
        }
    }
}
