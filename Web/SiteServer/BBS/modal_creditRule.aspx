<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.CreditRule" %>

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
      <td width="120">动作：</td>
      <td><asp:Literal ID="ltlRuleName" runat="server"></asp:Literal></td>
    </tr>
    <tr>
      <td>周期：</td>
      <td>
        <asp:RadioButtonList ID="PeriodType" OnSelectedIndexChanged="PeriodType_SelectedIndexChanged" AutoPostBack="true" RepeatColumns="2" runat="server"></asp:RadioButtonList>
      </td>
    </tr>
    <asp:PlaceHolder ID="phPeriodCount" runat="server">
    <tr>
      <td>间隔时间：</td>
      <td>
        <asp:TextBox  Width="80" id="PeriodCount" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="PeriodCount"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
      <td>奖励次数：</td>
      <td>
        <asp:TextBox  Width="80" id="MaxNum" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="MaxNum"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
        <span class="gray">(0代表不限制次数)</span>
      </td>
    </tr>
    <tr>
      <td><asp:Literal ID="ltlNamePrestige" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="Prestige" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="Prestige"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td><asp:Literal ID="ltlNameContribution" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="Contribution" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="Contribution"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td><asp:Literal ID="ltlNameCurrency" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="Currency" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="Currency"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <asp:PlaceHolder ID="phExtCredit1" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit1" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="ExtCredit1" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="ExtCredit1"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phExtCredit2" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit2" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="ExtCredit2" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="ExtCredit2"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phExtCredit3" runat="server">
    <tr>
      <td><asp:Literal ID="ltlNameExtCredit3" runat="server"></asp:Literal>：</td>
      <td>
        <asp:TextBox  Width="80" id="ExtCredit3" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="ExtCredit3"
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