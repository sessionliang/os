using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.Project.Core;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.Project.Model;
using SiteServer.Project.BackgroundPages.Modal;
using System.Collections;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundApplication : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public DropDownList ddlApplicationType;
        public TextBox tbApplyResource;
        public TextBox tbKeyword;

        private string type;

        public void Page_Load(object sender, EventArgs E)
        {
            this.type = base.Request.QueryString["type"];

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = 50;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (base.Request.QueryString["applicationType"] == null)
            {
                this.spContents.SelectCommand = DataProvider.ApplicationDAO.GetSelectSqlStringNotHandled();
            }
            else
            {
                string applicationType = base.Request.QueryString["applicationType"];
                string applyResource = base.Request.QueryString["applyResource"];
                string keyword = base.Request.QueryString["keyword"];

                this.spContents.SelectCommand = DataProvider.ApplicationDAO.GetSelectSqlStringNotHandled(applicationType, applyResource, keyword);
            }
            
            this.spContents.SortField = DataProvider.ApplicationDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("全部类型", string.Empty);
                this.ddlApplicationType.Items.Add(listItem);

                EApplicationTypeUtils.AddListItems(this.ddlApplicationType);

                if (base.Request.QueryString["applicationType"] != null)
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlApplicationType, base.Request.QueryString["applicationType"]);
                    this.tbApplyResource.Text = base.Request.QueryString["applyResource"];
                    this.tbKeyword.Text = base.Request.QueryString["keyword"];

                    if (EApplicationTypeUtils.Equals(base.Request.QueryString["applicationType"], EApplicationType.GeXia))
                    {
                        this.ddlApplicationType.Enabled = false;
                    }
                }

                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                EApplicationType applicationType = EApplicationTypeUtils.GetEnumType((string)DataBinder.Eval(e.Item.DataItem, "applicationType"));
                string applyResource = (string)DataBinder.Eval(e.Item.DataItem, "applyResource");
                string ipAddress = (string)DataBinder.Eval(e.Item.DataItem, "ipAddress");
                DateTime addDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "AddDate");
                string contactPerson = (string)DataBinder.Eval(e.Item.DataItem, "contactPerson");
                string email = (string)DataBinder.Eval(e.Item.DataItem, "email");
                string mobile = (string)DataBinder.Eval(e.Item.DataItem, "mobile");
                string qq = (string)DataBinder.Eval(e.Item.DataItem, "qq");
                string telephone = (string)DataBinder.Eval(e.Item.DataItem, "telephone");
                string location = (string)DataBinder.Eval(e.Item.DataItem, "location");
                string address = (string)DataBinder.Eval(e.Item.DataItem, "address");
                string orgType = (string)DataBinder.Eval(e.Item.DataItem, "orgType");
                string orgName = (string)DataBinder.Eval(e.Item.DataItem, "orgName");
                bool isITDepartment = TranslateUtils.ToBool((string)DataBinder.Eval(e.Item.DataItem, "isITDepartment"));
                string comment = (string)DataBinder.Eval(e.Item.DataItem, "comment");

                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlApplicationType = e.Item.FindControl("ltlApplicationType") as Literal;
                Literal ltlApplyResource = e.Item.FindControl("ltlApplyResource") as Literal;
                Literal ltlContactPerson = e.Item.FindControl("ltlContactPerson") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlQQ = e.Item.FindControl("ltlQQ") as Literal;
                Literal ltlTelephone = e.Item.FindControl("ltlTelephone") as Literal;
                Literal ltlLocation = e.Item.FindControl("ltlLocation") as Literal;
                Literal ltlOrgType = e.Item.FindControl("ltlOrgType") as Literal;
                Literal ltlOrgName = e.Item.FindControl("ltlOrgName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlHandle = e.Item.FindControl("ltlHandle") as Literal;

                ltlID.Text = (e.Item.ItemIndex + 1).ToString();
                ltlApplicationType.Text = EApplicationTypeUtils.GetText(applicationType);
                ltlApplyResource.Text = applyResource;
                ltlContactPerson.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.ApplicationView.GetShowPopWinString(id), contactPerson);
                ltlEmail.Text = email;
                ltlMobile.Text = mobile;
                ltlQQ.Text = qq;
                ltlTelephone.Text = telephone;
                ltlLocation.Text = location;
                ltlOrgType.Text = orgType;
                ltlOrgName.Text = orgName;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate, BaiRong.Model.EDateFormatType.Chinese, BaiRong.Model.ETimeFormatType.ShortTime);

                ltlHandle.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">设置<br />处理</a>", ApplicationSet.GetShowPopWinString(id, false));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundApplication.GetRedirectUrl(this.ddlApplicationType.SelectedValue, this.tbApplyResource.Text, this.tbKeyword.Text);
                }
                return _pageUrl;
            }
        }

        private static string GetRedirectUrl(string applicationType, string applyResource, string keyword)
        {
            return string.Format("product_application.aspx?applicationType={0}&applyResource={1}&keyword={2}", applicationType, applyResource, keyword);
        }
    }
}
