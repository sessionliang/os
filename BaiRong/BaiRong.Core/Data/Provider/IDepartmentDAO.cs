using System;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IDepartmentDAO
	{
        int Insert(DepartmentInfo departmentInfo);

        void Update(DepartmentInfo departmentInfo);

        void UpdateTaxis(int selectedDepartmentID, bool isSubtract);

        void UpdateCountOfAdmin();

        void Delete(int departmentID);

        int GetNodeCount(int departmentID);

        ArrayList GetDepartmentIDArrayListByParentID(int parentID);

        ArrayList GetDepartmentIDArrayListForDescendant(int departmentID);

        ArrayList GetDepartmentIDArrayListByDepartmentIDCollection(string departmentIDCollection);

        ArrayList GetDepartmentIDArrayListByFirstDepartmentIDArrayList(ArrayList firstIDArrayList);

        DictionaryEntryArrayList GetDepartmentInfoDictionaryEntryArrayList();
	}
}
