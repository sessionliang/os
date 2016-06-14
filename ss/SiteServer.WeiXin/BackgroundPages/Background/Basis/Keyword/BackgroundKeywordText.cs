using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeywordText : BackgroundBasePageWX
    {
        public DataGrid dgContents;
        public Button btnAdd;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_keywordText.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.Request.QueryString["Delete"] != null)
            {
                int keywordID = TranslateUtils.ToInt(base.Request.QueryString["keywordID"]);

                try
                {
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
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_TextReply, string.Empty, AppManager.WeiXin.Permission.WebSite.TextReply);

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

                this.dgContents.DataSource = DataProviderWX.KeywordDAO.GetDataSource(base.PublishmentSystemID, EKeywordType.Text);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string showPopWinString = Modal.KeywordAddText.GetOpenWindowStringToAdd(base.PublishmentSystemID);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int keywordID = TranslateUtils.EvalInt(e.Item.DataItem, "KeywordID");
                string keywords = TranslateUtils.EvalString(e.Item.DataItem, "Keywords");
                string reply = TranslateUtils.EvalString(e.Item.DataItem, "Reply");
                bool isDisabled = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsDisabled"));
                EMatchType matchType = EMatchTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "MatchType"));
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");

                Literal ltlReply = e.Item.FindControl("ltlReply") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlMatchType = e.Item.FindControl("ltlMatchType") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                HyperLink hlUp = e.Item.FindControl("hlUp") as HyperLink;
                HyperLink hlDown = e.Item.FindControl("hlDown") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlReply.Text = StringUtils.CleanText(reply);
                ltlAddDate.Text = DateUtils.GetDateString(addDate);
                ltlMatchType.Text = EMatchTypeUtils.GetText(matchType);
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!isDisabled);

                string redirectUrl = BackgroundKeywordText.GetRedirectUrl(base.PublishmentSystemID);

                hlUp.NavigateUrl = string.Format("{0}&SetTaxis=True&KeywordID={1}&Direction=UP", redirectUrl, keywordID);
                hlDown.NavigateUrl = string.Format("{0}&SetTaxis=True&KeywordID={1}&Direction=DOWN", redirectUrl, keywordID);

                string showPopWinString = Modal.KeywordAddText.GetOpenWindowStringToEdit(base.PublishmentSystemID, keywordID);
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onClick=""{0}"">修改</a>", showPopWinString);

                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&Delete=True&KeywordID={1}"" onClick=""javascript:return confirm('此操作将删除关键字“{2}”，确认吗？');"">删除</a>", redirectUrl, keywordID, keywords);
            }
        }
    }
}
