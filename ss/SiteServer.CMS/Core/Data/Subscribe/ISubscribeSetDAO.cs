using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface ISubscribeSetDAO
    {
        string TableName
        {
            get;
        }

        SubscribeSetInfo GetSubscribeSetInfo(int publishmentSystemID);

        int Insert(SubscribeSetInfo info);

        void Update(SubscribeSetInfo info);

    }
}
