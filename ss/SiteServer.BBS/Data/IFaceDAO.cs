using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IFaceDAO
    {
        List<FaceInfo> GetFaces(int publishmentSystemID);

        List<FaceInfo> GetFaces(int publishmentSystemID, bool isEnabled);

        FaceInfo GetFaceInfo(int publishmentSystemID, int id);

        string GetFirstFaceName(int publishmentSystemID);

        void Delete(int publishmentSystemID, int id);

        void Update(FaceInfo info);

        void Insert(FaceInfo info);

        void UpdateTaxisToUp(int publishmentSystemID, int id);

        void UpdateTaxisToDown(int publishmentSystemID, int id);

        int GetMaxTaxis(int publishmentSystemID);

        int GetMinTaxis(int publishmentSystemID);

        void CreateDefaultFace(int publishmentSystemID);
    }
}