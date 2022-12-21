using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSampleProject.Filters
{
    public class AdminAuthorization : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["Loggedin"] == null && filterContext.HttpContext.Session["Role"].ToString() == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}