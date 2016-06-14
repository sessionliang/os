using System;
using System.Web;
using System.Web.UI;

using BaiRong.Core;

namespace BaiRong.Controls
{
	// *********************************************************************
	//  StyleSkin
	//
	/// <summary>
	/// Encapsulated rendering of script based on the selected skin.
	/// </summary>
	// ********************************************************************/ 
	public class Script : LiteralControl 
	{

		public virtual String Src 
		{
			get 
			{
				string state = (string) ViewState["Src"];
                if (state != null)
                {
                    if (state.StartsWith("~"))
                    {
                        return PageUtils.ParseNavigationUrl(state);
                    }
                    return ResolveUrl(state);
                }
                else
                    return string.Empty;
			}
			set 
			{
				ViewState["Src"] = value;
			}
		}
        
		const string srcFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(Src))
				writer.Write(srcFormat,Src);
		}


	}
}
