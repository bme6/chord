﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Configuration;
using dp2Command.Service;
using DigitalPlatform.IO;
using dp2Command.Server;
using DigitalPlatform.Text;
using dp2weixin;

namespace dp2weixinP2P
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 从web config中取出mongoDb地址
            string mongoDbConnStr = WebConfigurationManager.AppSettings["mongoDbConnStr"];
            if (String.IsNullOrEmpty(mongoDbConnStr) == true)
            {
                throw new Exception("尚未配置mongodb连接字符串");
            }
            string instancePrefix = WebConfigurationManager.AppSettings["instancePrefix"];
            LibDatabase.Current.Open(mongoDbConnStr, instancePrefix);

            // 从web config中取出mserver服务器地址，微信自己的账号
            string dp2MServerUrl = WebConfigurationManager.AppSettings["dp2MServerUrl"];
            string userName = WebConfigurationManager.AppSettings["userName"];
            string password = WebConfigurationManager.AppSettings["password"];
            if (string.IsNullOrEmpty(password) == false)// 解密
                password = Cryptography.Decrypt(password, WeiXinClientUtil.EncryptKey);

            // 错误日志目录
            string weiXinLogDir = WebConfigurationManager.AppSettings["weiXinLogDir"];
            PathUtil.CreateDirIfNeed(weiXinLogDir);	// 确保目录创建

            string weiXinUrl = WebConfigurationManager.AppSettings["weiXinUrl"];

            // 初始化命令服务类
            dp2CmdService2.Instance.Init(dp2MServerUrl,
                userName,
                password,
                weiXinUrl,
                weiXinLogDir,
                mongoDbConnStr,
                instancePrefix);
        }
    }
}