using EWF.Util.Log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util
{
    public class GlobalExceptions : IExceptionFilter
    {
        private readonly LoggerHelper logger;
        private readonly IHostingEnvironment env;
        private IHttpContextAccessor accessor;
        public GlobalExceptions(IHostingEnvironment _env, LoggerHelper _logger, IHttpContextAccessor _accessor)
        {
            env = _env;
            logger = _logger;
            accessor = _accessor;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                if (env.IsDevelopment())
                {
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
                }
                context.Result = new BadRequestObjectResult(json);//返回异常数据
            }
            else
            {
                json.Message = "发生了未知内部错误";
                if (env.IsDevelopment())
                {
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
                }
                
            }

            //日志帮助类记录日志
            logger.Exception(context.Exception);


            var ajaxResult = new AjaxResult
            {
                state = ResultType.error.ToString(),
                message = context.Exception.Message
            };

            if (accessor.HttpContext.Request.IsAjax())
            {
                context.Result = new ContentResult()
                {

                    StatusCode = 200,
                    ContentType = "application/json",
                    Content = ajaxResult.ToJson()
                };
            }
            else
            {
                if (accessor.HttpContext.Request.Method.ToUpper() == "POST")
                {
                    context.Result = new ContentResult()
                    {

                        StatusCode = 200,
                        ContentType = "application/json",
                        Content = ajaxResult.ToJson()
                    };
                }
                else
                {
                    context.Result = new InternalServerErrorObjectResult(json);
                }
            }
        }
    }
}
