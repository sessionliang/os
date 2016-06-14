<%@ Page Language="C#" validateRequest="false" Inherits="BaiRong.BackgroundPages.Modal.UserLevelCalculate" %>

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
    <%--<tr>
      <td>发帖数(PostCount)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtPostCount" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtPostCount"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>精华帖数(PostDigestCount)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtPostDigestCount" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtPostDigestCount"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>--%>
    <tr>
      <td><asp:Literal ID="ltlNameCreditNum" runat="server"></asp:Literal>(CreditNum)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtCreditNum" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtCreditNum"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>
        <asp:Literal ID="ltlNameCashNum" runat="server"></asp:Literal>(CashNum)：
      </td>
      <td>
        × <asp:TextBox  Width="60" id="txtCashNum" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtCashNum"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr> 
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->