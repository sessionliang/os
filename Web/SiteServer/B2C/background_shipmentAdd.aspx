<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundShipmentAdd" %>

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

  <script>
  function changeInsurance(){
    if($('#cbIsInsurance').attr("checked")){
      $('#trInsurance').show();
    }else{
      $('#trInsurance').hide();
    }
  }
  $(function(){
    changeInsurance();
    $("#cbIsInsurance").click(function() {
      changeInsurance();
    });
  });
  </script>

  <div class="popover popover-static">
  <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="180">配送方式名称：</td>
        <td>
          <asp:TextBox id="tbShipmentName" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbShipmentName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbShipmentName" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>配送时间：</td>
        <td>
          <asp:RadioButtonList id="rblShipmentPeriod" runat="server" repeatDirection="Horizontal" class="radiobuttonlist"></asp:RadioButtonList>
        </td>
      </tr>
      <tr>
        <td>配送方式描述：</td>
        <td>
          <bairong:BREditor id="breDescription" runat="server"></bairong:BREditor>
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
          <input type="button" value="返 回" onclick="javascript:location.href = 'background_shipment.aspx?PublishmentSystemID=<%=PublishmentSystemID%>';" class="btn">
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->