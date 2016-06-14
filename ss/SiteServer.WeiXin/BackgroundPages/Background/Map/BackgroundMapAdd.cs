using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Controls;
using System.Collections.Generic;
using System.Text;
using MapManager = SiteServer.WeiXin.Core.MapManager;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundMapAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public PlaceHolder phStep2;
        public TextBox tbMapWD;
        public Literal ltlMap;

        public HtmlInputHidden imageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int mapID;

        public static string GetRedirectUrl(int publishmentSystemID, int mapID)
        {
            return PageUtils.GetWXUrl(string.Format("background_mapAdd.aspx?publishmentSystemID={0}&mapID={1}", publishmentSystemID, mapID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.mapID = TranslateUtils.ToInt(base.GetQueryString("mapID"));

            if (!IsPostBack)
            {
                string pageTitle = this.mapID > 0 ? "编辑微导航" : "添加微导航";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Map, pageTitle, AppManager.WeiXin.Permission.WebSite.Map);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", MapManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.mapID > 0)
                {
                    MapInfo mapInfo = DataProviderWX.MapDAO.GetMapInfo(this.mapID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(mapInfo.KeywordID);
                    this.cbIsEnabled.Checked = !mapInfo.IsDisabled;
                    this.tbTitle.Text = mapInfo.Title;
                    if (!string.IsNullOrEmpty(mapInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, mapInfo.ImageUrl));
                    }
                    this.tbSummary.Text = mapInfo.Summary;

                    this.tbMapWD.Text = mapInfo.MapWD;

                    this.imageUrl.Value = mapInfo.ImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundMap.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        public void Preview_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "K", "<script>window.open(\"http://map.baidu.com/mobile/webapp/place/list/qt=s&wd=" + this.tbMapWD.Text + "/vt=map\");</script>");
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int selectedStep = 0;
                if (this.phStep1.Visible)
                {
                    selectedStep = 1;
                }
                else if (this.phStep2.Visible)
                {
                    selectedStep = 2;
                }

                this.phStep1.Visible = this.phStep2.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.mapID > 0)
                        {
                            MapInfo mapInfo = DataProviderWX.MapDAO.GetMapInfo(this.mapID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, mapInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                    }

                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                        this.phStep1.Visible = true;
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                        this.btnSubmit.Text = "确 认";
                    }
                }
                else if (selectedStep == 2)
                {
                    MapInfo mapInfo = new MapInfo();
                    if (this.mapID > 0)
                    {
                        mapInfo = DataProviderWX.MapDAO.GetMapInfo(this.mapID);
                    }
                    mapInfo.PublishmentSystemID = base.PublishmentSystemID;

                    mapInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.mapID > 0, this.tbKeywords.Text, EKeywordType.Map, mapInfo.KeywordID);
                    mapInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    mapInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    mapInfo.ImageUrl = this.imageUrl.Value; ;
                    mapInfo.Summary = this.tbSummary.Text;

                    mapInfo.MapWD = this.tbMapWD.Text;

                    try
                    {
                        if (this.mapID > 0)
                        {
                            DataProviderWX.MapDAO.Update(mapInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微导航", string.Format("微导航:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微导航成功！");
                        }
                        else
                        {
                            this.mapID = DataProviderWX.MapDAO.Insert(mapInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微导航", string.Format("微导航:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微导航成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_map.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微导航设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
            }
        }
    }
}
