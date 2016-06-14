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
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlTemplateEdit : BackgroundBasePage
    {
        public Button btnEditorType;
        public TextBox tbContent;
        public PlaceHolder phCodeMirror;

        private TemplateInfo templateInfo;
        private string includeUrl;
        private int stlSequence;
        private bool isStlElement;
        private bool isStlInsert;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToEdit(int publishmentSystemID, int templateID, string includeUrl, int stlSequence)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("stlSequence", stlSequence.ToString());
            arguments.Add("isStlElement", true.ToString());
            arguments.Add("isStlInsert", false.ToString());
            return JsUtils.Layer.GetOpenLayerString("修改代码", PageUtils.GetSTLUrl("modal_stlTemplateEdit.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlElement", true.ToString());
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("新增标签", PageUtils.GetSTLUrl("modal_stlTemplateEdit.aspx"), arguments);
        }

        public static string GetOpenLayerStringToTemplate(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlElement", false.ToString());
            arguments.Add("isStlInsert", false.ToString());
            return JsUtils.Layer.GetOpenLayerString("编辑模板", PageUtils.GetSTLUrl("modal_stlTemplateEdit.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.includeUrl = base.GetQueryString("includeUrl");
            this.stlSequence = TranslateUtils.ToInt(base.GetQueryString("stlSequence"));
            this.isStlElement = TranslateUtils.ToBool(base.GetQueryString("isStlElement"));
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            if (!IsPostBack)
            {
                bool isCodeMirror = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror);
                this.btnEditorType.Text = isCodeMirror ? "采用纯文本编辑模式" : "采用代码编辑模式";
                this.phCodeMirror.Visible = isCodeMirror;

                if (this.isStlElement)
                {
                    if (!isStlInsert)
                    {
                        this.tbContent.Text = TemplateDesignManager.GetStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, this.stlSequence);
                    }
                    else
                    {
                        string stlElementToAdd = base.GetQueryString("stlElementToAdd");
                        stlElementToAdd = stlElementToAdd.Replace("__R_A_N__", StringUtils.Constants.ReturnAndNewline);
                        this.tbContent.Text = stlElementToAdd.Trim();
                    }
                }
                else
                {
                    this.tbContent.Text = TemplateDesignUndoRedo.GetTemplateContent(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl);
                }
            }
        }

        public void EditorType_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isCodeMirror = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror);
                isCodeMirror = !isCodeMirror;
                base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror = isCodeMirror.ToString();
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                this.btnEditorType.Text = isCodeMirror ? "采用纯文本编辑模式" : "采用代码编辑模式";
                this.phCodeMirror.Visible = isCodeMirror;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                if (this.isStlElement)
                {
                    if (this.isStlInsert)
                    {
                        bool stlIsContainer = TranslateUtils.ToBool(base.GetQueryString("stlIsContainer"));
                        string stlSequenceCollection = base.GetQueryString("stlSequenceCollection");
                        int stlDivIndex = TranslateUtils.ToInt(base.GetQueryString("stlDivIndex"));

                        TemplateDesignManager.InsertStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, stlSequenceCollection, base.Request.Form["tbContent"], stlDivIndex, stlIsContainer);
                    }
                    else
                    {
                        TemplateDesignManager.UpdateStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, this.stlSequence, base.Request.Form["tbContent"]);
                    }
                }
                else
                {
                    TemplateDesignManager.UpdateTemplateInfo(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, base.Request.Form["tbContent"]);
                }

                StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}", ETemplateTypeUtils.GetText(this.templateInfo.TemplateType)), string.Format("模板名称:{0}", this.templateInfo.TemplateName));

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
