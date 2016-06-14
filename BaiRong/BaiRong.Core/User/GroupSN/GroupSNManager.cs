using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core
{
    public sealed class GroupSNManager
	{
        private GroupSNManager()
		{
			
		}

        public static string GetCurrentGroupSN()
        {
            if (FileConfigManager.Instance.IsSaas)
            {
                return EIntegrationTypeUtils.GetValue(FileConfigManager.Instance.SSOConfig.IntegrationType) + ":" + AdminManager.Current.UserName;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetGroupSN(string adminUserName)
        {
            if (FileConfigManager.Instance.IsSaas)
            {
                return EIntegrationTypeUtils.GetValue(FileConfigManager.Instance.SSOConfig.IntegrationType) + ":" + adminUserName;
            }
            else
            {
                return string.Empty;
            }
        }
	}
}
