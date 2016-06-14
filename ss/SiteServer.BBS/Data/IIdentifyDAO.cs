using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IIdentifyDAO
    {
        List<IdentifyInfo> GetIdentifyList(int publishmentSystemID);

        //根据ID取主题鉴定表中的信息
        IdentifyInfo GetIdentifyInfo(int id);

        //后台主题鉴定的更新
        void Update(int publishmentSystemID, IdentifyInfo info);

        //根据ID进行主题鉴定的删除
        void Delete(int publishmentSystemID, int id);

        //添加记录
        void Insert(int publishmentSystemID, IdentifyInfo info);

        void UpdateTaxisToUp(int publishmentSystemID, int id);

        void UpdateTaxisToDown(int publishmentSystemID, int id);

        string GetSelectCommend(int publishmentSystemID);

        Hashtable GetIdentifyInfoHashtable(int publishmentSystemID);

        void CreateDefaultIdentify(int publishmentSystemID);
    }
}
