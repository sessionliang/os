<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.AdminSelect" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table">
    <tr>
      <td style="border-top:none">

        <table class="table table-bordered table-hover">
          <tr class="info" treeItemLevel="2">
            <td><img align="absmiddle" style="cursor:pointer" onClick="displayChildren(this);" isAjax="false" isOpen="true" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;部门选择</td>
          </tr>
          <asp:Repeater ID="rptDepartment" runat="server">
            <itemtemplate>
              <asp:Literal id="ltlHtml" runat="server" />
            </itemtemplate>
          </asp:Repeater>
        </table>

      </td>
      <td style="border-top:none">

        <table class="table table-bordered table-hover">
          <tr class="info" treeItemLevel="2">
            <td><img align="absmiddle" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;<asp:Literal ID="ltlDepartment" runat="server"></asp:Literal></td>
          </tr>
          <asp:Repeater ID="rptUser" runat="server">
          	<itemtemplate>         
            <tr>
                <td>
                    <img align="absmiddle" src="/sitefiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" src="/sitefiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" src="/sitefiles/bairong/icons/menu/user.gif" />&nbsp;
                    <asp:Literal ID="ltlUrl" runat="server"></asp:Literal>
                </td>
            </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>

      </td>
    </tr>
  </table>
  
</form>
</body>
</html>