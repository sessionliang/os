using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundInputContent : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlHeadRowReply;

        public Button AddButton;
        public Button Check;
        public Button Delete;
        public Button ImportExcel;
        public Button ExportExcel;
        public Button TaxisButton;
        public Button SelectListButton;
        public Button SelectFormButton;
        public Button btnReturn;

        private ArrayList relatedIdentities;
        private InputInfo inputInfo = null;
        private ArrayList tableStyleInfoArrayList;
        private int itemID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "InputName");
            string theInputName = base.GetQueryString("InputName");
            this.itemID = TranslateUtils.ToInt(base.GetQueryString("ItemID"));

            this.inputInfo = DataProvider.InputDAO.GetInputInfo(theInputName, base.PublishmentSystemID);
            if (this.inputInfo == null) return;

            if (base.GetQueryString("ItemID") != null && base.GetQueryString("Delete") != null && base.GetQueryString("ContentIDCollection") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                try
                {
                    DataProvider.InputContentDAO.Delete(this.itemID, arraylist);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除提交表单内容", string.Format("提交表单:{0}", this.inputInfo.InputName));
                    base.SuccessMessage("删除成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "删除失败！");
                }
            }



            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, this.inputInfo.InputID);

            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, this.relatedIdentities);

            bool isAnythingVisible = false;
            foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
            {
                if (styleInfo.IsVisibleInList)
                {
                    isAnythingVisible = true;
                    break;
                }
            }
            if (!isAnythingVisible && this.tableStyleInfoArrayList != null && this.tableStyleInfoArrayList.Count > 0)
            {
                TableStyleInfo tableStyleInfo = (TableStyleInfo)this.tableStyleInfoArrayList[0];
                tableStyleInfo.IsVisibleInList = true;
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.InputContentDAO.GetSelectStringOfContentID(this.inputInfo.InputID, string.Empty);
            this.spContents.SortField = DataProvider.InputContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "提交表单内容管理", string.Format("{0}({1})", this.inputInfo.InputName, this.spContents.TotalCount), AppManager.CMS.Permission.WebSite.InputContentView);

                string showPopWinString = string.Empty;

                //将表单权限必为分类权限 
                //if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + this.inputInfo.InputName))
                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + this.inputInfo.ClassifyID))
                {
                    showPopWinString = Modal.InputContentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.inputInfo.InputID, this.PageUrl);
                    this.AddButton.Attributes.Add("onclick", showPopWinString);


                    //this.Delete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选内容，确定吗？\");");
                    this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_inputContent.aspx?PublishmentSystemID={0}&Delete=true&ItemID={1}&InputName={2}", base.PublishmentSystemID, this.itemID, inputInfo.InputName)), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的表单内容！", "此操作将删除所选内容，确定删除吗？"));

                    this.Check.Attributes.Add("onclick", "return confirm(\"此操作将把所选内容设为审核通过，确定吗？\");");


                    showPopWinString = Modal.InputContentTaxis.GetOpenWindowString(base.PublishmentSystemID, this.inputInfo.InputID, this.PageUrl);
                    this.TaxisButton.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.SelectColumns.GetOpenWindowStringToInputContent(base.PublishmentSystemID, this.inputInfo.InputID, true);
                    this.SelectListButton.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.SelectColumns.GetOpenWindowStringToInputContent(base.PublishmentSystemID, this.inputInfo.InputID, false);
                    this.SelectFormButton.Attributes.Add("onclick", showPopWinString);
                }
                else
                {
                    this.AddButton.Visible = this.Delete.Visible = this.Check.Visible = this.TaxisButton.Visible = this.SelectListButton.Visible = this.SelectFormButton.Visible = this.ImportExcel.Visible = this.ExportExcel.Visible = false;
                }

                showPopWinString = PageUtility.ModalSTL.InputContentImport_GetOpenWindowString(base.PublishmentSystemID, this.inputInfo.InputID);
                this.ImportExcel.Attributes.Add("onclick", showPopWinString);

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToInputContent(base.PublishmentSystemID, this.inputInfo.InputID);
                this.ExportExcel.Attributes.Add("onclick", showPopWinString);

                string urlReturn = PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&itemID={1}", base.PublishmentSystemID, this.itemID));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));


                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisibleInList)
                        {
                            this.ltlColumnHeadRows.Text += string.Format(@"<td class=""center"">{0}</td>", styleInfo.DisplayName);
                        }
                    }
                }

                if (this.inputInfo.IsReply)
                {
                    if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + this.inputInfo.InputName))
                    {
                        this.ltlHeadRowReply.Text = @"
<td class=""center"" style=""width:60px;"">是否回复</td>
<td class=""center"" style=""width:60px;"">&nbsp;</td>
";
                    }
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                InputContentInfo contentInfo = DataProvider.InputContentDAO.GetContentInfo(contentID);
                Literal columnItemRows = (Literal)e.Item.FindControl("ColumnItemRows");
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");
                Literal itemViewRow = (Literal)e.Item.FindControl("ItemViewRow");
                Literal itemRowReply = e.Item.FindControl("ItemRowReply") as Literal;
                Literal itemDateTime = e.Item.FindControl("ItemDateTime") as Literal;

                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisibleInList)
                        {
                            string value = contentInfo.Attributes.Get(styleInfo.AttributeName);

                            if (!string.IsNullOrEmpty(value))
                            {
                                value = InputTypeParser.GetContentByTableStyle(value, base.PublishmentSystemInfo, ETableStyle.InputContent, styleInfo);
                            }

                            if (contentInfo.IsChecked == false && string.IsNullOrEmpty(columnItemRows.Text))
                            {
                                columnItemRows.Text += string.Format(@"<td>{0}<span style=""color:red"">[未审核]</span></td>", value);
                            }
                            else
                            {
                                columnItemRows.Text += string.Format(@"<td>{0}</td>", value);
                            }
                        }
                    }
                }

                itemViewRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.InputContentView.GetOpenWindowString(base.PublishmentSystemID, this.inputInfo.InputID, contentInfo.ID));
                //将表单权限必为分类权限 
                //if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + this.inputInfo.InputName))
                //{ 
                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + this.inputInfo.ClassifyID))
                {
                    itemEidtRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.InputContentAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.inputInfo.InputID, contentInfo.ID, this.PageUrl));

                    if (this.inputInfo.IsReply)
                    {
                        string text = string.IsNullOrEmpty(contentInfo.Reply) ? "提交回复" : "修改回复";
                        itemRowReply.Text = string.Format(@"
<td class=""center"">{0}</a></td>
<td class=""center""><a href=""javascript:;"" onclick=""{1}"">{2}</a></td>", StringUtils.GetTrueImageHtml(!string.IsNullOrEmpty(contentInfo.Reply)), Modal.InputContentReply.GetOpenWindowString(base.PublishmentSystemID, this.inputInfo.InputID, contentInfo.ID), text);
                    }
                }

                itemDateTime.Text = DateUtils.GetDateString(contentInfo.AddDate);
            }
        }

        public void Delete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.InputContentDAO.Delete(this.inputInfo.InputID, arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除提交表单内容", string.Format("提交表单:{0}", this.inputInfo.InputName));
                        base.SuccessMessage("删除成功！");
                        PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_inputContent.aspx?PublishmentSystemID={0}&InputName={1}&ItemID={2}", base.PublishmentSystemID, this.inputInfo.InputName, this.inputInfo.ClassifyID)));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
                else
                    base.FailMessage("删除失败,请选择需要删除的内容！");
            }
        }

        public void Check_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.InputContentDAO.Check(this.inputInfo.InputID, arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "审核提交表单内容", string.Format("提交表单:{0}", this.inputInfo.InputName));
                        base.SuccessMessage("审核成功！");
                        PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_inputContent.aspx?PublishmentSystemID={0}&InputName={1}&ItemID={2}", base.PublishmentSystemID, this.inputInfo.InputName, this.inputInfo.ClassifyID)));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "审核失败！");
                    }
                }
                else
                    base.FailMessage("删除失败,请选择需要审核的内容！");
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_inputContent.aspx?PublishmentSystemID={0}&InputName={1}&ItemID={2}", base.PublishmentSystemID, this.inputInfo.InputName, this.inputInfo.ClassifyID));
                }
                return _pageUrl;
            }
        }
    }
}
