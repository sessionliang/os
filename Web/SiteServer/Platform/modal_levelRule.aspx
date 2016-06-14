<%@ Page Language="C#" ValidateRequest="false" Inherits="BaiRong.BackgroundPages.Modal.LevelRule" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <table class="table table-noborder table-hover">
            <tr>
                <td width="120">动作：</td>
                <td>
                    <asp:Literal ID="ltlRuleName" runat="server"></asp:Literal></td>
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
                        <asp:TextBox Width="80" ID="PeriodCount" runat="server" />
                        <asp:RequiredFieldValidator
                            ControlToValidate="PeriodCount"
                            ErrorMessage=" *" ForeColor="red"
                            Display="Dynamic"
                            runat="server" />
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td>奖励次数：</td>
                <td>
                    <asp:TextBox Width="80" ID="MaxNum" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="MaxNum"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                    <span class="gray">(0代表不限制次数)</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlNameCreditNum" runat="server"></asp:Literal>：</td>
                <td>
                    <asp:TextBox Width="80" ID="CreditNum" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="CreditNum"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlNameCashNum" runat="server"></asp:Literal>：</td>
                <td>
                    <asp:TextBox Width="80" ID="CashNum" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="CashNum"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
