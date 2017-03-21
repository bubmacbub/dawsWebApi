using DirectorySearchBusinessLogicLayer.gov.ny.ds.daws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySearchBusinessLogicLayer.Service
{
    /// <summary>
    /// This class will just create service reference objects
    /// </summary>
    public sealed class ProxyFactory
    {
        private static readonly Lazy<ProxyFactory> lazy =
                new Lazy<ProxyFactory>(() => new ProxyFactory(), true); // true for thread safety

            public static ProxyFactory Instance { get { return lazy.Value; } }

            private ProxyFactory()
            {
            }

        public directoryRequestRequest createDirReq(BatchRequestTypes batchRequestType)
        {
            directoryRequestRequest request = new directoryRequestRequest();
            BatchRequest batchRequest = new BatchRequest();
            request.batchRequest = batchRequest;
            switch (batchRequestType)
            {
                case BatchRequestTypes.searchRequest:
                    {
                        SearchRequest searchRequest = new SearchRequest();
                        batchRequest.Items = new SearchRequest[] { searchRequest };
                        
                        break;
                    }
                case BatchRequestTypes.addRequest:
                    {
                        AddRequest addRequest = new AddRequest();
                        batchRequest.Items = new AddRequest[] { addRequest };
                        break;
                    }
                case BatchRequestTypes.modifyRequest:
                    {
                        ModifyRequest modifyRequest = new ModifyRequest();
                        batchRequest.Items = new ModifyRequest[] { modifyRequest };
                        break;
                    }
                case BatchRequestTypes.delRequest:
                    {
                        DelRequest delRequest = new DelRequest();
                        batchRequest.Items = new DelRequest[] { delRequest };
                        break;
                    }
                default:
                    break;
            }
            return request;
        }

        public dsmlSoapClient createClient()
        {
            dsmlSoapClient client = new dsmlSoapClient();
            client.ClientCredentials.UserName.UserName = "prxWSTL2OFTmedia";
            client.ClientCredentials.UserName.Password = "3RywE4?w";
            return client;
        }

        public enum BatchRequestTypes
        {
            searchRequest, addRequest, modifyRequest, delRequest
        }
    }
 
}
