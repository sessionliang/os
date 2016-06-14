<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserConfigRegister" %>

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
  <h3 class="popover-title">用户注册设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="200">允许新用户注册：</td>
        <td>
          <asp:RadioButtonList ID="IsRegisterAllowed" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
          <span class="gray">选择否将禁止新用户注册, 但不影响过去已注册的会员的使用</span>
        </td>
      </tr>
      <tr>
        <td>注册用户名最小长度：</td>
        <td>
          <asp:TextBox ID="tbRegisterUserNameMinLength" class="input-mini" runat="server"></asp:TextBox>
          <span class="gray">0代表不限制</span>
        </td>
      </tr>
      <tr>
        <td>注册密码限制：</td>
        <td>
          <asp:DropDownList ID="ddlRegisterPasswordRestriction" runat="server"></asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td>允许同一Email注册不同用户：</td>
        <td>
          <asp:RadioButtonList ID="IsEmailDuplicated" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
          <span class="gray">如果选择否，一个 Email 地址只能注册一个用户名</span>
        </td>
      </tr>
      <tr>
        <td>用户名称保留关键字：</td>
        <td>
          <asp:TextBox ID="ReservedUserNames" TextMode="MultiLine" Width="360" Height="60" runat="server"></asp:TextBox>
          <br /><span class="gray">使用&ldquo;,&rdquo;分隔多个用户名</span>
        </td>
      </tr>
      <tr>
        <td>新用户注册验证：</td>
        <td>
          <asp:DropDownList ID="RegisterVerifyType" AutoPostBack="true" OnSelectedIndexChanged="RegisterType_SelectedIndexChanged" runat="server"></asp:DropDownList>
          <br />
          <span class="gray">选择&quot;无验证&quot;用户可直接注册成功;选择&quot;Email 验证&quot;将向用户注册 Email 发送一封验证邮件以确认邮箱的有效性;选择&quot;人工审核&quot;将由管理员人工逐个确定是否允许新用户注册</span>
        </td>
      </tr>
      <tr>
        <td>注册成功欢迎消息内容：</td>
        <td><asp:TextBox ID="RegisterWelcome" TextMode="MultiLine" Width="360" Height="60" runat="server"></asp:TextBox></td>
      </tr>
      <asp:PlaceHolder ID="phVerifyMailContent" runat="server">
        <tr>
          <td>Email验证邮件内容：</td>
          <td>
            <asp:TextBox ID="RegisterVerifyMailContent" TextMode="MultiLine" Width="95%" Height="200" runat="server"></asp:TextBox>
            <br />
            <span class="gray">[UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间，[VerifyUrl]代表用户注册验证地址，邮件内容必须包含[VerifyUrl]</span>
          </td>
        </tr>
      </asp:PlaceHolder>
      <tr>
        <td>同一IP注册间隔限制：</td>
        <td>
          <asp:TextBox class="input-mini" MaxLength="10" id="tbRegisterMinHoursOfIPAddress" runat="server" /> 小时
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterMinHoursOfIPAddress" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span>同一IP在本时间间隔内将只能注册一个帐号，0 为不限制</span>
        </td>
      </tr>
       <tr>
        <td>发送欢迎信息：</td>
        <td>
          <asp:DropDownList ID="ddlRegisterWelcomeType" AutoPostBack="true" OnSelectedIndexChanged="RegisterType_SelectedIndexChanged" runat="server"></asp:DropDownList>
          <br>
          <span>可选择是否自动向新注册用户发送一条欢迎信息</span>
      </td>
      </tr>
      <asp:PlaceHolder ID="phRegisterWelcome" runat="server">
       <tr>
        <td>欢迎信息标题：</td>
        <td>
          <asp:TextBox Columns="60" id="tbRegisterWelcomeTitle" runat="server" Text="" />  
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterWelcomeTitle" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span>系统发送的欢迎信息的标题</span>
        </td>
      </tr>
      <tr>
        <td>欢迎信息内容：</td>
        <td>
          <asp:TextBox TextMode="MultiLine" Width="90%" style="height:160px;" MaxLength="500" id="tbRegisterWelcomeContent" runat="server" Text="" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterWelcomeContent" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span>系统发送的欢迎信息的内容，[UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间</span>
        </td>
      </tr>
      </asp:PlaceHolder>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->