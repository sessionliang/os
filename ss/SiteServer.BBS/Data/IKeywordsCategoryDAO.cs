using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IKeywordsCategoryDAO
    {
        List<KeywordsCategoryInfo> GetKeywordsCategoryList(int publishmentSystemID);

        //根据ID取敏感词分类表中的信息
        KeywordsCategoryInfo GetKeywordsCategoryInfo(int categoryID);

        //根据ID进行删除
        void Delete(int categoryID);

        //插入记录
        void Insert(KeywordsCategoryInfo info);

        //插入记录返回主键
        int Add(KeywordsCategoryInfo info);

        //对敏感词分类表中的信息进行修改
        void Update(KeywordsCategoryInfo info);

        //搜索此分类中的敏感词汇的数量
        int KeyWordsFiltersCount(int categoryID);

        //取分类表中记录数量
        int KeywordsCategoryCount(int publishmentSystemID);

        //创建默认的分类
        void CreateDefaultKeywordsCategory(int publishmentSystemID);
    }
}
