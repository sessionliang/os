<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundAccessControl" %>

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
  <h3 class="popover-title">访问控制</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="170">新手见习期限：</td>
        <td>
          <asp:TextBox Columns="10" id="txtNovitiateByMinute" runat="server" /> 分钟  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtNovitiateByMinute"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br />
          <span class="gray">新注册用户在本期限内将无法发帖和短消息，不影响版主和管理员，0 为不限制</span>
        </td>
      </tr> 
      <tr>
        <td width="170">禁止访问时间段：</td>
        <td>
          <asp:TextBox TextMode="MultiLine" Columns="40" Rows="6" id="txtForbiddenAccessTime" runat="server" /> 
          <br />
          <span class="gray">每天该时间段内用户不能访问站点，请使用 24 小时时段格式，每个时间段一行，如需要也可跨越零点，留空为不限制。例如:
每日晚 11:25 到次日早 5:05 可设置为: 23:25-5:05
每日早 9:00 到当日下午 2:30 可设置为: 9:00-14:30
注意: 格式不正确将可能导致意想不到的问题，用户组中如开启“不受时间段限制”的选项，则该组可不被任何时间段设置约束。</span>
        </td>
      </tr> 
      <tr>
        <td width="170">禁止发帖时间段：</td>
        <td>
          <asp:TextBox TextMode="MultiLine" Columns="40" Rows="6" id="txtForbiddenPostTime" runat="server" /> 
          <br />
          <span class="gray">每天该时间段内用户不能发帖，格式用法同上</span>
        </td>
      </tr> 
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->