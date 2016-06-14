using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ICountDAO
    {
        void Insert(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType, int countNum);
        void AddCountNum(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType);
        void DeleteByRelatedTableName(string applicationName, string relatedTableName);
        void DeleteByIdentity(string applicationName, string relatedTableName, string relatedIdentity);
        bool IsExists(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType);
        int GetCountNum(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType);
        int GetCountNum(string applicationName, string relatedTableName, int publishmentSystemID, ECountType countType);
    }
}
