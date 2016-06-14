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
	public class BackgroundMenuAdd : BackgroundBasePageWX
	{
        public Literal ltlPageTitle;
        public TextBox tbMenuName;

        public PlaceHolder phMenuType;
        public DropDownList ddlMenuType;

        public PlaceHolder phKeyword;
        public TextBox tbKeyword;
        public Button btnKeywordSelect;

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
            return PageUtils.GetWXUrl(string.Format("background_menuAdd.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}", publishmentSystemID, parentID, menuID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.menuID = TranslateUtils.ToInt(base.GetQueryString("menuID"));
            this.parentID = TranslateUtils.ToInt(base.GetQueryString("parentID"));

			if (!IsPostBack)
			{
                EMenuTypeUtils.AddListItems(this.ddlMenuType);

                MenuInfo menuInfo = DataProviderWX.MenuDAO.GetMenuInfo(this.menuID);
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
                    ControlUtils.SelectListItems(this.ddlMenuType, EMenuTypeUtils.GetValue(menuInfo.MenuType));
                    this.tbKeyword.Text = menuInfo.Keyword;
                    this.tbUrl.Text = menuInfo.Url;
                    this.ltlScript.Text = string.Format("<script>{0}</script>", MPUtils.GetChannelOrContentSelectScript(base.PublishmentSystemInfo, menuInfo.ChannelID, menuInfo.ContentID));
                }

                this.ddlMenuType_OnSelectedIndexChanged(null, EventArgs.Empty);

                this.btnKeywordSelect.Attributes.Add("onclick", "parent." + Modal.KeywordSelect.GetOpenWindowString(base.PublishmentSystemID, "selectKeyword"));

                this.btnContentSelect.Attributes.Add("onclick", "parent." + Modal.ContentSelect.GetOpenWindowString(base.PublishmentSystemID, false, "contentSelect"));
                this.btnChannelSelect.Attributes.Add("onclick", "parent." + SiteServer.CMS.BackgroundPages.Modal.ChannelSelect.GetOpenWindowString(base.PublishmentSystemID));
			}
		}

        public void ddlMenuType_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            bool isHideAll = false;
            if (this.parentID == 0 && this.menuID > 0)
            {
                int childrenCount = DataProviderWX.MenuDAO.GetCount(this.menuID);
                if (childrenCount > 0)
                {
                    isHideAll = true;
                }
            }

            if (isHideAll)
            {
                this.phMenuType.Visible = this.phUrl.Visible = this.phKeyword.Visible = this.phSite.Visible = false;
            }
            else
            {
                EMenuType menuType = EMenuTypeUtils.GetEnumType(this.ddlMenuType.SelectedValue);

                this.phUrl.Visible = this.phKeyword.Visible = this.phSite.Visible = false;

                if (menuType == EMenuType.Url)
                {
                    this.phUrl.Visible = true;
                }
                else if (menuType == EMenuType.Keyword)
                {
                    this.phKeyword.Visible = true;
                }
                else if (menuType == EMenuType.Site)
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
                    MenuInfo menuInfo = new MenuInfo();
                    if (this.menuID > 0)
                    {
                        menuInfo = DataProviderWX.MenuDAO.GetMenuInfo(this.menuID);
                    }

                    menuInfo.MenuName = this.tbMenuName.Text;
                    menuInfo.ParentID = this.parentID;
                    menuInfo.PublishmentSystemID = base.PublishmentSystemID;
                    menuInfo.MenuType = EMenuTypeUtils.GetEnumType(this.ddlMenuType.SelectedValue);

                    if (this.phMenuType.Visible)
                    {
                        if (menuInfo.MenuType == EMenuType.Keyword)
                        {
                            if (!DataProviderWX.KeywordMatchDAO.IsExists(base.PublishmentSystemID, this.tbKeyword.Text))
                            {
                                base.FailMessage("菜单保存失败，所填关键词不存在，请先在关键词回复中添加");
                                return;
                            }
                        }
                        else if (menuInfo.MenuType == EMenuType.Site)
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
                    }

                    if (this.parentID > 0)
                    {
                        if (StringUtils.GetByteCount(this.tbMenuName.Text) > 26)
                        {
                            base.FailMessage("菜单保存失败，子菜单菜名称不能超过26个字节（13个汉字）");
                            return;
                        }
                    }
                    else
                    {
                        if (StringUtils.GetByteCount(this.tbMenuName.Text) > 16)
                        {
                            base.FailMessage("菜单保存失败，子菜单菜名称不能超过16个字节（8个汉字）");
                            return;
                        }
                    }
                    if (menuInfo.MenuType == EMenuType.Url)
                    {
                        if (StringUtils.GetByteCount(this.tbUrl.Text) > 256)
                        {
                            base.FailMessage("菜单保存失败，菜单网址不能超过256个字节");
                            return;
                        }
                    }

                    if (menuInfo.MenuType == EMenuType.Url)
                    {
                        menuInfo.Url = this.tbUrl.Text;
                    }
                    else if (menuInfo.MenuType == EMenuType.Keyword)
                    {
                        menuInfo.Keyword = this.tbKeyword.Text;
                    }
                    else if (menuInfo.MenuType == EMenuType.Site)
                    {
                        string idsCollection = base.Request.Form["idsCollection"];
                        if (!string.IsNullOrEmpty(idsCollection))
                        {
                            menuInfo.ChannelID = TranslateUtils.ToInt(idsCollection.Split('_')[0]);
                            menuInfo.ContentID = TranslateUtils.ToInt(idsCollection.Split('_')[1]);
                        }
                    }

                    if (this.menuID > 0)
                    {
                        DataProviderWX.MenuDAO.Update(menuInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "修改自定义菜单");
                        base.SuccessMessage("自定义菜单修改成功！");
                    }
                    else
                    {
                        this.menuID = DataProviderWX.MenuDAO.Insert(menuInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "新增自定义菜单");
                        base.SuccessMessage("自定义菜单新增成功！");
                    }

                    string redirectUrl = BackgroundMenu.GetRedirectUrl(base.PublishmentSystemID, this.parentID, this.menuID);
                    this.ltlPageTitle.Text += string.Format(@"<script>parent.redirect('{0}');</script>", redirectUrl);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "自定义菜单配置失败！");
				}
			}
		}
	}
}
