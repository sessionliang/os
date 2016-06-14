<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.AppointmentItemAdd" Trace="false" %>

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
      <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
      <bairong:Alerts runat="server"></bairong:Alerts>
      <bairong:Code type="ajaxupload" runat="server" />
      <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/swfupload.js"></script>
      <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/handlers.js"></script>
      <div class="popover-content">
      <div class="container-fluid" id="weixinactivate">
       <div class="row-fluid">

          <div class="span6">
           <table class="table noborder table-hover">
              <tr>
                <td width="120"> 预约标题：</td>
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
                <td>是否显示简介：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsDescription" runat="server" checked="true" text="显示简介" />
                   <script type="text/javascript"> $("#cbIsDescription").change(function () { checkboxChange(this); }); </script>
                </td>
              </tr>
               <tr>
                <td width="120">简介标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbDescriptionTitle" runat="server" text="活动简介" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbDescriptionTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>简介内容：</td>
                <td>
                  <asp:TextBox id="tbDescription" textMode="Multiline" class="textarea" rows="4" style="width:95%; padding:5px;" runat="server" />
                </td>
              </tr>
               <tr>
                <td>是否显示图片：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsImageUrl" runat="server" checked="true" text="显示图片" />
                   <script type="text/javascript"> $("#cbIsImageUrl").change(function () { checkboxChange(this); }); </script>
                </td>
              </tr>
               <tr>
                <td width="120">图片标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbImageUrlTitle" runat="server" text="活动现场" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbImageUrlTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>上传图片：</td>
                <td>
                    <asp:TextBox id="tbContentImageUrl" class="itemImageUrl input-xlarge" Width="180" runat="server" />
                    <span class="btn-group">
                    <asp:Literal id="ltlContentImageUrl" runat="server" />
                    </span>
                 </td>
              </tr>
                <tr>
                <td>是否显示视频：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsVideoUrl" runat="server" checked="true" text="显示视频" />
                  <script type="text/javascript"> $("#cbIsVideoUrl").change(function () { checkboxChange(this); }); </script>
                </td>
              </tr>
               <tr>
                <td width="120">视频标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbVideoUrlTitle" runat="server" text="活动视频" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbVideoUrlTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>上传视频：</td>
                <td>
                    <asp:TextBox id="tbContentVideoUrl" class="itemImageUrl input-xlarge" Width="180" runat="server" />
                    <span class="btn-group">
                    <asp:Literal id="ltlContentVideoUrl" runat="server" />
                    </span>
                 </td>
              </tr>
                <tr>
                <td>是否显示相册：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsImageUrlCollection" runat="server" checked="true" text="显示相册" />
                     <script type="text/javascript"> $("#cbIsImageUrlCollection").change(function () { 
                         checkboxChange(this);
                         if($(this).attr("checked")){
                             $("#images").show();
                         }
                         else{
                             $("#images").hide();
                         } 
                     });
                  </script>
                </td>
              </tr>
               <tr>
                <td width="120">相册标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbImageUrlCollectionTitle" runat="server" text="活动相册" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbImageUrlCollectionTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>上传照片：</td>
                <td>
                   <input type="button" class="btn" id="btnAddImageUrl" value="上传照片" />
                    <script type="text/javascript">
                        $("#btnAddImageUrl").click(function () {
                            var imageUrlCollection = $("#imageUrlCollection").val();
                            var largeImageUrlCollection = $("#largeImageUrlCollection").val();
                            openWindow('上传照片', '/siteserver/weixin/modal_appointmentItemPhotoUpload.aspx?publishmentSystemID=<%=base.PublishmentSystemID%>&imageUrlCollection=' + imageUrlCollection + '&largeImageUrlCollection=' + largeImageUrlCollection + '', 0, 0, 'false');
                        });
                    </script>
                <%-- <asp:Button class="btn" id="btnAddImageUrl" text="上传照片" runat="server"/> --%>
                </td>
              </tr>

                <tr>
                <td>是否显示地址：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsMap" runat="server" checked="true" text="显示地址" />
                   <script type="text/javascript"> $("#cbIsMap").change(function () {
                       checkboxChange(this);
                       if($(this).attr("checked")){
                          $("#mapAddress").show();
                       }
                      else {
                          $("#mapAddress").hide();
                       } 
                     });
                  </script>
                </td>
              </tr>
               <tr>
                <td width="120">地址标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbMapTitle" runat="server" text="活动地点" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbMapTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>预约地址：</td>
                <td>
                   <asp:TextBox id="tbMapAddress" class="input-xlarge" runat="server" />
                     <input type="button" id="btnMap"  class="btn" value="查看效果" />
                   <script type="text/javascript">
                       $("#btnMap").click(function () {
                           $("#map").children().remove();
                           var mapUrl = "http://map.baidu.com/mobile/webapp/place/list/qt=s&wd=" + $("#" + "<%=tbMapAddress.ClientID%>").val() + "/vt=map";
                           var iframe = $("<iframe style='width:100%;height:100%;background-color:#ffffff;margin-bottom:15px;' scrolling='auto' frameborder='0' width='100%' height='100%' src='" + mapUrl + "'></iframe>");
                           $("#map").append(iframe);
                       });
                   </script>
                  <%-- <asp:Button class="btn" id="btnMap" text="查看效果" OnClick="Preview_OnClick" runat="server"/>--%>
                </td>
              </tr>

                <tr>
                <td>是否显示电话：</td>
                <td class="checkbox">
                  <asp:CheckBox id="cbIsTel" runat="server" checked="true" text="显示电话" />
                  <script type="text/javascript"> $("#cbIsTel").change(function () { checkboxChange(this);  }); </script>
                </td>
              </tr>
               <tr>
                <td width="120">电话标题：</td>
                <td>
                  <asp:TextBox class="input-xlarge" id="tbTelTitle" runat="server" text="预约电话" />
                  <asp:RegularExpressionValidator
                    runat="server"
                    ControlToValidate="tbTelTitle"
                    ValidationExpression="[^']+"
                    errorMessage=" *" foreColor="red" 
                    Display="Dynamic" />
                </td>
              </tr>
              <tr>
                <td>电话号码：</td>
                <td>
                   <asp:TextBox id="tbTel" class="input-xlarge" runat="server" />
                    <asp:RegularExpressionValidator 
                      runat="server"
                      ControlToValidate="tbTel" 
                      ValidationExpression="^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$|([0-9]{4}-[0-9]{3}-[0-9]{3})"
                      ErrorMessage="电话格式不正确！" Display="Dynamic" ForeColor="#CC0000"> 
                   </asp:RegularExpressionValidator>
                  </td>
              </tr>
                
            </table>
          </div>
            <script type="text/javascript">
                $(document).ready(function(){
                    if($("#cbIsImageUrlCollection").attr("checked"))
                    {
                        if($("#imageUrlCollection").val().length>0){
                     
                            $("#images").show();
                        }
                    }
                    if($("#cbIsMap").attr("checked"))
                    { 
                        if($("#tbMapAddress").val().length>0)
                        {
                            $("#mapAddress").show();
                        }
                    }
                })
                function checkboxChange(checkbox)
                {
                    if ($(checkbox).attr("checked")) {
                        $(checkbox).removeAttr("checked");
                        $(checkbox).parent().parent().next('tr').hide();
                        $(checkbox).parent().parent().next('tr').next('tr').hide();
                    }
                    else {
                        $(checkbox).attr("checked","checked");
                        $(checkbox).parent().parent().next('tr').show();
                        $(checkbox).parent().parent().next('tr').next('tr').show();
                    }
                }
          </script>

          <div class="span6">
            <div class="intro-grid">
              <p><strong>预约顶部显示图片：</strong></p>
              <asp:Literal id="ltlTopImageUrl" runat="server" />
              <a id="js_topImageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
            </div>
           </div>
           <br />
          <div class="span6" id="images" style="display:none">
            <div class="intro-grid">
              <p><strong> 预约相册</strong></p>
                <div id="imageList" style="width:80%;">

                </div>
                <input type="hidden" id="imageUrlCollection" name="imageUrlCollection" runat="server" />
                <input type="hidden" id="largeImageUrlCollection" name="largeImageUrlCollection"  runat="server"/>
            </div>
           </div>
           <script type="text/javascript">
               function addImage(imageUrlCollection, largeImageUrlCollection) {
                  $("#images").show();
                  $("#imageList").children("div").remove();
                   var imageUrls = imageUrlCollection.split(",");
                   var publishmentSystemUrl = '<%=base.PublishmentSystemInfo.PublishmentSystemUrl%>';
                   for (var i = 0; i < imageUrls.length; i++) {
                       var imageUrl = imageUrls[i];
                       if (imageUrl.indexOf("@") >= 0) {
                           imageUrl = imageUrl.substring(1);
                       }
                       var image = $("<div style=' margin:4px; float:left;'><img class='img-rounded' style='width:120px;' src='" + publishmentSystemUrl + imageUrl + "'></div>");
                       $("#imageList").append(image);
                   }
                   $("#imageUrlCollection").val(imageUrlCollection);
                   $("#largeImageUrlCollection").val(largeImageUrlCollection);
               }
               <asp:Literal ID="ltlScript" runat="server"></asp:Literal>
           </script>
           
           <div class="span6" id="mapAddress" style="display:none">
            <div class="intro-grid">
              <p><strong>预约地址</strong></p>
               <div style="height:300px;" id="map" ><asp:Literal id="ltlMap" runat="server" /></div>
            </div>
           </div>
        </div>
          
        <script type="text/javascript">
            new AjaxUpload('js_topImageUrl', {
                action: '<%=GetUploadUrl()%>',
            name: "Upload",
            data: {},
            onSubmit: function (file, ext) {
                var reg = /^(jpg|jpeg|png|gif)$/i;
                if (ext && reg.test(ext)) {
                    //$('#img_upload_txt_').text('上传中... ');
                } else {
                    //$('#img_upload_txt_').text('只允许上传JPG,PNG,GIF图片');
                    alert('只允许上传JPG,PNG,GIF图片');
                    return false;
                }
            },
            onComplete: function (file, response) {
                if (response) {
                    response = eval("(" + response + ")");
                    if (response.success == 'true') {
                        $('#preview_topImageUrl').attr('src', response.url);
                        $('#topImageUrl').val(response.virtualUrl);
                    } else {
                        alert(response.message);
                    }
                }
            }
            });
      </script>
     </div>

      <input id="topImageUrl" name="topImageUrl" type="hidden" runat="server" />
      <input id="contentImageUrl" name="contentImageUrl" type="hidden" runat="server" />
    </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
