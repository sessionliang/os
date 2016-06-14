using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using BaiRong.Core;
using SiteServer.CMS.Model;
using System;
using System.Text;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Controls
{
    public class AuxiliaryControl : Control
    {
        private NameValueCollection formCollection;
        private PublishmentSystemInfo publishmentSystemInfo;
        private int nodeID;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private string tableName;
        private bool isEdit;
        private bool isPostBack;
        ArrayList excludeAttributeNames = new ArrayList();

        public void SetParameters(NameValueCollection formCollection, PublishmentSystemInfo publishmentSystemInfo, int nodeID, ArrayList relatedIdentities, ETableStyle tableStyle, string tableName, bool isEdit, bool isPostBack)
        {
            this.formCollection = formCollection;
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.nodeID = nodeID;
            this.relatedIdentities = relatedIdentities;
            this.tableStyle = tableStyle;
            this.tableName = tableName;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;
        }

        public void AddExcludeAttributeNames(ArrayList arraylist)
        {
            this.excludeAttributeNames.AddRange(arraylist);
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (!string.IsNullOrEmpty(this.tableName))
            {
                if (this.formCollection == null)
                {
                    if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                    {
                        this.formCollection = HttpContext.Current.Request.Form;
                    }
                    else
                    {
                        this.formCollection = new NameValueCollection();
                    }
                }

                StringBuilder builder = new StringBuilder();
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
                NameValueCollection pageScripts = new NameValueCollection();

                if (styleInfoArrayList != null)
                {
                    bool isPreviousSingleLine = true;
                    bool isPreviousLeftColumn = false;
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible && !this.excludeAttributeNames.Contains(styleInfo.AttributeName.ToLower()))
                        {
                            string text = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);
                            string value = InputTypeParser.Parse(publishmentSystemInfo, nodeID, styleInfo, this.tableStyle, styleInfo.AttributeName, this.formCollection, this.isEdit, isPostBack, null, pageScripts, true, true);

                            if (builder.Length > 0)
                            {
                                if (isPreviousSingleLine)
                                {
                                    builder.Append("</tr>");
                                }
                                else
                                {
                                    if (!isPreviousLeftColumn)
                                    {
                                        builder.Append("</tr>");
                                    }
                                    else if (styleInfo.IsSingleLine)
                                    {
                                        builder.Append(@"<td></td><td></td></tr>");
                                    }
                                }
                            }

                            //this line

                            if (styleInfo.IsSingleLine || isPreviousSingleLine || !isPreviousLeftColumn)
                            {
                                builder.Append("<tr>");
                            }

                            if (styleInfo.InputType == EInputType.TextEditor)
                            {
                                string commands = WebUtils.GetTextEditorCommands(publishmentSystemInfo, ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString), styleInfo.AttributeName);
                                builder.AppendFormat(@"<td>{0}</td><td colspan=""3"">{1}</td></tr><tr><td colspan=""4"">{2}</td>", text, commands, value);

                                //自动检测敏感词
                                builder.AppendFormat(@"{0}", WebUtils.GetAutoCheckKeywordsCommands(publishmentSystemInfo, ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString), styleInfo.AttributeName));
                            }
                            else
                            {
                                #region by 20160215 sofuny 标题和副标题增加敏感词监测
                                if (styleInfo.AttributeName == "Title" || styleInfo.AttributeName == "SubTitle")
                                {
                                    builder.AppendFormat(@"<td>{0}</td><td {1}>{2}</td>", text, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, value);
                                    //自动检测敏感词
                                    builder.AppendFormat(@"{0}", WebUtils.GetAutoCheckKeywordsCommandsByInput(publishmentSystemInfo, styleInfo.AttributeName));
                                }
                                #endregion
                                else
                                    builder.AppendFormat(@"<td>{0}</td><td {1}>{2}</td>", text, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, value);
                            }

                            if (styleInfo.IsSingleLine)
                            {
                                isPreviousSingleLine = true;
                                isPreviousLeftColumn = false;
                            }
                            else
                            {
                                isPreviousSingleLine = false;
                                isPreviousLeftColumn = !isPreviousLeftColumn;
                            }
                        }
                    }

                    if (builder.Length > 0)
                    {
                        if (isPreviousSingleLine || !isPreviousLeftColumn)
                        {
                            builder.Append("</tr>");
                        }
                        else
                        {
                            builder.Append(@"<td></td><td></td></tr>");
                        }
                    }

                    output.Write(builder.ToString());

                    foreach (string key in pageScripts.Keys)
                    {
                        output.Write(pageScripts[key]);
                    }
                }
            }
        }
    }
}
