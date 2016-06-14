<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundInvoiceAdd" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" enctype="multipart/form-data" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">

      <table class="table noborder table-hover">
        <tr>
          <td width="180">发票号：</td>
          <td><input name="SN" type="text" class="input-large" id="SN" value="<%=GetValue("SN")%>" /></td>
          <td width="140">业务种类：</td>
          <td><asp:Literal id="ltlInvoiceType" runat="server" /></td>
        </tr>
        <asp:PlaceHolder id="phSiteYun" visible="false" runat="server">
        <tr>
          <td width="180">订单ID：</td>
          <td><asp:Literal id="ltlOrderSN" runat="server" /></td>
          <td width="140">客户ID：</td>
          <td><asp:Literal id="ltlLoginName" runat="server" /></td>
        </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="phSiteServer" visible="false" runat="server">
        <tr>
          <td width="180">客户名称：</td>
          <td><asp:Literal id="ltlAccountName" runat="server" /></td>
          <td width="140">负责人：</td>
          <td><asp:Literal id="ltlChargeUserName" runat="server" /></td>
        </tr>
        </asp:PlaceHolder>
        <tr>
          <td align="right">发票类型：</td>
          <td>
            <select name="IsVAT" id="IsVAT">
              <option <%=GetSelected("IsVAT", "True")%> value="True">增值税发票</option>
              <option <%=GetSelected("IsVAT", "False", true)%> value="False">普通发票</option>
            </select>
          </td>
          <td align="right">发票金额：</td>
          <td>
            <input name="Amount" type="text" class="input-mini" id="Amount" value="<%=GetValue("Amount")%>" /> 元
          </td>
        </tr>
        <tr>
          <td align="right">发票抬头：</td>
          <td>
            <input name="InvoiceTitle" type="text" class="input-large" id="InvoiceTitle" value="<%=GetValue("InvoiceTitle")%>" />
          </td>
          <td align="right">发票收件人：</td>
          <td>
            <input name="InvoiceReceiver" type="text" class="input-large" id="InvoiceReceiver" value="<%=GetValue("InvoiceReceiver")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">发票联系电话：</td>
          <td>
            <input name="InvoiceTel" type="text" class="input-large" id="InvoiceTel" value="<%=GetValue("InvoiceTel")%>" />
          </td>
          <td align="right">发票邮寄地址：</td>
          <td>
            <input name="InvoiceAddress" type="text" class="input-large" id="InvoiceAddress" value="<%=GetValue("InvoiceAddress")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">纳税人识别号（增值税）：</td>
          <td>
            <input name="VATTaxpayerID" type="text" class="input-large" id="VATTaxpayerID" value="<%=GetValue("VATTaxpayerID")%>" />
          </td>
          <td align="right">开户银行（增值税）：</td>
          <td>
            <input name="VATBankName" type="text" class="input-large" id="VATBankName" value="<%=GetValue("VATBankName")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">开户账号（增值税）：</td>
          <td colspan="3">
            <input name="VATBankCardNo" type="text" class="input-large" id="VATBankCardNo" value="<%=GetValue("VATBankCardNo")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">备注：</td>
          <td colspan="3">
            <textarea name="Summary" type="text" style="width:90%;height:120px;" id="Summary"><%=GetValue("Summary")%></textarea>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" OnClick="Submit_OnClick" runat="server" />
            <asp:Button class="btn" id="Return" text="返 回" OnClick="Return_OnClick" CausesValidation="false" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->