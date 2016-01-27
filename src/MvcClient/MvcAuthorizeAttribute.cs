using System.Web.Mvc;

namespace MvcClient
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //To prevent infinite redirect loop
            filterContext.Result = filterContext.HttpContext.User.Identity.IsAuthenticated
                ? new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden)
                : new HttpUnauthorizedResult();
        }
    }
}