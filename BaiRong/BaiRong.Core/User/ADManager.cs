using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace BaiRong.Core
{
    public class AdHandler
    {
        protected string m_domainUser;
        protected string m_password;
        protected string m_path;
        protected string m_groupPath;

        public AdHandler()
        {
            Initialize();
        }

        protected void Initialize()
        {
            IDictionary identity = (IDictionary)ConfigurationSettings.GetConfig("identity");

            m_domainUser = identity["userId"].ToString();

            m_password = identity["password"].ToString();

            m_path = identity["path"].ToString();

            m_groupPath = identity["groupPath"].ToString();

        }

        #region GetUser

        /// <summary>

        /// Get user's information by user's id

        /// </summary>

        /// <param name="userID">User's id</param>

        /// <returns>Datatable containning 'Area','Domain','UserID','DisplayName','Department','EmailAddress','MemberOf'</returns>

        public DataTable GetUser(string userID)
        {

            DataTable result = new DataTable();

            result.Columns.AddRange(new DataColumn[]{

new DataColumn("Domain",typeof(String)),

new DataColumn("UserID",typeof(String)),

new DataColumn("FullName",typeof(String)),

new DataColumn("DeptShortName",typeof(String)),

new DataColumn("EmailAddress",typeof(String)),

new DataColumn("MemberOf",typeof(String)),

new DataColumn("UserName",typeof(String)),

new DataColumn("Rank",typeof (String))

});

            string

            propDistinguishedname = "", propDomain = "", propUserID = "", propDisplayName = "", propDepartment = "", propEmailAddress = "", propMemberof = "", propFullName = "";



            //��û�����Ϣ 

            string filter =

            string.Format("(&(objectCategory=person)(mailnickname={0}))", userID);

            string[] propertiesToLoad = new

            string[] { "distinguishedname", "mailnickname", "displayName", "mail" };

            SearchResult r = SearchOne(filter, propertiesToLoad);

            if (r == null)
            {

                return result;

            }

            propDistinguishedname = GetValue(r, "distinguishedname");

            propUserID = GetValue(r, "mailnickname");

            propDomain = ParseDomain(propDistinguishedname);

            propDisplayName = GetValue(r, "displayName");

            propDepartment = ParseDepartment(propDisplayName);

            propEmailAddress = GetValue(r, "mail");

            propFullName = ParseFullName(propDisplayName);

            //����Group

            filter = string.Format(

            "(&(objectClass=group)(member={0}))", propDistinguishedname);//group���� 

            SearchResultCollection rs = SearchBatch(filter, new

            string[] { "displayname" });

            foreach (SearchResult entry in rs)
            {

                string t = GetValue(entry, "displayname");

                if (t != "")
                {

                    propMemberof += t + ",";

                }

            }

            if (propMemberof != string.Empty)
            {

                propMemberof = propMemberof.Substring(0, propMemberof.Length - 1);

            }

            result.Rows.Add(new

            string[] { propDomain, propUserID, propFullName, propDepartment, propEmailAddress, propMemberof, propDisplayName });

            return result;

        }

        #endregion

        #region GetUserGroup

        /// <summary>

        /// Get the user or group by the domain and keyword of display name

        /// </summary>

        /// <param name="keyword">Keyword of display name</param>

        /// <returns>Datatable containning 'DisplayName','Domain' and 'UserID'</returns>

        public DataTable GetUserGroup(string keyword)
        {

            DataTable result = new DataTable();

            result.Columns.AddRange(new DataColumn[]{

new DataColumn("DisplayName",typeof(String)),

new DataColumn("Domain",typeof(String)),

new DataColumn("UserID",typeof(String)),

new DataColumn("userAccountControl",typeof(String)) 

});

            string propDisplayName = "";

            string propDomain = "";

            string propUserID = "";

            string userAccountControl = "";

            string filter = string.Format(

            "(&(|(objectCategory=person)(objectClass=group))(displayname=*{0}*))", keyword);//user���� 

            SearchResultCollection rsGroup = SearchBatch(filter,

            new string[]{"displayname", "distinguishedname", "mailnickname", 

"userAccountControl"});

            foreach (SearchResult entry in rsGroup)
            {

                propDisplayName = GetValue(entry, "displayname");

                propDomain = ParseDomain(GetValue(entry, "distinguishedname"));

                propUserID = GetValue(entry, "mailnickname");

                userAccountControl = GetValue(entry, "userAccountControl");

                if (userAccountControl.Trim() != "66050")
                {

                    result.Rows.Add(new

                    string[] { propDisplayName, propDomain, propUserID, userAccountControl });

                }

            }

            return result;

        }

        #endregion

        #region GetGroupMember

        /// <summary>

        /// ����һ��Group�����֣���ȡ���������Member

        /// </summary>

        /// <param name="strGroupName">group��name,����:"** microsoft(cn.sh)";</param>

        /// <param name="bIncludeSubGroup">�Ƿ��������group��member</param>

        /// <returns>Datatable containning 'DisplayName','EmailAddress' and 'UserID'</returns>

        public DataTable GetGroupMember(string strGroupName, bool bIncludeSubGroup)
        {

            DataTable resultTable = new DataTable();

            resultTable.Columns.AddRange(new DataColumn[]{

new DataColumn("DisplayName",typeof(String)),

new DataColumn("EmailAddress",typeof(String)),

new DataColumn("UserID",typeof(String)),

});

            string strPrex = m_groupPath;

            string strPath = m_path;

            string strUser = m_domainUser;

            string strPassword = m_password;

            string filter = string.Format(

            "(&(objectClass=group)(|(displayname={0})(name={0})))", strGroupName);//����name��displaynameΪָ��ֵ��group���� 

            SearchResult result = this.SearchOne(filter, null);

            if (result == null)
            {

                return resultTable;

            }

            Stack stack = new Stack(); //��ΪҪ������Group,��������stack��֧�ֱ��� 

            stack.Push(result.Properties["adspath"][0].ToString());

            Hashtable htResult = new Hashtable();

            while (stack.Count != 0)
            {

                using (DirectoryEntry cur = new DirectoryEntry())
                {

                    cur.Path = (string)stack.Pop();

                    cur.Username = strUser;

                    cur.Password = strPassword;

                    foreach (string strProp in cur.Properties["member"])//����member����
                    {

                        using (DirectoryEntry member = new DirectoryEntry())
                        {

                            member.Path = strPrex + strProp.ToString();

                            member.Username = strUser;

                            member.Password = strPassword;

                            if (member.SchemaClassName.ToLower().Equals("user"))//user����
                            {

                                if (!htResult.ContainsKey(member.Guid))//һ��user�����ڶ��group�£���˿����ظ����������Ƿ��ظ�
                                {

                                    htResult.Add(member.Guid,

                                    member.Properties["displayName"][0].ToString());//���黹Ҫ����member.Path,�������Ժܷ����ع���Ӧ��DirectoryEntry�������������

                                    string propDisplayName = GetValue(member, "displayname");

                                    string propEmailAddress = GetValue(member, "mail");

                                    string propUserID = GetValue(member, "mailnickname");

                                    resultTable.Rows.Add(new

                                    string[] { propDisplayName, propEmailAddress, propUserID });

                                }

                            }

                            else if (member.SchemaClassName.ToLower().Equals("group") &&

                            bIncludeSubGroup)//group����
                            {

                                stack.Push(member.Path);

                            }

                        }

                    }

                }

            }

            return resultTable;

        }

        #endregion

        #region Protected Method

        protected SearchResult SearchOne(string filter, string[] propertiesToLoad)
        {

            DirectorySearcher searcher = null;

            try
            {

                searcher = GetSearcher(filter, propertiesToLoad);

                return searcher.FindOne();

            }

            finally
            {

                if (searcher != null)
                {

                    if (searcher.SearchRoot != null)
                    {

                        searcher.SearchRoot.Dispose();

                    }

                    searcher.Dispose();

                }

            }

        }

        protected SearchResultCollection SearchBatch(string filter, string[]

        propertiesToLoad)
        {

            DirectorySearcher searcher = null;

            try
            {

                searcher = GetSearcher(filter, propertiesToLoad);

                return searcher.FindAll();

            }

            finally
            {

                if (searcher != null)
                {

                    if (searcher.SearchRoot != null)
                    {

                        searcher.SearchRoot.Dispose();

                    }

                    searcher.Dispose();

                }

            }

        }

        protected DirectorySearcher GetSearcher(string filter, string[]

        propertiesToLoad)
        {

            DirectorySearcher searcher = new DirectorySearcher();

            searcher.SearchRoot = new DirectoryEntry(m_path, m_domainUser,

            m_password); ;

            searcher.SearchScope = SearchScope.Subtree;

            searcher.Filter = filter;

            if (propertiesToLoad != null)
            {

                searcher.PropertiesToLoad.AddRange(propertiesToLoad);

            }

            return searcher;

        }

        protected string ParseDomain(string distinguishedname)
        {

            if (distinguishedname == null || distinguishedname.Trim().Length ==

            0) return "";

            Regex reg = new Regex(@"(DC|dc)\s*=\s*(?<DC>[^,]+)");

            Match m = reg.Match(distinguishedname);

            return m.Groups["DC"].Value;

        }

        protected string ParseDepartment(string displayname)
        {

            if (displayname == null || displayname.Trim().Length == 0)
            {

                return "";

            }

            Regex reg = new Regex(@"\(\s*(?<Department>[^.]+.[^.]+).[^.]+\)");

            Match m = reg.Match(displayname);

            return m.Groups["Department"].Value;

        }

        protected string ParseFullName(string displayname)
        {

            if (displayname == null || displayname.Trim().Length == 0)
            {

                return "";

            }

            Regex reg = new Regex(@"([a-zA-Z]*)\.[a-zA-Z]*\.([a-zA-Z]*)");

            Match m = reg.Match(displayname);

            return m.Groups[1].Value + " " + m.Groups[2].Value;

        }

        protected string GetValue(DirectoryEntry entry, string strProp)
        {
            string strValue;

            if (entry.Properties[strProp] == null || entry.Properties[strProp].Count == 0)
            {
                strValue = "";
            }

            else
            {
                strValue = entry.Properties[strProp][0].ToString();
            }

            return strValue;

        }

        protected string GetValue(SearchResult result, string strProp)
        {
            string strValue;

            if (result.Properties[strProp] != null && result.Properties[strProp].Count != 0)
            {
                strValue = result.Properties[strProp][0].ToString();
            }
            else
            {
                strValue = "";
            }

            return strValue;
        }

        #endregion

    }
}