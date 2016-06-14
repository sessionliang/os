<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.Modal.AuthSettingDomain" Trace="false"%>

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

  <table class="table table-noborder">
    <tr>
      <td width="120">是否绑定域名：</td>
      <td>
        <asp:DropDownList id="ddlIsDomain" onSelectedIndexChanged="ddlIsDomain_SelectedIndexChanged" autopostback="true" runat="server" />
      </td>
    </tr>
    <asp:PlaceHolder id="phDomain" runat="server">
      <tr>
        <td>绑定域名：</td>
        <td>
          <asp:TextBox id="tbDomain" runat="server" />
        </td>
      </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->