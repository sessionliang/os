using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using BaiRong.Core;


namespace SiteServer.CMS.Core.Security
{
	public class ProductPermissionsManager
	{
        private string userName;
        private ProductAdministratorWithPermissions permissions = null;

        private ProductPermissionsManager(string userName)
        {
            this.userName = userName;
        }

        public static ProductAdministratorWithPermissions Current
        {
            get
            {
                ProductPermissionsManager instance = new ProductPermissionsManager(AdminManager.Current.UserName);
                return instance.Permissions;
            }
        }

        public ProductAdministratorWithPermissions Permissions
        {
            get
            {
                if (permissions == null)
                {
                    if (!string.IsNullOrEmpty(this.userName))
                    {
                        permissions = new ProductAdministratorWithPermissions(this.userName, false);
                    }
                    else
                    {
                        permissions = ProductAdministratorWithPermissions.GetProductAnonymousUserWithPermissions();
                    }
                }
                return permissions;
            }
            set { permissions = value; }
        }
	}
}
