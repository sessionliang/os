using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundWebsiteMessage : BackgroundBasePage
	{
		public DataGrid dgContents;

        public Button AddWebsiteMessage;
        public Button Import;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("WebsiteMessageID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int websiteMessageID = base.GetIntQueryString("WebsiteMessageID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.WebsiteMessageDAO.UpdateTaxisToDown(base.PublishmentSystemID, websiteMessageID);
                }
                else
                {
                    DataProvider.WebsiteMessageDAO.UpdateTaxisToUp(base.PublishmentSystemID, websiteMessageID);
                }

                StringUtility.AddLog(base.PublishmentSystemID, "留言排序" + (isDown ? "下降" : "上升"));

                PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                return;
            }
            else if (base.GetQueryString("Delete") != null)
            {
                int websiteMessageID = TranslateUtils.ToInt(base.GetQueryString("WebsiteMessageID"));
                try
                {
                    WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
                    if (websiteMessageInfo != null)
                    {
                        DataProvider.WebsiteMessageDAO.Delete(websiteMessageID);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除留言", string.Format("留言:{0}", websiteMessageInfo.WebsiteMessageName));
                    }

                    base.SuccessMessage("删除成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "删除失败！");
                }
            }

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "留言管理", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(base.PublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(base.PublishmentSystemID);
                #endregion

                this.dgContents.DataSource = DataProvider.WebsiteMessageDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddWebsiteMessage.Attributes.Add("onclick", Modal.WebsiteMessageAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
                this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_INPUT));

                if (base.GetQueryString("RefreshLeft") != null || base.GetQueryString("Delete") != null)
                {
                    base.Page.RegisterStartupScript("RefreshLeft", string.Format(@"
<script language=""javascript"">
top.frames[""left""].location.reload( false );
</script>
"));
                }
			}
		}

        public string GetIsCheckedHtml(string isCheckedString)
        {
            bool val = !TranslateUtils.ToBool(isCheckedString);
            return StringUtils.GetTrueImageHtml(val.ToString());
        }

        public string GetIsCodeValidateHtml(string isCodeValidateString)
        {
            return StringUtils.GetTrueImageHtml(isCodeValidateString);
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int websiteMessageID = TranslateUtils.EvalInt(e.Item.DataItem, "WebsiteMessageID");

                Literal upLink = (Literal)e.Item.FindControl("UpLink");
                Literal downLink = (Literal)e.Item.FindControl("DownLink");
                Literal styleUrl = (Literal)e.Item.FindControl("StyleUrl");
                Literal templateUrl = (Literal)e.Item.FindControl("TemplateUrl");
                Literal mailSMSUrl = (Literal)e.Item.FindControl("MailSMSUrl");
                Literal previewUrl = (Literal)e.Item.FindControl("PreviewUrl");
                Literal editUrl = (Literal)e.Item.FindControl("EditUrl");
                Literal exportUrl = (Literal)e.Item.FindControl("ExportUrl");

                string urlUp = PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}&Up=True", base.PublishmentSystemID, websiteMessageID));
                upLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}&Down=True", base.PublishmentSystemID, websiteMessageID));
                downLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                styleUrl.Text = string.Format(@"<a href=""{0}"">表单字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, websiteMessageID));

                templateUrl.Text = string.Format(@"<a href=""{0}"">自定义模板</a>", PageUtils.GetSTLUrl(string.Format("background_websiteMessageTemplate.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}", base.PublishmentSystemID, websiteMessageID)));

                mailSMSUrl.Text = string.Format(@"<a href=""{0}"">邮件/短信发送</a>", PageUtils.GetCMSUrl(string.Format("background_websiteMessageMailSMS.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}", base.PublishmentSystemID, websiteMessageID)));

                previewUrl.Text = string.Format(@"<a href=""{0}"">预览</a>", PageUtils.GetSTLUrl(string.Format("background_websiteMessagePreview.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}", base.PublishmentSystemID, websiteMessageID)));

                editUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.WebsiteMessageAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, websiteMessageID, false));

                exportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToWebsiteMessage(base.PublishmentSystemID, websiteMessageID));
            }
        }
	}
}
