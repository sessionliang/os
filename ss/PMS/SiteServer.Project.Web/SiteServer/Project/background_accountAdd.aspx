<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundAccountAdd" %>

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
          <td width="80"></td>
          <td></td>
          <td width="80"></td>
          <td></td>
        </tr>
        <tr>
          <td align="right">客户编号：</td>
          <td>
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
            </td>
            <td align="right">客户名称：</td>
          <td>
            <input name="AccountName" type="text" id="AccountName" value="<%=GetValue("AccountName")%>" style="width:350px;" />
            </td>
        </tr>
        <tr>
          <td width="80" align="right">添加人：</td>
          <td>
            <asp:Literal id="ltlAddUserName" runat="server" />
          </td>
          <td width="80" align="right">负责人：</td>
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
        </tr>
        <tr>
          <td align="right">状态：</td>
          <td>
            <select name="Status" id="Status">
              <option <%=GetSelected("Status", "Contracted", true)%> value="Contracted">签约客户</option>
              <option <%=GetSelected("Status", "Invalid")%> value="Invalid">无效客户</option>
            </select>
          </td>
          <td align="right">客户等级：</td>
          <td>
            <select name="Priority" id="Priority">
              <option <%=GetSelected("Priority", "1", true)%> value="1">普通</option>
              <option <%=GetSelected("Priority", "2")%> value="2">高</option>
              <option <%=GetSelected("Priority", "3")%> value="3">重点</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">行业：</td>
          <td>
            <select name="BusinessType" id="BusinessType">
              <option <%=GetSelected("BusinessType", "未知", true)%> value="未知">未知</option>
              <option <%=GetSelected("BusinessType", "省级及以上政府")%> value="省级及以上政府">省级及以上政府</option>
              <option <%=GetSelected("BusinessType", "市级政府")%> value="市级政府">市级政府</option>
              <option <%=GetSelected("BusinessType", "县级政府")%> value="县级政府">县级政府</option>
              <option <%=GetSelected("BusinessType", "事业单位")%> value="事业单位">事业单位</option>
              <option <%=GetSelected("BusinessType", "上市公司")%> value="上市公司">上市公司</option>
              <option <%=GetSelected("BusinessType", "大中型企业")%> value="大中型企业">大中型企业</option>
              <option <%=GetSelected("BusinessType", "中小企业")%> value="中小企业">中小企业</option>
              <option <%=GetSelected("BusinessType", "学校")%> value="学校">学校</option>
              <option <%=GetSelected("BusinessType", "医院")%> value="医院">医院</option>
              <option <%=GetSelected("BusinessType", "其他")%> value="其他">其他</option>
            </select>
          </td>
          <td align="right">分类：</td>
          <td colspan="3">
            <select name="Classification" id="Classification">
              <option <%=GetSelected("Classification", "直接客户", true)%> value="直接客户">直接客户</option>
              <option <%=GetSelected("Classification", "间接客户")%> value="间接客户">间接客户</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">网址：</td>
          <td>
          	<input name="Website" type="text" class="input-large" id="Website" value="<%=GetValue("Website")%>" />
          </td>
          <td align="right">电话：</td>
          <td>
            <input name="Telephone" type="text" class="input-large" id="Telephone" value="<%=GetValue("Telephone")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">所处地区：</td>
          <td colspan="3">
            <div id="province" class="fl province"></div>
          </td>
        </tr>
        <tr>
          <td align="right">详细地址：</td>
          <td colspan="3">
            <input name="Address" type="text" style="width:450px;"  id="Address" value="<%=GetValue("Address")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">客户简介：</td>
          <td colspan="3">
            <textarea name="Description" rows="4" cols="20" id="Description" style="width:100%;"><%=GetValue("Description")%></textarea>
          </td>
        </tr>
        <tr>
          <td align="right">聊天记录<br />或者<br />活动记录：</td>
          <td colspan="3"><bairong:BREditor id="ChatOrNote" runat="server"></bairong:BREditor></td>
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


  <script src="js/jquery.provincesCity.js" type="text/javascript"></script>
<script src="js/provincesdata.js" type="text/javascript"></script>
<script type="text/javascript">
$(function(){
  $("#province").ProvinceCity();
  $(".province select").attr('class', 'input-medium');
  $(".province select").css('margin-right', '5px');
  $(".province select").eq(0).val("<%=GetValue("Province")%>");
  $(".province select").eq(0).change();
  $(".province select").eq(1).val("<%=GetValue("City")%>");
  $(".province select").eq(1).change();
  $(".province select").eq(2).val("<%=GetValue("Area")%>");
});
</script>


</form>
</body>
</html>
<!-- check for 3.6 html permissions -->