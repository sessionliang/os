using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IScenceDAO
    {
        ScenceInfo GetScenceInfo(int scenceID);

        void UpdateClickNum(int scenceID, int publishmentSystemID);
    }
}
