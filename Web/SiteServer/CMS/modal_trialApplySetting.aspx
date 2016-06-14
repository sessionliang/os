<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.TrialApplySetting" Trace="false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
                <td>试用开始时间：</td>
                <td>
                    <asp:TextBox runat="server" ID="tbBeginDate" OnFocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'});"></asp:TextBox>
                     <asp:RequiredFieldValidator ControlToValidate="tbBeginDate" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                </td>
            </tr>
            <tr>
                <td>试用结束时间：</td>
                <td>
                    <asp:TextBox runat="server" ID="tbEndDate" OnFocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'});"></asp:TextBox></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
