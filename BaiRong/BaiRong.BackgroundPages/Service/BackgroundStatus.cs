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
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Service, "����״̬", AppManager.Platform.Permission.Platform_Service);

                string errorMessage = "ϵͳδ��⵽���е�SiteServer Service ����";
                DateTime dateTime = DateTime.MinValue;
                bool isOk = CacheManager.IsServiceOnline(out dateTime);
                if (isOk)
                {
                    TimeSpan ts = DateTime.Now - dateTime;
                    if (ts.TotalMinutes > CacheManager.Slide_Minutes_Status * 2)
                    {
                        isOk = false;
                        errorMessage = string.Format("�������һ������ʱ��Ϊ{0}", DateUtils.ParseThisMoment(dateTime, DateTime.Now));
                    }
                }
                if (isOk)
                {
                    this.ltlDateTime.Text = string.Format("����{0}�ķ���״̬", DateUtils.ParseThisMoment(dateTime, DateTime.Now));
                    this.ltlService.Text = this.GetStatus(true, "SiteServer Service ��������Ѱ�װ��������������״̬");

                    this.ltlTaskCreate.Text = this.GetStatus(true, "��ʱ������������������״̬");
                    this.ltlTaskPublish.Text = this.GetStatus(true, "��ʱ������������������״̬");
                    this.ltlTaskGather.Text = this.GetStatus(true, "��ʱ�ɼ���������������״̬");
                    this.ltlTaskBackup.Text = this.GetStatus(true, "��ʱ������������������״̬");
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        this.ltlDateTime.Text = errorMessage;
                    }
                    this.ltlService.Text = this.GetStatus(false, "SiteServer Service �������δ��װ��δ����");

                    this.ltlTaskCreate.Text = this.GetStatus(false, "��ʱ��������δ����");
                    this.ltlTaskPublish.Text = this.GetStatus(false, "��ʱ��������δ����");
                    this.ltlTaskGather.Text = this.GetStatus(false, "��ʱ�ɼ�����δ����");
                    this.ltlTaskBackup.Text = this.GetStatus(false, "��ʱ��������δ����");
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
                            builder.Append(this.GetStatus(true, storageInfo.StorageName + " д����Գɹ�"));
                        }
                        else
                        {
                            builder.Append(this.GetStatus(false, storageInfo.StorageName + " д�����ʧ��"));
                        }

                        if (storageManager.TestRead())
                        {
                            builder.Append(this.GetStatus(true, storageInfo.StorageName + " ��ȡ���Գɹ�"));
                        }
                        else
                        {
                            builder.Append(this.GetStatus(false, storageInfo.StorageName + " ��ȡ����ʧ��"));
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
