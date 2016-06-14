using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class SpecAdd : BackgroundBasePage
	{
        protected TextBox SpecName;
        protected RadioButtonList IsIcon;
        protected RadioButtonList IsMultiple;
        protected RadioButtonList IsRequired;
        protected TextBox Description;

        private int channelID;
        private SpecInfo specInfo;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int channelID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("channelID", channelID.ToString());
            return PageUtilityB2C.GetOpenWindowString("��ӹ��", "modal_specAdd.aspx", arguments, 420, 450);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int channelID, int specID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("channelID", channelID.ToString());
            arguments.Add("SpecID", specID.ToString());
            return PageUtilityB2C.GetOpenWindowString("�޸Ĺ��", "modal_specAdd.aspx", arguments, 420, 450);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = base.GetIntQueryString("channelID");
            int specID = base.GetIntQueryString("SpecID");
            if (specID > 0)
            {
                this.specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, specID);
            }

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.IsIcon, "ͼƬ", "����");
                ControlUtils.SelectListItems(this.IsIcon, false.ToString());

                EBooleanUtils.AddListItems(this.IsMultiple, "��ѡ", "��ѡ");
                ControlUtils.SelectListItems(this.IsMultiple, false.ToString());

                EBooleanUtils.AddListItems(this.IsRequired, "��ѡ��", "��ѡ��");
                ControlUtils.SelectListItems(this.IsRequired, true.ToString());

                if (this.specInfo != null)
                {
                    this.SpecName.Text = this.specInfo.SpecName;
                    ControlUtils.SelectListItems(this.IsIcon, this.specInfo.IsIcon.ToString());
                    ControlUtils.SelectListItems(this.IsMultiple, this.specInfo.IsMultiple.ToString());
                    ControlUtils.SelectListItems(this.IsRequired, this.specInfo.IsRequired.ToString());
                    this.Description.Text = this.specInfo.Description;
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            if (this.specInfo != null)
			{
				try
				{
                    this.specInfo.SpecName = this.SpecName.Text;
                    this.specInfo.IsIcon = TranslateUtils.ToBool(this.IsIcon.SelectedValue);
                    this.specInfo.IsMultiple = TranslateUtils.ToBool(this.IsMultiple.SelectedValue);
                    this.specInfo.IsRequired = TranslateUtils.ToBool(this.IsRequired.SelectedValue);
                    this.specInfo.Description = this.Description.Text;

                    DataProviderB2C.SpecDAO.Update(this.specInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "�޸Ĺ��", string.Format("���:{0}", this.specInfo.SpecName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "����޸�ʧ�ܣ�");
				}
			}
			else
			{
                if (DataProviderB2C.SpecDAO.IsExists(base.PublishmentSystemID, this.channelID, this.SpecName.Text))
				{
                    base.FailMessage("������ʧ�ܣ���������Ѵ��ڣ�");
				}
				else
				{
					try
					{
                        this.specInfo = new SpecInfo(0, base.PublishmentSystemID, this.channelID, this.SpecName.Text, TranslateUtils.ToBool(this.IsIcon.SelectedValue), TranslateUtils.ToBool(this.IsMultiple.SelectedValue), TranslateUtils.ToBool(this.IsRequired.SelectedValue), this.Description.Text, 0);

                        DataProviderB2C.SpecDAO.Insert(this.specInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "��ӹ��", string.Format("���:{0}", this.specInfo.SpecName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "������ʧ�ܣ�");
					}
				}
			}

			if (isChanged)
			{
                string redirectUrl = BackgroundSpec.GetRedirectUrl(base.PublishmentSystemID, this.channelID);
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);
			}
		}
	}
}
