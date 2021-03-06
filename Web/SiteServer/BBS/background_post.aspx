﻿<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundPost" %>

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
          发布日期：
          从 <bairong:DateTimeTextBox ID="DateFrom" class="input-small" runat="server" /> 至 
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" runat="server" />
          板块：
          <asp:DropDownList ID="ddlForum" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td>
          用户名：
          <asp:TextBox ID="txtUserName" MaxLength="50" size="30" runat="server" />
          标题：
          <asp:TextBox ID="txtTitle" MaxLength="255" Size="45" runat="server" />
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
            主题帖
        </td>
        <td class="center">
            已屏蔽
        </td>
        <td class="center">
            发布IP
        </td>
        <td class="center">
            发表时间
        </td>
        <td width="20">
            <input onclick="_checkFormAll(this.checked)" type="checkbox" />
        </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td class="center" style="width: 30px;">
                <asp:Literal ID="ltlPostID" runat="server"></asp:Literal>
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
                <asp:Literal ID="ltlIsThread" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlIsBanned" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlIP" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
            </td>
            <td class="center" width="20">
                <asp:Literal ID="ltlCheckBox" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnShield" Cssclass="btn" runat="server" Text="屏蔽帖子" />
    <asp:Button ID="btnUnShield" Cssclass="btn" runat="server" Text="解除屏蔽" />
    <asp:Button ID="btnDelete" Cssclass="btn" runat="server" Text="删 除" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->