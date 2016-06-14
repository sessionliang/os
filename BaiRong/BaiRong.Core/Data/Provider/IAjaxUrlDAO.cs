using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections;

namespace BaiRong.Core.Data.Provider
{
    public interface IAjaxUrlDAO
    {
        void Insert(AjaxUrlInfo ajaxUrlInfo);

        bool InsertForCreate(AjaxUrlInfo ajaxUrlInfo, out int existsID);

        void Delete(string sn);

        DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayList();
        DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForQueueCreate();

        DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForCreate();

        void DeleteByTaskID(int createTaskID);

        void DeleteSameQueue(AjaxUrlInfo ajaxUrlInfo);
    }
}
