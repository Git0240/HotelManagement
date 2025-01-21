using System.Web;
using System.Web.Mvc;

namespace HotelManagement.Filters
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["AdminLoggedIn"] != null && (bool)httpContext.Session["AdminLoggedIn"];
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Admin/Login");
        }
    }
}
