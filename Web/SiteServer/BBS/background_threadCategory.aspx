<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundThreadCategory" enableViewState = "false" %>

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
        <td class="center">分类名称</td>
        <td class="center" style="width:100px;">所属版块</td> 
        <td class="center" style="width:30px;">上升</td>
        <td class="center" style="width:30px;">下降</td>
        <td class="center" style="width:50px;">&nbsp;</td>
        <td class="center" style="width:30px;"></td>
        <td width="20">
            <input onclick="_checkFormAll(this.checked)" type="checkbox" />
        </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <td >
                <asp:Literal ID="ltlCategoryName" runat="server"></asp:Literal>
            </td>
            <td>
                <nobr><asp:Literal ID="ltlForumName" runat="server"></asp:Literal></nobr>
            </td> 
            <td class="center" style="width:30px;">
                <asp:Literal ID="ltlUpLink" runat="server"></asp:Literal>
            </td>
            <td class="center" style="width:30px;">
                <asp:Literal ID="ltlDownLink" runat="server"></asp:Literal>
            </td>
            <td class="center" style="width:50px;">
                <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
            </td>
            <td class="center" style="width:30px;">
                <asp:Literal ID="ltlDeleteLink" runat="server"></asp:Literal>
            </td>
            <td class="center" style="width:20px;">
                <asp:Literal ID="ltlCheckBox" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddCategory" Text="添 加" runat="server" />
    <asp:Button class="btn" id="Delete" Text="删 除" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->