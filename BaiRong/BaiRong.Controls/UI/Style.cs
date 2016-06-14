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
	/// Encapsulated rendering of style based on the selected skin.
	/// </summary>
	// ********************************************************************/ 
	public class Style : LiteralControl 
	{

		/// <summary>
		/// Property Media (string)
		/// </summary>
		[
		System.ComponentModel.DefaultValue( "screen" ),
		]
		public virtual String Media 
		{
			get 
			{
				Object state = ViewState["Media"];
				if ( state != null ) 
				{
					return (String)state;
				}
				return "screen";
			}
			set 
			{
				ViewState["Media"] = value;
			}
		}

		/// <summary>
		/// Property Href (string)
		/// </summary>
		public virtual String Href 
		{
			get 
			{
				string state = (string) ViewState["Href"];
				if ( state != null )
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
				ViewState["Href"] = value;
			}
		}
      
		const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(Href))
				writer.Write(linkFormat,Href,Media);
		}
	}
}
