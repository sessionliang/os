using System.Web.UI;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System;

namespace SiteServer.CMS.Pages.Mlib
{
    public class SystemBasePage : Page
    {
        private int publishmentSystemID = 0;
        public int PublishmentSystemID
        {
            get
            {
                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = PublishmentSystemInfo.PublishmentSystemID;
                }
                return publishmentSystemID;
            }
        }

        private PublishmentSystemInfo publishmentSystemInfo;
        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (publishmentSystemInfo == null)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetUniqueMLib();
                }
                return publishmentSystemInfo;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(UserManager.Current.UserName))
            {
                PageUtils.Redirect("/home/");
            }
        }


        /// <summary>
        /// 获取投稿管理的草稿表名
        /// </summary>
        private string mLibDraftContentTableName;
        public string MLibDraftContentTableName
        {
            get
            {
                if (mLibDraftContentTableName == null)
                {
                    mLibDraftContentTableName = "bairong_MLibDraftContent";
                }
                return mLibDraftContentTableName;
            }
        }
    }
}
