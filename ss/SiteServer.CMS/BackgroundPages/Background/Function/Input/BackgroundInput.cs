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
	public class BackgroundInput : BackgroundBasePage
	{
		public DataGrid dgContents;

        public Button AddInput;
        public Button Import;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("InputID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int inputID = base.GetIntQueryString("InputID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.InputDAO.UpdateTaxisToDown(base.PublishmentSystemID, inputID);
                }
                else
                {
                    DataProvider.InputDAO.UpdateTaxisToUp(base.PublishmentSystemID, inputID);
                }

                StringUtility.AddLog(base.PublishmentSystemID, "提交表单排序" + (isDown ? "下降" : "上升"));

                PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_input.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                return;
            }
            else if (base.GetQueryString("Delete") != null)
            {
                int inputID = TranslateUtils.ToInt(base.GetQueryString("InputID"));
                try
                {
                    InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
                    if (inputInfo != null)
                    {
                        DataProvider.InputDAO.Delete(inputID);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除提交表单", string.Format("提交表单:{0}", inputInfo.InputName));
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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "提交表单管理", AppManager.CMS.Permission.WebSite.Input);

                this.dgContents.DataSource = DataProvider.InputDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddInput.Attributes.Add("onclick", Modal.InputAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
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
                int inputID = TranslateUtils.EvalInt(e.Item.DataItem, "InputID");

                Literal upLink = (Literal)e.Item.FindControl("UpLink");
                Literal downLink = (Literal)e.Item.FindControl("DownLink");
                Literal styleUrl = (Literal)e.Item.FindControl("StyleUrl");
                Literal templateUrl = (Literal)e.Item.FindControl("TemplateUrl");
                Literal mailSMSUrl = (Literal)e.Item.FindControl("MailSMSUrl");
                Literal previewUrl = (Literal)e.Item.FindControl("PreviewUrl");
                Literal editUrl = (Literal)e.Item.FindControl("EditUrl");
                Literal exportUrl = (Literal)e.Item.FindControl("ExportUrl");

                string urlUp = PageUtils.GetCMSUrl(string.Format("background_input.aspx?PublishmentSystemID={0}&InputID={1}&Up=True", base.PublishmentSystemID, inputID));
                upLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = PageUtils.GetCMSUrl(string.Format("background_input.aspx?PublishmentSystemID={0}&InputID={1}&Down=True", base.PublishmentSystemID, inputID));
                downLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                styleUrl.Text = string.Format(@"<a href=""{0}"">表单字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, inputID));

                templateUrl.Text = string.Format(@"<a href=""{0}"">自定义模板</a>", PageUtils.GetSTLUrl(string.Format("background_inputTemplate.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));

                mailSMSUrl.Text = string.Format(@"<a href=""{0}"">邮件/短信发送</a>", PageUtils.GetCMSUrl(string.Format("background_inputMailSMS.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));

                previewUrl.Text = string.Format(@"<a href=""{0}"">预览</a>", PageUtils.GetSTLUrl(string.Format("background_inputPreview.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));

                editUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.InputAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, inputID, false));

                exportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToInput(base.PublishmentSystemID, inputID));
            }
        }
	}
}
