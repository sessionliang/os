<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserConfigMenu" %>

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
  <bairong:alerts text="此操作将改变用户中心菜单设置，请谨慎使用" runat="server" />

  <asp:Repeater ID="rptModules" runat="server">
    <itemtemplate>

      <div class="popover popover-static">
      <h3 class="popover-title"><asp:Literal ID="ltlModuleName" runat="server"></asp:Literal> 菜单设置</h3>
      <div class="popover-content">

        <div class="alert alert-info">
          <asp:Literal id="ltlTips" runat="server" />
        </div>
        
        <table class="table table-bordered table-hover" style="table-layout:fixed">
          <tr class="info thead">
            <td width="160">菜单名称</td>
            <td>链接</td>
            <td width="70">目标</td>
            <td>权限</td>
            <td width="40">&nbsp;</td>
            <td width="50">&nbsp;</td>
            <td width="40">&nbsp;</td>
          </tr>
          <asp:Repeater ID="rptMenues" runat="server">
            <itemtemplate>
              <tr>
                <td><asp:Literal ID="ltlText" runat="server"></asp:Literal></td>
                <td><asp:Literal ID="ltlHref" runat="server"></asp:Literal></td>
                <td><asp:Literal ID="ltlTarget" runat="server"></asp:Literal></td>
                <td style="word-break: break-all"><asp:Literal ID="ltlPermissions" runat="server"></asp:Literal></td>
                <td class="center">
                  <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                </td>
                <td class="center">
                  <asp:Literal ID="ltlAddUrl" runat="server"></asp:Literal>
                </td>
                <td class="center"><asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal></td>
              </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>
      
        </div>
      </div>

    </itemtemplate>
  </asp:Repeater>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->