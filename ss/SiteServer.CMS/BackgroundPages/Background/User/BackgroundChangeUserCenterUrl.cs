using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
    public class ChangeUserCenterUrl : BackgroundBasePage
    {
        public TextBox tbPublishmentSystemUrl;
        public DropDownList ddlIsMultiDeployment;
        public PlaceHolder phIsMultiDeployment;
        public TextBox tbOuterUrl;
        public TextBox tbInnerUrl;
        public DropDownList ddlFuncFilesType;
        public PlaceHolder phCrossDomainFilesCopy;
        public PlaceHolder phFuncFilesCopy;
        public Literal ltOuterUrl;
        public Literal ltInnerUrl;


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

                if (base.PublishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    ltOuterUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", base.PublishmentSystemInfo.Additional.OuterUrl);
                    ltInnerUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", base.PublishmentSystemInfo.Additional.InnerUrl);
                }
                else
                {
                    ltOuterUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty));
                }


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

                if (base.PublishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    //内外网分离部署
                    this.ddlFuncFilesType.Items.Remove(new ListItem("直接访问（非跨域）", "Direct"));
                    this.phCrossDomainFilesCopy.Visible = true;

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("通过代理访问（跨域）", "CrossDomain")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("复制到站内访问（跨域）", "CopyToSite")))
                        this.ddlFuncFilesType.Items.Insert(1, new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                }
                else
                {
                    this.ddlFuncFilesType.Items.Remove(new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    this.ddlFuncFilesType.Items.Remove(new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                    this.phCrossDomainFilesCopy.Visible = false;

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("直接访问（非跨域）", "Direct")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("直接访问（非跨域）", "Direct"));
                }
                this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
                this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
            }
        }

        public void ddlIsMultiDeployment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsMultiDeployment.Visible = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
            if (base.IsPostBack)
            {
                if (TranslateUtils.ToBool(ddlIsMultiDeployment.SelectedValue))
                {
                    //内外网分离部署
                    this.ddlFuncFilesType.Items.Remove(new ListItem("直接访问（非跨域）", "Direct"));                    

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("通过代理访问（跨域）", "CrossDomain")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("复制到站内访问（跨域）", "CopyToSite")))
                        this.ddlFuncFilesType.Items.Insert(1, new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                }
                else
                {
                    this.ddlFuncFilesType.Items.Remove(new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    this.ddlFuncFilesType.Items.Remove(new ListItem("复制到站内访问（跨域）", "CopyToSite"));

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("直接访问（非跨域）", "Direct")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("直接访问（非跨域）", "Direct"));
                }
                this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
                this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
            }
        }

        public void ddlFuncFilesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
            this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
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
