using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	/// <summary>
	/// Inteface for the MenuDisplay Data Access Object
	/// </summary>
	public interface IMenuDisplayDAO
	{

		void Insert(MenuDisplayInfo menuDisplayInfo);

		void CreateDefaultMenuDisplayInfo(int publishmentSystemID);

		void Update(MenuDisplayInfo menuDisplayInfo);

		void SetDefault(int menuDisplayID);

		void Delete(int menuDisplayID);

		MenuDisplayInfo GetMenuDisplayInfo(int menuDisplayID);

		MenuDisplayInfo GetMenuDisplayInfoByMenuDisplayName(int publishmentSystemID, string menuDisplayName);

		MenuDisplayInfo GetDefaultMenuDisplayInfo(int publishmentSystemID);

		string GetMenuDisplayName(int menuDisplayID);

		int GetMenuDisplayIDByName(int publishmentSystemID, string menuDisplayName);

		string GetImportMenuDisplayName(int publishmentSystemID, string menuDisplayName);

		IEnumerable GetDataSource(int publishmentSystemID);

		ArrayList GetMenuDisplayInfoArrayList(int publishmentSystemID);

		ArrayList GetMenuDisplayNameCollection(int publishmentSystemID);
	}
}
