using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.B2C.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Model;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class ConsultationSetting : BackgroundBasePage
    {
        protected DropDownList ddlStatus;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityB2C.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_consultationSetting.aspx", arguments, "IDCollection", "请选择需要设置的咨询！", 500, 350);
        }

        public static string GetShowPopWinString(int consultationID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", consultationID.ToString());
            return PageUtilityB2C.GetOpenWindowString("设置属性", "modal_consultationSetting.aspx", arguments, 500, 350);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("IDCollection"));

            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("<<保持不变>>", "0");
                this.ddlStatus.Items.Add(listItem);
                EBooleanUtils.AddListItems(this.ddlStatus, "已关闭", "处理中");

                listItem = new ListItem("<<保持不变>>", string.Empty);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                string status = this.ddlStatus.SelectedValue;
                foreach (int consultationID in this.idArrayList)
                {
                    ConsultationInfo consultationInfo = DataProviderB2C.ConsultationDAO.GetConsultationInfo(consultationID);
                    if (!string.IsNullOrEmpty(status))
                    {
                        consultationInfo.IsReply = TranslateUtils.ToBool(status);
                    }
                    DataProviderB2C.ConsultationDAO.Update(consultationInfo);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

    }
}
