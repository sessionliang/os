using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleOrganizationAreaAdd : BackgroundBasePage
    {
        protected TextBox tbItemName;

        public DropDownList ParentItemID;

        private int classifyID;//分类ID
        private int areaID;//区域ID
        private string returnUrl;


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            OrganizationClassifyInfo pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
            if (base.GetIntQueryString("ClassifyID") == pinfo.ItemID)
                return;

            PageUtils.CheckRequestParameter("ItemID");
            this.classifyID = TranslateUtils.ToInt(base.GetQueryString("ClassifyID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.areaID = TranslateUtils.ToInt(base.GetQueryString("ItemID"));

            if (!IsPostBack)
            {
                ListItem item = new ListItem("无", "0");
                this.ParentItemID.Items.Add(item);

                TreeManager.AddListItemsByClassify(this.ParentItemID.Items, base.PublishmentSystemID, this.classifyID, true, false, "OrganizationArea", this.classifyID, 1);
                ControlUtils.SelectListItems(this.ParentItemID, "0");

                if (this.areaID > 0)
                {
                    OrganizationAreaInfo info = DataProvider.OrganizationAreaDAO.GetInfo(this.areaID
);
                    this.tbItemName.Text = info.ItemName;
                    ControlUtils.SelectListItems(this.ParentItemID, info.ParentID.ToString());
                }
            }
        }


        public static string GetRedirectUrl(int publishmentSystemID, int areaID, int classifyID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("console_organizationAreaAdd.aspx?PublishmentSystemID={0}&ItemID={1}&ClassifyID={2}&ReturnUrl={3}", publishmentSystemID, areaID, classifyID, StringUtils.ValueToUrl(returnUrl)));
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (this.areaID > 0)
            {
                try
                {
                    if (this.ParentItemID.SelectedValue == this.areaID.ToString())
                    {
                        base.FailMessage("区域修改失败，不成为自己的下级！");
                        return;
                    }
                    OrganizationAreaInfo info = DataProvider.OrganizationAreaDAO.GetInfo(this.areaID);
                    info.ItemName = PageUtils.FilterXSS(this.tbItemName.Text.Trim());
                    info.ParentID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
                    info.ClassifyID = this.classifyID;
                    DataProvider.OrganizationAreaDAO.Update(info);

                    base.SuccessMessage("区域修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "修改区域失败！");
                }
            }
            else
            {
                OrganizationClassifyInfo pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
                if (this.classifyID == pinfo.ItemID)
                {
                    base.FailMessage("区域添加失败，请选择分类！");
                    return;
                }
                if (DataProvider.OrganizationAreaDAO.IsExists(this.tbItemName.Text, this.classifyID))
                {
                    base.FailMessage("区域添加失败，区域名称已存在！");
                    return;
                }
                else
                {
                    try
                    {
                        OrganizationAreaInfo info = new OrganizationAreaInfo();
                        info.ItemName = PageUtils.FilterXSS(this.tbItemName.Text.Trim());
                        info.ParentID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
                        info.ClassifyID = this.classifyID;
                        DataProvider.OrganizationAreaDAO.Insert(info);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "添加区域失败！");
                    }
                }

                base.SuccessMessage("区域添加成功！");
            }
            base.AddWaitAndRedirectScript(this.returnUrl);
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
