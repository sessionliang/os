using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.MLib;
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class DraftEdit : MLibBackgroundBasePage
    {
        public Literal ltlPageTitle;

        public AuxiliaryControl acAttributes;

        public RadioButtonList ContentLevel;
        public DropDownList ddlNodeID;
        public PlaceHolder phStatus;

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
                Response.Write("<script>alert('您没有初审权限')</script>");
                Response.End();
            }

            int nodeID = base.PublishmentSystemID;
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            ContentInfo contentInfo = null;

            contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);

            if (!IsPostBack)
            {
                string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeInfo.NodeID);
                string pageTitle = "稿件收集";
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

                var nodelist = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);
                this.ddlNodeID.DataSource = nodelist;
                this.ddlNodeID.DataValueField = "NodeID";
                this.ddlNodeID.DataTextField = "NodeName";

                this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, this.nodeInfo.NodeID, this.relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);

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
                string status = this.ContentLevel.SelectedValue;
                if (status == "True")//初审通过,新增 level=1,ischecked=true
                {

                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);
                    bool isTop = contentInfo.IsTop;
                    bool theIsChecked = contentInfo.IsChecked;
                    try
                    {
                        contentInfo.LastEditUserName = AdminManager.Current.UserName;
                        contentInfo.LastEditDate = DateTime.Now;
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                        contentInfo.CheckedLevel = 1;
                        contentInfo.IsChecked = true;
                        contentInfo.NodeID = TranslateUtils.ToInt(this.ddlNodeID.SelectedValue);
                        DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                        SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                        submissionInfo.CheckedLevel = 1;
                        submissionInfo.IsChecked = true;
                        DataProvider.MlibDAO.Update(submissionInfo);

                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        //LogUtils.SystemErrorLogWrite(ex);
                        return;
                    }


                }
                else if (status == "False")//初审不通过,新增 level=1,ischecked=false
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);
                    bool isTop = contentInfo.IsTop;
                    bool theIsChecked = contentInfo.IsChecked;
                    try
                    {
                        contentInfo.LastEditUserName = AdminManager.Current.UserName;
                        contentInfo.LastEditDate = DateTime.Now;
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                        contentInfo.CheckedLevel = 1;
                        contentInfo.IsChecked = false;
                        DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);


                        SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                        submissionInfo.CheckedLevel = 1;
                        submissionInfo.IsChecked = false;
                        DataProvider.MlibDAO.Update(submissionInfo);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        //LogUtils.SystemErrorLogWrite(ex);
                        return;
                    }

                }
                else //保持不变,不新增 只修改
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);

                    try
                    {
                        contentInfo.LastEditUserName = AdminManager.Current.UserName;
                        contentInfo.LastEditDate = DateTime.Now;
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                        DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        //LogUtils.SystemErrorLogWrite(ex);
                        return;
                    }
                }

                PageUtils.Redirect("DraftList.aspx?PublishmentSystemID="+this.PublishmentSystemID);
            }

        }



        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
