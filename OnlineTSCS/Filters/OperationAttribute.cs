using OnlineTSCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTSCS.Filters
{
    public class OperationAttribute : ActionFilterAttribute
    {
        private OTSCSModel db = new OTSCSModel();
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session["AccountName"] == null)
            {
                return;
            }
            LogModel logModel = new LogModel()
            {
                AccessDate = DateTime.Now,
                ActionAccount = filterContext.HttpContext.Session["AccountName"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionParameters = "无"
            };

            db.LogModels.Add(logModel);
            db.SaveChanges();

            base.OnResultExecuted(filterContext);
        }
    }
}