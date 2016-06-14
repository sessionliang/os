using System;
using System.Text;
using BaiRong.Core;
using System.Web;
using System.Collections.Specialized;

namespace SiteServer.BBS.Model
{
    public class PagerInfo
    {
        private string m_ID = string.Empty;
        private int m_PageNumber = -1;
        private int m_PageSize = 10;
        private int m_TotalRecords;
        private int m_ReduceCount;
        private string m_UrlFormat = string.Empty;
        private string m_Ajaxloader = string.Empty;
        private string m_AjaxPanelID = string.Empty;//",";
        private int m_ButtonCount = 10;

        public PagerInfo()
        {

        }

        public static PagerInfo GetPagerInfo(int totalCount, int pageNum, HttpRequest request, string urlFormat)
        {
            if (totalCount <= pageNum)
            {
                return null;
            }
            PagerInfo pagerInfo = new PagerInfo();

            pagerInfo.TotalRecords = totalCount;
            pagerInfo.PageSize = pageNum;

            string url = request.RawUrl;
            NameValueCollection queryString = PageUtils.GetQueryString(url);
            queryString.Remove("page");

            if (string.IsNullOrEmpty(urlFormat))
            {
                if (url.IndexOf("?") == -1 || url.EndsWith("?"))
                {
                    pagerInfo.UrlFormat = PageUtils.AddQueryString(url, queryString);
                }
                else
                {
                    pagerInfo.UrlFormat = PageUtils.AddQueryString(PageUtils.GetUrlWithoutQueryString(url), queryString);
                }
                pagerInfo.UrlFormat = PageUtils.AddQuestionOrAndToUrl(pagerInfo.UrlFormat) + "page={0}";
            }
            else
            {
                pagerInfo.UrlFormat = urlFormat;
            }

            pagerInfo.PageNumber = TranslateUtils.ToInt(request.QueryString["page"], 1);
            return pagerInfo;
        }

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int PageNumber
        {
            get
            {
                //if (m_PageNumber < 1)
                //    m_PageNumber = 1;

                //else if (m_PageNumber > PageCount)
                if (m_PageNumber > PageCount)
                    m_PageNumber = PageCount;

                return m_PageNumber;
            }
            set { m_PageNumber = value; }
        }

        public int PageSize
        {
            get { return m_PageSize; }
            set { m_PageSize = value; }
        }

        public int TotalRecords
        {
            get { return m_TotalRecords; }
            set { m_TotalRecords = value; }
        }

        /// <summary>
        /// TotalRecords - ReduceCount = 拿来分页的记录数
        /// 例如 帖子列表页 要把总置顶的排除
        /// </summary>
        public int ReduceCount
        {
            get { return m_ReduceCount; }
            set { m_ReduceCount = value; }
        }

        public string UrlFormat
        {
            get
            {
                if (string.IsNullOrEmpty(m_UrlFormat))
                {
                    string rawUrl = HttpContext.Current.Request.RawUrl;
                    string urlFormat = PageUtils.GetUrlWithoutQueryString(rawUrl);
                    NameValueCollection queryString = PageUtils.GetQueryString(rawUrl);
                    queryString.Remove("page");
                    queryString.Add("page", "{0}");

                    m_UrlFormat = urlFormat + "?" + TranslateUtils.NameValueCollectionToString(queryString);
                }
                return m_UrlFormat;
            }
            set { m_UrlFormat = value; }
        }

        public string AjaxLoader
        {
            get { return m_Ajaxloader; }
            set { m_Ajaxloader = value; }
        }

        public string AjaxPanelID
        {
            get { return m_AjaxPanelID; }
            set { m_AjaxPanelID = value; }
        }

        public string GetUrl(int page)
        {
            return string.Format(UrlFormat, page);
        }

        public int PageCount
        {
            get
            {
                int pageCount;
                int records = TotalRecords - ReduceCount;
                if (records < 1)
                    pageCount = 1;
                else
                    pageCount = (int)Math.Ceiling((double)records / (double)PageSize);
                return pageCount;
            }
        }

        public int ButtonCount
        {
            get { return m_ButtonCount; }
            set { m_ButtonCount = value; }
        }

        public int Page
        {
            get { return PageNumber; }
        }

        private int m_Start = -1;
        public int Start
        {
            get
            {
                if (m_Start == -1)
                {
                    setPageNumber();
                }
                return m_Start;
            }
        }

        private int m_End = -1;
        public int End
        {
            get
            {
                if (m_End == -1)
                {
                    setPageNumber();
                }
                return m_End;
            }
        }

        private void setPageNumber()
        {
            int pageNumber = PageNumber;
            if (PageCount <= ButtonCount)
            {
                m_Start = 1;
                m_End = PageCount;
            }
            else
            {
                if (pageNumber > 3)
                {
                    m_Start = pageNumber - 2;
                    m_End = m_Start + ButtonCount - 1;
                }
                else
                {
                    m_Start = 1;
                    m_End = ButtonCount;
                }
                if (m_End > PageCount)
                {
                    m_Start = PageCount - ButtonCount + 1;
                    m_End = PageCount;
                }
            }
        }
    }
}
