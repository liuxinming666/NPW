using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EWF.Application.Web.Controllers
{
    public class EWFBaseController : Controller
    {
        //过滤器，验证用户是否登录，cookie是否存在
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var route = filterContext.RouteData.Values;
            var url = string.Format("/{0}/{1}/{2}", route["area"], route["controller"], route["action"]);
            //用户没有登录
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                //刷新当前页面localhost: 18293 / Home / Index,如果没有cookie直接跳转到登录页面，其他页面提示没有权限
                //if (!url.EndsWith("/Index"))
                //{
                //    filterContext.Result = new ContentResult() { Content = "你没有访问此功能的权限，请联系管理员！" };
                //}
                
                if (url.EndsWith("/GetTopMenu") || url.EndsWith("/GetUserMenuByParentCode"))
                {
                    filterContext.Result = new ContentResult() { Content = "登录信息已过期，请刷新当前页面重新登录！" };
                }
                else
                    base.OnActionExecuting(filterContext);
            }
            base.OnActionExecuting(filterContext);
        }
        protected virtual IActionResult Success(string message)
        {
            return Json(new AjaxResult { state = ResultType.success.ToString(), message = message });
        }
        protected virtual IActionResult Success(object data)
        {
            return Json(new AjaxResult { state = ResultType.success.ToString(), message = "", data = data });
        }
        protected virtual IActionResult Success(string message, object data)
        {
            return Json(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data });
        }
        protected virtual IActionResult Error(string message)
        {
            return Json(new AjaxResult { state = ResultType.error.ToString(), message = message });
        }
        
    }
}