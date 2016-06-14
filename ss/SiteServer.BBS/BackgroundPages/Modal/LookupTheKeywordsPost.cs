using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class LookupTheKeywordsPost : BackgroundBasePage
    {
        public Literal ltlTitle;

        protected Literal ltlThread;
        protected Literal ltlContent;
        private int id;
        private string type;

        public static string GetOpenWindowString(int publishmentSystemID, int id, string type)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            arguments.Add("Type", type);
            return JsUtils.OpenWindow.GetOpenWindowString("查看", PageUtils.GetBBSUrl("modal_lookupTheKeywordsPost.aspx"), arguments, 700, 600, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");
            this.type = base.GetQueryString("Type");

            if (!IsPostBack)
            {
                PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, this.id);
                this.ltlTitle.Text = postInfo.UserName + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;发表于&nbsp;&nbsp;&nbsp;" + postInfo.AddDate.ToString();
                ltlContent.Text = postInfo.Content;
                int threadID = postInfo.ThreadID;
                ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, threadID);
                ltlThread.Text = threadInfo.Title;
            }
        }
    }
}
