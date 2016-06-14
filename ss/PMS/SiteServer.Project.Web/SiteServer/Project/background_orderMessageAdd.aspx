<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrderMessageAdd" %>

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
        <tr>
          <td align="right" width="120">消息名称：</td>
          <td>
            <input name="MessageName" type="text" id="MessageName" value="<%=GetValue("MessageName")%>" />
          </td>
        </tr>
        <tr>
          <td align="right">发送短信：</td>
          <td>
            <select name="IsSMS">
              <option <%=GetSelected("IsSMS", "True", true)%> value="True">发送</option>
              <option <%=GetSelected("IsSMS", "False")%> value="False">不发送</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">
            短信模板：
            <br />
            <code id="txtCount" style="display:none"></code>
          </td>
          <td>
          <script type="text/javascript">
          function countChar(){
            var count = $('#TemplateSMS').val().length;
            if (count > 0){
              $('#txtCount').show().html(count + ' 字');  
            }
          }
          </script>
          <textarea id="TemplateSMS" name="TemplateSMS" style="width:90%;height:60px;" id="TemplateSMS" onkeydown='countChar();' onkeyup='countChar();'><%=GetValue("TemplateSMS")%></textarea>

          </td>
        </tr>
        <tr>
          <td align="right">发送邮件：</td>
          <td>
            <select name="IsEmail">
              <option <%=GetSelected("IsEmail", "True", true)%> value="True">发送</option>
              <option <%=GetSelected("IsEmail", "False")%> value="False">不发送</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">邮件模板：</td>
          <td>
          <textarea name="TemplateEmail" style="width:90%;height:260px;" id="TemplateEmail" ><%=GetValue("TemplateEmail")%></textarea>
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