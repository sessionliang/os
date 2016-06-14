<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.CreditEdit" %>

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

  <table class="table table-noborder table-hover">
    <tr>
      <td>积分代号：</td>
      <td><asp:Literal ID="ltlCreditID" runat="server"></asp:Literal></td>
    </tr>
    <tr>
      <td>积分名称：</td>
      <td><asp:TextBox  Width="180" id="txtCreditName" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtCreditName"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>积分单位：</td>
      <td><asp:TextBox  Width="180" id="txtCreditUnit" runat="server"/></td>
    </tr>
    <tr>
      <td>初始值：</td>
      <td>
        <asp:TextBox  Width="180" id="txtInitial" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtInitial"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>是否启用：</td>
      <td>
        <asp:RadioButtonList ID="rblIsUsing" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
      </td>
    </tr>
  </table>
  
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->