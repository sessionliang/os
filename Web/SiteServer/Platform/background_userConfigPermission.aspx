<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserConfigPermission" %>

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
  <bairong:alerts text="此操作将改变用户中心权限设置，请谨慎使用" runat="server" />

  <asp:Repeater ID="rptModules" runat="server">
    <itemtemplate>

      <div class="popover popover-static">
      <h3 class="popover-title"><asp:Literal ID="ltlModuleName" runat="server"></asp:Literal> 权限设置</h3>
      <div class="popover-content">

        <div class="alert alert-info">
          <asp:Literal id="ltlTips" runat="server" />
        </div>
        
        <table class="table table-bordered table-hover">
          <tr class="info thead">
            <td width="200">权限标识</td>
            <td>权限名称</td>
            <td width="80">&nbsp;</td>
            <td width="80">&nbsp;</td>
          </tr>
          <asp:Repeater ID="rptPermissions" runat="server">
            <itemtemplate>
                <tr>
                  <td><asp:Literal ID="ltlName" runat="server"></asp:Literal></td>
                  <td><asp:Literal ID="ltlText" runat="server"></asp:Literal></td>
                  <td class="center">
                    <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                  </td>
                  <td class="center">
                    <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
                  </td>
                </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>
      
        <hr />
        <table class="table noborder">
          <tr>
            <td class="center">
              <asp:Button ID="btnSubmit" class="btn btn-success" Text="添加权限" runat="server"/>
            </td>
          </tr>
        </table>
      
        </div>
      </div>

    </itemtemplate>
  </asp:Repeater>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->