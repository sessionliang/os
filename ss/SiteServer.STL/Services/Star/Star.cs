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
    public class Star : BasePage
    {
        private string theme;

        public Literal ltlNum1;
        public Literal ltlNum2;
        public Repeater rpStars;
        public Literal ltlTotalCount;
        private int point;
        private int totalStar;
        private int initStar;
        private int updaterID;

        public void Page_Load(object sender, System.EventArgs e)
        {
            this.updaterID = int.Parse(base.Request.QueryString["updaterID"]);
            int channelID = int.Parse(base.Request.QueryString["channelID"]);
            int contentID = int.Parse(base.Request.QueryString["contentID"]);
            this.totalStar = int.Parse(base.Request.QueryString["totalStar"]);
            int theInitStar = int.Parse(base.Request.QueryString["initStar"]);
            this.theme = PageUtils.FilterXSS(base.Request.QueryString["theme"]);

            if (!IsPostBack)
            {
                bool isStar = TranslateUtils.ToBool(base.Request.QueryString["isStar"]);
                if (isStar)
                {
                    int thePoint = TranslateUtils.ToInt(base.Request.QueryString["point"]);
                    DataProvider.StarDAO.AddCount(base.PublishmentSystemID, channelID, contentID, BaiRongDataProvider.UserDAO.CurrentUserName, thePoint, string.Empty, DateTime.Now);
                }

                int[] counts = DataProvider.StarDAO.GetCount(base.PublishmentSystemID, channelID, contentID);
                int totalCount = counts[0];
                int totalPoint = counts[1];

                object[] totalCountAndPointAverage = DataProvider.StarSettingDAO.GetTotalCountAndPointAverage(base.PublishmentSystemID, contentID);
                int settingTotalCount = (int)totalCountAndPointAverage[0];
                decimal settingPointAverage = (decimal)totalCountAndPointAverage[1];
                if (settingTotalCount > 0 || settingPointAverage > 0)
                {
                    totalCount += settingTotalCount;
                    totalPoint += Convert.ToInt32(settingPointAverage * settingTotalCount);
                }

                decimal num = 0;
                if (totalCount > 0)
                {
                    num = Convert.ToDecimal(totalPoint) / Convert.ToDecimal(totalCount);
                    this.initStar = 0;
                }
                else
                {
                    this.initStar = theInitStar;
                    num = this.initStar;
                }

                if (num > this.totalStar)
                {
                    num = this.totalStar;
                }

                if (this.ltlNum1 != null && this.ltlNum2 != null)
                {
                    string numString = num.ToString();
                    if (numString.IndexOf('.') == -1)
                    {
                        numString += ".0";
                    }
                    this.ltlNum1.Text = numString.Substring(0, numString.IndexOf('.'));
                    this.ltlNum2.Text = numString.Substring(numString.IndexOf('.') + 1, 1);
                }
                this.point = (int)Math.Round(num);
                if (this.point > this.totalStar)
                {
                    this.point = this.totalStar;
                }

                if (this.ltlTotalCount != null)
                {
                    this.ltlTotalCount.Text = totalCount.ToString();
                }

                ArrayList arraylist = new ArrayList();
                for (int i = 0; i < this.totalStar; i++)
                {
                    arraylist.Add(i);
                }
                rpStars.DataSource = arraylist;
                rpStars.ItemDataBound += new RepeaterItemEventHandler(rpStars_ItemDataBound);
                rpStars.DataBind();
            }
        }

        void rpStars_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int currentNum = e.Item.ItemIndex + 1;
                Literal ltlStar = e.Item.FindControl("ltlStar") as Literal;

                string imageName = string.Format("{0}_off.gif", this.theme);
                if (currentNum <= this.point || (this.initStar > 0 && this.initStar >= currentNum))
                {
                    imageName = string.Format("{0}_on.gif", this.theme);
                }

                string clickString = StlTemplateManager.Star.GetClickString(currentNum, this.updaterID);

                ltlStar.Text = string.Format(@"<img id=""stl_star_item_{0}_{1}"" alt=""{1}"" onmouseover=""stlStarDraw({1}, {2}, {0}, '{3}', '{4}')"" onmouseout=""stlStarInit({2}, {0})"" oriSrc=""{4}/{5}"" src=""{4}/{5}"" onclick=""{6}""/>", this.updaterID, currentNum, this.totalStar, this.theme, PageUtility.Services.GetUrl("star"), imageName, clickString);
            }
        }
    }
}
