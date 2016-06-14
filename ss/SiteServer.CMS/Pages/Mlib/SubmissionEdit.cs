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

namespace SiteServer.CMS.Pages.Mlib
{
    public class SubmissionEdit : SystemBasePage
    {
        public Literal ltlTitle;
        public AuxiliaryControl acAttributes;
        public Button btnSubmit;

        public int submissionID;
        public int contentID;


        private string returnUrl;
        public string homeUrl;


        public void Page_Load(object sender, EventArgs E)
        {
            submissionID = TranslateUtils.ToInt(Request.QueryString["SubmissionID"]);

            //ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.PublishmentSystemInfo.Additional.UserContributeNodeIDs);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);

            var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(submissionID);
            DataView dv = new DataView(contentIDs.Tables[0]);
            dv.RowFilter = "checkedLevel=0";

            this.contentID = TranslateUtils.ToInt(dv[0]["ID"].ToString());

            ContentInfo contentInfo = null;

            if (this.contentID != 0)
            {
                contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfo != null)
                homeUrl = publishmentSystemInfo.PublishmentSystemDir;

            if (!IsPostBack)
            {
                this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, true, base.IsPostBack);

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

                this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, nodeInfo.NodeID, relatedIdentities, tableStyle, tableName, true, base.IsPostBack);

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

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);

            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                try
                {
                    contentInfo.NodeID = base.PublishmentSystemID;
                    contentInfo.LastEditUserName = UserManager.Current.UserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.AddDate = DateTime.Now;

                    NameValueCollection attributes = new NameValueCollection();
                    foreach (string key in base.Request.Form.Keys)
                    {
                        attributes[key] = PageUtils.GetSafeHtmlFragment(base.Request.Form[key]);
                    }
                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, base.PublishmentSystemInfo, relatedIdentities, attributes, contentInfo.Attributes, ContentAttribute.HiddenAttributes);
                    this.Upload(contentInfo, tableStyle, tableName, relatedIdentities);

                    contentInfo.IsChecked = true;
                    contentInfo.CheckedLevel = 0;


                    DataProvider.ContentDAO.Update(tableName, base.PublishmentSystemInfo, contentInfo);
                    Response.Write("<script>alert('草稿修改完成');location.href='contents.aspx'</script>");
                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("<script>alert('内容添加失败：{0}');</script>", ex.Message.Replace("'", "\\'")));
                    return;
                }
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
