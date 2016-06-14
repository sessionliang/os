<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundContentTeleplayUpload" %>

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

        <bairong:Code Type="ajaxupload" runat="server" />
        <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/swfupload.js"></script>
        <script type="text/javascript" src="../../sitefiles/bairong/scripts/swfUpload/handlers.js"></script>

        <script type="text/javascript">
            function uploadSuccess(file, response) {
                try {
                    if (response) {
                        response = eval("(" + response + ")");
				 
                        if (response.success == 'true') {
                            add_form();
                            var $count = $('#Teleplay_Count');
                            var index = parseInt($count.val());
                            $("#title_"+index).val(response.file);
                            $("#StillUrl_"+index).val(response.url);
                        } else {
                            alert(response.message);
                        }
                    }
                } catch (ex) {
                    this.debug(ex);
                }
            }

            var swfu;
            $(document).ready(function(){
                swfu = new SWFUpload({
                    // Backend Settings
                    upload_url: "<%=GetContentTeleplayUploadMultipleUrl()%>",

                    // File Upload Settings
                    file_size_limit : "100 MB",
                    file_types : "*.swf;*.flv;*.mp4;",
                    file_types_description : "Images",
                    file_upload_limit : 0,    // Zero means unlimited

                    // Event Handler Settings - these functions as defined in Handlers.js
                    //  The handlers are not part of SWFUpload but are part of my website and control how
                    //  my website reacts to the SWFUpload events.
                    swfupload_preload_handler : preLoad,
                    swfupload_load_failed_handler : loadFailed,
                    file_queue_error_handler : fileQueueError,
                    file_dialog_complete_handler : fileDialogComplete,
                    upload_error_handler : uploadError,
                    upload_success_handler : uploadSuccess,
                    upload_complete_handler : uploadComplete,

                    // Button settings
                    button_image_url : "../../sitefiles/bairong/scripts/swfUpload/button.png",
                    button_placeholder_id : "swfUploadPlaceholder",
                    button_width: 114,
                    button_height: 22,
                    button_text : '» 批量添加视频',
                    button_text_top_padding: 1,
                    button_text_left_padding: 10,

                    // Flash Settings
                    flash_url : "../../sitefiles/bairong/scripts/swfUpload/swfupload.swf",	// Relative to this file
                    flash9_url : "../../sitefiles/bairong/scripts/swfUpload/swfupload_FP9.swf",	// Relative to this file

                    // Debug Settings
                    debug: false
                });
            });
        </script>

        <div class="popover popover-static">
            <h3 class="popover-title">上传电视剧集</h3>
            <div class="popover-content">

                <div id="contents">
                    <table border="0" cellspacing="5" cellpadding="5" width="95%">
                        <tr>
                            <td colspan="2">
                                <input id="Teleplay_Count" type="hidden" name="Teleplay_Count" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <table width="240" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td><span id="swfUploadPlaceholder"></span></td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">&nbsp;</td>
                        </tr>
                    </table>
                </div>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" OnClick="Submit_OnClick" Text="确 定" runat="server" />
                            <asp:Button class="btn" ID="Return" CausesValidation="false" OnClick="Return_OnClick" Text="返 回" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div id="Teleplay_0" style="display: none">
            <table class="table table-noborder">
                <tr>
                    <td>
                        <input type="text" id="title_0" name="title_0" value="" /><span id="preview_0"></span>
                        <div><a id="uploadFile_0" href="javascript:void(0);">» 上传</a></div>
                        <span id="video_upload_txt_0" style="clear: both; font-size: 12px; color: #FF3737;"></span>
                        <input type="hidden" id="ID_0" name="ID_0" value="" />
                        <input type="hidden" id="StillUrl_0" name="StillUrl_0" value="" />
                    </td>
                    <td>
                        <table cellpadding="5">
                            <tr>
                                <td>剧集说明:</td>
                                <td>
                                    <textarea id="Description_0" name="Description_0" style="width: 350px; height: 66px;"></textarea></td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: bottom">
                        <a id="Up_0">上升</a>
                    </td>
                    <td style="vertical-align: bottom">
                        <a id="Down_0">下降</a>
                    </td>
                    <td style="vertical-align: bottom">
                        <a href="javascript:;" onclick="remove_form('#Teleplay_0');">删除剧集</a>
                    </td>
                </tr>
            </table>

            <hr />
        </div>

        <script type="text/javascript">
            var ajaxUploadUrl = '<%=GetContentTeleplayUploadSingleUrl()%>';

            function add_form(id, url, description, title, upUrl, downUrl, previewClick){
                var $count = $('#Teleplay_Count');
                var count = parseInt($count.val());
                count = count + 1;
                var $el = $("<div id='Teleplay_" + count + "'>" + $('#Teleplay_0').html().replace(/_0/g, '_' + count) + "</div>");
                $el.insertBefore($count);	
                $('#Teleplay_Count').val(count);
                add_ajaxUpload(count);
		
                if (id && id > 0){
                    $('#ID_' + count).val(id);
                    $("#title_"+count).val(title);  
                    $("#preview_"+count).html(previewClick);
                    $('#StillUrl_'+count).val(url);
                    $('#Description_' + count).val(description);
                    $('#Up_'+count).attr("href",upUrl);
                    $('#Down_'+count).attr("href",downUrl);
                }
            }

            function remove_form(divID){
                $(divID).remove();
            }

            function add_ajaxUpload(index){
                new AjaxUpload('uploadFile_' + index, {
                    action: ajaxUploadUrl,
                    name: "VideoUrl",
                    data: {},
                    onSubmit: function(file, ext) {
                        var reg = /^(swf|flv|mp4)$/i;
                        if (ext && reg.test(ext)) {
                            $('#video_upload_txt_' + index).text('上传中... ');
                        } else {
                            $('#video_upload_txt_' + index).text('只允许上传swf,flv,mp4视频');
                            return false;
                        }
                    },
                    onComplete: function(file, response) {
                        $('#video_upload_txt_' + index).text(' ');
                        if (response) {
                            response = eval("(" + response + ")");
                            if (response.success == 'true') {
                                //$("#videoTeleplay_" + index).attr('src', response.url);
                                $("#title_"+index).val(response.file);//默认剧集名称为文件名
                                $('#StillUrl_'+count).val(response.url);
                            } else {
                                $('#video_upload_txt_' + index).text(response.message);
                            }
                        }
                    }
                });	
            }

            <asp:Literal ID="ltlScript" runat="server"></asp:Literal>

        </script>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
