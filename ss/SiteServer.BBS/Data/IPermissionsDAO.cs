using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IPermissionsDAO
    {
        Hashtable GetForbiddenHashtable(int publishmentSystemID);

        SortedList GetUserGroupIDWithForbiddenSortedList(string groupSN, int forumID);

        void Save(int publishmentSystemID, int forumID, SortedList userGroupIDWithForbidden);
    }
}