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
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSubscribe : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlHeadRowReply;

        public Button AddButton;
        public Button Delete;
        public Button btnTrue;
        public Button btnFalse;

        private int index = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.index = 0;
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("ItemID") != null && base.GetQueryString("Delete") != null)
            {
                int subscribeID = base.GetIntQueryString("ItemID");
                DataProvider.SubscribeDAO.Delete(base.PublishmentSystemID, subscribeID);
                StringUtility.AddLog(base.PublishmentSystemID, "删除订阅内容");
                base.SuccessMessage("删除成功！");

            }
            if (base.GetQueryString("ContentIDCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                try
                {
                    DataProvider.SubscribeDAO.Delete(base.PublishmentSystemID, arraylist);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除订阅内容");
                    base.SuccessMessage("删除成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "删除失败！");
                }
            }

            if (base.GetQueryString("ContentIDCollection") != null && base.GetQueryString("Enabled") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                EBoolean type = EBooleanUtils.GetEnumType(base.GetQueryString("Enabled"));
                try
                {
                    DataProvider.SubscribeDAO.UpdateEnabled(base.PublishmentSystemID, arraylist, type);
                    StringUtility.AddLog(base.PublishmentSystemID, (type == EBoolean.True ? "恢复" : "暂停") + "订阅内容状态");
                    base.SuccessMessage((type == EBoolean.True ? "恢复" : "暂停") + "订阅内容成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, (type == EBoolean.True ? "恢复" : "暂停") + "订阅内容失败！");
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.SubscribeDAO.GetAllString(base.PublishmentSystemID, " and ParentID != 0 ");//string.Empty
            this.spContents.SortField = DataProvider.SubscribeDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "订阅内容设置", AppManager.CMS.Permission.WebSite.Subscribe);


                #region 默认创建一个全部内容，订阅内容
                DataProvider.SubscribeDAO.SetDefaultInfo(base.PublishmentSystemID);
                #endregion

                string showPopWinString = Modal.SubscribeAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID);
                this.AddButton.Attributes.Add("onclick", showPopWinString);

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_subscribe.aspx?PublishmentSystemID={0}&Delete=true", base.PublishmentSystemID
                    )), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的内容！", "还有会员在订阅所选内容，如果删除则这批人将无法订阅此内容，确定要删除吗？"));

                this.btnTrue.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_subscribe.aspx?PublishmentSystemID={0}&Enabled=true", base.PublishmentSystemID
                    )), "ContentIDCollection", "ContentIDCollection", "请选择需要恢复推送的内容！", "确定要恢复所选内容的状态吗？"));
                this.btnFalse.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_subscribe.aspx?PublishmentSystemID={0}&Enabled=false", base.PublishmentSystemID
                    )), "ContentIDCollection", "ContentIDCollection", "请选择需要暂停推送的内容！", "还有会员在订阅所选内容，如果暂停则这批人将无法收到此内容且将不能订阅，确定要暂停所选内容的状态吗？"));

            }
            if (base.GetQueryString("getlist") == "1")
            {
                this.Response.Clear();
                this.Response.Write(getSubscribeList());
                this.Response.End();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int subscribeID = TranslateUtils.EvalInt(e.Item.DataItem, "ItemID");
                string subscribeName = TranslateUtils.EvalString(e.Item.DataItem, "ItemName");
                string contentType = TranslateUtils.EvalString(e.Item.DataItem, "ContentType");
                int subscribeNum = TranslateUtils.EvalInt(e.Item.DataItem, "SubscribeNum");
                string enabled = TranslateUtils.EvalString(e.Item.DataItem, "Enabled");


                Literal itemNum = (Literal)e.Item.FindControl("ItemNum");
                Literal itemSubscribeName = (Literal)e.Item.FindControl("ItemSubscribeName");
                Literal itemSubscribeNum = (Literal)e.Item.FindControl("ItemSubscribeNum");
                Literal itemContentType = (Literal)e.Item.FindControl("ItemContentType");
                Literal itemEnabled = (Literal)e.Item.FindControl("ItemEnabled");
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");
                Literal itemDelRow = (Literal)e.Item.FindControl("ItemDelRow");

                //此操作将删除订阅内容“{1}”及取消会员订阅下的当前内容，目前还有{3}人订阅中，确认删除吗？
                itemDelRow.Text = string.Format(@"<a href=""background_subscribe.aspx?Delete=True&PublishmentSystemID={0}&ItemID={2}"" onClick=""javascript:return confirm('还有 {3} 位会员在订阅内容“{1}”，如果删除则这批人将无法订阅此内容，确定要删除吗？');"">删除</a>", base.PublishmentSystemID, subscribeName, subscribeID, subscribeNum);

                itemEidtRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.SubscribeAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, subscribeID));

                itemNum.Text = (++index).ToString();
                itemSubscribeName.Text = subscribeName;
                itemContentType.Text = ESubscribeContentTypeUtils.GetText(ESubscribeContentTypeUtils.GetEnumType(contentType));
                itemSubscribeNum.Text = subscribeNum.ToString();
                itemEnabled.Text = EBooleanUtils.Equals(EBoolean.True, enabled) ? "正常" : "暂停";

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
                        DataProvider.SubscribeDAO.Delete(base.PublishmentSystemID, arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除订阅内容");
                        base.SuccessMessage("删除成功！");
                        PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_subscribe.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
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
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_subscribe.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }


        public string getSubscribeList()
        {

            StringBuilder sbList = new StringBuilder();

            ArrayList alist = DataProvider.SubscribeDAO.GetInfoList(base.PublishmentSystemID, " and ParentID != 0 ");
            if (alist.Count > 0)
            {
                sbList.Append("<div>");
                foreach (SubscribeInfo info in alist)
                {
                    string input = "<input id=\"" + info.ItemID + "\" type=\"checkbox\" name=\"IDsSub\" value=\"" + info.ItemID + "\">" + info.ItemName + " ";
                    sbList.Append(input);
                }
                sbList.Append("</div>");
            }


            return sbList.ToString();

        }

    }
}
