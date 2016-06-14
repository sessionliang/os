using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.STL.Parser.StlElement;

using SiteServer.CMS.Core;
using System.Collections.Specialized;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Services
{
    public class ResumePreview : BasePage
    {
        public Literal ltlRealName;
        public Literal ltlNationality;
        public Literal ltlGender;
        public Literal ltlEmail;
        public Literal ltlMobilePhone;
        public Literal ltlHomePhone;
        public Literal ltlLastSchoolName;
        public Literal ltlEducation;
        public Literal ltlIDCardType;
        public Literal ltlIDCardNo;
        public Literal ltlBirthday;
        public Literal ltlMarriage;
        public Literal ltlWorkYear;
        public Literal ltlProfession;
        public Literal ltlExpectSalary;
        public Literal ltlAvailabelTime;
        public Literal ltlLocation;
        public Literal ltlImageUrl;
        public Literal ltlSummary;

        public Repeater rptExp;
        public Repeater rptPro;
        public Repeater rptEdu;
        public Repeater rptTra;
        public Repeater rptLan;
        public Repeater rptSki;
        public Repeater rptCer;

        private ResumeContentInfo contentInfo;

        public void Page_Load(object sender, System.EventArgs e)
        {
            int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
            if (contentID > 0)
            {
                this.contentInfo = DataProvider.ResumeContentDAO.GetContentInfo(contentID);
                if (this.contentInfo != null)
                {
                    DataProvider.ResumeContentDAO.SetIsView(TranslateUtils.ToArrayList(contentID), true);
                }
            }
            else
            {
                this.contentInfo = DataProvider.ResumeContentDAO.GetContentInfo(base.PublishmentSystemID, 0, base.Request.Form);
            }

            this.ltlRealName.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.RealName);
            this.ltlNationality.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Nationality);
            this.ltlGender.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Gender);
            this.ltlEmail.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Email);
            this.ltlMobilePhone.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.MobilePhone);
            this.ltlHomePhone.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.HomePhone);
            this.ltlLastSchoolName.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.LastSchoolName);
            this.ltlEducation.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Education);
            this.ltlIDCardType.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.IDCardType);
            this.ltlIDCardNo.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.IDCardNo);
            this.ltlBirthday.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Birthday);
            this.ltlMarriage.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Marriage);
            this.ltlWorkYear.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.WorkYear);
            this.ltlProfession.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Profession);
            this.ltlExpectSalary.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.ExpectSalary);
            this.ltlAvailabelTime.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.AvailabelTime);
            this.ltlLocation.Text = this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Location);
            
            if (!string.IsNullOrEmpty(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.ImageUrl)))
            {
                this.ltlImageUrl.Text = string.Format(@"<img src=""{0}"" width=""120"" height=""150"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.ImageUrl)));
            }
            else
            {
                this.ltlImageUrl.Text = @"<img src=""images/resume_picture.jpg"" width=""120"" height=""150"" />";
            }
            this.ltlSummary.Text = StringUtils.ReplaceNewlineToBR(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Summary));

            int count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_Count));
            int[] array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptExp.DataSource = array;
            this.rptExp.ItemDataBound += new RepeaterItemEventHandler(rptExp_ItemDataBound);
            this.rptExp.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptPro.DataSource = array;
            this.rptPro.ItemDataBound += new RepeaterItemEventHandler(rptPro_ItemDataBound);
            this.rptPro.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptEdu.DataSource = array;
            this.rptEdu.ItemDataBound += new RepeaterItemEventHandler(rptEdu_ItemDataBound);
            this.rptEdu.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptTra.DataSource = array;
            this.rptTra.ItemDataBound += new RepeaterItemEventHandler(rptTra_ItemDataBound);
            this.rptTra.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Lan_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptLan.DataSource = array;
            this.rptLan.ItemDataBound += new RepeaterItemEventHandler(rptLan_ItemDataBound);
            this.rptLan.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Ski_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptSki.DataSource = array;
            this.rptSki.ItemDataBound += new RepeaterItemEventHandler(rptSki_ItemDataBound);
            this.rptSki.DataBind();

            count = TranslateUtils.ToInt(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Cer_Count));
            array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = i + 1;
            }
            this.rptCer.DataSource = array;
            this.rptCer.ItemDataBound += new RepeaterItemEventHandler(rptCer_ItemDataBound);
            this.rptCer.DataBind();
        }

        private void rptExp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlExp_Date = e.Item.FindControl("ltlExp_Date") as Literal;
                Literal ltlExp_EmployerName = e.Item.FindControl("ltlExp_EmployerName") as Literal;
                Literal ltlExp_Department = e.Item.FindControl("ltlExp_Department") as Literal;
                Literal ltlExp_EmployerPhone = e.Item.FindControl("ltlExp_EmployerPhone") as Literal;
                Literal ltlExp_WorkPlace = e.Item.FindControl("ltlExp_WorkPlace") as Literal;
                Literal ltlExp_PositionTitle = e.Item.FindControl("ltlExp_PositionTitle") as Literal;
                Literal ltlExp_Industry = e.Item.FindControl("ltlExp_Industry") as Literal;
                Literal ltlExp_Summary = e.Item.FindControl("ltlExp_Summary") as Literal;
                Literal ltlExp_Score = e.Item.FindControl("ltlExp_Score") as Literal;

                int fromYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_FromYear), index));
                int fromMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_FromMonth), index));
                int toYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_ToYear), index), DateTime.Now.Year);
                int toMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_ToMonth), index), DateTime.Now.Month);

                if (fromYear > 0 && fromMonth > 0)
                {
                    ltlExp_Date.Text = string.Format("{0}年{1}月 到 {2}年{3}月", fromYear, fromMonth, toYear, toMonth);
                }

                ltlExp_EmployerName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_EmployerName), index);
                ltlExp_Department.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_Department), index);
                ltlExp_EmployerPhone.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_EmployerPhone), index);
                ltlExp_WorkPlace.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_WorkPlace), index);
                ltlExp_PositionTitle.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_PositionTitle), index);
                ltlExp_Industry.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_Industry), index);
                ltlExp_Summary.Text = StringUtils.ReplaceNewlineToBR(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_Summary), index));
                ltlExp_Score.Text = StringUtils.ReplaceNewlineToBR(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Exp_Score), index));
            }
        }

        private void rptPro_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlPro_Date = e.Item.FindControl("ltlPro_Date") as Literal;
                Literal ltlPro_ProjectName = e.Item.FindControl("ltlPro_ProjectName") as Literal;
                Literal ltlPro_Summary = e.Item.FindControl("ltlPro_Summary") as Literal;

                int fromYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_FromYear), index));
                int fromMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_FromMonth), index));
                int toYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_ToYear), index), DateTime.Now.Year);
                int toMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_ToMonth), index), DateTime.Now.Month);

                if (fromYear > 0 && fromMonth > 0)
                {
                    ltlPro_Date.Text = string.Format("{0}年{1}月 到 {2}年{3}月", fromYear, fromMonth, toYear, toMonth);
                }

                ltlPro_ProjectName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_ProjectName), index);
                ltlPro_Summary.Text = StringUtils.ReplaceNewlineToBR(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Pro_Summary), index));
            }
        }

        private void rptEdu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlEdu_Date = e.Item.FindControl("ltlEdu_Date") as Literal;
                Literal ltlEdu_SchoolName = e.Item.FindControl("ltlEdu_SchoolName") as Literal;
                Literal ltlEdu_Education = e.Item.FindControl("ltlEdu_Education") as Literal;
                Literal ltlEdu_Profession = e.Item.FindControl("ltlEdu_Profession") as Literal;
                Literal ltlEdu_Summary = e.Item.FindControl("ltlEdu_Summary") as Literal;

                int fromYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_FromYear), index));
                int fromMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_FromMonth), index));
                int toYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_ToYear), index), DateTime.Now.Year);
                int toMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_ToMonth), index), DateTime.Now.Month);

                if (fromYear > 0 && fromMonth > 0)
                {
                    ltlEdu_Date.Text = string.Format("{0}年{1}月 到 {2}年{3}月", fromYear, fromMonth, toYear, toMonth);
                }

                ltlEdu_SchoolName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_SchoolName), index);
                ltlEdu_Education.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_Education), index);
                ltlEdu_Profession.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_Profession), index);
                ltlEdu_Summary.Text = StringUtils.ReplaceNewlineToBR(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Edu_Summary), index));
            }
        }

        private void rptTra_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlTra_Date = e.Item.FindControl("ltlTra_Date") as Literal;
                Literal ltlTra_TrainerName = e.Item.FindControl("ltlTra_TrainerName") as Literal;
                Literal ltlTra_TrainerAddress = e.Item.FindControl("ltlTra_TrainerAddress") as Literal;
                Literal ltlTra_Lesson = e.Item.FindControl("ltlTra_Lesson") as Literal;
                Literal ltlTra_Centification = e.Item.FindControl("ltlTra_Centification") as Literal;
                Literal ltlTra_Summary = e.Item.FindControl("ltlTra_Summary") as Literal;

                int fromYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_FromYear), index));
                int fromMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_FromMonth), index));
                int toYear = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_ToYear), index), DateTime.Now.Year);
                int toMonth = TranslateUtils.ToInt(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_ToMonth), index), DateTime.Now.Month);

                if (fromYear > 0 && fromMonth > 0)
                {
                    ltlTra_Date.Text = string.Format("{0}年{1}月 到 {2}年{3}月", fromYear, fromMonth, toYear, toMonth);
                }

                ltlTra_TrainerName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_TrainerName), index);
                ltlTra_TrainerAddress.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_TrainerAddress), index);
                ltlTra_Lesson.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_Lesson), index);
                ltlTra_Centification.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_Centification), index);
                ltlTra_Summary.Text = StringUtils.ReplaceNewlineToBR(ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Tra_Summary), index));
            }
        }

        private void rptLan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlLan_Language = e.Item.FindControl("ltlLan_Language") as Literal;
                Literal ltlLan_Level = e.Item.FindControl("ltlLan_Level") as Literal;

                ltlLan_Language.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Lan_Language), index);
                ltlLan_Level.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Lan_Level), index);
            }
        }

        private void rptSki_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlSki_SkillName = e.Item.FindControl("ltlSki_SkillName") as Literal;
                Literal ltlSki_UsedTimes = e.Item.FindControl("ltlSki_UsedTimes") as Literal;
                Literal ltlSki_Ability = e.Item.FindControl("ltlSki_Ability") as Literal;

                ltlSki_SkillName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Ski_SkillName), index);
                ltlSki_UsedTimes.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Ski_UsedTimes), index);
                ltlSki_Ability.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Ski_Ability), index);
            }
        }

        private void rptCer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = (int)e.Item.DataItem;

                Literal ltlCer_CertificationName = e.Item.FindControl("ltlCer_CertificationName") as Literal;
                Literal ltlCer_EffectiveDate = e.Item.FindControl("ltlCer_EffectiveDate") as Literal;

                ltlCer_CertificationName.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Cer_CertificationName), index);
                ltlCer_EffectiveDate.Text = ResumeContentAttribute.GetAttributeValue(this.contentInfo.GetExtendedAttribute(ResumeContentAttribute.Cer_EffectiveDate), index);
            }
        }
    }
}
