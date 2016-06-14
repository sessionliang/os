<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.OrderSetting" Trace="false"%>

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
<bairong:alerts runat="server" text="在此设置订单状态，如状态未更改，请选择保持不变"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td class="center" width="120">订单状态：</td>
      <td><asp:DropDownList ID="ddlOrderStatus" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center">支付状态：</td>
      <td><asp:DropDownList ID="ddlPaymentStatus" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center">发货状态：</td>
      <td><asp:DropDownList ID="ddlShipmentStatus" runat="server"></asp:DropDownList></td>
    </tr>
  </table>

</form>
</body>
</html>
