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
    public class ConsoleOrganizationAdd : BackgroundBasePage
    {
        public DropDownList ddlClassifyID;
        public TextBox tbName;
        public DropDownList ddlAreaID;
        public TextBox tbAddress;
        public TextBox tbExplain;
        public TextBox tbPhone;
        public TextBox tbLongitude;
        public TextBox tbLatitude;
        public TextBox tbLogoUrl;

        public Button ImgSelect;
        public Button ImgUpload;
        public Button ImgCut;
        public Button ImgView;


        private int itemID;
        private int ID;
        private string returnUrl;

        private OrganizationClassifyInfo pinfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ItemID", "ReturnUrl");
            this.itemID = base.GetIntQueryString("ItemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.ID = base.GetIntQueryString("ID");

            OrganizationInfo info = DataProvider.OrganizationInfoDAO.GetContentInfo(this.ID);
            pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();

            if (!base.IsPostBack)
            {

                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "添加分支机构", AppManager.Platform.Permission.Platform_Organization);


                if (this.ID > 0)
                {
                    TreeManager.AddListItems(this.ddlClassifyID.Items, base.PublishmentSystemID, info.ClassifyID, true, true, "OrganizationClassify");

                    TreeManager.AddListItemsByClassify(this.ddlAreaID.Items, base.PublishmentSystemID, info.AreaID, true, true, "OrganizationArea", info.ClassifyID, 0);

                    ControlUtils.SelectListItems(this.ddlClassifyID, info.ClassifyID.ToString());
                    this.tbName.Text = info.OrganizationName;
                    ControlUtils.SelectListItems(this.ddlAreaID, info.AreaID.ToString());
                    this.tbAddress.Text = info.OrganizationAddress;
                    this.tbPhone.Text = info.Phone;
                    this.tbExplain.Text = info.Explain;
                    this.tbLatitude.Text = info.Latitude.ToString();
                    this.tbLongitude.Text = info.Longitude.ToString();
                    this.tbLogoUrl.Text = info.LogoUrl;
                }
                else
                {
                    TreeManager.AddListItems(this.ddlClassifyID.Items, base.PublishmentSystemID, this.itemID, true, true, "OrganizationClassify");
                    if (this.itemID == pinfo.ItemID)
                        this.ddlClassifyID.SelectedIndex = 1;
                    else
                        ControlUtils.SelectListItems(this.ddlClassifyID, this.itemID.ToString());

                    TreeManager.AddListItemsByClassify(this.ddlAreaID.Items, base.PublishmentSystemID, this.itemID, true, true, "OrganizationArea", this.itemID, 0);
                    if (this.ddlAreaID.Items.Count > 0)
                        this.ddlAreaID.SelectedIndex = 0;
                }

                this.ImgUpload.Attributes.Add("onClick", Modal.UploadImageSingleSystem.GetOpenWindowStringToTextBox("tbLogoUrl"));
            }
        }

        public void ddlClassifyID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlAreaID.Items.Clear();
            if (this.ddlClassifyID.SelectedValue == pinfo.ItemID.ToString())
            {
                this.ddlClassifyID.SelectedIndex = 1;
            }
            int theItemID = TranslateUtils.ToInt(this.ddlClassifyID.SelectedValue);
            TreeManager.AddListItemsByClassify(this.ddlAreaID.Items, base.PublishmentSystemID, theItemID, true, true, "OrganizationArea", theItemID, 0);
            if (this.ddlAreaID.Items.Count > 0)
                this.ddlAreaID.SelectedIndex = 0;
        }

        public static string GetRedirectUrl(int publishmentSystemID, int itemID, int  ID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("console_organizationAdd.aspx?PublishmentSystemID={0}&ItemID={1}&ID={2}&ReturnUrl={3}", publishmentSystemID, itemID, ID, StringUtils.ValueToUrl(returnUrl)));
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int insertItemID = 0;

                if (this.ddlClassifyID.SelectedValue == pinfo.ItemID.ToString())
                {
                    base.FailMessage("请选择所属分类！");
                    return;
                }
                if (TranslateUtils.ToInt(this.ddlAreaID.SelectedValue) == 0)
                {
                    base.FailMessage("请选择所在区域！");
                    return;
                }
                try
                {
                    if (this.ID == 0)
                    {
                        if (DataProvider.OrganizationInfoDAO.IsExists(TranslateUtils.ToInt(this.ddlClassifyID.SelectedValue), 0, this.tbName.Text))
                        {
                            base.FailMessage("分支机构添加失败，该分类下分支机构名称已存在！");
                            return;
                        }
                        OrganizationInfo info = new OrganizationInfo();
                        info.ClassifyID = TranslateUtils.ToInt(this.ddlClassifyID.SelectedValue);
                        info.OrganizationName = PageUtils.FilterXSS(this.tbName.Text);
                        info.AreaID = TranslateUtils.ToInt(this.ddlAreaID.SelectedValue);
                        info.OrganizationAddress = PageUtils.FilterXSS(this.tbAddress.Text);
                        info.Explain = PageUtils.FilterXSS(this.tbExplain.Text);
                        info.Phone = PageUtils.FilterXSS(this.tbPhone.Text);
                        info.Longitude = this.tbLongitude.Text;
                        info.Latitude = this.tbLatitude.Text;
                        info.LogoUrl = PageUtils.FilterXSS(this.tbLogoUrl.Text);
                        info.PublishmentSystemID = base.PublishmentSystemID;
                        info.Enabled = true;
                        info.AddDate = DateTime.Now;

                        insertItemID = DataProvider.OrganizationInfoDAO.Insert(info);
                        base.SuccessMessage("分支机构添加成功！");
                        //统计全部分类下的机构数量
                        pinfo.ContentNum = DataProvider.OrganizationInfoDAO.GetCount();
                        DataProvider.OrganizationClassifyDAO.Update(pinfo);
                    }
                    else
                    {
                        if (DataProvider.OrganizationInfoDAO.IsExists(TranslateUtils.ToInt(this.ddlClassifyID.SelectedValue), this.ID, this.tbName.Text))
                        {
                            base.FailMessage("分支机构修改失败，该分类下机构名称已存在！");
                            return;
                        }
                        OrganizationInfo info = DataProvider.OrganizationInfoDAO.GetContentInfo(this.ID);
                        info.ClassifyID = TranslateUtils.ToInt(this.ddlClassifyID.SelectedValue);
                        info.OrganizationName = PageUtils.FilterXSS(this.tbName.Text);
                        info.AreaID = TranslateUtils.ToInt(this.ddlAreaID.SelectedValue);
                        info.OrganizationAddress = PageUtils.FilterXSS(this.tbAddress.Text);
                        info.Explain = PageUtils.FilterXSS(this.tbExplain.Text);
                        info.Phone = PageUtils.FilterXSS(this.tbPhone.Text);
                        info.Longitude = this.tbLongitude.Text;
                        info.Latitude = this.tbLatitude.Text;
                        info.LogoUrl = PageUtils.FilterXSS(this.tbLogoUrl.Text);
                        DataProvider.OrganizationInfoDAO.Update(info);
                        base.SuccessMessage("分支机构修改成功！");
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("分支机构添加失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }

                base.AddWaitAndRedirectScript(this.returnUrl);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
