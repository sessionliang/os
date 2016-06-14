using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Text.Sgml;
using System;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core.TemplateParser.Element;

namespace SiteServer.BBS.Core.TemplateParser
{
    public class DataUtility
    {
        public static int GetForumIDByLevel(int publishmentSystemID, int forumID, int upLevel, int topLevel)
        {
            int theForumID = forumID;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                if (topLevel >= 0)
                {
                    if (topLevel > 0)
                    {
                        if (topLevel < forumInfo.ParentsCount)
                        {
                            ArrayList parentIDStrArrayList = TranslateUtils.StringCollectionToArrayList(forumInfo.ParentsPath);
                            if (parentIDStrArrayList[topLevel] != null)
                            {
                                string parentIDStr = (string)parentIDStrArrayList[topLevel];
                                theForumID = int.Parse(parentIDStr);
                            }
                        }
                    }
                    else
                    {
                        theForumID = forumID;
                    }
                }
                else if (upLevel > 0)
                {
                    if (upLevel < forumInfo.ParentsCount)
                    {
                        ArrayList parentIDStrArrayList = TranslateUtils.StringCollectionToArrayList(forumInfo.ParentsPath);
                        if (parentIDStrArrayList[upLevel] != null)
                        {
                            string parentIDStr = (string)parentIDStrArrayList[forumInfo.ParentsCount - upLevel];
                            theForumID = int.Parse(parentIDStr);
                        }
                    }
                    else
                    {
                        theForumID = forumID;
                    }
                }
            }
            return theForumID;
        }

        public static int GetForumColumns(int publishmentSystemID, int columns, int forumID)
        {
            if (columns == 0 && forumID > 0)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
                if (forumInfo != null)
                {
                    columns = forumInfo.Columns;
                }
            }

            if (columns == 0)
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                columns = additional.DisplayColumns;
            }

            return columns;
        }

        public static int GetForumIDByForumIndexOrForumName(int publishmentSystemID, int forumID, string forumIndex, string forumName)
        {
            int retval = forumID;

            if (!string.IsNullOrEmpty(forumIndex))
            {
                int theForumID = DataProvider.ForumDAO.GetForumIDByIndexName(publishmentSystemID, forumIndex);
                if (theForumID != 0)
                {
                    retval = theForumID;
                }
            }
            if (!string.IsNullOrEmpty(forumName))
            {
                int theForumID = DataProvider.ForumDAO.GetForumIDByParentIDAndForumName(publishmentSystemID, retval, forumName, true);
                if (theForumID == 0)
                {
                    theForumID = DataProvider.ForumDAO.GetForumIDByParentIDAndForumName(publishmentSystemID, 0, forumName, true);
                }
                if (theForumID != 0)
                {
                    retval = theForumID;
                }
            }

            return retval;
        }

        public static IEnumerable GetForumsDataSource(int publishmentSystemID, int forumID, string group, string groupNot, int startNum, int totalNum, string orderByString, string where)
        {
            string sqlWhereString = DataProvider.ForumDAO.GetWhereString(publishmentSystemID, group, groupNot, where);
            return DataProvider.ForumDAO.GetParserDataSource(publishmentSystemID, forumID, startNum, totalNum, sqlWhereString, orderByString);
        }

        public static IEnumerable GetThreadsDataSource(int publishmentSystemID, int forumID, int categoryID, string type, int startNum, int totalNum, string orderByString)
        {
            return GetThreadsDataSource(publishmentSystemID, forumID, categoryID, type, startNum, totalNum, true, false, string.Empty, orderByString, false);
        }

        public static IEnumerable GetThreadsDataSource(int publishmentSystemID, int forumID, int categoryID, string type, int startNum, int totalNum, bool isTopExists, bool isTop, string where, string orderByString, bool isAllChildren)
        {
            IEnumerable ie = null;

            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                string sqlWhereString = DataProvider.ThreadDAO.GetParserWhereString(publishmentSystemID, categoryID, type, isTopExists, isTop, where);
                ie = DataProvider.ThreadDAO.GetParserDataSourceChecked(publishmentSystemID, forumID, startNum, totalNum, orderByString, sqlWhereString, isAllChildren);
            }

            return ie;
        }

        public static IEnumerable GetPostsDataSource(int publishmentSystemID, int threadID, int startNum, int totalNum, bool isThreadExists, bool isThread, string orderByString, string where)
        {
            IEnumerable ie = null;
            if (threadID > 0)
            {
                ie = DataProvider.PostDAO.GetParserDataSource(publishmentSystemID, threadID, startNum, totalNum, isThreadExists, isThread, orderByString, where);
            }

            return ie;
        }

        public static IEnumerable GetSqlContentsDataSource(int publishmentSystemID, string connectionString, string queryString, string predefined, int startNum, int totalNum, string orderByString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_NavHeader))
                {
                    queryString = DataProvider.NavigationDAO.GetSqlString(publishmentSystemID, ENavType.Header);
                }
                else if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_NavSecondary))
                {
                    queryString = DataProvider.NavigationDAO.GetSqlString(publishmentSystemID, ENavType.Secondary);
                }
                else if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_NavFooter))
                {
                    queryString = DataProvider.NavigationDAO.GetSqlString(publishmentSystemID, ENavType.Footer);
                }
                else if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_Announcement))
                {
                    queryString = DataProvider.AnnouncementDAO.GetSqlString(publishmentSystemID);
                }
                else if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_LinkIcon))
                {
                    queryString = DataProvider.LinkDAO.GetSqlString(publishmentSystemID, true);
                }
                else if (StringUtils.EqualsIgnoreCase(predefined, SqlContents.Predefined_LinkText))
                {
                    queryString = DataProvider.LinkDAO.GetSqlString(publishmentSystemID, false);
                }
            }
            string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(connectionString, queryString, startNum, totalNum, orderByString);
            return BaiRongDataProvider.DatabaseDAO.GetDataSource(connectionString, sqlString);
        }
    }
}
