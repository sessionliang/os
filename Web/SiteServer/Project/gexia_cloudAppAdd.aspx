<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundCloudAppAdd" %>

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
          <td align="right">应用节点编号：</td>
          <td colspan="3">
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">服务器：</td>
          <td>
            <select name="ServerSN" id="ServerSN">
              <%=GetOptions("ServerSN")%>
            </select>
          </td>
          <td align="right">用户类型：</td>
          <td>
            <select name="UserType" id="UserType">
              <%=GetOptions("UserType")%>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">用户名：</td>
          <td>
            <input name="Administrator" type="text" id="Administrator" value="<%=GetValue("Administrator")%>" />
          </td>
          <td align="right">密码：</td>
          <td>
            <input name="Password" type="text" id="Password" value="<%=GetValue("Password")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">ISVKey：</td>
          <td colspan="3">
            <input name="ISVKey" type="text" id="ISVKey" value="<%=GetValue("ISVKey")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">开通时间：</td>
          <td colspan="3">
            <input name="StartDate" type="text" class="input-large" id="StartDate" value="<%=GetValue("StartDate")%>" />
          </td>
        </tr>
        
        <tr>
          <td align="right">备注：</td>
          <td colspan="3">
          <textarea name="Summary" style="width:90%;height:60px;" id="Summary" ><%=GetValue("Summary")%></textarea>
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