using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ChangePublishmentSystemUrl : BackgroundBasePage
    {
        public TextBox tbPublishmentSystemUrl;
        public DropDownList ddlIsMultiDeployment;
        public PlaceHolder phIsMultiDeployment;
        public TextBox tbOuterUrl;
        public TextBox tbInnerUrl;
        public DropDownList ddlFuncFilesType;
        public PlaceHolder phCrossDomainFilesCopy;
        public PlaceHolder phFuncFilesCopy;

        //add by liangjian at 20150727, 设置API访问路径，方便API分离部署
        public TextBox tbAPIUrl;
        public PlaceHolder phAPIUrl;

        //子站单独部署 20151116 sessionliang
        public DropDownList ddlIsSonSiteAlone;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("修改访问地址", "modal_changePublishmentSystemUrl.aspx", arguments, 600, 550);
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!Page.IsPostBack)
            {
                this.tbPublishmentSystemUrl.Text = base.PublishmentSystemInfo.PublishmentSystemUrl;
                EBooleanUtils.AddListItems(this.ddlIsMultiDeployment, "内外网分离部署", "同一台服务器");
                ControlUtils.SelectListItems(this.ddlIsMultiDeployment, base.PublishmentSystemInfo.Additional.IsMultiDeployment.ToString());
                this.tbOuterUrl.Text = base.PublishmentSystemInfo.Additional.OuterUrl;
                this.tbInnerUrl.Text = base.PublishmentSystemInfo.Additional.InnerUrl;
                this.tbAPIUrl.Text = base.PublishmentSystemInfo.Additional.APIUrl;
                //子站单独部署 20151116 sessionliang
                EBooleanUtils.AddListItems(this.ddlIsSonSiteAlone);
                ControlUtils.SelectListItems(this.ddlIsSonSiteAlone, base.PublishmentSystemInfo.Additional.IsSonSiteAlone.ToString());

                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    this.ddlFuncFilesType.Items.Add(EFuncFilesTypeUtils.GetListItem(EFuncFilesType.Direct, false));
                }
                else
                {
                    EFuncFilesTypeUtils.AddListItems(this.ddlFuncFilesType);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlFuncFilesType, EFuncFilesTypeUtils.GetValue(base.PublishmentSystemInfo.Additional.FuncFilesType));
                }

                this.ddlIsMultiDeployment_SelectedIndexChanged(null, EventArgs.Empty);
                this.ddlFuncFilesType_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void ddlIsMultiDeployment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsMultiDeployment.Visible = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
        }

        public void ddlFuncFilesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
            this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
            this.phAPIUrl.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.Cors, this.ddlFuncFilesType.SelectedValue);
        }

        public void btnCopyCrossDomainFiles_OnClick(object sender, EventArgs E)
        {
            FileUtility.CopyCrossDomainFilesToSite(base.PublishmentSystemInfo);
            base.SuccessMessage("跨域代理页复制成功！");
        }

        public void btnCopyFuncFiles_OnClick(object sender, EventArgs E)
        {
            FileUtility.CopyFuncFilesToSite(base.PublishmentSystemInfo);
            base.SuccessMessage("功能页复制成功！");
        }

        public string GetSiteName()
        {
            return base.PublishmentSystemInfo.PublishmentSystemName;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                base.PublishmentSystemInfo.PublishmentSystemUrl = this.tbPublishmentSystemUrl.Text;
                base.PublishmentSystemInfo.Additional.IsMultiDeployment = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
                base.PublishmentSystemInfo.Additional.OuterUrl = this.tbOuterUrl.Text;
                base.PublishmentSystemInfo.Additional.InnerUrl = this.tbInnerUrl.Text;
                base.PublishmentSystemInfo.Additional.FuncFilesType = EFuncFilesTypeUtils.GetEnumType(this.ddlFuncFilesType.SelectedValue);
                base.PublishmentSystemInfo.Additional.APIUrl = this.tbAPIUrl.Text;
                //子站单独部署 20151116 sessionliang
                base.PublishmentSystemInfo.Additional.IsSonSiteAlone = TranslateUtils.ToBool(this.ddlIsSonSiteAlone.SelectedValue);
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                PublishmentSystemManager.UpdateUrlRewriteFile();
                StringUtility.AddLog(base.PublishmentSystemID, "修改网站访问设置");

                isChanged = true;
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(string.Format("修改失败：{0}", ex.Message));
                return;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
