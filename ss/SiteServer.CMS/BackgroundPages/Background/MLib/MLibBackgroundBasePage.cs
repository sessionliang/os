using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class MLibBackgroundBasePage : BackgroundBasePage
    {
        public string[] HasNodePermissions(string checklevel)
        {
            var roles = RoleManager.GetRolesForUser(AdminManager.Current.UserName);
            string where = " 1=0 ";
            foreach (var item in roles)
            {
                var ds = DataProvider.MlibDAO.GetRoleCheckLevel("RoleName='" + item + "' and CheckLevel=" + checklevel);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    where += " or RCID=" + ds.Tables[0].Rows[0]["ID"].ToString();
                }
            }

            var nodeIds = DataProvider.MlibDAO.GetRCNode(where);

            string[] returnVal = new string[nodeIds.Tables[0].Rows.Count];
            for (int i = 0; i < returnVal.Length; i++)
            {
                returnVal[i] = nodeIds.Tables[0].Rows[i]["NodeId"].ToString();
            }
            return returnVal;
        }
        public bool HasNodePermissions(string checkLevel, string nodeid) {
            var nodeids = HasNodePermissions(checkLevel);
            foreach (var item in nodeids)
            {
                if (nodeid == item)
                    return true;
            }
            return false;
        }

        public string[] HasCheckLevelPermissions()
        {
            var roles = RoleManager.GetRolesForUser(AdminManager.Current.UserName);
            string where = " 1=0 ";
            foreach (var item in roles)
            {
                where += " or RoleName='" + item + "'";
            }

            var checklevelds = DataProvider.MlibDAO.GetRoleCheckLevel(where);

            string[] returnVal = new string[checklevelds.Tables[0].Rows.Count];
            for (int i = 0; i < returnVal.Length; i++)
            {
                returnVal[i] = checklevelds.Tables[0].Rows[i]["CheckLevel"].ToString();
            }
            return returnVal;
        }

        public bool HasCheckLevelPermissions(string checkLevel)
        {
            var cls = HasCheckLevelPermissions();

            foreach (var item in cls)
            {
                if (checkLevel == item)
                    return true;
            }
            return false;
        }
    }
}
