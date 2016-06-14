using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Web;
using SiteServer.CMS.Pages.Controls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model.MLib;

namespace SiteServer.CMS.Pages.Mlib
{
    public class Submission : SystemBasePage
    {
        public Literal ltlTitle;
        public AuxiliaryControl acAttributes;
        public Button btnSubmit;

        private string returnUrl;
        public string homeUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if(base.PublishmentSystemInfo==null){
		Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
                Response.End();
		return;
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfo != null)
                homeUrl = publishmentSystemInfo.PublishmentSystemDir;

            int nodeID = base.PublishmentSystemID;
            
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);



            if (!IsPostBack)
            {
                this.acAttributes.SetParameters(new NameValueCollection(), base.PublishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, false, base.IsPostBack);


                string syncScript = string.Empty;

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.IsVisible && styleInfo.InputType == EInputType.TextEditor)
                    {
                        ETextEditorType editorType = ETextEditorType.UEditor;
                        if (string.IsNullOrEmpty(styleInfo.Additional.EditorTypeString))
                        {
                            editorType = base.PublishmentSystemInfo.Additional.TextEditorType;
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

                this.btnSubmit.Attributes.Add("onclick", syncScript + InputParserUtils.GetValidateSubmitOnClickScript("myForm"));

                this.ltlTitle.Text = "";

            }
            else
            {

                this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, false, base.IsPostBack);

            }
            base.DataBind();
        }

        private void Upload(ContentInfo contentInfo, ETableStyle tableStyle, string tableName, ArrayList relatedIdentities)
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
                        string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                        string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                        string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                        if (styleInfo.InputType == EInputType.Image)
                        {
                            if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("上传图片格式不正确！");
                            }
                            if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传图片超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                            string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                            imageUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, imageUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, imageUrl);
                        }
                        else if (styleInfo.InputType == EInputType.Video)
                        {
                            if (!PathUtility.IsVideoExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("上传视频格式不正确！");
                            }
                            if (!PathUtility.IsVideoSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传视频超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            string videoUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                            videoUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, videoUrl);
                            contentInfo.SetExtendedAttribute(styleInfo.AttributeName, videoUrl);
                        }
                        else if (styleInfo.InputType == EInputType.File)
                        {
                            if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                            {
                                throw new Exception("此格式不允许上传，请选择有效的文件！");
                            }
                            if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                            {
                                throw new Exception("上传失败，上传文件超出规定文件大小！");
                            }
                            postedFile.SaveAs(localFilePath);
                            FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                            string fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                            fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
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


                #region 插入ml_content
                int nodeID = base.PublishmentSystemID;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);


                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);
                try
                {
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                    contentInfo.NodeID = nodeID;
                    if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                    {
                        //base.FailMessage(string.Format("内容添加失败：系统时间不能为{0}年", DateUtils.SqlMinValue.Year));
                        return;
                    }

                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                    this.Upload(contentInfo, tableStyle, tableName, relatedIdentities);
                    //contentInfo.SourceID = SourceManager.TouGao;
                    contentInfo.AddUserName = UserManager.Current.UserName;
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.IsChecked = true;
                    contentInfo.CheckedLevel = 0;

                    #region 插入ml_Submission
                    SubmissionInfo submissionInfo = new SubmissionInfo()
                    {
                        AddUserName = UserManager.Current.UserName,
                        Title = contentInfo.Title,
                        AddDate = DateTime.Now,
                        IsChecked = true,
                        CheckedLevel = 0,
                        PassDate = null,
                        ReferenceTimes = 0
                    };
                    int submissionId = DataProvider.MlibDAO.Insert(submissionInfo);
                    #endregion
                    //ReferenceID不为空的时候，SourceID不能为空，update by sessionliang at 20151207
                    contentInfo.SourceID = nodeID;
                    contentInfo.ReferenceID = submissionId;
                    int contentID = DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);
                    contentInfo.ID = contentID;
                    //base.SuccessMessage("草稿添加完成");

                    Response.Write("<script>alert('草稿添加完成');location.href='contents.aspx'</script>");
                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("<script>alert('内容添加失败：{0}');</script>", ex.Message.Replace("'","\\'")));
                }
                //PageUtils.Redirect(this.ReturnUrl);

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

        public string ReturnUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.returnUrl))
                {
                    this.returnUrl = string.Format("submission.aspx");
                }
                return this.returnUrl;
            }
        }
    }
}
