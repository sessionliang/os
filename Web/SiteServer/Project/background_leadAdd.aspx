<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundLeadAdd" %>

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
          <td align="right">状态：</td>
          <td>
            <select name="Status" id="Status">
              <%=GetOptions("Status")%>
            </select>
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
          <td align="right">来源：</td>
          <td>
            <select name="Source" id="Source">
               <option <%=GetSelected("Source", "电话呼入")%>  value="电话呼入">电话呼入</option>
              <option <%=GetSelected("Source", "百度商桥")%> value="百度商桥">百度商桥</option>
              <option <%=GetSelected("Source", "官网申请")%> value="官网申请">官网申请</option>
              <option <%=GetSelected("Source", "阿里云")%> value="阿里云">阿里云</option>
              <option <%=GetSelected("Source", "其他")%> value="其他">其他</option>
            </select>
          </td>
          <td align="right">优先级：</td>
          <td>
            <select name="Priority" id="Priority">
              <option <%=GetSelected("Priority", "1", true)%> value="1">普通</option>
              <option <%=GetSelected("Priority", "2")%> value="2">优先</option>
              <option <%=GetSelected("Priority", "3")%> value="3">紧急重要</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">可能性：</td>
          <td colspan="3">
            <input name="Possibility" type="text" id="Possibility" value="<%=GetValue("Possibility")%>" class="input-mini" /> <span>%</span>
          </td>
        </tr>
        <tr class="info"><td colspan="4" class="center">
        
        <div style="position:absolute; left:45%; margin-top:-8px;"><h5>客户信息</h5></div>

        <div class="pull-right">
          <label class="checkbox">
            <input type="checkbox" id="IsAccount" name="IsAccount" value="True" <%=GetChecked("IsAccount", "True")%>> 保存为客户
          </label>  
        </div>
        </td></tr>
        <tr><td colspan="4">
          
          <table class="table noborder">
            <tr>
              <td align="right">客户名称：</td>
              <td colspan="3">
                <input name="AccountName" type="text" style="width:450px;" id="AccountName" value="<%=GetValue("AccountName")%>" />
              </td>
             </tr>
             <tr>
             <td align="right">行业：</td>
              <td>
                <select name="BusinessType" id="BusinessType">
                  <option <%=GetSelected("BusinessType", "未知", true)%> value="未知">未知</option>
                  <option <%=GetSelected("BusinessType", "省级政府")%> value="省级政府">省级政府</option>
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
              <td align="right">网址：</td>
              <td>
                <input name="Website" type="text" size="20" id="Website" value="<%=GetValue("Website")%>" />
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
          </table>

        </td></tr>
        
        <tr class="info"><td colspan="4" class="center lead">

          <div style="position:absolute; left:45%; margin-top:-5px;"><h5>联系人信息</h5></div>

            <div class="pull-right">
              <label class="checkbox">
                <input type="checkbox" id="IsContact" name="IsContact" value="True"> 保存为联系人
              </label>
              <a class="btn" href="javascript:;" onclick="addContact(true);">新增联系人</a>
            </div>

            <script language="javascript">
            var i = 1;
            function addContact(isContact, contactID, contactName, jobTitle, accountRole, mobile, telephone, email, qq){
                var tableID = 'contact_' + i++;
                var html = $('#contact_REP').html();
                html = html.replace(/_REP/g, "");
                if (i > 2){
                  html = "<hr />" + html;
                }
                html = '<table class="table noborder" id="' + tableID + '">' + html + '</table>';
                $(html).insertBefore('#contactEnd');
                $('#' + tableID).find('#contactID').val(contactID);
                $('#' + tableID).find('#ContactName').val(contactName);
                $('#' + tableID).find('#JobTitle').val(jobTitle);
                $('#' + tableID).find('#AccountRole').val(accountRole);
                $('#' + tableID).find('#Mobile').val(mobile);
                $('#' + tableID).find('#Telephone').val(telephone);
                $('#' + tableID).find('#Email').val(email);
                $('#' + tableID).find('#QQ').val(qq);
                if (isContact){
                  $('#IsContact').attr("checked", 'checked');
                  $('#IsContact').hide(); 
                }
            }
            $(document).ready(function(){
              <asp:Literal id="ltlContactScript" runat="server" />
            });
            </script>
        </td></tr>
        <tr><td colspan="4">
          
            <table class="table noborder hide" id="contact_REP">
              <tr>
                <td align="right">联系人：</td>
                <td colspan="3">
                  <input id="contactID_REP" name="contactID_REP" type="hidden" value="" />
                  <input id="ContactName_REP" name="ContactName_REP" type="text" style="width:450px;" />
                </td>
              </tr>
              <tr>
                <td align="right">职位：</td>
                <td>
                  <input id="JobTitle_REP" name="JobTitle_REP" type="text" value=""  />
                </td>
                <td align="right">角色：</td>
                <td>
                  <select id="AccountRole_REP" name="AccountRole_REP">
                    <option value="决策者">决策者</option>
                    <option value="影响者">影响者</option>
                    <option value="员工">员工</option>
                  </select>
                </td>
              </tr>
              <tr>
                <td align="right">手机：</td>
                <td>
                  <input id="Mobile_REP" name="Mobile_REP" type="text" size="20" />
                </td>
                <td align="right">固定电话：</td>
                <td>
                  <input id="Telephone_REP" name="Telephone_REP" type="text" size="20" />
                </td>
              </tr>
              <tr>
                <td align="right">邮箱：</td>
                <td>
                  <input id="Email_REP" name="Email_REP" type="text" size="20" />
                </td>
                <td align="right">QQ号码：</td>
                <td>
                  <input id="QQ_REP" name="QQ_REP" type="text" size="20"  />
                </td>
              </tr>
            </table>
            <span id="contactEnd"></span>
        </td></tr>
        
        <tr class="info"><td colspan="4" class="center">
          <div style="position:absolute; left:45%; margin-top:-8px;"><h5>线索信息</h5></div>&nbsp;
        </td></tr>
        <tr>
          <td align="right">背景信息：</td>
          <td colspan="3">
            <textarea name="BackgroundInfo" rows="4" cols="20" id="BackgroundInfo" style="width:100%;"><%=GetValue("BackgroundInfo")%></textarea>
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