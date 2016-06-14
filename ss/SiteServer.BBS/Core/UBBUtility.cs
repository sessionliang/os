using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.BBS.Pages;


namespace SiteServer.BBS.Core
{
    public class UBBUtility
    {
        public static Regex[] r = new Regex[2];

        static UBBUtility()
        {
            r[0] = new Regex(@"\s*\[code\][\n\r]*([\s\S]+?)[\n\r]*\[\/code\]\s*", RegexOptions.IgnoreCase);
            r[1] = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);
        }

        private static void ParseAttachment(int publishmentSystemID, StringBuilder builder, List<AttachmentInfo> attachmentInfoList)
        {
            if (attachmentInfoList != null && attachmentInfoList.Count > 0)
            {
                foreach (AttachmentInfo attachmentInfo in attachmentInfoList)
                {
                    string ubb = UBBUtility.GetUBB_Attachment(attachmentInfo.ID);
                    if (StringUtils.Contains(builder.ToString(), ubb))
                    {
                        string attachHtml = AttachManager.GetAttachmentHtml(publishmentSystemID, attachmentInfo);
                        builder.Replace(ubb, attachHtml);
                    }
                }
            }
        }

        private static void ParseCode(StringBuilder builder)
        {
            if (StringUtils.Contains(builder.ToString(), "[code]"))
            {
                builder.Replace("<p>[code]</p>", "[code]");
                builder.Replace("<p>[/code]</p>", "[/code]");
                for (Match m = r[0].Match(builder.ToString()); m.Success; m = m.NextMatch())
                {
                    StringBuilder ubbCode = new StringBuilder();
                    string code = m.Groups[1].ToString();
                    if (!string.IsNullOrEmpty(code))
                    {
                        code = code.Replace("<p>", string.Empty);
                        code = code.Replace("</p>", "_ubbcode_break_");
                        code = code.Replace("<br>", "_ubbcode_break_");
                        code = code.Replace("<br/>", "_ubbcode_break_");
                        code = code.Replace("<br />", "_ubbcode_break_");
                        code = StringUtils.StripTags(code);
                        code = StringUtils.RemoveNewline(code);
                        string[] liArray = StringUtils.SplitStringIgnoreCase(code, "_ubbcode_break_");
                        ubbCode.Append(@"<div class=""ubbcode""><div><ol>");
                        foreach (string li in liArray)
                        {
                            if (!string.IsNullOrEmpty(li))
                            {
                                ubbCode.AppendFormat("<li>{0}</li>", li);
                            }
                        }
                        ubbCode.AppendFormat(@"</ol></div><em onclick=""copyCode('{0}');return false;"">复制代码</em></div>", StringUtils.ToJsString(code.Replace("_ubbcode_break_", "\r\n")));
                    }
                    builder.Replace(m.Groups[0].ToString(), ubbCode.ToString());
                }
            }
        }

        private static void ParseHide(StringBuilder builder, PostInfo postInfo)
        {
            if (StringUtils.Contains(builder.ToString(), "[hide]"))
            {
                for (Match m = r[1].Match(builder.ToString()); m.Success; m = m.NextMatch())
                {
                    string parsedContent = ForumUtils.GetInstance(postInfo.PublishmentSystemID).GetHideContent(postInfo.ForumID, postInfo.ThreadID, postInfo.ID, postInfo.UserName, RuntimeUtils.EncryptStringByTranslate(m.Groups[1].ToString()));
                    builder.Replace(m.Groups[0].ToString(), parsedContent);
                }
            }
        }

        public static string Parse(int publishmentSystemID, PostInfo postInfo, List<AttachmentInfo> attachmentInfoList)
        {
            if (!string.IsNullOrEmpty(postInfo.Content))
            {
                StringBuilder builder = new StringBuilder(postInfo.Content);
                UBBUtility.ParseAttachment(publishmentSystemID, builder, attachmentInfoList);
                UBBUtility.ParseCode(builder);
                UBBUtility.ParseHide(builder, postInfo);
                return builder.ToString();
            }
            return string.Empty;
        }

        public static string ClearUBB(string content)
        {
            return Regex.Replace(content, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string GetUBB_Attachment(int attachmentID)
        {
            return string.Format("[attachment id={0}]", attachmentID);
        }
    }
}
