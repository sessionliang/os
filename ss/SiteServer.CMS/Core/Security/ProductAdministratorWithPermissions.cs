using System.Collections;
using System.Web;
using System.Web.Caching;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using SiteServer.CMS.Model;


using System.Collections.Generic;

namespace SiteServer.CMS.Core.Security
{
	public class ProductAdministratorWithPermissions : AdministratorWithPermissions
	{
		private SortedList websitePermissionSortedList = null;
		private SortedList channelPermissionSortedList = null;
        private SortedList govInteractPermissionSortedList = null;
        private ArrayList channelPermissionArrayListIgnoreNodeID = null;
        private List<int> publishmentSystemIDList = null;
        private ArrayList owningNodeIDArrayList = null;

		#region user cache keys

		private readonly string websitePermissionSortedListKey;
		private readonly string channelPermissionSortedListKey;
        private readonly string govInteractPermissionSortedListKey;
        private readonly string channelPermissionArrayListIgnoreNodeIDKey;
        private readonly string publishmentSystemIDListKey;
        private readonly string owningNodeIDArrayListKey;

		#endregion

        public ProductAdministratorWithPermissions(string userName, bool isAnonymous)
            : base(userName, isAnonymous)
		{
            this.websitePermissionSortedListKey = PermissionsManager.GetWebsitePermissionSortedListKey(userName, AppManager.CMS.AppID);
            this.channelPermissionSortedListKey = PermissionsManager.GetChannelPermissionSortedListKey(userName, AppManager.CMS.AppID);
            this.govInteractPermissionSortedListKey = PermissionsManager.GetGovInteractPermissionSortedListKey(userName, AppManager.CMS.AppID);
            this.channelPermissionArrayListIgnoreNodeIDKey = PermissionsManager.GetChannelPermissionArrayListIgnoreNodeIDKey(userName, AppManager.CMS.AppID); ;
            this.publishmentSystemIDListKey = PermissionsManager.GetPublishmentSystemIDKey(userName, AppManager.CMS.AppID);
            this.owningNodeIDArrayListKey = PermissionsManager.GetOwningNodeIDArrayListKey(userName, AppManager.CMS.AppID);
		}

		public SortedList WebsitePermissionSortedList
		{
			get
			{
				if (this.websitePermissionSortedList == null)
				{
                    if (!string.IsNullOrEmpty(base.userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(websitePermissionSortedListKey) != null)
                        {
                            this.websitePermissionSortedList = (SortedList)CacheUtils.Get(websitePermissionSortedListKey);
                        }
                        else
                        {
                            if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                            {
                                ArrayList allWebsitePermissionArrayList = new ArrayList();
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.WebsitePermissions)
                                {
                                    allWebsitePermissionArrayList.Add(permission.Name);
                                }

                                this.websitePermissionSortedList = new SortedList();
                                if (this.PublishmentSystemIDList.Count > 0)
                                {
                                    foreach (int publishmentSystemID in this.PublishmentSystemIDList)
                                    {
                                        websitePermissionSortedList.Add(publishmentSystemID, allWebsitePermissionArrayList);
                                    }
                                }
                            }
                            else
                            {
                                this.websitePermissionSortedList = DataProvider.SystemPermissionsDAO.GetWebsitePermissionSortedList(this.Roles);
                            }
                            CacheUtils.Insert(websitePermissionSortedListKey, this.websitePermissionSortedList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
				}
				if (this.websitePermissionSortedList == null)
				{
					this.websitePermissionSortedList = new SortedList();
				}
				return websitePermissionSortedList;
			}
		}

		public SortedList ChannelPermissionSortedList
		{
			get
			{
				if (this.channelPermissionSortedList == null)
				{
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(channelPermissionSortedListKey) != null)
                        {
                            this.channelPermissionSortedList = (SortedList)CacheUtils.Get(channelPermissionSortedListKey);
                        }
                        else
                        {
                            if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                            {
                                ArrayList allChannelPermissionArrayList = new ArrayList();
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.ChannelPermissions)
                                {
                                    allChannelPermissionArrayList.Add(permission.Name);
                                }

                                this.channelPermissionSortedList = new SortedList();

                                if (this.PublishmentSystemIDList.Count > 0)
                                {
                                    foreach (int publishmentSystemID in this.PublishmentSystemIDList)
                                    {
                                        this.channelPermissionSortedList.Add(publishmentSystemID, allChannelPermissionArrayList);
                                    }
                                }
                            }
                            else
                            {
                                this.channelPermissionSortedList = DataProvider.SystemPermissionsDAO.GetChannelPermissionSortedList(this.Roles);
                            }
                            CacheUtils.Insert(channelPermissionSortedListKey, this.channelPermissionSortedList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
				}
				if (this.channelPermissionSortedList == null)
				{
					this.channelPermissionSortedList = new SortedList();
				}
				return channelPermissionSortedList;
			}
		}

        public ArrayList ChannelPermissionArrayListIgnoreNodeID
        {
            get
            {
                if (this.channelPermissionArrayListIgnoreNodeID == null)
                {
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(channelPermissionArrayListIgnoreNodeIDKey) != null)
                        {
                            this.channelPermissionArrayListIgnoreNodeID = (ArrayList)CacheUtils.Get(channelPermissionArrayListIgnoreNodeIDKey);
                        }
                        else
                        {
                            if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                            {
                                this.channelPermissionArrayListIgnoreNodeID = new ArrayList();
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.ChannelPermissions)
                                {
                                    this.channelPermissionArrayListIgnoreNodeID.Add(permission.Name);
                                }
                            }
                            else
                            {
                                this.channelPermissionArrayListIgnoreNodeID = DataProvider.SystemPermissionsDAO.GetChannelPermissionArrayListIgnoreNodeID(this.Roles);
                            }
                            CacheUtils.Insert(channelPermissionArrayListIgnoreNodeIDKey, this.channelPermissionArrayListIgnoreNodeID, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
                }
                if (this.channelPermissionArrayListIgnoreNodeID == null)
                {
                    this.channelPermissionArrayListIgnoreNodeID = new ArrayList();
                }
                return channelPermissionArrayListIgnoreNodeID;
            }
        }

        public SortedList GovInteractPermissionSortedList
        {
            get
            {
                if (this.govInteractPermissionSortedList == null)
                {
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(govInteractPermissionSortedListKey) != null)
                        {
                            this.govInteractPermissionSortedList = (SortedList)CacheUtils.Get(govInteractPermissionSortedListKey);
                        }
                        else
                        {
                            if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                            {
                                ArrayList allGovInteractPermissionArrayList = new ArrayList();
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.GovInteractPermissions)
                                {
                                    allGovInteractPermissionArrayList.Add(permission.Name);
                                }

                                this.govInteractPermissionSortedList = new SortedList();

                                if (this.PublishmentSystemIDList.Count > 0)
                                {
                                    foreach (int publishmentSystemID in this.PublishmentSystemIDList)
                                    {
                                        this.govInteractPermissionSortedList.Add(publishmentSystemID, allGovInteractPermissionArrayList);
                                    }
                                }
                            }
                            else
                            {
                                this.govInteractPermissionSortedList = DataProvider.GovInteractPermissionsDAO.GetPermissionSortedList(this.userName);
                            }
                            CacheUtils.Insert(govInteractPermissionSortedListKey, this.govInteractPermissionSortedList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
                }
                if (this.govInteractPermissionSortedList == null)
                {
                    this.govInteractPermissionSortedList = new SortedList();
                }
                return govInteractPermissionSortedList;
            }
        }

        //public string[] Roles
        //{
        //    get
        //    {
        //        if (base.roles == null)
        //        {
        //            if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
        //            {
        //                if (CacheUtils.Get(base.rolesKey) != null)
        //                {
        //                    base.roles = (string[])CacheUtils.Get(base.rolesKey);
        //                }
        //                else
        //                {
        //                    if (AdminFactory.IsActiveDirectory && StringUtils.EqualsIgnoreCase(userName, AdminFactory.ADAccount))
        //                    {
        //                        base.roles = new string[] { EPredefinedRoleUtils.GetValue(EPredefinedRole.ConsoleAdministrator), EPredefinedRoleUtils.GetValue(EPredefinedRole.Administrator) };
        //                    }
        //                    else
        //                    {
        //                        base.roles = RoleManager.GetRolesForUser(userName);
        //                    }
        //                    CacheUtils.Insert(rolesKey, base.roles, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
        //                }
        //            }
        //        }
        //        if (roles != null && roles.Length > 0)
        //        {
        //            return base.roles;
        //        }
        //        else
        //        {
        //            return new string[]{EPredefinedRoleUtils.GetValue(EPredefinedRole.Everyone)};
        //        }
        //    }
        //}

        //public bool IsInRole(string role)
        //{
        //    foreach (string r in this.Roles)
        //    {
        //        if (role == r) return true;
        //    }
        //    return false;
        //}

        public List<int> PublishmentSystemIDList
        {
            get
            {
                if (this.publishmentSystemIDList == null)
                {
                    if (CacheUtils.Get(publishmentSystemIDListKey) != null)
                    {
                        this.publishmentSystemIDList = (List<int>)CacheUtils.Get(publishmentSystemIDListKey);
                    }
                    else
                    {
                        if (EPredefinedRoleUtils.IsConsoleAdministrator(this.Roles))
                        {
                            this.publishmentSystemIDList = PublishmentSystemManager.GetPublishmentSystemIDList();
                        }
                        else if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                        {
                            List<int> thePublishmentSystemIDList = BaiRongDataProvider.AdministratorDAO.GetPublishmentSystemIDList(userName);
                            this.publishmentSystemIDList = new List<int>();
                            foreach (int publishmentSystemID in PublishmentSystemManager.GetPublishmentSystemIDList())
                            {
                                if (thePublishmentSystemIDList != null && thePublishmentSystemIDList.Contains(publishmentSystemID))
                                {
                                    this.publishmentSystemIDList.Add(publishmentSystemID);
                                }
                            }
                        }
                        else
                        {
                            this.publishmentSystemIDList = new List<int>();
                            foreach (int publishmentSystemID in this.WebsitePermissionSortedList.Keys)
                            {
                                this.publishmentSystemIDList.Add(publishmentSystemID);
                            }
                        }

                        if (this.publishmentSystemIDList == null)
                        {
                            this.publishmentSystemIDList = new List<int>();
                        }

                        CacheUtils.Insert(publishmentSystemIDListKey, this.publishmentSystemIDList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                    }
                }
                return this.publishmentSystemIDList;
            }
        }

        public ArrayList OwningNodeIDArrayList
        {
            get
            {
                if (this.owningNodeIDArrayList == null)
                {
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(this.owningNodeIDArrayListKey) != null)
                        {
                            this.owningNodeIDArrayList = (ArrayList)CacheUtils.Get(this.owningNodeIDArrayListKey);
                        }
                        else
                        {
                            this.owningNodeIDArrayList = new ArrayList();

                            if (!PermissionsManager.Current.IsSystemAdministrator)
                            {
                                foreach (int nodeID in ProductPermissionsManager.Current.ChannelPermissionSortedList.Keys)
                                {
                                    this.owningNodeIDArrayList.Add(nodeID);
                                    ArrayList descendantNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
                                    this.owningNodeIDArrayList.AddRange(descendantNodeIDArrayList);
                                }
                            }

                            CacheUtils.Insert(this.owningNodeIDArrayListKey, this.owningNodeIDArrayList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
                }
                if (this.owningNodeIDArrayList == null)
                {
                    this.owningNodeIDArrayList = new ArrayList();
                }
                return owningNodeIDArrayList;
            }
        }

        public void ClearCache()
        {
            this.websitePermissionSortedList = null;
            this.channelPermissionSortedList = null;
            this.govInteractPermissionSortedList = null;
            this.channelPermissionArrayListIgnoreNodeID = null;
            this.publishmentSystemIDList = null;
            this.owningNodeIDArrayList = null;

            CacheUtils.Remove(this.websitePermissionSortedListKey);
            CacheUtils.Remove(this.channelPermissionSortedListKey);
            CacheUtils.Remove(this.govInteractPermissionSortedListKey);
            CacheUtils.Remove(this.channelPermissionArrayListIgnoreNodeIDKey);
            CacheUtils.Remove(this.publishmentSystemIDListKey);
            CacheUtils.Remove(this.owningNodeIDArrayListKey);
        }

        public static ProductAdministratorWithPermissions GetProductAnonymousUserWithPermissions()
        {
            ProductAdministratorWithPermissions userWithPermissions = new ProductAdministratorWithPermissions(AdminManager.AnonymousUserName, true);
            return userWithPermissions;
        }
	}
}
