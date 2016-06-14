using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ChannelAdd : BackgroundBasePage
    {
        public HtmlControl divSelectChannel;
        public Literal ltlSelectChannelScript;
        public DropDownList ContentModelID;
        public TextBox NodeNames;

        public CheckBox ckNameToIndex;
        public DropDownList ChannelTemplateID;
        public DropDownList ContentTemplateID;

        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加栏目", "modal_channelAdd.aspx", arguments);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtils.AddQueryString(PageUtils.GetCMSUrl("modal_channelAdd.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl");

            int nodeID = base.GetIntQueryString("NodeID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            if (!IsPostBack)
            {
                NodeInfo parentNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                this.ContentModelID.Items.Add(new ListItem("<<与父栏目相同>>", string.Empty));
                ArrayList contentModelArrayList = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                foreach (ContentModelInfo modelInfo in contentModelArrayList)
                {
                    this.ContentModelID.Items.Add(new ListItem(modelInfo.ModelName, modelInfo.ModelID));
                }

                ChannelTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                ContentTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ContentTemplate);

                ChannelTemplateID.DataBind();
                ChannelTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                ChannelTemplateID.Items[0].Selected = true;
                ContentTemplateID.DataBind();
                ContentTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                ContentTemplateID.Items[0].Selected = true;

                this.divSelectChannel.Attributes.Add("onclick", Modal.ChannelSelect.GetOpenWindowString(base.PublishmentSystemID));
                this.ltlSelectChannelScript.Text = string.Format(@"<script>selectChannel('{0}', '{1}');</script>", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID), nodeID);
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            bool isChanged = false;
            int parentNodeID = TranslateUtils.ToInt(base.Request.Form["nodeID"]);
            if (parentNodeID == 0)
            {
                parentNodeID = base.PublishmentSystemID;
            }

            try
            {
                if (string.IsNullOrEmpty(this.NodeNames.Text))
                {
                    base.FailMessage("请填写需要添加的栏目名称");
                    return;
                }

                Hashtable insertedNodeIDHashtable = new Hashtable();//key为栏目的级别，1为第一级栏目
                insertedNodeIDHashtable[1] = parentNodeID;

                string[] nodeNameArray = this.NodeNames.Text.Split('\n');
                ArrayList nodeIndexNameList = null;
                foreach (string item in nodeNameArray)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //count为栏目的级别
                        int count = (StringUtils.GetStartCount('－', item) == 0) ? StringUtils.GetStartCount('-', item) : StringUtils.GetStartCount('－', item);
                        string nodeName = item.Substring(count, item.Length - count);
                        string nodeIndex = string.Empty;
                        count++;

                        if (!string.IsNullOrEmpty(nodeName) && insertedNodeIDHashtable.Contains(count))
                        {
                            if (ckNameToIndex.Checked)
                            {
                                nodeIndex = nodeName.Trim();
                            }

                            if (StringUtils.Contains(nodeName, "(") && StringUtils.Contains(nodeName, ")"))
                            {
                                int length = nodeName.IndexOf(')') - nodeName.IndexOf('(');
                                if (length > 0)
                                {
                                    nodeIndex = nodeName.Substring(nodeName.IndexOf('(') + 1, length);
                                    nodeName = nodeName.Substring(0, nodeName.IndexOf('('));
                                }
                            }
                            nodeName = nodeName.Trim();
                            nodeIndex = nodeIndex.Trim(' ', '(', ')');
                            if (!string.IsNullOrEmpty(nodeIndex))
                            {
                                if (nodeIndexNameList == null)
                                {
                                    nodeIndexNameList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(base.PublishmentSystemID);
                                }
                                if (nodeIndexNameList.IndexOf(nodeIndex) != -1)
                                {
                                    nodeIndex = string.Empty;
                                }
                                else
                                {
                                    nodeIndexNameList.Add(nodeIndex);
                                }
                            }

                            int parentID = (int)insertedNodeIDHashtable[count];
                            string contentModelID = this.ContentModelID.SelectedValue;
                            if (string.IsNullOrEmpty(contentModelID))
                            {
                                NodeInfo parentNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, parentID);
                                contentModelID = parentNodeInfo.ContentModelID;
                            }

                            int channelTemplateID = TranslateUtils.ToInt(this.ChannelTemplateID.SelectedValue);
                            int contentTemplateID = TranslateUtils.ToInt(this.ContentTemplateID.SelectedValue);

                            int insertedNodeID = DataProvider.NodeDAO.InsertNodeInfo(base.PublishmentSystemID, parentID, nodeName, nodeIndex, contentModelID, channelTemplateID, contentTemplateID);
                            insertedNodeIDHashtable[count + 1] = insertedNodeID;

                            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ChannelTemplate, insertedNodeID, 0, 0);
                            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                        }
                    }
                }

                StringUtility.AddLog(base.PublishmentSystemID, parentNodeID, 0, "快速添加栏目", string.Format("父栏目:{0},栏目:{1}", NodeManager.GetNodeName(base.PublishmentSystemID, parentNodeID), this.NodeNames.Text.Replace('\n', ',')));

                isChanged = true;
            }
            catch (Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
    }
}
