using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;

using BaiRong.Core.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OracleClient;
using BaiRong.Core.Data;
using BaiRong.Core.Service;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMLibManageSite : BackgroundBasePage
    {
        public RadioButtonList IsUseMLib;

        public PlaceHolder phPublishmentSystem;

        public TextBox tbUnifiedMLibAddUser;
        public CheckBox cbUnifiedMLibAddUser;

        public TextBox tbMLibValidityDate;
        public CheckBox cbUnifiedMLibValidityDate;

        public TextBox tbUnifiedMlibNum;
        public CheckBox cbUnifiedMLibNum;

        public RadioButtonList rblMLibCheckType;


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            ArrayList hasInfoList = new ArrayList();

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_MLibManage, "Ͷ���������", AppManager.User.Permission.Usercenter_MLibManageSetting);

                EBooleanUtils.AddListItems(this.IsUseMLib, "����", "������");
                EBooleanUtils.AddListItems(this.rblMLibCheckType, "��վ����ĿΪγ��", "���û���Ϊγ��");
                this.IsUseMLib.SelectedValue = ConfigManager.Additional.IsUseMLib.ToString();

                this.cbUnifiedMLibAddUser.Checked = ConfigManager.Additional.IsUnifiedMLibAddUser;
                this.tbUnifiedMLibAddUser.Text = ConfigManager.Additional.UnifiedMLibAddUser;

                this.cbUnifiedMLibValidityDate.Checked = ConfigManager.Additional.IsUnifiedMLibValidityDate;
                this.tbMLibValidityDate.Text = ConfigManager.Additional.UnifiedMLibValidityDate.ToString();

                this.cbUnifiedMLibNum.Checked = ConfigManager.Additional.IsUnifiedMLibNum;
                this.tbUnifiedMlibNum.Text = ConfigManager.Additional.UnifiedMlibNum.ToString();

                this.rblMLibCheckType.SelectedValue = ConfigManager.Additional.MLibCheckType.ToString();

                IsUseMLib_SelectedIndexChanged(sender, E);
            }
        }

        /// <summary>
        /// �Ƿ������û�����Ͷ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IsUseMLib_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phPublishmentSystem.Visible = TranslateUtils.ToBool(this.IsUseMLib.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            { 
                if (AdminManager.GetAdminInfo(this.tbUnifiedMLibAddUser.Text) == null)
                {
                    base.FailMessage("����ʧ�ܣ����������������Ĺ���Ա������");
                    return;
                }
                if (AdminManager.GetAdminInfo(this.tbUnifiedMLibAddUser.Text).IsLockedOut)
                {
                    base.FailMessage("����ʧ�ܣ����������������Ĺ���Ա������������ʹ��");
                    return;
                }
                if (!AdminManager.HasChannelPermissionByAdmin(this.tbUnifiedMLibAddUser.Text))
                {
                    if (PublishmentSystemManager.GetPublishmentSystem(this.tbUnifiedMLibAddUser.Text).Count == 0)
                    {
                        base.FailMessage("����ʧ�ܣ�����Ա��վ������ĿȨ�ޣ���������Ϊ���������");
                        return;
                    }
                }
                ConfigManager.Instance.Additional.IsUseMLib = TranslateUtils.ToBool(this.IsUseMLib.SelectedValue);
                if (TranslateUtils.ToBool(this.IsUseMLib.SelectedValue))
                {
                    ConfigManager.Instance.Additional.IsUnifiedMLibAddUser = this.cbUnifiedMLibAddUser.Checked;
                    ConfigManager.Instance.Additional.UnifiedMLibAddUser = this.tbUnifiedMLibAddUser.Text;

                    ConfigManager.Instance.Additional.IsUnifiedMLibValidityDate = this.cbUnifiedMLibValidityDate.Checked;
                    ConfigManager.Instance.Additional.UnifiedMLibValidityDate = TranslateUtils.ToInt(this.tbMLibValidityDate.Text);

                    ConfigManager.Instance.Additional.IsUnifiedMLibNum = this.cbUnifiedMLibNum.Checked;
                    ConfigManager.Instance.Additional.UnifiedMlibNum = TranslateUtils.ToInt(this.tbUnifiedMlibNum.Text);

                    ConfigManager.Instance.Additional.MLibCheckType = TranslateUtils.ToBool(this.rblMLibCheckType.SelectedValue);

                    if (ConfigManager.Instance.Additional.MLibStartTime == DateUtils.SqlMinValue)
                        ConfigManager.Instance.Additional.MLibStartTime = DateTime.Now;
                }
                else
                {
                    ConfigManager.Instance.Additional.IsUnifiedMLibAddUser = false;
                    ConfigManager.Instance.Additional.UnifiedMLibAddUser = string.Empty;

                    ConfigManager.Instance.Additional.IsUnifiedMLibValidityDate = false;
                    ConfigManager.Instance.Additional.UnifiedMLibValidityDate = 0;

                    ConfigManager.Instance.Additional.IsUnifiedMLibNum = false;
                    ConfigManager.Instance.Additional.UnifiedMlibNum = 0;

                    ConfigManager.Instance.Additional.MLibCheckType = true;

                    ConfigManager.Instance.Additional.MLibStartTime = DateUtils.SqlMinValue;

                    ConfigManager.Instance.Additional.MLibPublishmentSystemIDs = string.Empty;

                }

                ConfigManager.Instance.Additional.IsUseMLib = TranslateUtils.ToBool(this.IsUseMLib.SelectedValue);
                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "Ͷ���������");
                base.SuccessMessage("Ͷ��������óɹ�");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
