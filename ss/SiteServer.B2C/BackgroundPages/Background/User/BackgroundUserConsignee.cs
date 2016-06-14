using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundUserConsignee : BackgroundBasePage
    {
        public Repeater rptContents;
        public Button btnAdd;

        private string userName;
        private string returnUrl;
        private List<ConsigneeInfo> consigneeInfoList;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userName = base.GetQueryString("userName");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));

            if (base.GetQueryString("isDelete") != null && base.GetQueryString("consigneeID") != null)
            {
                int consigneeID = base.GetIntQueryString("consigneeID");
                if (consigneeID > 0)
                {
                    DataProviderB2C.ConsigneeDAO.Delete(consigneeID);
                    base.SuccessMessage("收货方式删除成功");
                }
            }
            else if (base.GetQueryString("isDefault") != null && base.GetQueryString("consigneeID") != null)
            {
                int consigneeID = base.GetIntQueryString("consigneeID");
                if (consigneeID > 0)
                {
                    DataProviderB2C.ConsigneeDAO.SetDefault(consigneeID);
                    base.SuccessMessage("默认收货方式设置成功");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_User, "收货方式", string.Empty);

                this.consigneeInfoList = DataProviderB2C.ConsigneeDAO.GetConsigneeInfoList(base.PublishmentSystemInfo.GroupSN, this.userName);

                this.rptContents.DataSource = this.consigneeInfoList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                this.btnAdd.Attributes.Add("onclick", Modal.UserConsigneeAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID,this.userName));
            }
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConsigneeInfo consigneeInfo = e.Item.DataItem as ConsigneeInfo;

                Literal ltlConsignee = e.Item.FindControl("ltlConsignee") as Literal;
                Literal ltlLocation = e.Item.FindControl("ltlLocation") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlTel = e.Item.FindControl("ltlTel") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlIsDefault = e.Item.FindControl("ltlIsDefault") as Literal;
                Literal ltlIsDefaultUrl = e.Item.FindControl("ltlIsDefaultUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlConsignee.Text = consigneeInfo.Consignee;
                ltlLocation.Text = consigneeInfo.Province + consigneeInfo.City + consigneeInfo.Area + consigneeInfo.Address;
                ltlMobile.Text = consigneeInfo.Mobile;
                ltlTel.Text = consigneeInfo.Tel;
                ltlEmail.Text = consigneeInfo.Email;

                if (consigneeInfo.IsDefault)
                {
                    ltlIsDefault.Text = StringUtils.GetTrueImageHtml(true);
                }
                else
                {
                    ltlIsDefaultUrl.Text = string.Format(@"<a href=""console_userConsignee.aspx?userName={0}&isDefault=True&consigneeID={1}"">设为默认</a>", this.userName, consigneeInfo.ID);
                }

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.UserConsigneeAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.userName, consigneeInfo.ID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""console_userConsignee.aspx?userName={0}&isDelete=True&consigneeID={1}"" onclick=""javascript:return confirm('此操作将删除选定收货方式，确认吗？');"">删除</a>", this.userName, consigneeInfo.ID);
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
