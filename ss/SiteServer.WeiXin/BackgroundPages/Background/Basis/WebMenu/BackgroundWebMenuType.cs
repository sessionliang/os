using System;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;
using BaiRong.Core.Integration;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundWebMenuType : BackgroundBasePage
	{
        //public TextBox tbWebMenuColor;
        //public RadioButtonList rblIsWebMenuLeft;

		public DataList dlContents;

        private EWebMenuType webMenuType;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_webMenuType.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.webMenuType = EWebMenuTypeUtils.GetEnumType(base.PublishmentSystemInfo.Additional.WX_WebMenuType);

			if (!IsPostBack)
            {
                base.InfoMessage("当前底部导航菜单风格：" + EWebMenuTypeUtils.GetText(this.webMenuType));

                //this.tbWebMenuColor.Text = base.PublishmentSystemInfo.Additional.WX_WebMenuColor;

                //EBooleanUtils.AddListItems(this.rblIsWebMenuLeft, "左对齐", "右对齐");
                //ControlUtils.SelectListItems(this.rblIsWebMenuLeft, base.PublishmentSystemInfo.Additional.WX_IsWebMenuLeft.ToString());

                this.dlContents.DataSource = EWebMenuTypeUtils.GetList();
                this.dlContents.ItemDataBound += dlContents_ItemDataBound;
                this.dlContents.DataBind();
			}
		}

        void dlContents_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                EWebMenuType menuType = (EWebMenuType)e.Item.DataItem;

                Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlRadio = e.Item.FindControl("ltlRadio") as Literal;

                string checkedStr = string.Empty;
                if (menuType == this.webMenuType)
                {
                    checkedStr = "checked";
                }

                ltlRadio.Text = string.Format(@"
<label class=""radio lead"">
  <input type=""radio"" name=""choose"" id=""choose{0}"" value=""{1}"" {2}>
  {3}
</label>", e.Item.ItemIndex + 1, EWebMenuTypeUtils.GetValue(menuType), checkedStr, EWebMenuTypeUtils.GetText(menuType));

                ltlImageUrl.Text = string.Format(@"<img class=""cover"" src=""images/webMenu/{0}.png"" class=""img-polaroid""><p></p>", EWebMenuTypeUtils.GetValue(menuType));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    //base.PublishmentSystemInfo.Additional.WX_WebMenuColor = this.tbWebMenuColor.Text;
                    //base.PublishmentSystemInfo.Additional.WX_IsWebMenuLeft = TranslateUtils.ToBool(this.rblIsWebMenuLeft.SelectedValue);

                    EWebMenuType menuType = EWebMenuTypeUtils.GetEnumType(base.Request.Form["choose"]);
                    base.PublishmentSystemInfo.Additional.WX_WebMenuType = EWebMenuTypeUtils.GetValue(menuType);

                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    base.SuccessMessage("底部导航菜单风格配置成功！");
                    base.AddWaitAndRedirectScript(BackgroundWebMenuType.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "底部导航菜单风格配置失败！");
                }
            }
        }
	}
}
