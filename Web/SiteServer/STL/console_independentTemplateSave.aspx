<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsoleIndependentTemplateSave" %>

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

  <bairong:Code type="ajaxupload" runat="server" />
  <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/swfupload.js"></script>
  <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/handlers.js"></script>

  <div class="popover popover-static">
    <h3 class="popover-title">保存独立模板</h3>
    <div class="popover-content">
    
      <asp:PlaceHolder id="phWelcome" runat="server" Visible="false">

        <blockquote>
          <p>欢迎使用保存为独立模板向导</p>
          <small>您能够将此站点的指定模板以及相关文件保存在独立模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">独立模板名称：</td>
            <td><asp:TextBox Columns="35" MaxLength="50" id="IndependentTemplateName" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="IndependentTemplateName"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic"
                runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="IndependentTemplateName"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" /></td>
          </tr>
          <tr>
            <td width="200">独立模板文件夹名称：</td>
            <td><asp:TextBox Columns="25" MaxLength="500" id="IndependentTemplateDir" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="IndependentTemplateDir"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic"
                runat="server"/>
              <asp:RegularExpressionValidator 
                 ControlToValidate="IndependentTemplateDir" ValidationExpression="^IN_.+" 
                 errorMessage=" *" foreColor="red" display="Dynamic" runat="server"/>
              （文件名必须以IN_开头，且以英文或拼音取名） </td>
          </tr>
          <tr>
            <td width="200">独立模板网站地址：</td>
            <td><asp:TextBox Columns="50" MaxLength="200" id="WebSiteUrl" runat="server"/>
              <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="WebSiteUrl"
                ValidationExpression="[^']+"
                errorMessage=" *" foreColor="red" 
                Display="Dynamic" /></td>
          </tr>
          <tr>
            <td width="200">独立模板介绍：</td>
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
          <small>点击下一步将站点的文件保存到独立模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">指定保存的文件及文件夹：</td>
            <td><asp:CheckBoxList ID="cblDirectoriesAndFiles" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phSaveTemplates" runat="server" Visible="false">

        <blockquote>
          <p>保存模板</p>
          <small>点击下一步将站点的模板保存到独立模板中。</small>
        </blockquote>

        <table class="table table-noborder table-hover">
          <tr>
            <td width="200">首页模板：</td>
            <td><asp:CheckBoxList ID="cblIndexTemplates" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
          <tr>
            <td>栏目模板：</td>
            <td><asp:CheckBoxList ID="cblChannelTemplates" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
          <tr>
            <td>内容模板：</td>
            <td><asp:CheckBoxList ID="cblContentTemplates" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
          <tr>
            <td>文件模板：</td>
            <td><asp:CheckBoxList ID="cblFileTemplates" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server"></asp:CheckBoxList></td>
          </tr>
        </table>

      </asp:PlaceHolder>

      <asp:PlaceHolder id="phSaveSiteContents" runat="server" Visible="false">

        <blockquote style="margin-top:20px;">
          <p>保存站点内容</p>
          <small>点击下一步将站点的栏目及内容信息保存到站点模板中。</small>
        </blockquote>

        <table class="table table-noborder">
          <tr>
            <td width="250" valign="top">
              指定保存的栏目：
              <br />
              <span>从下边选择需要保存的栏目，所选栏目的上级栏目将自动保存到站点模板中</span>
            </td>
            <td><asp:Literal id="ltlChannelTree" runat="server" /></td>
          </tr>
          <tr>
            <td width="200">是否保存内容数据：</td>
            <td><asp:RadioButtonList ID="rblIsSaveContents" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
          </tr>
        </table>

      </asp:PlaceHolder>
      
      <asp:PlaceHolder id="phUploadImageFile" runat="server" Visible="false">
        
        <blockquote>
          <p>载入样图文件</p>
          <small>选择样图文件的名称</small>
        </blockquote>

        <table class="table table-noborder">
          <tr>
            <td width="150">选择样图文件：</td>
            <td>
              <a id="btn_upload" href="javascript:void(0);" onclick="return false;" class="btn btn-success">上传</a>
              <input type="hidden" id="imageUrls" name="imageUrls" value="">
              <hr />
              <p id="imageUrlContainer"></p>
            </td>
          </tr>
        </table>

      </asp:PlaceHolder>
      <asp:PlaceHolder id="phDone" runat="server" Visible="false">

        <blockquote>
          <p>独立模板保存成功</p>
          <small>您已经完成保存独立模板的操作。</small>
        </blockquote>

        <div class="alert alert-success">
          <h4>独立模板保存在"SiteFiles\IndependentTemplates\<%=IndependentTemplateDir.Text%>"文件夹中。</h4>
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

  <script type="text/javascript">
  var imageUrlList = [];
  function deleteImageUrl(e, virtualUrl){
    imageUrlList.splice($.inArray(virtualUrl,imageUrlList),1);
    $('#imageUrls').val(imageUrlList.join(','));
    $(e).parent().remove();
  }
  new AjaxUpload('btn_upload', {
     action: '<%=GetUploadUrl()%>',
     name: "Upload",
     data: {},
     onSubmit: function(file, ext) {
       var reg = /^(jpg|jpeg|png|gif)$/i;
       if (ext && reg.test(ext)) {
         //$('#img_upload_txt_').text('上传中... ');
       } else {
         //$('#img_upload_txt_').text('只允许上传JPG,PNG,GIF图片');
         alert('只允许上传JPG,PNG,GIF图片');
         return false;
       }
     },
     onComplete: function(file, response) {
       if (response) {
         response = eval("(" + response + ")");
         if (response.success == 'true') {
           var element = $('<span style="margin-right:15px;"><img src="' + response.url + '" width="64" style="vertical-align: text-bottom;max-height: 100px;margin-right:5px;"><a href="javascript:;" onclick="deleteImageUrl(this);return false;">删除</a></span>');
           $('#imageUrlContainer').append(element);
           imageUrlList.push(response.virtualUrl);
           $('#imageUrls').val(imageUrlList.join(','));
         } else {
           alert(response.message);
         }
       }
     }
    });
  </script>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->