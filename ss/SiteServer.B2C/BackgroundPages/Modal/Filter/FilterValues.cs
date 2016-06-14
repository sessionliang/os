using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class FilterValues : BackgroundBasePage
	{
        protected Literal ltlAttributeName;
        protected Literal ltlValues;
        protected RadioButtonList rblIsDefaultValues;

        protected PlaceHolder phValues;
        protected TextBox tbValues;

        private FilterInfo filterInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int filterID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("FilterID", filterID.ToString());
            return PageUtilityB2C.GetOpenWindowString("设置筛选值", "modal_filterValues.aspx", arguments, 580, 550);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "FilterID");
            int filterID = base.GetIntQueryString("FilterID");

            this.filterInfo = DataProviderB2C.FilterDAO.GetFilterInfo(filterID);

			if (!IsPostBack)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.filterInfo.NodeID);
                this.ltlAttributeName.Text = FilterManager.GetFilterFullName(base.PublishmentSystemInfo, nodeInfo, this.filterInfo);
                List<FilterItemInfo> itemInfoArrayList = FilterManager.GetDefaultFilterItemInfoList(base.PublishmentSystemInfo, this.filterInfo.NodeID, filterID, this.filterInfo.AttributeName);
                ltlValues.Text = FilterManager.GetFilterTitles(itemInfoArrayList);
                this.phValues.Visible = !this.filterInfo.IsDefaultValues;
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsDefaultValues, this.filterInfo.IsDefaultValues.ToString());

                if (!this.filterInfo.IsDefaultValues)
                {
                    itemInfoArrayList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(this.filterInfo.FilterID);
                }

                this.tbValues.Text = FilterManager.GetFilterValues(itemInfoArrayList);
			}
		}

        protected void rblIsDefaultValues_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (TranslateUtils.ToBool(this.rblIsDefaultValues.SelectedValue))
            {
                this.phValues.Visible = false;
            }
            else
            {
                this.phValues.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = true;

            bool isDefaultValues = TranslateUtils.ToBool(this.rblIsDefaultValues.SelectedValue);
            DataProviderB2C.FilterDAO.UpdateIsDefaultValues(isDefaultValues, this.filterInfo.FilterID);

            if (!isDefaultValues)
            {
                List<FilterItemInfo> itemInfoArrayList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(this.filterInfo.FilterID);
                if (FilterManager.GetFilterValues(itemInfoArrayList) != this.tbValues.Text)
                {
                    try
                    {
                        DataProviderB2C.FilterItemDAO.Delete(this.filterInfo.FilterID);

                        itemInfoArrayList = FilterManager.GetFilterItemInfoList(this.filterInfo.FilterID, this.tbValues.Text);
                        if (itemInfoArrayList.Count > 0)
                        {
                            foreach (FilterItemInfo itemInfo in itemInfoArrayList)
                            {
                                DataProviderB2C.FilterItemDAO.Insert(itemInfo);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        isChanged = false;
                        base.FailMessage(ex, "设置筛选值失败！");
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
