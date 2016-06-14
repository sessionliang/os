<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderView" Trace="false"%>

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

  <table class="table table-bordered table-hover">
    <tr>
      <td width="120" align="right">订单ID：</td>
      <td>
        <%=GetValue("SN")%>
      </td>
      <td width="120" align="right">订单类型：</td>
      <td colspan="3">
        <%=GetValue("IsReNew")%>
        <%=GetValue("Duration")%> 年
      </td>
    </tr>
    <tr>
      <td align="right">订单金额：</td>
      <td>
        <%=GetValue("Amount")%> 元
      </td>
      <td align="right">订单状态：</td>
      <td>
        <%=GetValue("Status")%>
      </td>
    </tr>
    <tr>
      <td align="right">模板ID：</td>
      <td>
        <%=GetValue("MobanID")%>
      </td>
      <td align="right">下单时间：</td>
      <td>
        <%=GetValue("AddDate")%>
      </td>
    </tr>
    <tr>
      <td align="right">临时域名：</td>
      <td>
        <%=GetValue("DomainTemp")%>
      </td>
      <td align="right">正式域名：</td>
      <td>
        <%=GetValue("DomainFormal")%>
      </td>
    </tr>
    <tr>
      <td align="right">后台账号：</td>
      <td>
        <%=GetValue("BackgroundUserName")%>
      </td>
      <td align="right">后台密码：</td>
      <td>
        <%=GetValue("BackgroundPassword")%>
      </td>
    </tr>
    <tr>
      <td align="right">客户ID：</td>
      <td>
        <%=GetValue("LoginName")%>
      </td>
      <td align="right">电子邮件：</td>
      <td>
        <%=GetValue("Email")%>
      </td>
    </tr>
    <tr>
      <td align="right">联系人：</td>
      <td>
        <%=GetValue("ContactName")%>
      </td>
      <td align="right">手机：</td>
      <td>
        <%=GetValue("Mobile")%>
      </td>
    </tr>
    <tr>
      <td align="right">QQ：</td>
      <td>
        <%=GetValue("QQ")%>
      </td>
      <td align="right">阿里旺旺：</td>
      <td>
        <%=GetValue("AliWangWang")%>
      </td>
    </tr>
    <tr>
      <td align="right">发票抬头：</td>
      <td>
        <%=GetValue("InvoiceTitle")%>
      </td>
      <td align="right">发票收件人：</td>
      <td>
        <%=GetValue("InvoiceReceiver")%>
      </td>
    </tr>
    <tr>
      <td align="right">发票联系电话：</td>
      <td>
        <%=GetValue("InvoiceTel")%>
      </td>
      <td align="right">发票邮寄地址：</td>
      <td>
        <%=GetValue("InvoiceAddress")%>
      </td>
    </tr>
    <tr>
      <td align="right">备注：</td>
      <td colspan="3"><%=GetValue("Summary")%></td>
    </tr>
  </table>

</form>
</body>
</html>