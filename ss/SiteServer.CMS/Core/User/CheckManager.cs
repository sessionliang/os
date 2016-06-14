using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;

using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.Core
{
	public class CheckManager
	{
        public static object[] GetUserCheckLevel(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return new object[] { true, publishmentSystemInfo.CheckContentLevel };
            }

            bool isChecked = false;
            int checkedLevel = 0;
            if (publishmentSystemInfo.IsCheckContentUseLevel == false)
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                {
                    isChecked = true;
                }
                else
                {
                    isChecked = false;
                }
            }
            else
            {
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel5))
                {
                    isChecked = true;
                }
                else if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel4))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 4)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 4;
                    }
                }
                else if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel3))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 3)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 3;
                    }
                }
                else if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel2))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 2)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 2;
                    }
                }
                else if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel1))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 1)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 1;
                    }
                }
                else
                {
                    isChecked = false;
                    checkedLevel = 0;
                }
            }
            return new object[] { isChecked, checkedLevel };
        }

        public static bool GetUserCheckLevel(PublishmentSystemInfo publishmentSystemInfo, int nodeID, out int userCheckedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

            object[] pair = CheckManager.GetUserCheckLevel(publishmentSystemInfo, nodeID);
            bool isChecked = (bool)pair[0];
            int checkedLevel = (int)pair[1];
            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }
            userCheckedLevel = checkedLevel;
            return isChecked;
        }

        public static ArrayList GetUserCountArrayListUnChecked()
        {
            ArrayList arraylist = new ArrayList();

            ArrayList tableInfoArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent, EAuxiliaryTableType.GovPublicContent, EAuxiliaryTableType.GovInteractContent, EAuxiliaryTableType.VoteContent, EAuxiliaryTableType.JobContent, EAuxiliaryTableType.UserDefined);

            foreach (AuxiliaryTableInfo tableInfo in tableInfoArrayList)
            {
                arraylist.AddRange(GetUserCountArrayListUnChecked(tableInfo.TableENName));
            }

            return arraylist;
        }

        private static ArrayList GetUserCountArrayListUnChecked(string tableName)
        {
            return DataProvider.BackgroundContentDAO.GetCountArrayListUnChecked(PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.PublishmentSystemIDList, ProductPermissionsManager.Current.OwningNodeIDArrayList, tableName);
        }
	}
}
