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



using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundConsultationAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public DropDownList type;
        public TextBox question;

        private string returnUrl;
        public ConsultationInfo consultationInfo;
        public GoodsContentInfo goodsContentInfo;

        public static string GetAddUrl(int publishmentSystemID, int channelID, int contentID, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_consultationAdd.aspx?publishmentSystemID={0}&channelID={1}&contentID={2}&returnUrl={3}", publishmentSystemID, channelID, contentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetEditUrl(int publishmentSystemID, int channelID, int contentID, int id, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_consultationAdd.aspx?publishmentSystemID={0}&channelID={1}&contentID={2}&id={3}&returnUrl={4}", publishmentSystemID, channelID, contentID, id, StringUtils.ValueToUrl(returnUrl)));
        }

        public override string GetValue(string attributeName)
        {
            string value = string.Empty;
            if (this.consultationInfo != null)
            {
                value = Convert.ToString(this.consultationInfo.GetValue(attributeName));
            }
            string retval = value;

            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = base.GetIntQueryString("ID");
            int channelID = base.GetIntQueryString("channelID");
            int contentID = base.GetIntQueryString("contentID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(base.PublishmentSystemInfo, channelID, contentID);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundConsultation.GetRedirectUrl(base.PublishmentSystemID, true, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.consultationInfo = DataProviderB2C.ConsultationDAO.GetConsultationInfo(id);

                this.ltlPageTitle.Text = "修改咨询";
            }
            else
            {
                this.ltlPageTitle.Text = "新增咨询";
            }

            if (!IsPostBack)
            {
                this.question.Text = this.GetValue(ConsultationAttribute.Question);
                EConsultationTypeUtils.AddListItems(this.type);
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

                if (this.consultationInfo != null)
                {
                    //ConsultationInfo consultationInfoToEdit = new ConsultationInfo(base.Request.Form);
                    this.consultationInfo.Question = question.Text;
                    this.consultationInfo.Type = this.type.SelectedValue;
                    DataProviderB2C.ConsultationDAO.Update(this.consultationInfo);
                    base.SuccessMessage("咨询修改成功");
                }
                else
                {

                    ConsultationInfo consultationInfoToAdd = new ConsultationInfo(base.Request.Form);
                    consultationInfoToAdd.IsReply = false;
                    if (this.goodsContentInfo != null)
                    {
                        consultationInfoToAdd.Title = goodsContentInfo.Title;
                        consultationInfoToAdd.ThumbUrl = goodsContentInfo.ThumbUrl;
                    }
                    this.consultationInfo.Type = this.type.SelectedValue;
                    consultationInfoToAdd.AddUser = AdminManager.Current.UserName;
                    consultationInfoToAdd.AddDate = DateTime.Now;

                    int consultationID = DataProviderB2C.ConsultationDAO.Insert(consultationInfoToAdd);

                    base.SuccessMessage("咨询添加成功");
                }

                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
