<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.UserTableStyleAdd" Trace="false" %>

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
      <td width="120">字段名称：</td>
      <td colspan="3"><asp:TextBox Columns="25" MaxLength="50" id="tbAttributeName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbAttributeName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAttributeName"
						ValidationExpression="[^',\s]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>显示名称：</td>
      <td colspan="3"><asp:TextBox Columns="25" MaxLength="50" id="DisplayName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="DisplayName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="DisplayName"
							ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>显示帮助提示：</td>
      <td colspan="3"><asp:TextBox Columns="60" id="HelpText" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="HelpText"
							ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>是否启用：</td>
      <td><asp:RadioButtonList id="IsVisible" RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="是" Selected="True" />
          <asp:ListItem Text="否" />
        </asp:RadioButtonList></td>
      <td width="120">是否单行显示：</td>
      <td><asp:RadioButtonList id="IsSingleLine" RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="是" />
          <asp:ListItem Text="否" Selected="True" />
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>表单提交类型：</td>
      <td colspan="3"><asp:DropDownList ID="InputType" OnSelectedIndexChanged="ReFresh" AutoPostBack="true" runat="server"></asp:DropDownList></td>
    </tr>
    <tr id="RowEditorType" runat="server">
      <td>编辑器类型：</td>
      <td colspan="3"><asp:DropDownList id="EditorType" runat="server" /></td>
    </tr>
    <tr id="RowHeightAndWidth" runat="server">
      <td>显示宽度：</td>
      <td>
        <asp:TextBox class="input-mini" MaxLength="50" Text="0" id="Width" runat="server" /> px
        <asp:RegularExpressionValidator
					ControlToValidate="Width"
					ValidationExpression="\d+"
					Display="Dynamic"
					errorMessage=" *" foreColor="red" 
					runat="server"/>
        <span class="gray">（0代表默认）</span>
      </td>
      <td>显示高度：</td>
      <td>
        <asp:TextBox class="input-mini" MaxLength="50" Text="0" id="Height" runat="server" />
        px
        <asp:RegularExpressionValidator
					ControlToValidate="Height"
					ValidationExpression="\d+"
					Display="Dynamic"
					errorMessage=" *" foreColor="red" 
					runat="server"/>
        <span class="gray">（0代表默认）</span>
      </td>
    </tr>
    <tr id="RowDefaultValue" runat="server">
      <td>默认显示值：</td>
      <td colspan="3">
        <asp:TextBox Columns="60" TextMode="MultiLine" id="DefaultValue" runat="server" />
        <span id="DateTip" runat="server"><br>
        {Current}代表当前日期/日期时间</span>
        <asp:RegularExpressionValidator runat="server" ControlToValidate="DefaultValue" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr id="RowRepeat" runat="server">
      <td>排列方向：</td>
      <td>
        <asp:DropDownList id="IsHorizontal" class="input-small" runat="server">
          <asp:ListItem Text="水平" Selected="True"/>
          <asp:ListItem Text="垂直"  />
        </asp:DropDownList>
      </td>
      <td>列数：</td>
      <td>
        <asp:TextBox class="input-mini" MaxLength="50" Text="0" id="Columns" runat="server" />
        <asp:RegularExpressionValidator
					ControlToValidate="Columns"
					ValidationExpression="\d+"
					Display="Dynamic"
					errorMessage=" *" foreColor="red" 
					runat="server"/>
        <span class="gray">（0代表未设置此属性）</span>
      </td>
    </tr>
    <tr id="RowItemCount" runat="server">
      <td>设置选项数目：</td>
      <td colspan="3">
        <asp:TextBox class="input-mini" Text="2" id="ItemCount" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="ItemCount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:Button class="btn" style="margin-bottom:0px;" id="SetCount" text="设 置" onclick="SetCount_OnClick" CausesValidation="false" runat="server" />
        <asp:RegularExpressionValidator ControlToValidate="ItemCount" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server" />
      </td>
    </tr>
    <tr id="RowSetItems" runat="server">
      <td colspan="4" class="center"><asp:Repeater ID="MyRepeater" runat="server">
          <itemtemplate>
            <table width="100%" border="0" cellspacing="2" cellpadding="2">
              <tr>
                <td class="center" style="width:20px;"><strong><%# Container.ItemIndex + 1 %></strong></td>
                <td><table width="100%" border="0" cellspacing="3" cellpadding="3">
                    <tr>
                      <td width="120"><bairong:help HelpText="设置选项的标题。" Text="选项标题：" runat="server" ></bairong:help></td>
                      <td colspan="3"><asp:TextBox ID="ItemTitle" Columns="40" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemTitle") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="ItemTitle" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
                    </tr>
                    <tr>
                      <td width="120"><bairong:help HelpText="设置选项的值。" Text="选项值：" runat="server" ></bairong:help></td>
                      <td colspan="3"><asp:TextBox ID="ItemValue" Columns="40" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemValue") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="ItemValue" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
                    </tr>
                    <tr>
                      <td width="120"><bairong:help HelpText="设置是否初始化时选定此项。" Text="初始化时选定：" runat="server" ></bairong:help></td>
                      <td colspan="3"><asp:CheckBox ID="IsSelected" runat="server" Checked="False" Text="选定"></asp:CheckBox></td>
                    </tr>
                  </table></td>
              </tr>
            </table>
          </itemtemplate>
        </asp:Repeater></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->