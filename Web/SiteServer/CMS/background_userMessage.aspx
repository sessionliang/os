﻿<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserMessage" %>

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
    <h3 class="popover-title">短消息群发</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="140">短消息发送对象：</td>
          <td>
            <asp:RadioButtonList ID="rblSelect" RepeatDirection="Horizontal" class="noborder" OnSelectedIndexChanged="rblSelect_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:RadioButtonList>
          </td>
        </tr>
        <asp:PlaceHolder ID="phGroup" runat="server">
          <tr>
            <td>用户组：</td>
            <td>
              <asp:CheckBoxList ID="cblGroupID" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" class="noborder"></asp:CheckBoxList>
            </td>
          </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phUser" Visible="false" runat="server">
          <tr>
            <td>用户名：</td>
            <td>
              <asp:TextBox Width="360" Rows="4" TextMode="MultiLine" id="tbUserNameList" runat="server" />
              <asp:RequiredFieldValidator ControlToValidate="tbUserNameList" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
              <asp:Literal id="ltlSelectUser" runat="server" />
              <br>
              <span class="gray">（要发送的用户名列表，多个用户以“,”分割）</span>
            </td>
          </tr>
        </asp:PlaceHolder>
        <tr>
          <td>短消息正文：</td>
          <td>
          <bairong:BREditor id="breContent" runat="server"></bairong:BREditor>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="发 送" onclick="Submit_OnClick" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->