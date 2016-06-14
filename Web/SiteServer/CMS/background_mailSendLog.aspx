<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMailSendLog" %>

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
          时间：从
          <bairong:DateTimeTextBox id="DateFrom" class="input-small" runat="server" />
          &nbsp;到&nbsp;
          <bairong:DateTimeTextBox id="DateTo" class="input-small" runat="server" />
          关键字：
          <asp:TextBox ID="Keyword"
            MaxLength="500"
            size="37"
            runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>推荐内容标题</td>
      <td>邮箱地址</td>
      <td>IP地址</td>
      <td width="150">日期</td>
      <td width="20" class="center">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td>
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlMail" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIPAddress" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" id="Delete" Text="删 除" runat="server" />
    <asp:Button class="btn" id="DeleteAll" Text="删除全部" runat="server" />
    <asp:Literal ID="ltlState" runat="server"></asp:Literal>
    <asp:Button class="btn" ID="Setting" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->