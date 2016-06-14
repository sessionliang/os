<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundRight" %>

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

  <asp:Literal id="ltlQQ" runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <asp:Literal ID="ltlWelcome" runat="server"></asp:Literal>。
        </td>
      </tr>
    </table>
  </div>

  <div class="popover popover-static">
  <h3 class="popover-title"><lan>系统信息</lan></h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="150"><lan>当前版本</lan>：</td>
        <td>
        	<asp:Literal ID="ltlVersionInfo" runat="server" />
        </td>
      </tr>
      <tr>
        <td><lan>最近升级时间</lan>：</td>
        <td>
        	<asp:Literal ID="ltlUpdateDate" runat="server"></asp:Literal>
        </td>
      </tr>
      <tr>
        <td><lan>上次登录时间</lan>：</td>
        <td>
        	<asp:Literal ID="ltlLastLoginDate" runat="server"></asp:Literal>
        </td>
      </tr>
    </table>
  
    </div>
  </div>

  <asp:PlaceHolder id="phCheck" runat="server">
    <div class="popover popover-static">
      <h3 class="popover-title">待审核内容</h3>
      <div class="popover-content">
      
        <table class="table table-bordered table-hover">
          <asp:Literal id="ltlCheckHtml" runat="server" />
        </table>
    
      </div>
    </div>
  </asp:PlaceHolder>

</form>
</body>
</html>
