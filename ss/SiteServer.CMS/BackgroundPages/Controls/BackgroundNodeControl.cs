using System.Collections;
using System.Web;
using System.Web.UI;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages
{

	public class BackgroundNodeControl : Control
	{

		ArrayList allPublishmentSystemIDArrayList = new ArrayList();
		ArrayList managedPublishmentSystemIDArrayList = new ArrayList();

		public ArrayList AllPublishmentSystemIDArrayList
		{
			set
			{
				allPublishmentSystemIDArrayList = value;
			}
		}

		public ArrayList ManagedPublishmentSystemIDArrayList
		{
			set
			{
				managedPublishmentSystemIDArrayList = value;
			}
		}

		protected override void Render(HtmlTextWriter output)
		{
			
			output.WriteLine("<table width='100%' cellpadding='4' cellspacing='0' border='0'>");
			int count = 1;
			foreach (int publishmentSystemID in allPublishmentSystemIDArrayList)
			{
				PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
				string imageName = "cantedit";			
				if (managedPublishmentSystemIDArrayList.Contains(publishmentSystemID))
				{
					imageName = "canedit";
				}

				string space = "";
				if (count % 4 == 0)
				{
					space = "<TR>";
				}

				string withENName = "";
				if (HttpContext.Current.Request.QueryString["ENName"] != null)
				{
					withENName = "&ENName=" + HttpContext.Current.Request.QueryString["ENName"];
				}
				string content = string.Format(@"
					<TD height=20><img id='PublishmentSystemImage_{0}' align='absmiddle' border='0' src='../pic/{1}.gif'/>
					<A href='background_bgRoleAddSelectNode.aspx?PublishmentSystemID={0}{2}'>{3}</A>{4}</TD>
				", publishmentSystemID, imageName, withENName, psInfo.PublishmentSystemName, space);
				output.WriteLine(content);
				count++;
			}
			output.WriteLine("</TABLE>");
		}
	}
}
