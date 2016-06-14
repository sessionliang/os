using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundWebsiteMessageTemplateTree : BackgroundBasePage
    {
        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Unnamed_OnLoadItemArrayList(Tree sender, EventArgs e)
        {
            //提交留言模板，留言列表模板，留言详情模板，邮件回复模板，短信回复模板
            sender.ItemArrayList.Add(new TreeBaseItem() { ItemID = 1, ItemName = "提交留言模板", Taxis = 1 });
            sender.ItemArrayList.Add(new TreeBaseItem() { ItemID = 2, ItemName = "留言列表模板", Taxis = 1 });
            sender.ItemArrayList.Add(new TreeBaseItem() { ItemID = 3, ItemName = "留言详情模板", Taxis = 1 });
            sender.ItemArrayList.Add(new TreeBaseItem() { ItemID = 4, ItemName = "邮件回复模板", Taxis = 1 });
            sender.ItemArrayList.Add(new TreeBaseItem() { ItemID = 5, ItemName = "短信回复模板", Taxis = 1 });
        }
    }
}
