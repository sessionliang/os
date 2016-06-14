using System;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;


namespace BaiRong.BackgroundPages
{
    public class BackgroundDBLogDelete : BackgroundBasePage
    {
        protected override bool IsAccessable
        {
            get { return true; }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "清空数据库日志", AppManager.Platform.Permission.Platform_Utility);
            }
        }

        //TODO: 建立数据库清空日志工作
        public string GetLastExecuteDate()
        {
            string retval = string.Empty;
            DateTime dt = BaiRongDataProvider.LogDAO.GetLastRemoveLogDate(BaiRongDataProvider.AdministratorDAO.UserName);
            retval = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return retval;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    BaiRongDataProvider.DatabaseDAO.DeleteDBLog();

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "清空数据库日志");

                    base.SuccessMessage("清空日志成功！");
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(ex.Message);
                }
            }
        }

    }
}
