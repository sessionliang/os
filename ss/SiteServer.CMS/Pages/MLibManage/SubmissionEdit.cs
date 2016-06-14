using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Web;
using System.Data;
using SiteServer.CMS.Pages.Controls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;

namespace SiteServer.CMS.Pages.MLibManage
{
    public class SubmissionEdit : SystemBasePage
    {
        public Literal ltlTitle;
        public MLibAuxiliaryControl acAttributes;
        public Button Submit;
        public DropDownList ddlPublishmentSystem;
        public DropDownList NodeIDDropDownList;
        public HiddenField MLibDraftContentID;

        public int contentID;


        private string returnUrl;
        public string homeUrl;
        private MLibDraftContentInfo minfo;


        public void Page_Load(object sender, EventArgs E)
        {
            if (ConfigManager.Additional.IsUseMLib == false)
            {
                Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
                Response.End();
            }
            #region 是否还在投稿时间内
            bool canMlib = UserManager.CurrentCanDoByValidityDate;
             
            if (!canMlib)
            {
                Response.Write("<script>alert('当前时间已超出你的投稿有效期，不能使用投稿，如有需要请联系管理员. ');history.go(-1);</script>");
                Response.End();
                return;
            }
            #endregion
            //是否还有投稿数未使用 
            canMlib = UserManager.CurrentCanDoByMLibNum;
            if (!canMlib)
            {
                Response.Write("<script>alert('您的投稿数量已达到上限.');history.go(-1);</script>");
                Response.End();
                return;
            }
            //稿件发布者被锁定，不能投稿 
            canMlib = UserManager.CurrentCanDoByAdmin;
            if (!canMlib)
            {
                Response.Write("<script>alert('您的投稿设置已被修改，有可能是稿件发布者的管理员账号被锁定，请联系管理员.');history.go(-1);</script>");
                Response.End();
                return;
            }
            contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);

            this.minfo = DataProvider.MLibDraftContentDAO.GetMLibDraftContentInfo(contentID);

            PublishmentSystemInfo userCenterPinfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (userCenterPinfo != null)
                homeUrl = userCenterPinfo.PublishmentSystemDir;

            if (!IsPostBack)
            {
                #region 获取稿件发布都有权限的站点和栏目
                if (this.ddlPublishmentSystem.Items.Count == 0)
                {
                    this.ddlPublishmentSystem.Items.Clear();
                    ListItem itemd = new ListItem("请选择", "0");
                    this.ddlPublishmentSystem.Items.Add(itemd);
                    foreach (PublishmentSystemInfo info in PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser))
                    {
                        ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                        this.ddlPublishmentSystem.Items.Add(item);
                    }
                }
                #endregion
                this.ddlPublishmentSystem.SelectedValue = this.minfo.PublishmentSystemID.ToString(); PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.minfo.PublishmentSystemID);
                this.NodeIDDropDownList.Items.Clear();
                ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(ConfigManager.Additional.UnifiedMLibAddUser, pinfo.PublishmentSystemID);
                foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
                {
                    ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                    this.NodeIDDropDownList.Items.Add(item);
                }
                this.NodeIDDropDownList.SelectedValue = this.minfo.NodeID.ToString();
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.minfo.PublishmentSystemID);
                getView(publishmentSystemInfo, this.minfo.NodeID);
            }
            else
            {

                //this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, true, base.IsPostBack);

            }
            base.DataBind();
        }

        public void getView(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (nodeInfo == null)
            {
                Response.Write("<script>alert('所选择站点不能投稿，请选择其他站点.'); </script>");
                return;
            }
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = base.MLibDraftContentTableName;
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID);

            ContentInfo contentInfo = null;

            if (this.contentID != 0)
            {
                contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
            }

            //获取栏目字段 
            MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            string files = this.MLibDraftContentAttributeNames(tableName);
            if (minfo != null && !string.IsNullOrEmpty(minfo.Field))
                files = minfo.Field;

            string nodeTableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            try
            {

                this.acAttributes.SetParameters(contentInfo.Attributes, publishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, nodeTableName, true, base.IsPostBack, files);
            }
            catch
            {
                Response.Write("<script>location.href='contents.aspx'; </script>");
            }


            string syncScript = string.Empty;
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, nodeTableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible && styleInfo.InputType == EInputType.TextEditor)
                {
                    ETextEditorType editorType = ETextEditorType.UEditor;
                    if (string.IsNullOrEmpty(styleInfo.Additional.EditorTypeString))
                    {
                        editorType = publishmentSystemInfo.Additional.TextEditorType;
                    }
                    else
                    {
                        editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
                    }

                    if (editorType == ETextEditorType.UEditor)
                    {
                        syncScript += string.Format(@"UE.getEditor('{0}').sync();", styleInfo.AttributeName);
                    }
                }
            }


            this.Submit.Attributes.Add("onclick", syncScript + InputParserUtils.GetValidateSubmitOnClickScript("myForm", true, "autoCheckKeywords()"));

            this.ltlTitle.Text = "";

        }


        public void ddlPublishmentSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);
            this.NodeIDDropDownList.Items.Clear();
            ListItem itemd = new ListItem("请选择", "0");
            this.NodeIDDropDownList.Items.Add(itemd);
            ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, pinfo.PublishmentSystemID);
            foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
            {
                ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                this.NodeIDDropDownList.Items.Add(item);
            }
        }

        public void NodeIDDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);
            getView(pinfo, TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue));
        }

        private void Upload(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, ETableStyle tableStyle, string tableName, ArrayList relatedIdentities)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                if (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.Video || styleInfo.InputType == EInputType.File)
                {
                    string attributeName = InputTypeParser.GetAttributeNameToUploadForTouGao(styleInfo.AttributeName);
                    if (base.Request.Files[attributeName] != null && !string.IsNullOrEmpty(base.Request.Files[attributeName].FileName))
                    {
                        HttpPostedFile postedFile = base.Request.Files[attributeName];
                        string filePath = postedFile.FileName;
                        string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                        string localDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                        string localFileName = PathUtility.GetUploadFileName(publishmentSystemInfo, filePath);
                        string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                        if (styleInfo.InputType == EInputType.Image)
                        {
                            if (!PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("上传图片格式不正确！");
                            }
                            if (!PathUtility.IsImageSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传图片超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(publishmentSystemInfo, localFilePath);
                            string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            imageUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, imageUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, imageUrl);
                        }
                        else if (styleInfo.InputType == EInputType.Video)
                        {
                            if (!PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("上传视频格式不正确！");
                            }
                            if (!PathUtility.IsVideoSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传视频超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            string videoUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            videoUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, videoUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, videoUrl);
                        }
                        else if (styleInfo.InputType == EInputType.File)
                        {
                            if (!PathUtility.IsFileExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("此格式不允许上传，请选择有效的文件！");
                            }
                            if (!PathUtility.IsFileSizeAllowed(publishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传文件超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(publishmentSystemInfo, localFilePath);
                            string fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                            fileUrl = PageUtility.GetVirtualUrl(publishmentSystemInfo, fileUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, fileUrl);
                        }
                    }
                }
            }
        }

        public void btnSubmit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {

                #region 是否还在投稿时间内
                bool canMlib = UserManager.CurrentCanDoByValidityDate;
                if (!canMlib)
                {
                    Response.Write("<script>alert('当前时间已超出你的投稿有效期，不能使用投稿，如有需要请联系管理员.');history.go(-1);</script>");
                    Response.End();
                    return;
                }
                #endregion

                //是否还有投稿数未使用 
                canMlib = UserManager.CurrentCanDoByMLibNum;
                if (!canMlib)
                {
                    Response.Write("<script>alert('您的投稿数量已达到上限.');history.go(-1);</script>");
                    Response.End();
                    return;
                }

                if (this.ddlPublishmentSystem.SelectedValue == "0")
                {
                    Response.Write("<script>alert('请选择站点');</script>");
                    return;
                }
                if (this.NodeIDDropDownList.SelectedValue == "0")
                {
                    Response.Write("<script>alert('请选择栏目');</script>");
                    return;
                }
                int publishmentSystemID = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);

                //发布者 
                string userName = UserManager.CurrentNewGroupMLibAddUser;

                #region 提交到站点下去
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);
                try
                {
                    contentInfo.PublishmentSystemID = publishmentSystemID;
                    contentInfo.NodeID = nodeID;
                    if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                    { 
                        return;
                    }

                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                    if (string.IsNullOrEmpty(contentInfo.Title))
                    {
                        return;
                    }
                    this.Upload(publishmentSystemInfo, contentInfo, tableStyle, tableName, relatedIdentities);
                    contentInfo.AddUserName = userName;
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.SourceID = nodeID;
                    contentInfo.ReferenceID = 0;
                    contentInfo.MemberName = UserManager.Current.UserName;

                    int checkedLevel = 0;
                    bool isChecked = base.ContentIsChecked(contentInfo.AddUserName, publishmentSystemInfo, nodeInfo.NodeID, out checkedLevel);

                    contentInfo.IsChecked = isChecked;
                    contentInfo.CheckedLevel = checkedLevel;

                    int id = DataProvider.ContentDAO.Insert(tableName, publishmentSystemInfo, contentInfo);
                    contentInfo.ID = id;

                    //判断是不是有审核权限
                    int checkedLevelOfUser = 0;
                    bool isCheckedOfUser = CheckManager.GetUserCheckLevel(publishmentSystemInfo, contentInfo.NodeID, out checkedLevelOfUser);
                    if (LevelManager.IsCheckable(publishmentSystemInfo, contentInfo.NodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isCheckedOfUser, checkedLevelOfUser))
                    {
                        //添加审核记录
                        BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, new ArrayList() { id }, 0, true, contentInfo.AddUserName, contentInfo.IsChecked, contentInfo.CheckedLevel, "");
                    }

                    //修改用户表中的投稿数量
                    UserInfo userInfo = UserManager.Current;
                    userInfo.MLibNum = userInfo.MLibNum + 1;
                    BaiRongDataProvider.UserDAO.Update(userInfo);

                    //删除草稿
                    DataProvider.MLibDraftContentDAO.Delete(TranslateUtils.StringCollectionToArrayList(this.contentID.ToString()));


                    //生成页面
                    if (isChecked)
                    {
                        //                        NameValueCollection nvc = new NameValueCollection();
                        //                        nvc.Add("type", "AjaxUrlFSO");
                        //                        nvc.Add("method", "CreateImmediately");
                        //                        nvc.Add("publishmentSystemID", publishmentSystemID.ToString());
                        //                        nvc.Add("changedType", "Edit");
                        //                        nvc.Add("templateType", "ContentTemplate");
                        //                        nvc.Add("channelID", contentInfo.NodeID.ToString());
                        //                        nvc.Add("contentID", id.ToString());
                        //                        nvc.Add("fileTemplateID", "0");
                        //                        string url = @"/siteserver/stl/background_serviceSTL.aspx";
                        //                        StringBuilder builder = new StringBuilder();
                        //                        builder.AppendFormat(@" 
                        //    function submitAjaxUrl(url,params){{
                        //        $.post(url,params);
                        //    }}
                        //submitAjaxUrl('{0}',{1});
                        // ", url, TranslateUtils.NameValueCollectionToJsonString(nvc));
                        //                        if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "keys"))
                        //                        {

                        //                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "keys", builder.ToString(), true);

                        //                        }


                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(publishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, nodeInfo.NodeID, id, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }
                    Response.Write("<script>alert('稿件提交完成');location.href='contents.aspx'</script>");
                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("<script>alert('内容添加失败：{0}');</script>", ex.Message.Replace("'", "\\'")));
                    return;
                }

                #endregion
            }
        }
        public void btnSubmitCaogao_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.ddlPublishmentSystem.SelectedValue == "0")
                {
                    Response.Write("<script>alert('请选择站点');</script>");
                    return;
                }

                if (this.NodeIDDropDownList.SelectedValue == "0")
                {
                    Response.Write("<script>alert('请选择栏目');</script>");
                    return;
                }
                int publishmentSystemID = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                int nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);

                #region 修改草稿
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);
                try
                {
                    contentInfo.PublishmentSystemID = publishmentSystemID;
                    contentInfo.NodeID = nodeID;
                    if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                    { 
                        return;
                    }

                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                    if (string.IsNullOrEmpty(contentInfo.Title))
                    {
                        return;
                    }
                    this.Upload(publishmentSystemInfo, contentInfo, tableStyle, tableName, relatedIdentities);
                    contentInfo.ID = this.contentID;
                    contentInfo.AddUserName = UserManager.Current.UserName;
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;

                    contentInfo.CheckedLevel = LevelManager.LevelInt.CaoGao;
                    contentInfo.IsChecked = false;
                    contentInfo.SourceID = nodeID;
                    contentInfo.ReferenceID = 0;
                    contentInfo.MemberName = UserManager.Current.UserName;
                    DataProvider.ContentDAO.Update(base.MLibDraftContentTableName, publishmentSystemInfo, contentInfo);

                    Response.Write("<script>alert('草稿修改完成');location.href='draftcontent.aspx'</script>");

                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("<script>alert('内容修改失败：{0}');</script>", ex.Message.Replace("'", "\\'")));
                }
                #endregion
            }

        }

        public static string GetUploadWordUrl(int publishmentSystemID, bool isClearFormat, bool isClearImages, string returnUrl)
        {
            return string.Format("contentAdd.aspx?PublishmentSystemID={0}&IsUploadWord=True&isClearFormat={1}&isClearImages={2}&ReturnUrl={3}", publishmentSystemID, isClearFormat, isClearImages, returnUrl);
        }

        public static string GetRedirectUrlOfEdit(int publishmentSystemID, int nodeID, int id, string returnUrl)
        {
            return string.Format("contentAdd.aspx?PublishmentSystemID={0}&NodeID={1}&ID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, id, StringUtils.ValueToUrl(returnUrl));
        }
    }
}
