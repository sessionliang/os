using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.BBS.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class RestrictionAdd : BackgroundBasePage
	{
        protected TextBox Restriction;
        private string type;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string type)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Type", type);
            return JsUtils.OpenWindow.GetOpenWindowString("添加IP访问规则", PageUtils.GetBBSUrl("modal_restrictionAdd.aspx"), arguments, 450, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string type, string restriction)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Type", type);
            arguments.Add("Restriction", restriction);
            return JsUtils.OpenWindow.GetOpenWindowString("修改IP访问规则", PageUtils.GetBBSUrl("modal_restrictionAdd.aspx"), arguments, 450, 300);
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

            if (this.type == "Black")
            {
                restrictionList = base.Additional.RestrictionBlackList;
            }
            else
            {
                restrictionList = base.Additional.RestrictionWhiteList;
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
                        base.Additional.RestrictionBlackList = stringColl;
                    }
                    else
                    {
                        base.Additional.RestrictionWhiteList = stringColl;
                    }

                    ConfigurationManager.Update(base.PublishmentSystemID);

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

                        if (this.type == "Black")
                        {
                            base.Additional.RestrictionBlackList = restrictionList;
                        }
                        else
                        {
                            base.Additional.RestrictionWhiteList = restrictionList;
                        }

                        ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(base.PublishmentSystemID);

                        ConfigurationManager.Update(base.PublishmentSystemID);

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
                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置后台IP访问规则");
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
