using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Service;

namespace BaiRong.BackgroundPages.Modal
{
	public class StorageTest : BackgroundBasePage
	{
        public Literal ltlStorageName;
        public Literal ltlStorageURL;
        public Literal ltlStorageType;
        public Literal ltlTest;

        public static string GetOpenWindowString(int storageID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("StorageID", storageID.ToString());
            return PageUtilityPF.GetOpenWindowString("�ռ����Ӳ���", "modal_storageTest.aspx", arguments, 440, 400, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.GetQueryString("StorageID")))
                {
                    int storageID = TranslateUtils.ToInt(base.GetQueryString("StorageID"));
                    StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
                    if (storageInfo != null)
                    {
                        this.ltlStorageName.Text = storageInfo.StorageName;
                        this.ltlStorageURL.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(storageInfo.StorageUrl));
                        this.ltlStorageType.Text = EStorageTypeUtils.GetText(storageInfo.StorageType);

                        StringBuilder builder = new StringBuilder();
                        StorageManager storageManager = new StorageManager(null, storageInfo, string.Empty);

                        if (storageManager.TestWrite())
                        {
                            builder.AppendFormat(@"<div class=""alert alert-success"">
    <strong>д����Գɹ�!</strong>&nbsp;&nbsp; �ɹ���ռ�д���ļ���{0}��!
  </div>", StorageManager.TEST_FILE_NAME);
                        }
                        else
                        {
                            builder.AppendFormat(@"<div class=""alert alert-error"">
    <strong>д�����ʧ��!</strong>&nbsp;&nbsp; �޷���ռ�д���ļ�!
  </div>", StorageManager.TEST_FILE_NAME);
                        }

                        string fileUrl = PageUtils.Combine(storageInfo.StorageUrl, StorageManager.TEST_FILE_NAME);

                        if (storageManager.TestRead())
                        {
                            builder.AppendFormat(@"<div class=""alert alert-success"">
    <strong>��ȡ���Գɹ�!</strong>&nbsp;&nbsp; �ɹ���ȡ�ռ��ļ���<a href=""{0}"" target=""_blank"">{1}</a>��!
  </div>", fileUrl, StorageManager.TEST_FILE_NAME);
                        }
                        else
                        {
                            builder.AppendFormat(@"<div class=""alert alert-error"">
    <strong>��ȡ����ʧ��!</strong>&nbsp;&nbsp;������룺{0}
  </div>", storageManager.ErrorMessage);
                        }

                        this.ltlTest.Text = builder.ToString();
                    }
                }
			}
		}
	}
}
