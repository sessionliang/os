<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundCloudServerAdd" %>

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
          <td align="right">服务器编号：</td>
          <td>
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
          </td>
          <td align="right">用户类型：</td>
          <td>
            <select name="UserType" id="UserType"><%=GetOptions("UserType")%></select>
          </td>
        </tr>
        <tr>
          <td align="right">IP地址：</td>
          <td>
            <input name="IPAddress" type="text" id="IPAddress" value="<%=GetValue("IPAddress")%>" />
          </td>
          <td align="right">内网IP：</td>
          <td>
            <input name="IntranetIP" type="text" id="IntranetIP" value="<%=GetValue("IntranetIP")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">密码：</td>
          <td>
            <input name="Password" type="text" id="Password" value="<%=GetValue("Password")%>" />
          </td>
          <td align="right">Sql密码：</td>
          <td>
            <input name="SqlPassword" type="text" id="SqlPassword" value="<%=GetValue("SqlPassword")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">购买时间：</td>
          <td>
            <input name="StartDate" type="text" class="input-large" id="StartDate" value="<%=GetValue("StartDate")%>" />
          </td>
          <td align="right">到期时间：</td>
          <td>
            <input name="EndDate" type="text" class="input-large" id="EndDate" value="<%=GetValue("EndDate")%>" />
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