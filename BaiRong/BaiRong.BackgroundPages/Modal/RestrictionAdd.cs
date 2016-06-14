using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages.Modal
{
	public class RestrictionAdd : BackgroundBasePage
	{
        protected TextBox Restriction;
        private string type;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string type)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Type", type);
            return PageUtilityPF.GetOpenWindowString("���IP���ʹ���", "modal_restrictionAdd.aspx", arguments, 450, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string type, string restriction)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Type", type);
            arguments.Add("Restriction", restriction);
            return PageUtilityPF.GetOpenWindowString("�޸�IP���ʹ���", "modal_restrictionAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            if (base.IsForbidden) return;

            this.type = base.GetQueryString("Type");
			if (!IsPostBack)
			{
                if (base.GetQueryString("Restriction") != null)
				{
                    this.Restriction.Text = base.GetQueryString("Restriction");
				}
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
			bool isChanged = false;

            StringCollection restrictionList = null;

            if (this.type == "Black")
            {
                restrictionList = ConfigManager.Instance.RestrictionBlackList;
            }
            else
            {
                restrictionList = ConfigManager.Instance.RestrictionWhiteList;
            }

            if (base.GetQueryString("Restriction") != null)
			{
				try
				{
                    StringCollection stringColl = new StringCollection();
                    foreach (string restriction in restrictionList)
                    {
                        if (restriction == base.GetQueryString("Restriction"))
                        {
                            stringColl.Add(this.Restriction.Text);
                        }
                        else
                        {
                            stringColl.Add(restriction);
                        }
                    }

                    if (this.type == "Black")
                    {
                        ConfigManager.Instance.RestrictionBlackList = stringColl;
                    }
                    else
                    {
                        ConfigManager.Instance.RestrictionWhiteList = stringColl;
                    }

                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

					isChanged = true;
				}
				catch
				{
                    base.FailMessage("IP���ʹ����޸�ʧ�ܣ�");
				}
			}
			else
			{
                if (restrictionList.IndexOf(this.Restriction.Text) != -1)
				{
                    base.FailMessage("IP���ʹ������ʧ�ܣ�IP���ʹ����Ѵ��ڣ�");
				}
				else
				{
					try
					{
                        restrictionList.Add(this.Restriction.Text);

                        if (this.type == "Black")
                        {
                            ConfigManager.Instance.RestrictionBlackList = restrictionList;
                        }
                        else
                        {
                            ConfigManager.Instance.RestrictionWhiteList = restrictionList;
                        }

                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "IP���ʹ������ʧ�ܣ�");
					}
				}
			}

			if (isChanged)
			{
                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "���ú�̨IP���ʹ���");
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
