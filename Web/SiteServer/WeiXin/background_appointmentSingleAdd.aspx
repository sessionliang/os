<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundAppointmentSingleAdd" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <script type="text/javascript" src="background_appointmentSingleAdd.js"></script>
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

            #ConfigTable td {
                text-align: center;
            }
        </style>

        <bairong:Code Type="ajaxupload" runat="server" />
        <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/swfupload.js"></script>
        <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/handlers.js"></script>

        <div class="popover popover-static operation-area">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" />
            </h3>
            <div class="popover-content">
                <div class="container-fluid" id="weixinactivate">

                    <asp:PlaceHolder ID="phStep1" runat="server">
                        <div class="row-fluid">

                            <div class="span6">
                                <div class="step">第一步：配置微预约开始属性</div>
                                <table class="table noborder table-hover">
                                    <tr>
                                        <td width="120">主题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbTitle" runat="server" />
                                            <asp:RequiredFieldValidator
                                                ControlToValidate="tbTitle"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic"
                                                runat="server" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>摘要：</td>
                                        <td>
                                            <asp:TextBox ID="tbSummary" TextMode="Multiline" class="textarea" Rows="4" Style="width: 95%; padding: 5px;" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>开始时间：</td>
                                        <td>
                                            <bairong:DateTimeTextBox ID="dtbStartDate" Now="true" ShowTime="true" Columns="20" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>结束时间：</td>
                                        <td>
                                            <bairong:DateTimeTextBox ID="dtbEndDate" ShowTime="true" Columns="20" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>预约状态：</td>
                                        <td class="checkbox">
                                            <asp:CheckBox ID="cbIsEnabled" runat="server" Checked="true" Text="启用预约" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>触发关键词：</td>
                                        <td>
                                            <asp:TextBox ID="tbKeywords" runat="server" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbKeywords"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                            <br>
                                            <span class="gray">多个关键词请用空格格开：例如: 微信 腾讯</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div class="span6">
                                <div class="intro-grid">
                                    <p><strong>预约进行中显示图片：</strong></p>
                                    <asp:Literal ID="ltlImageUrl" runat="server" />
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

                    <asp:PlaceHolder ID="phStep2" Visible="false" runat="server">
                        <div class="row-fluid">

                            <div class="span6">
                                <div class="step">第二步：配置微预约属性</div>
                                <table class="table noborder table-hover">
                                    <tr>
                                        <td width="120">预约标题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbItemTitle" runat="server" />
                                            <asp:RequiredFieldValidator
                                                ControlToValidate="tbItemTitle"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic"
                                                runat="server" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbItemTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>是否显示简介：</td>
                                        <td class="checkbox">
                                            <asp:CheckBox ID="cbIsDescription" runat="server" Checked="true" Text="显示简介" />
                                            <script type="text/javascript"> $("#cbIsDescription").change(function () { checkboxChange(this); }); </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">简介标题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbDescriptionTitle" runat="server" Text="活动简介" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbDescriptionTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>简介内容：</td>
                                        <td>
                                            <asp:TextBox ID="tbDescription" TextMode="Multiline" class="textarea" Rows="4" Style="width: 95%; padding: 5px;" runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>是否显示图片：</td>
                                        <td class="checkbox">
                                            <asp:CheckBox ID="cbIsImageUrl" runat="server" Checked="true" Text="显示图片" />
                                            <script type="text/javascript"> $("#cbIsImageUrl").change(function () { checkboxChange(this); }); </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">图片标题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbImageUrlTitle" runat="server" Text="活动现场" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbImageUrlTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>上传图片：</td>
                                        <td>
                                            <asp:TextBox ID="tbContentImageUrl" class="itemImageUrl input-xlarge" Width="180" runat="server" />
                                            <span class="btn-group">
                                                <asp:Literal ID="ltlContentImageUrl" runat="server" />
                                            </span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>是否显示视频：</td>
                                        <td class="checkbox">
                                            <asp:CheckBox ID="cbIsVideoUrl" runat="server" Checked="true" Text="显示视频" />
                                            <script type="text/javascript"> $("#cbIsVideoUrl").change(function () { checkboxChange(this); }); </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">视频标题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbVideoUrlTitle" runat="server" Text="活动视频" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbVideoUrlTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>上传视频：</td>
                                        <td>
                                            <asp:TextBox ID="tbContentVideoUrl" class="itemImageUrl input-xlarge" Width="180" runat="server" />
                                            <span class="btn-group">
                                                <asp:Literal ID="ltlContentVideoUrl" runat="server" />
                                            </span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>是否显示相册：</td>
                                        <td class="checkbox">
                                            <asp:CheckBox ID="cbIsImageUrlCollection" runat="server" Checked="true" Text="显示相册" />
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
                                            <asp:TextBox class="input-xlarge" ID="tbImageUrlCollectionTitle" runat="server" Text="活动相册" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbImageUrlCollectionTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>上传照片：</td>
                                        <td>
                                            <input type="button" class="btn" id="btnAddImageUrl" value="上传相册照片" />
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
                                            <asp:CheckBox ID="cbIsMap" runat="server" Checked="true" Text="显示地址" />
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
                                            <asp:TextBox class="input-xlarge" ID="tbMapTitle" runat="server" Text="活动地点" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbMapTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>预约地址：</td>
                                        <td>
                                            <asp:TextBox ID="tbMapAddress" class="input-xlarge" runat="server" />
                                            <input type="button" id="btnMap" class="btn" value="查看效果" />
                                            <script type="text/javascript">
                                                $("#btnMap").click(function () {
                                                    $("#map").children().remove();
                                                    var mapUrl = "http://map.baidu.com/mobile/webapp/place/list/qt=s&wd=" + $("#"+"<%=tbMapAddress.ClientID%>").val() + "/vt=map"; 
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
                                            <asp:CheckBox ID="cbIsTel" runat="server" Checked="true" Text="显示电话" />
                                            <script type="text/javascript"> $("#cbIsTel").change(function () { checkboxChange(this);  }); </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120">电话标题：</td>
                                        <td>
                                            <asp:TextBox class="input-xlarge" ID="tbTelTitle" runat="server" Text="预约电话" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbTelTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>电话号码：</td>
                                        <td>
                                            <asp:TextBox ID="tbTel" class="input-xlarge" runat="server" />
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
                                    <asp:Literal ID="ltlTopImageUrl" runat="server" />
                                    <a id="js_topImageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
                                </div>
                            </div>

                            <div class="span6">
                                <div class="intro-grid">
                                    <p><strong>我的预约头部图片：</strong></p>
                                    <asp:Literal ID="ltlResultTopImageUrl" runat="server" />
                                    <a id="js_resultTopImageUrl" href="javascript:;" onclick="return false;" class="btn btn-success">上传</a>
                                </div>
                            </div>

                            <div class="span6" id="images" style="display: none">
                                <div class="intro-grid">
                                    <p><strong>预约相册</strong></p>
                                    <div id="imageList" style="width: 80%;">
                                    </div>
                                    <input type="hidden" id="imageUrlCollection" name="imageUrlCollection" runat="server" />
                                    <input type="hidden" id="largeImageUrlCollection" name="largeImageUrlCollection" runat="server" />
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

                            <div class="span6" id="mapAddress" style="display: none">
                                <div class="intro-grid">
                                    <p><strong>预约地址</strong></p>
                                    <div style="height: 300px;" id="map">
                                        <asp:Literal ID="ltlMap" runat="server" />
                                    </div>
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
                        <script type="text/javascript">
                            new AjaxUpload('js_resultTopImageUrl', {
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
                                            $('#preview_resultTopImageUrl').attr('src', response.url);
                                            $('#resultTopImageUrl').val(response.virtualUrl);
                                        } else {
                                            alert(response.message);
                                        }
                                    }
                                }
                            });
                        </script>
                        <input id="topImageUrl" name="topImageUrl" type="hidden" runat="server" />
                        <input id="resultTopImageUrl" name="resultTopImageUrl" type="hidden" runat="server" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phStep3" Visible="false" runat="server">
                        <div class="row-fluid">
                            <script type="text/javascript"><asp:Literal id="ltlAwardItems" runat="server" /></script>
                            <script type="text/html" class="itemController">
                                <input id="itemCount" name="itemCount" type="hidden" value="{{itemCount}}" />
                                <div>
                                    <div class="step">第三步：配置预约提交表单</div>
                                    <table class="table noborder">
                                        <tr>
                                            <td>
                                                <table class="table noborder table-hover">
                                                    <tbody>
                                                        <tr>
                                                            <td width="160">是否显示姓名字段：</td>
                                                            <td class="checkbox">
                                                                <asp:CheckBox ID="cbIsFormRealName" runat="server" Checked="true" Text="显示" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>姓名重命名：</td>
                                                            <td>
                                                                <asp:TextBox class="input-xlarge" ID="tbFormRealNameTitle" Text="姓名" runat="server" />
                                                                <asp:RequiredFieldValidator
                                                                    ControlToValidate="tbFormRealNameTitle"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic"
                                                                    runat="server" />
                                                                <asp:RegularExpressionValidator
                                                                    runat="server"
                                                                    ControlToValidate="tbFormRealNameTitle"
                                                                    ValidationExpression="[^']+"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>是否显示手机字段：</td>
                                                            <td class="checkbox">
                                                                <asp:CheckBox ID="cbIsFormMobile" runat="server" Checked="true" Text="显示" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>手机重命名：</td>
                                                            <td>
                                                                <asp:TextBox class="input-xlarge" ID="tbFormMobileTitle" Text="手机" runat="server" />
                                                                <asp:RequiredFieldValidator
                                                                    ControlToValidate="tbFormMobileTitle"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic"
                                                                    runat="server" />
                                                                <asp:RegularExpressionValidator
                                                                    runat="server"
                                                                    ControlToValidate="tbFormMobileTitle"
                                                                    ValidationExpression="[^']+"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>是否显示邮箱字段：</td>
                                                            <td class="checkbox">
                                                                <asp:CheckBox ID="cbIsFormEmail" runat="server" Checked="true" Text="显示" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>邮箱重命名：</td>
                                                            <td>
                                                                <asp:TextBox class="input-xlarge" ID="tbFormEmailTitle" Text="邮箱" runat="server" />
                                                                <asp:RequiredFieldValidator
                                                                    ControlToValidate="tbFormEmailTitle"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic"
                                                                    runat="server" />
                                                                <asp:RegularExpressionValidator
                                                                    runat="server"
                                                                    ControlToValidate="tbFormEmailTitle"
                                                                    ValidationExpression="[^']+"
                                                                    ErrorMessage=" *" ForeColor="red"
                                                                    Display="Dynamic" />
                                                            </td>
                                                        </tr>
                                                        {{each items}}
                                                        <tr>
                                                            <td>是否显示新字段：</td>
                                                            <td class="checkbox">
                                                                <input id="cbox_{{$index}}" type="checkbox" name="itemIsVisible" class="itemIsVisible" {{$value.isVisible}}><label for="cbox_{{$index}}">显示</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>新字段重命名：
                                                            </td>
                                                            <td>
                                                                <input type="hidden" name="itemID" class="itemID" value="{{$value.id ? $value.id + '' : '0'}}">
                                                                <input type="text" name="itemAttributeName" value="{{$value.attributeName}}" class="itemAttributeName input-xlarge">
                                                                <a href="javascript:;" onclick="itemController.removeItem({{$index}});">删除</a>
                                                            </td>
                                                        </tr>
                                                        {{/each}}
                                                        <tr>
                                                            <td colspan="3" style="text-align: left">
                                                                <a href="javascript:;" onclick="itemController.addItem({})" class="btn btn-success">添加新字段</a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </script>
                        </div>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phStep4" Visible="false" runat="server">
                        <div class="row-fluid">

                            <div class="span6">
                                <div class="step">第四步：配置预约结束属性</div>
                                <table class="table noborder table-hover">
                                    <tr>
                                        <td width="120">预约结束主题：</td>
                                        <td>
                                            <asp:TextBox class="input-large" ID="tbEndTitle" runat="server" />
                                            <asp:RequiredFieldValidator
                                                ControlToValidate="tbEndTitle"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic"
                                                runat="server" />
                                            <asp:RegularExpressionValidator
                                                runat="server"
                                                ControlToValidate="tbEndTitle"
                                                ValidationExpression="[^']+"
                                                ErrorMessage=" *" ForeColor="red"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>预约结束摘要：</td>
                                        <td>
                                            <asp:TextBox ID="tbEndSummary" TextMode="Multiline" class="textarea" Rows="4" Style="width: 95%; padding: 5px;" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div class="span6">
                                <div class="intro-grid">
                                    <p><strong>预约已结束显示图片：</strong></p>
                                    <asp:Literal ID="ltlEndImageUrl" runat="server" />
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
                            <asp:Button class="btn btn-primary" ID="btnSubmit" Text="下一步" OnClick="Submit_OnClick" runat="server" />
                            <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
