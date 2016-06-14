<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.UserGroupAdd" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <asp:PlaceHolder ID="phCreditsAddTips" runat="server">
    <div class="tips"> 增加用户组时最低积分和最高积分必须在已有的某个用户组积分范围之内.<br>
      例如: 注册会员的积分范围是50 - 200 ,那么要添加的用户组的积分上下限必须在(200≥ 积分范围 >50) 或 (200> 积分范围 ≥50)之间. 如果积分范围跨越多个用户组积分范围区间的用户组时, 系统将视为无效
    </div>
  </asp:PlaceHolder>
  <asp:PlaceHolder ID="phCreditsEditTips" runat="server">
    <div class="tips"> 编辑用户组时最低积分和最高积分必须在当前用户组积分范围之内. <br>
      例如: 注册会员的积分范围是50 - 200 ,那么编辑该用户组的积分必须在50 - 200 之间. 如果积分范围跨越多个用户组积分上下限区间时, 系统将视为无效. <br>
      如果想要扩展积分的范围时可通过增加最低积分或缩小最高积分来进行调整. 
      例如: 注册会员的积分范围分别是50 - 200 , 如果想把最高积分扩展为300 ,只需调整相邻的"中级会员"组(范围为200和500) 的最低积分修改为300即可. 调整最低积分的方法与调整最高积分的方法类似
    </div>
  </asp:PlaceHolder>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="160">会员组名称：</td>
      <td>
        <asp:TextBox  id="GroupName" MaxLength="50" Size="30" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="GroupName"
          ErrorMessage=" *" foreColor="red"
          Display="Dynamic"
          runat="server"
          />
      </td>
    </tr>
    <asp:PlaceHolder ID="phCredits" runat="server">
      <tr>
        <td>最低积分：</td>
        <td>
          <asp:TextBox  id="CreditsFrom" Columns="10" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="CreditsFrom"
            ErrorMessage=" *" foreColor="red"
            Display="Dynamic"
            runat="server"
            />
          <asp:RegularExpressionValidator
            ControlToValidate="CreditsFrom"
            ValidationExpression="[0-9]+"
            Display="Dynamic"
            ErrorMessage=" *" foreColor="red"
            runat="server"/>
        </td>
      </tr>
      <tr>
        <td>最高积分：</td>
        <td>
          <asp:TextBox  id="CreditsTo" MaxLength="50" Columns="10" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="CreditsTo"
            ErrorMessage=" *" foreColor="red"
            Display="Dynamic"
            runat="server"
            />
          <asp:RegularExpressionValidator
            ControlToValidate="CreditsTo"
            ValidationExpression="[0-9]+"
            Display="Dynamic"
            ErrorMessage=" *" foreColor="red"
            runat="server"/>
        </td>
      </tr>
    </asp:PlaceHolder>
    <tr>
      <td>星星数：</td>
      <td>
        <asp:TextBox  id="Stars" MaxLength="50" Columns="10" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="Stars"
          ErrorMessage=" *" foreColor="red"
          Display="Dynamic"
          runat="server"
          />
        <asp:RegularExpressionValidator
            ControlToValidate="Stars"
            ValidationExpression="[0-9]+"
            Display="Dynamic"
            ErrorMessage=" *" foreColor="red"
            runat="server"/> (0 - 10)
      </td>
    </tr>
    <tr>
      <td>组名称颜色：</td>
      <td>
        <asp:TextBox  id="Color" MaxLength="50" Columns="15" runat="server"/>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->