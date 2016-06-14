using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;

namespace SiteServer.BBS.Model 
{

    /// <summary>
    /// 首页版块列表中在版块简介下方显示下级子版块名字和链接(如果存在的话)。
    /// </summary>
    public enum EForumSummaryType {

        [Description("默认")]
        Default,
        [Description("包含子版块名字和链接")]
        ContainSubForum,
        [Description("不包含子版块名字和链接")]
        NoSubForum
    }
}
