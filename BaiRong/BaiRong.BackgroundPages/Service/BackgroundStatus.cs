using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;

using BaiRong.Core.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OracleClient;
using BaiRong.Core.Data;
using System.Text;
using BaiRong.Model.Service;
using BaiRong.Core.Service;

namespace BaiRong.BackgroundPages
{
    public class BackgroundStatus : BackgroundBasePage
	{
        public Literal ltlDateTime;
        public Literal ltlService;

        public Literal ltlTaskCreate;
        public Literal ltlTaskPublish;
        public Literal ltlTaskGather;
        public Literal ltlTaskBackup;

        public PlaceHolder phStorage;
        public Literal ltlStorage;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Service, "服务状态", AppManager.Platform.Permission.Platform_Service);

                string errorMessage = "系统未检测到运行的SiteServer Service 服务";
                DateTime dateTime = DateTime.MinValue;
                bool isOk = CacheManager.IsServiceOnline(out dateTime);
                if (isOk)
                {
                    TimeSpan ts = DateTime.Now - dateTime;
                    if (ts.TotalMinutes > CacheManager.Slide_Minutes_Status * 2)
                    {
                        isOk = false;
                        errorMessage = string.Format("服务最后一次运行时间为{0}", DateUtils.ParseThisMoment(dateTime, DateTime.Now));
                    }
                }
                if (isOk)
                {
                    this.ltlDateTime.Text = string.Format("截至{0}的服务状态", DateUtils.ParseThisMoment(dateTime, DateTime.Now));
                    this.ltlService.Text = this.GetStatus(true, "SiteServer Service 服务组件已安装并处于正常运行状态");

                    this.ltlTaskCreate.Text = this.GetStatus(true, "定时生成任务处于正常运行状态");
                    this.ltlTaskPublish.Text = this.GetStatus(true, "定时发布任务处于正常运行状态");
                    this.ltlTaskGather.Text = this.GetStatus(true, "定时采集任务处于正常运行状态");
                    this.ltlTaskBackup.Text = this.GetStatus(true, "定时备份任务处于正常运行状态");
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        this.ltlDateTime.Text = errorMessage;
                    }
                    this.ltlService.Text = this.GetStatus(false, "SiteServer Service 服务组件未安装或未启动");

                    this.ltlTaskCreate.Text = this.GetStatus(false, "定时生成任务未启动");
                    this.ltlTaskPublish.Text = this.GetStatus(false, "定时发布任务未启动");
                    this.ltlTaskGather.Text = this.GetStatus(false, "定时采集任务未启动");
                    this.ltlTaskBackup.Text = this.GetStatus(false, "定时备份任务未启动");
                }

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                if (storageInfoArrayList.Count > 0)
                {
                    StringBuilder builder = new StringBuilder("<tr>");
                    int i = 1;
                    foreach (StorageInfo storageInfo in storageInfoArrayList)
                    {
                        if ((i++ % 2) == 0)
                        {
                            builder.Append("<tr>");
                        }
                        StorageManager storageManager = new StorageManager(null, storageInfo, string.Empty);
                        if (storageManager.TestWrite())
                        {
                            builder.Append(this.GetStatus(true, storageInfo.StorageName + " 写入测试成功"));
                        }
                        else
                        {
                            builder.Append(this.GetStatus(false, storageInfo.StorageName + " 写入测试失败"));
                        }

                        if (storageManager.TestRead())
                        {
                            builder.Append(this.GetStatus(true, storageInfo.StorageName + " 读取测试成功"));
                        }
                        else
                        {
                            builder.Append(this.GetStatus(false, storageInfo.StorageName + " 读取测试失败"));
                        }

                        if ((i++ % 2) == 0)
                        {
                            builder.Append("</tr>");
                        }
                    }

                    this.ltlStorage.Text = builder.ToString();
                }
                else
                {
                    this.phStorage.Visible = false;
                }
			}
		}

        private string GetStatus(bool isNormal, string text)
        {
            return string.Format(@"<td><span class=""{0}""></span><span class=""text"">{1}</span></td>", isNormal ? "normal" : "issue", text);
        }
	}
}
