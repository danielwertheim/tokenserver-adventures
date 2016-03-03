using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SecuredApi.Controllers
{
    /// <summary>
    /// Persons.
    /// </summary>
    [RoutePrefix("persons")]
    [Authorize]
    public class PersonsController : ApiController
    {
        /// <summary>
        /// Returns some persons.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Things can go wrong</remarks>
        [Route]
        [HttpGet]
        [ResponseType(typeof(Person[]))]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new[]
            {
                new Person
                {
                    Name = "Joe the man",
                    Score = 1000
                }
            });
        }
    }

    /// <summary>
    /// A person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Score
        /// </summary>
        public int Score { get; set; }
    }
}