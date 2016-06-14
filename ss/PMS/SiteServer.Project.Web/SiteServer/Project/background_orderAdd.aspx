<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrderAdd" %>

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
        <tr height="2">
          <td width="120"></td>
          <td></td>
          <td width="120"></td>
          <td></td>
        </tr>
        <tr>
          <td align="right">订单ID：</td>
          <td>
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
          </td>
          <td align="right">订单类型：</td>
          <td colspan="3">
            <select name="IsReNew" id="IsReNew" class="input-small">
              <option <%=GetSelected("IsReNew", "False", true)%> value="False">新购</option>
              <option <%=GetSelected("IsReNew", "True")%> value="True">续费</option>
            </select>
            <input name="Duration" type="text" class="input-mini" id="Duration" value="<%=GetValue("Duration")%>" /> 年
          </td>
        </tr>
        <tr>
          <td align="right">订单金额：</td>
          <td>
            <input name="Amount" type="text" class="input-mini" id="Amount" value="<%=GetValue("Amount")%>" /> 元
          </td>
          <td align="right">业务来源：</td>
          <td>
            <select name="BizType" id="BizType">
              <option <%=GetSelected("BizType", "阿里云", true)%> value="阿里云">阿里云</option>
              <option <%=GetSelected("BizType", "淘宝")%> value="淘宝">淘宝</option>
              <option <%=GetSelected("BizType", "siteyun.com")%> value="siteyun.com">siteyun.com</option>
              <option <%=GetSelected("BizType", "其他")%> value="其他">其他</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">模板ID：</td>
          <td>
            <input id="MobanID" name="MobanID" value="<%=GetValue("MobanID")%>" type="hidden">
            <a id="mobanSN" href="javascript:;" onclick="<%=GetValue("SelectMobanID")%>" class="btn btn-info"><%=GetValue("MobanSN")%></a>
            <script language="javascript">
            function selectMobanID(mobanSN){
                $('#MobanID').val(mobanSN);
                $('#mobanSN').html(mobanSN);
            }
            </script>
          </td>
          <td align="right">下单时间：</td>
          <td>
            <input name="AddDate" type="text" class="input-large" id="AddDate" value="<%=GetValue("AddDate")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">临时域名：</td>
          <td>
            <input name="DomainTemp" type="text" class="input-large" id="DomainTemp" value="<%=GetValue("DomainTemp")%>" />
          </td>
          <td align="right">正式域名：</td>
          <td>
            <input name="DomainFormal" type="text" class="input-large" id="DomainFormal" value="<%=GetValue("DomainFormal")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">后台账号：</td>
          <td>
            <input name="BackgroundUserName" type="text" class="input-large" id="BackgroundUserName" value="<%=GetValue("BackgroundUserName")%>" />
          </td>
          <td align="right">后台密码：</td>
          <td>
            <input name="BackgroundPassword" type="text" class="input-large" id="BackgroundPassword" value="<%=GetValue("BackgroundPassword")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">客户ID：</td>
          <td>
            <input name="LoginName" type="text" class="input-large" id="LoginName" value="<%=GetValue("LoginName")%>" />
          </td>
          <td align="right">电子邮件：</td>
          <td>
            <input name="Email" type="text" class="input-large" id="Email" value="<%=GetValue("Email")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">联系人：</td>
          <td>
            <input name="ContactName" type="text" class="input-large" id="ContactName" value="<%=GetValue("ContactName")%>" />
          </td>
          <td align="right">手机：</td>
          <td>
            <input name="Mobile" type="text" class="input-large" id="Mobile" value="<%=GetValue("Mobile")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">QQ：</td>
          <td>
            <input name="QQ" type="text" class="input-large" id="QQ" value="<%=GetValue("QQ")%>" />
          </td>
          <td align="right">阿里旺旺：</td>
          <td>
            <input name="AliWangWang" type="text" class="input-large" id="AliWangWang" value="<%=GetValue("AliWangWang")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">发票抬头：</td>
          <td>
            <input name="InvoiceTitle" type="text" class="input-large" id="InvoiceTitle" value="<%=GetValue("InvoiceTitle")%>" />
          </td>
          <td align="right">发票收件人：</td>
          <td>
            <input name="InvoiceReceiver" type="text" class="input-large" id="InvoiceReceiver" value="<%=GetValue("InvoiceReceiver")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">发票联系电话：</td>
          <td>
            <input name="InvoiceTel" type="text" class="input-large" id="InvoiceTel" value="<%=GetValue("InvoiceTel")%>" />
          </td>
          <td align="right">发票邮寄地址：</td>
          <td>
            <input name="InvoiceAddress" type="text" class="input-large" id="InvoiceAddress" value="<%=GetValue("InvoiceAddress")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">备注：</td>
          <td colspan="3"><bairong:BREditor id="Summary" runat="server"></bairong:BREditor></td>
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