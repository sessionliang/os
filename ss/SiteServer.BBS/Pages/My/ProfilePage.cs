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
                        base.SuccessMessage("���������޸ĳɹ���");
                    }
                    else
                    {
                        base.FailureMessage("�Բ������������û��鲻������Ӹ���ǩ��");
                    }
                }
                catch (Exception ex)
                {
                    base.FailureMessage(string.Format("���������޸�ʧ�ܣ�{0}��", ex.Message));
                }
            }
        }
    }
}
