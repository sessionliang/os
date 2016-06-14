using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using System.Text;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
    public class GovInteractApplyTranslate : BackgroundBasePage
	{
        public DropDownList ddlNodeID;
        protected TextBox tbTranslateRemark;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private int nodeID;
        private ArrayList idArrayList;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtilityWCM.GetOpenWindowStringWithCheckBoxValue("转移办件", "modal_govInteractApplyTranslate.aspx", arguments, "IDCollection", "请选择需要转移的办件！", 580, 400);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

            if (!IsPostBack)
            {
                ArrayList nodeInfoArrayList = GovInteractManager.GetNodeInfoArrayList(base.PublishmentSystemInfo);
                foreach (NodeInfo nodeInfo in nodeInfoArrayList)
                {
                    if (nodeInfo.NodeID != this.nodeID)
                    {
                        ListItem listItem = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                        this.ddlNodeID.Items.Add(listItem);
                    }
                }

                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;

                
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                int translateNodeID = TranslateUtils.ToInt(this.ddlNodeID.SelectedValue);
                if (translateNodeID == 0)
                {
                    base.FailMessage("转移失败，必须选择转移目标");
                    return;
                }

                foreach (int contentID in this.idArrayList)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    contentInfo.SetExtendedAttribute(GovInteractContentAttribute.TranslateFromNodeID, contentInfo.NodeID.ToString());
                    contentInfo.NodeID = translateNodeID;
                    
                    DataProvider.ContentDAO.Update(base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, base.PublishmentSystemInfo, contentInfo);

                    if (!string.IsNullOrEmpty(this.tbTranslateRemark.Text))
                    {
                        GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, contentInfo.NodeID, contentID, EGovInteractRemarkType.Translate, this.tbTranslateRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);
                    }

                    GovInteractApplyManager.LogTranslate(base.PublishmentSystemID, contentInfo.NodeID, contentID, NodeManager.GetNodeName(base.PublishmentSystemID, this.nodeID));
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('办件转移成功!');"));
            }
        }
	}
}
