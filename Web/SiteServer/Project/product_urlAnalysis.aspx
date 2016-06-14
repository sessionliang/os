<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundUrlAnalysis" %>

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
    <h3 class="popover-title">活动统计分析</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
    <tr align="Center">
      <td width="16%">今日活动网站</td><td width="84%" align="left"><asp:Literal ID="ltlActiveCount" runat="server"></asp:Literal> 个</td>
    </tr>
      <tr align="Center">
      <td>今日新增网站</td><td align="left"><asp:Literal ID="ltlNewCount" runat="server"></asp:Literal> 个</td>
    </tr>
      <tr align="Center">
      <td>共有网站</td><td align="left"><asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal> 个</td>
    </tr>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">产品使用统计</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr align="Center">
          <td width="16%">SiteServer CMS</td><td width="84%" align="left"><asp:Literal ID="ltlProductCMS" runat="server"></asp:Literal> 个</td>
        </tr>
        <tr align="Center">
          <td width="16%">SiteServer WCM</td><td width="84%" align="left"><asp:Literal ID="ltlProductWCM" runat="server"></asp:Literal> 个</td>
        </tr>
        <tr align="Center">
          <td>SiteServer BBS</td><td align="left"><asp:Literal ID="ltlProductBBS" runat="server"></asp:Literal> 个</td>
        </tr>
          <tr align="Center">
          <td>SiteServer ASK</td><td align="left"><asp:Literal ID="ltlProductASK" runat="server"></asp:Literal> 个</td>
        </tr>

      <tr align="Center">
          <td>SiteServer Space</td><td align="left"><asp:Literal ID="ltlProductSpace" runat="server"></asp:Literal> 个</td>
        </tr>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">版本使用统计</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">数据库使用统计</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <asp:Literal ID="ltlDatabase" runat="server"></asp:Literal>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>