using System;
using System.Web.Http;

namespace DawsWebApiService.Controllers
{
    /// <summary>
    /// Service for looking up DAWS users in different ways.  Finds are http gets.  Post is create.  <i>so and on and so on.</i>
    /// </summary>
    public class NyGovQueryController : ApiController
    {

        /// <summary>
        /// Looks up a particular user from NYSD's DAWS
        /// </summary>
        /// <param name="id">returns json</param>
        [HttpGet]
        public IHttpActionResult GetUser(String uid, String ou)
        {
            //DirectorySearchBusinessLogicLayer.DirectoryAccessSearch search = new DirectorySearchBusinessLogicLayer.DirectoryAccessSearch();
            //NyGovUser user = search.GetUserByUid(uid, ou);
            DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface search = new DirectorySearchBusinessLogicLayer.Service.DirectoryAccessServiceInterface();

            return Ok(search.findUser(uid,ou));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        //[HttpPut("{id}")]
        //public IHttpActionResult Update(long id, [FromBody]  item)
        //{

        //    }

        }
}
