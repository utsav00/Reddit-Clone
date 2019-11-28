using System.Web;
using System.Web.Mvc;

namespace Reddit_Clone
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    /*public class UserSessionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if(HttpContext.Current.Session["key"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }*/
}
