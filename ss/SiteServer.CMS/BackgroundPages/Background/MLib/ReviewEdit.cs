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
    public class ReviewEdit : MLibBackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlTabAction;

        public AuxiliaryControl acAttributes;

        public RadioButtonList ContentLevel;
        public DropDownList ddlNodeID;
        public DropDownList ddlTrunslateType;

        public PlaceHolder phStatus;

        public Button Submit;

        private NodeInfo nodeInfo;
        private int contentID;
        private ArrayList relatedIdentities;
        private string returnUrl;
        private ETableStyle tableStyle;
        private string tableName;
        private int MaxCheckLevel;

        public void Page_Load(object sender, EventArgs E)
        {
            int nodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));



            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);
            SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);

            if (contentInfo.CheckedLevel > MaxCheckLevel || (contentInfo.CheckedLevel == MaxCheckLevel && contentInfo.IsChecked))
            {
                if (!this.HasNodePermissions(MaxCheckLevel.ToString(), nodeID.ToString()))
                {
                    Response.Write("<script>alert('您没有此栏目的终审权限');history.go(-1);</script>");
                    Response.End();
                }
            }
            else
            {
                if (contentInfo.IsChecked)
                {
                    if (!this.HasNodePermissions((contentInfo.CheckedLevel + 1).ToString(), nodeID.ToString()))
                    {
                        Response.Write("<script>alert('您没有此栏目的" + Number2Chinese(contentInfo.CheckedLevel + 1) + "审权限');history.go(-1);</script>");
                        Response.End();
                    }
                }
                else
                {
                    if (!this.HasNodePermissions((contentInfo.CheckedLevel - 1).ToString(), nodeID.ToString()))
                    {
                        Response.Write("<script>alert('您没有此栏目的" + Number2Chinese(contentInfo.CheckedLevel - 1) + "审权限');history.go(-1);</script>");
                        Response.End();
                    }
                }
            }

            //if (contentInfo.CheckedLevel >= MaxCheckLevel)
            //{
            //    Response.Redirect("ReviewShow.aspx?PublishmentSystemID=" + base.PublishmentSystemID
            //        + "&nodeid=" + nodeID + "&id=" + contentID);
            //    return;
            //}

            if (!IsPostBack)
            {
                string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeInfo.NodeID);
                string pageTitle = "稿件审核";
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

                //绑定分类
                var nodelist = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);

                nodelist.Add(new NodeInfo());
                this.ddlNodeID.Items.Add("不转移");
                foreach (NodeInfo item in nodelist)
                {
                    if (item.NodeID != this.nodeInfo.NodeID)
                    {
                        this.ddlNodeID.Items.Add(new ListItem(item.NodeName, item.NodeID.ToString()));
                    }
                }
                this.ddlNodeID.SelectedIndex = 0;

                //审核选项
                if (submissionInfo.CheckedLevel > MaxCheckLevel || (submissionInfo.CheckedLevel == MaxCheckLevel && submissionInfo.IsChecked))
                {
                    this.ContentLevel.Items.Add(new ListItem("终审通过", "True"));
                    this.ContentLevel.Items.Add(new ListItem("终审不通过", "False"));
                    this.ContentLevel.Items.Add(new ListItem("保持不变", "NoChange"));
                    this.ContentLevel.SelectedIndex = 0;
                }
                else
                {
                    if (contentInfo.IsChecked)
                    {
                        this.ContentLevel.Items.Add(new ListItem(Number2Chinese(contentInfo.CheckedLevel + 1) + "审通过", "True"));
                        this.ContentLevel.Items.Add(new ListItem(Number2Chinese(contentInfo.CheckedLevel + 1) + "审不通过", "False"));
                        this.ContentLevel.Items.Add(new ListItem("保持不变", "NoChange"));
                        this.ContentLevel.SelectedIndex = 0;
                    }
                    else
                    {
                        this.ContentLevel.Items.Add(new ListItem(Number2Chinese(contentInfo.CheckedLevel - 1) + "审通过", "True"));
                        this.ContentLevel.Items.Add(new ListItem(Number2Chinese(contentInfo.CheckedLevel - 1) + "审不通过", "False"));
                        this.ContentLevel.Items.Add(new ListItem("保持不变", "NoChange"));
                        this.ContentLevel.SelectedIndex = 0;
                    }
                }

                #region 其他阶段稿件链接
                var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(contentInfo.ReferenceID);
                for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
                {
                    int itemCheckLevel = TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["checkedLevel"].ToString());

                    string statusText = "";
                    if (submissionInfo.PassDate != null && contentIDs.Tables[0].Rows[i]["IsChecked"].ToString() == "True" && itemCheckLevel == MaxCheckLevel && i == contentIDs.Tables[0].Rows.Count - 1)
                    {
                        statusText = "终审稿";
                    }
                    else
                    {
                        if (itemCheckLevel >= MaxCheckLevel)
                        {

                            statusText = Number2Chinese(itemCheckLevel - 1) + "审稿";
                        }
                        else
                        {
                            statusText = Number2Chinese(itemCheckLevel) + "审稿";
                        }
                    }


                    ltlTabAction.Text += string.Format("<input type=\"button\" class=\"btn\" onclick=\"location.href='ReviewShow.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}';\" value=\"{3}\"/>",
                        base.PublishmentSystemID,
                        TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["NodeID"].ToString()),
                        TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()),
                        itemCheckLevel == 0 ? "草稿" : statusText);
                }
                ltlTabAction.Text += string.Format("<input type=\"button\" class=\"btn btn-info\" value=\"编辑\"/>");

                #endregion

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
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);
                string status = this.ContentLevel.SelectedValue;
                if (status == "True")
                {
                    try
                    {
                        if (ddlNodeID.SelectedValue == "不转移")
                        {
                            contentInfo.LastEditUserName = AdminManager.Current.UserName;
                            contentInfo.LastEditDate = DateTime.Now;
                            InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                            if (contentInfo.IsChecked)
                            {
                                contentInfo.CheckedLevel++;
                            }
                            else
                            {
                                contentInfo.CheckedLevel--;
                            }

                            contentInfo.IsChecked = true;
                            DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                            SubmissionInfo submissionInfo =DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                            submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                            submissionInfo.IsChecked = contentInfo.IsChecked;
                            if (submissionInfo.CheckedLevel >= MaxCheckLevel)
                            {
                                submissionInfo.PassDate = DateTime.Now;
                            }
                            DataProvider.MlibDAO.Update(submissionInfo);
                        }
                        else
                        {
                            if (ddlTrunslateType.SelectedValue == "Cut")
                            {
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                if (contentInfo.IsChecked)
                                {
                                    contentInfo.CheckedLevel++;
                                }
                                else
                                {
                                    contentInfo.CheckedLevel--;
                                }
                                contentInfo.IsChecked = true;
                                contentInfo.NodeID = TranslateUtils.ToInt(this.ddlNodeID.SelectedValue);
                                DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                                SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                                submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                                submissionInfo.IsChecked = contentInfo.IsChecked;
                                if (submissionInfo.CheckedLevel >= MaxCheckLevel)
                                {
                                    submissionInfo.PassDate = DateTime.Now;
                                }
                                DataProvider.MlibDAO.Update(submissionInfo);
                            }
                            else
                            {
                                #region 原稿
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                if (contentInfo.IsChecked)
                                {
                                    contentInfo.CheckedLevel++;
                                }
                                else
                                {
                                    contentInfo.CheckedLevel--;
                                }
                                contentInfo.IsChecked = true;
                                DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);
                                SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                                submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                                submissionInfo.IsChecked = contentInfo.IsChecked;
                                if (submissionInfo.CheckedLevel >= MaxCheckLevel)
                                {
                                    submissionInfo.PassDate = DateTime.Now;
                                }
                                DataProvider.MlibDAO.Update(submissionInfo);
                                #endregion

                                #region 新增稿

                                SubmissionInfo submissionInfo1 = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);

                                if (submissionInfo1.CheckedLevel >= MaxCheckLevel)
                                {
                                    submissionInfo1.PassDate = DateTime.Now;
                                }

                                int newSid = DataProvider.MlibDAO.Insert(submissionInfo1);
                                var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(contentInfo.ReferenceID);
                                for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
                                {
                                    ContentInfo contentInfotmp = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()));
                                    contentInfotmp.ReferenceID = newSid;
                                    if (contentInfo.CheckedLevel == contentInfotmp.CheckedLevel && contentInfo.IsChecked == contentInfotmp.IsChecked)
                                    {
                                        contentInfotmp.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);

                                    }
                                    DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfotmp);


                                }
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        //LogUtils.SystemErrorLogWrite(ex);
                        return;
                    }
                }
                else if (status == "False")
                {
                    try
                    {
                        if (ddlNodeID.SelectedValue == "不转移")
                        {
                            contentInfo.LastEditUserName = AdminManager.Current.UserName;
                            contentInfo.LastEditDate = DateTime.Now;
                            InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                            if (contentInfo.IsChecked)
                            {
                                contentInfo.CheckedLevel++;
                            }
                            else
                            {
                                contentInfo.CheckedLevel--;
                            }
                            contentInfo.IsChecked = false;
                            DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                            SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                            submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                            submissionInfo.IsChecked = contentInfo.IsChecked;
                            DataProvider.MlibDAO.Update(submissionInfo);
                        }
                        else
                        {
                            if (ddlTrunslateType.SelectedValue == "Cut")
                            {
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                contentInfo.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                if (contentInfo.IsChecked)
                                {
                                    contentInfo.CheckedLevel++;
                                }
                                else
                                {
                                    contentInfo.CheckedLevel--;
                                }
                                contentInfo.IsChecked = false;
                                DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                                SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                                submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                                submissionInfo.IsChecked = contentInfo.IsChecked;
                                DataProvider.MlibDAO.Update(submissionInfo);
                            }
                            else
                            {
                                #region 原稿
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                if (contentInfo.IsChecked)
                                {
                                    contentInfo.CheckedLevel++;
                                }
                                else
                                {
                                    contentInfo.CheckedLevel--;
                                }
                                contentInfo.IsChecked = false;
                                DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                                SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                                submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                                submissionInfo.IsChecked = contentInfo.IsChecked;
                                DataProvider.MlibDAO.Update(submissionInfo);
                                #endregion

                                #region 新增稿

                                SubmissionInfo submissionInfo1 = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                                if (submissionInfo1.CheckedLevel >= MaxCheckLevel)
                                {
                                    submissionInfo1.PassDate = DateTime.Now;
                                }
                                int newSid = DataProvider.MlibDAO.Insert(submissionInfo1);
                                var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(contentInfo.ReferenceID);
                                for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
                                {
                                    ContentInfo contentInfotmp = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()));
                                    contentInfotmp.ReferenceID = newSid;
                                    if (contentInfo.CheckedLevel == contentInfotmp.CheckedLevel && contentInfo.IsChecked == contentInfotmp.IsChecked)
                                    {
                                        contentInfotmp.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);

                                    }
                                    DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfotmp);

                                }
                                #endregion
                            }
                        }
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
                    try
                    {
                        if (ddlNodeID.SelectedValue == "不转移")
                        {
                            contentInfo.LastEditUserName = AdminManager.Current.UserName;
                            contentInfo.LastEditDate = DateTime.Now;
                            InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                            DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);


                        }
                        else
                        {
                            if (ddlTrunslateType.SelectedValue == "Cut")
                            {
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                contentInfo.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                            }
                            else
                            {

                                #region 原稿
                                contentInfo.LastEditUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = DateTime.Now;
                                InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                                DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);

                                #endregion


                                #region 新增稿
                                SubmissionInfo submissionInfo1 = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);

                                if (submissionInfo1.CheckedLevel >= MaxCheckLevel)
                                {
                                    submissionInfo1.PassDate = DateTime.Now;
                                }

                                int newSid = DataProvider.MlibDAO.Insert(submissionInfo1);
                                var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(contentInfo.ReferenceID);
                                for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
                                {
                                    ContentInfo contentInfotmp = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()));
                                    contentInfotmp.ReferenceID = newSid;
                                    if (contentInfo.CheckedLevel == contentInfotmp.CheckedLevel && contentInfo.IsChecked == contentInfotmp.IsChecked)
                                    {
                                        contentInfotmp.NodeID = TranslateUtils.ToInt(ddlNodeID.SelectedValue);

                                    }
                                    DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfotmp);

                                }
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("内容修改失败：{0}", ex.Message));
                        //LogUtils.SystemErrorLogWrite(ex);
                        return;
                    }
                }

                PageUtils.Redirect("ReviewList.aspx?PublishmentSystemID=" + this.PublishmentSystemID + "&NodeID=" + nodeInfo.NodeID);
            }

        }



        public string ReturnUrl { get { return this.returnUrl; } }


        public string Number2Chinese(int n)
        {

            int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            if (n == MaxCheckLevel)
            {
                return "终";
            }
            var chinese = new string[] { "", "初", "二", "三", "四", "五", "六", "七", "八", "九" };
            return chinese[n];
        }
    }
}
