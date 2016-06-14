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

namespace SiteServer.STL.StlTemplate
{
	public class SearchInputTemplate
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleInfo tagStyleInfo;
        private TagStyleSearchInputInfo inputInfo;
        private string formID;
        private string type;
        private string formAttributes;

        public SearchInputTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo, TagStyleSearchInputInfo inputInfo, string type, string formAttributes)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.tagStyleInfo = tagStyleInfo;
            this.inputInfo = inputInfo;
            int styleID = this.tagStyleInfo.StyleID;
            if (styleID <= 0)
            {
                styleID = StringUtils.GetRandomInt(1, 100000);
            }
            this.formID = string.Format("searchForm_{0}", styleID);
            this.type = type;
            this.formAttributes = formAttributes;
        }

        public string GetTemplate(bool isTemplate)
        {
            StringBuilder builder = new StringBuilder();

            if (isTemplate)
            {
                builder.Append(this.tagStyleInfo.ContentTemplate);
            }
            else
            {
                builder.Append(this.GetContent());
            }

            return builder.ToString();
        }

        public string GetContent()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"
<form id=""{0}"" method=""get"" action=""{1}"" target=""{2}""{3}>", this.formID, this.inputInfo.SearchUrl, this.inputInfo.OpenWin ? "_blank" : "_self", formAttributes);

            if (inputInfo.IsType == false && !string.IsNullOrEmpty(this.type))
            {
                builder.AppendFormat(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" />", this.type);
            }

            builder.Append(@"
<table><tr><td>");

            if (inputInfo.IsType)
            {
                DropDownList ddl = new DropDownList();
                ddl.ID = "type";
                ListItem listItem = new ListItem("标题", ContentAttribute.Title);
                ddl.Items.Add(listItem);
                listItem = new ListItem("内容正文", BackgroundContentAttribute.Content);
                ddl.Items.Add(listItem);

                builder.AppendFormat("类型：{0}&nbsp;", ControlUtils.GetControlRenderHtml(ddl));
            }

            builder.AppendFormat(@"
关键字：&nbsp;<input style=""border: #ccc 1px solid;width:{0}"" name=""word"" />&nbsp;", TranslateUtils.ToWidth(inputInfo.InputWidth));

            if (inputInfo.IsChannel)
            {
                if (inputInfo.IsChannelRadio)
                {
                    builder.AppendFormat(@"
所属栏目：</td><td>{0}</td>
<td>&nbsp;", this.GetChannelList());
                }
                else
                {
                    builder.AppendFormat("所属栏目：{0}&nbsp;", this.GetChannelList());
                }
            }

            if (inputInfo.IsDate)
            {
                DropDownList ddl = new DropDownList();
                ddl.ID = "date";
                ListItem listItem = new ListItem("全部", "0");
                ddl.Items.Add(listItem);
                listItem = new ListItem("1天内", "1");
                ddl.Items.Add(listItem);
                listItem = new ListItem("1周内", "7");
                ddl.Items.Add(listItem);
                listItem = new ListItem("1个月内", "30");
                ddl.Items.Add(listItem);
                listItem = new ListItem("1年内", "365");
                ddl.Items.Add(listItem);

                builder.AppendFormat("日期：{0}&nbsp;", ControlUtils.GetControlRenderHtml(ddl));
            }

            builder.AppendFormat(@"</td>
<td><input type=""button"" onclick=""document.getElementById('{0}').submit();return false;"" value="" 搜 索 "" />
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
	}
}
