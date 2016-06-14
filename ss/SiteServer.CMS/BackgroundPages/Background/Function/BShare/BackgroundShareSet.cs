using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.Core;
namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundShareSet : BackgroundBasePage
    {
        public Label txtName;
        public Label txtUuid;
        public Literal ltlScriptView;
        public TextBox bscode;
        public string uuid;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_BShare, "插件设置", AppManager.CMS.Permission.WebSite.BShare);

                uuid = base.PublishmentSystemInfo.Additional.BshareUUID;
                
                string scriptSrc = "<a class='bshareDiv' href='http://www.bshare.cn/share'>分享按钮</a><script language='javascript' type='text/javascript' src='http://static.bshare.cn/b/buttonLite.js#uuid=" + uuid + "&amp;style=2&amp;textcolor=#000&amp;bgcolor=none&amp;bp=sinaminiblog,qzone,renren,kaixin001&amp;ssc=false&amp;sn=true&amp;text=分享到'></script>";
                this.txtName.Text = base.PublishmentSystemInfo.Additional.BshareUserName;
                this.txtUuid.Text = base.PublishmentSystemInfo.Additional.BshareUUID;
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.BshareJs))
                {
                    this.bscode.Text = base.PublishmentSystemInfo.Additional.BshareJs;
                    this.ltlScriptView.Text = base.PublishmentSystemInfo.Additional.BshareJs;

                }
                else
                {
                    base.PublishmentSystemInfo.Additional.BshareJs=scriptSrc;
                    this.bscode.Text = scriptSrc;
                    this.ltlScriptView.Text = scriptSrc;
                    
                }
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                if (string.IsNullOrEmpty( base.PublishmentSystemInfo.Additional.BshareUUID))
                {
                   string PublishmentSystemID= Request["PublishmentSystemID"];
                   Response.Redirect(PageUtils.GetCMSUrl("background_shareRegister.aspx?PublishmentSystemID=" + PublishmentSystemID));
                }
               


            }
        }
        public void btnSetStyle_Click(object sender, EventArgs E)
        {
            this.ltlScriptView.Text = bscode.Text;
            this.ClientScript.RegisterStartupScript(this.GetType(),"", "<script language='javascript'>alert('样式保存成功')</script>");
            base.PublishmentSystemInfo.Additional.BshareJs = bscode.Text;
            DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
        }
      
    }
}
