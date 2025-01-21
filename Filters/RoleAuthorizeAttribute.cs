using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HotelManagement.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string RequiredRole { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var role = httpContext.Session["AdminRole"] as string;
            return role == RequiredRole || role == "SuperAdmin"; // SuperAdmin has all access
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Admin" },
                { "action", "Login" }
            });
        }
    }
}
