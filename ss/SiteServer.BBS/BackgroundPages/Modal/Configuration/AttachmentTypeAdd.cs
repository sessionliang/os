using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class AttachmentTypeAdd : BackgroundBasePage
    {
        protected TextBox tbFileExtName;
        protected TextBox tbMaxSize;

        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加附件类型", PageUtils.GetBBSUrl("modal_attachmentTypeAdd.aspx"), arguments, 460, 230);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑附件类型", PageUtils.GetBBSUrl("modal_attachmentTypeAdd.aspx"), arguments, 460, 230);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    AttachmentTypeInfo typeInfo = DataProvider.AttachmentTypeDAO.GetAttachmentTypeInfo(this.id);

                    this.tbFileExtName.Text = typeInfo.FileExtName;
                    this.tbMaxSize.Text = typeInfo.MaxSize.ToString();
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.id > 0)
            {
                try
                {
                    AttachmentTypeInfo typeInfo = DataProvider.AttachmentTypeDAO.GetAttachmentTypeInfo(this.id);

                    typeInfo.FileExtName = this.tbFileExtName.Text.Trim('.');
                    typeInfo.MaxSize = TranslateUtils.ToInt(this.tbMaxSize.Text);

                    DataProvider.AttachmentTypeDAO.Update(typeInfo);
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, string.Format("编辑附件类型出错:{0}", ex.Message));
                }
            }
            else
            {
                try
                {
                    AttachmentTypeInfo typeInfo = new AttachmentTypeInfo(0, base.PublishmentSystemID, this.tbFileExtName.Text.Trim('.'), TranslateUtils.ToInt(this.tbMaxSize.Text));

                    DataProvider.AttachmentTypeDAO.Insert(typeInfo);
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, string.Format("添加附件类型出错:{0}", ex.Message));
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundAttachmentType.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
