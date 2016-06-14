<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleVoteAdd" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <div class="column">
    <div class="columntitle">
      <asp:Literal id="literalTitle" runat="server" />投票</div>
    <table class="table table-noborder table-hover">
      <tr>
        <td width="160"><bairong:help ID="Help1" HelpText="投票的名称" Text="投票名称：" runat="server" ></bairong:help></td>
        <td colspan="3"><asp:TextBox Columns="45" id="VoteName" runat="server"/>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="VoteName" ErrorMessage="请设置投票名称" Display="Dynamic"
										runat="server" />
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="VoteName"
										ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td width="160"><bairong:help ID="Help2" HelpText="此投票的标题名称" Text="标题：" runat="server" ></bairong:help></td>
        <td colspan="3"><asp:TextBox Columns="60" TextMode="MultiLine" id="TheTitle" runat="server" Rows="5"/>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TheTitle" ErrorMessage="请设置投票标题" Display="Dynamic"
										runat="server" />
          <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TheTitle"
										ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td width="160"><bairong:help ID="Help3" HelpText="设置用户投票的方式，可以是单选或多选。" Text="投票类型：" runat="server" ></bairong:help></td>
        <td colspan="3"><asp:RadioButtonList id="VoteType" RepeatDirection="Horizontal" class="noborder" AutoPostBack="true" OnSelectedIndexChanged="ReFresh" runat="server"></asp:RadioButtonList></td>
      </tr>
      <tr id="MaxVoteItemNumRow" runat="server">
        <td width="160"><bairong:help ID="Help4" HelpText="设置用户用户最多能够选择的选项数目，0代表不限制。" Text="对多可选择项数：" runat="server" ></bairong:help></td>
        <td width="80%"><asp:TextBox id="MaxVoteItemNum" Columns="4" Text="0" runat="server"></asp:TextBox>
          项
          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="MaxVoteItemNum" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="MaxVoteItemNum" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server" />
          （0代表不限制） </td>
      </tr>
      <tr>
        <td  width="160"><bairong:help ID="Help5" HelpText="投票限制类型" Text="投票限制：" runat="server" ></bairong:help></td>
        <td width="80%"><asp:RadioButtonList ID="VoteRestrictType" runat="server"></asp:RadioButtonList></td>
      </tr>
      <tr>
        <td width="160"><bairong:help ID="Help6" HelpText="设置用户投票项的类型，可以是文字型、图片型或图文混合型。" Text="投票项类型：" runat="server" ></bairong:help></td>
        <td colspan="3"><asp:DropDownList id="VoteItemType" AutoPostBack="true" OnSelectedIndexChanged="ReFresh" runat="server"></asp:DropDownList></td>
      </tr>
      <tr>
        <td width="160"><bairong:help ID="Help7" HelpText="设置需要投票的项数，设置完项数后需要设置每一投票项的标题和显示颜色。" Text="设置投票项数目：" runat="server" ></bairong:help></td>
        <td colspan="3"><asp:TextBox Columns="4" Text="4" id="ItemCount" runat="server" />
          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ItemCount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:Button class="btn" style="margin-bottom:0px;" id="SetCount" text="设 置" onclick="SetCount_OnClick" CausesValidation="false" runat="server" />
          <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="ItemCount" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server" /></td>
      </tr>
      <tr>
        <td colspan="8" class="center"><asp:Repeater ID="MyRepeater" runat="server">
            <itemtemplate>
              <table width="100%" border="0" cellspacing="2" cellpadding="2">
                <tr>
                  <td class="center" style="width:20px;"><strong><%# Container.ItemIndex + 1 %></strong></td>
                  <td><table width="100%" border="0" cellspacing="3" cellpadding="3">
                      <tr id="VoteItemTitleRow" runat="server">
                        <td width="120"><bairong:help ID="Help8" HelpText="设置投票项的标题。" Text="标题：" runat="server" ></bairong:help></td>
                        <td colspan="3"><asp:TextBox ID="ItemTitle" Columns="60" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemTitle") %>'></asp:TextBox>
                          <asp:RequiredFieldValidator ID="ItemTitleRequired" ControlToValidate="ItemTitle" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
                      </tr>
                      <tr id="VoteItemImageUrlRow" runat="server">
                        <td width="120"><bairong:help ID="Help9" HelpText="设置投票项的图片地址。" Text="图片地址：" runat="server" ></bairong:help></td>
                        <td colspan="3"><asp:TextBox ID="ItemImageUrl" Columns="60" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ItemImageUrl") %>'></asp:TextBox>
                          <asp:RequiredFieldValidator ID="ItemImageUrlRequired" ControlToValidate="ItemImageUrl" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
                      </tr>
                      <tr>
                        <td width="120"><bairong:help ID="Help10" HelpText="设置投票项的链接。" Text="链接：" runat="server" ></bairong:help></td>
                        <td colspan="3"><asp:TextBox ID="NavigationUrl" Columns="60" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NavigationUrl") %>'></asp:TextBox></td>
                      </tr>
                      <tr>
                        <td width="120"><bairong:help ID="Help11" HelpText="设置投票项显示时的颜色。" Text="颜色：" runat="server" ></bairong:help></td>
                        <td width="170"><asp:TextBox Columns="15" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayColor") %>' id="DisplayColor" runat="server" />
                          <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="DisplayColor"
								ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
                          <a href="javascript:;" onClick="javascript:var TempReturnValue=window.showModalDialog('../inc/color.htm','ResWin','dialogWidth:18.5em; dialogHeight:17.5em;');if (typeof(TempReturnValue)!='undefined') this.parentElement.firstChild.value=TempReturnValue;" style="text-decoration:underline">选择</a></td>
                        <td width="60"><bairong:help ID="Help12" HelpText="设置投票项的票数。" Text="票数：" runat="server" ></bairong:help></td>
                        <td ><asp:TextBox Columns="15" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem,"VoteNum") %>' id="VoteNum" runat="server" />
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="VoteNum" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
                          <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="VoteNum" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server" /></td>
                      </tr>
                    </table></td>
                </tr>
              </table>
            </itemtemplate>
            <SeparatorTemplate>
              <table width="100%" class="center" cellspacing="0" cellpadding="0">
                <tr>
                  <td class="mframe-b-mid">&nbsp;</td>
                </tr>
              </table>
            </SeparatorTemplate>
          </asp:Repeater></td>
      </tr>
      <tr>
        <td colspan="4" class="center"><table width="95%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td><asp:Literal id="VoteItems" runat="server" /></td>
            </tr>
          </table></td>
      </tr>
    </table>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->