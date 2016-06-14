using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using SiteServer.Project.Core;
using BaiRong.Core;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundUrlCohort : BackgroundBasePage
    {
        public Literal ltlCategories;
        public Literal ltlSeries1;
        public Literal ltlSeries2;
        public Literal ltlSeries3;
        public Literal ltlSeries4;
        public Literal ltlSeries5;

        public void Page_Load(object sender, EventArgs E)
        {
            DateTime dateTime = TranslateUtils.ToDateTime("2013-5-1");

            StringBuilder categories = new StringBuilder();
            StringBuilder series1 = new StringBuilder();
            StringBuilder series2 = new StringBuilder();
            StringBuilder series3 = new StringBuilder();
            StringBuilder series4 = new StringBuilder();
            StringBuilder series5 = new StringBuilder();

            for (; dateTime <= DateTime.Now; dateTime = dateTime.AddMonths(1))
            {
                categories.AppendFormat("'{0}', ", dateTime.ToString("yyyy-MM"));
                Hashtable hashtable = DataProvider.UrlDAO.GetCountOfActivityHashtable(dateTime.Year, dateTime.Month);

                series1.AppendFormat("{0}, ", hashtable[0]);
                series2.AppendFormat("{0}, ", hashtable[1]);
                series3.AppendFormat("{0}, ", hashtable[2]);
                series4.AppendFormat("{0}, ", hashtable[3]);
                series5.AppendFormat("{0}, ", hashtable[4]);
            }


            if (categories.Length > 0) categories.Length -= 2;
            ltlCategories.Text = categories.ToString();

            if (series1.Length > 0) series1.Length -= 2;
            ltlSeries1.Text = series1.ToString();

            if (series2.Length > 0) series2.Length -= 2;
            ltlSeries2.Text = series2.ToString();

            if (series3.Length > 0) series3.Length -= 2;
            ltlSeries3.Text = series3.ToString();

            if (series4.Length > 0) series4.Length -= 2;
            ltlSeries4.Text = series4.ToString();

            if (series5.Length > 0) series5.Length -= 2;
            ltlSeries5.Text = series5.ToString();
        }
    }
}
