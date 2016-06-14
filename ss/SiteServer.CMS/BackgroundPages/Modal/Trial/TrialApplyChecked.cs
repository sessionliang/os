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
    public class TrialApplyChecked : BackgroundBasePage
    {
        public Literal ltlTitles;
        public RadioButtonList rblIsMobile;
        public RadioButtonList rblIsReport;
        public RadioButtonList rblIsChecked;
        public PlaceHolder phIsChecked;

        private int nodeID;
        private int contentID;
        private ArrayList taids;
        private ETableStyle tableStyle;
        private ContentInfo contentInfo;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("�����������", "modal_trialApplyChecked.aspx", arguments, "ContentIDCollection", "��ѡ����Ҫ��˵����������¼��", 360, 350);
        }

        public static string GetOneOpenWindowString(int publishmentSystemID, int nodeID, int contentID, int id, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ContentIDCollection", id.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowString("�����������", "modal_trialApplyChecked.aspx", arguments, 360, 350);
        }



        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.taids = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
            this.nodeID = base.GetIntQueryString("NodeID");
            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.returnUrl = base.GetQueryString("ReturnUrl");

            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            if (this.contentID != 0)
                contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblIsMobile, "��", "��");
                EBooleanUtils.AddListItems(this.rblIsReport, "��", "��");
                EBooleanUtils.AddListItems(this.rblIsChecked, "��", "��");
                this.rblIsMobile.SelectedValue = "False";
                this.rblIsReport.SelectedValue = "False";
                this.rblIsChecked.SelectedValue = "False";
                StringBuilder titles = new StringBuilder();
            }
        }

        public void rblIsChecked_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsChecked.Visible = TranslateUtils.ToBool(this.rblIsChecked.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            bool isChecked = TranslateUtils.ToBool(this.rblIsChecked.SelectedValue);
            bool isReport = TranslateUtils.ToBool(this.rblIsReport.SelectedValue);
            bool isMobile = TranslateUtils.ToBool(this.rblIsMobile.SelectedValue);
            string checkAdmin = AdminManager.Current.UserName;
            string applystatus = EFunctionStatusType.AuditNotPassed.ToString();

            if (isChecked)
                applystatus = EFunctionStatusType.AuditPassed.ToString();
            else
                applystatus = EFunctionStatusType.AuditNotPassed.ToString();

            DataProvider.TrialApplyDAO.ApplyChecked(base.PublishmentSystemID, this.nodeID, this.taids, true, applystatus, isReport, isMobile, checkAdmin, DateTime.Now);
            if (isMobile)
                setMobile();
            if (this.contentInfo != null)
                StringUtility.AddLog(base.PublishmentSystemID, "������ü�¼", string.Format(this.contentInfo.Title + "���ü�¼:{0}", TranslateUtils.ObjectCollectionToString(this.taids)));
            else
            {
                StringUtility.AddLog(base.PublishmentSystemID, "������ü�¼", string.Format("���ü�¼:{0}", TranslateUtils.ObjectCollectionToString(this.taids)));
            }


            JsUtils.OpenWindow.CloseModalPage(Page);
        }


        public void setMobile()
        { 
            ArrayList list = DataProvider.TrialApplyDAO.GetInfoList(base.PublishmentSystemID, this.nodeID, this.contentID, this.taids);

            ArrayList mobileArrayList = new ArrayList();
            foreach (TrialApplyInfo info in list)
            {
                if (string.IsNullOrEmpty(info.Phone))
                {
                    mobileArrayList.Add(info.Phone);
                }
            }

            string strBody = "��������������ͨ�������¼�û�������д���ñ��档 ��" + base.PublishmentSystemInfo.PublishmentSystemName + "��";

            string errorMessage = string.Empty;
            if (mobileArrayList.Count > 0)
            {
                bool isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);
            }
        }
    }
}
