using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Text.RegularExpressions;
using System.Collections;

namespace BaiRong.Model
{
    public enum ETextEditorType
    {
        UEditor,                    //UEditor
        CKEditor,                   //CKEditor
        KindEditor,                 //KindEditor
        EWebEditor,					//EWebEditor
        FCKEditor,					//FCKEditor
        xHtmlEditor,                //xHtmlEditor
    }

    public class ETextEditorTypeUtils
    {
        public static string GetValue(ETextEditorType type)
        {
            if (type == ETextEditorType.UEditor)
            {
                return "UEditor";
            }
            else if (type == ETextEditorType.CKEditor)
            {
                return "CKEditor";
            }
            else if (type == ETextEditorType.KindEditor)
            {
                return "KindEditor";
            }
            else if (type == ETextEditorType.EWebEditor)
            {
                return "EWebEditor";
            }
            else if (type == ETextEditorType.FCKEditor)
            {
                return "FCKEditor";
            }
            else if (type == ETextEditorType.xHtmlEditor)
            {
                return "xHtmlEditor";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ETextEditorType type)
        {
            if (type == ETextEditorType.UEditor)
            {
                return "UEditor";
            }
            else if (type == ETextEditorType.CKEditor)
            {
                return "CKEditor";
            }
            else if (type == ETextEditorType.KindEditor)
            {
                return "KindEditor";
            }
            else if (type == ETextEditorType.EWebEditor)
            {
                return "EWebEditor";
            }
            else if (type == ETextEditorType.FCKEditor)
            {
                return "FCKEditor";
            }
            else if (type == ETextEditorType.xHtmlEditor)
            {
                return "xHtmlEditor";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ETextEditorType GetEnumType(string typeStr)
        {
            ETextEditorType retval = ETextEditorType.UEditor;

            if (Equals(ETextEditorType.UEditor, typeStr))
            {
                retval = ETextEditorType.UEditor;
            }
            else if (Equals(ETextEditorType.CKEditor, typeStr))
            {
                retval = ETextEditorType.CKEditor;
            }
            else if (Equals(ETextEditorType.KindEditor, typeStr))
            {
                retval = ETextEditorType.KindEditor;
            }
            else if (Equals(ETextEditorType.EWebEditor, typeStr))
            {
                retval = ETextEditorType.EWebEditor;
            }
            else if (Equals(ETextEditorType.FCKEditor, typeStr))
            {
                retval = ETextEditorType.FCKEditor;
            }
            else if (Equals(ETextEditorType.xHtmlEditor, typeStr))
            {
                retval = ETextEditorType.xHtmlEditor;
            }

            return retval;
        }

        public static bool Equals(ETextEditorType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ETextEditorType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ETextEditorType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(ETextEditorType.UEditor, false));
                listControl.Items.Add(GetListItem(ETextEditorType.CKEditor, false));
                //listControl.Items.Add(GetListItem(ETextEditorType.KindEditor, false));
                //listControl.Items.Add(GetListItem(ETextEditorType.EWebEditor, false));
                //listControl.Items.Add(GetListItem(ETextEditorType.FCKEditor, false));
                //listControl.Items.Add(GetListItem(ETextEditorType.xHtmlEditor, false));
            }
        }

        public static bool IsInsertHtml(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor || editorType == ETextEditorType.CKEditor || editorType == ETextEditorType.EWebEditor || editorType == ETextEditorType.FCKEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetInsertHtmlScript(ETextEditorType editorType, string attributeName, string html)
        {
            html = html.Replace("\"", "'");
            string script = string.Format(@"UE.getEditor(""{0}"").execCommand(""insertHTML"",""{1}"");", attributeName, html);
            if (!string.IsNullOrEmpty(html))
            {
                html = html.Replace(@"""", @"\""");
                if (editorType == ETextEditorType.UEditor)
                {
                    script = string.Format(@"UE.getEditor(""{0}"").execCommand(""insertHTML"",""{1}"");", attributeName, html);
                }
                else if (editorType == ETextEditorType.CKEditor)
                {
                    script = string.Format(@"CKEDITOR.instances.{0}.insertHtml(""{1}"");", attributeName, html);
                }
                else if (editorType == ETextEditorType.EWebEditor)
                {
                    script = string.Format(@"document.getElementById(""eWebEditor_{0}"").insertHTML(""{1}"");", attributeName, html);
                }
                else if (editorType == ETextEditorType.FCKEditor)
                {
                    script = string.Format(@"FCKeditorAPI.GetInstance(""{0}"").InsertHtml(""{1}"");", attributeName, html);
                }
            }
            return script;
        }

        public static string GetEditorInstanceScript(ETextEditorType editorType)
        {
            string script = "UE";

            if (editorType == ETextEditorType.UEditor)
            {
                script = "UE";
            }
            else if (editorType == ETextEditorType.CKEditor)
            {
                script = "CKEDITOR";
            }
            else if (editorType == ETextEditorType.EWebEditor)
            {
                script = "document";
            }
            else if (editorType == ETextEditorType.FCKEditor)
            {
                script = "FCKeditorAPI";
            }

            return script;
        }

        public static bool IsInsertVideo(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetInsertVideoScript(ETextEditorType editorType, string attributeName, string playUrl, int width, int height, bool isAutoPlay)
        {
            string script = string.Empty;
            if (!string.IsNullOrEmpty(playUrl))
            {
                if (editorType == ETextEditorType.UEditor)
                {
                    if (width > 0 && height > 0)
                    {
                        script = string.Format(@"UE.getEditor(""{0}"").execCommand(""insertVideo"",{{url: ""{1}"",width: {2},height: {3},isAutoPlay: ""{4}""}});", attributeName, playUrl, width, height, isAutoPlay.ToString().ToLower());
                    }
                    else
                    {
                        script = string.Format(@"UE.getEditor(""{0}"").execCommand(""insertVideo"",{{url: ""{1}"",isAutoPlay: ""{2}""}});", attributeName, playUrl, isAutoPlay.ToString().ToLower());
                    }
                }
            }
            return script;
        }

        public static bool IsInsertAudio(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetInsertAudioScript(ETextEditorType editorType, string attributeName, string playUrl, bool isAutoPlay)
        {
            string script = string.Empty;
            if (!string.IsNullOrEmpty(playUrl))
            {
                if (editorType == ETextEditorType.UEditor)
                {
                    script = string.Format(@"UE.getEditor(""{0}"").execCommand(""music"",{{url: ""{1}"",isAutoPlay: ""{2}""}});", attributeName, playUrl, isAutoPlay.ToString().ToLower());
                }
            }
            return script;
        }

        public static bool IsGetPureText(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetPureTextScript(ETextEditorType editorType, string attributeName)
        {
            string script = string.Empty;
            if (editorType == ETextEditorType.UEditor)
            {
                script = string.Format(@"UE.getEditor(""{0}"").getContentTxt();", attributeName);
            }
            return script;
        }

        public static bool IsGetContent(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetContentScript(ETextEditorType editorType, string attributeName)
        {
            string script = string.Empty;
            if (editorType == ETextEditorType.UEditor)
            {
                script = string.Format(@"UE.getEditor(""{0}"").getContent();", attributeName);
            }
            return script;
        }

        public static bool IsSetContent(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string GetSetContentScript(ETextEditorType editorType, string attributeName, string contentWithoutQuote)
        {
            string script = string.Empty;
            if (editorType == ETextEditorType.UEditor)
            {
                script = string.Format(@"UE.getEditor(""{0}"").setContent({1});", attributeName, contentWithoutQuote);
            }
            return script;
        }

        public static bool IsInsertHtmlTranslateStlElement(ETextEditorType editorType)
        {
            if (editorType == ETextEditorType.UEditor)
            {
                return true;
            }
            return false;
        }

        public static string TranslateToStlElement(ETextEditorType editorType, string html)
        {
            string retval = html;
            if (!string.IsNullOrEmpty(retval))
            {
                if (editorType == ETextEditorType.UEditor)
                {
                    Regex regex = new Regex(@"<embed[^>]*class=""edui-faked-[^>]*/>", ((RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.IgnorePatternWhitespace) | RegexOptions.Compiled);

                    MatchCollection mc = regex.Matches(retval);
                    for (int i = 0; i < mc.Count; i++)
                    {
                        string original = mc[i].Value;
                        if (original.Contains("edui-faked-video"))
                        {
                            string replace = original.Replace("embed", "stl:player");
                            retval = retval.Replace(original, replace);
                        }
                        else if (original.Contains("edui-faked-music"))
                        {
                            string replace = original.Replace("embed", "stl:audio");
                            retval = retval.Replace(original, replace);
                        }
                    }

                    //retval = retval.Replace(@"<embed class=""edui-faked-video"" ", "<stl:player ");
                    //retval = retval.Replace(@"<embed class=""edui-faked-music"" ", "<stl:audio ");
                }
            }
            return retval;
        }

        public static string TranslateToHtml(ETextEditorType editorType, string html)
        {
            string retval = html;
            if (!string.IsNullOrEmpty(retval))
            {
                if (editorType == ETextEditorType.UEditor)
                {
                    retval = retval.Replace("<stl:player ", @"<embed ");
                    retval = retval.Replace("<stl:audio ", @"<embed ");
                }
            }
            return retval;
        }
    }
}
