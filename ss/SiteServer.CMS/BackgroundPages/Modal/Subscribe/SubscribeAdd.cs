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

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SubscribeAdd : BackgroundBasePage
    {
        protected TextBox tbSubscribeName;
        protected TextBox tbSubscribeValue;
        protected RadioButtonList rblType;
        protected RadioButtonList rblEnabled;

        private int publishmentSystemID;
        private int subscribeID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加订阅内容", "modal_subscribeAdd.aspx", arguments, 580, 400);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int subscribeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SubscribeID", subscribeID.ToString());
            return PageUtility.GetOpenWindowString("修改订阅内容", "modal_subscribeAdd.aspx", arguments, 580, 400);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("PublishmentSystemID"));
            this.subscribeID = TranslateUtils.ToInt(base.GetQueryString("SubscribeID"));

            if (!IsPostBack)
            {
                ESubscribeContentTypeUtils.AddListItems(this.rblType);
                this.rblType.SelectedIndex = 0;
                EBooleanUtils.AddListItems(this.rblEnabled, "正常", "暂停");
                this.rblEnabled.SelectedIndex = 0;

                if (this.subscribeID > 0)
                {
                    SubscribeInfo subscribeInfo = DataProvider.SubscribeDAO.GetContentInfo(subscribeID);
                    this.tbSubscribeName.Text = subscribeInfo.ItemName;
                    this.tbSubscribeValue.Text = subscribeInfo.SubscribeValue;
                    ControlUtils.SelectListItems(this.rblType, ESubscribeContentTypeUtils.GetValue(ESubscribeContentTypeUtils.GetEnumType(subscribeInfo.ContentType)));
                    this.rblType.SelectedValue = ESubscribeContentTypeUtils.GetValue(ESubscribeContentTypeUtils.GetEnumType(subscribeInfo.ContentType));
                    this.rblType.SelectedValue = EBooleanUtils.GetEnumType(subscribeInfo.Enabled).ToString();
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.subscribeID > 0)
            {
                try
                {
                    SubscribeInfo subscribeInfo = DataProvider.SubscribeDAO.GetContentInfo(this.subscribeID);
                    subscribeInfo.ItemName = PageUtils.FilterXSS(this.tbSubscribeName.Text.Trim());
                    subscribeInfo.SubscribeValue = PageUtils.FilterXSS(this.tbSubscribeValue.Text.Trim());
                    subscribeInfo.ContentType = ESubscribeContentTypeUtils.GetValue(ESubscribeContentTypeUtils.GetEnumType(this.rblType.SelectedValue));
                    subscribeInfo.Enabled = this.rblEnabled.SelectedValue;
                    subscribeInfo.PublishmentSystemID = this.publishmentSystemID;
                    DataProvider.SubscribeDAO.Update(subscribeInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "修改订阅内容失败！");
                }
            }
            else
            {
                if (DataProvider.SubscribeDAO.IsExists(this.tbSubscribeName.Text))
                {
                    base.FailMessage("订阅内容添加失败，订阅内容名称已存在！");
                }
                else
                {
                    try
                    {
                        //获取全部内容的ID 
                        SubscribeInfo info = DataProvider.SubscribeDAO.GetDefaultInfo(base.PublishmentSystemID);

                        SubscribeInfo subscribeInfo = new SubscribeInfo();
                        subscribeInfo.ParentID = info.ItemID;
                        subscribeInfo.ItemName = PageUtils.FilterXSS(this.tbSubscribeName.Text.Trim());
                        subscribeInfo.SubscribeValue = PageUtils.FilterXSS(this.tbSubscribeValue.Text.Replace('　', ' ').Trim().Trim(',').Replace('，', ',').Trim(',').Trim());
                        subscribeInfo.ContentType = ESubscribeContentTypeUtils.GetValue(ESubscribeContentTypeUtils.GetEnumType(this.rblType.SelectedValue));
                        subscribeInfo.Enabled = this.rblEnabled.SelectedValue;
                        subscribeInfo.PublishmentSystemID = this.publishmentSystemID;
                        DataProvider.SubscribeDAO.Insert(subscribeInfo);
                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "添加订阅内容失败:" + ex);
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
