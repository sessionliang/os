using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Controls 
{
    public abstract class BaiRongTemplatedWebControl : TemplatedWebControl
    {
        protected override string SkinFolder
        {
            get
            {
                return PageUtils.GetAbsoluteSiteFilesUrl(SiteFiles.Directory.Skins);
            }
        }
    }
}
