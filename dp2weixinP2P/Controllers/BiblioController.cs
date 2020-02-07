﻿using dp2weixin.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dp2weixinWeb.Controllers
{
    public class BiblioController : BaseController
    {
        /// <summary>
        /// 查看PDF
        /// </summary>
        /// <param name="libId"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(string libId, string uri)
        {
            ViewBag.libId = libId;
            ViewBag.objectUri = uri;

            string strError = "";
            string filename = "";
            int totalPage = dp2WeiXinService.Instance.GetPDFCount(libId, uri, out filename, out strError);
            ViewBag.pageCount = totalPage;

            string strImgUri = uri + "/page:1,format:jpeg,dpi:75";
            ViewBag.firstUrl = "../patron/getphoto?libId=" + HttpUtility.UrlEncode(libId)
                            + "&objectPath=" + HttpUtility.UrlEncode(strImgUri);
            return View();
        }

        // 书目查询主界面
        public ActionResult Index(string code, string state)
        {
            // 登录检查
            string strError = "";
            int nRet = 0;

            // 获取当前sessionInfo，里面有选择的图书馆和帐号等信息
            // -1 出错
            // 0 成功
            nRet = this.GetSessionInfo3(code, state,
                out SessionInfo sessionInfo,
                out strError);
            if (nRet == -1)
            {
                ViewBag.Error = strError;
                return View();
            }

            // 当前帐号不存在，尚未选择图书馆
            if (sessionInfo.ActiveUser == null)
            {
                ViewBag.RedirectInfo = dp2WeiXinService.GetSelLibLink(state, "/Biblio/Index?a=1");// ("书目查询", "/Biblio/Index?a=1", lib.libName);
                return View();
            }

            // 不允许外部访问，转到绑定帐号界面
            if (sessionInfo.CurrentLib.Entity.noShareBiblio == 1)
            {
                List<WxUserItem> users = WxUserDatabase.Current.Get(sessionInfo.WeixinId, sessionInfo.CurrentLib.Entity.id, -1);
                if (users.Count == 0)
                {
                    ViewBag.RedirectInfo = dp2WeiXinService.GetLinkHtml("书目查询", "/Biblio/Index?a=1", sessionInfo.CurrentLib.Entity.libName);
                    return View();
                }
            }

            // 为啥要给ViewBag设置证条码号？ 2020-2-7
            ViewBag.PatronBarcode = sessionInfo.ActiveUser.readerBarcode;

            // 检索匹配方式
            string match = sessionInfo.CurrentLib.Entity.match;
            if (String.IsNullOrEmpty(match) == true)
                match = "left";
            ViewBag.Match = match;

            return View();
        }

        // 书目查询详细界面
        public ActionResult Detail(string code, string state, string biblioPath)
        {
            string strError = "";
            int nRet = 0;

            // 获取当前sessionInfo，里面有选择的图书馆和帐号等信息
            // -1 出错
            // 0 成功
            nRet = this.GetSessionInfo3(code, state,
                out SessionInfo sessionInfo,
                out strError);
            if (nRet == -1)
            {
                ViewBag.Error = strError;
                return View();
            }

            // 当前帐号不存在，尚未选择图书馆
            if (sessionInfo.ActiveUser == null)
            {
                ViewBag.RedirectInfo = dp2WeiXinService.GetSelLibLink(state, "/Biblio/Detail");
                return View();
            }

            ViewBag.PatronBarcode = sessionInfo.ActiveUser.readerBarcode;
            ViewBag.BiblioPath = biblioPath;
            return View();
        }

    }
}