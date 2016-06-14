using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IAttachmentDAO
    {
        int Insert(AttachmentInfo info);

        int ImportAttachment(AttachmentInfo info);

        void Update(AttachmentInfo info);

        void Update(int id, int threadID, int postID, bool isInContent, int price, string description);

        void AddDownloadCount(int id);

        int GetDownloadCount(int id);

        void Delete(int publishmentSystemID, List<int> attachIDList);

        void DeleteByPostIDList(int publishmentSystemID, List<int> postIDList);

        void DeleteByThreadIDList(int publishmentSystemID, List<int> threadIDList);

        void DeleteByThreadAll(int publishmentSystemID);
     
        AttachmentInfo GetAttachmentInfo(int id);

        List<int> GetAttachIDList(int publishmentSystemID, int threadID, int postID);

        List<AttachmentInfo> GetList(int publishmentSystemID, int threadID, int postID);

        string GetSqlString(int publishmentSystemID, int threadID, int postID);
    }
}