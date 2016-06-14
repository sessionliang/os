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
    public class InputContentTaxis : BackgroundBasePage
    {
        protected RadioButtonList TaxisType;
        protected TextBox TaxisNum;

        private int inputID;
        private string returnUrl;
        private ArrayList contentIDArrayList;

        public static string GetOpenWindowString(int publishmentSystemID,   int inputID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("表单内容排序", "modal_inputContentTaxis.aspx", arguments, "ContentIDCollection", "请选择需要排序的内容！", 300, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID",  "ReturnUrl");
            this.inputID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("InputID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));

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
                for (int i = 1; i <= taxisNum; i++)
                {
                    if (isUp)
                    {
                        if (DataProvider.InputContentDAO.UpdateTaxisToUp(this.inputID, contentID))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (DataProvider.InputContentDAO.UpdateTaxisToDown(this.inputID, contentID))
                        {
                            break;
                        }
                    }
                }
            }

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
