using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicCategoryClassDAO
	{
        void Insert(GovPublicCategoryClassInfo categoryClassInfo);

        void Update(GovPublicCategoryClassInfo categoryClassInfo);

        void Delete(string classCode, int publishmentSystemID);

        GovPublicCategoryClassInfo GetCategoryClassInfo(string classCode, int publishmentSystemID);

        ArrayList GetCategoryClassInfoArrayList(int publishmentSystemID, ETriState isSystem, ETriState isEnabled);

        string GetContentAttributeName(string classCode, int publishmentSystemID);

        bool IsExists(string classCode, int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        ArrayList GetClassCodeArrayList(int publishmentSystemID);

        int GetCount(int publishmentSystemID);

        ArrayList GetClassNameArrayList(int publishmentSystemID);

        bool UpdateTaxisToUp(string classCode, int publishmentSystemID);

        bool UpdateTaxisToDown(string classCode, int publishmentSystemID);
	}
}
