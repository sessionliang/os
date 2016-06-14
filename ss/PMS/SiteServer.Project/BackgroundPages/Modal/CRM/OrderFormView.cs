using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Cryptography;

using System.Collections.Generic;

namespace SiteServer.Project.BackgroundPages.Modal
{
    public class OrderFormView : BackgroundBasePage
    {
        private OrderFormInfo formInfo;
        public static string GetShowPopWinString(int orderFormID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderFormID", orderFormID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("表单查看", "modal_orderFormView.aspx", arguments, true);
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == OrderAttribute.MobanID)
            {
                MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(this.formInfo.MobanID);
                retval = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", DataProvider.MobanDAO.GetMobanUrl(mobanInfo));
            }
            else if (attributeName == OrderAttribute.AddDate)
            {
                retval = DateUtils.GetDateString(this.formInfo.AddDate, EDateFormatType.Chinese);
            }
            else if (attributeName == "List")
            {

            }
            
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int orderFormID = TranslateUtils.ToInt(base.Request.QueryString["orderFormID"]);
            this.formInfo = DataProvider.OrderFormDAO.GetOrderFormInfo(orderFormID);

            StringBuilder builder = new StringBuilder();

            Dictionary<int, string> pages = DataProvider.FormPageDAO.GetPages(this.formInfo.MobanID);
            foreach (var val in pages)
            {
                builder.AppendFormat(@"<tr><td align=""right"" colspan=""4""><strong>{0}</strong></td></tr>", val.Value);

                List<FormGroupInfo> groupList = DataProvider.FormGroupDAO.GetFormGroupInfoList(val.Key);

                foreach (FormGroupInfo groupInfo in groupList)
                {
                    List<FormElementInfo> list = DataProvider.FormElementDAO.GetFormElementInfoList(groupInfo.PageID, groupInfo.ID);

                    foreach (FormElementInfo elementInfo in list)
                    {
                        builder.AppendFormat(@"<tr><td align=""right"">{0}：</td><td colspan=""3"">{1}</td></tr>", elementInfo.DisplayName, FormElementParser.GetContent(this.formInfo.GetExtendedAttribute(elementInfo.AttributeName), elementInfo));
                    }
                }
            }

            base.SetValue("List", builder.ToString());
        }
    }
}
