<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundReport" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td>编号</td>
        <td>内容</td>
        <td>版块</td>
        <td>帖子</td>
        <td>投诉人</td>
        <td width="120">投诉日期</td>
        <td width="70"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
            <tr>
                <td class="center" style="width: 30px;"> 
                    <asp:Literal ID="ltlReportID" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
                </td>
                <td class="center" >
                    <asp:Literal ID="ltlForumName" runat="server"></asp:Literal>
                </td>
                <td class="center" >
                    <asp:Literal ID="ltlPost" runat="server"></asp:Literal>
                </td>
                <td class="center">
                    <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                </td>
                <td class="center">
                    <asp:Literal ID="ltlDatetime" runat="server"></asp:Literal>
                </td>
                <td class="center">
                    <asp:Literal ID="ltlDel" runat="server"></asp:Literal>
                </td>
            </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->