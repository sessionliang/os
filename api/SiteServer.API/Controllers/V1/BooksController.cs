using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
namespace SiteServer.API.Controllers
{
    [RoutePrefix("api/v1/books")]
    public class BooksController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("~/api/v1/authors/{authorId:int}/books")]
        [HttpGet]
        public IEnumerable<string> GetByAuthor(int authorId) {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
