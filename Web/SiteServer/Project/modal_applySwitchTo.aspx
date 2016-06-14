<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplySwitchTo" Trace="false"%>

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
<bairong:alerts runat="server" text="转办后办件将由对应负责人进行处理"></bairong:alerts>

  <table class="table table-noborder table-hover">
      <tr>
        <td class="center" width="120">转办到：</td>
        <td><div class="fill_box" id="switchToContainer" style="display:none">
            <div class="addr_base addr_normal"> <b id="switchToUserDisplayName"></b>
              <input id="switchToUserName" name="switchToUserName" value="0" type="hidden">
            </div>
          </div>
          <div ID="divSwitchToUserName" class="btn_pencil" runat="server"><span class="pencil"></span>　选择</div>
          <script language="javascript">
      function switchToUserName(displayName, userName){
          $('#switchToUserDisplayName').html(displayName);
          $('#switchToUserName').val(userName);
          if (userName == ''){
            $('#switchToContainer').hide();
          }else{
              $('#switchToContainer').show();
          }
      }
      </script></td>
      </tr>
      <tr>
        <td class="center">意见：</td>
        <td><asp:TextBox ID="tbSwitchToRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
      </tr>
      <tr>
        <td class="center">操作部门：</td>
        <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td class="center">操作人：</td>
        <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
      </tr>
    </table>

</form>
</body>
</html>
