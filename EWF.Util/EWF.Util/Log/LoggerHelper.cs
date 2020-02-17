using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace EWF.Util.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LoggerHelper
    {
        private ILogger logger;
        public LoggerHelper(ILogger<LoggerHelper> _loger)
        {
            this.logger = _loger;
        }
        public void Debug(object message)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendLine("内容: " + message.ToString() );
            strInfo.AppendLine("--------------------------------------------");
            
            this.logger.LogDebug(strInfo.ToString());
        }
        public void Info(object message)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendLine("内容: " + message.ToString());
            strInfo.AppendLine("--------------------------------------------");

            this.logger.LogInformation(strInfo.ToString());
        }
        public void Warn(object message)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendLine("内容: " + message.ToString());
            strInfo.AppendLine("--------------------------------------------");

            this.logger.LogWarning(strInfo.ToString());
        }
        public void Error(object message)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendLine("内容: " + message.ToString());
            strInfo.AppendLine("--------------------------------------------");

            this.logger.LogError(strInfo.ToString());
        }
       
        public void Exception(Exception exception)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendLine("信息: " + exception.Message);
            strInfo.AppendLine("堆栈: " + exception.StackTrace);
            strInfo.AppendLine("内部: " + exception.InnerException);
            strInfo.AppendLine("源: " + exception.Source);
            strInfo.AppendLine("--------------------------------------------");

            this.logger.LogError(strInfo.ToString());
        }
    }
}
