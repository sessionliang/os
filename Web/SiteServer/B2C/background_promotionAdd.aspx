<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundPromotionAdd" %>

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

  <script language="javascript" type="text/javascript">
  function excludeChannelAdd(name, value){
    $('#excludeChannelContainer').append("<div id='excludeChannel_" + value + "' class='addr_base addr_normal'><b>" + name + "</b> <a class='addr_del' href='javascript:;' onClick=\"excludeChannelRemove('" + value + "')\"></a></div>");
    $('#excludeChannelIDCollection').val(value + ',' + $('#excludeChannelIDCollection').val());
  }
  function excludeChannelRemove(value){
    $('#excludeChannel_' + value).remove();
    var val = '';
    var values = $('#excludeChannelIDCollection').val().split(",");
    for (i=0;i<values.length ;i++ )
    {
      if (values[i] && value != values[i]){val += values[i] + ',';}
    } 
    $('#excludeChannelIDCollection').val(val);
  }

  function channelAdd(name, value){
    $('#channelContainer').append("<div id='channel_" + value + "' class='addr_base addr_normal'><b>" + name + "</b> <a class='addr_del' href='javascript:;' onClick=\"channelRemove('" + value + "')\"></a></div>");
    $('#channelIDCollection').val(value + ',' + $('#channelIDCollection').val());
  }
  function channelRemove(value){
    $('#channel_' + value).remove();
    var val = '';
    var values = $('#channelIDCollection').val().split(",");
    for (i=0;i<values.length ;i++ )
    {
      if (values[i] && value != values[i]){val += values[i] + ',';}
    } 
    $('#channelIDCollection').val(val);
  }

  function excludeIDsAdd(name, value){
    $('#excludeContentContainer').append("<div id='excludeIDs_" + value + "' class='addr_base addr_normal'><b>" + name + "</b> <a class='addr_del' href='javascript:;' onClick=\"excludeIDsRemove('" + value + "')\"></a></div>");
    $('#excludeIDsCollection').val(value + ',' + $('#excludeIDsCollection').val());
  }
  function excludeIDsRemove(value){
    $('#excludeIDs_' + value).remove();
    var val = '';
    var values = $('#excludeIDsCollection').val().split(",");
    for (i=0;i<values.length ;i++ )
    {
      if (values[i] && value != values[i]){val += values[i] + ',';}
    } 
    $('#excludeIDsCollection').val(val);
  }

  function idsAdd(name, value){
    $('#contentContainer').append("<div id='ids_" + value + "' class='addr_base addr_normal'><b>" + name + "</b> <a class='addr_del' href='javascript:;' onClick=\"idsRemove('" + value + "')\"></a></div>");
    $('#idsCollection').val(value + ',' + $('#idsCollection').val());
  }
  function idsRemove(value){
    $('#ids_' + value).remove();
    var val = '';
    var values = $('#idsCollection').val().split(",");
    for (i=0;i<values.length ;i++ )
    {
      if (values[i] && value != values[i]){val += values[i] + ',';}
    } 
    $('#idsCollection').val(val);
  }
  </script>

  <div class="popover popover-static">
  <h3 class="popover-title">打折促销设置</h3>
  <div class="popover-content">

  <table class="table noborder table-hover">
    <tr>
      <td width="160">促销名称：</td>
      <td>
        <asp:TextBox  Columns="25" MaxLength="50" id="tbPromotionName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbPromotionName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPromotionName" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
      <td width="160">价格标签：</td>
      <td>
        <asp:TextBox  Columns="25" MaxLength="50" id="tbTags" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTags" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbTags" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>开始时间：</td>
      <td>
        <bairong:DateTimeTextBox ID="tbStartDate" showTime="true" runat="server"></bairong:DateTimeTextBox>
      </td>
      <td>结束时间：</td>
      <td>
        <bairong:DateTimeTextBox ID="tbEndDate" showTime="true" runat="server"></bairong:DateTimeTextBox>
      </td>
    </tr>
    <tr>
      <td>促销目标：</td>
      <td colspan="3">
          <asp:RadioButtonList ID="rblTarget" class="radiobuttonlist" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblTarget_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:RadioButtonList>
      </td>
    </tr>
    <asp:PlaceHolder id="phTargetChannel" runat="server">
    <tr>
      <td>促销目标（分类）：</td>
      <td colspan="3">
        <div class="fill_box" id="channelContainer">
          <asp:Literal id="ltlChannelIDCollection" runat="server" />
        </div>
        <asp:Button id="btnSelectChannel" class="btn btn-success" runat="server" text="选择" />
      </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phTargetContent" runat="server">
    <tr>
      <td>促销目标（商品）：</td>
      <td colspan="3">
        <div class="fill_box" id="contentContainer">
          <asp:Literal id="ltlIDsCollection" runat="server" />
        </div>
        <asp:Button id="btnSelectContent" class="btn btn-success" runat="server" text="选择" />
      </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
      <td>排除目标（分类）：</td>
      <td colspan="3">
        <div class="fill_box" id="excludeChannelContainer">
          <asp:Literal id="ltlExcludeChannelIDCollection" runat="server" />
        </div>
        <asp:Button id="btnSelectExcludeChannel" class="btn btn-success" runat="server" text="选择" />
      </td>
    </tr>
    <tr>
      <td>排除目标（商品）：</td>
      <td colspan="3">
        <div class="fill_box" id="excludeContentContainer">
          <asp:Literal id="ltlExcludeIDsCollection" runat="server" />
        </div>
        <asp:Button id="btnSelectExcludeContent" class="btn btn-success" runat="server" text="选择" />
      </td>
    </tr>
    <tr>
      <td>触发条件（商品金额）：</td>
      <td>
      <asp:TextBox  class="input-mini" id="tbIfAmount" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbIfAmount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbIfAmount" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /> 元
      </td>
      <td>触发条件（商品数量）：</td>
      <td>
      <asp:TextBox  class="input-mini" id="tbIfCount" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbIfCount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbIfCount" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /> 件
      </td>
    </tr>
    <tr>
      <td>促销优惠（折扣）：</td>
      <td>
      <asp:TextBox  class="input-mini" id="tbDiscount" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbDiscount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbDiscount" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /> 折
        <span class="gray">0.85代表85折</span>
      </td>
      <td>促销优惠（返现）：</td>
      <td>
      <asp:TextBox  class="input-mini" id="tbReturnAmount" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbReturnAmount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbReturnAmount" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /> 元
        
        <label class="checkbox">
          <asp:CheckBox id="cbIsReturnMultiply" runat="server" />
          翻倍返现，上不封顶
        </label>
      </td>
    </tr>
    <tr>
      <td>促销优惠（免运费）：</td>
      <td>
        <label class="checkbox">
          <asp:CheckBox id="cbIsShipmentFree" runat="server" />
          免运费
        </label>
      </td>
      <td>促销优惠（送礼品）：</td>
      <td>
        <label class="checkbox">
          <asp:CheckBox id="cbIsGift" runat="server" />
          送礼品
        </label>
      </td>
    </tr>
    <tr>
      <td>送礼品（礼品名称）：</td>
      <td>
        <asp:TextBox  id="tbGiftName" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbGiftName" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
      <td>送礼品（礼品网址）：</td>
      <td>
        <asp:TextBox id="tbGiftUrl" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbGiftUrl" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>备注：</td>
      <td colspan="3">
        <asp:TextBox TextMode="MultiLine" Rows="2"  style="width:95%" id="tbDescription" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbDescription" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
  </table>

  <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
            <input type="button" value="返 回" onclick="javascript:location.href = 'background_promotion.aspx?publishmentSystemID=<%=PublishmentSystemID%>';" class="btn">
          </td>
        </tr>
      </table>

    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->