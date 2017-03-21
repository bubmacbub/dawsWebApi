using DirectorySearchBusinessLogicLayer.gov.ny.svc.daws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DirectoryServiceModel.Model;


/// <summary>
/// A web reference proxy for DAWS @ https://qadaws.svc.ny.gov/daws/dsmlQuery_v3.wsdl
/// </summary>
namespace DirectorySearchBusinessLogicLayer
{
    public class DirectoryAccessSearch
    {
        /*
         * first time running this by just changing the filter a little i got this message.
         * There was an error generating the XML document. ---> System.InvalidOperationException: 
         * Value of ItemElementName mismatches the type of DirectorySearchBusinessLogicLayer.gov.ny.svc.daws.SubstringFilter; 
         * you need to set it to DirectorySearchBusinessLogicLayer.gov.ny.svc.daws.ItemChoiceType.@substrings.
         * 
         * 
         * System.InvalidOperationException: There was an error generating the XML document. ---> 
         * System.InvalidOperationException: Value of ItemElementName mismatches the type of DirectorySearchBusinessLogicLayer.gov.ny.svc.daws.SubstringFilter; 
         * you need to set it to DirectorySearchBusinessLogicLayer.gov.ny.svc.daws.ItemChoiceType.@substrings.
         * 
         * 
         * The next time I changed out the ava filter for a substring type and got this message
         * Error Response
        [LDAP: error code 50 - Search filter not permitted (substring too short)]
         */
        public IEnumerable<NyGovUser> GetUsers(String ou)
        {
            GlobalProxySelection.Select = new WebProxy("127.0.0.1", 8888);
            List<NyGovUser> returnList = new List<NyGovUser>();
            //example OU ||||   dn =”ou = Department of General Services,ou = Government,o = ny,c = us
            BatchRequest batch = new BatchRequest();
            SearchRequest search = new SearchRequest();
            Filter filter = new Filter();
            dsmlQueryService client = new dsmlQueryService();
            client.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            batch.searchRequest = new SearchRequest[1] { search };
            client.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            search.dn = "'ou = Department of General Services,ou = Government,o = ny,c = us'";
            //search.dn = ou;

            //can't use attribute value assertion for substring choice.  instead make substring filter
            //AttributeValueAssertion ava = new AttributeValueAssertion();
            //ava.name = "nyacctgovernment";
            //ava.value = "Y";
            SubstringFilter[] substrings = new SubstringFilter[4];
            SubstringFilter substring = new SubstringFilter();
            
            substring.name = "nyacctgovernment";
            substring.initial = "Y";
            substrings[0] = substring;
            SubstringFilter substring1 = new SubstringFilter();
            substring1.name = "nyacctlevel1";
            substring1.initial = "Y";
            substrings[1] = substring1;
            SubstringFilter substring2 = new SubstringFilter();
            substring2.name = "sn";
            substring2.initial = "smith";
            substrings[2] = substring2;
            SubstringFilter substring3 = new SubstringFilter();
            substring3.name = "ou";
            substring3.initial = "Department of General Services";
            substrings[3] = substring3;
            //FilterSet fSet = new FilterSet();
            //ItemsChoiceType[] chioceTypes = new ItemsChoiceType[4];
            //fSet.ItemsElementName = chioceTypes;

            

            filter.ItemElementName = ItemChoiceType.substrings;
            filter.Item = substring2;
            search.filter = filter;
            search.scope = SearchRequestScope.wholeSubtree;

            AttributeDescriptions attrBucket = new AttributeDescriptions();
            AttributeDescription[] attributeDescriptionList = new AttributeDescription[7];
            attributeDescriptionList[0] = new AttributeDescription() { name = "nyacctgovernment" };
            attributeDescriptionList[1] = new AttributeDescription() { name = "sn" };
            attributeDescriptionList[2] = new AttributeDescription() { name = "givenname" };
            attributeDescriptionList[3] = new AttributeDescription() { name = "mail" };
            attributeDescriptionList[4] = new AttributeDescription() { name = "uid" };
            attributeDescriptionList[5] = new AttributeDescription() { name = "nyacctpersonal" };
            attributeDescriptionList[6] = new AttributeDescription() { name = "nyacctbusiness" };
            attrBucket.attribute = attributeDescriptionList;

            search.attributes = attrBucket;
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
                                NyGovUser user = new NyGovUser();
                                user.NysSogUid = srEntries[r].dn;
                                System.Diagnostics.Debug.WriteLine(srEntries[r].dn);
                                System.Diagnostics.Debug.WriteLine(srEntries[r].attr);
                                DsmlAttr[] attributeList = srEntries[r].attr;
                                if (attributeList != null)
                                {
                                    for (int a = 0; a < attributeList.Length; a++)
                                    {
                                        System.Diagnostics.Debug.WriteLine("name: " + attributeList[a].name);
                                        String attName = attributeList[a].name;
                                        StringBuilder valueBuilder = new StringBuilder();
                                        if (attributeList[a].value != null)
                                        {
                                            for (int x = 0; x < attributeList[a].value.Length; x++)
                                            {
                                                System.Diagnostics.Debug.WriteLine("value: " + attributeList[a].value[x]);
                                                valueBuilder.Append(attributeList[a].value[x]);
                                            }
                                        }

                                        if (attName.Equals("uid"))
                                        {
                                            user.Uid = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("cn"))
                                        {
                                            user.CommonName = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("nyacctgovernment"))
                                        {
                                            user.IsGovernmentAccount = Convert.ToBoolean(valueBuilder.ToString());
                                        }
                                        else if (attName.Equals("sn"))
                                        {
                                            user.Surname = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("givenname"))
                                        {
                                            user.Firstname = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("mail"))
                                        {
                                            user.EmailAddress = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("nyacctbusiness"))
                                        {
                                            user.IsBusinessPartnerAccount = Convert.ToBoolean(valueBuilder.ToString());
                                        }
                                        else if (attName.Equals("nyacctpersonal"))
                                        {
                                            user.IsCitizenAccount = Convert.ToBoolean(valueBuilder.ToString());
                                        }




                                    }
                                }
                                returnList.Add(user);
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Search results list is null for some reason");
                        }
                    }
                }






                ErrorResponse[] eResponses = response.errorResponse;

                if (eResponses != null)
                {
                    System.Diagnostics.Debug.WriteLine("Checking out errors from the batch response");
                    System.Diagnostics.Debug.WriteLine("Errors Count: " + eResponses.Length);
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
                else
                {
                    System.Diagnostics.Debug.WriteLine("No errors from the response");
                }
            }

            return returnList;
        }
        public NyGovUser GetUserByUid(String uid, String ou)
        {

            NyGovUser user = new NyGovUser();

            BatchRequest batch = new BatchRequest();
            SearchRequest search = new SearchRequest();
            Filter filter = new Filter();
            dsmlQueryService client = new dsmlQueryService();
            client.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            batch.searchRequest = new SearchRequest[1] { search };
            client.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");
            //search.dn = "'o=ny, c=us'";
            search.dn = ou;
            AttributeValueAssertion ava = new AttributeValueAssertion();
            ava.name = "uid";
            ava.value = uid;
            filter.ItemElementName = ItemChoiceType.equalityMatch;
            filter.Item = ava;
            search.filter = filter;
            search.scope = SearchRequestScope.wholeSubtree;

            AttributeDescriptions attrBucket = new AttributeDescriptions();
            AttributeDescription[] attributeDescriptionList = new AttributeDescription[7];
            attributeDescriptionList[0] = new AttributeDescription() { name= "nyacctgovernment" };
            attributeDescriptionList[1] = new AttributeDescription() { name = "sn" };
            attributeDescriptionList[2] = new AttributeDescription() { name = "givenname" };
            attributeDescriptionList[3] = new AttributeDescription() { name = "mail" };
            attributeDescriptionList[4] = new AttributeDescription() { name = "uid" };
            attributeDescriptionList[5] = new AttributeDescription() { name = "nyacctpersonal" };
            attributeDescriptionList[6] = new AttributeDescription() { name = "nyacctbusiness" };
            attrBucket.attribute = attributeDescriptionList;

            search.attributes = attrBucket;
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
                                user.NysSogUid = srEntries[r].dn;
                                System.Diagnostics.Debug.WriteLine(srEntries[r].dn);
                                System.Diagnostics.Debug.WriteLine(srEntries[r].attr);
                                DsmlAttr[] attributeList = srEntries[r].attr;
                                if (attributeList != null)
                                {
                                    for (int a = 0; a < attributeList.Length; a++)
                                    {
                                        System.Diagnostics.Debug.WriteLine("name: " + attributeList[a].name);
                                        String attName = attributeList[a].name;
                                        StringBuilder valueBuilder = new StringBuilder();
                                        if (attributeList[a].value != null)
                                        {
                                            for (int x = 0; x < attributeList[a].value.Length; x++)
                                            {
                                                System.Diagnostics.Debug.WriteLine("value: " + attributeList[a].value[x]);
                                                valueBuilder.Append(attributeList[a].value[x]);
                                            }
                                        }

                                        if (attName.Equals("uid"))
                                        {
                                            user.Uid = valueBuilder.ToString();
                                        }
                                        else if(attName.Equals("cn"))
                                        {
                                            user.CommonName = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("nyacctgovernment"))
                                        {
                                            user.IsGovernmentAccount = Convert.ToBoolean(valueBuilder.ToString());
                                        }
                                        else if (attName.Equals("sn"))
                                        {
                                            user.Surname = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("givenname"))
                                        {
                                            user.Firstname = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("mail"))
                                        {
                                            user.EmailAddress = valueBuilder.ToString();
                                        }
                                        else if (attName.Equals("nyacctbusiness"))
                                        {
                                            user.IsBusinessPartnerAccount= Convert.ToBoolean(valueBuilder.ToString());
                                        }
                                        else if (attName.Equals("nyacctpersonal"))
                                        {
                                            user.IsCitizenAccount= Convert.ToBoolean(valueBuilder.ToString());
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
                
                if (eResponses != null)
                {
                    System.Diagnostics.Debug.WriteLine("Checking out errors from the batch response");
                    System.Diagnostics.Debug.WriteLine("Errors Count: " + eResponses.Length);
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
                else
                {
                    System.Diagnostics.Debug.WriteLine("No errors from the response");
                }
            }


            return user;

        }

        /*
         * When I first made this method it ran correctly but I received an error message about access. as seen below
         * Logged User prxwstl1hesc cannot create in ou = people,ou = OFA,ou = Government,o = ny,c = us because user is not in OU scope of DA
         * 
         * Since this is a HESC account I will make a HESC user but i now need to find out the ou for that.  Since we make these apps for other people there might need to be a master level id we use or each agency specific work we do should have their own id
         * 
         */
        public void AddUser()
        {
            dsmlQueryService client = new dsmlQueryService();
            client.Url = "https://qadaws.svc.ny.gov/daws/services/dsmlSoapQuery";
            client.Credentials = new NetworkCredential("prxwsTL1HESC", "sfvwRMnB7N");

            AddRequest[] addRequest = new AddRequest[1] { new AddRequest() };
            addRequest[0].dn = "ou = people,ou =NYS Department of Higher Education Services Corporation,ou = Government,o = ny,c = us";
            DsmlAttr[] attributesToAdd = new DsmlAttr[11];
            attributesToAdd[0] = new DsmlAttr() { name = "dn", value = new String[]{ "ou = people,ou =NYS Department of Higher Education Services Corporation,ou = Government,o = ny,c = us" } };
            attributesToAdd[1] = new DsmlAttr() { name = "sn", value = new String[] { "Jordan" } };
            attributesToAdd[2] = new DsmlAttr() { name = "uid", value = new String[] { "MontyJordanEBSTest001" } };
            attributesToAdd[3] = new DsmlAttr() { name = "givenname", value = new String[] { "Monty" } };
            attributesToAdd[4] = new DsmlAttr() { name = "mail", value = new String[] { "monty.jordan@its.ny.gov" } };
            attributesToAdd[5] = new DsmlAttr() { name = "userpassword", value = new String[] { "uzRpa$$w0Rd" } };
            attributesToAdd[6] = new DsmlAttr() { name = "nyaccttl", value = new String[] { "1" } };
            attributesToAdd[7] = new DsmlAttr() { name = "nyaccttlidsource1", value = new String[] { "n/a" } };
            attributesToAdd[8] = new DsmlAttr() { name = "nyaccttlidsource2", value = new String[] { "n/a" } };
            attributesToAdd[9] = new DsmlAttr() { name = "nyaccttlivmethod", value = new String[] { "n/a" } };
            attributesToAdd[10] = new DsmlAttr() { name = "vetted", value = new String[] { "n/a" } };

            //attributesToAdd[9] = new DsmlAttr() { name = "", value = new String[] { "" } };

            addRequest[0].attr = attributesToAdd;
            BatchRequest batchRequest = new BatchRequest();
            batchRequest.addRequest = addRequest;
            BatchResponse batchResponse = client.directoryRequest(batchRequest);

            if (batchResponse != null)
            {
                LDAPResult[] ldapResult = batchResponse.addResponse;
                ErrorResponse[] errorResponse = batchResponse.errorResponse;

                if(errorResponse != null)
                {
                    for (int i = 0; i < errorResponse.Length; i++)
                    {
                        System.Diagnostics.Debug.WriteLine(errorResponse[i].message);
                    }
                }
            }


        }
    }
}
