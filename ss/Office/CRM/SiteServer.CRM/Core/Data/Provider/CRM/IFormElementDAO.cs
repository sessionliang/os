using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.CRM.Core
{
    public interface IFormElementDAO
    {
        int Insert(FormElementInfo elementInfo);

        int InsertWithTaxis(FormElementInfo elementInfo);

        void InsertWithTransaction(FormElementInfo info, IDbTransaction trans);

        void InsertStyleItems(ArrayList styleItems);

        void DeleteStyleItems(int formElementID);

        ArrayList GetElementItemArrayList(int formElementID);

        void Update(FormElementInfo info);

        void Delete(int pageID, int groupID, string attributeName);

        void Delete(int pageID, int groupID);

        void Delete(int formElementID);

        List<FormElementInfo> GetFormElementInfoList(int pageID, int groupID);

        bool IsExists(int pageID, int groupID, string attributeName);

        FormElementInfo GetFormElementInfo(int formElementID);

        FormElementInfo GetFormElementInfo(int pageID, int groupID, string attributeName);

        ArrayList GetFormElementInfoWithItemsArrayList(int groupID, string attributeName);

        int GetNewFormElementInfoTaxis(int pageID, int groupID);

        void TaxisUp(int tableStyleID);

        void TaxisDown(int tableStyleID);
    }
}
