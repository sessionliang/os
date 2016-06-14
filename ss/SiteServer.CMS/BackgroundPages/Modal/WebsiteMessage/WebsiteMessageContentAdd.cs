using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class WebsiteMessageContentAdd : BackgroundBasePage
    {
        protected AuxiliaryControl ContentControl;

        private string returnUrl;
        private WebsiteMessageInfo websiteMessageInfo;
        private int contentID;
        private ArrayList relatedIdentities;
        private int classifyID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int websiteMessageID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加信息", "modal_websiteMessageContentAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int websiteMessageID, int contentID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("修改信息", "modal_websiteMessageContentAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int websiteMessageID = int.Parse(base.GetQueryString("WebsiteMessageID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);

            if (base.GetQueryString("ContentID") != null)
            {
                this.contentID = int.Parse(base.GetQueryString("ContentID"));
            }
            else
            {
                this.contentID = 0;
            }
            this.classifyID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("ClassifyID"));

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, websiteMessageID);

            if (!IsPostBack)
            {
                if (this.contentID != 0)
                {
                    WebsiteMessageContentInfo contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(this.contentID);
                    if (contentInfo != null)
                    {
                        this.ContentControl.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, true, base.IsPostBack);
                    }
                }
                else
                {
                    this.ContentControl.SetParameters(null, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, false, base.IsPostBack);
                }

            }
            else
            {
                if (this.contentID != 0)
                {
                    this.ContentControl.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, true, base.IsPostBack);
                }
                else
                {
                    this.ContentControl.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, false, base.IsPostBack);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.contentID != 0)
            {
                try
                {
                    WebsiteMessageContentInfo contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(this.contentID);
                    contentInfo.ClassifyID = this.classifyID;

                    InputTypeParser.AddValuesToAttributes(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, contentInfo.Attributes);

                    DataProvider.WebsiteMessageContentDAO.Update(contentInfo);

                    StringBuilder builder = new StringBuilder();

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                        builder.AppendFormat(@"{0}：{1},", styleInfo.DisplayName, theValue);
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }

                    if (builder.Length > 60)
                    {
                        builder.Length = 60;
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "修改提交表单内容", string.Format("提交表单:{0},{1}", this.websiteMessageInfo.WebsiteMessageName, builder.ToString()));
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "信息修改失败:" + ex.Message);
                }
            }
            else
            {
                try
                {
                    string ipAddress = PageUtils.GetIPAddress();
                    string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

                    WebsiteMessageContentInfo contentInfo = new WebsiteMessageContentInfo(0, this.websiteMessageInfo.WebsiteMessageID, 0, true, string.Empty, ipAddress, location, DateTime.Now, string.Empty);
                    contentInfo.ClassifyID = this.classifyID;
                    InputTypeParser.AddValuesToAttributes(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, contentInfo.Attributes);

                    DataProvider.WebsiteMessageContentDAO.Insert(contentInfo);

                    StringBuilder builder = new StringBuilder();

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                        builder.AppendFormat(@"{0}：{1},", styleInfo.DisplayName, theValue);
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }

                    if (builder.Length > 60)
                    {
                        builder.Length = 60;
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "添加提交表单内容", string.Format("提交表单:{0},{1}", this.websiteMessageInfo.WebsiteMessageName, builder.ToString()));
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "信息添加失败:" + ex.Message);
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
    }
}
