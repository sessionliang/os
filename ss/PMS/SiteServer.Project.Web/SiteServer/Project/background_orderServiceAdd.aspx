<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrderServiceAdd" %>

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
          <td colspan="3">
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">下单时间：</td>
          <td>
            <input name="AddDate" type="text" class="input-large" id="AddDate" value="<%=GetValue("AddDate")%>" />
          </td>
          <td align="right">关联订单：</td>
          <td>
            <input id="RelatedOrderID" name="RelatedOrderID" value="<%=GetValue("RelatedOrderID")%>" type="hidden">
            <a id="orderSN" href="javascript:;" onclick="<%=GetValue("SelectRelatedOrderID")%>" class="btn btn-info"><%=GetValue("RelatedOrderSN")%></a>
            <script language="javascript">
            function selectRelatedOrderID(orderSN, relatedOrderID, InvoiceTitle, InvoiceReceiver, InvoiceTel, InvoiceAddress){
                $('#RelatedOrderID').val(relatedOrderID);
                $('#orderSN').html(orderSN);

                $('#InvoiceTitle').val(InvoiceTitle);
                $('#InvoiceReceiver').val(InvoiceReceiver);
                $('#InvoiceTel').val(InvoiceTel);
                $('#InvoiceAddress').val(InvoiceAddress);
            }
            </script>
          </td>
        </tr>
        <tr>
          <td align="right">订单金额：</td>
          <td>
            <input name="Amount" type="text" class="input-mini" id="Amount" value="<%=GetValue("Amount")%>" /> 元
          </td>
          <td align="right">业务类型：</td>
          <td>
            <select name="BizType" id="BizType">
              <option <%=GetSelected("BizType", "网站调整", true)%> value="网站调整">网站调整</option>
              <option <%=GetSelected("BizType", "SEO优化")%> value="SEO优化">SEO优化</option>
              <option <%=GetSelected("BizType", "开通邮箱")%> value="开通邮箱">开通邮箱</option>
              <option <%=GetSelected("BizType", "设计定制")%> value="设计定制">设计定制</option>
            </select>
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