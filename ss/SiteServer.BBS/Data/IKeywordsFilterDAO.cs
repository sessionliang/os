using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using System.Data;

namespace SiteServer.BBS
{
    public interface IKeywordsFilterDAO
    {
        void Delete(int publishmentSystemID, int id);

        //根据分类表中ID删除敏感词汇
        void DelByCategoryID(int publishmentSystemID, int categoryID);

        //往敏感词汇表中添加数据
        void Insert(int publishmentSystemID, KeywordsFilterInfo info);

        //修改敏感词汇表中的信息
        void Update(int publishmentSystemID, KeywordsFilterInfo info);

        string GetSelectCommend(int publishmentSystemID, int grade, int categoryid, string keyword);

        //根据ID取信息
        KeywordsFilterInfo GetKeywordsFilterInfo(int id);

        //取敏感词分类表中的信息
        List<KeywordsFilterInfo> GetKeywordsFilterList(int publishmentSystemID);

        //根据敏感级别取敏感词汇
        List<KeywordsFilterInfo> GetKeywordsByGrade(int publishmentSystemID, int grade);
    }
}
