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
using SiteServer.STL.StlTemplate;

namespace SiteServer.CMS.Services
{
    public class Digg : BasePage
    {
        public PlaceHolder phGood;
        public Literal ltlGoodText;
        public Literal ltlGoodNum;

        public PlaceHolder phBad;        
        public Literal ltlBadText;        
        public Literal ltlBadNum;

        private string clickStringOfGood;
        private string clickStringOfBad;
        private decimal goodPercentage;
        private decimal badPercentage;

        public string ClickStringOfGood { get { return this.clickStringOfGood; } }
        public string ClickStringOfBad { get { return this.clickStringOfBad; } }
        public decimal GoodPercentage { get { return this.goodPercentage; } }
        public decimal BadPercentage { get { return this.badPercentage; } }

        public void Page_Load(object sender, System.EventArgs e)
        {
            int updaterID = int.Parse(base.Request.QueryString["updaterID"]);
            int relatedIdentity = int.Parse(base.Request.QueryString["relatedIdentity"]);
            EDiggType diggType = EDiggTypeUtils.GetEnumType(base.Request.QueryString["type"]);
            if (diggType == EDiggType.Good)
            {
                this.phBad.Visible = false;
            }
            else if (diggType == EDiggType.Bad)
            {
                this.phGood.Visible = false;
            }

            this.clickStringOfGood = StlTemplateManager.Digg.GetClickString(true, updaterID);
            this.clickStringOfBad = StlTemplateManager.Digg.GetClickString(false, updaterID);

            bool isCrossSite = TranslateUtils.ToBool(base.Request.QueryString["isCrossSite"]);

            if (this.ltlGoodText != null)
            {
                this.ltlGoodText.Text = RuntimeUtils.DecryptStringByTranslate(base.Request["goodText"]);
            }
            if (this.ltlBadText != null)
            {
                this.ltlBadText.Text = RuntimeUtils.DecryptStringByTranslate(base.Request["badText"]);
            }

            bool isDigg = TranslateUtils.ToBool(base.Request.QueryString["isDigg"]);
            if (isDigg)
            {
                bool isGood = TranslateUtils.ToBool(base.Request.QueryString["isGood"]);
                BaiRongDataProvider.DiggDAO.AddCount(base.PublishmentSystemID, relatedIdentity, isGood);
            }

            int[] counts = BaiRongDataProvider.DiggDAO.GetCount(base.PublishmentSystemID, relatedIdentity);
            int goodNum = counts[0];
            int badNum = counts[1];

            if (this.ltlGoodNum != null)
            {
                this.ltlGoodNum.Text = goodNum.ToString();
            }
            if (this.ltlBadNum != null)
            {
                this.ltlBadNum.Text = badNum.ToString();
            }

            if (goodNum == 0 && badNum == 0)
            {
                this.goodPercentage = 0;
                this.badPercentage = 0;
            }
            else if (goodNum > 0 && badNum == 0)
            {
                this.goodPercentage = 100;
                this.badPercentage = 0;
            }
            else if (goodNum == 0 && badNum > 0)
            {
                this.goodPercentage = 0;
                this.badPercentage = 100;
            }
            else
            {
                this.goodPercentage = Math.Round((Convert.ToDecimal(goodNum) / Convert.ToDecimal(goodNum + badNum)) * Convert.ToDecimal(100));
                this.badPercentage = 100 - this.goodPercentage;
            }
        }
    }
}
