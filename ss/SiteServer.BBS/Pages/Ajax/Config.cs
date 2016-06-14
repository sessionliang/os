using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Text.LitJson;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages.Ajax
{
    public class Config : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string type = PageUtils.FilterSqlAndXss(Request.Form["type"]).ToLower();
                string value = PageUtils.FilterSqlAndXss(Request.Form["value"]);
                Hashtable attributes = new Hashtable();

                //if (type == "fullscreen")
                //{
                //    string errorMessage = string.Empty;
                //    bool success = false;
                //    try
                //    {
                //        UserExtendInfo userInfo = UserExtendManager.GetCurrentUserInfo();
                //        if (userInfo != null)
                //        {
                //            userInfo.DisplayFullScreen = value;
                //            BaiRongDataProvider.UserExtendDAO.Update(userInfo);
                //            success = true;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        errorMessage = ex.Message;
                //    }
                //    attributes.Add("success", success.ToString().ToLower());
                //    attributes.Add("errorMessage", "…Ë÷√ ß∞‹£¨" + errorMessage);
                //}
                //else 
                if (type == "checkusername")
                {
                    bool success = CheckUserName(publishmentSystemID, value);
                    attributes.Add("success", success.ToString().ToLower());
                }

                string json = JsonMapper.ToJson(attributes);
                base.Response.Write(json);
                base.Response.End();
            }
        }

        public bool CheckUserName(int publishmentSystemID, string value)
        {
            string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);

            if (string.IsNullOrEmpty(value)) return false;
            bool result = true;
            if (!BaiRongDataProvider.UserDAO.IsUserNameCompliant(value))
            {
                result = false;
            }
            else if (BaiRongDataProvider.UserDAO.IsUserExists(groupSN, value))
            {
                result = false;
            }
            else
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(UserConfigManager.Additional.ReservedUserNames.ToLower());
                if (arraylist.Contains(value.ToLower()))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
