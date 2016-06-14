using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using BaiRong.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeywordNews : BackgroundBasePageWX
    {
        public Repeater rptContents;

        public Button btnAddSingle;
        public Button btnAddMultiple;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_keywordNews.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.Request.QueryString["delete"] != null)
            {
                int keywordID = TranslateUtils.ToInt(base.Request.QueryString["keywordID"]);

                try
                {
                    foreach (int resourceID in DataProviderWX.KeywordResourceDAO.GetResourceIDList(keywordID))
                    {
                        FileUtilityWX.DeleteWeiXinContent(base.PublishmentSystemInfo, keywordID, resourceID);
                    }
                    DataProviderWX.KeywordDAO.Delete(keywordID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除关键字");
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_ImageReply, string.Empty, AppManager.WeiXin.Permission.WebSite.ImageReply);
                if (Request.QueryString["SetTaxis"] != null)
                {
                    int keywordID = TranslateUtils.ToInt(base.Request.QueryString["keywordID"]);
                    string direction = Request.QueryString["Direction"];

                    switch (direction.ToUpper())
                    {
                        case "UP":
                            DataProviderWX.KeywordDAO.UpdateTaxisToUp(base.PublishmentSystemID, EKeywordType.Text, keywordID);
                            break;
                        case "DOWN":
                            DataProviderWX.KeywordDAO.UpdateTaxisToDown(base.PublishmentSystemID, EKeywordType.Text, keywordID);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("排序成功！");
                    base.AddWaitAndRedirectScript(BackgroundKeywordText.GetRedirectUrl(base.PublishmentSystemID));
                }

                this.rptContents.DataSource = DataProviderWX.KeywordDAO.GetDataSource(base.PublishmentSystemID, EKeywordType.News);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                this.btnAddSingle.Attributes.Add("onclick", Modal.KeywordAddNews.GetOpenWindowStringToAdd(base.PublishmentSystemID, true));
                this.btnAddMultiple.Attributes.Add("onclick", Modal.KeywordAddNews.GetOpenWindowStringToAdd(base.PublishmentSystemID, false));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int keywordID = TranslateUtils.EvalInt(e.Item.DataItem, "KeywordID");
                string keywords = TranslateUtils.EvalString(e.Item.DataItem, "Keywords");
                bool isDisabled = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsDisabled"));
                string reply = TranslateUtils.EvalString(e.Item.DataItem, "Reply");
                EMatchType matchType = EMatchTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "MatchType"));
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");

                PlaceHolder phSingle = e.Item.FindControl("phSingle") as PlaceHolder;
                PlaceHolder phMultiple = e.Item.FindControl("phMultiple") as PlaceHolder;

                List<KeywordResourceInfo> resourceInfoList = DataProviderWX.KeywordResourceDAO.GetResourceInfoList(keywordID);

                phMultiple.Visible = resourceInfoList.Count > 1;
                phSingle.Visible = !phMultiple.Visible;

                if (phSingle.Visible)
                {
                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo();
                    if (resourceInfoList.Count > 0)
                    {
                        resourceInfo = resourceInfoList[0];
                    }

                    Literal ltlSingleTitle = e.Item.FindControl("ltlSingleTitle") as Literal;
                    Literal ltlSingleKeywords = e.Item.FindControl("ltlSingleKeywords") as Literal;
                    Literal ltlSingleAddDate = e.Item.FindControl("ltlSingleAddDate") as Literal;
                    Literal ltlSingleImageUrl = e.Item.FindControl("ltlSingleImageUrl") as Literal;
                    Literal ltlSingleSummary = e.Item.FindControl("ltlSingleSummary") as Literal;
                    Literal ltlSingleEditUrl = e.Item.FindControl("ltlSingleEditUrl") as Literal;
                    Literal ltlSingleDeleteUrl = e.Item.FindControl("ltlSingleDeleteUrl") as Literal;

                    ltlSingleTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", "javascript:;", resourceInfo.Title);
                    ltlSingleKeywords.Text = string.Format(@"{0}&nbsp;<a href=""javascript:;"" onclick=""{1}"">修改</a>", keywords + (isDisabled ? "(禁用)" : string.Empty), Modal.KeywordAddNews.GetOpenWindowStringToEdit(base.PublishmentSystemID, keywordID));
                    ltlSingleAddDate.Text = addDate.ToShortDateString();
                    if (!string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        ltlSingleImageUrl.Text = string.Format(@"<img src=""{0}"" class=""appmsg_thumb"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
                    }
                    ltlSingleSummary.Text = MPUtils.GetSummary(resourceInfo.Summary, resourceInfo.Content);
                    ltlSingleEditUrl.Text = string.Format(@"<a class=""js_edit"" href=""{0}""><i class=""icon18_common edit_gray"">编辑</i></a>", BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, keywordID, resourceInfo.ResourceID, phSingle.Visible));
                    ltlSingleDeleteUrl.Text = string.Format(@"<a class=""js_del no_extra"" href=""{0}&delete=true&keywordID={1}"" onclick=""javascript:return confirm('此操作将删除图文回复“{2}”，确认吗？');""><i class=""icon18_common del_gray"">删除</i></a>", BackgroundKeywordNews.GetRedirectUrl(base.PublishmentSystemID), keywordID, keywords);
                }
                else
                {
                    KeywordResourceInfo resourceInfo = resourceInfoList[0];
                    resourceInfoList.Remove(resourceInfo);

                    Literal ltlMultipleKeywords = e.Item.FindControl("ltlMultipleKeywords") as Literal;
                    Literal ltlMultipleAddDate = e.Item.FindControl("ltlMultipleAddDate") as Literal;
                    Literal ltlMultipleTitle = e.Item.FindControl("ltlMultipleTitle") as Literal;
                    Literal ltlMultipleImageUrl = e.Item.FindControl("ltlMultipleImageUrl") as Literal;
                    Repeater rptMultipleContents = e.Item.FindControl("rptMultipleContents") as Repeater;
                    Literal ltlMultipleEditUrl = e.Item.FindControl("ltlMultipleEditUrl") as Literal;
                    Literal ltlMultipleDeleteUrl = e.Item.FindControl("ltlMultipleDeleteUrl") as Literal;

                    ltlMultipleKeywords.Text = string.Format(@"{0}&nbsp;<a href=""javascript:;"" onclick=""{1}"">修改</a>", keywords + (isDisabled ? "(禁用)" : string.Empty), Modal.KeywordAddNews.GetOpenWindowStringToEdit(base.PublishmentSystemID, keywordID));

                    ltlMultipleAddDate.Text = addDate.ToShortDateString();
                    ltlMultipleTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", "javascript:;", resourceInfo.Title);
                    if (!string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        ltlMultipleImageUrl.Text = string.Format(@"<img src=""{0}"" class=""appmsg_thumb"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
                    }

                    rptMultipleContents.DataSource = resourceInfoList;
                    rptMultipleContents.ItemDataBound += rptMultipleContents_ItemDataBound;
                    rptMultipleContents.DataBind();

                    ltlMultipleEditUrl.Text = string.Format(@"<a class=""js_edit"" href=""{0}""><i class=""icon18_common edit_gray"">编辑</i></a>", BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, keywordID, resourceInfo.ResourceID, phSingle.Visible));
                    ltlMultipleDeleteUrl.Text = string.Format(@"<a class=""js_del no_extra"" href=""{0}&delete=true&keywordID={1}"" onclick=""javascript:return confirm('此操作将删除图文回复“{2}”，确认吗？');""><i class=""icon18_common del_gray"">删除</i></a>", BackgroundKeywordNews.GetRedirectUrl(base.PublishmentSystemID), keywordID, keywords);
                }
            }
        }

        void rptMultipleContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            KeywordResourceInfo resourceInfo = e.Item.DataItem as KeywordResourceInfo;

            Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;
            Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;

            if (string.IsNullOrEmpty(resourceInfo.ImageUrl))
            {
                ltlImageUrl.Text = @"<i class=""appmsg_thumb default"">缩略图</i>";
            }
            else
            {
                ltlImageUrl.Text = string.Format(@"<img class=""appmsg_thumb"" style=""max-width:78px;max-height:78px;display:block"" src=""{0}"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, resourceInfo.ImageUrl));
            }
            ltlTitle.Text = string.Format(@"<a href=""javascript:;"">{0}</a>", resourceInfo.Title);
        }
    }
}
