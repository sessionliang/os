<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplyAccept" Trace="false"%>

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
<bairong:alerts runat="server" text="受理办件后信息将变为待办理状态"></bairong:alerts>

  <table class="table table-noborder table-hover">
      <tr>
        <td class="center" width="120">意见：</td>
        <td><asp:TextBox ID="tbAcceptRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
      </tr>
      <tr>
        <td class="center">负责人：</td>
        <td><div class="fill_box" id="selectToContainer" style="display:none">
            <div class="addr_base addr_normal"> <b id="selectToUserDisplayName"></b>
              <input id="selectToUserName" name="selectToUserName" value="" type="hidden">
            </div>
          </div>
          <div ID="divSelectToUserName" class="btn_pencil" runat="server"><span class="pencil"></span>　选择</div>
          <script language="javascript">
      function selectToUserName(displayName, userName){
          $('#selectToUserDisplayName').html(displayName);
          $('#selectToUserName').val(userName);
          if (userName == ''){
            $('#selectToContainer').hide();
          }else{
              $('#selectToContainer').show();
          }
      }
      </script></td>
      </tr>
      <tr>
        <td class="center">截止日期：</td>
        <td><bairong:DateTimeTextBox id="tbAcceptEndDate" Columns="20" runat="server" /></td>
      </tr>
      <tr>
        <td class="center">受理部门：</td>
        <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td class="center">受理人：</td>
        <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
      </tr>
    </table>

</form>
</body>
</html>