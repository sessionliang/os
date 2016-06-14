using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;

using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundTemplateEdit : BackgroundBasePage
	{
        public Literal ltlFilePath;
        public TextBox Content;

        private string templateDir;
        private string directoryName;
        private string fileName;

        public static string GetRedirectUrl(int publishmentSystemID, string templateDir, string directoryName, string fileName)
        {
            return PageUtils.GetBBSUrl(string.Format("background_templateEdit.aspx?publishmentSystemID={0}&templateDir={1}&directoryName={2}&fileName={3}", publishmentSystemID, templateDir, directoryName, fileName));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.templateDir = base.GetQueryString("templateDir");
            this.directoryName = base.GetQueryString("directoryName");
            this.fileName = base.GetQueryString("fileName");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "修改模板文件", AppManager.BBS.Permission.BBS_Template);

                this.ltlFilePath.Text = PathUtils.Combine(directoryName, fileName);
                string filePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(base.PublishmentSystemID, templateDir), directoryName, fileName);
                this.Content.Text = FileUtils.ReadText(filePath, ECharset.utf_8);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                try
                {
                    FileUtils.WriteText(PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(base.PublishmentSystemID, this.templateDir), this.directoryName, this.fileName), ECharset.utf_8, this.Content.Text);

                    base.SuccessMessage("模板修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "模板修改失败," + ex.Message);
                }
            }
		}
	}
}
