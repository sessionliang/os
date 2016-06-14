using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SurveySetting : BackgroundBasePage
    {
        public Literal ltlTitles;
        public TextBox tbBeginDate;
        public TextBox tbEndDate;

        private int nodeID;
        private ArrayList contentIDs;
        private ETableStyle tableStyle;
        private ContentInfo contentInfo;
        private string returnUrl;
        private string tableName;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("调查问卷设置", "modal_trialApplySetting.aspx", arguments, "ContentIDCollection", "请选择需要设置调查问卷选项的内容！", 360, 350);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.contentIDs = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
            this.nodeID = base.GetIntQueryString("NodeID");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.returnUrl = base.GetQueryString("ReturnUrl");

            if (!IsPostBack)
            {
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

                if (this.contentIDs.Count == 1)
                {
                    string settingsXML = BaiRongDataProvider.ContentDAO.GetValue(tableName, TranslateUtils.ToInt(this.contentIDs[0].ToString()), ContentAttribute.SettingsXML);
                    NameValueCollection attributes = TranslateUtils.ToNameValueCollection(settingsXML);
                    this.tbBeginDate.Text = attributes[ContentAttribute.Survey_BeginDate];
                    this.tbEndDate.Text = attributes[ContentAttribute.Survey_EndDate];
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

            ArrayList list = DataProvider.ContentDAO.GetContentInfoArrayList(tableName, this.tableStyle, base.PublishmentSystemID, this.nodeID, this.contentIDs);
            ArrayList titles = new ArrayList();
            foreach (ContentInfo info in list)
            {
                string settingsXML = info.GetExtendedAttribute(ContentAttribute.SettingsXML);
                NameValueCollection attributes = TranslateUtils.ToNameValueCollection(settingsXML);
                attributes[ContentAttribute.Survey_BeginDate] = this.tbBeginDate.Text;
                attributes[ContentAttribute.Survey_EndDate] = this.tbEndDate.Text;
                titles.Add(info.Title);
                info.SetExtendedAttribute(ContentAttribute.SettingsXML, TranslateUtils.NameValueCollectionToString(attributes));
            }
            DataProvider.ContentDAO.UpdateSettingXML(tableName, base.PublishmentSystemID, this.nodeID, list);
            StringUtility.AddLog(base.PublishmentSystemID, "调查问卷设置", string.Format("内容:{0}", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(titles)));

            JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page);
        }

    }
}
