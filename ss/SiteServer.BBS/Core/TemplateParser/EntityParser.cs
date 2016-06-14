using System.Collections;
using System.Text;

using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Core.TemplateParser.Entity;

namespace SiteServer.BBS.Core.TemplateParser
{
    public class EntityParser
	{
        private EntityParser()
		{
		}

        /// <summary>
        /// 将原始内容中的STL实体替换为实际内容
        /// </summary>
        public static void ReplaceEntities(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            ArrayList entityArrayList = ParserUtility.GetEntityArrayList(parsedBuilder.ToString());

            foreach (string entity in entityArrayList)
            {
                int startIndex = parsedBuilder.ToString().IndexOf(entity);
                if (startIndex != -1)
                {
                    string resultContent = EntityParser.ParseEntity(entity, pageInfo, contextInfo);
                    parsedBuilder.Replace(entity, resultContent, startIndex, entity.Length);
                }
            }
        }

        internal static string ParseEntity(string entity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            EntityType entityType = EntityTypeUtils.GetEntityType(entity);

            if (entityType == EntityType.BBS)
            {
                parsedContent = BBSEntities.Parse(entity, pageInfo, contextInfo);
            }
            else if (entityType == EntityType.Forum)
            {
                parsedContent = ForumEntities.Parse(entity, pageInfo, contextInfo);
            }
            else if (entityType == EntityType.Thread)
            {
                parsedContent = ThreadEntities.Parse(entity, pageInfo, contextInfo);
            }
            else if (entityType == EntityType.Post)
            {
                parsedContent = PostEntities.Parse(entity, pageInfo, contextInfo);
            }
            else if (entityType == EntityType.Sql)
            {
                parsedContent = SqlEntities.Parse(entity, pageInfo, contextInfo);
            }

            return parsedContent;
        }

        internal static string ReplaceEntitiesForAttributeValue(string attrValue, PageInfo pageInfo, ContextInfo contextInfo)
        {
            if (ParserUtility.IsEntityInclude(attrValue))
            {
                StringBuilder contentBuilder = new StringBuilder(attrValue);
                EntityParser.ReplaceEntities(contentBuilder, pageInfo, contextInfo);
                return contentBuilder.ToString();
            }
            return attrValue;
        }
	}
}
