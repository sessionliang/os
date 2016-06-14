using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerEdit : BackgroundBasePage
	{
        public RadioButtonList IsTracker;

        public TextBox TrackerDays;
        public TextBox TrackerPageView;
        public TextBox TrackerUniqueVisitor;
        public TextBox TrackerCurrentMinute;
        public DropDownList TrackerStyle;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "修改统计设置", AppManager.CMS.Permission.WebSite.Tracking);

                EBooleanUtils.AddListItems(this.IsTracker, "统计", "不统计");
                ControlUtils.SelectListItemsIgnoreCase(this.IsTracker, base.PublishmentSystemInfo.Additional.IsTracker.ToString());

                this.TrackerDays.Text = base.PublishmentSystemInfo.Additional.TrackerDays.ToString();
                this.TrackerPageView.Text = base.PublishmentSystemInfo.Additional.TrackerPageView.ToString();
                this.TrackerUniqueVisitor.Text = base.PublishmentSystemInfo.Additional.TrackerUniqueVisitor.ToString();
                this.TrackerCurrentMinute.Text = base.PublishmentSystemInfo.Additional.TrackerCurrentMinute.ToString();
                ETrackerStyleUtils.AddListItems(this.TrackerStyle);
                ControlUtils.SelectListItems(this.TrackerStyle, ETrackerStyleUtils.GetValue(base.PublishmentSystemInfo.Additional.TrackerStyle));
            }
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsTracker = TranslateUtils.ToBool(this.IsTracker.SelectedValue);

                base.PublishmentSystemInfo.Additional.TrackerDays = int.Parse(this.TrackerDays.Text);
                base.PublishmentSystemInfo.Additional.TrackerPageView = int.Parse(this.TrackerPageView.Text);
                base.PublishmentSystemInfo.Additional.TrackerUniqueVisitor = int.Parse(this.TrackerUniqueVisitor.Text);
                base.PublishmentSystemInfo.Additional.TrackerCurrentMinute = int.Parse(this.TrackerCurrentMinute.Text);
                base.PublishmentSystemInfo.Additional.TrackerStyle = ETrackerStyleUtils.GetEnumType(this.TrackerStyle.SelectedValue);

				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改统计设置");
					base.SuccessMessage("统计设置修改成功！");
				}
				catch(Exception ex)
				{
					base.FailMessage(ex, "统计设置修改失败！");
				}
			}
		}

	}
}
