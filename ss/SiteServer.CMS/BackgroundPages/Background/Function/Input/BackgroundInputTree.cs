using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using SiteServer.CMS.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages
{

    public class BackgroundSubscribeTree : BackgroundBasePage
    {
         
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
             
            if (!IsPostBack)
            {

            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        protected void SiteTree_OnLoadItemArrayList(Tree sender, EventArgs e)
        { 
            List<TreeBaseItem> list = new List<TreeBaseItem>();
            list = DataProvider.InputClassifyDAO.GetItemInfoArrayListByParentID(this.PublishmentSystemID, 0);
            sender.ItemArrayList.AddRange(list);
        }
    }
}
