using SiteServer.CMS.Model.MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SiteServer.CMS.Core
{
    public interface IMlibDAO
    {
        #region SubmissionInfo
        int Insert(SubmissionInfo submissionInfo);

        void Update(SubmissionInfo submissionInfo);
        void DeleteSubmission(int submissionID);

        void ClearSubmission();

        SubmissionInfo GetSubmissionInfo(int downloadID);
        #endregion
        #region 综合

        DataSet GetNodeInfoBySubmissionID(int id);

        DataSet GetContentIDsBySubmissionID(int id);

        DataSet GetContentIDsBySubmissionID1(int id);
        DataSet GetContentIDsAll();

        DataSet GetReferenceTypeList(string where="1=1");
        int InsertReferenceType(string rtName);
        int GetReferenceLogCount(int sid);

        int InsertReferenceLogs(ReferenceLog referenceLogInfo);

        string GetConfigAttr(string key);
        void UpdateConfigAttr(string key, string value);


        DataSet GetRoleCheckLevel(string where = "1=1");

        void UpdateRoleCheckLevel(string roleName, string[] checkLevel);

        DataSet GetRCNode(string where = "1=1");

        void UpdateNodeAdminRoles(int RCID, string[] nodeID);

        int GetSubmissionCount(string where = "1=1");

        int GetDraftCount(int PublishmentSystemID);

        void UpdateSubmissionTaxis(int nodeID, int contentID, bool isUp, int changeTaxis);
        #endregion



    }
}
