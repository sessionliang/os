using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace BaiRong.BackgroundPages
{
    public class BackgroundStorage : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button btnAdd;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int storageID = TranslateUtils.ToInt(base.GetQueryString("StorageID"));
                try
                {
                    BaiRongDataProvider.StorageDAO.Delete(storageID);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            if (base.GetQueryString("Enabled") != null)
            {
                int storageID = base.GetIntQueryString("StorageID");

                try
                {
                    BaiRongDataProvider.StorageDAO.UpdateState(storageID, true);
                    base.SuccessMessage("�ɹ����ÿռ䡣");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "���ÿռ�ʧ�ܡ�");
                }
            }

            if (base.GetQueryString("Disabled") != null)
            {
                int storageID = TranslateUtils.ToInt(base.GetQueryString("StorageID"));

                try
                {
                    BaiRongDataProvider.StorageDAO.UpdateState(storageID, false);
                    base.SuccessMessage("�ɹ����ÿռ䡣");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "���ÿռ�ʧ�ܡ�");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Storage, "�洢�ռ����", AppManager.Platform.Permission.Platform_Storage);

                this.btnAdd.Attributes.Add("onclick", Modal.StorageAdd.GetOpenWindowStringToAdd());

                this.dgContents.DataSource = BaiRongDataProvider.StorageDAO.GetDataSource();
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        public string GetEnabledHtml(int storageID, string isEnabled)
        {
            if (TranslateUtils.ToBool(isEnabled))
            {
                string urlStorage = PageUtils.GetPlatformUrl(string.Format("background_storage.aspx?Disabled=True&StorageID={0}", storageID));
                return string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('�˲��������ÿռ䣬ȷ����');\">����</a>", urlStorage);
            }
            else
            {
                string urlStorage = PageUtils.GetPlatformUrl(string.Format("background_storage.aspx?Enabled=True&StorageID={0}", storageID));
                return string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('�˲��������ÿռ䣬ȷ����');\">����</a>", urlStorage);
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int storageID = TranslateUtils.EvalInt(e.Item.DataItem, "StorageID");
                string storageName = TranslateUtils.EvalString(e.Item.DataItem, "StorageName");
                string storageUrl = TranslateUtils.EvalString(e.Item.DataItem, "StorageUrl");
                EStorageType storageType = EStorageTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "StorageType"));
                string isEnabled = TranslateUtils.EvalString(e.Item.DataItem, "IsEnabled");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");

                Literal ltlStorageURL = (Literal)e.Item.FindControl("ltlStorageURL");
                Literal ltlStorageType = (Literal)e.Item.FindControl("ltlStorageType");
                Literal ltlIsEnabledHtml = (Literal)e.Item.FindControl("ltlIsEnabledHtml");
                Literal ltlEditHtml = (Literal)e.Item.FindControl("ltlEditHtml");
                Literal ltlTestHtml = (Literal)e.Item.FindControl("ltlTestHtml");
                Literal ltlEnabledHtml = (Literal)e.Item.FindControl("ltlEnabledHtml");
                Literal ltlDeleteHtml = (Literal)e.Item.FindControl("ltlDeleteHtml");

                ltlStorageURL.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(storageUrl));
                ltlStorageType.Text = EStorageTypeUtils.GetText(storageType);
                ltlIsEnabledHtml.Text = StringUtils.GetTrueOrFalseImageHtml(isEnabled);
                ltlEditHtml.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">�޸�</a>", Modal.StorageAdd.GetOpenWindowStringToEdit(storageID));
                ltlTestHtml.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">����</a>", Modal.StorageTest.GetOpenWindowString(storageID));
                ltlEnabledHtml.Text = this.GetEnabledHtml(storageID, isEnabled);

                string urlDelete = PageUtils.GetPlatformUrl(string.Format("background_storage.aspx?Delete=True&StorageID={0}", storageID));
                ltlDeleteHtml.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('�˲�����ɾ���ռ䡰{1}����ȷ����');\">ɾ��</a>", urlDelete, storageName);
            }
        }

    }
}
