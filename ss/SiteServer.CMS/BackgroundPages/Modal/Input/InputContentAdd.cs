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
	public class InputContentAdd : BackgroundBasePage
	{
        protected AuxiliaryControl ContentControl;

        private string returnUrl;
        private InputInfo inputInfo;
        private int contentID;
        private ArrayList relatedIdentities;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int inputID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加信息", "modal_inputContentAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int inputID, int contentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("修改信息", "modal_inputContentAdd.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int inputID = int.Parse(base.GetQueryString("InputID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

			if (base.GetQueryString("ContentID") != null)
			{
                this.contentID = int.Parse(base.GetQueryString("ContentID"));
			}
			else
			{
                this.contentID = 0;
			}

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, inputID);

            if (!IsPostBack)
            {
                if (this.contentID != 0)
                {
                    InputContentInfo contentInfo = DataProvider.InputContentDAO.GetContentInfo(this.contentID);
                    if (contentInfo != null)
                    {
                        this.ContentControl.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, true, base.IsPostBack);
                    }
                }
                else
                {
                    this.ContentControl.SetParameters(null, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, false, base.IsPostBack);
                }
                
            }
            else
            {
                if (this.contentID != 0)
                {
                    this.ContentControl.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, true, base.IsPostBack);
                }
                else
                {
                    this.ContentControl.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, false, base.IsPostBack);
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
                    InputContentInfo contentInfo = DataProvider.InputContentDAO.GetContentInfo(this.contentID);

                    InputTypeParser.AddValuesToAttributes(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, contentInfo.Attributes);

                    DataProvider.InputContentDAO.Update(contentInfo);

                    StringBuilder builder = new StringBuilder();

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, this.relatedIdentities);
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, ETableStyle.InputContent, styleInfo);

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

                    StringUtility.AddLog(base.PublishmentSystemID, "修改提交表单内容", string.Format("提交表单:{0},{1}", this.inputInfo.InputName, builder.ToString()));
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

                    InputContentInfo contentInfo = new InputContentInfo(0, this.inputInfo.InputID, 0, true, string.Empty, ipAddress, location, DateTime.Now, string.Empty);

                    InputTypeParser.AddValuesToAttributes(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, contentInfo.Attributes);

                    DataProvider.InputContentDAO.Insert(contentInfo);

                    StringBuilder builder = new StringBuilder();

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, this.relatedIdentities);
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, ETableStyle.InputContent, styleInfo);

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

                    StringUtility.AddLog(base.PublishmentSystemID, "添加提交表单内容", string.Format("提交表单:{0},{1}", this.inputInfo.InputName, builder.ToString()));
                    isChanged = true;
                }
                catch(Exception ex)
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
