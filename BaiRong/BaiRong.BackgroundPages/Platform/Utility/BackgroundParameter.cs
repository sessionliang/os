using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Diagnostics;
using BaiRong.Model;

using System;

namespace BaiRong.BackgroundPages
{
	public class BackgroundParameter : BackgroundBasePage
	{
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "查看机器参数", AppManager.Platform.Permission.Platform_Utility);
            }
        }

		public string PrintParameter()
		{
			StringBuilder builder = new StringBuilder();
			string hostName = ComputerUtils.GetHostName();

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "系统主机名：", hostName));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "系统根目录地址：", ConfigUtils.Instance.PhysicalApplicationPath));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "系统程序目录地址：", PathUtils.PhysicalSiteServerPath));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "计算机的网卡地址：", ComputerUtils.GetMacAddress()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "计算机的CPU标识：", ComputerUtils.GetProcessorId()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "计算机的硬盘序列号：", ComputerUtils.GetColumnSerialNumber()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "域名：", PageUtils.GetHost()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "访问IP：", PageUtils.GetIPAddress()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ".NET版本：", System.Environment.Version));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "SiteServer 系统版本：", ProductManager.GetFullVersion()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "最近升级时间：", DateUtils.GetDateAndTimeString(ConfigManager.Instance.UpdateDate)));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "数据库类型：", EDatabaseTypeUtils.GetValue(BaiRongDataProvider.DatabaseType)));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "数据库名称：", BaiRong.Core.Data.SqlUtils.GetDatabaseNameFormConnectionString(BaiRongDataProvider.ADOType, BaiRongDataProvider.ConnectionString)));

			return builder.ToString();
		}

	}
}
