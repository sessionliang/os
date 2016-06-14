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
    public class BackgroundCard : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_Card.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CardDAO.Delete(base.PublishmentSystemID,list) ;
                        base.SuccessMessage("会员卡删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "会员卡删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CardDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = CardAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, "会员卡", AppManager.WeiXin.Permission.WebSite.Card);
                this.spContents.DataBind();

                string urlAdd = BackgroundCardAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                string urlDelete = PageUtils.AddQueryString(BackgroundCard.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的会员卡", "此操作将删除所选会员卡，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CardInfo CardInfo = new CardInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlCardTitle = e.Item.FindControl("ltlCardTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlUserUrl = e.Item.FindControl("ltlUserUrl") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlOperator = e.Item.FindControl("ltlOperator") as Literal;
                 
                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = CardInfo.Title;
                ltlCardTitle.Text = CardInfo.CardTitle;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(CardInfo.KeywordID);
                ltlPVCount.Text = CardInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!CardInfo.IsDisabled);

                string urlCardSN = BackgroundCardSN.GetRedirectUrl(base.PublishmentSystemID, CardInfo.ID,string.Empty,string.Empty,string.Empty,false);

                ltlUserUrl.Text = string.Format(@"<a href=""{0}"">会员卡</a>", urlCardSN);

                string urlPreview = CardManager.GetCardUrl(CardInfo, string.Empty);
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = BackgroundCardAdd.GetRedirectUrl(base.PublishmentSystemID, CardInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
                 
                ltlOperator.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">操作员</a>",Modal.CardOperatorAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID,CardInfo.ID));
            }
        }
    }
}
