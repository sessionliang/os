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
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "�鿴��������", AppManager.Platform.Permission.Platform_Utility);
            }
        }

		public string PrintParameter()
		{
			StringBuilder builder = new StringBuilder();
			string hostName = ComputerUtils.GetHostName();

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "ϵͳ��������", hostName));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "ϵͳ��Ŀ¼��ַ��", ConfigUtils.Instance.PhysicalApplicationPath));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "ϵͳ����Ŀ¼��ַ��", PathUtils.PhysicalSiteServerPath));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "�������������ַ��", ComputerUtils.GetMacAddress()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "�������CPU��ʶ��", ComputerUtils.GetProcessorId()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "�������Ӳ�����кţ�", ComputerUtils.GetColumnSerialNumber()));

			builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "������", PageUtils.GetHost()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "����IP��", PageUtils.GetIPAddress()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ".NET�汾��", System.Environment.Version));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "SiteServer ϵͳ�汾��", ProductManager.GetFullVersion()));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "�������ʱ�䣺", DateUtils.GetDateAndTimeString(ConfigManager.Instance.UpdateDate)));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "���ݿ����ͣ�", EDatabaseTypeUtils.GetValue(BaiRongDataProvider.DatabaseType)));

            builder.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", "���ݿ����ƣ�", BaiRong.Core.Data.SqlUtils.GetDatabaseNameFormConnectionString(BaiRongDataProvider.ADOType, BaiRongDataProvider.ConnectionString)));

			return builder.ToString();
		}

	}
}
