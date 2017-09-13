using System.Web.Mvc;

namespace OnlineTSCS.Filters
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public bool Checked { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Checked && filterContext.HttpContext.Session["AccountName"] == null)
                filterContext.Result = new RedirectResult("~/AccountModels/Login");

            base.OnActionExecuting(filterContext);
        }
    }
}