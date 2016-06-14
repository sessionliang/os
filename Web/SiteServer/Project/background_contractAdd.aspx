<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundContractAdd" %>

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
          <td width="180">合同号：</td>
          <td><input name="SN" type="text" class="input-large" id="SN" value="<%=GetValue("SN")%>" /></td>
          <td width="140">合同类型：</td>
          <td>
            <select name="ContractType" id="ContractType">
              <option <%=GetSelected("ContractType", "SiteServer_Software", true)%> value="SiteServer_Software">SiteServer 软件合同</option>
              <option <%=GetSelected("ContractType", "SiteServer_Project")%> value="SiteServer_Project">SiteServer 项目合同</option>
              <option <%=GetSelected("ContractType", "SiteServer_Agent")%> value="SiteServer_Agent">SiteServer 代理合同</option>
              <option <%=GetSelected("ContractType", "SiteYun_Order")%> value="SiteYun_Order">SiteYun 订单合同</option>
              <option <%=GetSelected("ContractType", "SiteYun_Partner")%> value="SiteYun_Partner">SiteYun 合作协议</option>
              <option <%=GetSelected("ContractType", "Other")%> value="Other">其他</option>
            </select>  
          </td>
        </tr>
        <asp:PlaceHolder id="phOrder" visible="false" runat="server">
        <tr>
          <td width="180">订单ID：</td>
          <td><asp:Literal id="ltlOrderSN" runat="server" /></td>
          <td width="140">客户ID：</td>
          <td><asp:Literal id="ltlLoginName" runat="server" /></td>
        </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="phAccount" visible="false" runat="server">
        <tr>
          <td width="180">客户名称：</td>
          <td><asp:Literal id="ltlAccountName" runat="server" /></td>
          <td width="140">负责人：</td>
          <td><asp:Literal id="ltlAccountChargeUserName" runat="server" /></td>
        </tr>
        </asp:PlaceHolder>
        <tr>
          <td align="right">负责人：</td>
          <td>
            <div class="fill_box" id="chargeUserContainer" style="display:none">
              <div class="addr_base addr_normal"> <b id="chargeUserDisplayName"></b>
                <input id="chargeUserName" name="chargeUserName" value="" type="hidden">
              </div>
            </div>
            <script language="javascript">
            function chargeUserName(displayName, userName){
                $('#chargeUserDisplayName').html(displayName);
                $('#chargeUserName').val(userName);
                if (userName == ''){
                  $('#chargeUserContainer').hide();
                }else{
                    $('#chargeUserContainer').show();
                }
            }
            </script>
            <asp:Literal id="ltlChargeUserName" runat="server" />
          </td>
          <td align="right">合同金额：</td>
          <td>
            <input name="Amount" type="text" class="input-mini" id="Amount" value="<%=GetValue("Amount")%>" /> 元
          </td>
        </tr>
        <tr>
          <td align="right">合同名称：</td>
          <td>
            <input name="ContractTitle" type="text" class="input-large" id="ContractTitle" value="<%=GetValue("ContractTitle")%>" />
          </td>
          <td align="right">合同收件人：</td>
          <td>
            <input name="ContractReceiver" type="text" class="input-large" id="ContractReceiver" value="<%=GetValue("ContractReceiver")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">联系电话：</td>
          <td>
            <input name="ContractTel" type="text" class="input-large" id="ContractTel" value="<%=GetValue("ContractTel")%>" />
          </td>
          <td align="right">邮寄地址：</td>
          <td>
            <input name="ContractAddress" type="text" class="input-large" id="ContractAddress" value="<%=GetValue("ContractAddress")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">备注：</td>
          <td colspan="3">
            <textarea name="Summary" type="text" style="width:90%;height:120px;" id="Summary"><%=GetValue("Summary")%></textarea>
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