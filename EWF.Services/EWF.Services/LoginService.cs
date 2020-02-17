using EWF.Entity;
using EWF.IRepository.SysManage;
using EWF.IServices;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Services
{
    public class LoginService: ILoginService
    {
        private ISYS_BACKUSERRepository backUserRepository;
        private ISYS_LOGINERROERepository loginErroeRepository;
        private ISYS_USERRepository userRepository;
        private ISYS_LOGINLOGRepository loginLogRepository;
        private ISYS_LOGINSUCCESSRepository loginSuccessRepository;
        private ISYS_ACCESSTIMERepository accessTimeRepository;
        private ISYS_ACCESSRECORDRepository accessRecordRepository;
        private ISYS_OBJECTUSERRELATIONRepository objectRepository;
        public LoginService(ISYS_BACKUSERRepository _backUserRepository, ISYS_LOGINERROERepository _loginErroeRepository, ISYS_USERRepository _userRepository, ISYS_LOGINLOGRepository _loginLogRepository,
            ISYS_LOGINSUCCESSRepository _loginSuccessRepository, ISYS_ACCESSTIMERepository _accessTimeRepository, ISYS_ACCESSRECORDRepository _accessRecordRepository, ISYS_OBJECTUSERRELATIONRepository _objectRepository)
        {
            backUserRepository = _backUserRepository;
            loginErroeRepository = _loginErroeRepository;
            userRepository = _userRepository;
            loginLogRepository = _loginLogRepository;
            loginSuccessRepository = _loginSuccessRepository;
            accessTimeRepository = _accessTimeRepository;
            accessRecordRepository = _accessRecordRepository;
            objectRepository = _objectRepository;
        }
        public dynamic GetBackUser(string ip)
        {
            return backUserRepository.GetBackUser(ip);
        }
        //登录判断
        public string LoginCheck(string username, string userpwd, string userip, string host, string path, out DataTable dtCheckUser, out string roleid)
        {
            string loginMsg = "";
            string ishassql = "";
            dtCheckUser = new DataTable();
            roleid = "";
            #region 登录之前的检查
            loginMsg = LoginCheck(userip, username, userpwd);
            if (!loginMsg.Contains("success"))
            {
                ishassql = loginMsg.Contains("非法字符") ? "1" : "0";
                LoginError(userip, username, loginMsg, host, path, ishassql);
                return loginMsg;
            }
            #endregion
            #region 判断是否登录超过5次
            DataTable dtLoginError = loginErroeRepository.GetLoginErrorCount(userip, username, DateTime.Now.AddMinutes(-30));// GetLoginErrorCount(userip, username, DateTime.Now.AddMinutes(-30));
            int loginErrorCount = dtLoginError.Rows[0]["lcount"].ToInt();
            if (loginErrorCount > 5)
            {
                LoginError(userip, username, "登陆失败：错误超过5次!", host, path, ishassql);
                loginMsg = "登陆失败：错误超过5次，请确保登录信息正确并等30分钟后重新登录！";
                return loginMsg;
            }
            #endregion
            #region 登录及登录后信息的验证
            dtCheckUser = userRepository.GetLoginCheck(username, ZEncypt.MD5(userpwd));// GetLoginCheck(username, ZEncypt.MD5(userpwd));
            if (dtCheckUser.Rows.Count == 0)
            {
                loginMsg = "登陆失败：用户名或密码错误！";
            }
            else
            {
                if (dtCheckUser.Rows[0]["ISENABLED"].ToInt() == 0)
                {
                    loginMsg = "登陆失败：当前账号未启用，请联系管理员！";
                }
                else
                    loginMsg = "success";
            }
            if (!loginMsg.Contains("success"))
            {
                //写错误日志
                LoginError(userip, username, loginMsg, host, path, ishassql);
                return loginMsg;
            }
            #endregion
            #region 登录后操作日志
            //LoginSuccess(dtCheckUser, userip, host, path);
            #endregion

            #region 统计登录时间
            string accessurl = host;// Request.RawUrl;//地址
            string Authority = path;// Request.Url.Authority;//系统
            var record = new SYS_ACCESSRECORD();
            var recordtime = new SYS_ACCESSTIME();
            //先得到上一次最后登录时间
            accessTimeRepository.UpdateLastAccessTime(dtCheckUser.Rows[0]["USERID"].ToString(), Authority); //UpdateLastAccessTime(dtCheckUser.Rows[0]["USERID"].ToString(), Authority);
            #endregion

            #region 登录记录
            //记录
            record.AID = Guid.NewGuid().ToString("N").ToUpper();
            record.UID = dtCheckUser.Rows[0]["USERID"].ToString();
            record.UNAME = dtCheckUser.Rows[0]["UACCOUNT"].ToString();
            record.APPIP = Authority;
            record.AURL = accessurl;
            record.ATIME = DateTime.Now;
            record.ATYPE = "1";
            accessRecordRepository.Insert<string>(record); //AccessRecordInsert(record);

            //统计
            recordtime.AID = Guid.NewGuid().ToString("N").ToUpper();
            recordtime.UID = dtCheckUser.Rows[0]["USERID"].ToString();
            recordtime.UNAME = dtCheckUser.Rows[0]["UACCOUNT"].ToString();
            recordtime.APPIP = Authority;
            recordtime.LoginTime = DateTime.Now;
            recordtime.LoginOutTime = DateTime.Now;
            recordtime.ATYPE = "0";
            accessTimeRepository.Insert<string>(recordtime); //AccessTimeInsert(recordtime);
            #endregion

            roleid = objectRepository.GetRoleListByUID(dtCheckUser.Rows[0]["USERID"].ToString());// GetRoleListByUID(dtCheckUser.Rows[0]["USERID"].ToString());

            var item = new SYS_USER();
            item = userRepository.Get(dtCheckUser.Rows[0]["USERID"].ToString());// GetUserByID(dtCheckUser.Rows[0]["USERID"].ToString());
            if (dtCheckUser.Rows[0]["FIRSTVISIT"].ToString() == null || dtCheckUser.Rows[0]["FIRSTVISIT"].ToString().IsEmpty())
                item.FIRSTVISIT = DateTime.Now;
            else
                item.FIRSTVISIT = Convert.ToDateTime(dtCheckUser.Rows[0]["FIRSTVISIT"]);
            if (dtCheckUser.Rows[0]["LASTVISIT"].ToString() == null || dtCheckUser.Rows[0]["LASTVISIT"].ToString().IsEmpty())
                item.LASTVISIT = DateTime.Now;
            else
                item.LASTVISIT = Convert.ToDateTime(dtCheckUser.Rows[0]["LASTVISIT"]);
            item.LOGONCOUNT = item.LOGONCOUNT + 1;
            userRepository.Update(item);//UpdateLoginUser(item);

            return loginMsg;
        }
        public string LoginCheck(string userip, string username, string password)
        {
            string loginMsg = "success";

            if (string.IsNullOrEmpty(username))
            {
                return "登陆失败：用户名不能为空！";
            }
            if (string.IsNullOrEmpty(password))
            {
                return "登陆失败：用户密码不能为空！";
            }
            if (username.Length > 20)
            {
                return "登陆失败：用户名超过限定长度！";
            }
            if (!FilterSql(username))
            {
                return "登陆失败：用户名含有非法字符！";
            }
            //if (userip == "127.0.0.1")
            //{
            //    return "登陆失败：用户网络异常(用户IP地址不合法)！";
            //}
            return loginMsg;
        }
        /// <summary> 
        /// 过滤非法关键字，这个可以按照项目灵活配置 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public bool FilterSql(string key)
        {
            bool flag = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    //一般配置在公共的文件中，如xml文件，txt文本等等 
                    string sqlStr = "insert|delete|select|update|exec|varchar|drop|creat|declare |truncate|cursor|begin|open|<--|-->|--|>|<";
                    sqlStr += "'|;|#|*|%|select|union|update|insert|delete|count|chr|mid|master|truncate|char|declare|@|exec|dbcc|alter|drop|create|backup|if|else|end|and|or|add|set|open|close|use|begin|retun|as|go|exists|<script";
                    string[] sqlStrArr = sqlStr.Split('|');
                    foreach (string strChild in sqlStrArr)
                    {
                        if (key.ToUpper().IndexOf(strChild.ToUpper()) != -1)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        ///验证失败或登录失败的操作
        /// </summary>
        public void LoginError(string userip, string username, string msg, string host, string path, string ishassql = "0")
        {
            #region 添加操作日志
            AddSystemLog("系统登录", "Login", "error", username + msg, "", username, userip, host, path);
            #endregion

            #region 添加登录失败日志
            var logionErr = new SYS_LOGINERROE();
            logionErr.LEID = Guid.NewGuid().ToString("N").ToUpper();
            logionErr.USERIP = userip;
            logionErr.USERNAME = username;
            logionErr.LOGINTIME = DateTime.Now;
            logionErr.ISHASSQL = ishassql;
            loginErroeRepository.Insert<string>(logionErr); //LoginErrorInsert(logionErr);
            #endregion

            #region 检查是否加入到黑名单
            //加入黑名单
            var backuser = new SYS_BACKUSER();
            backuser.USERIP = userip;
            backuser.USERNAME = username;
            //一年5次的SQL注入和一小时50次出错记录
            DataTable dtLoginErrorCount = loginErroeRepository.GetLoginByHasSQLCount(userip, username, DateTime.Now.AddYears(-1), "1");// GetLoginByHasSQLCount(userip, username, DateTime.Now.AddYears(-1), "1");
            int loginErrorCount = dtLoginErrorCount.Rows.Count > 0 ? dtLoginErrorCount.Rows[0]["lcount"].ToInt() : 0;
            if (loginErrorCount > 5)
            {
                backuser.BULAYER = "3";
                backuser.BUDATE = DateTime.Now;
                //加入黑名单
                backUserRepository.Insert<string>(backuser); //BackUserInsert(backuser);
            }
            loginErrorCount = loginErroeRepository.GetLoginErrorCount(userip, username, DateTime.Now.AddMinutes(-60)).Rows[0]["lcount"].ToInt();// GetLoginErrorCount(userip, username, DateTime.Now.AddMinutes(-60)).Rows[0]["lcount"].ToInt();
            if (loginErrorCount > 50)
            {
                backuser.BULAYER = "2";
                backuser.BUDATE = DateTime.Now;
                //加入黑名单
                backUserRepository.Insert<string>(backuser); //BackUserInsert(backuser);
            }
            #endregion
        }

        #region 操作日志
        public void AddSystemLog(string modular, string type, string result, string message, string uid, string username, string userip, string host, string path)
        {
            var model = new SYS_LOGINLOG();
            model.SLID = Guid.NewGuid().ToString("N").ToUpper();
            model.SLSYSTEMURL = host; //Request.Url.Authority;
            model.SLURL = path;// Request.RawUrl;
            model.SLMODULAR = modular;
            model.SLTYPE = type;
            model.SLRESULT = result == "success" ? 0 : 1;
            model.SLMESSAGE = message;

            model.SLOPERATOR = uid;
            model.SLOPERATORName = username;
            model.SLDATETIME = DateTime.Now;
            model.SLCREATEBY = uid;
            model.SLCREATEON = DateTime.Now;
            model.SLDELETECODE = 0;//默认
            model.SLUSERIP = userip;
            loginLogRepository.Insert<string>(model);//LoginLogInsert(model);
        }
        #endregion

        /// <summary>
        /// 登录成功的操作
        /// </summary>
        public void LoginSuccess(DataTable dt, string userip, string host, string path)
        {
            #region 添加操作日志
            AddSystemLog("系统登录", "Login", "success", dt.Rows[0]["UACCOUNT"].ToString() + "登录系统", dt.Rows[0]["USERID"].ToString(), dt.Rows[0]["REALNAME"].ToString(), userip, host, path);
            #endregion

            #region 添加登录成功记录
            var loginSuccess = new SYS_LOGINSUCCESS();
            loginSuccess.LSID = Guid.NewGuid().ToString("N").ToUpper();
            loginSuccess.USERIP = userip;
            loginSuccess.UID = dt.Rows[0]["USERID"].ToString();
            loginSuccess.USERNAME = dt.Rows[0]["UACCOUNT"].ToString();
            loginSuccess.LOGINTIME = DateTime.Now;
            loginSuccessRepository.Insert<string>(loginSuccess); //LoginSuccessInsert(loginSuccess);
            loginSuccessRepository.DeleteLoginSuccess(userip, dt.Rows[0]["UACCOUNT"].ToString(), DateTime.Now.AddDays(-1)); //DeleteLoginSuccess(userip, dt.Rows[0]["UACCOUNT"].ToString(), DateTime.Now.AddDays(-1));
            #endregion

            #region 检查是否加入到黑名单
            //加入黑名单
            var backuser = new SYS_BACKUSER();
            backuser.BUID = Guid.NewGuid().ToString("N").ToUpper();
            backuser.USERIP = userip;
            backuser.USERNAME = dt.Rows[0]["UACCOUNT"].ToString();
            //一分钟登录成功超过5次
            DataTable dtLoginSuccessCount = loginSuccessRepository.GetLoginSuccessCount(userip, dt.Rows[0]["UACCOUNT"].ToString(), DateTime.Now.AddMinutes(-1));// GetLoginSuccessCount(userip, dt.Rows[0]["UACCOUNT"].ToString(), DateTime.Now.AddMinutes(-1));
            int loginSuccessCount = dtLoginSuccessCount.Rows.Count > 0 ? dtLoginSuccessCount.Rows[0]["lcount"].ToInt() : 0;
            if (loginSuccessCount > 5)
            {
                backuser.BULAYER = "1";
                backuser.BUDATE = DateTime.Now;
                //如果黑名单中有记录，则直接拒绝访问
                var item = backUserRepository.GetBackUser(userip);// GetBackUser(userip);
                if (item != null && !String.IsNullOrEmpty(item.BUID))//在黑名单中找到当前IP
                    backuser.BULAYER = "2";
                //加入黑名单
                backUserRepository.Insert<string>(backuser); //BackUserInsert(backuser);
            }
            #endregion

            #region 删除登录失败日志
            loginErroeRepository.DeleteLoginError(userip, dt.Rows[0]["UACCOUNT"].ToString()); //DeleteLoginError(userip, dt.Rows[0]["UACCOUNT"].ToString());
            #endregion
        }
    }
}
