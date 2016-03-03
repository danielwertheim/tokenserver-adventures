using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Shared;

namespace MvcClient.Controllers
{
    [MvcAuthorizeAttribute]
    public class ClaimsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [HttpGet]
        public async Task<ActionResult> Remote()
        {
            var token = ((ClaimsPrincipal)User).Claims.Single(c => c.Type == "token").Value;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = await client.GetStringAsync(EndPointConstants.SecuredApi + "info").ConfigureAwait(false);
            var claims = JArray.Parse(json).Select(j => new Claim(j.SelectToken("type").ToString(), j.SelectToken("value").ToString())).ToArray();

            return View("Index", claims);
        }
    }
}