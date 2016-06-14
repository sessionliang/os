using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Controls 
{

	/// <summary>
	/// ¸´ºÏ¿Ø¼þ
	/// </summary>
	[
	ParseChildren( true ),
	PersistChildren( false ),
	]
	public abstract class CompositeWebControl : WebControl, INamingContainer 
	{

		#region Composite Controls

		/// <exclude/>
		public override ControlCollection Controls 
		{
			get 
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		/// <exclude/>
		public override void DataBind() 
		{
			this.EnsureChildControls();
			base.DataBind ();
		}

		#endregion

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
		}
		   
		/// <summary>
		/// No End Span
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			//we don't need a span tag
		}

		/// <exclude/>
		public override Control FindControl( string id ) 
		{
			Control ctrl = base.FindControl( id );
            if (ctrl == null)
            {
                //if (this.Controls.Count == 1)
                //{
                //    ctrl = this.Controls[0].FindControl(id);
                //}
                if (ctrl == null && this.HasControls())
                {
                    ctrl = this.FindControlByControls(id, this.Controls);
                }
            }
			return ctrl;
		}

        private Control FindControlByControls(string id, ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                Control ctrl = control.FindControl(id);
                if (ctrl == null && control.HasControls())
                {
                    ctrl = FindControlByControls(id, control.Controls);
                }
                if (ctrl != null) return ctrl;
            }
            return null;
        }
	}
}
