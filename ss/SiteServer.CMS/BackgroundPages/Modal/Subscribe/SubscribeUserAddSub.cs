using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using BaiRong.Core.Net;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SubscribeUserAddSub : BackgroundBasePage
    {  
        public CheckBoxList cbSubscribe;

        private int publishmentSystemID;
        private string subscribeUserIDs;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string subscribeUserID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SubscribeUserIDs", subscribeUserID.ToString());
            return PageUtility.GetOpenWindowString("会员订阅内容", "modal_subscribeUserAddSub.aspx", arguments, 580, 500); 
        }

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());  
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("会员订阅内容", "modal_subscribeUserAddSub.aspx", arguments, "IDsCollection", "请选择需要订阅的会员信息", 580, 500);
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
            this.subscribeUserIDs = base.GetQueryString("SubscribeUserIDs");
            if (base.GetQueryString("IDsCollection") != null)
                this.subscribeUserIDs = base.GetQueryString("IDsCollection");

            if (!IsPostBack)
            {
                ArrayList alist = DataProvider.SubscribeDAO.GetInfoList(base.PublishmentSystemID, " and ParentID != 0 ");
                if (alist.Count > 0)
                {
                    foreach (SubscribeInfo info in alist)
                    {
                        ListItem listItem = new ListItem(info.ItemName, info.ItemID.ToString());
                        cbSubscribe.Items.Add(listItem);
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.subscribeUserIDs.Length > 0)
            {
                try
                {
                    ArrayList selList = new ArrayList();
                    foreach (ListItem item in cbSubscribe.Items)
                    {
                        if (item.Selected)
                        {
                            selList.Add(item.Value);
                        }
                    }
                    if (selList.Count == 0)
                    {
                        base.FailMessage("会员订阅信息失败，请选择订阅内容！");
                        return;
                    }

                    DataProvider.SubscribeUserDAO.UpdateUserSubscribe(this.publishmentSystemID, selList, TranslateUtils.StringCollectionToArrayList(this.subscribeUserIDs));


                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "会员订阅信息失败:" + ex);
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
         
    }
}
