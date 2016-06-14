using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Text;
using BaiRong.Model;
using BaiRong.Core.Rss;
using BaiRong.Core.IO;

namespace SiteServer.BBS.Pages
{
    public class RssPage : BasePage
    {
        public Literal ltlXML;

        private int forumID;

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = base.GetIntQueryString("forumID");
            ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);

            if (forumInfo != null)
            {
                this.ltlXML.Text = this.Parse(forumInfo);
            }
        }

        private string Parse(ForumInfo forumInfo)
        {
            RssFeed feed = new RssFeed();
            feed.Encoding = Encoding.UTF8;
            feed.Version = RssVersion.RSS20;

            RssChannel channel = new RssChannel();
            channel.Title = forumInfo.ForumName;
            channel.Description = forumInfo.Summary;

            if (string.IsNullOrEmpty(channel.Description))
            {
                channel.Description = StringUtils.MaxLengthText(StringUtils.StripTags(forumInfo.Content), 200);
            }
            if (string.IsNullOrEmpty(channel.Description))
            {
                channel.Description = forumInfo.ForumName;
            }
            channel.Link = new Uri(PageUtils.AddProtocolToUrl(PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo)));

            string orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByLastDate);
            IEnumerable dataSource = DataUtility.GetThreadsDataSource(base.PublishmentSystemID, this.forumID, 0, string.Empty, 0, 0, orderByString);

            if (dataSource != null)
            {
                foreach (object dataItem in dataSource)
                {
                    RssItem item = new RssItem();

                    ThreadInfo threadInfo = new ThreadInfo(dataItem);

                    item.Title = StringUtils.Replace("&", threadInfo.Title, "&amp;");
                    item.Description = string.Empty;
                    item.PubDate = threadInfo.AddDate;
                    item.Link = new Uri(PageUtils.AddProtocolToUrl(PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, this.forumID, threadInfo.ID)));

                    channel.Items.Add(item);
                }
            }

            feed.Channels.Add(channel);

            StringBuilder builder = new StringBuilder();
            EncodedStringWriter textWriter = new EncodedStringWriter(builder, Encoding.UTF8);
            feed.Write(textWriter);

            return builder.ToString();
        }
    }
}
