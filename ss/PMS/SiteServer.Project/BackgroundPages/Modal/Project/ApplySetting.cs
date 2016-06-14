using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.Project.Model;
using System.Text;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ApplySetting : BackgroundBasePage
	{
        protected DropDownList ddlTypeID;
        protected DropDownList ddlUserName;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_applySetting.aspx", arguments, "IDCollection", "请选择需要设置的办件！", 500, 350);
        }

        public static string GetShowPopWinString(int projectID, int applyID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("IDCollection", applyID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置属性", "modal_applySetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("<<保持不变>>", "0");
                this.ddlTypeID.Items.Add(listItem);
                ArrayList typeInfoArrayList = DataProvider.TypeDAO.GetTypeInfoArrayList(this.ProjectID);
                foreach (TypeInfo typeInfo in typeInfoArrayList)
                {
                    this.ddlTypeID.Items.Add(new ListItem(typeInfo.TypeName, typeInfo.TypeID.ToString()));
                }

                listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlUserName.Items.Add(listItem);
                ArrayList userNameArrayList = DataProvider.ApplyDAO.GetUserNameArrayList(base.ProjectID);
                foreach (string userName in userNameArrayList)
                {
                    this.ddlUserName.Items.Add(new ListItem(AdminManager.GetDisplayName(userName, true), userName));
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                int typeID = TranslateUtils.ToInt(this.ddlTypeID.SelectedValue);
                string userName = this.ddlUserName.SelectedValue;
                foreach (int applyID in this.idArrayList)
                {
                    ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(applyID);
                    if (typeID > 0)
                    {
                        applyInfo.TypeID = typeID;
                    }
                    if (!string.IsNullOrEmpty(userName))
                    {
                        applyInfo.UserName = userName;
                    }
                    DataProvider.ApplyDAO.Update(applyInfo);
                }

                isChanged = true;
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}

	}
}
