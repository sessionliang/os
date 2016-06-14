using SiteServer.CMS.Core;
using System;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReferenceAdd : MLibBackgroundBasePage
    {
        public TextBox tbRTName;
        public void Page_Load(object sender, EventArgs E)
        {

        }
        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                var rtName = Request["tbRTName"];
                if (rtName.Length > 0)
                {
                    var ds = DataProvider.MlibDAO.GetReferenceTypeList("RTName='" + rtName + "'");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        base.FailMessage("引用方已添加!");
                    }
                    else
                    {
                        DataProvider.MlibDAO.InsertReferenceType(rtName);
                        Response.Write("<script language=\"javascript\">window.parent.location.href='ReferenceSet.aspx';window.parent.closeWindow();</script>");
                    }
                }
                else
                {
                    base.FailMessage("引用方名称不能为空!");
                }
            }
        }
    }
}
