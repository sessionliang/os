using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationComments : BackgroundBasePage
	{
        public RadioButtonList IsContentCommentable;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                ControlUtils.SelectListItemsIgnoreCase(this.IsContentCommentable, base.PublishmentSystemInfo.Additional.IsContentCommentable.ToString());
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.Additional.IsContentCommentable = TranslateUtils.ToBool(this.IsContentCommentable.SelectedValue);

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改信息评论设置");

                    base.SuccessMessage("全局设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "全局设置修改失败！");
                }
            }
        }
	}
}
