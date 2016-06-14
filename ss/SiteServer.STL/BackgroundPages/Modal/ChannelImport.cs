using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using SiteServer.CMS.Model;

using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class ChannelImport : BackgroundBasePage
	{
        protected DropDownList ParentNodeID;
		public HtmlInputFile myFile;
		public RadioButtonList IsOverride;

        bool[] IsLastNodeArray;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                int nodeCount = nodeIDArrayList.Count;
                IsLastNodeArray = new bool[nodeCount];
                foreach (int theNodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);
                    int ItemNodeID = nodeInfo.NodeID;
                    string NodeName = nodeInfo.NodeName;
                    int ParentsCount = nodeInfo.ParentsCount;
                    bool IsLastNode = nodeInfo.IsLastNode;
                    string value = base.IsOwningNodeID(ItemNodeID) ? ItemNodeID.ToString() : string.Empty;
                    value = (nodeInfo.Additional.IsChannelAddable) ? value : string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (!base.HasChannelPermissions(theNodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
                        {
                            value = string.Empty;
                        }
                    }
                    ListItem listitem = new ListItem(this.GetTitle(ItemNodeID, NodeName, ParentsCount, IsLastNode), value);
                    if (ItemNodeID == nodeID)
                    {
                        listitem.Selected = true;
                    }
                    ParentNodeID.Items.Add(listitem);
                }

				
			}
		}

        public string GetTitle(int NodeID, string NodeName, int ParentsCount, bool IsLastNode)
        {
            string str = "";
            if (NodeID == base.PublishmentSystemID)
            {
                IsLastNode = true;
            }
            if (IsLastNode == false)
            {
                IsLastNodeArray[ParentsCount] = false;
            }
            else
            {
                IsLastNodeArray[ParentsCount] = true;
            }
            for (int i = 0; i < ParentsCount; i++)
            {
                if (IsLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (IsLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, NodeName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
				string filePath = myFile.PostedFile.FileName;
                if (!EFileSystemTypeUtils.IsCompressionFile(PathUtils.GetExtension(filePath)))
				{
                    base.FailMessage("必须上传压缩文件");
					return;
				}

				try
				{
                    string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

					myFile.PostedFile.SaveAs(localFilePath);

					ImportObject importObject = new ImportObject(base.PublishmentSystemID);
                    importObject.ImportChannelsAndContentsByZipFile(TranslateUtils.ToInt(this.ParentNodeID.SelectedValue), localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));

                    StringUtility.AddLog(base.PublishmentSystemID, "导入栏目");

					JsUtils.Layer.CloseModalLayer(Page);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "导入栏目失败！");
				}
			}
		}
	}
}
