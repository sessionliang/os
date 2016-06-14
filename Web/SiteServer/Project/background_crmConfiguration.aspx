<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundCRMConfiguration" %>

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

  <div class="popover popover-static">
    <h3 class="popover-title">客户管理配置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="160">客户管理参与部门：</td>
          <td>
            <asp:ListBox ID="lbDepartmentID" SelectionMode="Multiple" rows="8" runat="server"></asp:ListBox>
          </td>
        </tr>
        <tr>
          <td width="160">工单管理参与部门：</td>
          <td>
            <asp:ListBox ID="lbRequestDepartmentID" SelectionMode="Multiple" rows="8" runat="server"></asp:ListBox>
          </td>
        </tr>
        <tr>
          <td width="160">工单问题类型：</td>
          <td>
            <asp:TextBox ID="tbRequestType" runat="server"></asp:TextBox>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
