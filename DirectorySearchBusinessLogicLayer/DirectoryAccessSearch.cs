using DirectorySearchBusinessLogicLayer.gov.ny.svc.daws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySearchBusinessLogicLayer
{
    public class DirectoryAccessSearch
    {
        public String GetUser(String uid)
        {
            String user = "";

            BatchRequest batch = new BatchRequest();
            SearchRequest search = new SearchRequest();
            Filter filter = new Filter();
            dsmlQueryService client = new dsmlQueryService();
            client.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            batch.searchRequest = new SearchRequest[1] { search };
            client.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            search.dn = "'o=ny, c=us'";

            AttributeValueAssertion ava = new AttributeValueAssertion();
            ava.name = "uid";
            ava.value = "jjtester3";
            filter.ItemElementName = ItemChoiceType.equalityMatch;
            filter.Item = ava;
            search.filter = filter;
            search.scope = SearchRequestScope.wholeSubtree;


            //client.PreAuthenticate = true;
            //client.AllowAutoRedirect = true;





            BatchResponse response = null;
            try
            {
                //WebProxy myproxy = new WebProxy("proxy-internet.cio.state.nyenet", 80);
                //myproxy.BypassProxyOnLocal = false;
                //myproxy.Credentials = new NetworkCredential("mjordan", "fuckU023$6");
                //client.Proxy = myproxy;
                response = client.directoryRequest(batch);
                
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine("Dang it.  probably a 502 from the server and even more probable about async.  " + e);
            }
            System.Diagnostics.Debug.WriteLine("just sent the request for a batch seach to directory request");
            System.Diagnostics.Debug.WriteLine("Response: " + response);

            if (response != null)
            {
                SearchResponse[] sResponses = response.searchResponse;
                System.Diagnostics.Debug.WriteLine("Search Response: " + sResponses);
                if (sResponses != null)
                {
                    System.Diagnostics.Debug.WriteLine("Got " + sResponses.Length + " responses");
                    for (int i = 0; i < sResponses.Length; i++)
                    {
                        System.Diagnostics.Debug.WriteLine("Search Response #" + i + " requestID: " + sResponses[i].requestID);
                        SearchResultEntry[] srEntries = sResponses[i].searchResultEntry;
                        LDAPResult srd = sResponses[i].searchResultDone;
                        if (srd != null)
                        {
                            System.Diagnostics.Debug.WriteLine("LDAP Result AKA search result done");
                            System.Diagnostics.Debug.WriteLine(srd.resultCode.descr);
                        }
                        if (srEntries != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Search Result Entries Cycle");
                            for (int r = 0; r < srEntries.Length; r++)
                            {
                                System.Diagnostics.Debug.WriteLine(srEntries[r].dn);
                                System.Diagnostics.Debug.WriteLine(srEntries[r].attr);
                                DsmlAttr[] attributeList = srEntries[r].attr;
                                if (attributeList != null)
                                {
                                    for (int a = 0; a < attributeList.Length; a++)
                                    {
                                        System.Diagnostics.Debug.WriteLine("name: " + attributeList[a].name);
                                        for (int x = 0; x < attributeList[a].value.Length; x++)
                                        {
                                            System.Diagnostics.Debug.WriteLine("value: " + attributeList[a].value[a]);
                                            user = attributeList[a].value[a];
                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Search results list is null for some reason");
                        }
                    }
                }






                ErrorResponse[] eResponses = response.errorResponse;
                System.Diagnostics.Debug.WriteLine("Checking out errors from the batch response");
                System.Diagnostics.Debug.WriteLine("Errors: " + eResponses);
                //After adding a attribute value assertion and fitler to the search the error response ends up null so make a check for that
                if (eResponses != null)
                {
                    if (eResponses.Length > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Error Response");
                        for (int i = 0; i < eResponses.Length; i++)
                        {
                            ErrorResponse error = eResponses[i];
                            System.Diagnostics.Debug.WriteLine(error.message);
                            System.Diagnostics.Debug.WriteLine(error.detail);
                            System.Diagnostics.Debug.WriteLine(error.type);
                        }
                    }
                }
            }


            return user;

        }
    }
}
