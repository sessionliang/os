<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundProjectConfiguration" %>

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
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">项目参数配置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="140">项目类型：</td>
          <td colspan="3">
            <asp:TextBox ID="tbProjectType" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>办理时限：</td>
          <td colspan="3"><asp:TextBox id="tbApplyDateLimit" Columns="4" MaxLength="4" Style="text-align:right" Text="0" class="input-mini" runat="server"/>
            日
            <asp:RequiredFieldValidator
              ControlToValidate="tbApplyDateLimit"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>
            <asp:RegularExpressionValidator
              ControlToValidate="tbApplyDateLimit"
              ValidationExpression="\d+"
              ErrorMessage="办理时限只能是数字"
              Display="Dynamic"
              runat="server"/></td>
        </tr>
        <tr>
          <td>预警：</td>
          <td width="70">办理时限</td>
          <td width="140"><asp:RadioButtonList ID="rblApplyAlertDateIsAfter" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server">
            <asp:ListItem Text="前" Value="False" Selected="true"></asp:ListItem>
            <asp:ListItem Text="后" Value="True"></asp:ListItem>
          </asp:RadioButtonList></td>
          <td><asp:TextBox ID="tbApplyAlertDate" Columns="4" MaxLength="4" style="text-align:right" Text="0" class="input-mini" runat="server"/>        
  日
    <asp:RequiredFieldValidator
              ControlToValidate="tbApplyAlertDate"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>  
    <asp:RegularExpressionValidator
              ControlToValidate="tbApplyAlertDate"
              ValidationExpression="\d+"
              ErrorMessage="预警只能是数字"
              Display="Dynamic"
              runat="server"/></td>
        </tr>
        <tr>
          <td>黄牌：</td>
          <td colspan="3">预警后
          <asp:TextBox id="tbApplyYellowAlertDate" Columns="4" MaxLength="4" Style="text-align:right" Text="0" class="input-mini" runat="server"/>
            日
            <asp:RequiredFieldValidator
              ControlToValidate="tbApplyYellowAlertDate"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>
            <asp:RegularExpressionValidator
              ControlToValidate="tbApplyYellowAlertDate"
              ValidationExpression="\d+"
              ErrorMessage="黄牌只能是数字"
              Display="Dynamic"
              runat="server"/></td>
        </tr>
        <tr>
          <td>红牌：</td>
          <td colspan="3">黄牌后
          <asp:TextBox id="tbApplyRedAlertDate" Columns="4" MaxLength="4" Style="text-align:right" Text="0" class="input-mini" runat="server"/>
            日
            <asp:RequiredFieldValidator
              ControlToValidate="tbApplyRedAlertDate"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>
            <asp:RegularExpressionValidator
              ControlToValidate="tbApplyRedAlertDate"
              ValidationExpression="\d+"
              ErrorMessage="红牌只能是数字"
              Display="Dynamic"
              runat="server"/></td>
        </tr>
        <tr>
          <td>办件是否可删除：</td>
          <td colspan="3">
          <asp:RadioButtonList ID="rblApplyIsDeleteAllowed" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
              <asp:ListItem Text="否" Value="False"></asp:ListItem>
          </asp:RadioButtonList>
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
