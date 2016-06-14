using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages.Modal {

    public class ThreadDigest : BackgroundBasePage
    {
        protected RadioButtonList rpDigest;
        protected PlaceHolder phCheck;
        protected TextBox txtDigestDate;

        string threadIDList = "";

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("主题精华", PageUtils.GetBBSUrl("modal_threadDigest.aspx"), arguments, "ThreadIDCollection", "请选择需要精华的主题", 450, 350);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("ThreadIDCollection") == null)
            {
                base.FailMessage("参数出错！");
                return;
            }
            threadIDList = base.GetQueryString("ThreadIDCollection");
            
            if (!IsPostBack)
            {
                
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            int digest = TranslateUtils.ToInt(rpDigest.SelectedValue);
            DateTime digestDate = DateTime.Now;
            if (digest > 0)
            {
                int dates = TranslateUtils.ToInt(txtDigestDate.Text, 0);
                if (dates <= 0)
                    dates = 3;
                digestDate = digestDate.AddDays(dates);
            }
            try
            {
                DataProvider.ThreadDAO.DigestThread(threadIDList, digestDate, digest);
                StringUtilityBBS.AddLog(base.PublishmentSystemID, "主题精华成功", string.Format("主题IDList:{0}", threadIDList));
                isChanged = true;
            }
            catch (Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundThread.GetRedirectUrl(base.PublishmentSystemID));
            }
        }

        public void Digest_SelectedIndexChanged(object sender, EventArgs e)
        {

            int targetForumID = TranslateUtils.ToInt(rpDigest.SelectedValue);
            if (targetForumID <= 0)
                this.phCheck.Visible = false;
            else
                this.phCheck.Visible = true;
        }
    }
} 