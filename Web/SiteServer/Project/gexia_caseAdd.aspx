<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundCaseAdd" %>

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
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">

      <table class="table noborder table-hover">
        <tr>
          <td align="right">公众号类型：</td>
          <td>
            <select name="AccountType" id="AccountType">
              <%=GetOptions("AccountType")%>
            </select>
          </td>
          <td align="right">微信ID：</td>
          <td colspan="3">
            <input name="WeChatID" type="text" id="WeChatID" value="<%=GetValue("WeChatID")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">微信名称：</td>
          <td>
            <input name="WeChatName" type="text" id="WeChatName" value="<%=GetValue("WeChatName")%>" />
          </td>
          <td align="right">LOGO图标：</td>
          <td>
            <input name="IconUrl" type="text" id="IconUrl" value="<%=GetValue("IconUrl")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">缩略图：</td>
          <td>
            <input name="ThumbUrl" type="text" id="ThumbUrl" value="<%=GetValue("ThumbUrl")%>" />
          </td>
          <td align="right">二维码：</td>
          <td>
            <input name="QRCodeUrl" type="text" id="QRCodeUrl" value="<%=GetValue("QRCodeUrl")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">简介：</td>
          <td colspan="3">
            <textarea name="Summary" type="text" class="input-large" rows="3" style="width:95%" id="Summary"><%=GetValue("Summary")%></textarea>
          </td>
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