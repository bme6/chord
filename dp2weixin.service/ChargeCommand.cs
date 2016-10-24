﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp2weixin.service
{
    public class ChargeCommand
    {

        // 命令常量
        public const string C_Command_LoadPatron = "loadPatron";
        public const string C_Command_Borrow = "borrow";
        public const string C_Command_Return = "return";
        public const string C_Command_Renew = "renew";
        public const string C_Command_VerifyRenew = "verifyrenew";
        public const string C_Command_Read = "read";
        public const string C_Command_VerifyReturn = "verifyreturn";

        public int id { get; set; }
        public string type { get; set; }
        public string typeString { get; set; }
        public string patron { get; set; } //传进来的值，不一定全是barcode，有可能姓名与二维码
        public string item { get; set; }//传进来的值，不一定全是barcode，有可能isbn
        public string description { get; set; }
        public string operTime { get; set; }

        public string patronBarcode { get; set; }
        public string itemBarcode { get; set; }

        public string cmdCss
        {
            get
            {
                if (state == -1)
                    return "cmderror";
                if (state == 0)
                    return "cmdsuccess";
                if (state == 1)
                    return "cmdwarning";

                // 其它
                return "cmdwarning";
            }
        }

        // 处理状态
        public int state { get; set; }
        public string resultInfo { get; set; }

        // 是否需要划横线，不同读者间画线
        public int isAddLine = 0;
        public string itemBarcodeUrl { get; set; }
        public static string getTypeString(string type)
        {
            if (type == C_Command_LoadPatron)
                return "装载";
            if (type == C_Command_Borrow)
                return "借";
            if (type == C_Command_Return)
                return "还";
            if (type == C_Command_Renew)
                return "续借";
            if (type == C_Command_VerifyRenew)
                return "验证续借";
            if (type == C_Command_Read)
                return "读过";
            return "验证还";
        }

        // 本接口同时返回读者信息
        public string patronHtml { get; set; }

        public string cmdHtml { get; set; }

    }
}
