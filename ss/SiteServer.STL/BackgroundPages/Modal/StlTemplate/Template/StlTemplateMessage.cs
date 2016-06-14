using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Office;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlTemplateMessage : BackgroundBasePage
	{
        public const int Width = 380;
        public const int Height = 250;

        public const string MESSAGE_DeleteStlElement = "DeleteStlElement";
        
        private string messageType;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToDeleteStlElement(int publishmentSystemID, int templateID, string includeUrl, int elementIndex)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("elementIndex", elementIndex.ToString());
            arguments.Add("messageType", "DeleteStlElement");
            return JsUtils.Layer.GetOpenLayerString("删除标签", PageUtils.GetSTLUrl("modal_stlTemplateMessage.aspx"), arguments, 380, 250);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.messageType = base.GetQueryString("messageType");

            if (!IsPostBack)
            {
                if (this.messageType == StlTemplateMessage.MESSAGE_DeleteStlElement)
                {
                    base.InfoMessage("本操作将删除对应标签，确定吗？");
                }
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                if (this.messageType == StlTemplateMessage.MESSAGE_DeleteStlElement)
                {
                    int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
                    string includeUrl = base.GetQueryString("includeUrl");
                    TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
                    int elementIndex = TranslateUtils.ToInt(base.GetQueryString("elementIndex"));

                    TemplateDesignManager.UpdateStlElement(base.PublishmentSystemInfo, templateInfo, includeUrl, elementIndex, string.Empty);

                    StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}", ETemplateTypeUtils.GetText(templateInfo.TemplateType)), string.Format("模板名称:{0}", templateInfo.TemplateName));

                    isSuccess = true;
                }
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
