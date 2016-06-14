<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundThread" %>

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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          用户名：<asp:TextBox ID="txtUserName" MaxLength="50" size="30" runat="server" />
          标题：<asp:TextBox ID="txtTitle" MaxLength="255" Size="45" runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          板块：<asp:DropDownList ID="ddlForum" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          发布日期：
          从 <bairong:DateTimeTextBox ID="DateFrom" class="input-small" runat="server" /> 
          至 
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" runat="server" />
          <asp:Button class="btn" OnClick="Search_OnClick" ID="btnSearch" Text="搜 索" runat="server" />
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td class="center">
            编号
        </td>
        <td class="center">
            标题
        </td>
        <td class="center">
            版块
        </td>
        <td class="center">
            作者
        </td>
        <td class="center">
            回复
        </td>
        <td class="center">
            浏览
        </td>
        <td class="center">
            最后发表
        </td>
        <td width="60"></td>
      <td width="20">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <td class="center" style="width: 30px;">
                <asp:Literal ID="ltlThreadID" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlForumName" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlReplies" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlHits" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlLastDate" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlEdit" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlCheckBox" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnTranslate" Cssclass="btn" runat="server" Text="转 移" />
    <asp:Button ID="btnTop" Cssclass="btn" runat="server" Text="置 顶" />
    <asp:Button ID="btnDigest" Cssclass="btn" runat="server" Text="精 华" />
    <asp:Button ID="btnDelete" Cssclass="btn" runat="server" Text="删 除" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->