<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsolePublishmentSystemAddSaas" %>

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

  <script language="javascript">
  $(document).ready(function()
  {
    if (!isNull(_get('choose')))
    {
      var item = $(":radio:checked");
      if(item.length==0){
        if (!isNull(_get('choose').length) && _get('choose').length > 1)
        {
          _get('choose')[0].checked = true;
          document.getElementById('SiteTemplateDir').value=_get('choose')[0].value;
        }
        else
        {
          _get('choose').checked = true;
          document.getElementById('SiteTemplateDir').value=_get('choose').value;
        }
      } 
    }
  });

  function displaySiteTemplateDiv(obj)
  {
    if (obj.checked){
      document.getElementById('SiteTemplateDiv').style.display = '';
    }else{
      document.getElementById('SiteTemplateDiv').style.display = 'none';
    }
  }
  if (document.getElementById('choose ')!= null){
    if (document.getElementById('choose').length > 1){
      document.getElementById('choose')[0].checked = 'true';
      document.getElementById('SiteTemplateDir').value=document.getElementById('choose')[0].value;
    }else{
      document.getElementById('choose').checked = 'true';
      document.getElementById('SiteTemplateDir').value=document.getElementById('choose').value;
    }
  }
  </script>
  <input type="hidden" id="hihAuthPublishmentSystemID" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">
    
      <asp:PlaceHolder id="ChooseSiteTemplate" runat="server">

      <blockquote>
        <p>选择应用模板</p>
        <small>欢迎使用新建应用向导，您可以选择使用应用模板作为新建应用的基础。</small>
      </blockquote>

      <div class="well well-small">
        <table class="table table-noborder">
          <tr>
            <td>
              是否使用应用模板：
              <asp:CheckBox id="UseSiteTemplate" runat="server" Checked="true" Text="使用"> </asp:CheckBox>
            </td>
          </tr>
        </table>
      </div>

      <div id="SiteTemplateDiv">
        <input type="hidden" id="SiteTemplateDir" value="" runat="server" />
        <asp:dataList id="dlContents" DataKeyField="Name" CssClass="table table-bordered table-hover" repeatDirection="Horizontal" repeatColumns="4" runat="server">
          <ItemTemplate>
            <asp:Literal ID="ltlImageUrl" runat="server"></asp:Literal>
            <asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
            <asp:Literal ID="ltlRadio" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle cssClass="center" />
        </asp:dataList>
      </div>

      </asp:PlaceHolder>

      <asp:PlaceHolder id="CreateSiteParameters" runat="server" Visible="false">

      <blockquote>
        <p>设置应用参数</p>
        <small>在此设置新建应用的名称、文件夹以及辅助表等信息。</small>
      </blockquote>

      <table class="table table-hover table-noborder">
          <tr id="RowSiteTemplateName" runat="server">
            <td>使用的应用模板名称：</td>
            <td>
              <asp:Label ID="SiteTemplateName" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td width="160">应用名称：</td>
            <td>
              <asp:TextBox Columns="35" MaxLength="50" id="PublishmentSystemName" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="PublishmentSystemName"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic"
                runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="PublishmentSystemName"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" />
            </td>
          </tr>
          <tr>
            <td width="160">应用类型：</td>
            <td>
              <asp:Literal ID="ltlPublishmentSystemType" runat="server"/>
            </td>
          </tr>
        </table>

      </asp:PlaceHolder>

      <asp:PlaceHolder id="OperatingError" runat="server" Visible="false">

      <blockquote style="margin-top:20px;">
        <p>发生错误</p>
        <small>执行向导过程中出错</small>
      </blockquote>

      <div class="alert alert-error">
        <h4><asp:Literal ID="ltlErrorMessage" runat="server"></asp:Literal></h4>
      </div>

      </asp:PlaceHolder>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn" ID="Previous" OnClick="PreviousPlaceHolder" CausesValidation="false" runat="server" Text="< 上一步"></asp:Button>
            <asp:Button class="btn btn-primary" id="Next" onclick="NextPlaceHolder" runat="server" text="下一步 >"></asp:button>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->