using System.Web.UI;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System;
using System.Collections;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Pages.MLibManage
{
    public class SystemBasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(UserManager.Current.UserName))
            {
                PageUtils.Redirect("/home/");
            }
        }


        /// <summary>
        /// 获取投稿管理的草稿表名
        /// </summary>
        private string mLibDraftContentTableName;
        public string MLibDraftContentTableName
        {
            get
            {
                if (mLibDraftContentTableName == null)
                {
                    mLibDraftContentTableName = "bairong_MLibDraftContent";
                }
                return mLibDraftContentTableName;
            }
        }


        /// <summary>
        /// 获取投稿默认字段
        /// </summary>
        private string mLibDraftContentAttributeNames;
        public string MLibDraftContentAttributeNames(string tableName)
        {
            ArrayList arraylist = BaiRongDataProvider.AuxiliaryTableDataDAO.GetDefaultTableMetadataInfoArrayList(tableName, EAuxiliaryTableType.ManuscriptContent);

            ArrayList attributeNames = new ArrayList();
            foreach (TableMetadataInfo metadataInfo in arraylist)
            {
                attributeNames.Add(metadataInfo.AttributeName);
            }
            mLibDraftContentAttributeNames = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(attributeNames);

            return mLibDraftContentAttributeNames;
        }


        /// <summary>
        /// 是否有审核权限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="channelPermissionArray"></param>
        /// <returns></returns>
        public static bool HasChannelPermissions(string userName, int publishmentSystemID, int nodeID, string channelPermission)
        {
            if (string.IsNullOrEmpty(userName)) return false;
            if (publishmentSystemID == 0) return false;
            if (nodeID == 0) return false;

            bool returnval = false;
            string[] roles = RoleManager.GetRolesForUser(userName);
            ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
            foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
            {
                ArrayList nodeIDCollections = DataProvider.SystemPermissionsDAO.GetAllPermissionArrayList(roles, itemForPSID, true);
                foreach (SystemPermissionsInfo info in nodeIDCollections)
                {
                    ArrayList nodeIDCollection = TranslateUtils.StringCollectionToArrayList(info.NodeIDCollection);
                    ArrayList channelPermissions = TranslateUtils.StringCollectionToArrayList(info.ChannelPermissions);
                    if (nodeIDCollection.Contains(nodeID.ToString()) && channelPermissions.Contains(channelPermission))
                    {
                        returnval = true; 
                    } 
                }
            }
            return returnval;
        }
        public bool ContentIsChecked(string userName, PublishmentSystemInfo publishmentSystemInfo, int nodeID, out int userCheckedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

            object[] pair = GetUserCheckLevel(userName,publishmentSystemInfo, nodeID);
            bool isChecked = (bool)pair[0];
            int checkedLevel = (int)pair[1];
            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }
            userCheckedLevel = checkedLevel;
            return isChecked;
        }

        public static object[] GetUserCheckLevel(string userName, PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            int publishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(userName) || AdminManager.HasChannelPermissionIsSystemAdministrator(userName))//如果是超级管理员或站点管理员
            {
                return new object[] { true, publishmentSystemInfo.CheckContentLevel };
            }

            bool isChecked = false;
            int checkedLevel = 0;
            if (publishmentSystemInfo.IsCheckContentUseLevel == false)
            {
                if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
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
                if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel5))
                {
                    isChecked = true;
                }
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel4))
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
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel3))
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
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel2))
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
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel1))
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
    }
}
