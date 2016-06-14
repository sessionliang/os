<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundOrderView" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form id="form1" runat="server">
<div class="column">
<div class="columntitle">查看订单</div>
<div  class="summary-title"  style="text-align:left; padding:3px 5px; margin-bottom:10px;"><strong>订单状态操作:</strong>&nbsp;&nbsp;
<asp:Button ID="Pay" class="btn" runat="server" Text="支付" style="MARGIN-BOTTOM: 0px" />
<asp:Button ID="Consignment" class="btn" runat="server" Text="发货" style="MARGIN-BOTTOM: 0px"/>
<asp:Button ID="Success" class="btn" runat="server" Text="完成"  style="MARGIN-BOTTOM: 0px"/>
<asp:Button ID="Refund" class="btn" runat="server" Text="退款" style="MARGIN-BOTTOM: 0px" />
<asp:Button ID="Return" class="btn" runat="server" Text="退货" style="MARGIN-BOTTOM: 0px" />
<asp:Button ID="Fail" class="btn" runat="server" Text="作废" style="MARGIN-BOTTOM: 0px" />
</div>
<div class="baseinfo">
<div   class="summary-title" style="text-align:left; padding:3px 5px" ><strong>基本信息</strong></div>
<table width="95%" cellpadding="3" cellspacing="3">
    <tr>
        <td width="60"  align="right">订单号:</td>
        <td width="200">43453454</td>
        <td width="60"  align="right">订单状态:</td>
        <td>未发货，未支付</td>
    </tr>
      <tr>
        <td width="60"  align="right">购货人:</td>
        <td width="200">张三</td>
        <td width="60"  align="right">下单时间:</td>
        <td>2011-09-12</td>
    </tr>
      <tr>
        <td width="60"  align="right">支付方式:</td>
        <td width="200">在线支付</td>
        <td width="60"  align="right">付款时间:</td>
        <td>2011-09-12</td>
    </tr>
      <tr>
        <td width="60"  align="right">配送方式:</td>
        <td width="200">申通快递</td>
        <td width="60"  align="right">发货时间:</td>
        <td>未发货</td>
    </tr>      
</table>
</div>
<div class="baseinfo">
 <div   class="summary-title" style="text-align:left; padding:3px 5px" ><strong>其它信息</strong></div>
<table width="95%" cellpadding="3" cellspacing="3">
    <tr>
        <td width="60"  align="right">发票类型:</td>
        <td width="200">个人</td>
        <td width="60"  align="right">发票内容:</td>
        <td></td>
    </tr>
      <tr>
        <td width="60"  align="right">发票抬头:</td>
        <td width="200"></td>
        <td width="60"  align="right">订单赋言:</td>
        <td>无</td>
    </tr>
</table>
 </div>
 <div class="baseinfo">
 <div   class="summary-title" style="text-align:left; padding:3px 5px" ><strong>收货人信息</strong></div>
<table width="95%" cellpadding="3" cellspacing="3">
    <tr>
        <td width="60"  align="right">收货人:</td>
        <td width="200">张三</td>
        <td width="60"  align="right">电子邮箱:</td>
        <td>234@qq.com</td>
    </tr>
      <tr>
        <td width="60"  align="right">地址:</td>
        <td width="200">河南省新密市</td>
        <td width="60"  align="right">邮篇:</td>
        <td>452310</td>
    </tr>
       <tr>
        <td width="60"  align="right">电话:</td>
        <td width="200">无</td>
        <td width="60"  align="right">手机:</td>
        <td>12321324321</td>
    </tr>
    <tr>
        <td width="60"  align="right">送货时间:</td>
        <td width="200">仅周未</td>
    </tr>
</table>
 </div>
 <div class="baseinfo">
 <div   class="summary-title" style="text-align:left; padding:3px 5px" ><strong>商品信息</strong></div>
<table width="95%" cellpadding="3" cellspacing="3">
    <tr>
        <td width="60"  align="right">商品名称:</td>
        <td width="200">摩托罗拉A810 [ 摩托罗拉 ] </td>
        <td width="60"  align="right">商品号:</td>
        <td>ECS000012</td>
    </tr>
      <tr>
        <td width="60"  align="right">价格:</td>
        <td width="200">30元</td>
        <td width="60"  align="right">数量:</td>
        <td>1</td>
    </tr>
   
</table>
 </div>
  <div class="baseinfo">
 <div   class="summary-title" style="text-align:left; padding:3px 5px" ><strong>费用信息</strong></div>
<table width="95%" cellpadding="3" cellspacing="3">
    <tr>
        <td width="60"  align="right">商品总额:</td>
        <td width="200">30.00 </td>
        <td width="60">折扣:</td>
        <td>0.00</td>
    </tr>
      <tr>
        <td width="60"  align="right">配送费用:</td>
        <td width="200">10.00</td>
    </tr>
    <tr>
         <td width="60"  align="right">订单总额:</td>
        <td>40.00</td>
    </tr>
   
</table>
 </div>
</div>
</form>
</body>
</html>
