using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;


namespace SiteServer.CMS.BackgroundPages.Modal
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
            return PageUtility.GetOpenWindowString("添加IP访问规则", "modal_restrictionAdd.aspx", arguments, 450, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string type, string restriction)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Type", type);
            arguments.Add("Restriction", restriction);
            return PageUtility.GetOpenWindowString("修改IP访问规则", "modal_restrictionAdd.aspx", arguments, 450, 300);
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

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            StringCollection restrictionList = null;

            if (base.PublishmentSystemID == 0)
            {
                if (this.type == "Black")
                {
                    restrictionList = ConfigManager.Instance.RestrictionBlackList;
                }
                else
                {
                    restrictionList = ConfigManager.Instance.RestrictionWhiteList;
                }
            }
            else
            {
                if (this.type == "Black")
                {
                    restrictionList = base.PublishmentSystemInfo.Additional.RestrictionBlackList;
                }
                else
                {
                    restrictionList = base.PublishmentSystemInfo.Additional.RestrictionWhiteList;
                }
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

                    if (base.PublishmentSystemID == 0)
                    {
                        if (this.type == "Black")
                        {
                            ConfigManager.Instance.RestrictionBlackList = stringColl;
                        }
                        else
                        {
                            ConfigManager.Instance.RestrictionWhiteList = stringColl;
                        }

                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                    }
                    else
                    {
                        if (this.type == "Black")
                        {
                            base.PublishmentSystemInfo.Additional.RestrictionBlackList = stringColl;
                        }
                        else
                        {
                            base.PublishmentSystemInfo.Additional.RestrictionWhiteList = stringColl;
                        }

                        DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    }

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "IP访问规则修改失败！");
				}
			}
			else
			{
                if (restrictionList.IndexOf(this.Restriction.Text) != -1)
				{
                    base.FailMessage("IP访问规则添加失败，IP访问规则已存在！");
				}
				else
				{
					try
					{
                        restrictionList.Add(this.Restriction.Text);

                        if (base.PublishmentSystemID == 0)
                        {
                            if (this.type == "Black")
                            {
                                ConfigManager.Instance.RestrictionBlackList = restrictionList;
                            }
                            else
                            {
                                ConfigManager.Instance.RestrictionWhiteList = restrictionList;
                            }

                            BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                        }
                        else
                        {
                            if (this.type == "Black")
                            {
                                base.PublishmentSystemInfo.Additional.RestrictionBlackList = restrictionList;
                            }
                            else
                            {
                                base.PublishmentSystemInfo.Additional.RestrictionWhiteList = restrictionList;
                            }

                            DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                        }

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "IP访问规则添加失败！");
					}
				}
			}

			if (isChanged)
			{
                if (base.PublishmentSystemID == 0)
                {
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置后台IP访问规则");
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "设置页面访问限制规则");
                }
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
