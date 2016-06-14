<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.CreditCalculate" %>

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
    </tr>
    <tr>
      <td><asp:Literal ID="ltlNamePrestige" runat="server"></asp:Literal>(Prestige)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtPrestige" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtPrestige"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>
        <asp:Literal ID="ltlNameContribution" runat="server"></asp:Literal>(Contribution)：
      </td>
      <td>
        × <asp:TextBox  Width="60" id="txtContribution" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtContribution"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td><asp:Literal ID="ltlNameCurrency" runat="server"></asp:Literal>(Currency)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtCurrency" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtCurrency"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <asp:PlaceHolder ID="phExtCredit1" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit1" runat="server"></asp:Literal>(ExtCredit1)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtExtCredit1" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtExtCredit1"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phExtCredit2" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit2" runat="server"></asp:Literal>(ExtCredit2)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtExtCredit2" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtExtCredit2"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phExtCredit3" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit3" runat="server"></asp:Literal>(ExtCredit3)：</td>
      <td>
        × <asp:TextBox  Width="60" id="txtExtCredit3" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtExtCredit3"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->