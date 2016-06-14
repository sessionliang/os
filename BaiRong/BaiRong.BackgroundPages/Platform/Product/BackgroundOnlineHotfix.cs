using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Controls;

using BaiRong.Core.Data.Provider;

using BaiRong.Model;
using BaiRong.Core.Net;
using System.Xml;
using BaiRong.Core.IO;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
	public class BackgroundOnlineHotfix : BackgroundBasePage
	{
        public Literal ltlHotfix;
        public Literal ltlVersion;
        public Literal ltlUpdateDate;

        public Button btnImport;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Product, "产品在线升级", AppManager.Platform.Permission.Platform_Product);

                this.ltlHotfix.Text = StringUtils.Constants.GetHotfixHTML(true);

                this.ltlVersion.Text = ProductManager.GetFullVersion();
                this.ltlUpdateDate.Text = DateUtils.GetDateAndTimeString(ConfigManager.Instance.UpdateDate);

                //this.btnHotfix.Attributes.Add("onclick", Modal.ProgressBar.GetOpenWindowStringWithHotfix(fileName));
                this.btnImport.Attributes.Add("onclick", Modal.HotfixUpload.GetOpenWindowString());
			}
		}

        //public void Content_OnClick(object sender, EventArgs E)
        //{
        //    if (base.Page.IsPostBack && base.Page.IsValid)
        //    {
        //        try
        //        {
        //            double localVersion = ProductManager.GetVersionDouble(ProductManager.Version);

        //            string content = WebClientUtils.GetRemoteFileSource(PageUtils.Combine(StringUtils.Constants.Url_Hotfix, ProductManager.Version, "service."), ECharset.utf_8);

        //            XmlDocument document = XmlUtils.GetXmlDocument(content);
        //            string name = XmlUtils.GetXmlNodeInnerText(document, "//hotfix/name");
        //            double onlineVersion = ProductManager.GetVersionDouble(XmlUtils.GetXmlNodeInnerText(document, "//hotfix/version"));
        //            string date = XmlUtils.GetXmlNodeInnerText(document, "//hotfix/date");
        //            string summary = XmlUtils.GetXmlNodeInnerText(document, "//hotfix/summary");
        //            if (onlineVersion > localVersion)
        //            {
        //                this.phContent.Visible = false;
        //                this.phHotfix.Visible = true;

        //                this.ltlName.Text = name;
        //                this.ltlDate.Text = date;
        //                this.ltlSummary.Text = summary;

        //                base.SuccessMessage(@"系统检测到存在新补丁可更新，在进行补丁修复之前，<font style=""color:red"">建议手动备份您的数据库和程序文件</font>。");

        //                string fileName = this.ltlName.Text + ".zip";
                        
        //            }
        //            else
        //            {
        //                base.SuccessMessage("暂无新补丁可更新！");
        //                this.btnContent.Enabled = false;
        //            }
        //        }
        //        catch
        //        {
        //            base.FailMessage("操作失败：无法访问更新网址");
        //        }
        //    }
        //}
	}
}
