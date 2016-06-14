using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;


namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundConferenceContent : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnReturn;

        private int conferenceID;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int conferenceID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_conferenceContent.aspx?publishmentSystemID={0}&conferenceID={1}&returnUrl={2}", publishmentSystemID, conferenceID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.conferenceID = TranslateUtils.ToInt(base.Request.QueryString["conferenceID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.ConferenceContentDAO.Delete(base.PublishmentSystemID, list);
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.ConferenceContentDAO.GetSelectString(base.PublishmentSystemID, this.conferenceID);
            this.spContents.SortField = ConferenceContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Conference, "申请查看", AppManager.WeiXin.Permission.WebSite.Conference);
                this.spContents.DataBind();

                string urlDelete = PageUtils.AddQueryString(BackgroundConferenceContent.GetRedirectUrl(base.PublishmentSystemID, this.conferenceID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的申请项", "此操作将删除所选申请项，确认吗？"));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConferenceContentInfo contentInfo = new ConferenceContentInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlRealName = e.Item.FindControl("ltlRealName") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlCompany = e.Item.FindControl("ltlCompany") as Literal;
                Literal ltlPosition = e.Item.FindControl("ltlPosition") as Literal;
                Literal ltlNote = e.Item.FindControl("ltlNote") as Literal;
                Literal ltlWXOpenID = e.Item.FindControl("ltlWXOpenID") as Literal;
                Literal ltlIPAddress = e.Item.FindControl("ltlIPAddress") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                ltlRealName.Text = contentInfo.RealName;
                ltlMobile.Text = contentInfo.Mobile;
                ltlEmail.Text = contentInfo.Email;
                ltlCompany.Text = contentInfo.Company;
                ltlPosition.Text = contentInfo.Position;
                ltlNote.Text = contentInfo.Note;
                ltlWXOpenID.Text = contentInfo.WXOpenID;
                ltlIPAddress.Text = contentInfo.IPAddress;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
            }
        }
    }
}
