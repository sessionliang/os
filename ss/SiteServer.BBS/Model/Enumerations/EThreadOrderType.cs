using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;

namespace SiteServer.BBS.Model {

    public enum EThreadOrderType { 
        [Description("升序")]
        Asc,
        [Description("降序")]
        Desc
    }
    public enum EThreadOrderField {
        [Description("回复时间")]
        LastDate,
        [Description("发布时间")]
        AddDate,
        [Description("回复数量")]
        Replies,
        [Description("浏览次数")]
        Hits
    }
    public enum EThreadCheckType {
        [Description("不需要")]
        None,
        [Description("审核新主题")]
        OnlyThread,
        [Description("审核新主题和新回复")]
        ThreadAndPost 
    }
    public enum EThreadAutoCloseType {
        [Description("不自动关闭")]
        NoAutoClose,
        [Description("按发布时间自动关闭")]
        AutoCloseByAddDate,
        [Description("按最后回复时间自动关闭")]
        AutoCloseByLastReplyDate
    }
    public enum EThreadStateType {
        [Description("显示")]
        Normal,
        [Description("不显示")]
        Lock,
        [Description("删除")]
        Delete
    }
    public enum EThreadCategoryType
    {
        [Description("禁用分类")]
        No,
        [Description("允许分类")]
        Yes,
        [Description("强制分类")]
        Must
    }
    public enum EThreadCategoryDisplayType {
        [Description("不显示")]
        No,
        [Description("文字")]
        Text,
        [Description("图标")]
        Pic
    }
}
