using MVCSampleProject.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSampleProject.Controllers
{
    public class ClientBaseController : Controller
    {
        // GET: ClientBase
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null || session.isBlock == 1)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
            }
            else if (session.isBlock == 1)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Blocked", area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}