using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;
using BaiRong.Controls;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundInputMainContent : BackgroundBasePage
    {
        public DataGrid dgContents;
        public SqlPager spContents;
        public TextBox InputName;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Button AddInput;
        public Button Import;
        public PlaceHolder PhButton;

        private int itemID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.itemID = TranslateUtils.ToInt(base.GetQueryString("itemID"));

            InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(base.PublishmentSystemID);

            if (pinfo != null)//导入表单后需要重新统计分类的数量
                DataProvider.InputClassifyDAO.UpdateInputCountByClassifyID(base.PublishmentSystemID, this.itemID, pinfo.ItemID);
            if (this.itemID == pinfo.ItemID)
                PhButton.Visible = false;
            else
                PhButton.Visible = true;

            if (base.GetQueryString("InputID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int inputID = base.GetIntQueryString("InputID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.InputDAO.UpdateTaxisToDown(base.PublishmentSystemID, inputID, this.itemID);
                }
                else
                {
                    DataProvider.InputDAO.UpdateTaxisToUp(base.PublishmentSystemID, inputID, this.itemID);
                }

                StringUtility.AddLog(base.PublishmentSystemID, "提交表单排序" + (isDown ? "下降" : "上升"));

                PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&itemID={1}", base.PublishmentSystemID, this.itemID)));
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


                        //修改分类下表单的数量
                        DataProvider.InputClassifyDAO.UpdateInputCount(base.PublishmentSystemID, this.itemID, 0);

                        //修改全部分类下表单的数量
                        DataProvider.InputClassifyDAO.UpdateCountByAll(base.PublishmentSystemID, pinfo.ItemID);
                    }
                    base.SuccessMessage("删除成功！");
                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&itemID={1}", base.PublishmentSystemID, this.itemID)));
                    return;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "删除失败！");
                }
            }

            ArrayList itemList = new ArrayList();
            //获取当前登录用户所有的表单分类权限及截取表单分类ID,管理员用户
            if (!PermissionsManager.Current.IsConsoleAdministrator && !PermissionsManager.Current.IsSystemAdministrator)
            {
                itemList.Add(pinfo.ItemID);
                ArrayList websitePermissionArrayList = SiteServer.CMS.Core.Security.ProductPermissionsManager.Current.WebsitePermissionSortedList[base.PublishmentSystemID] as ArrayList;
                if (websitePermissionArrayList != null && websitePermissionArrayList.Count > 0)
                {
                    foreach (string permission in websitePermissionArrayList)
                    {
                        if (permission.StartsWith(AppManager.CMS.Permission.WebSite.InputClassifyView + "_"))
                        {
                            itemList.Add(permission.TrimStart((AppManager.CMS.Permission.WebSite.InputClassifyView + "_").ToCharArray()));
                        }
                    }
                }
            }

            this.spContents.ControlToPaginate = this.dgContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID).Additional.PageSize;

            if (string.IsNullOrEmpty(base.GetQueryString("itemID")))
            {
                //this.dgContents.DataSource = DataProvider.InputDAO.GetDataSource(base.PublishmentSystemID, this.itemID, this.InputName.Text, this.DateFrom.Text, this.DateTo.Text);
                this.dgContents.DataSource = DataProvider.InputDAO.GetDataSource(base.PublishmentSystemID, this.itemID, this.InputName.Text, this.DateFrom.Text, this.DateTo.Text, itemList);
            }
            else
            {
                //this.dgContents.DataSource = DataProvider.InputDAO.GetDataSource(base.PublishmentSystemID, this.itemID, base.GetQueryString("InputName"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
                this.dgContents.DataSource = DataProvider.InputDAO.GetDataSource(base.PublishmentSystemID, this.itemID, base.GetQueryString("InputName"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), itemList);
            }
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.spContents.SortField = "Taxis";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "提交表单管理", AppManager.CMS.Permission.WebSite.Input);

                this.dgContents.DataBind();

                if (!string.IsNullOrEmpty(base.GetQueryString("itemID")))
                {
                    this.InputName.Text = base.GetQueryString("InputName");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }
                //将表单权限必为分类权限增加 
                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + itemID))
                {
                    this.AddInput.Attributes.Add("onclick", Modal.InputAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.itemID));
                    this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_INPUT, this.itemID));

                }
                else
                {
                    this.AddInput.Visible = false;
                    this.Import.Visible = false;
                }
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
                string inputName = TranslateUtils.EvalString(e.Item.DataItem, "InputName");
                int itemID = TranslateUtils.EvalInt(e.Item.DataItem, "ClassifyID");

                Literal upLink = (Literal)e.Item.FindControl("UpLink");
                Literal downLink = (Literal)e.Item.FindControl("DownLink");
                Literal styleUrl = (Literal)e.Item.FindControl("StyleUrl");
                Literal templateUrl = (Literal)e.Item.FindControl("TemplateUrl");
                Literal mailSMSUrl = (Literal)e.Item.FindControl("MailSMSUrl");
                Literal previewUrl = (Literal)e.Item.FindControl("PreviewUrl");
                Literal editUrl = (Literal)e.Item.FindControl("EditUrl");
                Literal exportUrl = (Literal)e.Item.FindControl("ExportUrl");
                Literal deleteUrl = (Literal)e.Item.FindControl("DeleteUrl");
                //Literal contentUrl = (Literal)e.Item.FindControl("ContentUrl");


                //将表单权限必为分类权限增加 
                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + itemID))
                {
                    string urlUp = PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&InputID={1}&Up=True&itemID={2}", base.PublishmentSystemID, inputID, itemID));
                    upLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                    string urlDown = PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&InputID={1}&Down=True&itemID={2}", base.PublishmentSystemID, inputID, itemID));
                    downLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                    //styleUrl.Text = string.Format(@"<a href=""{0}"">表单字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, inputID));

                    styleUrl.Text = string.Format(@"<a href=""{0}"">表单字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.ClassifyInputContent, DataProvider.InputContentDAO.TableName, inputID, itemID));

                    templateUrl.Text = string.Format(@"<a href=""{0}"">自定义模板</a>", PageUtils.GetSTLUrl(string.Format("background_inputTemplate.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));

                    mailSMSUrl.Text = string.Format(@"<a href=""{0}"">邮件/短信发送</a>", PageUtils.GetCMSUrl(string.Format("background_inputMailSMS.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));


                    // editUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.InputAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, inputID, false));

                    editUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.InputAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, inputID, false, itemID));

                    previewUrl.Text = string.Format(@"<a href=""{0}"">预览</a>", PageUtils.GetSTLUrl(string.Format("background_inputPreview.aspx?PublishmentSystemID={0}&InputID={1}", base.PublishmentSystemID, inputID)));
                    // contentUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">内容</a>", PageUtils.GetCMSUrl(string.Format("background_inputContent.aspx?PublishmentSystemID={0}&InputID={1}&InputName={2}", base.PublishmentSystemID, inputID, inputName)));

                    exportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToInput(base.PublishmentSystemID, inputID));
                    deleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('此操作将删除提交表单“{1}”及相关数据，确认吗？');"">删除</a>", string.Format("background_inputMainContent.aspx?Delete=True&PublishmentSystemID={0}&itemID={1}&InputID={2}", base.PublishmentSystemID, this.itemID, inputID), inputName);
                }
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&itemID={1}&InputName={2}&DateFrom={3}&DateTo={4}", base.PublishmentSystemID, this.itemID, this.InputName.Text, this.DateFrom.Text, this.DateTo.Text));
                }
                return this._pageUrl;
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }
    }
}
