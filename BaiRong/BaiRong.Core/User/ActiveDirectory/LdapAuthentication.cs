using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace BaiRong.Core.ActiveDirectory
{
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domain, string username, string pwd, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string domainAndUsername = domain + @"\" + username;
                DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    errorMessage = "用户名不存在";
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            return true;
        }
    }
}