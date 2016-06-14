using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IAttachmentTypeDAO
    {
        void Insert(AttachmentTypeInfo info);

        void Update(AttachmentTypeInfo info);

        void Delete(int id);

        AttachmentTypeInfo GetAttachmentTypeInfo(int id);

        List<AttachmentTypeInfo> GetList(int publishmentSystemID);
    }
}