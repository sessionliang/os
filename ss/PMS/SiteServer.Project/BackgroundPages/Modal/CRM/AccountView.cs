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


namespace SiteServer.Project.BackgroundPages.Modal
{
    public class AccountView : BackgroundBasePage
    {
        public static string GetShowPopWinString(int accountID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("accountID", accountID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("客户信息", "modal_accountView.aspx", arguments, true);
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == AccountAttribute.AddUserName || attributeName == AccountAttribute.ChargeUserName)
            {
                retval = AdminManager.GetDisplayName(value, true);
            }
            else if (attributeName == AccountAttribute.Status)
            {
                retval = EAccountStatusUtils.GetText(EAccountStatusUtils.GetEnumType(value));
            }
            else if (attributeName == AccountAttribute.Priority)
            {
                retval = "普通";
                if (value == "2")
                {
                    retval = "高";
                }
                else if (value == "3")
                {
                    retval = "重点";
                }
            }
            
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int accountID = int.Parse(base.Request.QueryString["accountID"]);
            AccountInfo accountInfo = DataProvider.AccountDAO.GetAccountInfo(accountID);
            base.AddAttributes(accountInfo);
        }
    }
}
