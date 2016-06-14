using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BaiRong.Controls
{

	/// <summary>
	/// This control serves two distincts purposes:
	/// - it marks the location where the Master Page will be inserted into the Page
	/// - it contains the various Content sections that will be matched to the Master Page's
	///   Region controls (based on their ID's).
	/// </summary>
	public class ContentContainer : PlaceHolder, INamingContainer
	{

		private ArrayList _contents = new ArrayList();
		protected override void AddParsedSubObject(object obj)
		{
			if (obj is Content)
			{
				_contents.Add(obj);
			}
			else
			{
				throw new Exception("The ContentContainer control can only contain content controls");
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (MasterPageFile == null)
				throw new Exception("You need to set the MasterPageFile property");

			// Load the master page
			Control masterPage = Page.LoadControl(MasterPageFile);

			foreach (Content content in _contents)
			{
				// Look for a region with the same ID as the content control
				Control region = masterPage.FindControl(content.ID);
				if (region == null)
				{
					throw new Exception("Could not find matching region for content with ID '" + content.ID + "'");
				}

				// Set the Content control's TemplateSourceDirectory to be the one from the
				// page.  Otherwise, it would end up using the one from the Master Pages user control,
				// which would be incorrect.
				content._templateSourceDirectory = TemplateSourceDirectory;

				// Clear out any default content that the region might have
				region.Controls.Clear();

				// Move the content control into its designated region
				region.Controls.Add(content);
			}

			// Add the master page to the content container control
			Controls.Add(masterPage);

			base.OnInit(e);
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The MasterPage that specifies the layout used to render the contained content.")
		]
		public string MasterPageFile
		{
			get
			{
				return (string)ViewState["MasterPageFile"];
			}
			set
			{
				ViewState["MasterPageFile"] = value;
				ChildControlsCreated = false;
			}
		}
	}

	/// <summary>
	/// The control marks a place holder for content in a master page.
	/// </summary>
	public class Region : PlaceHolder, INamingContainer { }

	/// <summary>
	/// This control contains the content for a particular region
	/// </summary>
	[ToolboxData("<{0}:Content width=300 height=100 runat=server></{0}:Content>")]
	public class Content : Panel
	{

		internal string _templateSourceDirectory;

		public override string TemplateSourceDirectory
		{
			get
			{
				return _templateSourceDirectory;
			}
		}
	}
}

