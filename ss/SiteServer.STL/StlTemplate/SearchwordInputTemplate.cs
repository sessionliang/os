using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using BaiRong.Core.Data.Provider;

using System;
using System.Collections.Specialized;
using System.Collections;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser;

using System.Web.UI.WebControls;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Core;
using SiteServer.CMS.Services;

namespace SiteServer.STL.StlTemplate
{
    /// <summary>
    /// 搜索关键词模板
    /// </summary>
    public class SearchwordInputTemplate
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleSearchInputInfo inputInfo;
        private SearchwordSettingInfo settingInfo;
        private string formID;
        private string type;
        private string formAttributes;

        public SearchwordInputTemplate(PublishmentSystemInfo publishmentSystemInfo, SearchwordSettingInfo settingInfo, TagStyleSearchInputInfo inputInfo, string type, string formAttributes)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.inputInfo = inputInfo;
            this.settingInfo = settingInfo;
            this.formID = string.Format("searchForm_{0}", StringUtils.GetRandomInt(1, 100000));
            this.type = type;
            this.formAttributes = formAttributes;
        }

        public string GetTemplate(bool isTemplate, bool isRelated, string searchwordInputTemplateString)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(searchwordInputTemplateString))
            {
                if (isTemplate)
                {
                    //自定义模板
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.settingInfo.StyleTemplate);
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.settingInfo.ScriptTemplate.Replace("{formID}", this.formID));
                    builder.Append(this.settingInfo.ContentTemplate.Replace("{onkeyup}", isRelated ? "stlSearchwordRelated_{formID}()" : string.Empty).Replace("{formID}", this.formID));
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle());
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript(isRelated).Replace("{formID}", this.formID));
                    builder.Append(this.GetContent(isRelated).Replace("{onkeyup}", isRelated ? "stlSearchwordRelated_{formID}()" : string.Empty).Replace("{formID}", this.formID));
                }
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.settingInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.settingInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.settingInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.settingInfo.ScriptTemplate);
                    }
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle());
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript(isRelated).Replace("{formID}", this.formID));
                }
                builder.Append(searchwordInputTemplateString.Replace("{onkeyup}", isRelated ? "stlSearchwordRelated_{formID}()" : string.Empty).Replace("{formID}", this.formID));
            }
            return builder.ToString();
        }

        public string GetContent(bool isRelated)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"
<form id=""{{formID}}"" method=""get"" action=""{1}"" target=""{2}""{3}>", this.formID, this.inputInfo.SearchUrl, this.inputInfo.OpenWin ? "_blank" : "_self", formAttributes);

            if (inputInfo.IsType == false && !string.IsNullOrEmpty(this.type))
            {
                builder.AppendFormat(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" />", this.type);
            }

            builder.Append(@"
<table><tr><td>");

            //暂时只搜索标题
            //if (inputInfo.IsType)
            //{
            //    DropDownList ddl = new DropDownList();
            //    ddl.ID = "type";
            //    ListItem listItem = new ListItem("标题", ContentAttribute.Title);
            //    ddl.Items.Add(listItem);
            //    listItem = new ListItem("内容正文", BackgroundContentAttribute.Content);
            //    ddl.Items.Add(listItem);

            //    builder.AppendFormat("类型：{0}&nbsp;", ControlUtils.GetControlRenderHtml(ddl));
            //}

            builder.AppendFormat(@"
关键字：&nbsp;<input autocomplete=""off"" style=""border: #ccc 1px solid;width:{0}"" name=""word"" onkeyup=""{{onkeyup}}""/>&nbsp;", TranslateUtils.ToWidth(inputInfo.InputWidth));
            if (isRelated)
                builder.AppendFormat(@"
<ul id=""relatedwordpanel_{{formID}}"" style=""display:none;""></ul>
<script id=""relatedwordtemplate_{{formID}}"" type=""text/html"">
<li><a href=""javascript:;"" onmouseover=""relatedwordSet(this);"" onclick=""relatedwordSearch(this);"" value=""{{word}}"">{{word}}<span>大约{{resultcount}}个结果</span></a></li>
</script>");

            //            if (inputInfo.IsChannel)
            //            {
            //                if (inputInfo.IsChannelRadio)
            //                {
            //                    builder.AppendFormat(@"
            //所属栏目：</td><td>{0}</td>
            //<td>&nbsp;", this.GetChannelList());
            //                }
            //                else
            //                {
            //                    builder.AppendFormat("所属栏目：{0}&nbsp;", this.GetChannelList());
            //                }
            //            }

            //if (inputInfo.IsDate)
            //{
            //    DropDownList ddl = new DropDownList();
            //    ddl.ID = "date";
            //    ListItem listItem = new ListItem("全部", "0");
            //    ddl.Items.Add(listItem);
            //    listItem = new ListItem("1天内", "1");
            //    ddl.Items.Add(listItem);
            //    listItem = new ListItem("1周内", "7");
            //    ddl.Items.Add(listItem);
            //    listItem = new ListItem("1个月内", "30");
            //    ddl.Items.Add(listItem);
            //    listItem = new ListItem("1年内", "365");
            //    ddl.Items.Add(listItem);

            //    builder.AppendFormat("日期：{0}&nbsp;", ControlUtils.GetControlRenderHtml(ddl));
            //}

            builder.AppendFormat(@"</td>
<td><input type=""button"" onclick=""document.getElementById('{{formID}}').submit();return false;"" value="" 搜 索 "" />
</td></tr></table>", this.formID);

            builder.Append(@"
</form>");

            return builder.ToString();
        }

        private string GetChannelList()
        {
            ListControl list = new DropDownList();
            if (inputInfo.IsChannelRadio)
            {
                RadioButtonList rbl = new RadioButtonList();
                rbl.RepeatDirection = RepeatDirection.Horizontal;
                list = rbl;
            }
            list.ID = "channelID";

            if (!inputInfo.IsChannelRadio)
            {
                NodeManager.AddListItems(list.Items, this.publishmentSystemInfo, false, false);
            }
            else
            {
                ListItem listItem = new ListItem("全部", "");
                listItem.Selected = true;
                list.Items.Add(listItem);
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, this.publishmentSystemInfo.PublishmentSystemID);
                if (inputInfo.IsChannelRadio && nodeIDArrayList.Count > 5)
                {
                    nodeIDArrayList.RemoveRange(5, nodeIDArrayList.Count - 5);
                }
                foreach (int nodeID in nodeIDArrayList)
                {
                    listItem = new ListItem(NodeManager.GetNodeName(this.publishmentSystemInfo.PublishmentSystemID, nodeID), nodeID.ToString());
                    list.Items.Add(listItem);
                }
            }
            return ControlUtils.GetControlRenderHtml(list);
        }

        public string FormID
        {
            get { return this.formID; }
        }

        internal string GetStyle()
        {
            StringBuilder builder = new StringBuilder();
            return builder.ToString();
        }

        internal string GetScript(bool isRelated)
        {
            StringBuilder builder = new StringBuilder();
            if (isRelated)
            {
                string getSearchwordString = PageService.GetSearchwordsUrl(this.publishmentSystemInfo);
                builder.AppendFormat(@"
function relatedwordSearch(element){{
      if(!!element){{
         relatedwordSet(element);
         document.getElementById('{{formID}}').submit();
         return false;
      }}
}}
function relatedwordSet(element){{
     if(!!element){{
         $(""input[name='word']"").val($(element).attr(""value""));
     }}
}}
function stlSearchwordRelated_{{formID}}(){{
      var searchword = $('input[name=""word""]').val();
      var filterSearchwordDataTemplate = $(""#relatedwordtemplate_{{formID}}"").html();
      if(!searchword){{
            $('#relatedwordpanel_{{formID}}').html('未找到相关内容');
            $('#relatedwordpanel_{{formID}}').css('display','none');
            return false;
      }}
      else{{
           $.get('{0}', {{searchword:searchword}},function(data){{
           if(data.length == 0){{                                                                                                                                     
               $('#relatedwordpanel_{{formID}}').html('未找到相关内容');
               $('#relatedwordpanel_{{formID}}').css('display','none');
               return false;
           }}
           else{{
                $('#relatedwordpanel_{{formID}}').html('');
                data=eval(""(""+data+"")"");
                for(var i=0;i<data.length;i++){{
                $('#relatedwordpanel_{{formID}}').append(filterSearchwordDataTemplate.replace(/{{word}}/g,data[i].searchWord)
                .replace(/{{resultcount}}/g,data[i].searchResultCount)
                .replace(/{{count}}/g,data[i].searchCount));
                }}
                $('#relatedwordpanel_{{formID}}').css('display','block');
           }}
           }});
     }}
}}
", getSearchwordString);
            }
            return builder.ToString();
        }
    }
}
