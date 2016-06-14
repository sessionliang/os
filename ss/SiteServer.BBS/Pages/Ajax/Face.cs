using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages.Ajax
{
    public class Face : Page
    {
        public Literal ltlContent;

        private int publishmentSystemID;
        private string faceName;
        private int page;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            this.faceName = PageUtils.FilterSqlAndXss(base.Request.QueryString["faceName"]);
            this.page = TranslateUtils.ToInt(base.Request.QueryString["page"], 1);
            int PAGE_COUNT = 16;
            
            if (!string.IsNullOrEmpty(faceName))
            {
                faceName = faceName.Replace(".", string.Empty).Replace("/", string.Empty).Replace("\\",  string.Empty);
                string directoryPath = PathUtility.GetPublishmentSystemPath(this.publishmentSystemID, "smile", this.faceName);
                string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                if (fileNames != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(@"<div id=""tabs-{0}"" class=""face_info"">", faceName);
                    int i = 0;
                    int totalNum = (fileNames.Length > PAGE_COUNT) ? PAGE_COUNT : fileNames.Length;
                    if (page > 1)
                    {
                        i = (page - 1) * PAGE_COUNT;
                        totalNum += i;
                    }
                    for (; i < totalNum; i++)
                    {
                        builder.AppendFormat(@"<a href=""#""><img src=""{0}"" border=""0"" /></a>", PageUtilityBBS.GetBBSUrl(this.publishmentSystemID, string.Format("smile/{0}/{1}", faceName, fileNames[i])));
                    }
                    if (fileNames.Length > PAGE_COUNT)
                    {
                        int pageNum = fileNames.Length / PAGE_COUNT;
                        if (pageNum > 9) pageNum = 9;
                        builder.AppendFormat(@"<div class=""face_page"">");
                        for (int j = 1; j <= pageNum; j++)
                        {
                            if (j == this.page)
                            {
                                builder.AppendFormat(@"<span class=""face_cur_page"">{0}</span>", j);
                            }
                            else
                            {
                                builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetAjaxPageUrl(this.publishmentSystemID, string.Format("face.aspx?publishmentSystemID={0}&faceName={1}&page={2}", publishmentSystemID, faceName, j)), j);
                            }
                        }
                        builder.Append("</div>");
                    }
                    builder.Append("</div>");
                    this.ltlContent.Text = builder.ToString();
                }
            }
        }
    }
}
