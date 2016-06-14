<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundKeyword" %>

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
  <bairong:alerts runat="server"></bairong:alerts>
  
    <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          关键词类型：
          <asp:DropDownList ID="ddlKeywordType" runat="server"></asp:DropDownList>
          关键字：
          <asp:TextBox id="tbKeyword" MaxLength="500" runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" id="Search" text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered">
    <tr class="info thead">
      <td>所有关键词</td>
    </tr>
    <tr><td>
      <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal ID="ltlKeyword" runat="server"></asp:Literal>
      </itemtemplate>
    </asp:Repeater>
    </td></tr>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->