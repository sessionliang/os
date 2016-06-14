using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Generic;
using BaiRong.Text.LitJson;
using System.Web;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlElementFile : BackgroundBasePage
    {
        public Literal ltlSetting;

        public DropDownList ddlFileSource;
        public DropDownList ddlType;
        public TextBox tbSrc;
        public CheckBox cbIsCount;
        public CheckBox cbIsFilesize;

        public Literal ltlStlElement;

        private TemplateInfo templateInfo;
        private int nodeID;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string oldStlElement;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToEdit(int publishmentSystemID, int templateID, int nodeID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", false.ToString());
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlEncryptElement", stlEncryptElement);
            return JsUtils.Layer.GetOpenLayerString("附件（stl:file）编辑", PageUtils.GetSTLUrl("modal_stlElementFile.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, int nodeID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("附件（stl:file）新增", PageUtils.GetSTLUrl("modal_stlElementFile.aspx"), arguments);
        }

        public string UploadUrl
        {
            get
            {
                return PageUtils.GetSTLUrl(string.Format("modal_stlElementFile.aspx?PublishmentSystemID={0}&upload=True", base.PublishmentSystemID));
            }
        }

        public string TypeCollection
        {
            get { return base.PublishmentSystemInfo.Additional.FileUploadTypeCollection; }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!string.IsNullOrEmpty(base.GetQueryString("upload")))
            {
                string json = JsonMapper.ToJson(this.Upload());
                base.Response.Write(json);
                base.Response.End();
                return;
            }

            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.nodeID = base.GetIntQueryString("nodeID");
            this.includeUrl = base.GetQueryString("includeUrl");
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.oldStlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlEncryptElement"));

            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("指定内容附件字段", "attribute");
                this.ddlFileSource.Items.Add(listItem);                
                listItem = new ListItem("指定附件地址", "src");
                this.ddlFileSource.Items.Add(listItem);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
                StringCollection attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(nodeInfo.Additional.ContentAttributesOfDisplay);

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                ArrayList columnTableStyleInfoArrayList = ContentUtility.GetColumnTableStyleInfoArrayList(base.PublishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
                foreach (TableStyleInfo styleInfo in columnTableStyleInfoArrayList)
                {
                    listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    this.ddlType.Items.Add(listItem);
                }

                this.ddlType.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                ControlUtils.SelectListItemsIgnoreCase(this.ddlType, BackgroundContentAttribute.FileUrl);
            }

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);

            if (!string.IsNullOrEmpty(attributes[StlFile.Attribute_Type]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlType, attributes[StlFile.Attribute_Type]);
                ControlUtils.SelectListItemsIgnoreCase(this.ddlFileSource, "attribute");
            }
            else if (!string.IsNullOrEmpty(attributes[StlFile.Attribute_Src]))
            {
                this.tbSrc.Text = attributes[StlFile.Attribute_Src];
                ControlUtils.SelectListItemsIgnoreCase(this.ddlFileSource, "src");
            }
            if (!string.IsNullOrEmpty(attributes[StlFile.Attribute_IsCount]))
            {
                this.cbIsCount.Checked = TranslateUtils.ToBool(attributes[StlFile.Attribute_IsCount]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFile.Attribute_IsFilesize]))
            {
                this.cbIsFilesize.Checked = TranslateUtils.ToBool(attributes[StlFile.Attribute_IsFilesize]);
            }

            this.ltlStlElement.Text = StringUtils.HtmlEncode(stlElement).Trim().Replace("  ", string.Empty);
        }

        private Hashtable Upload()
        {
            bool success = false;
            string fileUrl = string.Empty;
            string message = "附件上传失败";

            if (base.Request.Files != null && base.Request.Files["filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["filedata"];
                try
                {
                    if (postedFile != null && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string filePath = postedFile.FileName;
                        string fileExtName = PathUtils.GetExtension(filePath);

                        bool isAllow = true;
                        if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                        {
                            message = "此格式不允许上传，请选择有效的附件文件";
                            isAllow = false;
                        }
                        if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            message = "上传失败，上传文件超出规定文件大小";
                            isAllow = false;
                        }

                        if (isAllow)
                        {
                            string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                            string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                            string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                            postedFile.SaveAs(localFilePath);

                            fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                            fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);

                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            Hashtable jsonAttributes = new Hashtable();
            if (success)
            {
                jsonAttributes.Add("success", "true");
                jsonAttributes.Add("fileUrl", fileUrl);
            }
            else
            {
                jsonAttributes.Add("success", "false");
                jsonAttributes.Add("message", message);
            }

            return jsonAttributes;
        }

        private string GetStlElement()
        {
            string stlElement = string.Empty;
            if (!this.isStlInsert)
            {
                stlElement = this.oldStlElement;
            }
            else
            {
                string stlElementToAdd = base.GetQueryString("stlElementToAdd");
                stlElementToAdd = stlElementToAdd.Replace("__R_A_N__", StringUtils.Constants.ReturnAndNewline);
                stlElement = stlElementToAdd.Trim();
            }

            if (base.Page.IsPostBack)
            {
                NameValueCollection attributes = StlParserUtility.GetStlAttributes(stlElement);

                if (this.ddlFileSource.SelectedValue == "attribute")
                {
                    TranslateUtils.SetOrRemoveAttribute(attributes, "type", this.ddlType.SelectedValue);
                }
                else if (this.ddlFileSource.SelectedValue == "src")
                {
                    TranslateUtils.SetOrRemoveAttribute(attributes, "src", this.tbSrc.Text);
                }
                TranslateUtils.SetOrRemoveAttribute(attributes, "isCount", this.cbIsCount.Checked.ToString().ToLower());
                TranslateUtils.SetOrRemoveAttribute(attributes, "isFilesize", this.cbIsFilesize.Checked.ToString().ToLower());

                stlElement = StlParserUtility.GetStlElement(StlFile.ElementName, attributes, null);
            }
            return stlElement;
        }

        public void btnApply_OnClick(object sender, EventArgs e)
        {
            this.ltlSetting.Text += "$('#myTab a').eq(1).click();";
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                string stlElement = this.GetStlElement();

                if (this.isStlInsert)
                {
                    bool stlIsContainer = TranslateUtils.ToBool(base.GetQueryString("stlIsContainer"));
                    string stlSequenceCollection = base.GetQueryString("stlSequenceCollection");
                    int stlDivIndex = TranslateUtils.ToInt(base.GetQueryString("stlDivIndex"));

                    TemplateDesignManager.InsertStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, stlSequenceCollection, stlElement, stlDivIndex, stlIsContainer);
                }
                else
                {
                    TemplateDesignManager.UpdateStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, this.stlIndex, this.oldStlElement, stlElement);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.Layer.CloseModalLayer(Page);
            }
        }
    }
}
