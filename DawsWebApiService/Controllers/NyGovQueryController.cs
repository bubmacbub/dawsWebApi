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
        [HttpGet]
        public IHttpActionResult GetUser(String uid, String ou)
        {
            DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            NyGovUser user = search.GetUserByUid(uid, ou);


            return Ok(user);
        }

        [HttpGet]
        public IHttpActionResult GetUsers(String ou)
        {
            //DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            //IEnumerable<NyGovUser> users = search.GetUsers(ou);


            //return Ok(users);

            DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface search = new DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface();
            search.GetUsers(); 

            return Ok();
        }


        [HttpPost]
        public IHttpActionResult AddUser()
        {
            DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            search.AddUser();


            return Ok("success");
        }



    }
}
