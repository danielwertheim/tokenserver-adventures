using System.Security.Claims;
using System.Web.Mvc;

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
    }
}