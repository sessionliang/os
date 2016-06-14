using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ITagStyleDAO
	{
        int Insert(TagStyleInfo tagStyleInfo);

        void Update(TagStyleInfo tagStyleInfo);

        void Delete(int styleID);

        TagStyleInfo GetTagStyleInfo(int styleID);

        TagStyleInfo GetTagStyleInfo(int publishmentSystemID, string elementName, string styleName);

        IEnumerable GetDataSource(int publishmentSystemID, string elementName);

        ArrayList GetStyleNameArrayList(int publishmentSystemID, string elementName);

        ArrayList GetTagStyleInfoArrayList(int publishmentSystemID);

        string GetImportStyleName(int publishmentSystemID, string elementName, string styleName); 
	}
}
