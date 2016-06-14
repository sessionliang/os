using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using System.Text;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundRelatedFieldItem : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;
        public Button ReturnButton;

        private int relatedFieldID;
        private int parentID;
        private int level;
        private int totalLevel;

        public static string GetRedirectUrl(int publishmentSystemID, int relatedFieldID, int parentID, int level)
        {
            return PageUtils.GetCMSUrl(string.Format("background_relatedFieldItem.aspx?PublishmentSystemID={0}&RelatedFieldID={1}&ParentID={2}&Level={3}", publishmentSystemID, relatedFieldID, parentID, level));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
            this.parentID = TranslateUtils.ToInt(base.GetQueryString("ParentID"));
            this.level = TranslateUtils.ToInt(base.GetQueryString("Level"));
            this.totalLevel = DataProvider.RelatedFieldDAO.GetRelatedFieldInfo(this.relatedFieldID).TotalLevel;

            if (base.GetQueryString("Delete") != null && base.GetQueryString("ID") != null)
            {
                int id = TranslateUtils.ToInt(base.GetQueryString("ID"));
                try
                {
                    DataProvider.RelatedFieldItemDAO.Delete(id);
                    base.SuccessMessage("成功删除字段项");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除字段项失败，{0}", ex.Message));
                }
            }
            else if ((base.GetQueryString("Up") != null || base.GetQueryString("Down") != null) && base.GetQueryString("ID") != null)
            {
                int id = int.Parse(base.GetQueryString("ID"));
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.RelatedFieldItemDAO.UpdateTaxisToUp(id, this.parentID);
                }
                else
                {
                    DataProvider.RelatedFieldItemDAO.UpdateTaxisToDown(id, this.parentID);
                }
            }
            else if (this.level != this.totalLevel)
            {
                base.InfoMessage("点击字段项名可以管理下级字段项");    
            }

            if (!IsPostBack)
            {
                string level = string.Empty;
                if (this.level == 1)
                {
                    level = "一级";
                }
                else
                {
                    RelatedFieldItemInfo itemInfo = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfo(this.parentID);
                    string levelString = "二";
                    if (this.level == 3)
                    {
                        levelString = "三";
                    }
                    else if (this.level == 4)
                    {
                        levelString = "四";
                    }
                    else if (this.level == 5)
                    {
                        levelString = "五";
                    }

                    level = string.Format("{0}级({1})", levelString, itemInfo.ItemName);
                }

                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_ContentModel, "内容模型管理", level, AppManager.CMS.Permission.WebSite.ContentModel);

                BindGrid();

                this.AddButton.Attributes.Add("onclick", Modal.RelatedFieldItemAdd.GetOpenWindowString(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level));

                if (this.level == 1)
                {
                    string urlReturn = PageUtils.GetCMSUrl(string.Format("background_relatedField.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    this.ReturnButton.Attributes.Add("onclick", string.Format("parent.location.href = '{0}';return false;", urlReturn));
                }
                else
                {
                    this.ReturnButton.Visible = false;
                }
            }
        }

        public void BindGrid()
        {
            if (this.totalLevel >= 5)
            {
                this.dgContents.Columns[1].Visible = false;
            }
            this.dgContents.DataSource = DataProvider.RelatedFieldItemDAO.GetDataSource(this.relatedFieldID, this.parentID);
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.dgContents.DataBind();
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                string itemName = (string)DataBinder.Eval(e.Item.DataItem, "ItemName");
                string itemValue = (string)DataBinder.Eval(e.Item.DataItem, "ItemValue");

                Literal ltlItemName = e.Item.FindControl("ltlItemName") as Literal;
                Literal ltlItemValue = e.Item.FindControl("ltlItemValue") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                if (this.level >= this.totalLevel)
                {
                    ltlItemName.Text = itemName;
                }
                else
                {
                    ltlItemName.Text = string.Format(@"<a href=""{0}"" target=""level{1}"">{2}</a>", BackgroundRelatedFieldItem.GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, id, this.level + 1), this.level + 1, itemName);
                }
                ltlItemValue.Text = itemValue;
                hlUpLinkButton.NavigateUrl = BackgroundRelatedFieldItem.GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level) + "&Up=True&ID=" + id;

                hlDownLinkButton.NavigateUrl = BackgroundRelatedFieldItem.GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level) + "&Down=True&ID=" + id;

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.RelatedFieldItemEdit.GetOpenWindowString(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level, id));

                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&Delete=True&ID={1}"" onClick=""javascript:return confirm('此操作将删除字段项“{2}”及其子类，确认吗？');"">删除</a>", GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level), id, itemName);
            }
        }
    }
}
