using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Model
{
    public class ForumInfoExtend : ExtendedAttributes
    {
        public ForumInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        //----------------系统设置------------------------

        /// <summary>
        /// 是否只显示下级子版块
        /// </summary>
        public bool IsOnlyDisplaySubForums
        {
            get {
                return base.GetBool("IsOnlyDisplaySubForums", false);
            }
            set {
                base.SetExtendedAttribute("IsOnlyDisplaySubForums", value.ToString());
            }
        }  
        /// <summary>
        /// 是否显示边栏
        /// </summary>
        public bool IsDisplayForumInfo
        {
            get {
                return base.GetBool("IsDisplayForumInfo", false);
            }
            set {
                base.SetExtendedAttribute("IsDisplayForumInfo", value.ToString());
            }
        }
        /// <summary>
        /// 本版块在首页版块列表中在版块简介下方显示下级子版块名字和链接(如果存在的话)
        /// </summary>
        public string ForumSummaryType {
            get {
                return base.GetString("ForumSummaryType", EForumSummaryType.Default.ToString());
            }
            set {
                base.SetExtendedAttribute("ForumSummaryType", value);
            }
        }  
        /// <summary>
        /// 是否在本版显示全局置顶和分版置顶
        /// </summary>
        public bool IsDisplayTopThread {
            get {
                return base.GetBool("IsDisplayTopThread", true);
            }
            set {
                base.SetExtendedAttribute("IsDisplayTopThread", value.ToString());
            }
        }
        /// <summary>
        /// 主题排序字段
        /// </summary>
        public string ThreadOrderField {
            get {
                return base.GetString("ThreadOrderField", EThreadOrderField.LastDate.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadOrderField", value);
            }
        }
        /// <summary>
        /// 主题排序方式
        /// </summary>
        public string ThreadOrderType {
            get {
                return base.GetString("ThreadOrderType", EThreadOrderType.Asc.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadOrderType", value);
            }
        }
        /// <summary>
        /// 主题审核方式
        /// </summary>
        public string ThreadCheckType {
            get {
                return base.GetString("ThreadCheckType", EThreadCheckType.None.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadCheckType", value);
            }
        }  
        /// <summary>
        /// 是否允许编辑帖子
        /// </summary>
        public bool IsEditThread {
            get {
                return base.GetBool("IsEditThread", true);
            }
            set {
                base.SetExtendedAttribute("IsEditThread", value.ToString());
            }
        }
        /// <summary>
        /// 是否开启主题回收站
        /// </summary>
        public bool IsOpenRecycle {
            get {
                return base.GetBool("IsOpenRecycle", false);
            }
            set {
                base.SetExtendedAttribute("IsOpenRecycle", value.ToString());
            }
        }
        /// <summary>
        /// 是否允许使用 HTML 代码
        /// </summary>
        public bool IsAllowHtml {
            get {
                return base.GetBool("IsAllowHtml", false);
            }
            set {
                base.SetExtendedAttribute("IsAllowHtml", value.ToString());
            }
        }
        /// <summary>
        /// 是否允许使用 [img] 代码
        /// </summary>
        public bool IsAllowImg {
            get {
                return base.GetBool("IsAllowImg", true);
            }
            set {
                base.SetExtendedAttribute("IsAllowImg", value.ToString());
            }
        }
        /// <summary>
        /// 是否允许使多媒体代码
        /// </summary>
        public bool IsAllowMultimedia {
            get {
                return base.GetBool("IsAllowMultimedia", false);
            }
            set {
                base.SetExtendedAttribute("IsAllowMultimedia", value.ToString());
            }
        }
        /// <summary>
        /// 是否允许编辑帖子
        /// </summary>
        public bool IsAllowEmotionSymbol {
            get {
                return base.GetBool("IsAllowEmotionSymbol", true);
            }
            set {
                base.SetExtendedAttribute("IsAllowEmotionSymbol", value.ToString());
            }
        }
        /// <summary>
        /// 是否启用内容干扰码
        /// </summary>
        public bool IsOpenDisturbCode {
            get {
                return base.GetBool("IsOpenDisturbCode", false);
            }
            set {
                base.SetExtendedAttribute("IsOpenDisturbCode", value.ToString());
            }
        }
        /// <summary>
        /// 是否允许匿名发帖
        /// </summary>
        public bool IsAllowAnonymousThread {
            get {
                return base.GetBool("IsAllowAnonymousThread", false);
            }
            set {
                base.SetExtendedAttribute("IsAllowAnonymousThread", value.ToString());
            }
        }
        /// <summary>
        /// 图片附件是否加水印
        /// </summary>
        public bool IsOpenWatermark {
            get {
                return base.GetBool("IsOpenWatermark", true);
            }
            set {
                base.SetExtendedAttribute("IsOpenWatermark", value.ToString());
            }
        } 
        /// <summary>
        /// 主题自动关闭方式
        /// </summary>
        public string ThreadAutoCloseType {
            get {
                return base.GetString("ThreadAutoCloseType", EThreadAutoCloseType.NoAutoClose.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadAutoCloseType", value);
            }
        }
        /// <summary>
        /// 主题自动关闭时间(天）
        /// </summary>
        public int ThreadAutoCloseWithDateNum {
            get {
                return base.GetInt("ThreadAutoCloseWithDateNum", 30);
            }
            set {
                base.SetExtendedAttribute("ThreadAutoCloseWithDateNum", value.ToString());
            }
        }
        /// <summary>
        /// 发主题时允许附件类型
        /// </summary>
        public string AllowAccessoryType {
            get {
                return base.GetString("AllowAccessoryType", string.Empty);
            }
            set {
                base.SetExtendedAttribute("AllowAccessoryType", value);
            }
        }
        /// <summary>
        /// 版块状态
        /// </summary>
        public string ThreadState {
            get {
                return base.GetString("ThreadState", EThreadStateType.Normal.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadState", value);
            }
        }
        /// <summary>
        /// 主题分类控制
        /// </summary>
        public string ThreadCategoryType
        {
            get
            {
                return base.GetString("ThreadCategoryType", EThreadCategoryType.No.ToString());
            }
            set
            {
                base.SetExtendedAttribute("ThreadCategoryType", value);
            }
        }
        /// <summary>
        /// 是否允许按类别浏览
        /// </summary>
        public bool IsReadByCategory {
            get {
                return base.GetBool("IsReadByCategory", true);
            }
            set {
                base.SetExtendedAttribute("IsReadByCategory", value.ToString());
            }
        }
        /// <summary>
        /// 主题前面分类的显示类型
        /// </summary>
        public string ThreadCategoryDisplayType
        {
            get {
                return base.GetString("ThreadCategoryDisplayType", EThreadCategoryDisplayType.Text.ToString());
            }
            set {
                base.SetExtendedAttribute("ThreadCategoryDisplayType", value);
            }
        }

        public string Moderators
        {
            get
            {
                return base.GetString("Moderators", string.Empty);
            }
            set
            {
                base.SetExtendedAttribute("Moderators", value);
            }
        }

        public bool IsModeratorsInherit
        {
            get
            {
                return base.GetBool("IsModeratorsInherit", true);
            }
            set
            {
                base.SetExtendedAttribute("IsModeratorsInherit", value.ToString());
            }
        }

        //权限相关

        public string AccessPassword
        {
            get
            {
                return base.GetString("AccessPassword", string.Empty);
            }
            set
            {
                base.SetExtendedAttribute("AccessPassword", value);
            }
        }

        public string AccessUserNames
        {
            get
            {
                return base.GetString("AccessUserNames", string.Empty);
            }
            set
            {
                base.SetExtendedAttribute("AccessUserNames", value);
            }
        }
    }
}
