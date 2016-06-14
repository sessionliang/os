using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.MLib;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReviewAdd : MLibBackgroundBasePage
    {
        public Literal ltlPageTitle;

        public AuxiliaryControl acAttributes;

        public RadioButtonList ContentLevel;
        public PlaceHolder phStatus;
        public PlaceHolder phNode;
        public DropDownList ddlNodeID;

        public Button Submit;

        private NodeInfo nodeInfo;
        private int contentID;
        private ArrayList relatedIdentities;
        private string returnUrl;
        private ETableStyle tableStyle;
        private string tableName;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!this.HasCheckLevelPermissions("1"))
            {

                Response.Write("<script>alert('您没有添加稿件的权限(初审权限)');history.go(-1);</script>");
                Response.End();
            }

            int nodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            if (!IsPostBack)
            {
                string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeInfo.NodeID);
                string pageTitle = "添加稿件";
                //base.BreadCrumbWithItemTitle(ProductManager.WCM.LeftMenu.ID_Content, pageTitle, nodeNames, string.Empty);

                this.ltlPageTitle.Text = pageTitle;

                this.ltlPageTitle.Text += string.Format(@"
<script>
function submitPreview(){{
    var var1 = myForm.action;
    var var2 = myForm.target;
    myForm.action = ""{0}"";
    myForm.target = ""preview"";
    if (UE && UE.getEditor('Content')){{ UE.getEditor('Content').sync(); }}
    myForm.submit();
    myForm.action = var1;
    myForm.target = var2;
}}
</script>
", PageUtility.GetContentPreviewUrl(base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.contentID));

                if (this.contentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);
                    this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                    phNode.Visible = true;

                    //绑定分类
                    var nodelist = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);

                    nodelist.Add(new NodeInfo());
                    foreach (NodeInfo item in nodelist)
                    {
                        this.ddlNodeID.Items.Add(new ListItem(item.NodeName, item.NodeID.ToString()));
                    }
                    this.ddlNodeID.SelectedIndex = 0;

                }
                else
                {
                    this.acAttributes.SetParameters(new NameValueCollection(), base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);

                }
                this.Submit.Attributes.Add("onclick", InputParserUtils.GetValidateSubmitOnClickScript("myForm"));
            }
            else
            {
                this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);

            }
            base.DataBind();
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);
                try
                {
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                    if (this.contentID != 0)
                    {
                        contentInfo.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);
                    }
                    else
                    {
                        contentInfo.NodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
                    }
                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                    //contentInfo.SourceID = SourceManager.TouGao;
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.IsChecked = true;
                    contentInfo.CheckedLevel = 1;
                    if (contentInfo.NodeID == base.PublishmentSystemID)
                    {
                        contentInfo.CheckedLevel = 0;
                    }
                    #region 插入ml_Submission
                    SubmissionInfo submissionInfo = new SubmissionInfo()
                    {
                        AddUserName = AdminManager.Current.UserName,
                        Title = contentInfo.Title,
                        AddDate = DateTime.Now,
                        IsChecked = true,
                        CheckedLevel = contentInfo.CheckedLevel,
                        PassDate = null,
                        ReferenceTimes = 0
                    };
                    int submissionId = DataProvider.MlibDAO.Insert(submissionInfo);
                    #endregion

                    contentInfo.ReferenceID = submissionId;

                    int contentID = DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);
                    contentInfo.ID = contentID;


                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                   // LogUtils.SystemErrorLogWrite(ex);
                    return;
                }
                PageUtils.Redirect("ReviewList1.aspx?PublishmentSystemID=" + this.PublishmentSystemID + "&NodeID=" + nodeInfo.NodeID);
            }

        }



        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
