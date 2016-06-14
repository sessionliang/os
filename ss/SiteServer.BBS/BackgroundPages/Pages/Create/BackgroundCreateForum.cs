using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundCreateForum : BackgroundBasePage
	{
        public ListBox ForumIDCollectionToCreate;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Create, "生成板块页", AppManager.BBS.Permission.BBS_Create);

                ForumManager.AddListItems(base.PublishmentSystemID, this.ForumIDCollectionToCreate.Items, false);
			}
		}

        public void CreateNodeButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                ArrayList forumIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.ForumIDCollectionToCreate);

                if (forumIDArrayList.Count == 0)
                {
                    base.FailMessage("请首先选中希望生成页面的板块！");
                    return;
                }

                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + "ForumIDCollection", TranslateUtils.ObjectCollectionToString(forumIDArrayList));

                PageUtils.Redirect(BackgroundProgressBar.GetCreateForumsUrl(base.PublishmentSystemID, userKeyPrefix));
			}
		}
	}
}
