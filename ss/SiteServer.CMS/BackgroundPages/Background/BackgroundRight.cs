using System.Web.UI.WebControls;
using BaiRong.Core;
using System;


using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundRight : BackgroundBasePage
	{
        public Literal ltlQQ;
        public Literal ltlWelcome;
        public Literal ltlVersionInfo;
        public Literal ltlUpdateDate;
        public Literal ltlLastLoginDate;

        public PlaceHolder phCheck;
        public Literal ltlCheckHtml;

		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                if (FileConfigManager.Instance.IsSaas)
                {
                    if (ConfigManager.Additional.SiteYun_OrderID > 0)
                    {
                        this.ltlQQ.Text = @"<script charset=""utf-8"" type=""text/javascript"" src=""http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAxMTMwMF84ODA2MV80MDA4NzcwNTUwXw""></script>";
                    }

                    //this.ltlWelcome.Text = "欢迎使用 SiteYun 云建站平台<br />—— 我们为您提供包含服务、主机、模板以及管理软件为一体的云端在线应用与运维平台，致力于为中国数千万企业用户提供托管放心、运维安心、修改省心、服务贴心的在线应用保姆式服务";

                    if (FileConfigManager.Instance.OEMConfig.IsOEM)
                    {
                        this.ltlWelcome.Text = "欢迎使用 微信公众号管理平台";
                    }
                    else
                    {
                        this.ltlWelcome.Text = "欢迎使用 GEXIA 阁下微信平台";
                    }
                }
                else
                {
                    this.ltlWelcome.Text = "欢迎使用 SiteServer 应用管理平台";
                }

                this.ltlVersionInfo.Text = ProductManager.GetFullVersion();

                DateTime dateTime = BaiRongDataProvider.LogDAO.GetLastLoginDate(AdminManager.Current.UserName);

                if (dateTime != DateTime.MinValue)
                {
                    this.ltlLastLoginDate.Text = DateUtils.GetDateAndTimeString(dateTime);
                }

                this.ltlUpdateDate.Text = DateUtils.GetDateAndTimeString(ConfigManager.Instance.UpdateDate);

                this.LoadCheckRepeater();
            }
		}

        public void LoadCheckRepeater()
        {
            ArrayList unCheckedArrayList = CheckManager.GetUserCountArrayListUnChecked();
            if (unCheckedArrayList.Count > 0)
            {
                int totalNum = 0;

                Hashtable hashtable = new Hashtable();

                foreach (int[] pair in unCheckedArrayList)
                {
                    int publishmentSystemID = pair[0];
                    int count = pair[1];
                    if (hashtable.ContainsKey(publishmentSystemID))
                    {
                        hashtable[publishmentSystemID] = (int)hashtable[publishmentSystemID] + count;
                    }
                    else
                    {
                        hashtable[publishmentSystemID] = count;
                    }
                }

                StringBuilder builder = new StringBuilder();
                foreach (int publishmentSystemID in hashtable.Keys)
                {
                    int count = (int)hashtable[publishmentSystemID];
                    if (PublishmentSystemManager.IsExists(publishmentSystemID))
                    {
                        string urlCheck = PageUtils.GetCMSUrl(string.Format("background_contentCheck.aspx?PublishmentSystemID={0}", publishmentSystemID));
                        builder.AppendFormat(@"<tr><td><a href=""{0}"">{1} 有 <span style=""color:#F00"">{2}</span> 篇</a></td></tr>", urlCheck, PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).PublishmentSystemName, count);
                        totalNum += count;
                    }
                }

                this.ltlCheckHtml.Text = string.Format(@"<tr class=""info thead""><td>共有 <span style=""color:#F00"">{0}</span> 篇内容待审核</td></tr>{1}", totalNum, builder.ToString());
            }
            else
            {
                this.phCheck.Visible = false;
            }
        }
	}
}
