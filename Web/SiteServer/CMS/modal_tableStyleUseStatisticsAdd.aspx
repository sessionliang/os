<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.TableStyleUseStatisticsAdd" Trace="false" %>

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
                <td>
                    <bairong:Help HelpText="保存在数据库中的字段名称" Text="字段名称：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:TextBox Columns="25" MaxLength="50" ID="tbAttributeName" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbAttributeName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAttributeName"
                        ValidationExpression="[a-zA-Z0-9_]+" ErrorMessage="字段名称只允许包含字母、数字以及下划线" Display="Dynamic" /></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="显示在表单界面中的名称" Text="显示名称：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox Columns="25" MaxLength="50" ID="tbDisplayName" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbDisplayName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="tbDisplayName"
                        ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /></td>
                <td>
                    <bairong:Help HelpText="是否启用统计字段。" Text="是否启用统计：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblUseStatistics" RepeatDirection="Horizontal"  OnSelectedIndexChanged="rblUseStatistics_SelectedIndexChanged" AutoPostBack="true" class="noborder" runat="server">
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="显示在表单界面中的帮助提示信息" Text="显示帮助提示：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:TextBox Columns="60" ID="tbHelpText" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="tbHelpText"
                        ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="是否启用本样式。" Text="是否启用：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIsVisible" RepeatDirection="Horizontal" class="noborder" runat="server">
                        <asp:ListItem Text="是" Selected="True" />
                        <asp:ListItem Text="否" />
                    </asp:RadioButtonList></td>
                <td>
                    <bairong:Help HelpText="是否在表单中采用单行显示。" Text="是否单行显示：" runat="server"></bairong:Help>
                </td>
                <td width="30%">
                    <asp:RadioButtonList ID="rblIsSingleLine" RepeatDirection="Horizontal" class="noborder" runat="server">
                        <asp:ListItem Text="是" />
                        <asp:ListItem Text="否" Selected="True" />
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="在表单界面中此字段的表单提交类型。" Text="表单提交类型：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlInputType" OnSelectedIndexChanged="ReFresh" AutoPostBack="true" runat="server"></asp:DropDownList></td>
            </tr>
            <asp:PlaceHolder ID="phIsFormatString" Visible="false" runat="server">
                <tr>
                    <td>
                        <bairong:Help HelpText="是否可设置此字段的文字显示格式。" Text="可否设置格式：" runat="server"></bairong:Help>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rblIsFormatString" RepeatDirection="Horizontal" class="noborder" runat="server">
                            <asp:ListItem Text="可设置" />
                            <asp:ListItem Text="不可设置" Selected="True" />
                        </asp:RadioButtonList></td>
                </tr>
            </asp:PlaceHolder>
            <tr id="rowEditorType" runat="server">
                <td>
                    <bairong:Help HelpText="编辑器类型" Text="编辑器类型：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlEditorType" runat="server" /></td>
            </tr>
            <tr id="rowRelatedField" runat="server">
                <td>
                    <bairong:Help HelpText="联动字段" Text="联动字段：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRelatedFieldID" runat="server" /></td>
                <td>
                    <bairong:Help HelpText="显示方式" Text="显示方式：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRelatedFieldStyle" class="input-medium" runat="server" /></td>
            </tr>
            <tr id="rowHeightAndWidth" runat="server">
                <td>
                    <bairong:Help HelpText="显示宽度" Text="显示宽度：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox class="input-mini" MaxLength="50" Text="0" ID="tbWidth" runat="server" />
                    px
        <asp:RegularExpressionValidator
            ControlToValidate="tbWidth"
            ValidationExpression="\d+"
            Display="Dynamic"
            ErrorMessage=" *" ForeColor="red"
            runat="server" />
                    <span class="gray">（0代表默认）</span>
                </td>
                <td>
                    <bairong:Help HelpText="显示高度" Text="显示高度：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox class="input-mini" MaxLength="50" Text="0" ID="tbHeight" runat="server" />
                    px
        <asp:RegularExpressionValidator
            ControlToValidate="tbHeight"
            ValidationExpression="\d+"
            Display="Dynamic"
            ErrorMessage=" *" ForeColor="red"
            runat="server" />
                    <span class="gray">（0代表默认）</span>
                </td>
            </tr>
            <tr id="rowRepeat" runat="server">
                <td>
                    <bairong:Help HelpText="各项的排列方向。" Text="排列方向：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ddlIsHorizontal" class="input-small" runat="server">
                        <asp:ListItem Text="水平" Selected="True" />
                        <asp:ListItem Text="垂直" />
                    </asp:DropDownList></td>
                <td>
                    <bairong:Help HelpText="项显示的列数。" Text="列数：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox class="input-mini" MaxLength="50" Text="0" ID="tbColumns" runat="server" />
                    <asp:RegularExpressionValidator
                        ControlToValidate="tbColumns"
                        ValidationExpression="\d+"
                        Display="Dynamic"
                        ErrorMessage=" *" ForeColor="red"
                        runat="server" />
                    <span class="gray">（0代表未设置此属性）</span>
                </td>
            </tr>
            <tr>
                <td>默认显示值：</td>
                <td colspan="3">
                    <asp:TextBox Columns="60" ID="tbDefaultValue" runat="server" />
                    <span id="DateTip" runat="server">
                        <br>
                        {Current}代表当前日期/日期时间</span>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="tbDefaultValue"
                        ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /></td>
            </tr>
            <tr id="rowItemsType" runat="server">
                <td>
                    <bairong:Help HelpText="设置选择项需要的项数，设置完项数后需要设置每一项的标题和值。" Text="设置选项：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlItemType" class="input-medium" OnSelectedIndexChanged="ReFresh" AutoPostBack="true" runat="server">
                        <asp:ListItem Text="快速设置" Value="True" Selected="True" />
                        <asp:ListItem Text="详细设置" Value="False" />
                    </asp:DropDownList>&nbsp;&nbsp;
      <asp:PlaceHolder ID="phItemCount" runat="server">共有
      <asp:TextBox class="input-mini" ID="tbItemCount" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbItemCount" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
          个选项&nbsp;
        <asp:Button class="btn" Style="margin-bottom: 0px;" ID="SetCount" Text="设 置" OnClick="SetCount_OnClick" CausesValidation="false" runat="server" />
          <asp:RegularExpressionValidator ControlToValidate="tbItemCount" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server" />
      </asp:PlaceHolder>
                </td>
            </tr>
            <tr id="rowItemsRapid" runat="server">
                <td>
                    <bairong:Help HelpText="设置选项可选值。" Text="选项可选值：" runat="server"></bairong:Help>
                </td>
                <td colspan="3">
                    <asp:TextBox Columns="60" ID="tbItemValues" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbItemValues" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /><span class="grey">英文","分隔</span>
                </td>
            </tr>
            <tr id="rowItems" runat="server">
                <td colspan="4" class="center">
                    <asp:Repeater ID="MyRepeater" runat="server">
                        <ItemTemplate>
                            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                <tr>
                                    <td class="center" style="width: 20px;"><strong><%# Container.ItemIndex + 1 %></strong></td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="3" cellpadding="3">
                                            <tr>
                                                <td width="120">
                                                    <bairong:Help HelpText="设置选项的标题。" Text="选项标题：" runat="server"></bairong:Help>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="ItemTitle" Columns="40" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemTitle") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="ItemTitle" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="120">
                                                    <bairong:Help HelpText="设置选项的值。" Text="选项值：" runat="server"></bairong:Help>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="ItemValue" Columns="40" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemValue") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="ItemValue" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="120">
                                                    <bairong:Help HelpText="设置是否初始化时选定此项。" Text="初始化时选定：" runat="server"></bairong:Help>
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="IsSelected" runat="server" Checked="False" Text="选定"></asp:CheckBox></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
