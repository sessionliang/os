using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.IO;
using BaiRong.Model;
using BaiRong.Core.Drawing;

namespace SiteServer.BBS.Pages
{
    public class ProfilePage : BasePage
    {
        public TextBox Signature;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (!base.Page.IsPostBack)
            {
                this.Signature.Text = UserManager.Current.Signature;
            }
        }

        public void Submit_Click(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                    if (groupInfo.Additional.IsAllowSignature)
                    {
                        UserInfo userInfo = UserManager.Current;
                        userInfo.Signature = PageUtils.FilterSqlAndXss(this.Signature.Text);
                        BaiRongDataProvider.UserDAO.Update(userInfo);
                        base.SuccessMessage("个人资料修改成功！");
                    }
                    else
                    {
                        base.FailureMessage("对不起，您所属的用户组不允许添加个人签名");
                    }
                }
                catch (Exception ex)
                {
                    base.FailureMessage(string.Format("个人资料修改失败：{0}！", ex.Message));
                }
            }
        }
    }
}
