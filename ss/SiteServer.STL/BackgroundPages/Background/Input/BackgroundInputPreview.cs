using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundInputPreview : BackgroundBasePage
	{
        public Literal ltlInputName;
        public Literal ltlInputCode;
        public Literal ltlForm;

        private InputInfo inputInfo; 

        public string GetEditUrl()
        {
            return InputAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.inputInfo.InputID, true);
        }

        public int GetItemID()
        {
            return this.inputInfo.ClassifyID;
        }
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("InputID");

            int inputID = base.GetIntQueryString("InputID"); 
            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "预览提交表单", AppManager.CMS.Permission.WebSite.Input);

                this.ltlInputName.Text = this.inputInfo.InputName;

                string stlElement = string.Format(@"<stl:input inputName=""{0}""></stl:input>", this.inputInfo.InputName);

                this.ltlInputCode.Text = StringUtils.HtmlEncode(stlElement);

                this.ltlForm.Text = StlParserManager.ParsePreviewContent(base.PublishmentSystemInfo, stlElement);
                
                //if (string.IsNullOrEmpty(this.inputInfo.Template))
                //{
                //    InputTemplate inputTemplate = new InputTemplate(base.PublishmentSystemID, this.inputInfo);
                //    this.ltlForm.Text = inputTemplate.GetTemplate();
                //}
                //else
                //{
                //    this.ltlForm.Text = this.inputInfo.Template;
                //}
			}
		}
	}
}
