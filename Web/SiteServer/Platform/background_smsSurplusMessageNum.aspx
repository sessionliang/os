<%@ Page Language="C#" AutoEventWireup="true" Inherits="BaiRong.BackgroundPages.BackgroundSMSSurplusMessageNum" %>

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

  <asp:PlaceHolder id="phTotalCount" runat="server" visible="false">
    <div class="popover popover-static">
      <h3 class="popover-title">剩余短信条数</h3>
      <div class="popover-content">
        
        <table class="table table-noborder table-hover">
          <tr>
            <td>
                您的剩余短信条数为：<asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal>条
            </td>
          </tr>
        </table>
      
      </div>
    </div>
  </asp:PlaceHolder>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->