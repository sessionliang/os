using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages
{
    public class BackgroundRestrictionOptions : BackgroundBasePage
	{
        public RadioButtonList RestrictionType;
		
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Restriction, "��������ѡ��", AppManager.Platform.Permission.Platform_Restriction);

                ERestrictionTypeUtils.AddListItems(this.RestrictionType);
                ControlUtils.SelectListItemsIgnoreCase(this.RestrictionType, ERestrictionTypeUtils.GetValue(ConfigManager.Additional.RestrictionType));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ConfigManager.Additional.RestrictionType = ERestrictionTypeUtils.GetEnumType(this.RestrictionType.SelectedValue);

                try
                {
                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "���÷�������ѡ��");

                    base.SuccessMessage("��������ѡ���޸ĳɹ���");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "��������ѡ���޸�ʧ�ܣ�");
                }
            }
        }
	}
}
