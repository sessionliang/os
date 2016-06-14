using SiteServer.B2C.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class PageItem
    {
        public bool PreviousPage { get; set; }				            //上一页
        public bool NextPage { get; set; }						            //下一页
        public bool FirstPage { get; set; }						        //首页
        public bool LastPage { get; set; }						            //末页
        public int CurrentPageIndex { get; set; }		            //当前页索引
        public int TotalPageNum { get; set; }		                    //总页数
        public int TotalNum { get; set; }		                            //总内容数
        public List<int> PageNavigation { get; set; }                     //页导航
        public const string PageSelect = "PageSelect";                              //页跳转
        public int PreviousPageIndex { get; set; }                      //上一页下标
        public int NextPageIndex { get; set; }                           //下一页下标

        //public const string Type_PreviousPage = "PreviousPage";				            //上一页
        //public const string Type_NextPage = "NextPage";						            //下一页
        //public const string Type_FirstPage = "FirstPage";						        //首页
        //public const string Type_LastPage = "LastPage";						            //末页
        //public const string Type_CurrentPageIndex = "CurrentPageIndex";		            //当前页索引
        //public const string Type_TotalPageNum = "TotalPageNum";		                    //总页数
        //public const string Type_TotalNum = "TotalNum";		                            //总内容数
        //public const string Type_PageNavigation = "PageNavigation";			            //页导航
        //public const string Type_PageSelect = "PageSelect";			                    //页跳转

    }
}