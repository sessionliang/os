using System;
using System.Collections;
using System.Drawing;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundChannelDelete : BackgroundBasePage
	{
        public Literal ltlPageTitle;
		public RadioButtonList RetainFiles;
        public Button Delete;

        private bool deleteContents;
        private string returnUrl;
        private ArrayList nodeNameArrayList = new ArrayList();

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.deleteContents = TranslateUtils.ToBool(base.GetQueryString("DeleteContents"));

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "删除栏目", string.Empty);

                List<int> nodeIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ChannelIDCollection"));
                nodeIDList.Sort();
                nodeIDList.Reverse();
                foreach (int nodeID in nodeIDList)
				{
                    if (nodeID == base.PublishmentSystemID) continue;
                    if (base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ChannelDelete))
					{
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                        string displayName = nodeInfo.NodeName;
                        if (nodeInfo.ContentNum > 0)
                        {
                            displayName += string.Format("({0})", nodeInfo.ContentNum);
                        }
                        nodeNameArrayList.Add(displayName);
					}
				}
                if (nodeNameArrayList.Count == 0)
                {
                    this.Delete.Enabled = false;
                }
                else
                {
                    if (this.deleteContents)
                    {
                        this.ltlPageTitle.Text = "删除内容";
                        base.InfoMessage(string.Format("此操作将会删除栏目“{0}”下的所有内容，确认吗？", TranslateUtils.ObjectCollectionToString(nodeNameArrayList)));
                    }
                    else
                    {
                        this.ltlPageTitle.Text = "删除栏目";
                        base.InfoMessage(string.Format("此操作将会删除栏目“{0}”及包含的下级栏目，确认吗？", TranslateUtils.ObjectCollectionToString(nodeNameArrayList)));
                    }
                }
			}
		}

        public void Delete_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				try
				{
					List<int> nodeIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ChannelIDCollection"));
                    nodeIDList.Sort();
                    nodeIDList.Reverse();

					ArrayList nodeIDArrayList = new ArrayList();
                    foreach (int nodeID in nodeIDList)
					{
                        if (nodeID == base.PublishmentSystemID) continue;
                        if (base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ChannelDelete))
						{
							nodeIDArrayList.Add(nodeID);
						}
					}

                    StringBuilder builder = new StringBuilder();
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        builder.Append(NodeManager.GetNodeName(base.PublishmentSystemID, nodeID)).Append(",");
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length -= 1;
                    }

                    if (this.deleteContents)
                    {
                        if (bool.Parse(this.RetainFiles.SelectedValue) == false)
                        {
                            base.SuccessMessage("成功删除内容以及生成页面！");
                        }
                        else
                        {
                            base.SuccessMessage("成功删除内容，生成页面未被删除！");
                        }

                        foreach (int nodeID in nodeIDArrayList)
                        {
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                            ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID);
                            DirectoryUtility.DeleteContents(base.PublishmentSystemInfo, nodeID, contentIDArrayList);
                            DataProvider.ContentDAO.TrashContents(base.PublishmentSystemID, tableName, contentIDArrayList);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "清空栏目下的内容", string.Format("栏目:{0}", builder.ToString()));
                    }
                    else
                    {
                        if (bool.Parse(this.RetainFiles.SelectedValue) == false)
                        {
                            DirectoryUtility.DeleteChannels(base.PublishmentSystemInfo, nodeIDArrayList);
                            base.SuccessMessage("成功删除栏目以及相关生成页面！");
                        }
                        else
                        {
                            base.SuccessMessage("成功删除栏目，相关生成页面未被删除！");
                        }

                        foreach (int nodeID in nodeIDArrayList)
                        {
                            try
                            {
                                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                                DataProvider.ContentDAO.TrashContentsByNodeID(base.PublishmentSystemID, tableName, nodeID);
                            }
                            catch { }
                            DataProvider.NodeDAO.Delete(nodeID);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "删除栏目", string.Format("栏目:{0}", builder.ToString()));
                    }

                    base.AddWaitAndRedirectScript(this.returnUrl);
				}
				catch (Exception ex)
				{
                    if (this.deleteContents)
                    {
                        base.FailMessage(ex, "删除内容失败！");
                    }
                    else
                    {
                        base.FailMessage(ex, "删除栏目失败！");
                    }

                    LogUtils.AddErrorLog(ex);
				}
			}
		}

        public string ReturnUrl { get { return this.returnUrl; } }
	}
}
