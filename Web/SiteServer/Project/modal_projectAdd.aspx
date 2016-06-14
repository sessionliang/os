<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ProjectAdd" Trace="false"%>

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
      <td width="120">项目名称：</td>
      <td><asp:TextBox id="tbProjectName" class="input-xlarge" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbProjectName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbProjectName" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
      <td width="120">项目类型：</td>
      <td>
        <asp:DropDownList ID="ddlProjectType" runat="server">
          <asp:ListItem value="" text="请选择"></asp:ListItem>
        </asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td>客户经理（AM）：</td>
      <td><asp:DropDownList ID="ddlUserNameAM" runat="server">
        <asp:ListItem value="" text="请选择"></asp:ListItem>
      </asp:DropDownList></td>
      <td width="120">项目经理（PM）：</td>
      <td><asp:DropDownList ID="ddlUserNamePM" runat="server">
        <asp:ListItem value="" text="请选择"></asp:ListItem>
      </asp:DropDownList></td>
    </tr>
    <tr>
      <td>项目组成人员：</td>
      <td colspan="3"><asp:CheckBoxList ID="cblUserNameCollection" repeatDirection="Horizontal" repeatColumns="5" class="checkboxlist" runat="server"></asp:CheckBoxList></td>
    </tr>
    <tr>
      <td>项目备注：</td>
      <td colspan="3"><asp:TextBox id="tbDescription" style="height:50px;width:90%" TextMode="MultiLine" runat="server" /></td>
    </tr>
    <tr>
      <td>项目类型：</td>
      <td colspan="3"><asp:RadioButtonList ID="rblIsContract" AutoPostBack="true" OnSelectedIndexChanged="rblIsContract_SelectedIndexChanged" class="radiobuttonlist" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phContract" runat="server" Visible="true">
    <tr>
      <td width="120">合同号：</td>
      <td colspan="3"><asp:TextBox id="tbContractNO" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbContractNO" errorMessage=" *" foreColor="red" display="Dynamic"
            runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbContractNO"
            ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td width="120">合同金额：</td>
      <td><asp:TextBox id="tbAmountTotal" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbAmountTotal" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAmountTotal" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      <td width="120">返款金额：</td>
      <td><asp:TextBox id="tbAmountCashBack" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbAmountCashBack" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAmountCashBack" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    </asp:PlaceHolder>
    <tr>
      <td>启动时间：</td>
      <td><bairong:DateTimeTextBox ID="tbAddDate" now="true" runat="server"></bairong:DateTimeTextBox></td>
      <td>项目状态：</td>
      <td><asp:DropDownList ID="ddlIsClosed" runat="server"></asp:DropDownList></td>
    </tr>
  </table>
  
</form>
</body>
</html>
