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
using SiteServer.STL.Parser.StlElement;

namespace SiteServer.STL.StlTemplate
{
	public class CommentsTemplate
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleInfo tagStyleInfo;
        private TagStyleCommentsInfo commentsInfo;

        public CommentsTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo, TagStyleCommentsInfo commentsInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.tagStyleInfo = tagStyleInfo;
            this.commentsInfo = commentsInfo;
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

            string linkToAll = string.Empty;
            if (this.commentsInfo.IsLinkToAll)
            {
                linkToAll = @"<a href=""{Stl.SiteUrl}/utils/comments.html?channelID={Channel.ChannelID}&contentID={Content.ContentID}"">[ �鿴ȫ�� ]</a>";
            }
            StringBuilder itemBuilder = new StringBuilder(@"<SPAN style=""float: right;""> ");
            if (this.commentsInfo.IsReference)
            {
                itemBuilder.AppendFormat(@"<a href=""javascript:;"" onclick=""{{Comment.Reference}}"">����</a>&nbsp;&nbsp; ");
            }
            if (this.commentsInfo.IsDigg)
            {
                itemBuilder.Append(@"<a title=""֧��һ��"" href=""javascript:;"" onclick=""{Comment.DiggGood}"">֧��[<stl:comment type=""Good""></stl:comment>]</a>&nbsp;&nbsp; <a title=""�ҷ���"" href=""javascript:;"" onclick=""{Comment.DiggBad}"">����[<stl:comment type=""Bad""></stl:comment>]</a>&nbsp;&nbsp; ");
                //itemBuilder.Append(@"<a href=""javascript:;"" onclick=""{Comment.Reply}"">�ظ�</a>&nbsp;&nbsp; ");
            }
            itemBuilder.AppendFormat(@"<span style=""color:#999""> <stl:comment type=""Floor""></stl:comment> ¥</span> </SPAN> <SPAN style=""color:#999"">{0}���ѣ�<stl:comment type=""UserName""></stl:comment> ", commentsInfo.IsLocation ? @"<stl:comment type=""Location""></stl:comment>" : string.Empty);
            if (this.commentsInfo.IsIPAddress)
            {
                itemBuilder.Append(@"(<stl:comment type=""IPAddress""></stl:comment>)");
            }
            itemBuilder.Append(@" �� <stl:comment type=""AddDate""></stl:comment> ���۵���</SPAN>");

            if (this.commentsInfo.IsPage)
            {
                builder.AppendFormat(@"
<div style=""border: #dadcdd 1px solid;background: #fff;"">
  <stl:dynamic>
    <div style=""line-height: 30px;padding-left: 15px;padding-right: 15px;height: 30px;font-size: 14px;font-weight: bold; color:#666""> <SPAN style=""float: right;color: #949494;font-size: 12px;font-weight: normal;"">����������<B style=""color: #e8581f;""><stl:count type=""Comments""></stl:count> </B>�� {0}</SPAN>�������� </div>
  <stl:pageComments pageNum=""{1}"">
  <div style=""border-bottom: #e6e6e6 1px solid;padding-bottom: 10px;line-height: 25px;margin: 15px 20px;padding-left: 5px;"">
    <div> {2}
      <div style=""clear:both""></div>
    </div>
    <div><stl:comment type=""Content""></stl:comment></div>
  </div>
  </stl:pageComments>
  <table width=""100%"" cellspacing=""3"" align=""center"">
    <tr>
      <td width='8'></td>
      <td><stl:pageItems>
        <table cellpadding=""0"" width=""90%"" cellspacing=""0"" height=""40"" align=""center"">
          <tr>
            <td align=""left""><stl:pageItem type=""FirstPage"" text=""��ҳ""></stl:pageItem>&nbsp;|&nbsp;<stl:pageItem type=""PreviousPage"" text=""��һҳ""></stl:pageItem>&nbsp;|&nbsp;<stl:pageItem type=""NextPage"" text=""��һҳ""></stl:pageItem>&nbsp;|&nbsp;<stl:pageItem type=""LastPage"" text=""ĩҳ""></stl:pageItem></td>
            <td align=""right""><stl:pageItem type=""CurrentPageIndex"" text=""��ǰҳ��""></stl:pageItem>/<stl:pageItem type=""TotalPageNum""></stl:pageItem>&nbsp;&nbsp;<stl:pageItem type=""PageNavigation""></stl:pageItem>&nbsp;&nbsp;<stl:pageItem type=""PageSelect""></stl:pageItem></td>
          </tr>
        </table>
        </stl:pageItems></td>
      <td width='8'></td>
    </tr>
  </table>
  </stl:dynamic>
</div>", linkToAll, this.commentsInfo.PageNum, itemBuilder.ToString());
            }
            else
            {
                builder.AppendFormat(@"
<div style=""border: #dadcdd 1px solid;background: #fff;"">
  <div style=""line-height: 30px;padding-left: 15px;padding-right: 15px;height: 30px;font-size: 14px;font-weight: bold; color:#666""> <SPAN style=""float: right;color: #949494;font-size: 12px;font-weight: normal;"">����������<B style=""color: #e8581f;""><stl:count type=""Comments""></stl:count> </B>�� {0}</SPAN>�������� </div>
  <stl:comments totalNum=""{1}"">
  <div style=""border-bottom: #e6e6e6 1px solid;padding-bottom: 10px;line-height: 25px;margin: 15px 20px;padding-left: 5px;"">
    <div> {2}
      <div style=""clear:both""></div>
    </div>
    <div><stl:comment type=""Content""></stl:comment></div>
  </div>
  </stl:comments>
</div>", linkToAll, commentsInfo.TotalNum, itemBuilder.ToString());
            }

            return builder.ToString();
        }
	}
}
