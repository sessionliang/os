using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IRelatedFieldDAO
	{
		int Insert(RelatedFieldInfo relatedFieldInfo);

        void Update(RelatedFieldInfo relatedFieldInfo);

        void Delete(int relatedFieldID);

        RelatedFieldInfo GetRelatedFieldInfo(int relatedFieldID);

        RelatedFieldInfo GetRelatedFieldInfo(int publishmentSystemID, string relatedFieldName);

        string GetRelatedFieldName(int relatedFieldID);

		IEnumerable GetDataSource(int publishmentSystemID);

		ArrayList GetRelatedFieldInfoArrayList(int publishmentSystemID);

        ArrayList GetRelatedFieldNameArrayList(int publishmentSystemID);

        string GetImportRelatedFieldName(int publishmentSystemID, string relatedFieldName);
	}
}
