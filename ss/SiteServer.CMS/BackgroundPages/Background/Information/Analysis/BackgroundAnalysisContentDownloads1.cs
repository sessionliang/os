using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;


using BaiRong.Controls;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisContentDownloads1 : BackgroundBasePage
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
        public readonly Hashtable YHashtableDownload = new Hashtable();
        //y轴类型
        public const string YType_Download = "YType_Download";
        //其他类型
        public const int YType_Other = -1;
        /// <summary>
        /// 设置x轴数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <param name="title"></param>
        public void SetXHashtable(int contentID, string title)
        {
            if (contentID == YType_Other)
            {
                if (!XHashtable.ContainsKey(YType_Other))
                {
                    XHashtable.Add(YType_Other, "其他");
                }
            }
            else if (!XHashtable.ContainsKey(contentID))
            {
                XHashtable.Add(contentID, title);
            }
            if (!keyArrayList.Contains(contentID))
            {
                keyArrayList.Add(contentID);
            }
            //keyArrayList.Sort();
            //keyArrayList.Reverse();
        }
        /// <summary>
        /// 获取x轴数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public string GetXHashtable(int contentID)
        {
            if (XHashtable.ContainsKey(contentID))
            {
                return XHashtable[contentID].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 设置y轴数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public void SetYHashtable(int contentID, int value)
        {

            if (!YHashtableDownload.ContainsKey(contentID))
            {
                YHashtableDownload.Add(contentID, value);
            }
            else
            {
                int num = TranslateUtils.ToInt(YHashtableDownload[contentID].ToString());
                YHashtableDownload[contentID] = num + value;
            }
            SetVertical(YType_Download, value);

            SetHorizental(contentID, value);
        }
        /// <summary>
        /// 获取y轴数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public string GetYHashtable(int contentID)
        {
            if (YHashtableDownload.ContainsKey(contentID))
            {
                int num = TranslateUtils.ToInt(YHashtableDownload[contentID].ToString());
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

        private readonly Hashtable nodeNameNavigations = new Hashtable();


        protected override bool IsSaasForbidden { get { return true; } }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.BackgroundContentDAO.GetSelectCommendByDownloads(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemID);

            this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "文件下载量统计", AppManager.Platform.Permission.Platform_Site);

                this.lbMore.Attributes.Add("href", BackgroundAnalysisContentDownloads.GetRedirectUrl(base.PublishmentSystemID, this.PageUrl));
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
                    Literal ltlFileUrl = e.Item.FindControl("ltlFileUrl") as Literal;

                    BackgroundContentInfo contentInfo = new BackgroundContentInfo(e.Item.DataItem);

                    ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);

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

                    ltlFileUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, contentInfo.FileUrl), contentInfo.FileUrl);

                    #region 绑定图标信息
                    //x轴信息
                    SetXHashtable(contentInfo.ID, contentInfo.Title);
                    //y轴信息
                    SetYHashtable(contentInfo.ID
                                               , CountManager.GetCount(AppManager.CMS.AppID, base.PublishmentSystemInfo.AuxiliaryTableForContent, contentInfo.ID.ToString(), ECountType.Download)
                                               );
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
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisContentDownloads1.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }
    }
}
