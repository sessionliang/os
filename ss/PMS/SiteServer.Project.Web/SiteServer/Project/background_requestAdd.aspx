<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundRequestAdd" %>

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

      <table class="table noborder">
        <tr>
          <td width="80" align="right">主题：</td>
          <td colspan="3">
            <input name="Subject" type="text" id="Subject" value="<%=GetValue("Subject")%>" style="width:450px;" />
            </td>
        </tr>
        <tr>
          <td align="right">问题类型：</td>
          <td>
            <select name="RequestType" id="RequestType"><%=GetRequestTypeOptions()%></select>
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
        <tr class="info"><td colspan="4" class="center">
        
        <div style="position:absolute; left:45%; margin-top:-8px;"><h5>联系方式</h5></div>&nbsp;
        </td></tr>
        <tr><td colspan="4">
          
          <table class="table noborder">
            <tr>
             <td align="right">客户：</td>
              <td colspan="3">
                <input id="accountID" name="accountID" value="<%=GetValue("AccountID")%>" type="hidden">
                <a id="accountName" href="javascript:;" onclick="<%=GetValue("SelectAccount")%>" class="btn btn-info"><%=GetValue("AccountName")%></a>
                <script language="javascript">
                function selectAccount(accountName, accountID){
                    $('#accountID').val(accountID);
                    $('#accountName').html(accountName);
                }
                </script>
              </td>
            </tr>
            <tr>
             <td align="right">网址：</td>
              <td>
                <input name="Website" type="text" size="20" id="Website" value="<%=GetValue("Website")%>" />
              </td>
              <td align="right">邮箱：</td>
              <td>
                <input name="Email" type="text" size="20" id="Email" value="<%=GetValue("Email")%>" />
              </td>
            </tr>
            <tr>
             <td align="right">手机：</td>
              <td>
                <input name="Mobile" type="text" size="20" id="Mobile" value="<%=GetValue("Mobile")%>" />
              </td>
              <td align="right">QQ：</td>
              <td>
                <input name="QQ" type="text" size="20" id="QQ" value="<%=GetValue("QQ")%>" />
              </td>
            </tr>
          </table>

        </td></tr>
        
        <tr class="info"><td colspan="4" class="center">
          <div style="position:absolute; left:45%; margin-top:-8px;"><h5>反馈信息</h5></div>&nbsp;
        </td></tr>
        <tr>
          <td align="right">问题描述：</td>
          <td colspan="3"><bairong:BREditor id="Content" runat="server"></bairong:BREditor></td>
        </tr>
        <tr>
          <td align="right"></td>
          <td colspan="3">
              <div style="position:relative;overflow:hidden;">
                  <a class="btn" style="margin:0px 15px 0 0;" href="javascript:;">添加附件</a> 附件大小不超过2M！
                  <input type="file" name="attachment" style="position:absolute;left:-160px;cursor:pointer;top:0px;z-index:999;opacity:0;filter:alpha(opacity=0);"/>
                  <span></span>
              </div>
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