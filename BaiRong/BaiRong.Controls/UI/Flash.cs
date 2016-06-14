using System;
using System.Web;
using System.Web.UI;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class Flash : Control 
	{
        public virtual String FlashUrl
        {
            get
            {
                string state = (string)ViewState["FlashUrl"];
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
                ViewState["FlashUrl"] = value;
            }
        }

        public virtual int Width
        {
            get
            {
                int state = (int)ViewState["Width"];
                return state;
            }
            set
            {
                ViewState["Width"] = value;
            }
        }

        public virtual int Height
        {
            get
            {
                int state = (int)ViewState["Height"];
                return state;
            }
            set
            {
                ViewState["Height"] = value;
            }
        }

		protected override void Render(HtmlTextWriter writer)
		{
            string scriptFormat = @"
<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"" width=""{0}"" height=""{1}"">
  <param name=""movie"" value=""{2}"" />
  <param name=""quality"" value=""high"" />
  <param name=""wmode"" value=""transparent"" />
  <embed src=""{2}"" quality=""high"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" type=""application/x-shockwave-flash"" width=""{0}"" height=""{1}""></embed>
</object>
";
            writer.Write(scriptFormat, this.Width, this.Height, this.FlashUrl);
		}
	}
}
