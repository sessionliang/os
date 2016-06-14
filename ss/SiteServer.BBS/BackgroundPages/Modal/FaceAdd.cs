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
    public class FaceAdd : BackgroundBasePage
    {
        protected TextBox tbFaceName;
        protected TextBox tbTitle;
        protected RadioButtonList rblIsEnabled;

        private int id;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加表情", PageUtils.GetBBSUrl("modal_faceAdd.aspx"), arguments, 450, 320);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("id", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("编辑表情", PageUtils.GetBBSUrl("modal_faceAdd.aspx"), arguments, 450, 320);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("id");

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblIsEnabled, "启用", "禁用");

                if (this.id == 0)
                {
                    ControlUtils.SelectListItems(this.rblIsEnabled, true.ToString());
                }
                else
                {
                    FaceInfo faceInfo = DataProvider.FaceDAO.GetFaceInfo(base.PublishmentSystemID, this.id);

                    this.tbFaceName.Text = faceInfo.FaceName;
                    this.tbFaceName.Enabled = false;
                    this.tbTitle.Text = faceInfo.Title;
                    ControlUtils.SelectListItems(this.rblIsEnabled, faceInfo.IsEnabled.ToString());
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
                    FaceInfo faceInfo = DataProvider.FaceDAO.GetFaceInfo(base.PublishmentSystemID, this.id);

                    faceInfo.FaceName = this.tbFaceName.Text;
                    faceInfo.Title = this.tbTitle.Text;
                    faceInfo.IsEnabled = TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue);

                    DataProvider.FaceDAO.Update(faceInfo);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, string.Format("编辑表情出错:{0}", ex.Message));
                }
            }
            else
            {
                try
                {
                    FaceInfo faceInfo = new FaceInfo(0, base.PublishmentSystemID, this.tbFaceName.Text, this.tbTitle.Text, 0, TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue));

                    DataProvider.FaceDAO.Insert(faceInfo);
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    isChanged = false;
                    base.FailMessage(ex, string.Format("添加表情出错:{0}", ex.Message));
                }
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundFace.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
