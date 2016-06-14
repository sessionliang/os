<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundLicenseUpload" %>

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
  <h3 class="popover-title">更换许可证</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td colspan="2">
          请在下面表单中上传您的
          <strong>
          <asp:Literal ID="ltlProductName" runat="server"></asp:Literal>
          </strong>
          产品许可证文件。
          <br />
          <span>您可以将您的机器标识或者网站域名提供给我们的销售渠道以便获取正式授权。</span>
        </td>
      </tr>
      <tr>
        <td width="120">您的机器标识</td>
        <td><asp:Literal ID="ltlComputerID" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td>您的网站域名</td>
        <td><asp:Literal ID="ltlDomain" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td>许可证文件</td>
        <td>
          <input type="file" size="40" id="LicenseFile" runat="server">
          <asp:RequiredFieldValidator ControlToValidate="LicenseFile" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" OnClick="ButtonClick" runat="server" Text="上 传"></asp:Button>
          <input type="button" class="btn" onClick="location.href='background_license.aspx'" value="返 回">
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->