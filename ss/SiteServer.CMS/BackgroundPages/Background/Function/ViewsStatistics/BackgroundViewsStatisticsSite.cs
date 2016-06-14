using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model.Service;
using System.Collections;
using System.Collections.Generic;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundViewsStatisticsSite : BackgroundBasePage
    {
        public RadioButtonList IsIntelligentPushCount;
        public PlaceHolder phDate;
        public RadioButtonList rblDate;

        private ArrayList relatedIdentities;
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.phDate.Visible = false;

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.Site, base.PublishmentSystemID, base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_IntelligentPush, "智能推送设置", AppManager.CMS.Permission.WebSite.IntelligentPush);

                EBooleanUtils.AddListItems(this.IsIntelligentPushCount, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsIntelligentPushCount, base.PublishmentSystemInfo.Additional.IsIntelligentPushCount.ToString());

                EIntelligentPushTypeUtils.AddListItems(this.rblDate);
                ControlUtils.SelectListItemsIgnoreCase(this.rblDate, base.PublishmentSystemInfo.Additional.IntelligentPushType.ToString());
            }
            if (base.PublishmentSystemInfo.Additional.IsIntelligentPushCount == false)
            {
                this.phDate.Visible = false;
            }
            else
            {
                this.phDate.Visible = true;
            }
        }

        public void IsIntelligentPushCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EBooleanUtils.Equals(EBoolean.False, this.IsIntelligentPushCount.SelectedValue))
            {
                this.phDate.Visible = false;
                this.rblDate.SelectedIndex = 0;
            }
            else
            {
                this.phDate.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    base.PublishmentSystemInfo.Additional.IsIntelligentPushCount = TranslateUtils.ToBool(this.IsIntelligentPushCount.SelectedValue);

                    if (EBooleanUtils.Equals(EBoolean.False, this.IsIntelligentPushCount.SelectedValue))
                    {
                        this.rblDate.SelectedIndex = 0;
                    }
                    base.PublishmentSystemInfo.Additional.IntelligentPushType = EIntelligentPushTypeUtils.GetEnumType(this.rblDate.SelectedValue);

                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "智能推送设置修改成功");

                    base.SuccessMessage("智能推送设置修改成功！");

                    if (base.PublishmentSystemInfo.Additional.IsIntelligentPushCount == false)
                    {
                        this.phDate.Visible = false;
                    }
                    else
                    {
                        this.phDate.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "智能推送设置修改失败！");
                }
            }
        }
    }
}
