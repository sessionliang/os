using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core.Office;

using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class UploadWord : BackgroundBasePage
    {
        public CheckBox cbIsFirstLineTitle;
        public CheckBox cbIsFirstLineRemove;
        public CheckBox cbIsClearFormat;
        public CheckBox cbIsFirstLineIndent;
        public CheckBox cbIsClearFontSize;
        public CheckBox cbIsClearFontFamily;
        public CheckBox cbIsClearImages;
        public RadioButtonList rblContentLevel;

        private NodeInfo nodeInfo;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("returnUrl", returnUrl);
            return PageUtility.GetOpenWindowString("批量导入Word文件", "modal_uploadWord.aspx", arguments, 550, 400);
        }

        public string GetUploadWordMultipleUrl()
        {
            return PageUtility.ServiceSTL.AjaxUpload.GetUploadWordMultipleUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            int nodeID = int.Parse(base.GetQueryString("NodeID"));
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.returnUrl = base.GetQueryString("ReturnUrl");

            if (!IsPostBack)
            {
                int checkedLevel = 0;
                bool isChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, base.PublishmentSystemID, out checkedLevel);
                LevelManager.LoadContentLevelToEdit(this.rblContentLevel, base.PublishmentSystemInfo, this.nodeInfo.NodeID, null, isChecked, checkedLevel);
                ControlUtils.SelectListItems(this.rblContentLevel, LevelManager.LevelInt.CaoGao.ToString());
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int file_Count = TranslateUtils.ToInt(base.Request.Form["File_Count"]);
                if (file_Count == 1)
                {
                    string fileName = base.Request.Form["fileName_1"];
                    string redirectUrl = WebUtils.GetContentAddUploadWordUrl(base.PublishmentSystemID, this.nodeInfo, this.cbIsFirstLineTitle.Checked, this.cbIsFirstLineRemove.Checked, this.cbIsClearFormat.Checked, this.cbIsFirstLineIndent.Checked, this.cbIsClearFontSize.Checked, this.cbIsClearFontFamily.Checked, this.cbIsClearImages.Checked, TranslateUtils.ToIntWithNagetive(this.rblContentLevel.SelectedValue), fileName, this.returnUrl);
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);

                    return;
                }
                else if (file_Count > 1)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeInfo.NodeID);

                    for (int index = 1; index <= file_Count; index++)
                    {
                        string fileName = base.Request.Form["fileName_" + index];
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            NameValueCollection formCollection = WordUtils.GetWordNameValueCollection(base.PublishmentSystemID, nodeInfo.ContentModelID, this.cbIsFirstLineTitle.Checked, this.cbIsFirstLineRemove.Checked, this.cbIsClearFormat.Checked, this.cbIsFirstLineIndent.Checked, this.cbIsClearFontSize.Checked, this.cbIsClearFontFamily.Checked, this.cbIsClearImages.Checked, TranslateUtils.ToInt(this.rblContentLevel.SelectedValue), fileName);

                            if (!string.IsNullOrEmpty(formCollection[ContentAttribute.Title]))
                            {
                                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                                InputTypeParser.AddValuesToAttributes(tableStyle, tableName, base.PublishmentSystemInfo, relatedIdentities, formCollection, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                                contentInfo.NodeID = this.nodeInfo.NodeID;
                                contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                                contentInfo.AddUserName = AdminManager.Current.UserName;
                                contentInfo.AddDate = DateTime.Now;
                                contentInfo.LastEditUserName = contentInfo.AddUserName;
                                contentInfo.LastEditDate = contentInfo.AddDate;

                                contentInfo.CheckedLevel = TranslateUtils.ToIntWithNagetive(this.rblContentLevel.SelectedValue);
                                contentInfo.IsChecked = contentInfo.CheckedLevel >= base.PublishmentSystemInfo.CheckContentLevel;

                                int contentID = DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);

                                if (contentInfo.IsChecked)
                                {
                                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeInfo.NodeID, contentInfo.ID, 0);
                                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                }
                            }
                        }
                    }
                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
