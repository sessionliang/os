using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentAttributes : BackgroundBasePage
	{
        protected CheckBox IsRecommend;
        protected CheckBox IsHot;
        protected CheckBox IsColor;
        protected CheckBox IsTop;
        protected HtmlInputHidden hdType;
        protected TextBox Hits;

        private int nodeID;
        private ETableStyle tableStyle;
        private string tableName;
        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("设置内容属性", "modal_contentAttributes.aspx", arguments, "ContentIDCollection", "请选择需要设置属性的内容！", 300, 240);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID); 
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                if (this.hdType.Value == "1")
                {
                    if (this.IsRecommend.Checked || this.IsHot.Checked || this.IsColor.Checked || this.IsTop.Checked)
                    {
                        foreach (int contentID in this.idArrayList)
                        {
                            BackgroundContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID) as BackgroundContentInfo;
                            if (contentInfo != null)
                            {
                                if (this.IsRecommend.Checked)
                                {
                                    contentInfo.IsRecommend = true;
                                }
                                if (this.IsHot.Checked)
                                {
                                    contentInfo.IsHot = true;
                                }
                                if (this.IsColor.Checked)
                                {
                                    contentInfo.IsColor = true;
                                }
                                if (this.IsTop.Checked)
                                {
                                    contentInfo.IsTop = true;
                                }
                                DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);
                            }
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "设置内容属性");

                        isChanged = true;
                    }
                }
                else if (this.hdType.Value == "2")
                {
                    if (this.IsRecommend.Checked || this.IsHot.Checked || this.IsColor.Checked || this.IsTop.Checked)
                    {
                        foreach (int contentID in this.idArrayList)
                        {
                            BackgroundContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID) as BackgroundContentInfo;
                            if (contentInfo != null)
                            {
                                if (this.IsRecommend.Checked)
                                {
                                    contentInfo.IsRecommend = false;
                                }
                                if (this.IsHot.Checked)
                                {
                                    contentInfo.IsHot = false;
                                }
                                if (this.IsColor.Checked)
                                {
                                    contentInfo.IsColor = false;
                                }
                                if (this.IsTop.Checked)
                                {
                                    contentInfo.IsTop = false;
                                }
                                DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);
                            }
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "取消内容属性");

                        isChanged = true;
                    }
                }
                else if (this.hdType.Value == "3")
                {
                    int hits = TranslateUtils.ToInt(this.Hits.Text);

                    foreach (int contentID in this.idArrayList)
                    {
                        BaiRongDataProvider.ContentDAO.SetValue(this.tableName, contentID, BaiRong.Model.ContentAttribute.Hits, hits.ToString());
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "设置内容点击量");

                    isChanged = true;
                }
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}

	}
}
