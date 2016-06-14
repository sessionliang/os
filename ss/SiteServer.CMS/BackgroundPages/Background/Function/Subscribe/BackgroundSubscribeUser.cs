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
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;
using BaiRong.Core.Net;
using System.Text;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSubscribeUser : BackgroundBasePage
    {
        public DropDownList PageNum;
        public DropDownList ddlOrder;
        public DropDownList State;
        public TextBox Mobile;
        public TextBox Email;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button AddSubscribeUser;
        public Button CancelButton;
        public Button Delete;
        public Button SubscribeButton;
        public Button ManualPush;

        public PlaceHolder PlaceHolder1;
        public PlaceHolder PhState;
        public PlaceHolder PhState2;
        public PlaceHolder CheckPlaceHolder;

        int nodeID = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            if (!string.IsNullOrEmpty(base.GetQueryString("ItemID")))
            {
                this.nodeID = base.GetIntQueryString("ItemID");
            }
            this.PlaceHolder1.Visible = true;
            SubscribeInfo info = DataProvider.SubscribeDAO.GetContentInfo(base.PublishmentSystemID, this.nodeID);
            SubscribeInfo pinfo = DataProvider.SubscribeDAO.GetDefaultInfo(base.PublishmentSystemID);
            if (info.ItemID == pinfo.ItemID)
            {
                this.PhState.Visible = true;
                this.PhState2.Visible = true;
                this.CheckPlaceHolder.Visible = true;
            }
            else
            {
                this.PhState.Visible = false;
                this.PhState2.Visible = false;
                this.CheckPlaceHolder.Visible = false;
            }

            #region 修改订阅状态
            if (base.GetQueryString("ItemID") != null && base.GetQueryString("IDsCollection") != null && base.GetQueryString("ChangeState") != null && base.GetQueryString("State") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDsCollection"));
                EBoolean state = EBooleanUtils.GetEnumType(base.GetQueryString("State"));
                string stateName = EBooleanUtils.Equals(EBoolean.False, state) ? "取消订阅" : "订阅";
                if (info.ItemID == pinfo.ItemID)//如果是所有内容,则是取消会员所有的订阅内容及取消会员的订阅状态
                {
                    DataProvider.SubscribeUserDAO.ClearSubscribeName(base.PublishmentSystemID, arraylist, string.Empty);
                    DataProvider.SubscribeUserDAO.ChangeSubscribeStatu(base.PublishmentSystemID, arraylist, state);
                }
                else //如果是内容，则是订阅或取消当前内容
                {
                    ArrayList listSub = new ArrayList();
                    listSub.Add(info.ItemID.ToString());
                    DataProvider.SubscribeUserDAO.UpdateHasSubscribe(base.PublishmentSystemID, listSub, arraylist, state);
                }
                StringUtility.AddLog(base.PublishmentSystemID, stateName + "订阅内容");

                base.SuccessMessage(stateName + "成功！");

            }
            #endregion

            #region 删除订阅信息
            if (base.GetQueryString("ItemID") != null && base.GetQueryString("IDsCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDsCollection"));




                DataProvider.SubscribeUserDAO.Delete(base.PublishmentSystemID, arraylist);
                StringUtility.AddLog(base.PublishmentSystemID, "订阅内容");

                //统计顶级的数量
                SubscribeInfo sinfo = DataProvider.SubscribeDAO.GetDefaultInfo(base.PublishmentSystemID);
                DataProvider.SubscribeDAO.UpdateContentNum(base.PublishmentSystemID, sinfo.ItemID, DataProvider.SubscribeUserDAO.GetCount(base.PublishmentSystemID, string.Empty));

                base.SuccessMessage("删除订阅信息！");

            }
            #endregion

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (TranslateUtils.ToInt(this.PageNum.SelectedValue) == 0)
            {
                this.spContents.ItemsPerPage = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID).Additional.PageSize;
            }
            else
            {
                this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.PageNum.SelectedValue);
            }
            if (string.IsNullOrEmpty(base.GetQueryString("ItemID")))
            {
                ETriState stateType = ETriStateUtils.GetEnumType(this.State.SelectedValue);
                this.spContents.SelectCommand = DataProvider.SubscribeUserDAO.GetSelectCommend(base.PublishmentSystemID, this.nodeID.ToString(), this.Mobile.Text, this.Email.Text, this.DateFrom.Text, this.DateTo.Text, stateType);
            }
            else
            {
                ETriState stateType = ETriStateUtils.GetEnumType(base.GetQueryString("SearchState"));
                this.spContents.SelectCommand = DataProvider.SubscribeUserDAO.GetSelectCommend(base.PublishmentSystemID, this.nodeID.ToString(), base.GetQueryString("Mobile"), base.GetQueryString("Email"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), stateType);
            }
            if (TranslateUtils.ToInt(this.ddlOrder.SelectedValue) == 0)
            {
                this.spContents.SortField = SubscribeUserAttribute.AddDate;
            }
            else
            {
                this.spContents.SortField = this.PageNum.SelectedValue;
            }
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "订阅会员列表", AppManager.CMS.Permission.WebSite.Subscribe);


                ETriStateUtils.AddListItems(this.State, "全部", "正常订阅", "取消订阅");


                if (!string.IsNullOrEmpty(base.GetQueryString("ItemID")))
                {
                    ControlUtils.SelectListItems(this.State, base.GetQueryString("SearchState"));
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    ControlUtils.SelectListItems(this.ddlOrder, base.GetQueryString("OrderName"));
                    this.Mobile.Text = base.GetQueryString("Mobile");
                    this.Email.Text = base.GetQueryString("Email");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }


                this.AddSubscribeUser.Attributes.Add("onclick", Modal.SubscribeUserAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.nodeID));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&Delete=true&ItemID={1}", base.PublishmentSystemID, this.nodeID)), "IDsCollection", "IDsCollection", "请选择需要删除的订阅会员！", "本操作将会删除订阅会员，确定删除吗？"));

                this.CancelButton.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&ChangeState=True&ItemID={1}&State={2}", base.PublishmentSystemID, this.nodeID, EBoolean.False)), "IDsCollection", "IDsCollection", "请选择需要取消订阅的会员信息！", "本操作将会使选中的会员不再接受此类信息的订阅，确定取消吗？"));


                this.SubscribeButton.Attributes.Add("onclick", Modal.SubscribeUserAddSub.GetOpenWindowStringToAdd(base.PublishmentSystemID));


                this.ManualPush.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("modal_subscribePush.aspx?PublishmentSystemID={0}&ItemID={1}", base.PublishmentSystemID, this.nodeID)), "IDsCollection", "IDsCollection", "请选择需要推送订阅信息的会员！"));
            }


            if (base.GetQueryString("submitType") == "2")
            {
                this.Response.Clear();
                this.Response.Write(add());
                this.Response.End();
            }
        }

        private readonly Hashtable valueHashtable = new Hashtable();

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemEmail = e.Item.FindControl("ltlItemEmail") as Literal;
                Literal ltlItemUser = e.Item.FindControl("ltlItemUser") as Literal;
                Literal ltlItemSubscribeName = e.Item.FindControl("ltlItemSubscribeName") as Literal;
                Literal ltlItemMobile = e.Item.FindControl("ltlItemMobile") as Literal;
                Literal ltlItemAddDate = e.Item.FindControl("ltlItemAddDate") as Literal;
                Literal ltlItemPushNum = e.Item.FindControl("ltlItemPushNum") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlItemStatusUrl = e.Item.FindControl("ltlItemStatusUrl") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;

                int subscribeUserID = TranslateUtils.EvalInt(e.Item.DataItem, "SubscribeUserID");
                string email = TranslateUtils.EvalString(e.Item.DataItem, "Email");
                string subscribeName = TranslateUtils.EvalString(e.Item.DataItem, "SubscribeName");
                string mobile = TranslateUtils.EvalString(e.Item.DataItem, "Mobile");
                int userID = TranslateUtils.EvalInt(e.Item.DataItem, "UserID");
                EBoolean subscribeStatu = EBooleanUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "SubscribeStatu"));
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                int pushNum = TranslateUtils.EvalInt(e.Item.DataItem, "PushNum");

                UserInfo uinfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                ltlItemEmail.Text = email;

                if (string.IsNullOrEmpty(subscribeName))
                    ltlItemSubscribeName.Text = "";
                else
                    ltlItemSubscribeName.Text = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, subscribeName);

                ltlItemUser.Text = uinfo == null ? "" : uinfo.UserName;
                ltlItemMobile.Text = mobile;
                ltlItemStatus.Text = EBooleanUtils.Equals(EBoolean.False, subscribeStatu) ? "取消订阅" : "正常订阅";
                ltlItemAddDate.Text = addDate.ToShortDateString().Replace('/', '-');
                ltlItemPushNum.Text = pushNum.ToString();


                ltlItemEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">修改</a>", Modal.SubscribeUserAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, subscribeUserID));

                //如果当前会员的状态为取消订阅，则进行订阅操作
                if (EBooleanUtils.Equals(EBoolean.False, subscribeStatu))
                {
                    ltlItemStatusUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">订阅</a>", Modal.SubscribeUserAddSub.GetOpenWindowStringToAdd(base.PublishmentSystemID, subscribeUserID.ToString()));
                }
                else
                {
                    ltlItemStatusUrl.Text = string.Format(@"<a title=""{0}"" href=""background_subscribeUser.aspx?PublishmentSystemID={1}&ChangeState=True&ItemID={2}&State={3}&IDsCollection={4}"">{0}</a>", "取消订阅", base.PublishmentSystemID, this.nodeID, EBoolean.False, subscribeUserID);
                }

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDsCollection"" value=""{0}"" />", subscribeUserID);
            }


            if (base.GetQueryString("submitType") == "2")
            {
                this.Response.Clear();
                this.Response.Write(add());
                this.Response.End();
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
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&ItemID={1}&SearchState={2}&Mobile={3}&Email={4}&DateFrom={5}&DateTo={6}&PageNum={7}&OrderName={8}", base.PublishmentSystemID, this.nodeID, this.State.SelectedValue, this.Mobile.Text, this.Email.Text, this.DateFrom.Text, this.DateTo.Text, this.PageNum.SelectedValue, this.ddlOrder.SelectedValue));
                }
                return this._pageUrl;
            }
        }


        public void send(ArrayList arraylist)
        {
            try
            {
                //获取订阅设置
                SubscribeSetInfo sinfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(base.PublishmentSystemID);

                ArrayList infoList = DataProvider.SubscribeUserDAO.GetSubscribeUserList(base.PublishmentSystemID, arraylist);

                foreach (SubscribeUserInfo info in infoList)
                {
                    ISmtpMail smtpMail = MailUtils.GetInstance();
                    smtpMail.AddRecipient(info.Email);

                    smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                    smtpMail.IsHtml = true;
                    smtpMail.Subject = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";

                    string strBody = "";

                    string cvalue = "";
                    string lvalue = "";
                    DataProvider.SubscribeDAO.GetValueByUserID(base.PublishmentSystemID, info.SubscribeName, out cvalue, out lvalue);

                    //获取邮件模板的静态代码 
                    try
                    {
                        TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByURLType(base.PublishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress + "");
                        if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
                        {
                            return;
                        }
                        StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(base.PublishmentSystemInfo, templateInfo));

                        NameValueCollection queryString = new NameValueCollection();
                        queryString.Remove("publishmentSystemID");
                        queryString.Add("channelIndex", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(TranslateUtils.StringCollectionToArrayList(cvalue)));
                        queryString.Add("tags", lvalue);

                        // strBody = StlUtility.ParseDynamicContent(publishmentSystemID, 0, 0, templateInfo.TemplateID, false, contentBuilder.ToString(), sinfo.EmailContentAddress, 1, "", queryString);
                    }
                    catch (Exception ex)
                    {

                    }




                    smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + strBody + "</pre>";
                    smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                    smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                    smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                    //开始发送
                    string errorMessage = string.Empty;
                    bool isSuccess = smtpMail.Send(out errorMessage);
                    if (isSuccess)
                    {
                        //修改会员信息推送次数 
                        DataProvider.SubscribeUserDAO.UpdatePushNum(base.PublishmentSystemID, info.SubscribeUserID);

                        StringUtility.AddLog(base.PublishmentSystemID, "订阅信息手动推送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                        base.SuccessMessage("订阅信息邮件发送成功！");
                    }
                    else
                    {
                        base.FailMessage("订阅信息邮件发送失败：" + errorMessage);
                    }

                    //记录邮件发送状态
                    SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                    {
                        Email = info.Email,
                        PushType = ESubscribePushType.ManualPush,
                        SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                        SubscriptionTemplate = "订阅信息成功发送邮件",
                        PublishmentSystemID = base.PublishmentSystemID,
                        PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                        UserName = BaiRongDataProvider.AdministratorDAO.UserName
                    };
                    DataProvider.SubscribePushRecordDAO.Insert(srinfo);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "订阅信息邮件发送失败：" + ex.Message);
            }
        }


        public int add()
        {
            int returnSB = 1;
            if (DataProvider.SubscribeUserDAO.IsExists(base.GetQueryString("Email")))
            {
                returnSB = 2;
                base.FailMessage("添加会员订阅信息失败，会员订阅信息已存在！");
            }
            else
            {
                try
                {
                    SubscribeUserInfo subscribeUserInfo = new SubscribeUserInfo();
                    subscribeUserInfo.Email = PageUtils.FilterXSS(base.GetQueryString("Email"));
                    subscribeUserInfo.Mobile = base.GetQueryString("Mobile");
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = base.GetQueryString("SubscribeID");
                    subscribeUserInfo.PublishmentSystemID = base.PublishmentSystemID;
                    DataProvider.SubscribeUserDAO.Insert(subscribeUserInfo);
                    DataProvider.SubscribeDAO.UpdateSubscribeNum(base.PublishmentSystemID, base.GetQueryString("SubscribeID"), 1);
                    sendText(subscribeUserInfo, "add");
                }
                catch (Exception ex)
                {
                    returnSB = 3;
                    base.FailMessage(ex, "添加会员订阅信息失败！");
                }
            }
            return returnSB;
        }


        public void sendText(SubscribeUserInfo info, string type)
        {

            string strBody = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "您的信息订阅成功";
            bool isSuccess = true;
            if (type == "add")
            {
                //发送订阅成功邮件
                try
                {
                    ISmtpMail smtpMail = MailUtils.GetInstance();
                    string[] usernames = info.Email.Split(new char[] { ',' });
                    smtpMail.AddRecipient(usernames);

                    smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                    smtpMail.IsHtml = true;
                    smtpMail.Subject = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";


                    smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + strBody + "</pre>";
                    smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                    smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                    smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                    //开始发送
                    string errorMessage = string.Empty;
                    isSuccess = smtpMail.Send(out errorMessage);
                    if (isSuccess)
                    {
                        StringUtility.AddLog(base.PublishmentSystemID, "订阅信息成功发送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                        base.SuccessMessage("订阅信息邮件发送成功！");
                    }
                    else
                    {
                        base.FailMessage("订阅信息邮件发送失败：" + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "订阅信息邮件发送失败：" + ex.Message);
                }

                //记录邮件发送状态
                SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                {
                    Email = info.Email,
                    PushType = ESubscribePushType.ManualPush,
                    SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                    SubscriptionTemplate = "订阅信息成功发送邮件",
                    PublishmentSystemID = base.PublishmentSystemID,
                    PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                    UserName = BaiRongDataProvider.AdministratorDAO.UserName
                };
                DataProvider.SubscribePushRecordDAO.Insert(srinfo);


                //发送手机提醒
                if (!string.IsNullOrEmpty(info.Mobile))
                {
                    ArrayList mobileArrayList = new ArrayList();
                    mobileArrayList.Add(info.Mobile);

                    if (mobileArrayList.Count > 0)
                    {
                        try
                        {
                            string errorMessage = string.Empty;
                            isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);

                            if (isSuccess)
                            {
                                StringUtility.AddLog(base.PublishmentSystemID, "订阅信息发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), strBody));

                                base.SuccessMessage("订阅信息短信发送成功！");
                            }
                            else
                            {
                                base.FailMessage("订阅信息短信发送失败：" + errorMessage + "! 请检查短信服务商设置！");
                            }
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "订阅信息短信发送失败：" + ex.Message + " 请与管理员联系！");
                        }
                    }

                    //记录邮件发送状态
                    srinfo = new SubscribePushRecordInfo()
                    {
                        Mobile = info.Mobile,
                        PushType = ESubscribePushType.ManualPush,
                        SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                        SubscriptionTemplate = "订阅信息成功发送手机提醒",
                        PublishmentSystemID = base.PublishmentSystemID,
                        PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                        UserName = BaiRongDataProvider.AdministratorDAO.UserName
                    };
                    DataProvider.SubscribePushRecordDAO.Insert(srinfo);
                }


            }
        }
    }
}
