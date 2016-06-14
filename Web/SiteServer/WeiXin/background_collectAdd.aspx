<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundCollectAdd" %>

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

  <style type="text/css">
  div.step {
  font-weight: bold;
  font-size: 16px;
  margin-bottom: 10px;
  }

  span.activate_title {
  line-height: 34px;
  font-size: 16px;
  color: #333;
  }

  p.activate_desc {
  width: 100%;
  margin-left: 32px;
  font-size: 13px;
  font-weight: bold;
  }

  div.step_one, div.step_two, div.step_three {
    display: inline-block;
    margin-left: 30px;
    width: 280px;
    height: 190px;
    background: transparent url("images/weixin-activate.png") no-repeat;
  }
  div.step_two, div.step_three {
    margin-top: 20px;
  }
  div.step_one {
    background-position: -40px -48px;
  }
  div.step_two {
    background-position: -395px -48px;
  }
  div.step_three {
    background-position: -760px -48px;
  }
  </style>

  <bairong:Code type="ajaxupload" runat="server" />
  <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/swfupload.js"></script>
  <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/handlers.js"></script>

  <div class="popover popover-static operation-area">
    <h3 class="popover-title">
      <asp:Literal id="ltlPageTitle" runat="server" />
    </h3>
    <div class="popover-content">
      <div class="container-fluid" id="weixinactivate">

      <asp:PlaceHolder id="phStep1" runat="server">
        <div class="row-fluid">

          <div class="span6">
            <div class="step">第一步：配置征集开始属性</div>
            <table class="table noborder table-hover">
              <tr>
                <td width="120">主题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbTitle" runat="server"/>
                  <asp:RequiredFieldValidator
                    ControlToValidate="tbTitle"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic"
                    runat="server"/>
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>摘要：</td>
                <td>
                  <asp:TextBox id="tbSummary" textMode="Multiline" class="textarea" rows="4" style="width:95%; padding:5px;" runat="server" />
                </td>
              </tr>
              <tr>
                <td>开始时间：</td>
                <td>
                  <bairong:DateTimeTextBox id="dtbStartDate" now="true" showTime="true" Columns="20" runat="server" />
                </td>
              </tr>
              <tr>
                <td>结束时间：</td>
                <td>
                  <bairong:DateTimeTextBox id="dtbEndDate" showTime="true" Columns="20" runat="server" />
                </td>
              </tr>
              <tr>
                <td>征集状态：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsEnabled" runat="server" checked="true" text="启用征集" />
                </td>
              </tr>
              <tr>
                <td>触发关键词：</td>
                <td>
                  <asp:TextBox id="tbKeywords" runat="server" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbKeywords"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                    <br>
                  <span class="gray">多个关键词请用空格格开：例如: 微信 腾讯</span> 
                </td>
              </tr>
            </table>
          </div>

          <div class="span6">
            <div class="intro-grid">
              <p><strong>征集进行中显示图片：</strong></p>
              <asp:Literal id="ltlImageUrl" runat="server" />
              <a id="js_imageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
            </div>
          </div>
        
        </div>

        <script type="text/javascript">
        new AjaxUpload('js_imageUrl', {
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
               $('#preview_imageUrl').attr('src', response.url);
               $('#imageUrl').val(response.virtualUrl);
             } else {
               alert(response.message);
             }
           }
         }
        });
        </script>
      </asp:PlaceHolder>

      <asp:PlaceHolder id="phStep2" visible="false" runat="server">
        <div class="row-fluid">

          <div class="span6">
            <div class="step">第二步：配置征集详情页</div>
            <table class="table noborder table-hover">
              <tr>
                <td width="140">征集规则说明：</td>
                <td>
                  <asp:TextBox id="tbContentDescription" textMode="Multiline" class="textarea" rows="4" style="width:95%; padding:5px;" runat="server" />
                </td>
              </tr>
              <tr>
                <td>最多征集项数：</td>
                <td>
                  <asp:TextBox id="tbContentMaxVote" class="input-mini" text="1" runat="server" />
                  <asp:RequiredFieldValidator
                    ControlToValidate="tbContentMaxVote"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic"
                    runat="server"/>
                    <br>
                  <span class="gray">0代表不限制，1代表单选，以此类推</span> 
                </td>
              </tr>
              <tr>
                <td>是否审核征集作品：</td>
                <td>                  
                  <asp:DropDownList id="ddlContentIsCheck" class="input-medium" runat="server"></asp:DropDownList>
                </td>
              </tr>
            </table>
          </div>

          <div class="span6">
            <div class="intro-grid">
              <p><strong>征集详情页头部图片：</strong></p>
              <asp:Literal id="ltlContentImageUrl" runat="server" />
              <a id="js_contentImageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
            </div>
          </div>

          <div class="clearfix"></div>

        </div>

        <script type="text/javascript">
        new AjaxUpload('js_contentImageUrl', {
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
               $('#preview_contentImageUrl').attr('src', response.url);
               $('#contentImageUrl').val(response.virtualUrl);
             } else {
               alert(response.message);
             }
           }
         }
        });
        </script>
      </asp:PlaceHolder>

      <asp:PlaceHolder id="phStep3" visible="false" runat="server">
        <div class="row-fluid">

          <div class="span6">
            <div class="step">第三步：配置征集结束属性</div>
            <table class="table noborder table-hover">
              <tr>
                <td width="120">征集结束主题：</td>
                <td>
                  <asp:TextBox class="input-large" id="tbEndTitle" runat="server"/>
                  <asp:RequiredFieldValidator
                    ControlToValidate="tbEndTitle"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic"
                    runat="server"/>
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbEndTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>征集结束摘要：</td>
                <td>
                  <asp:TextBox id="tbEndSummary" textMode="Multiline" class="textarea" rows="4" style="width:95%; padding:5px;" runat="server" />
                </td>
              </tr>
            </table>
          </div>

          <div class="span6">
            <div class="intro-grid">
              <p><strong>征集已结束显示图片：</strong></p>
              <asp:Literal id="ltlEndImageUrl" runat="server" />
              <a id="js_endImageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
            </div>
          </div>

        </div>

        <script type="text/javascript">
        new AjaxUpload('js_endImageUrl', {
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
               $('#preview_endImageUrl').attr('src', response.url);
               $('#endImageUrl').val(response.virtualUrl);
             } else {
               alert(response.message);
             }
           }
         }
        });
        </script>
      </asp:PlaceHolder>

      </div>

      <input id="imageUrl" name="imageUrl" type="hidden" runat="server" />
      <input id="contentImageUrl" name="contentImageUrl" type="hidden" runat="server" />
      <input id="endImageUrl" name="endImageUrl" type="hidden" runat="server" />
  
      <hr />
      <table class="table table-noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="btnSubmit" text="下一步" OnClick="Submit_OnClick" runat="server"/>
            <asp:Button class="btn" id="btnReturn" text="返 回" runat="server"/>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->