using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IInputDAO
	{
		int Insert(InputInfo inputInfo);

        void Update(InputInfo inputInfo);

		void Delete(int inputID);

        InputInfo GetInputInfo(string inputName, int publishmentSystemID);

        InputInfo GetInputInfo(string inputName, int publishmentSystemID,int itemID);

        InputInfo GetInputInfo(int inputID);

        InputInfo GetInputInfoAsPossible(int inputID, int publishmentSystemID);

        int GetInputIDAsPossible(string inputName, int publishmentSystemID);

        InputInfo GetLastAddInputInfo(int publishmentSystemID);

        bool IsExists(string inputName, int publishmentSystemID);

		IEnumerable GetDataSource(int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID,int classifyID);

        IEnumerable GetDataSource(int publishmentSystemID, int classifyID, string inputName, string dateFrom, string dateTo);

        ArrayList GetInputIDArrayList(int publishmentSystemID);

        ArrayList GetInputNameArrayList(int publishmentSystemID);

        string GetImportInputName(string inputName, int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, int inputID);

        bool UpdateTaxisToDown(int publishmentSystemID, int inputID);

        bool UpdateTaxisToUp(int publishmentSystemID, int inputID, int classifyID);

        bool UpdateTaxisToDown(int publishmentSystemID, int inputID, int classifyID);

        void DeleteByClassifyID(int itemID);
         

        string GetSelectCommand(int classifyID);

        /// <summary>
        /// 加载有权限的分类下的表单
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="inputName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="classifyIDs"></param>
        /// <returns></returns>
        IEnumerable GetDataSource(int publishmentSystemID, int classifyID, string inputName, string dateFrom, string dateTo,ArrayList classifyIDs);
    }
}
