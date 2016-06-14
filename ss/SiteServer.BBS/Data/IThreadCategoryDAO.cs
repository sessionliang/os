using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS {

    public interface IThreadCategoryDAO
    {
        void Insert(ThreadCategoryInfo info);

        void Update(ThreadCategoryInfo info);

        void Delete(int publishmentSystemID, int categoryID);

        ThreadCategoryInfo GetThreadCategoryInfo(int categoryID);

        bool IsExists(int publishmentSystemID, string categoryName, int forumID);

        DataSet GetDataSource(int publishmentSystemID, string strWhere);

        ArrayList GetCategoryInfoArrayList(int publishmentSystemID, int forumID);

        ArrayList GetForumIDArrayList(int publishmentSystemID);

        ArrayList GetThreadCategoryInfoPairArrayList(int publishmentSystemID);

        void TaxisAdd(int publishmentSystemID, int categoryID, int forumID, int addNum);

        void TaxisSubtract(int publishmentSystemID, int categoryID, int forumID, int subtractNum);

        int GetMaxTaxisByCategoryID(int publishmentSystemID, int categoryID);

        int GetMaxTaxisByForumID(int publishmentSystemID, int forumID);
    }
}
