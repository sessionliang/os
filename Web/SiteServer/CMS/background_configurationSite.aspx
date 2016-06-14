<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationSite" %>
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
  <script>
    $(document).ready(function(){
      $('#myTab a').click(function (e) {
        e.preventDefault();
        changeTab($(this).attr('index'));
      });
      <%=GetChangeTabFunction()%>
    });
    function changeTab(index){
      $('#index').val(index);
      $($('#myTab a').get(index)).tab('show');
    }
  </script>

  <input type="hidden" id="index" name="index" value="0" />

  <ul class="nav nav-pills" id="myTab">
    <li class="active"><a href="#basic" index="0">基本设置</a></li>
    <li><a href="#advance" index="1">高级设置</a></li>
  </ul>

  <div class="tab-content">
    <div class="tab-pane active" id="basic">
    <table class="table table-bordered table-hover">
        <tr>
          <td width="200">站点名称：</td>
          <td>
            <asp:TextBox Columns="25" MaxLength="50" id="PublishmentSystemName" runat="server" class="input-xlarge" />
            <asp:RequiredFieldValidator ControlToValidate="PublishmentSystemName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="PublishmentSystemName" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          </td>
        </tr>
        <site:AuxiliaryControl ID="acAttributes" runat="server"/>
      </table>
    </div>
    <div class="tab-pane" id="advance">
      <table class="table table-bordered table-hover">
        <tr>
          <td width="200"></td>
          <td class="right"><asp:Literal id="ltlSettings" runat="server" /></td>
        </tr>
        <asp:PlaceHolder id="phVisutalType" runat="server">
        <tr>
          <td >访问方式：</td>
          <td><asp:RadioButtonList ID="IsStaticVisutalType" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
        </tr>
        </asp:PlaceHolder>
        <tr>
          <td>网页编码：</td>
          <td>
            <asp:DropDownList id="Charset" runat="server"></asp:DropDownList>
            <br>
            <span>模板编码将同步修改</span>
          </td>
        </tr>
        <tr>
          <td>信息每页显示数目：</td>
          <td>
            <asp:TextBox Columns="25" Text="18" MaxLength="50" id="PageSize" class="input-mini" runat="server"/>
            <asp:RequiredFieldValidator
              ControlToValidate="PageSize"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/> <span>条</span>
           </td>
        </tr>
        <tr>
          <td>是否统计内容总点击量：</td>
          <td>
            <asp:RadioButtonList ID="IsCountHits" AutoPostBack="true" OnSelectedIndexChanged="IsCountHits_SelectedIndexChanged" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
            <span>需要重新生成页面</span>
          </td>
        </tr>
        <asp:PlaceHolder ID="phIsCountHitsByDay" runat="server">
          <tr>
            <td>是否统计内容日/周/月点击量：</td>
            <td><asp:RadioButtonList ID="IsCountHitsByDay" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
          </tr>
        </asp:PlaceHolder>
        <tr>
          <td>是否统计文件下载量：</td>
          <td><asp:RadioButtonList ID="IsCountDownload" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList></td>
        </tr>
        <tr>
          <td>是否启用双击生成页面：</td>
          <td>
            <asp:RadioButtonList ID="IsCreateDoubleClick" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
            <span class="gray">此功能通常用于制作调试期间，网站开发期间建议启用</span>
          </td>
        </tr>
      </table>
    </div>
  </div>

  <hr />
  <table class="table noborder">
    <tr>
      <td class="center">
        <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->