using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;

using BaiRong.Controls;
using System.Collections.Generic;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ProjectAdd : BackgroundBasePage
	{
        public TextBox tbProjectName;
        public DropDownList ddlProjectType;
        public RadioButtonList rblIsContract;
        public PlaceHolder phContract;
        public TextBox tbAmountTotal;
        public TextBox tbAmountCashBack;
        public TextBox tbContractNO;
        public TextBox tbDescription;
        public DropDownList ddlIsClosed;
        public DropDownList ddlUserNameAM;
        public DropDownList ddlUserNamePM;
        public CheckBoxList cblUserNameCollection;
        public DateTimeTextBox tbAddDate;

        private bool isClosed;
        private int projectID;

        public static string GetShowPopWinStringToAdd(bool isClosed)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("isClosed", isClosed.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加项目", "modal_projectAdd.aspx", arguments);
        }

        public static string GetShowPopWinStringToEdit(bool isClosed, int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("isClosed", isClosed.ToString());
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改项目", "modal_projectAdd.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.isClosed = TranslateUtils.ToBool(base.Request.QueryString["isClosed"]);
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);

			if (!IsPostBack)
			{
                List<string> typeList = ConfigurationManager.Additional.ProjectTypeCollection;
                foreach (string type in typeList)
                {
                    ListItem listItem = new ListItem(type, type);
                    this.ddlProjectType.Items.Add(listItem);
                }

                EBooleanUtils.AddListItems(this.rblIsContract, "合同项目", "非合同项目");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsContract, true.ToString());

                EBooleanUtils.AddListItems(this.ddlIsClosed, "已结束", "进行中");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsClosed, false.ToString());

                ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList();
                foreach (string userName in userNameArrayList)
                {
                    string displayName = AdminManager.GetDisplayName(userName, true);
                    this.cblUserNameCollection.Items.Add(new ListItem(displayName, userName));
                    this.ddlUserNameAM.Items.Add(new ListItem(displayName, userName));
                    this.ddlUserNamePM.Items.Add(new ListItem(displayName, userName));
                }

                if (this.projectID > 0)
                {
                    ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(this.projectID);
                    if (projectInfo != null)
                    {
                        this.tbProjectName.Text = projectInfo.ProjectName;
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlProjectType, projectInfo.ProjectType);
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsContract, projectInfo.IsContract.ToString());
                        this.tbAmountTotal.Text = projectInfo.AmountTotal.ToString();
                        this.tbAmountCashBack.Text = projectInfo.AmountCashBack.ToString();
                        this.tbContractNO.Text = projectInfo.ContractNO;
                        this.tbDescription.Text = projectInfo.Description;
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlIsClosed, projectInfo.IsClosed.ToString());
                        ControlUtils.SelectListItemsIgnoreCase(this.cblUserNameCollection, projectInfo.UserNameCollection.Split(','));
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlUserNameAM, projectInfo.UserNameAM);
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlUserNamePM, projectInfo.UserNamePM);
                        this.tbAddDate.DateTime = projectInfo.AddDate;
                    }
                }

                this.rblIsContract_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phContract.Visible = TranslateUtils.ToBool(this.rblIsContract.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            ProjectInfo projectInfo = null;
				
			if (this.projectID > 0)
			{
				try
				{
                    projectInfo = DataProvider.ProjectDAO.GetProjectInfo(this.projectID);
                    if (projectInfo != null)
                    {
                        projectInfo.ProjectName = this.tbProjectName.Text;
                        projectInfo.ProjectType = this.ddlProjectType.SelectedValue;
                        projectInfo.IsContract = TranslateUtils.ToBool(this.rblIsContract.SelectedValue);
                        projectInfo.AmountTotal = TranslateUtils.ToInt(this.tbAmountTotal.Text);
                        projectInfo.AmountCashBack = TranslateUtils.ToInt(this.tbAmountCashBack.Text);
                        projectInfo.ContractNO = this.tbContractNO.Text;
                        projectInfo.AddDate = this.tbAddDate.DateTime;
                        projectInfo.Description = this.tbDescription.Text;
                        projectInfo.IsClosed = TranslateUtils.ToBool(this.ddlIsClosed.SelectedValue);
                        if (projectInfo.IsClosed)
                        {
                            projectInfo.CloseDate = DateTime.Now;
                        }
                        projectInfo.UserNameAM = this.ddlUserNameAM.SelectedValue;
                        projectInfo.UserNamePM = this.ddlUserNamePM.SelectedValue;
                        ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(ControlUtils.GetSelectedListControlValueCollection(this.cblUserNameCollection));
                        if (!userNameArrayList.Contains(projectInfo.UserNameAM))
                        {
                            userNameArrayList.Add(projectInfo.UserNameAM);
                        }
                        if (!userNameArrayList.Contains(projectInfo.UserNamePM))
                        {
                            userNameArrayList.Add(projectInfo.UserNamePM);
                        }
                        projectInfo.UserNameCollection = TranslateUtils.ObjectCollectionToString(userNameArrayList);
                    }
                    DataProvider.ProjectDAO.Update(projectInfo);

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "项目修改失败！");
				}
			}
			else
			{
                ArrayList interactNameArrayList = DataProvider.ProjectDAO.GetProjectNameArrayList();
                if (interactNameArrayList.IndexOf(this.tbProjectName.Text) != -1)
				{
                    base.FailMessage("项目添加失败，项目名称已存在！");
                }
				else
				{
					try
					{
                        ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(ControlUtils.GetSelectedListControlValueCollection(this.cblUserNameCollection));
                        if (!userNameArrayList.Contains(this.ddlUserNameAM.SelectedValue))
                        {
                            userNameArrayList.Add(this.ddlUserNameAM.SelectedValue);
                        }
                        if (!userNameArrayList.Contains(this.ddlUserNamePM.SelectedValue))
                        {
                            userNameArrayList.Add(this.ddlUserNamePM.SelectedValue);
                        }

                        projectInfo = new ProjectInfo(0, this.tbProjectName.Text, this.ddlProjectType.SelectedValue, TranslateUtils.ToBool(this.rblIsContract.SelectedValue), TranslateUtils.ToInt(this.tbAmountTotal.Text), TranslateUtils.ToInt(this.tbAmountCashBack.Text), this.tbContractNO.Text, this.tbDescription.Text, DateTime.Now, TranslateUtils.ToBool(this.ddlIsClosed.SelectedValue), DateTime.Now, this.ddlUserNameAM.SelectedValue, this.ddlUserNamePM.SelectedValue, TranslateUtils.ObjectCollectionToString(userNameArrayList), string.Empty);

                        this.projectID = DataProvider.ProjectDAO.Insert(projectInfo);

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "项目添加失败！");
					}
				}
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundProject.GetRedirectUrl(this.isClosed) + "#" + this.projectID);
			}
		}
	}
}
