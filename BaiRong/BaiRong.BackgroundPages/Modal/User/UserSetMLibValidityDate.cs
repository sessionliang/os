using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using System.Collections.Generic;
using BaiRong.Controls;

namespace BaiRong.BackgroundPages.Modal
{
    /// <summary>
    /// by 20160119 ��������Ͷ����Ч�� 
    /// 
    /// </summary>
    public class UserSetMLibValidityDate : BackgroundBasePage
    {
        protected DateTimeTextBox MLibValidityDate;

        private ArrayList userIDList;

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityPF.GetOpenWindowStringWithCheckBoxValue("�����û�Ͷ����Ч��", "modal_userSetMLibValidityDate.aspx", arguments, "UserIDCollection", "��ѡ����Ҫ����Ͷ����Ч�ڵ��û���", 400, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("UserIDCollection"));

            if (!IsPostBack)
            {
                this.MLibValidityDate.Text = DateTime.Now.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate).ToString();
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.MLibValidityDate.Text))
            {
                base.FailMessage("�����û�Ͷ����Ч��ʧ�ܣ�Ͷ����Ч�ڲ���Ϊ��");
                return;
            }
            try
            {
                DateTime MLibValidityDate = TranslateUtils.ToDateTime(this.MLibValidityDate.Text);
                BaiRongDataProvider.UserDAO.UpdateMLibValidityDate(this.userIDList, MLibValidityDate);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�����û�Ͷ����Ч��", string.Empty);
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "�����û�Ͷ����Ч��ʧ�ܣ�" + ex.Message);
            }
        }

    }
}
