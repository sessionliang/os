using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicCategoryClassAdd : BackgroundBasePage
	{
        protected TextBox tbClassName;
        protected TextBox tbClassCode;
        protected RadioButtonList rblIsEnabled;
        protected TextBox tbDescription;

        private string classCode;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowString("添加分类法", "modal_govPublicCategoryClassAdd.aspx", arguments, 400, 360);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string classCode)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ClassCode", classCode);
            return PageUtilityWCM.GetOpenWindowString("修改分类法", "modal_govPublicCategoryClassAdd.aspx", arguments, 400, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.classCode = base.Request.QueryString["ClassCode"];

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.classCode))
                {
                    GovPublicCategoryClassInfo categoryClassInfo = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfo(this.classCode, base.PublishmentSystemID);
                    if (categoryClassInfo != null)
                    {
                        this.tbClassName.Text = categoryClassInfo.ClassName;
                        this.tbClassCode.Text = categoryClassInfo.ClassCode;
                        this.tbClassCode.Enabled = false;
                        ControlUtils.SelectListItems(this.rblIsEnabled, categoryClassInfo.IsEnabled.ToString());
                        if (categoryClassInfo.IsSystem)
                        {
                            this.rblIsEnabled.Enabled = false;
                        }
                        this.tbDescription.Text = categoryClassInfo.Description;
                    }
                }
                
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            GovPublicCategoryClassInfo categoryClassInfo = null;
				
			if (!string.IsNullOrEmpty(this.classCode))
			{
				try
				{
                    categoryClassInfo = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfo(this.classCode, base.PublishmentSystemID);
                    if (categoryClassInfo != null)
                    {
                        categoryClassInfo.ClassName = this.tbClassName.Text;
                        categoryClassInfo.ClassCode = this.tbClassCode.Text;
                        categoryClassInfo.IsEnabled = TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue);
                        categoryClassInfo.Description = this.tbDescription.Text;
                    }
                    DataProvider.GovPublicCategoryClassDAO.Update(categoryClassInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改分类法", string.Format("分类法:{0}", categoryClassInfo.ClassName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "分类法修改失败！");
				}
			}
			else
			{
                ArrayList classNameArrayList = DataProvider.GovPublicCategoryClassDAO.GetClassNameArrayList(base.PublishmentSystemID);
                ArrayList classCodeArrayList = DataProvider.GovPublicCategoryClassDAO.GetClassCodeArrayList(base.PublishmentSystemID);
                if (classNameArrayList.IndexOf(this.tbClassName.Text) != -1)
				{
                    base.FailMessage("分类法添加失败，分类法名称已存在！");
                }
                else if (classCodeArrayList.IndexOf(this.tbClassCode.Text) != -1)
                {
                    base.FailMessage("分类法添加失败，分类代码已存在！");
                }
				else
				{
					try
					{
                        categoryClassInfo = new GovPublicCategoryClassInfo(this.tbClassCode.Text, base.PublishmentSystemID, this.tbClassName.Text, false, TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue), string.Empty, 0, this.tbDescription.Text);

                        DataProvider.GovPublicCategoryClassDAO.Insert(categoryClassInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加分类法", string.Format("分类法:{0}", categoryClassInfo.ClassName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "分类法添加失败！");
					}
				}
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(PageUtils.GetWCMUrl(string.Format("background_govPublicCategoryClass.aspx?PublishmentSystemID={0}", base.PublishmentSystemID))));
			}
		}
	}
}
