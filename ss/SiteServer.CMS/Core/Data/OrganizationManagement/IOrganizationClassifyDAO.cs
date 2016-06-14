using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IOrganizationClassifyDAO : ITreeDAO
    {
        int Insert(int publishmentSystemID, int parentID, string itemName, string itemIndexName);

        int Insert(OrganizationClassifyInfo info);

        void Update(OrganizationClassifyInfo info);

        OrganizationClassifyInfo GetInfo(int itemID);

        OrganizationClassifyInfo GetInfoByNew(int itemID);

        void Delete(int deleteID);

        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultInfo(int publishmentSystemID);


        OrganizationClassifyInfo GetFirstInfo();

        ArrayList GetInfoList(int parentID);

        ArrayList GetInfoList(string whereStr);


    }
}
