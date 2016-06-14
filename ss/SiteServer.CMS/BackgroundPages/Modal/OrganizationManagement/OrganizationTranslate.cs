using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class OrganizationTranslate : BackgroundBasePage
    {
        public DropDownList ParentItemID;
        public DropDownList ddlAreaID;

        private string returnUrl;
        private ArrayList contentIDArrayList;
        private string classifyID;

        public static string GetRedirectString(int publishmentSystemID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("分支机构转移", "modal_organizationTranslate.aspx", arguments, "ContentIDCollection", "请选择需要转移的内容！", 400, 320);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ClassifyID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
            this.classifyID = base.GetQueryStringNoSqlAndXss("ClassifyID");

            if (!IsPostBack)
            {
                TreeManager.AddListItems(this.ParentItemID.Items, base.PublishmentSystemID, 0, true, false, "OrganizationClassify");
                ControlUtils.SelectListItems(this.ParentItemID, this.classifyID); 
            }
        }

        public void ParentItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlAreaID.Items.Clear();
            int theItemID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
            TreeManager.AddListItemsByClassify(this.ddlAreaID.Items, base.PublishmentSystemID, theItemID, true, true, "OrganizationArea", theItemID, 0);
            if (this.ddlAreaID.Items.Count > 0)
                this.ddlAreaID.SelectedIndex = 0;
        }


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (this.ParentItemID.SelectedIndex == 0)
            {
                base.FailMessage("内容转移失败，请选择分类！");
                return;
            }
            if (this.ddlAreaID.SelectedIndex == -1)
            {
                base.FailMessage("内容转移失败，请选择区域！");
                return;
            }
            DataProvider.OrganizationInfoDAO.TranslateContent(this.contentIDArrayList, TranslateUtils.ToInt(this.ParentItemID.SelectedValue), TranslateUtils.ToInt(this.ddlAreaID.SelectedValue));

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
