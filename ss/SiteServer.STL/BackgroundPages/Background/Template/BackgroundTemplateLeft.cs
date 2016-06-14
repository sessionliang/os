using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.Services;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundTemplateLeft : BackgroundBasePage
    {
        public Literal ltlReplaceIndexPageTemplate;
        public Literal ltlReplaceChannelTemplate;
        public Literal ltlReplaceContentTemplate;
        public Literal ltlReplaceUserCenterTemplate;

        private Dictionary<ETemplateType, int> dictionary = null;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.dictionary = DataProvider.TemplateDAO.GetCountDictionary(base.PublishmentSystemID);

            this.ltlReplaceIndexPageTemplate.Text = string.Format(@"<a class=""btn btn-success"" href=""{0}"" target=""_parent"">更换首页模板</a>", BackgroundTemplateImport.GetRedirectUrl(base.PublishmentSystemID, ETemplateType.IndexPageTemplate));
            this.ltlReplaceChannelTemplate.Text = string.Format(@"<a class=""btn btn-success"" href=""{0}"" target=""_parent"">更换栏目模板</a>", BackgroundTemplateImport.GetRedirectUrl(base.PublishmentSystemID, ETemplateType.ChannelTemplate));
            this.ltlReplaceContentTemplate.Text = string.Format(@"<a class=""btn btn-success"" href=""{0}"" target=""_parent"">更换内容模板</a>", BackgroundTemplateImport.GetRedirectUrl(base.PublishmentSystemID, ETemplateType.ContentTemplate));
            this.ltlReplaceUserCenterTemplate.Text = string.Format(@"<a class=""btn btn-success"" href=""{0}"" target=""_parent"">更换整套模板</a>", ConsoleUserCenterReplace.GetRedirectUrl(base.PublishmentSystemID));

            if (EPublishmentSystemTypeUtils.IsUserCenter(base.PublishmentSystemInfo.PublishmentSystemType))
            {
                this.ltlReplaceIndexPageTemplate.Visible = false;
                this.ltlReplaceChannelTemplate.Visible = false;
                this.ltlReplaceContentTemplate.Visible = false;
            }
            else
            {
                this.ltlReplaceUserCenterTemplate.Visible = false;
            }
        }

        public string GetServiceUrl()
        {
            return BackgroundServiceSTL.GetRedirectUrl(BackgroundServiceSTL.TYPE_GetLoadingTemplates);
        }

        public int GetCount(string templateType)
        {
            int count = 0;
            if (string.IsNullOrEmpty(templateType))
            {
                foreach (var value in this.dictionary)
                {
                    count += value.Value;
                }
            }
            else
            {
                ETemplateType eTemplateType = ETemplateTypeUtils.GetEnumType(templateType);
                if (this.dictionary.ContainsKey(eTemplateType))
                {
                    count = this.dictionary[eTemplateType];
                }
            }
            return count;
        }
    }
}
