<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundLicense" %>

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

  <asp:Repeater ID="rptContents" runat="server">
    <itemtemplate>

      <div class="popover popover-static">
      <h3 class="popover-title">
        <asp:Literal ID="ltlProductName" runat="server"></asp:Literal> 许可证
      </h3>
      <div class="popover-content">
        
        <table class="table noborder table-hover">
          <tr>
            <td>
              <asp:Literal ID="ltlLicense" runat="server"></asp:Literal>
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