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
    public class StlElementPlayer : BackgroundBasePage
    {
        public Literal ltlSetting;

        public DropDownList ddlPlayBy;
        public TextBox tbPlayUrl;
        public TextBox tbWidth;
        public TextBox tbHeight;
        public CheckBox cbIsAutoPlay;

        public Literal ltlStlElement;

        private TemplateInfo templateInfo;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string oldStlElement;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToEdit(int publishmentSystemID, int templateID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", false.ToString());
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlEncryptElement", stlEncryptElement);
            return JsUtils.Layer.GetOpenLayerString("播放视频（stl:player）编辑", PageUtils.GetSTLUrl("modal_stlElementPlayer.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("播放视频（stl:player）新增", PageUtils.GetSTLUrl("modal_stlElementPlayer.aspx"), arguments);
        }

        public string UploadUrl
        {
            get
            {
                return PageUtils.GetSTLUrl(string.Format("modal_stlElementPlayer.aspx?PublishmentSystemID={0}&upload=True", base.PublishmentSystemID));
            }
        }

        public string TypeCollection
        {
            get { return base.PublishmentSystemInfo.Additional.VideoUploadTypeCollection; }
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
            this.includeUrl = base.GetQueryString("includeUrl");
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.oldStlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlEncryptElement"));

            if (!IsPostBack)
            {
                foreach (string playBy in StlPlayer.AttributeValuesPlayBy)
                {
                    ListItem listItem = new ListItem(playBy, playBy);
                    this.ddlPlayBy.Items.Add(listItem);
                }
                this.ddlPlayBy.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            }

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);
           
            if (!string.IsNullOrEmpty(attributes[StlPlayer.Attribute_PlayBy]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlPlayBy, attributes[StlPlayer.Attribute_PlayBy]);
            }
            if (!string.IsNullOrEmpty(attributes[StlPlayer.Attribute_PlayUrl]))
            {
                this.tbPlayUrl.Text = attributes[StlPlayer.Attribute_PlayUrl];
            }
            if (!string.IsNullOrEmpty(attributes[StlPlayer.Attribute_Width]))
            {
                this.tbWidth.Text = TranslateUtils.ToInt(attributes[StlPlayer.Attribute_Width], 260).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlPlayer.Attribute_Height]))
            {
                this.tbHeight.Text = TranslateUtils.ToInt(attributes[StlPlayer.Attribute_Height], 182).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlPlayer.Attribute_IsAutoPlay]))
            {
                this.cbIsAutoPlay.Checked = TranslateUtils.ToBool(attributes[StlPlayer.Attribute_IsAutoPlay]);
            }

            this.ltlStlElement.Text = StringUtils.HtmlEncode(stlElement).Trim().Replace("  ", string.Empty);
        }

        private Hashtable Upload()
        {
            bool success = false;
            string playUrl = string.Empty;
            string message = "视频上传失败";

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
                        if (!PathUtility.IsVideoExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                        {
                            message = "此格式不允许上传，请选择有效的视频文件";
                            isAllow = false;
                        }
                        if (!PathUtility.IsVideoSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
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

                            playUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                            playUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, playUrl);

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
                jsonAttributes.Add("playUrl", playUrl);
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

                TranslateUtils.SetOrRemoveAttribute(attributes, "playBy", this.ddlPlayBy.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "playUrl", this.tbPlayUrl.Text);
                TranslateUtils.SetOrRemoveAttribute(attributes, "width", TranslateUtils.ToInt(this.tbWidth.Text, 0) > 0 ? this.tbWidth.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "height", TranslateUtils.ToInt(this.tbHeight.Text, 0) > 0 ? this.tbHeight.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "isAutoPlay", this.cbIsAutoPlay.Checked.ToString().ToLower());

                stlElement = StlParserUtility.GetStlElement(StlPlayer.ElementName, attributes, null);
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
