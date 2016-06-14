using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlCommentEntities
	{
        private StlCommentEntities()
		{
		}

        public const string EntityName = "Comment";        //评论实体

        private static string Reference = "Reference";     //引用
        private static string DiggGood = "DiggGood";       //支持
        private static string DiggBad = "DiggBad";         //反对

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(CommentAttribute.CommentID, "评论ID");
                attributes.Add(CommentAttribute.AddDate, "评论时间");
                attributes.Add(CommentAttribute.UserName, "评论人");
                attributes.Add(CommentAttribute.IPAddress, "IP地址");
                attributes.Add(CommentAttribute.Good, "支持数目");
                attributes.Add(StlCommentEntities.Reference, "引用");
                attributes.Add(StlCommentEntities.DiggGood, "支持");
                attributes.Add(StlCommentEntities.DiggBad, "反对");
                attributes.Add(CommentAttribute.Content, "评论正文");
                attributes.Add(StlParserUtility.ItemIndex, "评论排序");                

                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.CommentItem == null) return string.Empty;

            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);

                string type = entityName.Substring(9, entityName.Length - 10).ToLower();

                int commentID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.CommentID);
                int nodeID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.ContentID);
                int good = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.Good);
                string userName = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IsChecked));
                bool isRecommend = TranslateUtils.ToBool(TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IsRecommend));
                string ipAddress = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(contextInfo.ItemContainer.CommentItem, CommentAttribute.AddDate);
                string content = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.Content);

                if (CommentAttribute.CommentID.ToLower().Equals(type))
                {
                    parsedContent = commentID.ToString();
                }
                else if (CommentAttribute.AddDate.ToLower().Equals(type))
                {
                    parsedContent = DateUtils.Format(addDate, string.Empty);
                }
                else if (CommentAttribute.UserName.ToLower().Equals(type))
                {
                    parsedContent = string.IsNullOrEmpty(userName) ? "匿名" : userName;
                }
                else if (CommentAttribute.IPAddress.ToLower().Equals(type))
                {
                    parsedContent = ipAddress;
                }
                else if (CommentAttribute.Good.ToLower().Equals(type))
                {
                    parsedContent = good.ToString();
                }
                else if (StlCommentEntities.Reference.ToLower().Equals(type))//引用
                {
                    parsedContent = string.Format("commentReference('{0}', '{1}', '{2}');return false;", userName, addDate, StringUtils.ToJsString(content));
                }
                else if (CommentAttribute.Content.ToLower().Equals(type))
                {
                    parsedContent = TranslateUtils.ParseCommentContent(content);
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.CommentItem.ItemIndex, type, contextInfo).ToString();
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
