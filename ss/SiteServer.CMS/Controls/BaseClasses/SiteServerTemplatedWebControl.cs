using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using SiteServer.CMS.Core;
using BaiRong.Core;

namespace SiteServer.CMS.Controls
{
	/// <summary>
	/// SiteServerTemplatedWebControl 的摘要说明。
	/// </summary>
	public abstract class SiteServerTemplatedWebControl : BaiRong.Controls.TemplatedWebControl
	{
		protected override string SkinFolder
		{
			get
			{
                return PageUtils.GetAdminDirectoryUrl("Themes/Skins/");
			}
		}
	}
}
