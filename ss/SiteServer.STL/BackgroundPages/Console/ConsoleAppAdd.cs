using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleAppAdd : BackgroundBasePage
    {
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public Repeater rptContents;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "创建新应用", AppManager.Platform.Permission.Platform_Site);

                List<EPublishmentSystemType> publishmentSystemTypeList = new List<EPublishmentSystemType>();
                if (FileConfigManager.Instance.IsSaas)
                {
                    publishmentSystemTypeList.Add(EPublishmentSystemType.Weixin);
                    publishmentSystemTypeList.Add(EPublishmentSystemType.WeixinB2C);
                }
                else
                {
                    publishmentSystemTypeList = EPublishmentSystemTypeUtils.AllList();
                    publishmentSystemTypeList.Remove(EPublishmentSystemType.UserCenter);
                }
                this.rptContents.DataSource = publishmentSystemTypeList;
                this.rptContents.ItemDataBound += rptContents_ItemDataBound;
                this.rptContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EPublishmentSystemType publishmentSystemType = (EPublishmentSystemType)e.Item.DataItem;

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            bool isEnabled = EPublishmentSystemTypeUtils.IsEnabled(publishmentSystemType);
            if (isEnabled)
            {
                isEnabled = AppManager.IsDirectoryExists(EPublishmentSystemTypeUtils.GetValue(publishmentSystemType));
            }
            if (!FileConfigManager.Instance.IsSaas && !isEnabled)
            {
                e.Item.Visible = false;
            }

            string redirectUrl = ConsolePublishmentSystemAdd.GetRedirectUrl(publishmentSystemType);
            if (FileConfigManager.Instance.IsSaas)
            {
                redirectUrl = ConsolePublishmentSystemAddSaas.GetRedirectUrl(publishmentSystemType);
            }
            string iconHtml = EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemType, "icon-5");
            string description = EPublishmentSystemTypeUtils.GetDescription(publishmentSystemType);
            //|| EPublishmentSystemTypeUtils.Equals(publishmentSystemType, EPublishmentSystemType.BBS)  || (EPublishmentSystemTypeUtils.Equals(publishmentSystemType, EPublishmentSystemType.B2C))
            if (!isEnabled || EPublishmentSystemTypeUtils.Equals(publishmentSystemType, EPublishmentSystemType.BBS) || (EPublishmentSystemTypeUtils.Equals(publishmentSystemType, EPublishmentSystemType.B2C)) || (EPublishmentSystemTypeUtils.Equals(publishmentSystemType, EPublishmentSystemType.WeixinB2C)))
            {
                e.Item.Visible = false;
            }

            //设置B2C，WeiXinB2c为即将开通)
            //if (!isEnabled)
            //{
            //    redirectUrl = "javascript:;";
            //    iconHtml = EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemType, "icon-5 notavaliable");
            //    description += "<br><span style='color:#cd0000'>（暂未开通）</span>";
            //}

            string appName = EPublishmentSystemTypeUtils.GetText(publishmentSystemType);

            ltlHtml.Text = string.Format(@"
  <span class=""icon-span"">
    <a href=""{0}"">
      {1}
      <h5>
        创建{2}应用
        <br>
        <small>{3}</small>
      </h5>
    </a>
  </span>
", redirectUrl, iconHtml, appName, description);
        }
    }
}
