using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using System.Data.OleDb;
using BaiRong.Controls;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserSelect : BackgroundBasePage
    {
        public TextBox Keyword;
        public DropDownList CreateDate;
        public DropDownList LastActivityDate;
        //public DropDownList ddlTypeID;
        public DropDownList ddlLevelID;
        //public DropDownList ddlDepartmentID;
        //public DropDownList ddlAreaID;

        public Repeater rptContents;
        public SqlPager spContents;

        private string textBoxID;
        private bool[] isLastNodeArrayOfDepartment;
        private bool[] isLastNodeArrayOfArea;

        public static string GetOpenWindowString(string textBoxID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("textBoxID", textBoxID);
            return JsUtils.OpenWindow.GetOpenWindowString("选择用户", "modal_userSelect.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.textBoxID = base.Request.QueryString["textBoxID"];

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = 25;

            if (string.IsNullOrEmpty(base.Request.QueryString["levelID"]))
            {
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(string.Empty, true);
            }
            else
            {
                bool isLockedOutSet = !string.IsNullOrEmpty(base.Request.QueryString["isLockedOut"]);
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(string.Empty, TranslateUtils.ToInt(base.Request.QueryString["levelID"]), base.Request.QueryString["keyword"], TranslateUtils.ToInt(base.Request.QueryString["createDate"]), TranslateUtils.ToInt(base.Request.QueryString["lastActivityDate"]), true);
            }

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {

                //this.ddlDepartmentID.Items.Add(new ListItem("<全部>", "0"));
                //int departmentID = TranslateUtils.ToInt(base.Request.QueryString["DepartmentID"]);
                //ArrayList departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
                //int count = departmentIDArrayList.Count;
                //this.isLastNodeArrayOfDepartment = new bool[count];
                //foreach (int theDepartmentID in departmentIDArrayList)
                //{
                //    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                //    ListItem listitem = new ListItem(this.GetDepartment(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), theDepartmentID.ToString());
                //    if (departmentID == theDepartmentID)
                //    {
                //        listitem.Selected = true;
                //    }
                //    this.ddlDepartmentID.Items.Add(listitem);
                //}

                //this.ddlAreaID.Items.Add(new ListItem("<全部>", "0"));
                //int areaID = TranslateUtils.ToInt(base.Request.QueryString["AreaID"]);
                //ArrayList areaIDArrayList = AreaManager.GetAreaIDArrayList();
                //count = areaIDArrayList.Count;
                //this.isLastNodeArrayOfArea = new bool[count];
                //foreach (int theAreaID in areaIDArrayList)
                //{
                //    AreaInfo areaInfo = AreaManager.GetAreaInfo(theAreaID);
                //    ListItem listitem = new ListItem(this.GetArea(areaInfo.AreaID, areaInfo.AreaName, areaInfo.ParentsCount, areaInfo.IsLastNode), theAreaID.ToString());
                //    if (areaID == theAreaID)
                //    {
                //        listitem.Selected = true;
                //    }
                //    this.ddlAreaID.Items.Add(listitem);
                //}

                ListItem theListItem = new ListItem("全部", "0");
                theListItem.Selected = true;
                this.ddlLevelID.Items.Add(theListItem);
                ArrayList userLevelInfoArrayList = UserLevelManager.GetLevelInfoArrayList(string.Empty);
                foreach (UserLevelInfo userLevelInfo in userLevelInfoArrayList)
                {
                    ListItem listitem = new ListItem(userLevelInfo.LevelName, userLevelInfo.ID.ToString());
                    this.ddlLevelID.Items.Add(listitem);
                }

                if (!string.IsNullOrEmpty(base.Request.QueryString["levelID"]))
                {
                    this.Keyword.Text = base.Request.QueryString["keyword"];
                    ControlUtils.SelectListItems(this.CreateDate, base.Request.QueryString["createDate"]);
                    ControlUtils.SelectListItems(this.LastActivityDate, base.Request.QueryString["lastActivityDate"]);
                    //ControlUtils.SelectListItems(this.ddlTypeID, base.Request.QueryString["typeID"]);
                    //ControlUtils.SelectListItems(this.ddlDepartmentID, base.Request.QueryString["departmentID"]);
                    //ControlUtils.SelectListItems(this.ddlAreaID, base.Request.QueryString["areaID"]);
                    ControlUtils.SelectListItems(this.ddlLevelID, base.Request.QueryString["levelID"]);
                }

                this.spContents.DataBind();
            }
        }

        private string GetDepartment(int departmentID, string departmentName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArrayOfDepartment[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, departmentName);
            return str;
        }

        private string GetArea(int areaID, string areaName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArrayOfArea[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArrayOfArea[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArrayOfArea[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, areaName);
            return str;
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);
                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(userInfo.UserName);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlevel = (Literal)e.Item.FindControl("ltLevel");
                //Literal ltlDepartmentID = (Literal)e.Item.FindControl("ltlDepartmentID");
                //Literal ltlAreaID = (Literal)e.Item.FindControl("ltlAreaID");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlCreateIPAddress = (Literal)e.Item.FindControl("ltlCreateIPAddress");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlevel.Text = UserLevelManager.GetLevelName(userInfo.GroupSN, userInfo.LevelID);
                //ltlDepartmentID.Text = userContactInfo.Department;
                //ltlAreaID.Text = userContactInfo.Position;
                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCreateIPAddress.Text = userInfo.CreateIPAddress;
                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserNameCollection"" value=""{0}"" />", userInfo.UserName);
            }
        }

        private string GetUserNameHtml(string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(userName);
            string state = string.Empty;
            if (isLockedOut)
            {
                state = @"<span style=""color:red;"">[已被锁定]</span>";
            }
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>{2}", showPopWinString, userName, state);
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["UserNameCollection"]);

            if (userNameArrayList.Count == 0)
            {
                base.FailMessage("请勾选所需用户");
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, string.Format(@"
var textBox = parent.$('#{0}');
if (textBox.val()){{
    textBox.val(textBox.val() + ',{1}');
}}else{{
    textBox.val('{1}');
}}
", this.textBoxID, base.Request.Form["UserNameCollection"]));
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("modal_userSelect.aspx?keyword={0}&createDate={1}&lastActivityDate={2}&levelID={3}", this.Keyword.Text, this.CreateDate.SelectedValue, this.LastActivityDate.SelectedValue, this.ddlLevelID.SelectedValue);
                }
                return this._pageUrl;
            }
        }
    }
}
