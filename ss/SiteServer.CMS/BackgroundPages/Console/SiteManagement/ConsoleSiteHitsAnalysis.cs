using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;


using BaiRong.Controls;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleSiteHitsAnalysis : BackgroundBasePage
    {
        //默认展示前10条数据
        public int SHOW_COUNT = 10;
        //总数
        private readonly Hashtable HorizentalHashtable = new Hashtable();
        private readonly Hashtable VerticalHashtable = new Hashtable();
        //sort key
        public ArrayList keyArrayList = new ArrayList();
        //x
        public readonly Hashtable XHashtable = new Hashtable();
        //y
        public readonly Hashtable YHashtableHits = new Hashtable();
        //y轴类型
        public const string YType_Hits = "YType_Hits";
        //其他类型
        public const int YType_Other = -1;
        /// <summary>
        /// 设置x轴数据
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="publishmentSystemName"></param>
        public void SetXHashtable(int publishmentSystemID, string publishmentSystemName)
        {
            if (publishmentSystemID == YType_Other)
            {
                if (!XHashtable.ContainsKey(YType_Other))
                {
                    XHashtable.Add(YType_Other, "其他");
                }
            }
            else if (!XHashtable.ContainsKey(publishmentSystemID))
            {
                XHashtable.Add(publishmentSystemID, publishmentSystemName);
            }
            if (!keyArrayList.Contains(publishmentSystemID))
            {
                keyArrayList.Add(publishmentSystemID);
            }
            keyArrayList.Sort();
            keyArrayList.Reverse();
        }
        /// <summary>
        /// 获取x轴数据
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetXHashtable(int publishmentSystemID)
        {
            if (XHashtable.ContainsKey(publishmentSystemID))
            {
                return XHashtable[publishmentSystemID].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 设置y轴数据
        /// </summary>
        /// <param name="publishemtSystemID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public void SetYHashtable(int publishemtSystemID, int value)
        {

            if (!YHashtableHits.ContainsKey(publishemtSystemID))
            {
                YHashtableHits.Add(publishemtSystemID, value);
            }
            else
            {
                int num = TranslateUtils.ToInt(YHashtableHits[publishemtSystemID].ToString());
                YHashtableHits[publishemtSystemID] = num + value;
            }
            SetVertical(YType_Hits, value);

            SetHorizental(publishemtSystemID, value);
        }
        /// <summary>
        /// 获取y轴数据
        /// </summary>
        /// <param name="publishemtSystemID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public string GetYHashtable(int publishemtSystemID)
        {

            if (YHashtableHits.ContainsKey(publishemtSystemID))
            {
                int num = TranslateUtils.ToInt(YHashtableHits[publishemtSystemID].ToString());
                return num.ToString();
            }
            else
            {
                return "0";
            }

        }
        /// <summary>
        /// 设置y总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="num"></param>
        public void SetHorizental(int publishmentSystemID, int num)
        {
            if (HorizentalHashtable[publishmentSystemID] == null)
            {
                HorizentalHashtable[publishmentSystemID] = num;
            }
            else
            {
                int totalNum = (int)HorizentalHashtable[publishmentSystemID];
                HorizentalHashtable[publishmentSystemID] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取y总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetHorizental(int publishmentSystemID)
        {
            if (HorizentalHashtable[publishmentSystemID] != null)
            {
                int num = TranslateUtils.ToInt(HorizentalHashtable[publishmentSystemID].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 设置type总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="num"></param>
        public void SetVertical(string type, int num)
        {
            if (VerticalHashtable[type] == null)
            {
                VerticalHashtable[type] = num;
            }
            else
            {
                int totalNum = (int)VerticalHashtable[type];
                VerticalHashtable[type] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取type总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetVertical(string type)
        {
            if (VerticalHashtable[type] != null)
            {
                int num = TranslateUtils.ToInt(VerticalHashtable[type].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        public string GetVerticalTotalNum()
        {
            int totalNum = 0;
            foreach (int num in VerticalHashtable.Values)
            {
                totalNum += num;
            }
            return (totalNum == 0) ? "0" : string.Format("<strong>{0}</strong>", totalNum);
        }

        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public Repeater rpContents;
        public LinkButton lbMore;



        protected override bool IsSaasForbidden { get { return true; } }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.Platform.TopMenu.ID_Statistics, "应用访问量统计", AppManager.Platform.Permission.Platform_Statistics);

                this.StartDate.Text = DateUtils.GetDateAndTimeString(DateTime.Now.AddMonths(-1));
                this.EndDate.Now = true;
                this.lbMore.Attributes.Add("href", ConsoleSiteHitsAnalysisAll.GetRedirectUrl(this.PageUrl));

                BindGrid();
            }
        }

        public void BindGrid()
        {
            IEnumerator ie = DataProvider.PublishmentSystemDAO.GetDataSource().GetEnumerator();
            int counter = 0;
            while (ie.MoveNext())
            {
                int publishmentSystemID = TranslateUtils.EvalInt(ie.Current, "PublishmentSystemID");
                SiteServer.CMS.Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                int key = publishmentSystemInfo.PublishmentSystemID;
                if (counter >= SHOW_COUNT)
                {
                    key = -1;
                }
                //x轴信息
                SetXHashtable(key, publishmentSystemInfo.PublishmentSystemName);
                //y轴信息
                SetYHashtable(key
                                           , DataProvider.TrackingDAO.GetHitsCountOfPublishmentSystem(publishmentSystemInfo.PublishmentSystemID, TranslateUtils.ToDateTime(this.StartDate.Text), TranslateUtils.ToDateTime(this.EndDate.Text))
                                           );

                counter++;
            }

            this.rpContents.DataSource = DataProvider.PublishmentSystemDAO.GetDataSource();
            this.rpContents.ItemDataBound += new RepeaterItemEventHandler(rpContents_ItemDataBound);
            this.rpContents.DataBind();
        }

        void rpContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int publishmentSystemID = TranslateUtils.EvalInt(e.Item.DataItem, "PublishmentSystemID");
                //展示默认条数
                if (e.Item.ItemIndex >= SHOW_COUNT)
                {
                    e.Item.Visible = false;
                }
                else
                {
                    SiteServer.CMS.Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    Literal ltlPublishmentSystemName = e.Item.FindControl("ltlPublishmentSystemName") as Literal;
                    Literal ltlHitsNum = e.Item.FindControl("ltlHitsNum") as Literal;

                    ltlPublishmentSystemName.Text = publishmentSystemInfo.PublishmentSystemName + "&nbsp;" + EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType);
                    ltlHitsNum.Text = GetYHashtable(publishmentSystemID);
                }
            }
        }

        public void Analysis_OnClick(object sender, EventArgs E)
        {
            BindGrid();
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("console_siteHitsAnalysis.aspx"));
                }
                return _pageUrl;
            }
        }
    }
}
