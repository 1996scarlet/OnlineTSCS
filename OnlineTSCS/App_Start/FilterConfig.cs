using OnlineTSCS.Filters;
using System.Web;
using System.Web.Mvc;

namespace OnlineTSCS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticationAttribute() { Checked = true });
        }
    }
}
