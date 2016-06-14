using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Core.Web.Controls
{
	public abstract class TextEditorBase : Control 
	{
		public virtual string Text
		{
			get
			{
				Object state = ViewState["Text"];
				if (state != null)
				{
					return (string)state;
				}
				return string.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		public virtual string Width
		{
			get
			{
				Object state = ViewState["Width"];
				if (state != null)
				{
					return (string)state;
				}
				return "0";
			}
			set
			{
				ViewState["Width"] = value;
			}
		}

		public virtual string Height
		{
			get
			{
				Object state = ViewState["Height"];
				if (state != null)
				{
					return (string)state;
				}
				return "0";
			}
			set
			{
				ViewState["Height"] = value;
			}
		}
	}
}
