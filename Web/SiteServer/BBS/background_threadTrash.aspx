<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundThreadTrash" %>

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
        <td class="center">编号</td>
        <td class="center">标题</td>
        <td class="center">版块</td>
        <td class="center">作者</td>
        <td class="center">删除时间</td>
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
            <td class="center" >
                <asp:Literal ID="ltlForumName" runat="server"></asp:Literal>
            </td>
            <td class="center" >
                <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
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
    <asp:Button ID="btnRestore" Cssclass="btn" runat="server" Text="还 原"/>
    <asp:Button ID="btnAllRestore" Cssclass="btn" runat="server" Text="全部还原"/>
    <asp:Button ID="btnDelete" Cssclass="btn" runat="server" Text="删 除"/>
    <asp:Button ID="btnDeleteAll" Cssclass="btn" runat="server" Text="清空回收站" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->