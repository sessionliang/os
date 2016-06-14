using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationInnerLink : BackgroundBasePage
	{
		public RadioButtonList IsInnerLink;

        public PlaceHolder phInnerLink;

        public DropDownList IsInnerLinkByChannelName;
        public TextBox InnerLinkFormatString;
        public TextBox InnerLinkMaxNum;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_InnerLink, "站内链接设置", AppManager.CMS.Permission.WebSite.InnerLink);

                EBooleanUtils.AddListItems(this.IsInnerLink, "启用", "禁用");
                ControlUtils.SelectListItemsIgnoreCase(this.IsInnerLink, base.PublishmentSystemInfo.Additional.IsInnerLink.ToString());

                EBooleanUtils.AddListItems(this.IsInnerLinkByChannelName);
                ControlUtils.SelectListItemsIgnoreCase(this.IsInnerLinkByChannelName, base.PublishmentSystemInfo.Additional.IsInnerLinkByChannelName.ToString());

                this.InnerLinkFormatString.Text = base.PublishmentSystemInfo.Additional.InnerLinkFormatString;

                this.InnerLinkMaxNum.Text = base.PublishmentSystemInfo.Additional.InnerLinkMaxNum.ToString();

				this.IsInnerLink_SelectedIndexChanged(null, null);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                //if (!StringUtils.Contains(this.InnerLinkFormatString.Text, "{0}") || !StringUtils.Contains(this.InnerLinkFormatString.Text, "{1}"))
                //{
                //    base.FailMessage("站内链接显示代码必须包含{0}及{1}！");
                //    return;
                //}
                base.PublishmentSystemInfo.Additional.IsInnerLink = TranslateUtils.ToBool(this.IsInnerLink.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsInnerLinkByChannelName = TranslateUtils.ToBool(this.IsInnerLinkByChannelName.SelectedValue);
                base.PublishmentSystemInfo.Additional.InnerLinkFormatString = this.InnerLinkFormatString.Text;
                base.PublishmentSystemInfo.Additional.InnerLinkMaxNum = TranslateUtils.ToInt(this.InnerLinkMaxNum.Text, base.PublishmentSystemInfo.Additional.InnerLinkMaxNum);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改站内链接设置");

					base.SuccessMessage("站内链接设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "站内链接设置修改失败！");
				}
			}
		}

        public void IsInnerLink_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (EBooleanUtils.Equals(this.IsInnerLink.SelectedValue, EBoolean.True))
			{
                this.phInnerLink.Visible = true;
			}
			else
			{
                this.phInnerLink.Visible = false;
			}
		}
	}
}
