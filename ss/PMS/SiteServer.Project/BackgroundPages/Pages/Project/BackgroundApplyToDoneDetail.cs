using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;

using System.Web.UI.HtmlControls;

using System.Text;

namespace SiteServer.Project.BackgroundPages
{
	public class BackgroundApplyToDoneDetail : BackgroundApplyToDetailBasePage
	{
        public static string GetRedirectUrl(int applyID)
        {
            return string.Format(@"background_ApplyToDoneDetail.aspx?ApplyID={0}", applyID);
        }
	}
}
