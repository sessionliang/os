using System;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

namespace BaiRong.Core
{
	/// <summary>
	/// TabCollection is a container for all of the tabs.
	/// </summary>
	[Serializable]
	public class TabCollection
	{
		private Tab[] _tabs;
		/// <summary>
		/// Property Tabs (Tab[])
		/// </summary>
		[XmlArray("Tabs")]
		public Tab[] Tabs
		{
			get {  return this._tabs; }
			set {  this._tabs = value; }
		}

		public TabCollection()
		{

		}

		public TabCollection(Tab[] tabs)
		{
			this._tabs = tabs;
		}

		/// <summary>
		/// Returns the current instance of the TabCollection
		/// </summary>
		/// <returns></returns>
		public static TabCollection GetTabs(string filePath)
		{
			if(filePath.StartsWith("/") || filePath.StartsWith("~"))
				filePath = HttpContext.Current.Server.MapPath(filePath);

			TabCollection tc = CacheUtils.Get(filePath) as TabCollection;
			if(tc == null)
			{
				tc = (TabCollection)Serializer.ConvertFileToObject(filePath,typeof(TabCollection));
				CacheUtils.Max(filePath,tc,new CacheDependency(filePath));
			}
			return tc;
		}
	}
}
