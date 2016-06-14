using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;


using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlUserEntities
	{
        private StlUserEntities()
		{
		}

        public const string EntityName = "User";              //用户实体

        public static string Group = "Group";               //用户组
        public static string LoginUrl = "LoginUrl";         //登录地址
        public static string LogoutUrl = "LogoutUrl";       //退出登录地址

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Group, "用户组");
                attributes.Add(LoginUrl, "登录地址");
                attributes.Add(LogoutUrl, "退出登录地址");

                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            return Parse(stlEntity, pageInfo, UserManager.Current);
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, UserInfo userInfo)
        {
            string parsedContent = string.Empty;

            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                string attributeName = entityName.Substring(6, entityName.Length - 7);

                if (StringUtils.EqualsIgnoreCase(attributeName, StlUserEntities.LoginUrl))
                {
                    string returnUrl = string.Empty;
                    if (pageInfo != null)
                    {
                        returnUrl = StlUtility.GetStlCurrentUrl(pageInfo, pageInfo.PageNodeID, pageInfo.PageContentID, null);
                    }
                    parsedContent = PageUtils.Combine(pageInfo.PublishmentSystemInfo.Additional.HomeUrl, string.Format("login.html?returnUrl={0}", returnUrl));
                }
                else if (StringUtils.EqualsIgnoreCase(attributeName, StlUserEntities.LogoutUrl))
                {
                    string returnUrl = string.Empty;
                    if (pageInfo != null)
                    {
                        returnUrl = StlUtility.GetStlCurrentUrl(pageInfo, pageInfo.PageNodeID, pageInfo.PageContentID, null);
                    }
                    parsedContent = PageUtils.Combine(pageInfo.PublishmentSystemInfo.Additional.HomeUrl, string.Format("logout.html?returnUrl={0}", returnUrl));
                }
                else
                {
                    if (userInfo == null)
                    {
                        userInfo = UserManager.Current;
                    }
                    if (userInfo != null)
                    {
                        if (StringUtils.EqualsIgnoreCase(UserAttribute.UserName, attributeName))
                        {
                            parsedContent = userInfo.UserName;
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.DisplayName, attributeName))
                        {
                            parsedContent = userInfo.DisplayName;
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.Email, attributeName))
                        {
                            parsedContent = userInfo.Email;
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.Mobile, attributeName))
                        {
                            parsedContent = userInfo.Mobile;
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.CreateDate, attributeName))
                        {
                            parsedContent = DateUtils.Format(userInfo.CreateDate, string.Empty);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.CreateIPAddress, attributeName))
                        {
                            parsedContent = userInfo.CreateIPAddress;
                        }
                        else if (StringUtils.EqualsIgnoreCase(StlUserEntities.Group, attributeName))
                        {
                            parsedContent = UserGroupManager.GetGroupName(pageInfo.PublishmentSystemInfo.GroupSN, userInfo.GroupID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.AvatarLarge, attributeName))
                        {
                            parsedContent = PageUtils.GetUserAvatarUrl(pageInfo.PublishmentSystemInfo.GroupSN, userInfo.UserName, EAvatarSize.Large);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.AvatarMiddle, attributeName))
                        {
                            parsedContent = PageUtils.GetUserAvatarUrl(pageInfo.PublishmentSystemInfo.GroupSN, userInfo.UserName, EAvatarSize.Middle);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.AvatarSmall, attributeName))
                        {
                            parsedContent = PageUtils.GetUserAvatarUrl(pageInfo.PublishmentSystemInfo.GroupSN, userInfo.UserName, EAvatarSize.Small);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.LastActivityDate, attributeName))
                        {
                            parsedContent = DateUtils.Format(userInfo.LastActivityDate, string.Empty);
                        }
                        else if (StringUtils.EqualsIgnoreCase(UserAttribute.Password, attributeName))
                        {
                            parsedContent = userInfo.Password;
                        }
                    }
                }
            }
            catch { }

            return parsedContent;
        }

        internal static string ReplaceEntityToArtTemplate(string html)
        {
            string parsedContent = html;

            try
            {
                List<string> entityList = StlParserUtility.GetStlEntityList(html);
                foreach (string stlEntity in entityList)
                {
                    if (!stlEntity.ToLower().StartsWith("{user."))
                    {
                        continue;
                    }
                    string attributeName = "<%=" + stlEntity.TrimStart('{').TrimEnd('}') + "%>";

                    parsedContent = parsedContent.Replace(stlEntity, attributeName);
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
