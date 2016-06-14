<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundFilePathRule" enableViewState = "false" %>

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

  <asp:Literal ID="ltlScript" runat="server"></asp:Literal>

  <div class="popover popover-static">
    <h3 class="popover-title">默认命名规则</h3>
    <div class="popover-content">
    
    <table class="table table-bordered table-hover">
      <tr class="info thead">
        <td align="Left">板块命名规则</td>
        <td class="center" style="width:70px;">&nbsp;</td>
      </tr>
      <tr>
        <td><asp:Literal ID="ltlFilePathRuleForum" runat="server"></asp:Literal></td>
        <td class="center" style="width:70px;"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
      </tr>
    </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">页面命名规则</h3>
    <div class="popover-content">
    
    <table class="table table-bordered table-hover">
      <tr class="info thead">
        <td>版块名称</td>
        <td>访问地址</td>
        <td class="center" width="70">&nbsp;</td>
      </tr>
      <asp:Repeater ID="rptContents" runat="server">
        <itemtemplate>
          <tr treeItemLevel="<%# GetTreeItemLevel((int)Container.DataItem) %>" >
            <td><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
            <td>
              <asp:Literal ID="ltlFilePath" runat="server"></asp:Literal>
            </td>
            <td class="center"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
          </tr>
        </itemtemplate>
      </asp:Repeater>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->