using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;
using BaiRong.Core;

namespace SiteServer.Project.Core
{
    public interface IHotfixDAO
    {
        void Insert(HotfixInfo hotfixInfo);

        void Update(HotfixInfo hotfixInfo);

        void Delete(int id);

        HotfixInfo GetHotfixInfo(int id);

        string GetFileUrl(int id);

        void AddDownloadCount(int id);

        ArrayList GetHotfixInfoArrayListEnabled();

        DictionaryEntryArrayList GetHotfixInfoDictionaryEntryArrayListEnabled();

        string GetSelectSqlString();

        string GetSortFieldName();
    }
}
