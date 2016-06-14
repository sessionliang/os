using System.Web;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


namespace BaiRong.BackgroundPages
{
    public class FrameworkError : BackgroundBasePage
    {
        public Literal ltlErrorMessage;

        protected override bool IsAccessable
        {
            get { return true; }
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.GetQueryString("ErrorMessage")))
                {
                    string errorMessage = PageUtils.FilterXSS(StringUtils.ValueFromUrl(base.GetQueryString("ErrorMessage")));
                    ltlErrorMessage.Text = errorMessage;
                }
            }
        }
    }
}
