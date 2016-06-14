using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;


using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
	public class StlEasyChannelEdit : BackgroundBasePage
	{
        public TextBox tbNodeName;
        public TextBox tbNodeIndexName;
        public TextBox tbLinkUrl;
        public DropDownList ddlLinkType;
        public TextBox tbImageUrl;
        public Literal ltlImageUrlButtonGroup;
        public TextEditorControl tecContent;
        public CheckBoxList cblNodeGroupNameCollection;

        public Button btnSubmit;

		private int nodeID;

        protected override bool IsSinglePage { get { return true; } }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("modal_stlEasyChannelEdit.aspx"), arguments);
        }

		public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
			this.nodeID = base.GetIntQueryString("NodeID");

            if (!IsPostBack)
            {
                if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                {
                    PageUtils.RedirectToErrorPage("您没有修改栏目的权限！");
                    return;
                }
                
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                if (nodeInfo != null)
                {
                    this.btnSubmit.Attributes.Add("onclick", "if (UE && UE.getEditor('Content')){ UE.getEditor('Content').sync(); }");

                    ELinkTypeUtils.AddListItems(this.ddlLinkType);

                    this.cblNodeGroupNameCollection.DataSource = DataProvider.NodeGroupDAO.GetDataSource(base.PublishmentSystemID);

                    DataBind();

                    this.tbNodeName.Text = nodeInfo.NodeName;
                    this.tbNodeIndexName.Text = nodeInfo.NodeIndexName;
                    this.tbLinkUrl.Text = nodeInfo.LinkUrl;

                    foreach (ListItem item in this.cblNodeGroupNameCollection.Items)
                    {
                        if (CompareUtils.Contains(nodeInfo.NodeGroupNameCollection, item.Value))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    ControlUtils.SelectListItems(this.ddlLinkType, ELinkTypeUtils.GetValue(nodeInfo.LinkType));

                    this.tbImageUrl.Text = nodeInfo.ImageUrl;
                    this.ltlImageUrlButtonGroup.Text = StringUtility.Controls.GetImageUrlButtonGroupHtml(base.PublishmentSystemInfo, this.tbImageUrl.ClientID);

                    NameValueCollection formCollection = new NameValueCollection();
                    formCollection[NodeAttribute.Content] = nodeInfo.Content;
                    this.tecContent.SetParameters(base.PublishmentSystemInfo, NodeAttribute.Content, formCollection, true, base.IsPostBack);
                }
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isChanged = false;

                NodeInfo nodeInfo = null;
                try
                {
                    nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                    if (!nodeInfo.NodeIndexName.Equals(this.tbNodeIndexName.Text) && this.tbNodeIndexName.Text.Length != 0)
                    {
                        ArrayList NodeIndexNameList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(base.PublishmentSystemID);
                        if (NodeIndexNameList.IndexOf(this.tbNodeIndexName.Text) != -1)
                        {
                            base.FailMessage("栏目修改失败，栏目索引已存在！");
                            return;
                        }
                    }

                    nodeInfo.NodeName = this.tbNodeName.Text;
                    nodeInfo.NodeIndexName = this.tbNodeIndexName.Text;

                    ArrayList list = new ArrayList();
                    foreach (ListItem item in this.cblNodeGroupNameCollection.Items)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.Value);
                        }
                    }
                    nodeInfo.NodeGroupNameCollection = TranslateUtils.ObjectCollectionToString(list);
                    nodeInfo.ImageUrl = this.tbImageUrl.Text;
                    nodeInfo.Content = StringUtility.TextEditorContentEncode(base.Request.Form[NodeAttribute.Content], base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.IsSaveImageInTextEditor);

                    nodeInfo.LinkUrl = this.tbLinkUrl.Text;
                    nodeInfo.LinkType = ELinkTypeUtils.GetEnumType(this.ddlLinkType.SelectedValue);

                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "修改栏目", string.Format("栏目:{0}", nodeInfo.NodeName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("栏目修改失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                }

                if (isChanged)
                {
                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ChannelTemplate, this.nodeID, 0, 0);
                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);

                    JsUtils.Layer.CloseModalLayer(Page);
                }
            }
        }
	}
}
