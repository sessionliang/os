using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model
{
    public class User
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public int GroupID { get; set; }

        public int Credits { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastActivityDate { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public int OnlineSeconds { get; set; }

        public string AvatarLarge { get; set; }

        public string AvatarMiddle { get; set; }

        public string AvatarSmall { get; set; }

        public string Signature { get; set; }

        public DateTime Birthday { get; set; }

        public string BloodType { get; set; }

        public string Gender { get; set; }

        public string MaritalStatus { get; set; }

        public string Education { get; set; }

        public string Graduation { get; set; }

        public string Profession { get; set; }
        public string QQ { get; set; }
        public string WeiBo { get; set; }
        public string WeiXin { get; set; }
        public string Interests { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }

        public bool IsBindEmail { get; set; }
        public bool IsBindPhone { get; set; }
        public bool IsSetSQCU { get; set; }


        #region 用户消息
        public bool HasNewMsg { get; set; }//是否有新信息
        public int NewMsgCount { get; set; }//新信息条数
        public DateTime LastSystemMsgDate { get; set; }//最新系统公告发布时间
        public int SystemNoticeCount { get; set; } //系统通知条数
        #endregion

        public static User GetInstance()
        {
            UserInfo userInfo = RequestUtils.Current;

            User user = new User { UserID = userInfo.UserID, UserName = userInfo.UserName, GroupID = userInfo.GroupID, Credits = userInfo.Credits, CreateDate = userInfo.CreateDate, LastActivityDate = userInfo.LastActivityDate, DisplayName = userInfo.DisplayName, Email = userInfo.Email, Mobile = userInfo.Mobile, OnlineSeconds = userInfo.OnlineSeconds, AvatarLarge = userInfo.AvatarLarge, AvatarMiddle = userInfo.AvatarMiddle, AvatarSmall = userInfo.AvatarSmall, Signature = userInfo.Signature, BloodType = userInfo.BloodType, Birthday = userInfo.Birthday, Gender = userInfo.Gender, MaritalStatus = userInfo.MaritalStatus, Education = userInfo.Education, Graduation = userInfo.Graduation, Profession = userInfo.Profession, QQ = userInfo.QQ, WeiBo = userInfo.WeiBo, WeiXin = userInfo.WeiXin, Interests = userInfo.Interests, Organization = userInfo.Organization, Department = userInfo.Department, Position = userInfo.Position, Address = userInfo.Address, IsBindEmail = userInfo.IsBindEmail, IsBindPhone = userInfo.IsBindPhone, IsSetSQCU = userInfo.IsSetSCQU };

            user.AvatarLarge = APIPageUtils.ParseUrlWithCase(PageUtils.GetUserAvatarUrl(userInfo.GroupSN, userInfo.UserName, EAvatarSize.Large));
            user.AvatarMiddle = APIPageUtils.ParseUrlWithCase(PageUtils.GetUserAvatarUrl(userInfo.GroupSN, userInfo.UserName, EAvatarSize.Middle));
            user.AvatarSmall = APIPageUtils.ParseUrlWithCase(PageUtils.GetUserAvatarUrl(userInfo.GroupSN, userInfo.UserName, EAvatarSize.Small));

            #region 用户消息
            int newMsgCount = BaiRongDataProvider.UserMessageDAO.GetCount("MessageTo = '" + user.UserName + "' AND IsViewed = 'False' AND (MessageType = '" + EUserMessageType.System.ToString() + "')");
            DateTime lastSysMsgDate = BaiRongDataProvider.UserMessageDAO.GetLastMessagePublishDate(EUserMessageType.SystemAnnouncement);
            int systemNoticeCount = BaiRongDataProvider.UserMessageDAO.GetCount("MessageTo = '' AND IsViewed = 'False' AND MessageType = '" + EUserMessageType.SystemAnnouncement.ToString() + "' AND GETDATE() - AddDate > 0 AND DATEDIFF(SS, AddDate, GETDATE()) < 3600 * 24 * " + (UserConfigManager.Additional.NewOfDays > 0 ? UserConfigManager.Additional.NewOfDays : 1));
            if (newMsgCount + systemNoticeCount > 0)
                user.HasNewMsg = true;
            user.NewMsgCount = newMsgCount;
            user.LastSystemMsgDate = lastSysMsgDate;
            user.SystemNoticeCount = systemNoticeCount;
            #endregion

            return user;
        }
    }
}