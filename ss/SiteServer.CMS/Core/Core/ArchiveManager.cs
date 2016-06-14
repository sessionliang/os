using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public class ArchiveManager
	{
        public static void CreateArchiveTableIfNotExists(PublishmentSystemInfo publishmentSystemInfo, string tableName)
        {
            if (!BaiRongDataProvider.TableStructureDAO.IsTableExists(TableManager.GetTableNameOfArchive(tableName)))
            {
                try
                {
                    BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTableOfArchive(tableName);
                }
                catch { }
            }
        }
	}
}
