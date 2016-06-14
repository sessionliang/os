using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using System.Web.UI;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundChannelFilter : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button AddFilter;

        private NodeInfo nodeInfo;
        private string tableName;
        private ArrayList relatedIdentities;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int nodeID = base.GetIntQueryString("NodeID");
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

			if (base.GetQueryString("Delete") != null)
			{
                int filterID = base.GetIntQueryString("FilterID");
			
				try
				{
                    DataProviderB2C.FilterDAO.Delete(base.PublishmentSystemID, this.nodeInfo.NodeID, filterID);

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
			{
                base.BreadCrumbWithItemTitle(AppManager.B2C.LeftMenu.ID_Content, "筛选属性", this.nodeInfo.NodeName, string.Empty);

                if (base.GetQueryString("SetTaxis") != null)
                {
                    int filterID = base.GetIntQueryString("FilterID");
                    string direction = base.GetQueryString("Direction");

                    switch (direction.ToUpper())
                    {
                        case "UP":
                            DataProviderB2C.FilterDAO.UpdateTaxisToUp(nodeID, filterID);
                            break;
                        case "DOWN":
                            DataProviderB2C.FilterDAO.UpdateTaxisToDown(nodeID, filterID);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("排序成功！");
                    base.AddWaitAndRedirectScript(PageUtils.GetB2CUrl(string.Format("background_channelFilter.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, nodeID)));
                }

                this.dgContents.DataSource = DataProviderB2C.FilterDAO.GetDataSource(base.PublishmentSystemID, this.nodeInfo.NodeID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string showPopWinString = Modal.FilterAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, nodeID);
                this.AddFilter.Attributes.Add("onclick", showPopWinString);
			}
		}

        private void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int filterID = TranslateUtils.EvalInt(e.Item.DataItem, "FilterID");
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                string attributeName = TranslateUtils.EvalString(e.Item.DataItem, "AttributeName");
                string displayName = TranslateUtils.EvalString(e.Item.DataItem, "DisplayName");
                bool isDefaultValues = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsDefaultValues"));

                Literal ltlAttributeName = e.Item.FindControl("ltlAttributeName") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlValues = e.Item.FindControl("ltlValues") as Literal;
                Literal ltlIsDefaultValues = e.Item.FindControl("ltlIsDefaultValues") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditValuesUrl = e.Item.FindControl("ltlEditValuesUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlAttributeName.Text = FilterManager.GetFilterFullName(this.tableName, this.relatedIdentities, this.nodeInfo, attributeName);
                ltlDisplayName.Text = FilterManager.GetFilterName(this.tableName, this.relatedIdentities, this.nodeInfo, attributeName, displayName);
                List<FilterItemInfo> itemInfoArrayList = null;

                if (isDefaultValues)
                {
                    ltlIsDefaultValues.Text = "默认";
                    itemInfoArrayList = FilterManager.GetDefaultFilterItemInfoList(base.PublishmentSystemInfo, this.nodeInfo.NodeID, filterID, attributeName);
                }
                else
                {
                    ltlIsDefaultValues.Text = "自定义";
                    itemInfoArrayList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterID);
                }
                ltlValues.Text = FilterManager.GetFilterTitles(itemInfoArrayList);

                hlUpLinkButton.NavigateUrl = PageUtils.GetB2CUrl(string.Format("background_channelFilter.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&FilterID={2}&Direction=UP", base.PublishmentSystemID, this.nodeInfo.NodeID, filterID));
                hlDownLinkButton.NavigateUrl = PageUtils.GetB2CUrl(string.Format("background_channelFilter.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&FilterID={2}&Direction=DOWN", base.PublishmentSystemID, this.nodeInfo.NodeID, filterID));
                string showPopWinString = Modal.FilterValues.GetOpenWindowString(base.PublishmentSystemID, filterID);
                ltlEditValuesUrl.Text = string.Format("<a href=\"javascript:undefined;\" onClick=\"{0}\">设置筛选值</a>", showPopWinString);
                showPopWinString = Modal.FilterAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, nodeID, filterID);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:undefined;\" onClick=\"{0}\">自定义名称</a>", showPopWinString);
                string deleteUrl = PageUtils.GetB2CUrl(string.Format("background_channelFilter.aspx?PublishmentSystemID={0}&NodeID={1}&Delete=True&FilterID={2}", base.PublishmentSystemID, this.nodeInfo.NodeID, filterID));
                ltlDeleteUrl.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除筛选属性“{1}”，确认吗？');\">删除</a>", deleteUrl, ltlAttributeName.Text);
            }
        }

        public void Setting_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(this.nodeInfo.NodeID);

                    List<FilterInfo> filterInfoList = DataProviderB2C.FilterDAO.GetFilterInfoList(base.PublishmentSystemID, this.nodeInfo.NodeID);
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        DataProviderB2C.FilterDAO.DeleteAll(base.PublishmentSystemID, nodeID);

                        foreach (FilterInfo filterInfo in filterInfoList)
                        {
                            if (filterInfo.Items == null)
                            {
                                filterInfo.Items = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);
                            }
                            FilterInfo filterInfoToAdd = new FilterInfo(0, base.PublishmentSystemID, nodeID, filterInfo.AttributeName, filterInfo.FilterName, filterInfo.IsDefaultValues, 0);

                            int filterID = DataProviderB2C.FilterDAO.Insert(filterInfoToAdd);

                            if (filterInfo.Items != null && filterInfo.Items.Count > 0)
                            {
                                foreach (FilterItemInfo itemInfo in filterInfo.Items)
                                {
                                    FilterItemInfo itemInfoToAdd = new FilterItemInfo(0, filterID, itemInfo.Title, itemInfo.Value, 0);
                                    DataProviderB2C.FilterItemDAO.Insert(itemInfoToAdd);
                                }
                            }
                        }
                    }

                    base.SuccessMessage("设置同步成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "设置同步失败！");
                }
            }
        }
	}
}
