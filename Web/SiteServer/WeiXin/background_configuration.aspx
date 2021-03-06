<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundConfiguration" %>
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
  <bairong:alerts runat="server"></bairong:alerts>

  <script type="text/javascript" src="js/jquery.zclip.min.js"></script>

  <div class="popover popover-static">
    <h3 class="popover-title">站点信息</h3>
    <div class="popover-content">
    
      <table class="table noborder">
        <tr>
          <td width="160">绑定域名：</td>
          <td>
            <asp:Literal ID="ltlIsDomain" runat="server"></asp:Literal>
            <asp:PlaceHolder id="phDomain" runat="server">
                <asp:TextBox id="tbPublishmentSystemUrl" Columns="40" MaxLength="200" style="ime-mode:disabled;" runat="server" />
                <asp:RequiredFieldValidator ControlToValidate="tbPublishmentSystemUrl" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server"/>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPublishmentSystemUrl" ValidationExpression="[^']+" ErrorMessage=" *" foreColor="red" Display="Dynamic" />
                <br>
                <span class="gray">在此设置需要绑定的域名</span>
            </asp:PlaceHolder>
          </td>
        </tr>
        <tr>
          <td width="160">自定义版权：</td>
          <td>
            <asp:Literal ID="ltlIsPoweredBy" runat="server"></asp:Literal>
            <asp:PlaceHolder id="phPoweredBy" runat="server">
                <asp:TextBox id="tbPoweredBy" runat="server" />
                <br>
                <span class="gray">在此设置自定义版权信息，不设置页面将不显示版权信息</span>
            </asp:PlaceHolder>
          </td>
        </tr>
        <tr>
          <td>首页地址：</td>
          <td>
            <asp:Literal ID="ltlPublishmentSystemUrl" runat="server"></asp:Literal>
          </td>
        </tr>
        <tr>
          <td>智能分流：</td>
          <td>
            请将以下代码复制到您PC网站&lt;/head&gt;前,这样手机访问PC网站的用户将能够自动跳转到微官网
            <hr />
            <asp:Literal ID="ltlRedirectJs" runat="server"></asp:Literal>
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
<!-- check for 3.6 html permissions -->