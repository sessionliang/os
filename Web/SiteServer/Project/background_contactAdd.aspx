<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundContactAdd" %>

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
          <td align="right">联系人：</td>
          <td>
            <input name="ContactName" type="text" id="ContactName" value="<%=GetValue("ContactName")%>"  />
          </td>
          <td width="80" align="right">添加人：</td>
          <td>
            <asp:Literal id="ltlAddUserName" runat="server" />
          </td>
        </tr>
        <tr>
          <td align="right">职位：</td>
          <td>
            <input name="JobTitle" type="text" id="JobTitle" value="<%=GetValue("JobTitle")%>"  />
          </td>
          <td align="right">角色：</td>
          <td>
            <select name="AccountRole" id="AccountRole">
              <option <%=GetSelected("AccountRole", "决策者")%> value="决策者">决策者</option>
              <option <%=GetSelected("AccountRole", "影响者", true)%> value="影响者">影响者</option>
              <option <%=GetSelected("AccountRole", "员工")%> value="员工">员工</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">手机：</td>
          <td>
            <input name="Mobile" type="text" size="20" id="Mobile" value="<%=GetValue("Mobile")%>" />
          </td>
          <td align="right">固定电话：</td>
          <td colspan="3">
            <input name="Telephone" type="text" size="20" id="Telephone" value="<%=GetValue("Telephone")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">邮箱：</td>
          <td>
            <input name="Email" type="text" size="20" id="Email" value="<%=GetValue("Email")%>" />
          </td>
          <td align="right">QQ号码：</td>
          <td>
            <input name="QQ" type="text" size="20" id="QQ" value="<%=GetValue("QQ")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">所属客户：</td>
          <td colspan="3">
            <div class="fill_box" id="accountIDContainer" style="display:none">
              <div class="addr_base addr_normal"> <b id="accountName"></b>
                <input id="accountID" name="accountID" value="" type="hidden">
              </div>
            </div>
            <script language="javascript">
            function selectAccountID(accountName, accountID){
                $('#accountName').html(accountName);
                $('#accountID').val(accountID);
                if (accountID == ''){
                  $('#accountIDContainer').hide();
                }else{
                    $('#accountIDContainer').show();
                }
            }
            </script>
            <asp:Literal id="ltlAccountID" runat="server" />
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
  $(".province select").eq(2).val("<%=GetValue("Area")%>");
});
</script>


</form>
</body>
</html>
<!-- check for 3.6 html permissions -->