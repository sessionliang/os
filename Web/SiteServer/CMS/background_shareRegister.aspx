<%@ Page Language="C#"  Inherits="SiteServer.CMS.BackgroundPages.BackgroundShareRegister" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
    <h3 class="popover-title">bShare分享插件设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
            <td colspan="2">bShare不止是一个分享按钮。bShare是全球中文互联网最强大的社交分享引擎！
</td>

        </tr>
        <tr>
            <td colspan="2">bShare智能分享引擎让您的用户可以轻松地将最喜欢的内容分享到社交网站、微博上与好友分享。用户无须离开您的网站，就能快速地进行分享，继续浏览您的网站！

</td>
        </tr>
        <tr>
            <td colspan="2" style="color:#cc0a0a;font-weight:bold">您尚未开通bShare,请先开通bShare服务</td>
        </tr>
        <tr>
            <td align="right" >网站域名:</td>
            <td><asp:TextBox ID="domain" runat="server"></asp:TextBox><span style="color:Red">*</span>(例如:siteserver.cn)
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="domain" Display="Dynamic" ErrorMessage="请输入域名"></asp:RequiredFieldValidator>
            
            </td>
        </tr>
         <tr>
            <td align="right" >账号类型:</td>
            <td>
        
            <input type="radio" value="新注册" name="num" checked="checked"  />新注册
    <input type="radio" value="已经有账号" name="num" />已经有账号
            </td>
        </tr>
         <tr>
            <td align="right" >用户名: </td>   
            <td><asp:TextBox ID="email" runat="server"></asp:TextBox><span style="color:Red">*</span>(E-mail地址)
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="email" Display="Dynamic" ErrorMessage="请输入用户名"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ErrorMessage="请输入正确的邮箱地址" 
                    
                    ControlToValidate="email" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ></asp:RegularExpressionValidator>
            </td>
        </tr>
         <tr>
            <td align="right" >密码:</td>
            <td ><asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox> <span style="color:Red">*</span>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="password" Display="Dynamic" ErrorMessage="请输入密码"></asp:RequiredFieldValidator></td>
        </tr>
         <tr>
            <td align="right" >确定密码:</td>
            <td><asp:TextBox ID="repPassword" runat="server" TextMode="Password"></asp:TextBox><span style="color:Red">*</span>
          
            <asp:CompareValidator ID="compwd" runat="server" ControlToCompare="password" 
                    ControlToValidate="repPassword" ErrorMessage="两次输入密码不一致"></asp:CompareValidator>
            </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button ID="btnOpenShare" class="btn btn-primary" runat="server" Text="开通bShare服务" OnClick="btnOpenShare_Click" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->