using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundConsultationAnswer : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public PlaceHolder phAnswer;
        public TextBox answer;

        private ConsultationInfo consultationInfo;
        public GoodsContentInfo goodsContentInfo;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int channelID, int contentID, int id, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_consultationAnswer.aspx?publishmentSystemID={0}&channelID={1}&contentID={2}&id={3}&returnUrl={4}", publishmentSystemID, channelID, contentID, id, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = base.GetIntQueryString("ID");
            int channelID = base.GetIntQueryString("channelID");
            int contentID = base.GetIntQueryString("contentID");
            this.goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(base.PublishmentSystemInfo, channelID, contentID);
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundConsultation.GetRedirectUrl(base.PublishmentSystemID, true, string.Empty, string.Empty, 1);
            }

            this.consultationInfo = DataProviderB2C.ConsultationDAO.GetConsultationInfo(id);

            if (!IsPostBack)
            {
                this.ltlPageTitle.Text = this.consultationInfo.Question;
                this.answer.Text = this.consultationInfo.Answer;
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                this.consultationInfo.Answer = this.answer.Text;
                this.consultationInfo.ReplyDate = DateTime.Now;
                this.consultationInfo.IsReply = true;

                DataProviderB2C.ConsultationDAO.Update(this.consultationInfo);

                base.SuccessMessage("×ÉÑ¯»Ø¸´³É¹¦");

                base.AddWaitAndRedirectScript(BackgroundConsultationAnswer.GetRedirectUrl(base.PublishmentSystemID, this.goodsContentInfo.NodeID, this.goodsContentInfo.ID, this.consultationInfo.ID, this.returnUrl));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
