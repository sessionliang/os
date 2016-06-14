using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;



namespace BaiRong.BackgroundPages
{
    public class FrameworkUserProfile : BackgroundBasePage
    {
        public Literal UserName;
        public TextBox DisplayName;
        public TextBox Email;
        public TextBox Mobile;
        public TextBox Question;
        public TextBox Answer;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                AdministratorInfo adminInfo = AdminManager.Current;
                this.UserName.Text = adminInfo.UserName;
                this.DisplayName.Text = adminInfo.DisplayName;
                this.Email.Text = adminInfo.Email;
                this.Mobile.Text = adminInfo.Mobile;
                this.Question.Text = adminInfo.Question;
                this.Answer.Text = adminInfo.Answer;
            }
        }

        public void Submit_Click(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                AdministratorInfo adminInfo = AdminManager.Current;
                adminInfo.DisplayName = this.DisplayName.Text;
                adminInfo.Email = this.Email.Text;
                adminInfo.Mobile = this.Mobile.Text;
                adminInfo.Question = this.Question.Text;
                adminInfo.Answer = this.Answer.Text;

                BaiRongDataProvider.AdministratorDAO.Update(adminInfo);

                base.SuccessMessage("资料更改成功");
            }
        }
    }
}
