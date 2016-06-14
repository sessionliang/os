using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;


using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Web;

namespace SiteServer.Project.BackgroundPages
{
    public class RequestForm : BackgroundBasePage
    {
        public Literal ltlJsonOrder;
        public Literal ltlJsonFormPages;
        public Literal ltlJsonFormGroups;

        private string domain = string.Empty;
        private int orderID = 0;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public static string GetRedirectUrl(string domain)
        {
            return string.Format("http://brs.siteserver.cn/siteserver/project/request_form.aspx?domain={0}", domain);
            //return string.Format("http://www.yun.com/siteserver/project/request_form2.aspx?domain={0}", domain);
        }

        private const int PAGE_WORKING = -1;
        private const int PAGE_DONE = -2;

        public void Page_Load(object sender, EventArgs E)
        {
            this.domain = base.GetQueryString("domain");
            this.domain = ProjectManager.GetCleanDomain(this.domain);
            this.orderID = DataProvider.OrderDAO.GetOrderID(this.domain);

            if (!IsPostBack)
            {
                if (this.orderID > 0)
                {
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(this.orderID);
                    MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(orderInfo.MobanID);
                    OrderFormInfo formInfo = DataProvider.OrderFormDAO.GetOrderFormInfoByOrderID(orderInfo.ID);

                    if (mobanInfo != null)
                    {
                        int currentPageID = 0;
                        if (formInfo != null)
                        {
                            if (formInfo.IsCompleted)
                            {
                                if (orderInfo.Status == EOrderStatus.New || orderInfo.Status == EOrderStatus.Messaged || orderInfo.Status == EOrderStatus.Formed)
                                {
                                    currentPageID = RequestForm.PAGE_WORKING;
                                }
                                else
                                {
                                    currentPageID = RequestForm.PAGE_DONE;
                                }
                            }
                            else
                            {
                                currentPageID = DataProvider.FormPageDAO.GetNextPageID(mobanInfo.ID, formInfo.CurrentPageID);
                            }
                            base.SetValue("submitDate", DateUtils.GetDateString(formInfo.AddDate, EDateFormatType.Chinese));
                            base.SetValue("doneDate", DateUtils.GetDateString(formInfo.AddDate.AddDays(2), EDateFormatType.Chinese));
                        }

                        if (base.Request.QueryString["currentPageID"] != null)
                        {
                            currentPageID = TranslateUtils.ToInt(base.Request.QueryString["currentPageID"]);
                        }

                        Dictionary<int, string> pages = DataProvider.FormPageDAO.GetPages(mobanInfo.ID);
                        pages.Add(RequestForm.PAGE_WORKING, "网站开通中");
                        pages.Add(RequestForm.PAGE_DONE, "网站开通成功");

                        StringBuilder pageBuilder = new StringBuilder();
                        pageBuilder.Append("[");
                        int i = 0;
                        bool isFinished = true;
                        int stage = 1;
                        foreach (var val in pages)
                        {
                            i++;
                            if (i == 1)
                            {
                                if (currentPageID == 0)
                                {
                                    currentPageID = val.Key;
                                }
                            }
                            string state = "ali_ok";//finish

                            if (currentPageID == val.Key)
                            {
                                isFinished = false;
                                stage = i;
                                state = "ali_posCut";
                            }
                            else if (isFinished)
                            {
                                state = "ali_ok";
                            }
                            else
                            {
                                state = "";
                            }
                            pageBuilder.AppendFormat(@"{{pageID:'{0}', title:'{1}', state:'{2}'}},", val.Key, val.Value, state);
                        }
                        if (i > 0)
                        {
                            pageBuilder.Length -= 1;
                        }
                        pageBuilder.Append("]");

                        base.SetValue("stage", Convert.ToString(stage));

                        base.AddAttributes(orderInfo.Attributes);
                        base.RemoveValue(OrderAttribute.Summary);
                        base.SetValue("mobanUrl", DataProvider.MobanDAO.GetMobanUrl(mobanInfo));
                        base.SetValue("orderFormID", formInfo == null ? "0" : formInfo.ID.ToString());
                        base.SetValue("currentPageID", currentPageID.ToString());
                        base.SetValue("orderID", orderInfo.ID.ToString());
                        base.SetValue("mobanID", mobanInfo.ID.ToString());

                        if (currentPageID == RequestForm.PAGE_WORKING)
                        {
                            base.SetValue("state", "working");
                        }
                        else if (currentPageID == RequestForm.PAGE_DONE)
                        {
                            base.SetValue("state", "done");
                        }
                        this.ltlJsonOrder.Text = TranslateUtils.NameValueCollectionToJsonString(base.GetAttributes());
                        this.ltlJsonFormGroups.Text = "''";

                        this.ltlJsonFormPages.Text = pageBuilder.ToString();

                        if (currentPageID >= 0)
                        {
                            List<FormGroupInfo> groupList = DataProvider.FormGroupDAO.GetFormGroupInfoList(currentPageID);

                            NameValueCollection pageScripts = new NameValueCollection();

                            StringBuilder groupBuilder = new StringBuilder();
                            groupBuilder.Append("[");
                            foreach (FormGroupInfo groupInfo in groupList)
                            {
                                List<FormElementInfo> list = DataProvider.FormElementDAO.GetFormElementInfoList(groupInfo.PageID, groupInfo.ID);

                                StringBuilder elementBuilder = new StringBuilder();
                                elementBuilder.Append("[");

                                foreach (FormElementInfo elementInfo in list)
                                {
                                    string inputHtml = FormElementParser.Parse(elementInfo, elementInfo.AttributeName, null, true, false, null, pageScripts);
                                    elementBuilder.AppendFormat(@"{{attributeName:'{0}', displayName:'{1}', require:'{2}', imageUrl:'{3}', inputHtml:'{4}'}},", elementInfo.AttributeName, elementInfo.DisplayName, elementInfo.Additional.IsValidate ? "*" : string.Empty, elementInfo.ImageUrl, inputHtml);
                                }

                                elementBuilder.Length -= 1;
                                elementBuilder.Append("]");

                                groupBuilder.AppendFormat(@"{{pageID:'{0}', groupID:'{1}', title:'{2}', iconUrl:'{3}', elements:{4}}},", groupInfo.PageID, groupInfo.ID, groupInfo.Title, groupInfo.IconUrl, elementBuilder);
                            }
                            if (groupBuilder.Length > 1)
                            {
                                groupBuilder.Length -= 1;
                            }
                            groupBuilder.Append("]");

                            this.ltlJsonFormGroups.Text = groupBuilder.ToString();
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                int currentPageID = TranslateUtils.ToInt(base.Request.Form["currentPageID"]);
                int orderFormID = TranslateUtils.ToInt(base.Request.Form["orderFormID"]);
                int orderID = TranslateUtils.ToInt(base.Request.Form["orderID"]);
                int mobanID = TranslateUtils.ToInt(base.Request.Form["mobanID"]);

                OrderFormInfo formInfo = null;
                if (orderFormID != 0)
                {
                    formInfo = DataProvider.OrderFormDAO.GetOrderFormInfo(orderFormID);
                }
                if (formInfo == null)
                {
                    formInfo = new OrderFormInfo(0, orderID, mobanID, false, currentPageID, DateTime.Now);
                }

                OrderFormInfo newFormInfo = DataProvider.OrderFormDAO.GetOrderFormInfo(base.Request.Form);
                formInfo.SetExtendedAttribute(newFormInfo.Attributes);
                formInfo.Attributes.Remove("__viewstate");

                formInfo.ID = orderFormID;
                formInfo.CurrentPageID = currentPageID;
                formInfo.IsCompleted = DataProvider.FormPageDAO.IsCompleted(mobanID, currentPageID);
                if (formInfo.IsCompleted)
                {
                    formInfo.CurrentPageID = 0;
                }

                if (base.Request.Files != null && base.Request.Files.Count > 0)
                {
                    foreach (string attributeName in base.Request.Files.AllKeys)
                    {
                        HttpPostedFile myFile = base.Request.Files[attributeName];
                        if (myFile != null && "" != myFile.FileName)
                        {
                            string fileUrl = this.UploadFile(myFile);
                            formInfo.SetExtendedAttribute(attributeName, fileUrl);
                        }
                    }
                }

                if (orderFormID == 0)
                {
                    DataProvider.OrderFormDAO.Insert(formInfo);
                }
                else
                {
                    DataProvider.OrderFormDAO.Update(formInfo);
                }

                PageUtils.Redirect(RequestForm.GetRedirectUrl(this.domain));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            string filePath = myFile.FileName;
            try
            {
                string fileExtName = PathUtils.GetExtension(filePath);
                string localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(this.domain);
                string localFileName = PathUtils.GetUserUploadFileName(filePath);

                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                myFile.SaveAs(localFilePath);

                fileUrl = PageUtils.GetRootUrlByPhysicalPath(localFilePath);
            }
            catch { }

            return fileUrl;
        }
    }
}
