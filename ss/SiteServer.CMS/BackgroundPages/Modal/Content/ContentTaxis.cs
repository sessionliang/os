using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ContentTaxis : BackgroundBasePage
    {
        protected RadioButtonList TaxisType;
        protected TextBox TaxisNum;

        private int nodeID;
        private string returnUrl;
        private ArrayList contentIDArrayList;
        private ETableStyle tableStyle;
        private string tableName;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("内容排序", "modal_contentTaxis.aspx", arguments, "ContentIDCollection", "请选择需要排序的内容！", 300, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl", "ContentIDCollection");

            this.nodeID = base.GetIntQueryString("NodeID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);

            if (!IsPostBack)
            {
                this.TaxisType.Items.Add(new ListItem("上升", "Up"));
                this.TaxisType.Items.Add(new ListItem("下降", "Down"));
                ControlUtils.SelectListItems(this.TaxisType, "Up");

                
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isUp = (this.TaxisType.SelectedValue == "Up");
            int taxisNum = int.Parse(this.TaxisNum.Text);

            if (isUp == false)
            {
                this.contentIDArrayList.Reverse();
            }

            foreach (int contentID in this.contentIDArrayList)
            {
                bool isTop = TranslateUtils.ToBool(BaiRongDataProvider.ContentDAO.GetValue(this.tableName, contentID, BaiRong.Model.ContentAttribute.IsTop));
                for (int i = 1; i <= taxisNum; i++)
                {
                    if (isUp)
                    {
                        if (BaiRongDataProvider.ContentDAO.UpdateTaxisToUp(this.tableName, this.nodeID, contentID, isTop) == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (BaiRongDataProvider.ContentDAO.UpdateTaxisToDown(this.tableName, this.nodeID, contentID, isTop) == false)
                        {
                            break;
                        }
                    }
                }
            }

            foreach (int contentID in this.contentIDArrayList)
            {
                string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Taxis, ETemplateType.ContentTemplate, this.nodeID, contentID, 0);
                AjaxUrlManager.AddAjaxUrl(ajaxUrl);
            }

            StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "对内容排序", string.Empty);

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
