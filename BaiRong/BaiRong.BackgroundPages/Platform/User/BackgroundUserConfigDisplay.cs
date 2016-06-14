using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Controls;
using System.Collections;
using System.Web;
using BaiRong.Text.LitJson;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigDisplay : BackgroundBasePage
    {
        public TextBox tbSystemName;
        public DropDownList ddlIsLogo;
        public PlaceHolder phLogo;
        public Literal ltlLogoUrl;
        public DropDownList ddlIsEnable;
        public PlaceHolder phOpen;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["uploadLogo"]))
            {
                Hashtable attributes = this.UploadLogo();
                string json = JsonMapper.ToJson(attributes);
                base.Response.Write(json);
                base.Response.End();
                return;
            }
            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserCenterSetting, "��������", AppManager.User.Permission.Usercenter_Setting);

                EBooleanUtils.AddListItems(this.ddlIsEnable, "����", "�ر�");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsEnable, UserConfigManager.Additional.IsEnable.ToString());
                this.tbSystemName.Text = UserConfigManager.Additional.SystemName;
                EBooleanUtils.AddListItems(this.ddlIsLogo, "�Զ����û�����LOGO", "ʹ��Ĭ��LOGO");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsLogo, UserConfigManager.Additional.IsLogo.ToString());
                this.ltlLogoUrl.Text = UserConfigManager.Additional.LogoUrl;
                this.ddlIsLogo_SelectedIndexChanged(null, EventArgs.Empty);
                this.phOpen.Visible = UserConfigManager.Additional.IsEnable;
            }
        }

        public void ddlIsLogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phLogo.Visible = TranslateUtils.ToBool(this.ddlIsLogo.SelectedValue);

            if (!this.phLogo.Visible || string.IsNullOrEmpty(UserConfigManager.Additional.LogoUrl))
            {
                this.ltlLogoUrl.Text = string.Format(@"<img id=""logoUrl"" src=""{0}"" />", PageUtils.GetAdminDirectoryUrl("images/usercenter.png"));

            }
            else
            {
                this.ltlLogoUrl.Text = string.Format(@"<img id=""logoUrl"" src=""{0}"" />", PageUtils.ParseNavigationUrl(UserConfigManager.Additional.LogoUrl));
            }
        }

        public void ddlIsEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            phOpen.Visible = TranslateUtils.ToBool(this.ddlIsEnable.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserConfigManager.Additional.SystemName = this.tbSystemName.Text;
                UserConfigManager.Additional.IsLogo = TranslateUtils.ToBool(this.ddlIsLogo.SelectedValue);
                this.ddlIsLogo_SelectedIndexChanged(null, EventArgs.Empty);
                UserConfigManager.Additional.IsEnable = TranslateUtils.ToBool(this.ddlIsEnable.SelectedValue);

                try
                {
                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸��û�������ʾ����");
                    base.SuccessMessage("��ʾ�����޸ĳɹ���");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "��ʾ�����޸�ʧ�ܣ�");
                }
            }
        }

        private Hashtable UploadLogo()
        {
            bool success = false;
            string message = string.Empty;

            if (base.Request.Files != null && base.Request.Files["Filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Filedata"];
                try
                {
                    if (postedFile != null && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string filePath = postedFile.FileName;
                        string fileExtName = filePath.ToLower().Substring(filePath.LastIndexOf(".") + 1);
                        EImageType imageType = EImageTypeUtils.GetEnumType(fileExtName);
                        if (imageType != EImageType.Unknown)
                        {
                            string fileName = string.Format("usercenter_logo.{0}", EImageTypeUtils.GetValue(imageType));
                            string logoPath = PathUtils.GetSiteFilesPath(fileName);
                            postedFile.SaveAs(logoPath);

                            UserConfigManager.Additional.LogoUrl = PageUtils.GetSiteFilesUrl(fileName);
                            BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            Hashtable jsonAttributes = new Hashtable();
            if (success)
            {
                jsonAttributes.Add("success", "true");
                jsonAttributes.Add("logoUrl", UserConfigManager.Additional.LogoUrl);
            }
            else
            {
                jsonAttributes.Add("success", "false");
                if (string.IsNullOrEmpty(message))
                {
                    message = "ͼ���ϴ�ʧ��";
                }
                jsonAttributes.Add("message", message);
            }

            return jsonAttributes;
        }
    }
}
