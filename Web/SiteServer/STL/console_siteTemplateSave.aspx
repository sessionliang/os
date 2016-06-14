<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsoleSiteTemplateSave" %>

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
    <h3 class="popover-title">保存站点模板</h3>
    <div class="popover-content">
    
      <asp:PlaceHolder id="phWelcome" runat="server" Visible="false">

        <blockquote>
          <p>欢迎使用保存为站点模板向导</p>
          <small>您能够将此站点的站点文件、站点内容、模板、菜单显示方式、采集规则、投票信息等保存在站点模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">站点模板名称：</td>
            <td><asp:TextBox Columns="35" MaxLength="50" id="SiteTemplateName" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="SiteTemplateName"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic"
                runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="SiteTemplateName"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" /></td>
          </tr>
          <tr>
            <td width="200">站点模板文件夹名称：</td>
            <td><asp:TextBox Columns="25" MaxLength="500" id="SiteTemplateDir" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="SiteTemplateDir"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic"
                runat="server"/>
              <asp:RegularExpressionValidator 
                 ControlToValidate="SiteTemplateDir" ValidationExpression="^T_.+" 
                 errorMessage=" *" foreColor="red" display="Dynamic" runat="server"/>
              （文件名必须以T_开头，且以英文或拼音取名） </td>
          </tr>
          <tr>
            <td width="200">站点模板网站地址：</td>
            <td><asp:TextBox Columns="50" MaxLength="200" id="WebSiteUrl" runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="WebSiteUrl"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" /></td>
          </tr>
          <tr>
            <td width="200">站点模板介绍：</td>
            <td><asp:TextBox Columns="50" Rows="4" TextMode="MultiLine" id="Description" runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="Description"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" /></td>
          </tr>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phSaveFiles" runat="server" Visible="false">

        <blockquote>
          <p>保存站点文件</p>
          <small>点击下一步将站点的文件保存到站点模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">是否保存全部文件：</td>
            <td><asp:RadioButtonList ID="rblIsSaveAllFiles" AutoPostBack="true" OnSelectedIndexChanged="rblIsSaveAllFiles_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
          </tr>
          <asp:PlaceHolder ID="phDirectoriesAndFiles" runat="server" Visible="false">
          <tr>
            <td width="200">指定保存的文件及文件夹：</td>
            <td><asp:CheckBoxList ID="cblDirectoriesAndFiles" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
          </asp:PlaceHolder>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phSaveSiteContents" runat="server" Visible="false">

        <blockquote style="margin-top:20px;">
          <p>保存站点内容</p>
          <small>点击下一步将站点的栏目及内容信息保存到站点模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">是否保存内容数据：</td>
            <td><asp:RadioButtonList ID="rblIsSaveContents" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
          </tr>
          <tr>
            <td width="200">是否保存全部栏目：</td>
            <td><asp:RadioButtonList ID="rblIsSaveAllChannels" AutoPostBack="true" OnSelectedIndexChanged="rblIsSaveAllChannels_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
          </tr>
          <asp:PlaceHolder ID="phChannels" runat="server" Visible="false">
          <tr>
            <td width="200" valign="top">
              指定保存的栏目：
              <br />
              <span>从下边选择需要保存的栏目，所选栏目的上级栏目将自动保存到站点模板中</span>
            </td>
            <td><asp:Literal id="ltlChannelTree" runat="server" /></td>
          </tr>
          </asp:PlaceHolder>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phSaveSiteStyles" runat="server" Visible="false">

        <blockquote>
          <p>保存站点信息</p>
          <small>点击下一步将站点信息（包括模板、辅助表、菜单显示方式、采集规则及投票信息等）保存到站点模板中。</small>
        </blockquote>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phUploadImageFile" runat="server" Visible="false">
        
        <blockquote>
          <p>载入样图文件</p>
          <small>选择样图文件的名称</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="150">选择样图文件：</td>
            <td><input type=file id=SamplePicFile size="35" runat="server"/></td>
          </tr>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phDone" runat="server" Visible="false">

        <blockquote>
          <p>站点模板保存成功</p>
          <small>您已经完成保存站点模板的操作。</small>
        </blockquote>

        <div class="alert alert-success">
          <h4>站点模版保存在"SiteFiles\SiteTemplates\<%=SiteTemplateDir.Text%>"文件夹中。</h4>
        </div>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phOperatingError" runat="server" Visible="false">

        <blockquote>
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
            <asp:Button class="btn" ID="Previous" OnClick="PreviousPanel" runat="server" Text="< 上一步"></asp:Button>
            <asp:Button class="btn btn-primary" id="Next" onclick="NextPanel" runat="server" text="下一步 >"></asp:button>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->