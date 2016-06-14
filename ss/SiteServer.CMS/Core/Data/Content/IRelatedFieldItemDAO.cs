using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Core
{
    public interface IRelatedFieldItemDAO
    {
        int Insert(RelatedFieldItemInfo info);

        void Update(RelatedFieldItemInfo info);

        void Delete(int id);

        IEnumerable GetDataSource(int relatedFieldID, int parentID);

        void UpdateTaxisToUp(int id, int parentID);

        void UpdateTaxisToDown(int id, int parentID);

        int GetMaxTaxis(int parentID);

        int GetMinTaxis(int parentID);

        RelatedFieldItemInfo GetRelatedFieldItemInfo(int id);

        ArrayList GetRelatedFieldItemInfoArrayList(int relatedFieldID, int parentID);
    }
}
