using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorySearchBusinessLogicLayer.gov.ny.ds.daws;
using System.Net;

namespace DirectorySearchBusinessLogicLayer.Service
{
    public class DirectoryAccessServiceInterface
    {
        public directoryRequestResponse GetUsers()
        {
            directoryRequestResponse returnThis = null;
            System.Diagnostics.Debug.WriteLine("Going to get users with the service reference proxy");
            //fiddler
            GlobalProxySelection.Select = new WebProxy("127.0.0.1", 8888);

            dsmlSoapClient client = new dsmlSoapClient();
            BatchRequest batchRequest = new BatchRequest();
            SearchRequest searchRequest = new SearchRequest();
            client.ClientCredentials.UserName.UserName ="prxwsTL1HESC";
            client.ClientCredentials.UserName.Password ="sfvwRMnB7N";
            //client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerOrChainTrust;
            //batchRequest.processing = BatchRequestProcessing.sequential;
            //DsmlMessage[] messages = new DsmlMessage[1];
            //Control[] cntrls = new Control[1];
            //Control control = new Control();
            //control.controlValue = searchRequest;
            //cntrls[0] = control;
            //DsmlMessage message = new DsmlMessage();
            //message.control = cntrls;
            //messages[0] = message;
            //batchRequest.Items = messages;
            //added these dsml message types and when i tried to send them thru directory request method got exception about 
            //communication exception
            //{"The type DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.DsmlMessage was not expected. Use the XmlInclude or SoapInclude attribute to specify types that are not known statically."}

            //figured out that search request extends dsml message and so does the other request types, did this by looking at the generated code then right clicking on service ref and configuring.  
            //next window i unselected reuse types and it made new types in the auto gen object

            //had to change the web config bindings to pick up the credentials for WCF
            /*
             <basicHttpBinding>
                <binding name="BasicHttpBinding_IService1">
                  <security mode="TransportCredentialOnly">
                    <transport clientCredentialType="Basic"/>
                  </security>
                </binding>
            </basicHttpBinding>
             * 
             */

            batchRequest.Items = new SearchRequest[] { searchRequest };
            directoryRequestRequest request = new directoryRequestRequest();
            request.batchRequest = batchRequest;
            //basic set up till now.  This just gets us talking to the daws server with connectivity that is ok.  now business logic about the correct dn and necessary search filters
            searchRequest.dn = "ou=People,ou=NYS Department of Taxation and Finance Business Partners,ou=Business,o=ny,c=us";
            Filter filter = new Filter();


            //there can only be one substring in WCF but you can send in multiple in a direct soap envelope
            SubstringFilter[] substrings = new SubstringFilter[2];
            //SubstringFilter substring = new SubstringFilter();
            //substring.name = "nyacctgovernment";
            //substring.initial = "Y";
            //substrings[0] = substring;
            //SubstringFilter substring1 = new SubstringFilter();
            //substring1.name = "nyacctlevel1";
            //substring1.initial = "Y";
            //substrings[1] = substring1;
            SubstringFilter substring2 = new SubstringFilter();
            substring2.name = "sn";
            substring2.initial = "smith";
            substrings[0] = substring2;
            //SubstringFilter substring3 = new SubstringFilter();
            //substring3.name = "ou";
            //substring3.initial = "Department of General Services";
            //substrings[3] = substring3;
            SubstringFilter substring4 = new SubstringFilter();
            substring4.name = "mail";
            substring4.initial = "tax_test@hotmail.com";
            substrings[1] = substring4;

            SubstringFilter anyFilter = new SubstringFilter();
            anyFilter.any = new String[]{ "nyacctgovernment=Y", "nyacctlevel1=Y", "sn=smith"};
            //this any filter produced this soap env
            /*
             * <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
             * <batchRequest xmlns="urn:oasis:names:tc:DSML:2:0:core">
             * <searchRequest dn="ou=People,ou=NYS Department of Taxation and Finance Business Partners,ou=Business,o=ny,c=us" scope="wholeSubtree" derefAliases="neverDerefAliases">
             *  <filter>
                    <substrings>
                        <any>nyacctgovernment=Y</any>
                        <any>nyacctlevel1=Y</any>
                        <any>sn=smith</any>
                    </substrings></filter></searchRequest></batchRequest></s:Body></s:Envelope>
             * 
             */
            //so going to make a fitlerset instead ..or atleast try
            //first attempt yielded this result
            //System.InvalidOperationException: There was an error generating the XML document. --->
            //System.InvalidOperationException: Invalid or missing value of the choice identifier 'ItemsElementName' of type 'DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.ItemsChoiceType1[]'.
            //second attempt after adding items element names to the andFilterSet got this
            //There was an error generating the XML document. ---> System.InvalidOperationException: 
            //Invalid or missing value of the choice identifier 'ItemsElementName' of type 'DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.ItemsChoiceType1[]'.
            //using andFilterSet.ItemsElementName = new ItemsChoiceType1[]{ ItemsChoiceType1.substrings, ItemsChoiceType1.substrings, ItemsChoiceType1.substrings};
            //i initialized the substringfilter array to 4 elements and took one out so i had to even them up to match each other.  itemchoices to filter objects
            //then got this soap env
            /*
             <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
<s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><batchRequest xmlns="urn:oasis:names:tc:DSML:2:0:core"><searchRequest dn="ou=People,ou=NYS Department of Taxation and Finance Business Partners,ou=Business,o=ny,c=us" scope="wholeSubtree" derefAliases="neverDerefAliases">
<filter>
	<and>
		<substrings name="nyacctgovernment"><initial>Y</initial></substrings>
		<substrings name="nyacctlevel1"><initial>Y</initial></substrings>
		<substrings name="sn"><initial>smith</initial></substrings>
	</and>
</filter></searchRequest></batchRequest></s:Body></s:Envelope>
             */
            //message is weird b/c if I remove two substring elements I no longer get the msg:
            //[LDAP: error code 50 - Search filter not permitted (substring too short)]

            //just used this in postman and it worked after some messing around with the results to find a possible list of people as a result
            /*
              <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
<s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><batchRequest xmlns="urn:oasis:names:tc:DSML:2:0:core"><searchRequest dn="ou=People,ou=NYS Department of Taxation and Finance Business Partners,ou=Business,o=ny,c=us" scope="wholeSubtree" derefAliases="neverDerefAliases">
<filter>
	<and>
		<substrings name="sn"><initial>smith</initial></substrings>
		<substrings name="mail"><initial>tax_test@hotmail.com</initial></substrings>
	</and>
</filter>
<attributes>
	<attribute name="nyacctgovernment"/>
	<attribute name="sn"/>
	<attribute name="givenname"/>
	<attribute name="mail"/>
	<attribute name="uid"/>
	<attribute name="nyacctpersonal"/>
	<attribute name="nyacctbusiness"/>
</attributes>
</searchRequest></batchRequest></s:Body></s:Envelope>
             */
            //now and requests work but got this message
            //ommunicationException: The maximum message size quota for incoming messages (65536) has been exceeded. To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element. ---> 
            //System.ServiceModel.QuotaExceededException: The maximum message size quota for incoming messages (65536) has been exceeded. 
            //To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element.  so I'm going to not take so many attributes back


            FilterSet andFilterSet = new FilterSet();
            andFilterSet.Items = substrings;
            andFilterSet.ItemsElementName = new ItemsChoiceType1[]{ ItemsChoiceType1.substrings, ItemsChoiceType1.substrings};

            filter.Item = andFilterSet;
            filter.ItemElementName = ItemChoiceType.and;
            searchRequest.filter = filter;
            searchRequest.scope = SearchRequestScope.wholeSubtree;
            //{"Value of ItemElementName mismatches the type of 
            //DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.SubstringFilter; you need to set it to DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.ItemChoiceType.@substrings."}
            //System.InvalidOperationException: Value of ItemElementName mismatches the type of DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.SubstringFilter; you need to set it to DirectorySearchBusinessLogicLayer.gov.ny.ds.daws.ItemChoiceType.@substrings.




            


            //THE CALL
            try
            {
                directoryRequestResponse response = client.directoryRequest(request);
                System.Diagnostics.Debug.WriteLine("Response: " + response);
                //The error handling changed from a web reference to a service reference.  guess the wsdl is more generic than i thought.
                ErrorResponse[] eResponses = null;
                BatchResponse bResponse = response.batchResponse;
                Object[] responseItems = bResponse.Items;
                if (responseItems != null)
                {
                    if (responseItems[0] is ErrorResponse)
                    {
                        ErrorResponse eResponse = (ErrorResponse)responseItems[0];
                        System.Diagnostics.Debug.WriteLine(eResponse.message);
                        System.Diagnostics.Debug.WriteLine(eResponse.detail);
                        System.Diagnostics.Debug.WriteLine(eResponse.type);

                    }
                    else
                    {
                        returnThis = response;
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Dang it: " + e);
            }



            




            //if (eResponses != null)
            //{
            //    System.Diagnostics.Debug.WriteLine("Checking out errors from the batch response");
            //    System.Diagnostics.Debug.WriteLine("Errors Count: " + eResponses.Length);
            //    //After adding a attribute value assertion and fitler to the search the error response ends up null so make a check for that
            //    if (eResponses != null)
            //    {
            //        if (eResponses.Length > 0)
            //        {
            //            System.Diagnostics.Debug.WriteLine("Error Response");
            //            for (int i = 0; i < eResponses.Length; i++)
            //            {
            //                ErrorResponse error = eResponses[i];
            //                System.Diagnostics.Debug.WriteLine(error.message);
            //                System.Diagnostics.Debug.WriteLine(error.detail);
            //                System.Diagnostics.Debug.WriteLine(error.type);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("No errors from the response");
            //}




            System.Diagnostics.Debug.WriteLine("Finished getting users with the service reference proxy");
            return returnThis;
        }
    }
}
