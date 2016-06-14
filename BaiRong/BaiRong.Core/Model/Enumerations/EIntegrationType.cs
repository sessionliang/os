using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EIntegrationType
    {
        None,
        GeXia,
        QCloud,
        Aliyun,
        SiteServer,
    }

    public class EIntegrationTypeUtils
    {
        public static string GetValue(EIntegrationType type)
        {
            if (type == EIntegrationType.None)
            {
                return "None";
            }
            else if (type == EIntegrationType.GeXia)
            {
                return "GeXia";
            }
            else if (type == EIntegrationType.QCloud)
            {
                return "QCloud";
            }
            else if (type == EIntegrationType.Aliyun)
            {
                return "Aliyun";
            }
            else if (type == EIntegrationType.SiteServer)
            {
                return "SiteServer";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EIntegrationType GetEnumType(string typeStr)
        {
            EIntegrationType retval = EIntegrationType.None;

            if (Equals(EIntegrationType.GeXia, typeStr))
            {
                retval = EIntegrationType.GeXia;
            }
            else if (Equals(EIntegrationType.QCloud, typeStr))
            {
                retval = EIntegrationType.QCloud;
            }
            else if (Equals(EIntegrationType.Aliyun, typeStr))
            {
                retval = EIntegrationType.Aliyun;
            }
            else if (Equals(EIntegrationType.SiteServer, typeStr))
            {
                retval = EIntegrationType.SiteServer;
            }

            return retval;
        }

        public static bool Equals(EIntegrationType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EIntegrationType type)
        {
            return Equals(type, typeStr);
        }
    }
}
