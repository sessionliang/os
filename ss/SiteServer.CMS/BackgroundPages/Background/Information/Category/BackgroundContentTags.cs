using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundContentTags : BackgroundBasePage
	{
		public DataGrid dgContents;
        public LinkButton pageFirst;
        public LinkButton pageLast;
        public LinkButton pageNext;
        public LinkButton pagePrevious;
        public Label currentPage;
		public Button AddTag;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                string tagName = base.GetQueryString("TagName");

                try
                {
                    ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTag(tagName, AppManager.CMS.AppID, base.PublishmentSystemID);
                    if (contentIDArrayList.Count > 0)
                    {
                        foreach (int contentID in contentIDArrayList)
                        {
                            string contentTags = BaiRongDataProvider.ContentDAO.GetValue(base.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, ContentAttribute.Tags);
                            ArrayList contentTagArrayList = TranslateUtils.StringCollectionToArrayList(contentTags);
                            contentTagArrayList.Remove(tagName);
                            BaiRongDataProvider.ContentDAO.SetValue(base.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, ContentAttribute.Tags, TranslateUtils.ObjectCollectionToString(contentTagArrayList));
                        }
                    }
                    BaiRongDataProvider.TagDAO.DeleteTag(tagName, AppManager.CMS.AppID, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除内容标签", string.Format("内容标签:{0}", tagName));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_Category, "内容标签管理", AppManager.CMS.Permission.WebSite.Category);

                this.BindGrid();

                string showPopWinString = Modal.ContentTagAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID);
                this.AddTag.Attributes.Add("onclick", showPopWinString);
			}
		}

        private void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TagInfo tagInfo = e.Item.DataItem as TagInfo;

                Literal ltlTagName = e.Item.FindControl("ltlTagName") as Literal;
                Literal ltlCount = e.Item.FindControl("ltlCount") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                string cssClass = "tag_popularity_1";
                if (tagInfo.Level == 2)
                {
                    cssClass = "tag_popularity_2";
                }
                else if (tagInfo.Level == 3)
                {
                    cssClass = "tag_popularity_3";
                }

                ltlTagName.Text = string.Format(@"<span class=""{0}"">{1}</span>", cssClass, tagInfo.Tag);
                ltlCount.Text = tagInfo.UseNum.ToString();

                string showPopWinString = Modal.ContentTagAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, tagInfo.Tag);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">编辑</a>", showPopWinString);

                string urlDelete = PageUtils.GetCMSUrl(string.Format("background_contentTags.aspx?PublishmentSystemID={0}&Delete=True&TagName={1}", base.PublishmentSystemID, tagInfo.Tag));
                ltlDeleteUrl.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除内容标签“{1}”，确认吗？');\">删除</a>", urlDelete, tagInfo.Tag);
            }
        }

        public void MyDataGrid_Page(object sender, DataGridPageChangedEventArgs e)
        {
            this.dgContents.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                this.dgContents.PageSize = Constants.PageSize;
                this.dgContents.DataSource = BaiRongDataProvider.TagDAO.GetTagInfoArrayList(AppManager.CMS.AppID, base.PublishmentSystemID, 0, true, 0);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                if (this.dgContents.CurrentPageIndex > 0)
                {
                    pageFirst.Enabled = true;
                    pagePrevious.Enabled = true;
                }
                else
                {
                    pageFirst.Enabled = false;
                    pagePrevious.Enabled = false;
                }

                if (this.dgContents.CurrentPageIndex + 1 == this.dgContents.PageCount)
                {
                    pageLast.Enabled = false;
                    pageNext.Enabled = false;
                }
                else
                {
                    pageLast.Enabled = true;
                    pageNext.Enabled = true;
                }

                currentPage.Text = string.Format("{0}/{1}", this.dgContents.CurrentPageIndex + 1, this.dgContents.PageCount);
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }

        }

        protected void NavigationButtonClick(object sender, System.EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string direction = button.CommandName;

            switch (direction.ToUpper())
            {
                case "FIRST":
                    this.dgContents.CurrentPageIndex = 0;
                    break;
                case "PREVIOUS":
                    this.dgContents.CurrentPageIndex =
                        Math.Max(this.dgContents.CurrentPageIndex - 1, 0);
                    break;
                case "NEXT":
                    this.dgContents.CurrentPageIndex =
                        Math.Min(this.dgContents.CurrentPageIndex + 1,
                        this.dgContents.PageCount - 1);
                    break;
                case "LAST":
                    this.dgContents.CurrentPageIndex = this.dgContents.PageCount - 1;
                    break;
                default:
                    break;
            }
            BindGrid();
        }
	}
}
