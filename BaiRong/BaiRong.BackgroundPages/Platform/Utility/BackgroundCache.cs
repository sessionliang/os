using System;
using System.Collections;
using System.Text;
using BaiRong.Core;


namespace BaiRong.BackgroundPages
{
    public class BackgroundCache : BackgroundBasePage
    {
        public int CacheCount;
        public long CacheSize;
        public string CachePercentStr;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                CacheCount = CacheUtils.GetCacheCount() + DbCacheManager.GetCacheCount();
                CacheSize = 100 - CacheUtils.GetCacheEnabledPercent();
                CachePercentStr = string.Format(@"<div style=""width:230px;"" class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%""></div><span>&nbsp;{1}</span>
          </div>", 100 - CacheUtils.GetCacheEnabledPercent(), CacheSize + "%");
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "ÏµÍ³»º´æ", AppManager.Platform.Permission.Platform_Utility);
            }
        }


        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                CacheUtils.Clear();
                DbCacheManager.Clear();
                PageUtils.Redirect(PageUtils.GetPlatformUrl("background_cache.aspx"));
            }
        }

    }
}
