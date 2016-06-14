using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundWebMenuAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;
        public TextBox tbMenuName;

        public PlaceHolder phNavigationType;
        public DropDownList ddlNavigationType;

        public PlaceHolder phFunction;
        public Button btnFunctionSelect;

        public PlaceHolder phUrl;
        public TextBox tbUrl;

        public PlaceHolder phSite;
        public Button btnContentSelect;
        public Button btnChannelSelect;

        public Literal ltlScript;

        private int parentID;
        private int menuID;

        public static string GetRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_webMenuAdd.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}", publishmentSystemID, parentID, menuID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.menuID = TranslateUtils.ToInt(base.GetQueryString("menuID"));
            this.parentID = TranslateUtils.ToInt(base.GetQueryString("parentID"));

			if (!IsPostBack)
			{
                ENavigationTypeUtils.AddListItems(this.ddlNavigationType);

                WebMenuInfo menuInfo = DataProviderWX.WebMenuDAO.GetMenuInfo(this.menuID);
                if (menuInfo == null)
                {
                    this.menuID = 0;
                }

                if (this.menuID == 0)
                {
                    this.ltlPageTitle.Text = string.Format("添加{0}菜单", this.parentID == 0 ? "主" : "子");
                }
                else
                {
                    this.ltlPageTitle.Text = string.Format("修改{0}菜单（{1}）", this.parentID == 0 ? "主" : "子", menuInfo.MenuName);

                    this.tbMenuName.Text = menuInfo.MenuName;
                    ControlUtils.SelectListItems(this.ddlNavigationType, menuInfo.NavigationType);
                    this.tbUrl.Text = menuInfo.Url;
                    this.ltlScript.Text = string.Format("<script>{0}</script>", this.GetFunctionOrChannelOrContentSelectScript(menuInfo));
                }

                this.ddlNavigationType_OnSelectedIndexChanged(null, EventArgs.Empty);

                this.btnFunctionSelect.Attributes.Add("onclick", "parent." + Modal.FunctionSelect.GetOpenWindowString(base.PublishmentSystemID, "selectKeyword"));

                this.btnContentSelect.Attributes.Add("onclick", "parent." + Modal.ContentSelect.GetOpenWindowString(base.PublishmentSystemID, false, "contentSelect"));
                this.btnChannelSelect.Attributes.Add("onclick", "parent." + SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowString(base.PublishmentSystemID));
			}
		}

        public string GetFunctionOrChannelOrContentSelectScript(WebMenuInfo menuInfo)
        {
            if (ENavigationTypeUtils.Equals(menuInfo.NavigationType, ENavigationType.Function))
            {
                if (menuInfo.FunctionID > 0)
                {
                    string functionName = KeywordManager.GetFunctionName(EKeywordTypeUtils.GetEnumType(menuInfo.KeywordType), menuInfo.FunctionID);
                    return string.Format(@"selectKeyword(""{0},{1},{2}"")", menuInfo.KeywordType, menuInfo.FunctionID, functionName);
                }
            }
            else if (ENavigationTypeUtils.Equals(menuInfo.NavigationType, ENavigationType.Site))
            {
                if (menuInfo.ContentID > 0)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, menuInfo.ChannelID);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, menuInfo.ChannelID);

                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, menuInfo.ContentID);

                    string pageUrl = PageUtilityWX.GetContentUrl(base.PublishmentSystemInfo, contentInfo);

                    return string.Format(@"contentSelect(""{0}"", ""{1}"", ""{2}"", ""{3}"")", contentInfo.Title, menuInfo.ChannelID, menuInfo.ContentID, pageUrl);
                }
                else if (menuInfo.ChannelID > 0)
                {
                    string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, menuInfo.ChannelID);
                    string pageUrl = PageUtilityWX.GetChannelUrl(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, menuInfo.ChannelID));
                    return string.Format("selectChannel('{0}', '{1}', '{2}');", nodeNames, menuInfo.ChannelID, pageUrl);
                }
            }
            
            return string.Empty;
        }

        public void ddlNavigationType_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            bool isHideAll = false;
            if (this.parentID == 0 && this.menuID > 0)
            {
                int childrenCount = DataProviderWX.WebMenuDAO.GetCount(base.PublishmentSystemID, this.menuID);
                if (childrenCount > 0)
                {
                    isHideAll = true;
                }
            }

            if (isHideAll)
            {
                this.phNavigationType.Visible = this.phUrl.Visible = this.phFunction.Visible = this.phSite.Visible = false;
            }
            else
            {
                ENavigationType navigationType = ENavigationTypeUtils.GetEnumType(this.ddlNavigationType.SelectedValue);

                this.phUrl.Visible = this.phFunction.Visible = this.phSite.Visible = false;

                if (navigationType == ENavigationType.Url)
                {
                    this.phUrl.Visible = true;
                }
                else if (navigationType == ENavigationType.Function)
                {
                    this.phFunction.Visible = true;
                }
                else if (navigationType == ENavigationType.Site)
                {
                    this.phSite.Visible = true;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				try
				{
                    WebMenuInfo menuInfo = new WebMenuInfo();
                    if (this.menuID > 0)
                    {
                        menuInfo = DataProviderWX.WebMenuDAO.GetMenuInfo(this.menuID);
                    }

                    menuInfo.MenuName = this.tbMenuName.Text;
                    menuInfo.ParentID = this.parentID;
                    menuInfo.PublishmentSystemID = base.PublishmentSystemID;
                    menuInfo.NavigationType = this.ddlNavigationType.SelectedValue;

                    if (ENavigationTypeUtils.Equals(menuInfo.NavigationType, ENavigationType.Url))
                    {
                        menuInfo.Url = this.tbUrl.Text;
                    }
                    else if (ENavigationTypeUtils.Equals(menuInfo.NavigationType, ENavigationType.Function))
                    {
                        string keywordType = base.Request.Form["keywordType"];
                        int functionID = TranslateUtils.ToInt(base.Request.Form["functionID"]);
                        if (!string.IsNullOrEmpty(keywordType) && functionID > 0)
                        {
                            menuInfo.KeywordType = keywordType;
                            menuInfo.FunctionID = functionID;
                        }
                        else
                        {
                            base.FailMessage("菜单保存失败，必须选择微功能页面，请点击下方按钮进行选择");
                            return;
                        }
                    }
                    else if (ENavigationTypeUtils.Equals(menuInfo.NavigationType, ENavigationType.Site))
                    {
                        string idsCollection = base.Request.Form["idsCollection"];
                        if (!string.IsNullOrEmpty(idsCollection))
                        {
                            menuInfo.ChannelID = TranslateUtils.ToInt(idsCollection.Split('_')[0]);
                            menuInfo.ContentID = TranslateUtils.ToInt(idsCollection.Split('_')[1]);
                        }

                        if (menuInfo.ChannelID == 0 && menuInfo.ContentID == 0)
                        {
                            base.FailMessage("菜单保存失败，必须选择微网站页面，请点击下方按钮进行选择");
                            return;
                        }
                    }

                    if (this.menuID > 0)
                    {
                        DataProviderWX.WebMenuDAO.Update(menuInfo);
                        base.SuccessMessage("底部导航菜单修改成功！");
                    }
                    else
                    {
                        this.menuID = DataProviderWX.WebMenuDAO.Insert(menuInfo);
                        base.SuccessMessage("底部导航菜单新增成功！");
                    }

                    string redirectUrl = BackgroundWebMenu.GetRedirectUrl(base.PublishmentSystemID, this.parentID, this.menuID);
                    this.ltlPageTitle.Text += string.Format(@"<script>parent.redirect('{0}');</script>", redirectUrl);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "底部导航菜单配置失败！");
				}
			}
		}
	}
}
