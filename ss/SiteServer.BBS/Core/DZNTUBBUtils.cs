﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.BBS.Pages;

namespace SiteServer.BBS.Core
{
    public class DZNTUBBUtils
    {
        private static string IMG_SIGN_SIGNATURE = "<img src=\"$1\" border=\"0\" />";
        //private static string IMG_SIGN = "<img src=\"$1\" border=\"0\" onload=\"thumbImg(this)\" />";
        private static RegexOptions options = RegexOptions.IgnoreCase;

        public static Regex[] r = new Regex[20];

        static DZNTUBBUtils()
        {
            r[0] = new Regex(@"\s*\[code\]([\s\S]+?)\[\/code\]\s*", options);
            r[1] = new Regex(@"(\[upload=([^\]]{1,4})(,.*?\.[^\]]{1,4})?\])(.*?)(\[\/upload\])", options);
            r[2] = new Regex(@"viewfile.asp\?id=(\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r[3] = new Regex(@"(\[uploadimage\])(.*?)(\[\/uploadimage\])", options);
            r[4] = new Regex(@"viewfile.asp\?id=(\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r[5] = new Regex(@"(\[uploadfile\])(.*?)(\[\/uploadfile\])", options);
            r[6] = new Regex(@"viewfile.asp\?id=(\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r[7] = new Regex(@"(\[upload\])(.*?)(\[\/upload\])", options);
            r[8] = new Regex(@"viewfile.asp\?id=(\d{1,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r[9] = new Regex(@"(\r\n((&nbsp;)|　| )+)(?<正文>\S+)", options);
            r[10] = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);
            r[11] = new Regex(@"\[table(?:=(\d{1,4}%?)(?:,([\(\)%,#\w ]+))?)?\]\s*([\s\S]+?)\s*\[\/table\]", options);
            r[12] = new Regex(@"\[media=(\w{1,4}),(\d{1,4}),(\d{1,4})(,(\d))?\]\s*([^\[\<\r\n]+?)\s*\[\/media\]", options);
            r[13] = new Regex(@"\[attach\](\d+)(\[/attach\])*", options);
            r[14] = new Regex(@"\[attachimg\](\d+)(\[/attachimg\])*", options);
            r[15] = new Regex(@"\s*\[free\][\n\r]*([\s\S]+?)[\n\r]*\[\/free\]\s*", RegexOptions.IgnoreCase);
            r[16] = new Regex(@"\s*\[hide=(\d+?)\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);
            r[17] = new Regex(@"\[audio(=(\d))?\]\s*([^\[\<\r\n]+?)\s*\[\/audio\]", options);
            r[18] = new Regex(@"\[p=(\d{1,4}), (\d{1,4}), (left|center|right)\]\s*([^\[\<\r\n]+?)\s*\[\/p\]", options);
            r[19] = new Regex(@"\[flash(=(\d{1,4}),(\d{1,4}))?\]\s*([^\[\<\r\n]+?)\s*\[\/flash\]", options);
        }

        /// <summary>
        /// UBB代码处理函数
        /// </summary>
        ///	<param name="_postpramsinfo">UBB转换参数表</param>
        /// <returns>输出字符串</returns>
        public static string UBBToHTML(string content, int pid)
        {
            Match m;

            string sDetail = content;

            StringBuilder sb = new StringBuilder();
            int pcodecount = -1;
            string sbStr = "";
            string prefix = pid.ToString();
            //if (_postpramsinfo.Bbcodeoff == 0)
            //{
                for (m = r[0].Match(sDetail); m.Success; m = m.NextMatch())
                {
                    sbStr = Parsecode(m.Groups[1].ToString(), prefix, ref pcodecount, ref sb);
                    sDetail = sDetail.Replace(m.Groups[0].ToString(), sbStr);
                }
            //}

            //if (_postpramsinfo.Bbcodeoff == 0)
            //{
            //    sDetail = HideDetail(sDetail, _postpramsinfo.Hide, _postpramsinfo.Usercredits);
            //}


            //清除无效的smilie标签
            sDetail = Regex.Replace(sDetail, @"\[smilie\](.+?)\[\/smilie\]", "$1", options);

            sDetail = sDetail.Replace(":O", @"<img src=""@editor/plugins/emoticons/14.gif"" />");
            sDetail = sDetail.Replace(":~", @"<img src=""@editor/plugins/emoticons/1.gif"" />");
            sDetail = sDetail.Replace(":-|", @"<img src=""@editor/plugins/emoticons/10.gif"" />");
            sDetail = sDetail.Replace(":@", @"<img src=""@editor/plugins/emoticons/11.gif"" />");
            sDetail = sDetail.Replace(":Z", @"<img src=""@editor/plugins/emoticons/12.gif"" />");
            sDetail = sDetail.Replace(":D", @"<img src=""@editor/plugins/emoticons/13.gif"" />");
            sDetail = sDetail.Replace(":)", @"<img src=""@editor/plugins/emoticons/0.gif"" />");
            sDetail = sDetail.Replace(":(", @"<img src=""@editor/plugins/emoticons/15.gif"" />");
            sDetail = sDetail.Replace(":+", @"<img src=""@editor/plugins/emoticons/16.gif"" />");
            sDetail = sDetail.Replace(":#", @"<img src=""@editor/plugins/emoticons/31.gif"" />");
            sDetail = sDetail.Replace(":Q", @"<img src=""@editor/plugins/emoticons/18.gif"" />");
            sDetail = sDetail.Replace(":T", @"<img src=""@editor/plugins/emoticons/19.gif"" />");
            sDetail = sDetail.Replace(":*", @"<img src=""@editor/plugins/emoticons/2.gif"" />");
            sDetail = sDetail.Replace(":P", @"<img src=""@editor/plugins/emoticons/20.gif"" />");
            sDetail = sDetail.Replace(":-D", @"<img src=""@editor/plugins/emoticons/21.gif"" />");
            sDetail = sDetail.Replace(":d", @"<img src=""@editor/plugins/emoticons/43.gif"" />");
            sDetail = sDetail.Replace(":o", @"<img src=""@editor/plugins/emoticons/23.gif"" />");
            sDetail = sDetail.Replace(":g", @"<img src=""@editor/plugins/emoticons/24.gif"" />");
            sDetail = sDetail.Replace(":|-)", @"<img src=""@editor/plugins/emoticons/25.gif"" />");
            sDetail = sDetail.Replace(":!", @"<img src=""@editor/plugins/emoticons/26.gif"" />");
            sDetail = sDetail.Replace(":L", @"<img src=""@editor/plugins/emoticons/27.gif"" />");
            sDetail = sDetail.Replace(":giggle", @"<img src=""@editor/plugins/emoticons/28.gif"" />");
            sDetail = sDetail.Replace(":smoke", @"<img src=""@editor/plugins/emoticons/29.gif"" />");
            sDetail = sDetail.Replace(":|", @"<img src=""@editor/plugins/emoticons/3.gif"" />");
            sDetail = sDetail.Replace(":f", @"<img src=""@editor/plugins/emoticons/30.gif"" />");
            sDetail = sDetail.Replace(":-S", @"<img src=""@editor/plugins/emoticons/18.gif"" />");
            sDetail = sDetail.Replace(":?", @"<img src=""@editor/plugins/emoticons/32.gif"" />");
            sDetail = sDetail.Replace(":xx", @"<img src=""@editor/plugins/emoticons/38.gif"" />");
            sDetail = sDetail.Replace(":x", @"<img src=""@editor/plugins/emoticons/33.gif"" />");
            sDetail = sDetail.Replace(":yun", @"<img src=""@editor/plugins/emoticons/34.gif"" />");
            sDetail = sDetail.Replace(":8", @"<img src=""@editor/plugins/emoticons/35.gif"" />");
            sDetail = sDetail.Replace(":bye", @"<img src=""@editor/plugins/emoticons/39.gif"" />");
            sDetail = sDetail.Replace(":<", @"<img src=""@editor/plugins/emoticons/9.gif"" />");
            sDetail = sDetail.Replace(":kn", @"<img src=""@editor/plugins/emoticons/71.gif"" />");
            sDetail = sDetail.Replace(":bome", @"<img src=""@editor/plugins/emoticons/70.gif"" />");
            sDetail = sDetail.Replace(":strong:", @"<img src=""@editor/plugins/emoticons/79.gif"" />");
            sDetail = sDetail.Replace(":weak:", @"<img src=""@editor/plugins/emoticons/80.gif"" />");
            sDetail = sDetail.Replace(":share:", @"<img src=""@editor/plugins/emoticons/81.gif"" />");
            sDetail = sDetail.Replace(":v:", @"<img src=""@editor/plugins/emoticons/82.gif"" />");
            sDetail = sDetail.Replace(":kiss", @"<img src=""@editor/plugins/emoticons/65.gif"" />");
            sDetail = sDetail.Replace(":t(", @"<img src=""@editor/plugins/emoticons/9.gif"" />");

            //#region 建立smile表情标签

            //if (_postpramsinfo.Smileyoff == 0 && _postpramsinfo.Smiliesinfo != null)
            //{
            //    sDetail = ParseSmilies(sDetail, _postpramsinfo.Smiliesinfo, _postpramsinfo.Smiliesmax);
            //}

            //#endregion

            // [smilie]处标记
            sDetail = Regex.Replace(sDetail, @"\[smilie\](.+?)\[\/smilie\]", "<img src=\"$1\" />", options);

            //if (_postpramsinfo.Bbcodeoff == 0)
            //{
                if (sDetail.ToLower().Contains("[free]") || sDetail.ToLower().Contains("[/free]"))
                {
                    for (m = r[15].Match(sDetail); m.Success; m = m.NextMatch())
                    {
                        sDetail = sDetail.Replace(m.Groups[0].ToString(), "<br /><div class=\"msgheader\">免费内容:</div><div class=\"msgborder\">" + m.Groups[1].ToString() + "</div><br />");

                    }
                }

                // Bold, Italic, Underline
                sDetail = parseBold(sDetail);
                //sDetail = Regex.Replace(sDetail, @"\[b(?:\s*)\]", "<b>", options);
                //sDetail = Regex.Replace(sDetail, @"\[i(?:\s*)\]", "<i>", options);
                //sDetail = Regex.Replace(sDetail, @"\[u(?:\s*)\]", "<u>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/b(?:\s*)\]", "</b>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/i(?:\s*)\]", "</i>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/u(?:\s*)\]", "</u>", options);

                // Sub/Sup
                sDetail = Regex.Replace(sDetail, @"\[sup(?:\s*)\]", "<sup>", options);
                sDetail = Regex.Replace(sDetail, @"\[sub(?:\s*)\]", "<sub>", options);
                sDetail = Regex.Replace(sDetail, @"\[/sup(?:\s*)\]", "</sup>", options);
                sDetail = Regex.Replace(sDetail, @"\[/sub(?:\s*)\]", "</sub>", options);

                // P
                sDetail = Regex.Replace(sDetail, @"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", "<p>$3</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                // Anchors
                sDetail = ParseUrl(sDetail);

                // Email
                sDetail = Regex.Replace(sDetail, @"\[email(?:\s*)\](.*?)\[\/email\]", "<a href=\"mailto:$1\" target=\"_blank\">$1</a>", options);
                sDetail = Regex.Replace(sDetail, @"\[email=(.[^\[]*)(?:\s*)\](.*?)\[\/email(?:\s*)\]", "<a href=\"mailto:$1\" target=\"_blank\">$2</a>", options);

                #region

                // Font
                sDetail = parseFont(sDetail);
                //sDetail = Regex.Replace(sDetail, @"\[color=([^\[\<]+?)\]", "<font color=\"$1\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[size=(\d+?)\]", "<font size=\"$1\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[size=(\d+(\.\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\]", "<font style=\"font-size: $1\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[font=([^\[\<]+?)\]", "<font face=\"$1\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[align=([^\[\<]+?)\]", "<p align=\"$1\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[float=(left|right)\]", "<br style=\"clear: both\"><span style=\"float: $1;\">", options);
                //sDetail = Regex.Replace(sDetail, @"\[/color(?:\s*)\]", "</font>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/size(?:\s*)\]", "</font>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/font(?:\s*)\]", "</font>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/align(?:\s*)\]", "</p>", options);
                //sDetail = Regex.Replace(sDetail, @"\[/float(?:\s*)\]", "</span>", options);

                // BlockQuote
                sDetail = Regex.Replace(sDetail, @"\[indent(?:\s*)\]", "<blockquote>", options);
                sDetail = Regex.Replace(sDetail, @"\[/indent(?:\s*)\]", "</blockquote>", options);
                sDetail = Regex.Replace(sDetail, @"\[simpletag(?:\s*)\]", "<blockquote>", options);
                sDetail = Regex.Replace(sDetail, @"\[/simpletag(?:\s*)\]", "</blockquote>", options);

                // List
                sDetail = Regex.Replace(sDetail, @"\[list\]", "<ul>", options);
                sDetail = Regex.Replace(sDetail, @"\[list=1\]", "<ul type=1 class=\"litype_1\">", options);
                sDetail = Regex.Replace(sDetail, @"\[list=a\]", "<ul type=1 class=\"litype_2\">", options);
                sDetail = Regex.Replace(sDetail, @"\[list=A\]", "<ul type=1 class=\"litype_3\">", options);
                sDetail = Regex.Replace(sDetail, @"\[\*\]", "<li>", options);
                sDetail = Regex.Replace(sDetail, @"\[/list\]", "</ul>", options);
                #endregion

                #region 循环转换table

                sDetail = ParseTable(sDetail);

                #endregion

                // shadow
                sDetail = Regex.Replace(sDetail, @"(\[SHADOW=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/SHADOW\])", "<table width='$2'  style='filter:SHADOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

                // glow
                sDetail = Regex.Replace(sDetail, @"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/glow\])", "<table width='$2'  style='filter:GLOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

                // center
                sDetail = Regex.Replace(sDetail, @"\[center\]([\s]||[\s\S]+?)\[\/center\]", "<center>$1</center>", options);

                // Media

                MatchCollection mc = r[12].Matches(sDetail);
                foreach (Match match in mc)
                {
                    sDetail = sDetail.Replace(match.Groups[0].Value, ParseMedia(match.Groups[1].Value, TranslateUtils.ToInt(match.Groups[2].Value, 64), TranslateUtils.ToInt(match.Groups[3].Value, 48), match.Groups[4].Value == "1" ? true : false, match.Groups[6].Value));
                }


                //Audio
                mc = r[17].Matches(sDetail);
                foreach (Match match in mc)
                {
                    sDetail = sDetail.Replace(match.Groups[0].Value, ParseAudio(match.Groups[2].Value, match.Groups[3].Value));
                }

                //p
                mc = r[18].Matches(sDetail);
                foreach (Match match in mc)
                {
                    sDetail = sDetail.Replace(match.Groups[0].Value, ParseP(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
                }
                sDetail = sDetail.Replace("[p=30, 2, left][/p]", "<p style=\"line-height: 30px; text-indent: 2em; text-align: left;\"></p>");
                //flash
                mc = r[19].Matches(sDetail);
                foreach (Match match in mc)
                {
                    sDetail = sDetail.Replace(match.Groups[0].Value, ParseFlash(match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
                }

                #region 处理[quote][/quote]标记

                int intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]");
                int quotecount = 0;
                while (intQuoteIndexOf >= 0 && sDetail.ToLower().IndexOf("[/quote]") >= 0 && quotecount < 3)
                {
                    quotecount++;
                    sDetail = Regex.Replace(sDetail, @"\[quote\]([\s\S]+?)\[/quote\]", "<table style=\"width: auto;\"><tr><td style=\"border:none;\"><div class=\"quote\"><blockquote>$1</blockquote></div></td></tr></table>", options);

                    intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]", intQuoteIndexOf + 7);
                }

                #endregion

                //处理[area]标签
                sDetail = Regex.Replace(sDetail, @"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]", "<div class=\"msgheader\">$1</div><div class=\"msgborder\">$2</div>", options);
                sDetail = Regex.Replace(sDetail, @"\[area\]([\s\S]+?)\[/area\]", "<br /><br /><div class=\"msgheader\"></div><div class=\"msgborder\">$1</div>", options);

                #region 动网兼容模式ubb

                //if (_postpramsinfo.Bbcodemode == 1)
                //{
                //    ///[upload=jpg].jpg[/upload]
                //    string attachCode = "<p><img alt=\"\" src=\"{0}\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"{1}\" target=\"_blank\">{2}</a> </p>";
                //    string replacement = "";
                //    string attachIcon = "images/attachicons/attachment.gif";
                //    for (m = r[1].Match(sDetail); m.Success; m = m.NextMatch())
                //    {

                //        Match m1 = r[2].Match(m.Groups[4].ToString().ToLower());
                //        if (m1.Success)
                //        {
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), "[attach]" + m1.Groups[1] + "[/attach]");
                //            continue;
                //        }

                //        string attachPath = m.Groups[4].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                //        if (attachPath.IndexOf("attachment.aspx?attachmentid") == -1)
                //        {
                //            attachPath = BaseConfigs.GetForumPath + "upload/" + attachPath;
                //        }
                //        else
                //        {
                //            attachPath = BaseConfigs.GetForumPath + attachPath;
                //        }

                //        if ("rar,zip".IndexOf(m.Groups[2].ToString().ToLower()) != -1)
                //        {
                //            attachIcon = "images/attachicons/rar.gif";
                //        }

                //        if ("jpg,jpeg,gif,bmp,png".IndexOf(m.Groups[2].ToString().ToLower()) != -1)
                //        {
                //            if (_postpramsinfo.Showimages == 1)
                //            {
                //                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<img src=\"" + attachPath + "\" border=\"0\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt=\'点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小\';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor=\'hand\'; this.alt=\'点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小\';}\" onclick=\"if(!this.resized) {return true;} else {window.open(this.src);}\" onmousewheel=\"return imgzoom(this);\" />");
                //            }
                //            else
                //            {
                //                replacement = attachPath;
                //                if (replacement.LastIndexOf("/") > 0)
                //                {
                //                    replacement = StringUtils.CutString(replacement, replacement.LastIndexOf("/"));
                //                }
                //                replacement = string.Format(attachCode, attachIcon, attachPath);
                //                sDetail = sDetail.Replace(m.Groups[0].ToString(), replacement);
                //            }

                //        }
                //        else
                //        {
                //            replacement = attachPath;
                //            if (replacement.LastIndexOf("/") > 0)
                //            {
                //                replacement = StringUtils.CutString(replacement, replacement.LastIndexOf("/"));
                //            }
                //            replacement = string.Format(attachCode, attachIcon, attachPath, replacement);
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), replacement);
                //        }
                //    }

                //    sDetail = Regex.Replace(sDetail, @"\[uploadimage\](\d{1,})\[/uploadimage\]", "[attach]$1[/attach]", options);

                //    replacement = "";
                //    for (m = r[3].Match(sDetail); m.Success; m = m.NextMatch())
                //    {
                //        Match m1 = r[4].Match(m.Groups[2].ToString().ToLower());
                //        if (m1.Success)
                //        {
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), "[attach]" + m1.Groups[1] + "[/attach]");
                //            continue;
                //        }

                //        string attachPath = m.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                //        if (attachPath.IndexOf("attachment.aspx?attachmentid") == -1)
                //        {
                //            attachPath = BaseConfigs.GetForumPath + "upload/" + attachPath;
                //        }
                //        else
                //        {
                //            attachPath = BaseConfigs.GetForumPath + attachPath;
                //        }

                //        if (_postpramsinfo.Showimages == 1)
                //        {
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), "<img src=\"" + attachPath + "\" border=\"0\" onload=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.alt=\'点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小\';}\" onmouseover=\"if(this.width>screen.width*0.7) {this.resized=true; this.width=screen.width*0.7; this.style.cursor=\'hand\'; this.alt=\'点击在新窗口浏览图片\\nCTRL+Mouse 滚轮可放大/缩小\';}\" onclick=\"if(!this.resized) {return true;} else {window.open(this.src);}\" onmousewheel=\"return imgzoom(this);\" />");
                //        }
                //        else
                //        {
                //            replacement = attachPath;
                //            if (replacement.LastIndexOf("/") > 0)
                //            {
                //                replacement = StringUtils.CutString(replacement, replacement.LastIndexOf("/"));
                //            }
                //            replacement = string.Format(attachCode, attachIcon, attachPath, replacement);
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), replacement);
                //        }
                //    }

                //    sDetail = Regex.Replace(sDetail, @"\[uploadfile\](\d{1,})\[/uploadfile\]", "[attach]$1[/attach]", options);

                //    replacement = "";
                //    for (m = r[5].Match(sDetail); m.Success; m = m.NextMatch())
                //    {
                //        Match m1 = r[6].Match(m.Groups[2].ToString().ToLower());
                //        if (m1.Success)
                //        {
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), "[attach]" + m1.Groups[1] + "[/attach]");
                //            continue;
                //        }

                //        replacement = m.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                //        if (replacement.IndexOf("attachment.aspx?attachmentid") == -1)
                //        {
                //            replacement = BaseConfigs.GetForumPath + "upload/" + replacement;
                //        }
                //        else
                //        {
                //            replacement = BaseConfigs.GetForumPath + replacement;
                //        }

                //        if (replacement.LastIndexOf("/") > 0)
                //        {
                //            replacement = StringUtils.CutString(replacement, replacement.LastIndexOf("/"));
                //        }
                //        replacement = string.Format(attachCode, attachIcon, BaseConfigs.GetForumPath + m.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid"), replacement);
                //        sDetail = sDetail.Replace(m.Groups[0].ToString(), replacement);
                //    }

                //    sDetail = Regex.Replace(sDetail, @"\[upload\](\d{1,})\[/upload\]", "[attach]$1[/attach]", options);

                //    //[upload].*[/upload]
                //    replacement = "";
                //    for (m = r[7].Match(sDetail); m.Success; m = m.NextMatch())
                //    {
                //        Match m1 = r[8].Match(m.Groups[2].ToString().ToLower());
                //        if (m1.Success)
                //        {
                //            sDetail = sDetail.Replace(m.Groups[0].ToString(), "[attach]" + m1.Groups[1] + "[/attach]");
                //            continue;
                //        }

                //        replacement = BaseConfigs.GetForumPath + m.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid");
                //        if (replacement.IndexOf("attachment.aspx?attachmentid") == -1)
                //        {
                //            replacement = BaseConfigs.GetForumPath + "upload/" + replacement;
                //        }
                //        else
                //        {
                //            replacement = BaseConfigs.GetForumPath + replacement;
                //        }

                //        if (replacement.LastIndexOf("/") > 0)
                //        {
                //            replacement = StringUtils.CutString(replacement, replacement.LastIndexOf("/"));
                //        }
                //        replacement = string.Format(attachCode, attachIcon, BaseConfigs.GetForumPath + m.Groups[2].ToString().ToLower().Replace("viewfile.asp?id", "attachment.aspx?attachmentid"), replacement);
                //        sDetail = sDetail.Replace(m.Groups[0].ToString(), replacement);
                //    }
                //}

                #endregion
            //}

            #region 将网址字符串转换为链接

            //if (_postpramsinfo.Parseurloff == 0)
            //{
                sDetail = sDetail.Replace("&amp;", "&");

                // p2p link
                sDetail = Regex.Replace(sDetail, @"^((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
                sDetail = Regex.Replace(sDetail, @"((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)$", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
                sDetail = Regex.Replace(sDetail, @"[^>=\]""]((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
            //}

            #endregion


            #region 处理[img][/img]标记

            //if (_postpramsinfo.Showimages == 1)
                sDetail = ParseImg(sDetail);

            #endregion

            pcodecount = 0;
            foreach (string str in StringUtils.SplitStringIgnoreCase(sb.ToString(), "<>"))
            {
                sDetail = sDetail.Replace("[\tDISCUZ_CODE_" + prefix + "_" + pcodecount.ToString() + "\t]", str);
                pcodecount++;
            }

            // [r/]
            sDetail = Regex.Replace(sDetail, @"\[r/\]", "\r", options);

            // [n/]
            sDetail = Regex.Replace(sDetail, @"\[n/\]", "\n", options);

            #region 处理换行

            //处理换行,在每个新行的前面添加两个全角空格
            //for (m = r[9].Match(sDetail); m.Success; m = m.NextMatch())
            //{
            //    sDetail = sDetail.Replace(m.Groups[0].ToString(), "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" + m.Groups["正文"].ToString());
            //}

            //if (_postpramsinfo.Allowhtml == 0)
            //{
                sDetail = sDetail.Replace("\r\n", "<br/>");
                sDetail = sDetail.Replace("\r", "");
                sDetail = sDetail.Replace("\n\n", "<br/><br/>");
                sDetail = sDetail.Replace("\n", "<br/>");
                sDetail = sDetail.Replace("{rn}", "\r\n");
                sDetail = sDetail.Replace("{nn}", "\n\n");
                sDetail = sDetail.Replace("{r}", "\r");
                sDetail = sDetail.Replace("{n}", "\n");
            //}
            #endregion

            #region 处理空格

            sDetail = sDetail.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            sDetail = sDetail.Replace("  ", "&nbsp;&nbsp;");

            #endregion


            #region 处理[hr]标记
            sDetail = Regex.Replace(sDetail, @"\[hr\]", "<hr/>", options);
            #endregion

            return sDetail;
        }
        /// <summary>
        /// 处理img标记
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        private static string ParseImg(string sDetail)
        {
            //if (Signature == 1)
            //{
                sDetail = Regex.Replace(sDetail, @"\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", IMG_SIGN_SIGNATURE, options);
                //sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", IMG_SIGN_SIGNATURE, options);
            //}
            //else
            //{
            //    sDetail = Regex.Replace(sDetail, @"\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", IMG_SIGN, options);

            //    //sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", IMG_SIGN, options);
            //}

            sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" onload=\"thumbImg(this)\" />", options).Replace("width=\"0\"", "").Replace("height=\"0\"", "");

            //sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" onload=\"thumbImg(this)\" />", options);
            sDetail = Regex.Replace(sDetail, @"\[image\]([\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);

            //sDetail = Regex.Replace(sDetail, @"\[image\](http://[\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);
            return sDetail;
        }

        /// <summary>
        /// 处理B标记
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        private static string parseBold(string sDetail)
        {
            sDetail = Regex.Replace(sDetail, @"\[b(?:\s*)\]", "<b>", options);
            sDetail = Regex.Replace(sDetail, @"\[i(?:\s*)\]", "<i>", options);
            sDetail = Regex.Replace(sDetail, @"\[u(?:\s*)\]", "<u>", options);
            sDetail = Regex.Replace(sDetail, @"\[/b(?:\s*)\]", "</b>", options);
            sDetail = Regex.Replace(sDetail, @"\[/i(?:\s*)\]", "</i>", options);
            sDetail = Regex.Replace(sDetail, @"\[/u(?:\s*)\]", "</u>", options);
            return sDetail;
        }

        /// <summary>
        /// 处理Font标记
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        private static string parseFont(string sDetail)
        {
            sDetail = Regex.Replace(sDetail, @"\[color=([^\[\<]+?)\]", "<font color=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, @"\[size=(\d+?)\]", "<font size=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, @"\[size=(\d+(\.\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\]", "<font style=\"font-size: $1\">", options);
            sDetail = Regex.Replace(sDetail, @"\[font=([^\[\<]+?)\]", "<font face=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, @"\[align=([^\[\<]+?)\]", "<p align=\"$1\">", options);
            sDetail = Regex.Replace(sDetail, @"\[float=(left|right)\]", "<br style=\"clear: both\"><span style=\"float: $1;\">", options);
            sDetail = Regex.Replace(sDetail, @"\[/color(?:\s*)\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, @"\[/size(?:\s*)\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, @"\[/font(?:\s*)\]", "</font>", options);
            sDetail = Regex.Replace(sDetail, @"\[/align(?:\s*)\]", "</p>", options);
            sDetail = Regex.Replace(sDetail, @"\[/float(?:\s*)\]", "</span>", options);
            return sDetail;
        }

        /// <summary>
        /// 处理URL标记
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string ParseUrl(string sDetail)
        {
            sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\](www\.(.*?))\[/url(?:\s*)\]", "<a href=\"http://$1\" target=\"_blank\">$1</a>", options);
            sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\]\s*(([^\[""']+?))\s*\[\/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$1</a>", options);

            //sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\]\s*((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent)([^\[""']+?))\s*\[\/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$1</a>", options);
            sDetail = Regex.Replace(sDetail, @"\[url=www.([^\[""']+?)(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"http://www.$1\" target=\"_blank\">$2</a>", options);
            sDetail = Regex.Replace(sDetail, @"\[url=(([^\[""']+?))(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$3</a>", options);

            //sDetail = Regex.Replace(sDetail, @"\[url=((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent://)([^\[""']+?))(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$4</a>", options);
            return sDetail;
        }

        /// <summary>
        /// 替换版规中UBB的方法
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string ParseSimpleUBB(string sDetail)
        {
            sDetail = ParseImg(sDetail);
            sDetail = parseFont(sDetail);
            sDetail = parseBold(sDetail);
            sDetail = ParseUrl(sDetail);
            return sDetail;
        }

        /// <summary>
        /// 隐藏[hide]标签中的内容
        /// </summary>
        /// <param name="str">帖子内容</param>
        /// <param name="hide">hide标记</param>
        /// <returns>帖子内容</returns>
        public static string HideDetail(string str, int hide, int usercredit)
        {
            if (hide == 0)
                return str;

            Match m;
            int hidecredits = 0;
            int intTableIndexOf = str.ToLower().IndexOf("[hide");

            while (intTableIndexOf >= 0 && str.ToLower().IndexOf("[/hide]") >= 0)
            {
                for (m = r[10].Match(str); m.Success; m = m.NextMatch())
                {
                    if (hide == 1)
                        str = str.Replace(m.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">***** 该内容需会员回复才可浏览 *****</div></div>");
                    else
                        str = str.Replace(m.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">以下内容会员跟帖回复才能看到</div><div class=\"hidetext\"><br />==============================<br /><br />" + m.Groups[1].ToString() + "<br /><br />==============================</div></div>");
                }
                for (m = r[16].Match(str); m.Success; m = m.NextMatch())
                {
                    hidecredits = TranslateUtils.ToInt(m.Groups[1].ToString());
                    if (hide != -2 && usercredit < hidecredits)
                        str = str.Replace(m.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">***** 该内容需浏览者积分高于" + hidecredits + " 才可浏览 ***** </div></div>");
                    else
                        str = str.Replace(m.Groups[0].ToString(), "<div class=\"hide\"><div class=\"hidestyle\">以下内容只有在浏览者积分高于 " + hidecredits + " 时才显示</div><div class=\"hidetext\"><br />==============================<br /><br />" + m.Groups[2].ToString() + "<br /><br />==============================</div></div>");
                }

                if (intTableIndexOf + 7 > str.Length)
                    intTableIndexOf = str.ToLower().IndexOf("[table", str.Length);
                else
                    intTableIndexOf = str.ToLower().IndexOf("[table", intTableIndexOf + 7);
            }
            return str;
        }



        /// <summary>
        /// 转换表情
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <param name="__smiliesinfo">表情数组</param>
        /// <param name="smiliesmax">每种表情的最大使用数</param>
        /// <returns>帖子内容</returns>
        //private static string ParseSmilies(string sDetail, SmiliesInfo[] smiliesinfo, int smiliesmax)
        //{
        //    if (smiliesinfo == null)
        //        return sDetail;

        //    string smilieformatstr = "[smilie]{0}editor/images/smilies/{1}[/smilie]";
        //    for (int i = 0; i < Smilies.regexSmile.Length; i++)
        //    {
        //        if (smiliesmax > 0)
        //            sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(smilieformatstr, BaseConfigs.GetForumPath, smiliesinfo[i].Url), smiliesmax);
        //        else
        //            sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(smilieformatstr, BaseConfigs.GetForumPath, smiliesinfo[i].Url));
        //    }
        //    return sDetail;
        //}

        /// <summary>
        /// 转换表格
        /// </summary>
        /// <param name="str">帖子内容</param>
        /// <returns>帖子内容</returns>
        private static string ParseTable(string str)
        {
            Match m;
            string stable = "";
            string width = "";
            string bgcolor = "";
            int intTableIndexOf = str.ToLower().IndexOf("[table");

            while (intTableIndexOf >= 0 && str.ToLower().IndexOf("[/table]") >= 0)
            {
                for (m = r[11].Match(str); m.Success; m = m.NextMatch())
                {
                    width = m.Groups[1].ToString();
                    width = StringUtils.CutString(width, width.Length - 1, width.Length).Equals("%") ? (TranslateUtils.ToInt(StringUtils.CutString(width, 0, width.Length - 1), 100) <= 98 ? width : "98%") : (TranslateUtils.ToInt(width, 560) <= 560 ? width : "560");

                    bgcolor = m.Groups[2].ToString();

                    stable = "<table class=\"t_table\" cellspacing=\"1\" cellpadding=\"4\" style=\"";
                    stable += width.Equals("") ? "" : ("width:" + width + ";");
                    stable += "".Equals(bgcolor) ? "" : ("background: " + bgcolor + ";");
                    stable += "\">";


                    width = m.Groups[3].ToString();
                    width = Regex.Replace(width, @"\[td=(\d{1,2}),(\d{1,2})(,(\d{1,4}%?))?\]", "<td colspan=\"$1\" rowspan=\"$2\" width=\"$4\" class=\"t_table\">", options);
                    width = Regex.Replace(width, @"\[tr\]", "<tr>", options);
                    width = Regex.Replace(width, @"\[td\]", "<td>", options);
                    width = Regex.Replace(width, @"\[\/td\]", "</td>", options);
                    width = Regex.Replace(width, @"\[\/tr\]", "</tr>", options);
                    width = Regex.Replace(width, @"\<td\>\<\/td\>", "<td>&nbsp;</td>", options);

                    stable += width;
                    stable += "</table>";

                    str = str.Replace(m.Groups[0].ToString(), stable);
                }
                intTableIndexOf = str.ToLower().IndexOf("[table", intTableIndexOf + 7);
            }

            return str;
        }


        /// <summary>
        /// 转换code标签
        /// </summary>
        /// <param name="text">帖子内容</param>
        /// <param name="pcodecount">code的数量</param>
        /// <param name="builder">转换后的内容</param>
        /// <returns>帖子内容</returns>
        private static string Parsecode(string text, string prefix, ref int pcodecount, ref StringBuilder builder)
        {
            text = Regex.Replace(text, @"^[\n\r]*([\s\S]+?)[\n\r]*$", "$1", options);

            if (!builder.ToString().Equals(""))
            {
                builder.Append("<>");
            }
            builder.Append("<div class=\"blockcode\"><div id=\"code" + prefix + "_" + pcodecount.ToString() + "\"><ol>");
            foreach (string str in StringUtils.SplitStringIgnoreCase(text, "\r\n"))
            {
                //解决Firefox下复制代码无换行的问题
                //if (allowhtml == 0)
                    builder.Append("<li>" + str + "<br/></li>{rn}");
                //else
                //    builder.Append("<li>" + str + "<br/></li>\r\n");
            }
            builder.Append("</ol></div><em onclick=\"copycode($('code" + prefix + "_" + pcodecount.ToString() + "'));\">复制代码</em></div>");

            pcodecount++;
            text = "[\tDISCUZ_CODE_" + prefix + "_" + pcodecount.ToString() + "\t]";
            return text;
        }

        public static string ParseMedia(string type, int width, int height, bool autostart, string url)
        {
            if (!StringUtils.ContainsIgnoreCase("ra,rm,wma,wmv,mp3,mov", type))
                return "";
            url = url.Replace("\\\\", "\\").Replace("<", string.Empty).Replace(">", string.Empty);
            Random r = new Random(3);
            string mediaid = "media_" + r.Next();
            switch (type)
            {
                case "ra":
                    return string.Format(@"<object classid=""clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA"" width=""{0}"" height=""32""><param name=""autostart"" value=""{1}"" /><param name=""src"" value=""{2}"" /><param name=""controls"" value=""controlpanel"" /><param name=""console"" value=""{3}_"" /><embed src=""{2}"" type=""audio/x-pn-realaudio-plugin"" controls=""ControlPanel"" {4} console=""{3}_"" width=""{0}"" height=""32""></embed></object>", width, autostart ? 1 : 0, url, mediaid, autostart ? "autostart=\"true\"" : string.Empty);
                case "rm":
                    return string.Format(@"<object classid=""clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA"" width=""{0}"" height=""{1}""><param name=""autostart"" value=""{2}"" /><param name=""src"" value=""{3}"" /><param name=""controls"" value=""imagewindow"" /><param name=""console"" value=""{4}_"" /><embed src=""{3}"" type=""audio/x-pn-realaudio-plugin"" controls=""IMAGEWINDOW"" console=""{4}_"" width=""{0}"" height=""{1}""></embed></object><br /><object classid=""clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA"" width=""{0}"" height=""32""><param name=""src"" value=""{3}"" /><param name=""controls"" value=""controlpanel"" /><param name=""console"" value=""{4}_"" /><embed src=""{3}"" type=""audio/x-pn-realaudio-plugin"" controls=""ControlPanel"" {5} console=""{4}_"" width=""{0}"" height=""32""></embed></object>", width, height, autostart ? 1 : 0, url, mediaid, autostart ? "autostart=\"true\"" : string.Empty);
                case "wma":
                    return string.Format(@"<object classid=""clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6"" width=""{0}"" height=""64""><param name=""autostart"" value=""{1}"" /><param name=""url"" value=""{2}"" /><embed src=""{2}"" autostart=""{1}"" type=""audio/x-ms-wma"" width=""{0}"" height=""64""></embed></object>", width, autostart ? 1 : 0, url);
                case "wmv":
                    return string.Format(@"<object classid=""clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6"" width=""{0}"" height=""{1}""><param name=""autostart"" value=""{2}"" /><param name=""url"" value=""{3}"" /><embed src=""{3}"" autostart=""{2}"" type=""video/x-ms-wmv"" width=""{0}"" height=""{1}""></embed></object>", width, height, autostart ? 1 : 0, url);
                case "mp3":
                    return string.Format(@"<object classid=""clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6"" width=""{0}"" height=""64""><param name=""autostart"" value=""{1}""/><param name=""url"" value=""{2}"" /><embed src=""{2}"" autostart=""{1}"" type=""application/x-mplayer2"" width=""{0}"" height=""64""></embed></object>", width, autostart ? 1 : 0, url);
                case "mov":
                    return string.Format(@"<object classid=""clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B"" width=""{0}"" height=""{1}""><param name=""autostart"" value=""{2}"" /><param name=""src"" value=""{3}"" /><embed controller=""true"" width=""{0}"" height=""{1}"" src=""{3}"" autostart=""{2}""></embed></object>", width, height, autostart.ToString().ToLower(), url);
            }

            return string.Empty;
        }

        public static string ParseAudio(string autostart, string url)
        {
            return string.Format(@"<object width=""400"" height=""64"" classid=""clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6""><param value=""0"" name=""invokeURLs""><param value=""{0}"" name=""autostart""><param value=""{1}"" name=""url""><embed width=""400"" height=""64"" type=""application/x-mplayer2"" autostart=""{0}"" src=""{1}""></object>", autostart != "" ? "1" : "0", url);
        }

        public static string ParseP(string lineHeight, string textIndent, string textAlign, string content)
        {
            return string.Format(@"<p style=""line-height: {0}px; text-indent: {1}em; text-align: {2};"">{3}</p>", lineHeight, textIndent, textAlign, content);
        }

        public static string ParseFlash(string flashWidth, string flashHeight, string flashUrl)
        {
            flashWidth = flashWidth == "" ? "550" : flashWidth;
            flashHeight = flashHeight == "" ? "400" : flashHeight;
            return string.Format(@"<script reload=""1"" type=""text/javascript"">document.write(AC_FL_RunContent('width', '{0}', 'height', '{1}', 'allowNetworking', 'internal', 'allowScriptAccess', 'never', 'src', '{2}', 'quality', 'high', 'bgcolor', '#ffffff', 'wmode', 'transparent', 'allowfullscreen', 'true'));</script>", flashWidth, flashHeight, flashUrl);
        }

        /// <summary>
        /// 该方法已被抽到Utils类中
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearBR(string sDetail)
        {
            return Regex.Replace(sDetail, @"[\r\n]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 清除[attach][attachimg]标签
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string ClearAttachUBB(string sDetail)
        {
            sDetail = r[13].Replace(sDetail, string.Empty);
            return r[14].Replace(sDetail, string.Empty);
        }

        public static string ReplaceAttachUBB(string content, int aid, int attachmentID, string fileName, string attachmentUrl, string imageUrl)
        {
            if (StringUtils.Contains(content, string.Format("[attachimg]{0}[/attachimg]", aid)))
            {
                //string imgHtml = string.Format(@"<a href=""@{0}"" target=""_blank""><img src=""@{1}"" border=""0"" /></a>", attachmentUrl.Trim('/'), imageUrl.Trim('/'));
                string imgHtml = UBBUtility.GetUBB_Attachment(attachmentID);
                return content.Replace(string.Format("[attachimg]{0}[/attachimg]", aid), imgHtml);
            }
            else if (StringUtils.Contains(content, string.Format("[attach]{0}[/attach]", aid)))
            {
                //string attachHtml = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", RedirectPage.GetRedirectUrl(attachmentID), fileName);
                string attachHtml = UBBUtility.GetUBB_Attachment(attachmentID);
                return content.Replace(string.Format("[attach]{0}[/attach]", aid), attachHtml);
            }
            return content;
        }

        public static bool IsInContent(string content, int aid)
        {
            if (StringUtils.Contains(content, string.Format("[attachimg]{0}[/attachimg]", aid)) || StringUtils.Contains(content, string.Format("[attach]{0}[/attach]", aid)))
            {
                return true;
            }
            return false;
        }
    }
}
