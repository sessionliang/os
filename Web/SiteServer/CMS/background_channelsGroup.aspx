<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundChannelsGroup" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <asp:Literal ID="ltlChannelGroupName" runat="server"></asp:Literal>
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td>栏目名</td>
        <td width="250">栏目索引</td>
        <td width="100">添加日期</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlItemChannelName" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlItemChannelIndex" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlItemAddDate" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" text="返 回" onclick="Return_OnClick" runat="server"/>
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->