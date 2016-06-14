using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Controls 
{

	/// <summary>
	/// The base class for ALL bairong controls that must load their UI either through an external skin
	/// or via an in-page template.
	/// </summary>
	[
	ParseChildren( true ),
	PersistChildren( false ),
	]
	public abstract class TemplatedWebControl : CompositeWebControl
	{
		#region External Skin

		protected abstract string SkinFolder
		{
			get;
		}

        protected virtual string DefaultSkinFolder
        {
            get
            {
                return "~/Themes/default/Skins/";
                
            }
        }

		protected virtual String SkinPath 
		{
			get 
			{
				return PageUtils.Combine(this.SkinFolder, ExternalSkinFileName);
			}
		}

		/// <summary>
		/// Gets the name of the skin file to load from
		/// </summary>
		protected virtual String ExternalSkinFileName 
		{
			get 
			{
				if (SkinName == null)
					return CreateExternalSkinFileName(null);

				return SkinName;
			}
			set 
			{
				SkinName = value;
			}
		}

		string skinName;
		public string SkinName 
		{
			get 
			{
				return skinName;
			}
			set 
			{
				skinName = value;
			}
		}

		protected virtual string CreateExternalSkinFileName(string path)
		{
			return CreateExternalSkinFileName(path, "Skin-" + this.GetType().Name);
		}

		protected virtual string CreateExternalSkinFileName(string path, string name)
		{
			if(path != null && !path.EndsWith("/"))
				path = path + "/";

			return string.Format("{0}{1}.ascx",path,name);
		}

		protected virtual bool TemplateOnly
		{
			get
			{
				return false;
			}
		}

		protected virtual bool SkinOnly
		{
			get
			{
				return false;
			}
		}

		private Boolean SkinFileExists 
		{
			get 
			{	
				HttpContext context = HttpContext.Current;
				if ( context != null ) 
				{
                    String filePath = context.Server.MapPath(this.SkinPath);
					return System.IO.File.Exists( filePath );
				}
				return true;
			}
		}

		private Boolean DefaultSkinFileExists 
		{
			get 
			{
				HttpContext context = HttpContext.Current;
				if ( context != null ) 
				{
					String filePath = context.Server.MapPath( this.DefaultSkinPath );
					return System.IO.File.Exists( filePath );
				}
				return true;
			}
		}

		protected virtual string DefaultSkinPath 
		{
			get 
			{
                return PageUtils.Combine(this.DefaultSkinFolder, ExternalSkinFileName);
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The template used to override the default UI of the control.
		/// </summary>
		/// <remarks>
		/// All serverside controls that are in the default UI must exist and have the same ID's.
		/// </remarks>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "TODO Template Description" ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		]
		public ITemplate Template 
		{
			get 
			{
				return _template;
			}
			set 
			{
				_template = value;
				ChildControlsCreated = false;
			}
		}
		private ITemplate _template;

		#endregion

		/// <exclude/>
		protected override void CreateChildControls() 
		{
			Controls.Clear();

			Boolean _skinLoaded = false;

			// 1) look for an inline template
			if (!SkinOnly && Template != null ) 
			{
				Template.InstantiateIn( this );
				_skinLoaded = true;
			}

			// 2) Next, look for skin folder
			if (!TemplateOnly && !_skinLoaded) 
			{
				if ( SkinFileExists && this.Page != null ) 
				{
					Control skin = this.Page.LoadControl( this.SkinPath );
					this.Controls.Add( skin );
					_skinLoaded = true;
				}
			}

			// 3) last resort is the default external skin
			if (!TemplateOnly && !_skinLoaded && this.Page != null && this.DefaultSkinFileExists ) 
			{
				Control defaultSkin = this.Page.LoadControl( this.DefaultSkinPath );
				this.Controls.Add( defaultSkin );
				_skinLoaded = true;
			}

			// 4) If none of the skin locations were successful, throw.
			if ( !_skinLoaded ) 
			{
                throw new ArgumentException(string.Format("Skin \"{0}\" Or \"{1}\" Not Found", this.SkinPath, this.DefaultSkinPath));
			}
				
			AttachChildControls();
		}

		
		/// <summary>
		/// Override this method to attach templated or external skin controls to local references.
		/// </summary>
		/// <remarks>
		/// This will only be called if the non-default skin is used.
		/// </remarks>
		protected abstract void AttachChildControls();

		protected override void Render(HtmlTextWriter writer)
		{
			//SourceMarker(true,writer);
			base.Render (writer);
			//SourceMarker(false,writer);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		protected void SourceMarker(bool isStart, HtmlTextWriter writer)
		{
           
			if(isStart)
			{
				writer.WriteLine("<!-- Start: {0} -->", this.GetType());
                
				if(System.IO.File.Exists(this.SkinPath))
					writer.WriteLine("<!-- Skin Path: {0} -->", this.SkinPath);
				else if(Template != null)
					writer.WriteLine("<!-- Inline Skin: {0} -->", true);
				else
					writer.WriteLine("<!-- Skin Path: {0} -->", this.DefaultSkinPath);

			}
			else
				writer.WriteLine("<!-- End: {0} -->", this.GetType());
		}



	}
}
