<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.PaymentAdd" Trace="false"%>

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
      <td>类型：</td>
      <td><asp:RadioButtonList ID="rblIsCashBack" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="SelectedIndexChanged" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><%=GetName()%>种类：</td>
      <td><asp:DropDownList ID="ddlPaymentType" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td width="120">次序：</td>
      <td><asp:TextBox id="tbPaymentOrder" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbPaymentOrder" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPaymentOrder" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><%=GetName()%>标准：</td>
      <td><asp:TextBox id="tbPremise" Columns="40" TextMode="MultiLine" Rows="4" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbPremise" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPremise" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
        <td>预计<%=GetName()%>日期：</td>
        <td><asp:TextBox id="tbExpectDate" Columns="25" MaxLength="50" runat="server" />
          </td>
      </tr>
    <tr>
      <td>预计<%=GetName()%>金额：</td>
      <td><asp:TextBox id="tbAmountExpect" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbAmountExpect" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAmountExpect" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    
    <tr>
      <td>是否开发票：</td>
      <td><asp:RadioButtonList ID="rblIsInvoice" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="SelectedIndexChanged" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phIsInvoice" Visible="false" runat="server">
      <tr>
        <td>发票号：</td>
        <td><asp:TextBox id="tbInvoiceNO" Columns="25" MaxLength="50" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbInvoiceNO" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbInvoiceNO" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td>发票日期：</td>
        <td><asp:TextBox id="tbInvoiceDate"  Columns="25" MaxLength="50" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbInvoiceDate" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbInvoiceDate" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
    </asp:PlaceHolder>
    
    <tr>
      <td>是否<%=GetName()%>：</td>
      <td><asp:RadioButtonList ID="rblIsPayment" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="SelectedIndexChanged" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phIsPayment" Visible="false" runat="server">
      <tr>
        <td>实际<%=GetName()%>金额：</td>
        <td><asp:TextBox id="tbAmountPaid" Columns="25" MaxLength="50" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbAmountPaid" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAmountPaid" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td>实际<%=GetName()%>日期：</td>
        <td><asp:TextBox id="tbPaymentDate"  Columns="25" MaxLength="50" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbPaymentDate" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPaymentDate" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
