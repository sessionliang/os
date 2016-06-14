using System;
using System.Web;
using BaiRong.Core;
using System.Text;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class PageDataUtils
    {
        private PageDataUtils()
        {
        }

        public static int PageIndex = 1;//当前页下标，默认第一页
        public static int PrePageNum = 15;//每一页多少条，默认15条
        public static string Container = "li";//链接容器
        public static string CssCurrent = "current";//当前页的样式
        public static int PageNumCount = 10;//显示链接个数，默认10个：1 2 3 4 5 6 7 8 9 10 ...


        public static string ParsePageHtml(int pageIndex, int prePageNum, int total, string url, string container, string cssCurrent, string cssNoCurrent, int pageNumCount)
        {
            StringBuilder sbHtml = new StringBuilder();
            try
            {
                int pageNum = (total % prePageNum == 0) ? total / prePageNum : ((total / prePageNum) + 1);//多少页
                if (total <= 0)
                    return "";
                if (pageIndex <= 0)
                    pageIndex = PageIndex;
                if (pageIndex > total)
                    pageIndex = total;
                if (prePageNum <= 0)
                    prePageNum = PrePageNum;
                if (string.IsNullOrEmpty(container))
                    container = Container;
                if (string.IsNullOrEmpty(cssCurrent))
                    cssCurrent = CssCurrent;
                if (pageNumCount <= 0)
                    pageNumCount = PageNumCount;
                if (pageNumCount > pageNum)
                    pageNumCount = pageNum;

                int start = 1;
                int end = pageNum;
                CalculateNumRange(pageIndex, pageNum, out start, out end);

                for (int i = start; i <= end; i++)
                {
                    if (i != pageIndex)
                        sbHtml.AppendFormat("<{3} class='{4}'><a href='{0}?pageIndex={1}&prePageNum={2}'>{5}</a></{3}>", url, pageIndex, prePageNum, container, cssCurrent, i);
                    else
                        sbHtml.AppendFormat("<{3} class='{4} {5}'><a href='{0}?pageIndex={1}&prePageNum={2}'>{6}</a></{3}>", url, pageIndex, prePageNum, container, cssNoCurrent, cssCurrent, i);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return sbHtml.ToString();
        }

        private static void CalculateNumRange(int pageIndex, int pageNum, out int startIndex, out int endIndex)
        {
            int range = PageNumCount / 2;
            if (pageIndex > 0 && pageIndex <= 5)
            {
                startIndex = 1;
                endIndex = pageNum > 10 ? 10 : pageNum;
            }
            else if (pageIndex > 5 && pageIndex < pageNum - 5)
            {
                startIndex = pageIndex - 5 + 1;
                endIndex = pageIndex + 5;
            }
            else
            {
                startIndex = pageNum - 10 + 1;
                endIndex = pageNum;
            }

            if (startIndex < 0)
                startIndex = 0;
            if (endIndex < 0)
                endIndex = 0;
            //if (pageIndex - range <= 0)
            //{
            //    startIndex = 1;
            //    endIndex = PageNumCount > pageNum ? pageNum : PageNumCount;
            //}
            //else if (pageIndex - range <= range) {
            //    startIndex = 1;
            //    endIndex = pageIndex;
            //}
            //else
            //{
            //    startIndex = pageNum - range;
            //    endIndex = PageNumCount > pageNum ? pageNum : PageNumCount;
            //}
            //else
            //{
            //    startIndex = pageIndex - range + 1;
            //    endIndex = pageIndex + range;
            //}
        }

        public static string ParsePageHtml(int pageIndex, int prePageNum, int total, int pageNumCount)
        {
            return ParsePageHtml(pageIndex, prePageNum, total, string.Empty, string.Empty, CssCurrent, string.Empty, pageNumCount);
        }

        public static string ParsePageHtml(int pageIndex, int prePageNum, int total)
        {
            return ParsePageHtml(pageIndex, prePageNum, total, string.Empty, string.Empty, CssCurrent, string.Empty, PageNumCount);
        }

        public static string ParsePageJson(int pageIndex, int prePageNum, int total)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("{");
            try
            {
                if (total <= 0)
                    return "";
                if (pageIndex <= 0)
                    pageIndex = PageIndex;
                if (pageIndex > total)
                    pageIndex = total;
                if (prePageNum <= 0)
                    prePageNum = PrePageNum;

                sbHtml.Append("\"list\":[");
                int pageNum = (total % prePageNum == 0) ? total / prePageNum : ((total / prePageNum) + 1);//多少页
                int start = 1;
                int end = pageNum;
                CalculateNumRange(pageIndex, pageNum, out start, out end);

                for (int i = start; i <= end; i++)
                {
                    sbHtml.AppendFormat("{0},", i);
                }
                if (pageNum > 0)
                    sbHtml.Length = sbHtml.Length - 1;
                sbHtml.Append("],");
                sbHtml.AppendFormat("\"pageIndex\":{0},", pageIndex);
                sbHtml.AppendFormat("\"prePageNum\":{0},", prePageNum);
                sbHtml.AppendFormat("\"total\":{0},", total);
                sbHtml.AppendFormat("\"first\":{0},", 1);
                sbHtml.AppendFormat("\"last\":{0},", pageNum);
                sbHtml.AppendFormat("\"pre\":{0},", pageIndex - 1 > 0 ? pageIndex - 1 : 1);
                sbHtml.AppendFormat("\"next\":{0}", pageIndex + 1 < pageNum ? pageIndex + 1 : pageNum);
            }
            catch (Exception)
            {
                throw;
            }
            sbHtml.Append("}");
            return sbHtml.ToString();
        }

    }
}
