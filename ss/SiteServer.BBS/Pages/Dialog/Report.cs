using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using System.Web;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BBS.Model;

using  System.Web.UI.HtmlControls;
namespace SiteServer.BBS.Pages.Dialog
{
    public class Report : BasePage
    {
        protected int publishmentSystemID;
        protected int forumID;
        protected int threadID;
        protected int postID;
        public Button btnSubmit;
        public HtmlTextArea txtCon;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public static string GetOpenWindowStringByReportPost(int publishmentSystemID, int forumID, int threadID, int postID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("postID", postID.ToString());

            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "Report.aspx"), arguments, 350, 300, "举报帖子", string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemID = base.GetIntQueryString("publishmentSystemID");
                this.forumID = base.GetIntQueryString("forumID");
                this.threadID = base.GetIntQueryString("threadID");
                this.postID = base.GetIntQueryString("postID");
            }
        }
    }
}
