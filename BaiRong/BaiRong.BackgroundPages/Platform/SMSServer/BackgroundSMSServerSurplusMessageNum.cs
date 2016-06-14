using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

using BaiRong.Core;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServerSurplusMessageNum : BackgroundBasePage
    {
        public PlaceHolder phTotalCount;
        public Literal ltlTotalCount;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "剩余短信条数", AppManager.Platform.Permission.platform_SMS);


                if (!SMSServerManager.IsEnabled)
                {
                    base.FailMessage("无法查询记录，请先设置短信服务商");
                }
                else
                {
                    int totalCount = 0;
                    string errorMessage = string.Empty;
                    bool isSuccess = SMSServerManager.GetLastSmsCount(out totalCount, out errorMessage);
                    if (isSuccess)
                    {
                        this.phTotalCount.Visible = true;
                        this.ltlTotalCount.Text = totalCount.ToString();
                    }
                    else
                    {
                        base.FailMessage(errorMessage + " 请检查短信服务商设置！");
                    }
                }
            }
        }
    }
}
