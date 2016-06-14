<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrderRefundAdd" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" enctype="multipart/form-data" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">

      <table class="table noborder table-hover">
        <tr>
          <td width="140">订单ID：</td>
          <td><asp:Literal id="ltlOrderSN" runat="server" /></td>
          <td width="140">客户ID：</td>
          <td><asp:Literal id="ltlLoginName" runat="server" /></td>
        </tr>
        <tr>
          <td align="right">退款方式：</td>
          <td>
            <select name="IsAliyunRefund" id="IsAliyunRefund">
              <option <%=GetSelected("IsAliyunRefund", "True", true)%> value="True">通过阿里云退款</option>
              <option <%=GetSelected("IsAliyunRefund", "False")%> value="False">直接退款</option>
            </select>
          </td>
          <td align="right">退款金额：</td>
          <td>
            <input name="Amount" type="text" class="input-mini" id="Amount" value="<%=GetValue("Amount")%>" /> 元
          </td>
        </tr>
        <tr>
          <td align="right">阿里云退款申请单：</td>
          <td colspan="3">
            <input type="file" id="AliyunFileUrl" name="AliyunFileUrl" />
            <%=GetValue("AliyunFileUrl")%>
          </td>
        </tr>
        <tr>
          <td align="right">退款账户：</td>
          <td>
            <select name="IsAlipayAccount" id="IsAlipayAccount">
              <option <%=GetSelected("IsAlipayAccount", "True", true)%> value="True">支付宝</option>
              <option <%=GetSelected("IsAlipayAccount", "False")%> value="False">银行汇款</option>
            </select>
          </td>
          <td align="right">退款人真实姓名：</td>
          <td>
            <input name="AccountRealName" type="text" class="input-large" id="AccountRealName" value="<%=GetValue("AccountRealName")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">支付宝账号：</td>
          <td colspan="3">
            <input name="AccountAlipayNo" type="text" class="input-large" id="AccountAlipayNo" value="<%=GetValue("AccountAlipayNo")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">汇款银行：</td>
          <td>
            <input name="AccountBankName" type="text" class="input-large" id="AccountBankName" value="<%=GetValue("AccountBankName")%>" />
          </td>
          <td align="right">银行账号：</td>
          <td>
            <input name="AccountBankCardNo" type="text" class="input-large" id="AccountBankCardNo" value="<%=GetValue("AccountBankCardNo")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">退款原因：</td>
          <td colspan="3">
            <textarea name="Reason" type="text" style="width:90%;height:120px;" id="Reason"><%=GetValue("Reason")%></textarea>
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