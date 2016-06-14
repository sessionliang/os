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
using SiteServer.CMS.Controls;


namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleOrganization : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public DropDownList ddlAreaID;
        public TextBox Keyword;
        public PlaceHolder PlaceHolder_AddChannel;

        public Button btnAdd;
        public Button btnTranslate;
        public Button btnDelete;
        public Button btnTaxis;

        private int itemID = 0;
        private OrganizationClassifyInfo pinfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            PageUtils.CheckRequestParameter("ItemID");
            if (!string.IsNullOrEmpty(base.GetQueryString("ItemID")))
            {
                this.itemID = base.GetIntQueryString("ItemID");
            }
            this.PlaceHolder_AddChannel.Visible = true;
            OrganizationClassifyInfo info = DataProvider.OrganizationClassifyDAO.GetInfo(this.itemID);
            pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
            if (this.itemID != pinfo.ItemID)
                this.PlaceHolder_AddChannel.Visible = true;
            else
                this.PlaceHolder_AddChannel.Visible = false;

            if (!IsPostBack)
            {
                OrganizationClassifyInfo selInfo = DataProvider.OrganizationClassifyDAO.GetInfo(this.itemID);
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "分支机构列表 / " + selInfo.ItemName, AppManager.Platform.Permission.Platform_Organization);

                if (base.GetQueryString("ItemID") != null && base.GetQueryString("Delete") != null && base.GetQueryString("ID") != null)
                {
                    int ID = base.GetIntQueryString("ID");
                    DataProvider.OrganizationInfoDAO.Delete(ID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除分支机构");
                    base.SuccessMessage("删除成功！");
                    //统计全部分类下的机构数量
                    pinfo.ContentNum = DataProvider.OrganizationInfoDAO.GetCount();
                    DataProvider.OrganizationClassifyDAO.Update(pinfo);
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("ItemID")))
                {
                    this.ddlAreaID.Items.Clear();
                    ListItem item = new ListItem("无", "");
                    this.ddlAreaID.Items.Add(item);
                    TreeManager.AddListItemsByClassify(this.ddlAreaID.Items, base.PublishmentSystemID, this.itemID, true, true, "OrganizationArea", this.itemID, 0);
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.ddlAreaID.SelectedValue = base.GetQueryString("AreaID");
                }

                this.spContents.ControlToPaginate = this.rptContents;
                //this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
                this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                if (string.IsNullOrEmpty(base.GetQueryString("ItemID")))
                {
                    this.spContents.SelectCommand = DataProvider.OrganizationInfoDAO.GetSelectCommend(base.PublishmentSystemID, this.itemID, this.ddlAreaID.SelectedValue, this.Keyword.Text);
                }
                else
                {
                    this.spContents.SelectCommand = DataProvider.OrganizationInfoDAO.GetSelectCommend(base.PublishmentSystemID, base.GetIntQueryString("ItemID"), base.GetQueryString("AreaID"), base.GetQueryString("Keyword"));
                }
                this.spContents.SortField = DataProvider.OrganizationInfoDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.spContents.DataBind();



                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", ConsoleOrganizationAdd.GetRedirectUrl(base.PublishmentSystemID, this.itemID, 0, PageUtils.GetCMSUrl(string.Format("console_organization.aspx?PublishmentSystemID={0}&ItemID={1}", base.PublishmentSystemID, this.itemID)))));

                this.btnDelete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选内容，确定吗？\");");

                string showPopWinString = Modal.OrganizationTaxis.GetOpenWindowString(base.PublishmentSystemID, this.itemID, PageUtils.GetCMSUrl(string.Format("console_organization.aspx?PublishmentSystemID={0}&ItemID={1}", base.PublishmentSystemID, this.itemID)));
                this.btnTaxis.Attributes.Add("onclick", showPopWinString);

                //转移
                showPopWinString = Modal.OrganizationTranslate.GetRedirectString(base.PublishmentSystemID, this.itemID, PageUtils.GetCMSUrl(string.Format("console_organization.aspx?PublishmentSystemID={0}&ItemID={1}", base.PublishmentSystemID, this.itemID)));
                this.btnTranslate.Attributes.Add("onclick", showPopWinString);

            }


        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int ID = TranslateUtils.EvalInt(e.Item.DataItem, OrganizationInfoAttribute.ID);
                string name = TranslateUtils.EvalString(e.Item.DataItem, OrganizationInfoAttribute.OrganizationName);
                int classifyID = TranslateUtils.EvalInt(e.Item.DataItem, OrganizationInfoAttribute.ClassifyID);
                int areaID = TranslateUtils.EvalInt(e.Item.DataItem, OrganizationInfoAttribute.AreaID);
                string address = TranslateUtils.EvalString(e.Item.DataItem, OrganizationInfoAttribute.OrganizationAddress);
                string phone = TranslateUtils.EvalString(e.Item.DataItem, OrganizationInfoAttribute.Phone);


                Literal itemName = (Literal)e.Item.FindControl("ItemName");
                Literal itemAddress = (Literal)e.Item.FindControl("ItemAddress");
                Literal itemPhone = (Literal)e.Item.FindControl("ItemPhone");
                Literal itemArea = (Literal)e.Item.FindControl("ItemArea");
                Literal itemClassify = (Literal)e.Item.FindControl("ItemClassify");
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");
                Literal itemDelRow = (Literal)e.Item.FindControl("ItemDelRow");

                OrganizationClassifyInfo ocInfo = DataProvider.OrganizationClassifyDAO.GetInfo(classifyID);
                string areaName = DataProvider.OrganizationAreaDAO.getArea(base.PublishmentSystemID, areaID);


                itemDelRow.Text = string.Format(@"<a href=""console_organization.aspx?Delete=True&PublishmentSystemID={0}&ItemID={2}&ID={3}"" onClick=""javascript:return confirm('此操作将删除机构“{1}”及相关数据，确认吗？');"">删除</a>", base.PublishmentSystemID, name, this.itemID, ID);

                itemEidtRow.Text = string.Format(@"<a href=""{0}"" >修改</a>", ConsoleOrganizationAdd.GetRedirectUrl(base.PublishmentSystemID, this.itemID, ID, string.Format(@"console_organization.aspx?PublishmentSystemID={0}&ItemID={1}", base.PublishmentSystemID, this.itemID)));

                itemName.Text = name;
                itemAddress.Text = address;
                itemPhone.Text = phone;
                itemArea.Text = areaName;
                itemClassify.Text = ocInfo.ItemName;

            }
        }

        public void btnDelete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.OrganizationInfoDAO.Delete(arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除分支机构");
                        base.SuccessMessage("删除成功！");

                        //统计全部分类下的机构数量
                        pinfo.ContentNum = DataProvider.OrganizationInfoDAO.GetCount();
                        DataProvider.OrganizationClassifyDAO.Update(pinfo);
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
                else
                    base.FailMessage("请选择需要删除的机构！");
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("console_organization.aspx?PublishmentSystemID={0}&ItemID={1}&AreaID={2}&Keyword={3}", base.PublishmentSystemID, this.itemID, this.ddlAreaID.SelectedValue, this.Keyword.Text));
                }
                return _pageUrl;
            }
        }

    }
}
