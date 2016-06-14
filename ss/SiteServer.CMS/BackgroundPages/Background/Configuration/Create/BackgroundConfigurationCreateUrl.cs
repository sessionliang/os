using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Controls;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundConfigurationCreateUrl : BackgroundBasePage
    {
        public TextBox tbPublishmentSystemUrl;
        public TextBox tbHomeUrl;
        public DropDownList ddlIsMultiDeployment;
        public PlaceHolder phIsMultiDeployment;
        public TextBox tbOuterUrl;
        public TextBox tbInnerUrl;
        public DropDownList ddlFuncFilesType;
        public PlaceHolder phCrossDomainFilesCopy;
        public PlaceHolder phFuncFilesCopy;
        public PlaceHolder phAPIUrl;
        public TextBox tbAPIUrl;


        //子站单独部署 20151116 sessionliang
        public DropDownList ddlIsSonSiteAlone;
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationCreate, "网站访问设置", AppManager.CMS.Permission.WebSite.Configration);

                this.tbPublishmentSystemUrl.Text = base.PublishmentSystemInfo.PublishmentSystemUrl;
                this.tbHomeUrl.Text = base.PublishmentSystemInfo.Additional.HomeUrl;
                EBooleanUtils.AddListItems(this.ddlIsMultiDeployment, "内外网分离部署", "同一台服务器");
                ControlUtils.SelectListItems(this.ddlIsMultiDeployment, base.PublishmentSystemInfo.Additional.IsMultiDeployment.ToString());
                this.tbOuterUrl.Text = base.PublishmentSystemInfo.Additional.OuterUrl;
                this.tbInnerUrl.Text = base.PublishmentSystemInfo.Additional.InnerUrl;

                //API独立地址 20151201
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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.PublishmentSystemUrl = this.tbPublishmentSystemUrl.Text;
                this.PublishmentSystemInfo.Additional.HomeUrl = this.tbHomeUrl.Text;
                base.PublishmentSystemInfo.Additional.IsMultiDeployment = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
                base.PublishmentSystemInfo.Additional.OuterUrl = this.tbOuterUrl.Text;
                base.PublishmentSystemInfo.Additional.InnerUrl = this.tbInnerUrl.Text;
                base.PublishmentSystemInfo.Additional.FuncFilesType = EFuncFilesTypeUtils.GetEnumType(this.ddlFuncFilesType.SelectedValue);

                //API独立地址 20151201 sessionliang
                base.PublishmentSystemInfo.Additional.APIUrl = this.tbAPIUrl.Text;

                //子站单独部署 20151116 sessionliang
                base.PublishmentSystemInfo.Additional.IsSonSiteAlone = TranslateUtils.ToBool(this.ddlIsSonSiteAlone.SelectedValue);
                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改网站访问设置");
                    base.SuccessMessage("网站访问设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "网站访问设置修改失败！");
                }
            }
        }
    }
}
