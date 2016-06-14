<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundConfigurationOrder" %>

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

  <div class="popover popover-static">
    <h3 class="popover-title">订单配置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="120">订单寄送地区：</td>
          <td>
            <asp:DropDownList id="ddlIsLocationAll" runat="server"/>
          </td>
        </tr>
        <tr>
          <td>可开发票内容：</td>
          <td>
            <asp:TextBox id="tbOrderInvoiceContentCollection" class="input-xlarge" runat="server"/>
            <asp:RequiredFieldValidator
              ControlToValidate="tbOrderInvoiceContentCollection"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>
            <br>
            <span class="gray">请用“,”分隔多个发票内容</span>
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
<!-- check for 3.6.4 html permissions -->