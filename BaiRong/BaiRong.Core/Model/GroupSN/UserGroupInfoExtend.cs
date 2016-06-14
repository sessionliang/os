using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Model
{
    public class UserGroupInfoExtend : ExtendedAttributes
    {
        public UserGroupInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public UserGroupInfoExtend(bool isAllowVisit, bool isAllowHide, bool isAllowSignature, ETriState searchType, int searchInterval, bool isAllowRead, bool isAllowPost, bool isAllowReply, bool isAllowPoll, int maxPostPerDay, int postInterval, ETriState uploadType, ETriState downloadType, bool isAllowSetAttachmentPermissions, int maxSize, int maxSizePerDay, int maxNumPerDay, string attachmentExtensions)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            base.SetExtendedAttribute(nameValueCollection);

            this.IsAllowVisit = isAllowVisit;
            this.IsAllowHide = isAllowHide;
            this.IsAllowSignature = isAllowSignature;
            this.SearchType = ETriStateUtils.GetValue(searchType);
            this.SearchInterval = searchInterval;
            this.IsAllowRead = isAllowRead;
            this.IsAllowPost = isAllowPost;
            this.IsAllowReply = isAllowReply;
            this.IsAllowPoll = isAllowPoll;
            this.MaxPostPerDay = maxPostPerDay;
            this.PostInterval = postInterval;
            this.UploadType = ETriStateUtils.GetValue(uploadType);
            this.DownloadType = ETriStateUtils.GetValue(downloadType);
            this.IsAllowSetAttachmentPermissions = isAllowSetAttachmentPermissions;
            this.MaxSize = maxSize;
            this.MaxSizePerDay = maxSizePerDay;
            this.MaxNumPerDay = maxNumPerDay;
            this.AttachmentExtensions = attachmentExtensions;
        }

        //基本权限

        public bool IsAllowVisit
        {
            get { return base.GetBool("IsAllowVisit", true); }
            set { base.SetExtendedAttribute("IsAllowVisit", value.ToString()); }
        }

        public bool IsAllowHide
        {
            get { return base.GetBool("IsAllowHide", true); }
            set { base.SetExtendedAttribute("IsAllowHide", value.ToString()); }
        }

        public bool IsAllowSignature
        {
            get { return base.GetBool("IsAllowSignature", true); }
            set { base.SetExtendedAttribute("IsAllowSignature", value.ToString()); }
        }

        public string SearchType
        {
            get
            {
                return base.GetString("SearchType", ETriStateUtils.GetValue(ETriState.True));
            }
            set
            {
                base.SetExtendedAttribute("SearchType", value);
            }
        }

        public int SearchInterval
        {
            get { return base.GetInt("SearchInterval", 10); }
            set { base.SetExtendedAttribute("SearchInterval", value.ToString()); }
        }

        //帖子权限

        public bool IsAllowRead
        {
            get { return base.GetBool("IsAllowRead", true); }
            set { base.SetExtendedAttribute("IsAllowRead", value.ToString()); }
        }

        public bool IsAllowPost
        {
            get { return base.GetBool("IsAllowPost", true); }
            set { base.SetExtendedAttribute("IsAllowPost", value.ToString()); }
        }

        public bool IsAllowReply
        {
            get { return base.GetBool("IsAllowReply", true); }
            set { base.SetExtendedAttribute("IsAllowReply", value.ToString()); }
        }

        public bool IsAllowPoll
        {
            get { return base.GetBool("IsAllowPoll", true); }
            set { base.SetExtendedAttribute("IsAllowPoll", value.ToString()); }
        }

        public int MaxPostPerDay
        {
            get { return base.GetInt("MaxPostPerDay", 0); }
            set { base.SetExtendedAttribute("MaxPostPerDay", value.ToString()); }
        }

        public int PostInterval
        {
            get { return base.GetInt("PostInterval", 0); }
            set { base.SetExtendedAttribute("PostInterval", value.ToString()); }
        }

        //附件权限

        public string UploadType
        {
            get
            {
                return base.GetString("UploadType", ETriStateUtils.GetValue(ETriState.True));
            }
            set
            {
                base.SetExtendedAttribute("UploadType", value);
            }
        }

        public string DownloadType
        {
            get
            {
                return base.GetString("DownloadType", ETriStateUtils.GetValue(ETriState.True));
            }
            set
            {
                base.SetExtendedAttribute("DownloadType", value);
            }
        }

        // 允许设置附件权限
        public bool IsAllowSetAttachmentPermissions
        {
            get {
                return base.GetBool("IsAllowSetAttachmentPermissions", true);
            }
            set {
                base.SetExtendedAttribute("IsAllowSetAttachmentPermissions", value.ToString());
            }
        }

        // 最大附件大小
        public int MaxSize
        {
            get
            {
                return base.GetInt("MaxSize", 0);
            }
            set
            {
                base.SetExtendedAttribute("MaxSize", value.ToString());
            }
        }

        // 每天总附件大小
        public int MaxSizePerDay
        {
            get
            {
                return base.GetInt("MaxSizePerDay", 0);
            }
            set
            {
                base.SetExtendedAttribute("MaxSizePerDay", value.ToString());
            }
        }

        // 每天最大附件数量
        public int MaxNumPerDay
        {
            get
            {
                return base.GetInt("MaxNumPerDay", 0);
            }
            set
            {
                base.SetExtendedAttribute("MaxNumPerDay", value.ToString());
            }
        }

        // 允许附件类型
        public string AttachmentExtensions
        {
            get
            {
                return base.GetString("AttachmentExtensions", "chm,pdf,zip,rar,tar,gz,7z,gif,jpg,jpeg,png,doc,docx,xls,xlsx,ppt,pptx");
            }
            set
            {
                base.SetExtendedAttribute("AttachmentExtensions", value);
            }
        }
    }
}
