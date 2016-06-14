<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundApplyAdd" %>

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
    <h3 class="popover-title">新增办件</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr height="2">
          <td width="80"></td>
          <td></td>
          <td width="80"></td>
          <td></td>
          <td width="80"></td>
          <td></td>
        </tr>
        <tr>
          <td width="80" height="30" align="right"><nobr>主题：</nobr></td>
          <td height="30" colspan="5">
            <asp:TextBox ID="tbTitle" Width="450" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTitle" errorMessage=" *" foreColor="red" Display="Dynamic" runat="server" /></td>            </tr>
        <tr>
          <td height="30" align="right">优先级：</td>
          <td height="30">
            <asp:DropDownList ID="ddlPriority" runat="server">
              <asp:ListItem Text="低" Value="1"></asp:ListItem>
                <asp:ListItem Text="普通" Selected="true" Value="2"></asp:ListItem>
                <asp:ListItem Text="高" Value="3"></asp:ListItem>
            </asp:DropDownList></td>
          <td width="80" height="30" align="right">预计开始：</td>
          <td height="30">
            <bairong:DateTimeTextBox id="tbExpectedDate" now="true" Columns="20" runat="server" />
          </td>
          <td width="80" align="right">发起人：</td>
          <td>
            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
          </td>
        </tr>
        <tr>
          <td height="30" align="right">类型：</td>
          <td height="30">
            <asp:DropDownList ID="ddlTypeID" runat="server"></asp:DropDownList>
          </td>
          <td height="30" align="right">截止日期：</td>
          <td height="30">
            <bairong:DateTimeTextBox id="tbEndDate" Columns="20" runat="server" />
          </td>
          <td height="30" align="right">负责人：</td>
          <td height="30"><asp:DropDownList ID="ddlIsSelf" runat="server"></asp:DropDownList><span id="spanOthers" style="display:none"><asp:DropDownList ID="ddlDepartmentID" runat="server"></asp:DropDownList></span></td>
        </tr>
        <tr>
          <td height="30" align="right">备注：</td>
          <td height="30" colspan="5">
            <asp:TextBox ID="tbSummary" Rows="2" style="width:100%;" runat="server" TextMode="MultiLine"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td height="30" align="right">内容：</td>
          <td height="30" colspan="5"><bairong:BREditor id="breContent" runat="server"></bairong:BREditor></td>
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