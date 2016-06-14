using System.Collections;
using System.Text;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;


using System.Collections.Generic;
using BaiRong.Model;
using System;

namespace SiteServer.STL.Parser
{
    /// <summary>
    /// Stl实体解析器
    /// </summary>
	public class StlEntityParser
	{
		private StlEntityParser()
		{
		}

        /// <summary>
        /// 将原始内容中的STL实体替换为实际内容
        /// </summary>
        public static void ReplaceStlEntities(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            List<string> stlEntityList = StlParserUtility.GetStlEntityList(parsedBuilder.ToString());

            foreach (string stlEntity in stlEntityList)
            {
                int startIndex = parsedBuilder.ToString().IndexOf(stlEntity);
                if (startIndex != -1)
                {
                    string resultContent = StlEntityParser.ParseStlEntity(stlEntity, pageInfo, contextInfo);
                    parsedBuilder.Replace(stlEntity, resultContent, startIndex, stlEntity.Length);
                }
            }
        }

        public static string ReplaceStlUserEntities(string content, UserInfo userInfo)
        {
            StringBuilder parsedBuilder = new StringBuilder(content);
            List<string> stlEntityList = StlParserUtility.GetStlUserEntityList(parsedBuilder.ToString());

            foreach (string stlEntity in stlEntityList)
            {
                int startIndex = parsedBuilder.ToString().IndexOf(stlEntity);
                if (startIndex != -1)
                {
                    EStlEntityType entityType = EStlEntityTypeUtils.GetEntityType(stlEntity);
                    if (entityType == EStlEntityType.User)
                    {
                        string resultContent = StlUserEntities.Parse(stlEntity, null, userInfo);
                        parsedBuilder.Replace(stlEntity, resultContent, startIndex, stlEntity.Length);
                    }
                }
            }
            return parsedBuilder.ToString();
        }

        readonly static Dictionary<EStlEntityType, Func<string, PageInfo, ContextInfo, string>> EntityDic = InitEntityDic();

        private static Dictionary<EStlEntityType, Func<string, PageInfo, ContextInfo, string>> InitEntityDic()
        {
            var dic = new Dictionary<EStlEntityType, Func<string, PageInfo, ContextInfo, string>>();
            dic.Add(EStlEntityType.Stl, StlEntities.Parse);
            dic.Add(EStlEntityType.StlElement, StlElementEntities.Parse);
            dic.Add(EStlEntityType.Content, StlContentEntities.Parse);
            dic.Add(EStlEntityType.Channel, StlChannelEntities.Parse);
            dic.Add(EStlEntityType.Photo, StlPhotoEntities.Parse);
            dic.Add(EStlEntityType.Comment, StlCommentEntities.Parse);
            dic.Add(EStlEntityType.Request, StlRequestEntities.Parse);
            dic.Add(EStlEntityType.Navigation, StlNavigationEntities.Parse);
            dic.Add(EStlEntityType.Sql, StlSqlEntities.Parse);
            dic.Add(EStlEntityType.User, StlUserEntities.Parse);
            dic.Add(EStlEntityType.B2C, StlB2CEntities.Parse);
            return dic;
        }

        internal static string ParseStlEntity(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            EStlEntityType entityType = EStlEntityTypeUtils.GetEntityType(stlEntity);

            #region Old version, use if else
            //if (entityType == EStlEntityType.Stl)
            //{
            //    parsedContent = StlEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.StlElement)
            //{
            //    parsedContent = StlElementEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Content)
            //{
            //    parsedContent = StlContentEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Channel)
            //{
            //    parsedContent = StlChannelEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Photo)
            //{
            //    parsedContent = StlPhotoEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Comment)
            //{
            //    parsedContent = StlCommentEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Request)
            //{
            //    parsedContent = StlRequestEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Navigation)
            //{
            //    parsedContent = StlNavigationEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.Sql)
            //{
            //    parsedContent = StlSqlEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.User)
            //{
            //    parsedContent = StlUserEntities.Parse(stlEntity, pageInfo, contextInfo);
            //}
            //else if (entityType == EStlEntityType.B2C)
            //{
            //    parsedContent = StlB2CEntities.Parse(stlEntity, pageInfo, contextInfo);
            //} 
            #endregion

            #region New version, use dictionary, exchange time with space, but initional is slower
            Func<string, PageInfo, ContextInfo, string> func;
            if (EntityDic.TryGetValue(entityType, out func))
            {
                parsedContent = func(stlEntity, pageInfo, contextInfo);
            }
            else
            {
                //parsedContent = stlEntity;
            }
            #endregion

            return parsedContent;
        }

        internal static string ReplaceStlEntitiesForAttributeValue(string attrValue, PageInfo pageInfo, ContextInfo contextInfo)
        {
            if (StlParserUtility.IsStlEntityInclude(attrValue))
            {
                StringBuilder contentBuilder = new StringBuilder(attrValue);
                StlEntityParser.ReplaceStlEntities(contentBuilder, pageInfo, contextInfo);
                return contentBuilder.ToString();
            }
            return attrValue;
        }
	}
}
