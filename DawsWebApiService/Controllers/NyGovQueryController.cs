using DawsWebApiService.dawsSoap;
using DirectoryServiceModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace DawsWebApiService.Controllers
{
    public class NyGovQueryController : ApiController
    {

        /// <summary>
        /// Looks up a particular user from NYSD's DAWS
        /// </summary>
        /// <param name="id">returns json</param>
        [HttpGet]
        public IHttpActionResult GetUser(String uid, String ou)
        {
            DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            NyGovUser user = search.GetUserByUid(uid, ou);


            return Ok(user);
        }

        /// <summary>
        /// Uses service reference to query an OU with additional filters
        /// </summary>
        /// <param name="ou"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUsers(String ou)
        {
            //DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            //IEnumerable<NyGovUser> users = search.GetUsers(ou);


            //return Ok(users);

            DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface search = new DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface();
             

            return Ok(search.GetUsers());
        }


        /// <summary>
        /// According to the permission of the user and the OU being inserted into will create a DAWS record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUser()
        {
            //DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            //search.AddUser();
            DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface search = new DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface();

            return Ok(search.AddUser());
        }



    }
}
