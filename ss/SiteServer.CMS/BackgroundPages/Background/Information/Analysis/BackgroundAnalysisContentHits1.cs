using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

using BaiRong.Model;

using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisContentHits1 : BackgroundBasePage
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
        public readonly Hashtable YHashtableHitsDay = new Hashtable();
        public readonly Hashtable YHashtableHitsWeek = new Hashtable();
        public readonly Hashtable YHashtableHitsMonth = new Hashtable();
        //y轴类型
        public const string YType_Hits = "YType_Hits";
        public const string YType_HitsDay = "YType_HitsDay";
        public const string YType_HitsWeek = "YType_HitsWeek";
        public const string YType_HitsMonth = "YType_HitsMonth";
        //其他类型
        public const int YType_Other = -1;
        /// <summary>
        /// 设置x轴数据
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="title"></param>
        public void SetXHashtable(int ContentID, string title)
        {
            if (ContentID == YType_Other)
            {
                if (!XHashtable.ContainsKey(YType_Other))
                {
                    XHashtable.Add(YType_Other, "其他");
                }
            }
            else if (!XHashtable.ContainsKey(ContentID))
            {
                XHashtable.Add(ContentID, title);
            }
            if (!keyArrayList.Contains(ContentID))
            {
                keyArrayList.Add(ContentID);
            }
            //keyArrayList.Sort();
            //keyArrayList.Reverse();
        }
        /// <summary>
        /// 获取x轴数据
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        public string GetXHashtable(int ContentID)
        {
            if (XHashtable.ContainsKey(ContentID))
            {
                return XHashtable[ContentID].ToString();
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
        public void SetYHashtable(int publishemtSystemID, int value, string yType)
        {
            switch (yType)
            {
                case YType_Hits:
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
                    break;
                case YType_HitsDay:
                    if (!YHashtableHitsDay.ContainsKey(publishemtSystemID))
                    {
                        YHashtableHitsDay.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsDay[publishemtSystemID].ToString());
                        YHashtableHitsDay[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_HitsDay, value);
                    break;
                case YType_HitsWeek:
                    if (!YHashtableHitsWeek.ContainsKey(publishemtSystemID))
                    {
                        YHashtableHitsWeek.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsWeek[publishemtSystemID].ToString());
                        YHashtableHitsWeek[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_HitsWeek, value);
                    break;
                case YType_HitsMonth:
                    if (!YHashtableHitsMonth.ContainsKey(publishemtSystemID))
                    {
                        YHashtableHitsMonth.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsMonth[publishemtSystemID].ToString());
                        YHashtableHitsMonth[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_HitsWeek, value);
                    break;
                default:
                    break;
            }
            SetHorizental(publishemtSystemID, value);
        }
        /// <summary>
        /// 获取y轴数据
        /// </summary>
        /// <param name="publishemtSystemID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public string GetYHashtable(int publishemtSystemID, string yType)
        {
            switch (yType)
            {
                case YType_Hits:
                    if (YHashtableHits.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableHits[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_HitsDay:
                    if (YHashtableHitsDay.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsDay[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_HitsWeek:
                    if (YHashtableHitsWeek.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsWeek[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_HitsMonth:
                    if (YHashtableHitsMonth.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableHitsMonth[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                default:
                    return "0";
            }
        }
        /// <summary>
        /// 设置y总数
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="num"></param>
        public void SetHorizental(int ContentID, int num)
        {
            if (HorizentalHashtable[ContentID] == null)
            {
                HorizentalHashtable[ContentID] = num;
            }
            else
            {
                int totalNum = (int)HorizentalHashtable[ContentID];
                HorizentalHashtable[ContentID] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取y总数
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        public string GetHorizental(int ContentID)
        {
            if (HorizentalHashtable[ContentID] != null)
            {
                int num = TranslateUtils.ToInt(HorizentalHashtable[ContentID].ToString());
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
        /// <param name="ContentID"></param>
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
        /// <param name="ContentID"></param>
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


        public Repeater rptContents;
        public SqlPager spContents;
        public LinkButton lbMore;

        private string pageUrl;
        private readonly Hashtable nodeNameNavigations = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommend(base.PublishmentSystemInfo.AuxiliaryTableForContent, nodeIDArrayList, ETriState.True);

            this.spContents.SortField = ContentAttribute.Hits;
            this.spContents.SortMode = SortMode.DESC;

            this.pageUrl = PageUtils.GetCMSUrl("background_AnalysisContentHits1.aspx?publishmentSystemID=" + base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "内容点击量排名", AppManager.CMS.Permission.WebSite.SiteAnalysis);
                this.lbMore.Attributes.Add("href", BackgroundAnalysisContentHits.GetRedirectUrl(base.PublishmentSystemID, this.PageUrl));
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //展示默认条数
                if (e.Item.ItemIndex >= SHOW_COUNT)
                {
                    e.Item.Visible = false;
                }
                else
                {
                    Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                    Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                    Literal ltlHits = e.Item.FindControl("ltlHits") as Literal;
                    Literal ltlHitsByDay = e.Item.FindControl("ltlHitsByDay") as Literal;
                    Literal ltlHitsByWeek = e.Item.FindControl("ltlHitsByWeek") as Literal;
                    Literal ltlHitsByMonth = e.Item.FindControl("ltlHitsByMonth") as Literal;
                    Literal ltlLastHitsDate = e.Item.FindControl("ltlLastHitsDate") as Literal;

                    ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                    ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.pageUrl);

                    string nodeNameNavigation = string.Empty;
                    if (!nodeNameNavigations.ContainsKey(contentInfo.NodeID))
                    {
                        nodeNameNavigation = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                        nodeNameNavigations.Add(contentInfo.NodeID, nodeNameNavigation);
                    }
                    else
                    {
                        nodeNameNavigation = nodeNameNavigations[contentInfo.NodeID] as string;
                    }
                    ltlChannel.Text = nodeNameNavigation;

                    ltlHits.Text = contentInfo.Hits.ToString();
                    ltlHitsByDay.Text = contentInfo.HitsByDay.ToString();
                    ltlHitsByMonth.Text = contentInfo.HitsByMonth.ToString();
                    ltlHitsByWeek.Text = contentInfo.HitsByWeek.ToString();
                    ltlLastHitsDate.Text = DateUtils.GetDateAndTimeString(contentInfo.LastHitsDate);


                    #region 绑定图标信息
                    //x轴信息
                    SetXHashtable(contentInfo.ID, contentInfo.Title);
                    //y轴信息
                    SetYHashtable(contentInfo.ID
                                               , contentInfo.Hits
                                               , YType_Hits);
                    SetYHashtable(contentInfo.ID
                                                , contentInfo.HitsByDay
                                                , YType_HitsDay);
                    SetYHashtable(contentInfo.ID
                                                , contentInfo.HitsByWeek
                                                , YType_HitsWeek);
                    SetYHashtable(contentInfo.ID
                            , contentInfo.HitsByMonth
                            , YType_HitsMonth);
                    #endregion
                }
            }
        }
        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisContentHits1.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }
    }
}
