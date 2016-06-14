using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSearchwordSelect : BackgroundBasePage
    {
        public ListBox NodeIDCollectionToAllow;
        public ListBox NodeIDCollectionToNoAllow;
        public Button SubmitButton;
        public RadioButtonList rbIsAllow;
        public PlaceHolder phAllow;
        public PlaceHolder phNoAllow;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Searchword, "搜索范围设置", AppManager.CMS.Permission.WebSite.Searchword);
                this.phAllow.Visible = this.phNoAllow.Visible = false;

                SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(base.PublishmentSystemID);
                if (settingInfo == null)
                    settingInfo = new SearchwordSettingInfo();
                EBooleanUtils.AddListItems(this.rbIsAllow, "允许", "不允许");
                ControlUtils.SelectListItems(this.rbIsAllow, settingInfo.IsAllow.ToString());
                if (settingInfo.IsAllow)
                    this.phAllow.Visible = true;
                else
                    this.phNoAllow.Visible = true;
                NodeManager.AddListItems(this.NodeIDCollectionToAllow.Items, base.PublishmentSystemInfo, false, true);
                NodeManager.AddListItems(this.NodeIDCollectionToNoAllow.Items, base.PublishmentSystemInfo, false, true);
                ControlUtils.SelectListItems(this.NodeIDCollectionToAllow, TranslateUtils.StringCollectionToArrayList(settingInfo.InNode));
                ControlUtils.SelectListItems(this.NodeIDCollectionToNoAllow, TranslateUtils.StringCollectionToArrayList(settingInfo.NotInNode));

                this.SubmitButton.Attributes.Add("onclick", "return confirm(\"此操作将设置搜索范围，确定吗？\");");
            }
        }

        protected void rbIsAllow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.rbIsAllow.SelectedValue))
            {
                this.phAllow.Visible = true;
                this.phNoAllow.Visible = false;
            }
            else
            {
                this.phAllow.Visible = false;
                this.phNoAllow.Visible = true;
            }
        }

        public void SubmitButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                //允许
                ArrayList selectedNodeIDArrayListToAllow = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDCollectionToAllow);
                //不允许
                ArrayList selectedNodeIDArrayListToNoAllow = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDCollectionToNoAllow);

                SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(base.PublishmentSystemID);
                if (settingInfo == null)
                    settingInfo = new SearchwordSettingInfo();
                settingInfo.PublishmentSystemID = base.PublishmentSystemID;
                settingInfo.IsAllow = TranslateUtils.ToBool(this.rbIsAllow.SelectedValue);

                ArrayList inNodeIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDCollectionToAllow);
                settingInfo.InNode = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(inNodeIDArrayList);

                ArrayList notInNodeIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDCollectionToNoAllow);
                settingInfo.NotInNode = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(notInNodeIDArrayList);
                if (settingInfo != null && settingInfo.ID == 0)
                {
                    DataProvider.SearchwordSettingDAO.Insert(settingInfo);
                }
                else
                {
                    DataProvider.SearchwordSettingDAO.Update(settingInfo);
                }
            }
        }

    }
}
