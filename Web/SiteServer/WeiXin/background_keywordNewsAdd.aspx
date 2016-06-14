<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundKeywordNewsAdd" %>

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

  <link rel="stylesheet" type="text/css" href="css/keywordListAdd.css">
  <script type="text/javascript">
    $(document).ready(function(){
      $('#js_add_appmsg').click(function(){
        var html = $('#empty_item').html();
        $('#js_appmsg_preview').append(html);
        document.getElementById("resource").src = $('#empty_item .edit_gray').attr('href');
      });
    });
    var redirect = function(redirectUrl)
    {
    window.location.href = redirectUrl;
    }
    var deleteItem = function(e){
      $(e).parent().parent().remove();
    }
    var contentSelect = function(title, nodeID, contentID, pageUrl, imageUrl, imageSrc, summary){
      document.getElementById("resource").contentWindow.contentSelect(title, nodeID, contentID, pageUrl, imageUrl, imageSrc, summary);
    };
    var selectChannel = function(nodeNames, nodeID, pageUrl){
      document.getElementById("resource").contentWindow.selectChannel(nodeNames, nodeID, pageUrl);
    };
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">
      <asp:Literal id="ltlPageTitle" runat="server" />
    </h3>
    <div class="popover-content">
    
      <!-- start -->
<div class="main_bd">
  <div class="media_preview_area">

    <asp:PlaceHolder id="phSingle" runat="server">
    <div class="appmsg  editing">
      <div class="appmsg_content">
        <div class="js_appmsg_item ">
          <h4 class="appmsg_title">
            <asp:Literal id="ltlSingleTitle" runat="server" />
          </h4>
          <div class="appmsg_info">
            <em class="appmsg_date">
            </em>
          </div>
          <div class="appmsg_thumb_wrp">
            <asp:Literal id="ltlSingleImageUrl" runat="server" />
          </div>
          <p class="appmsg_desc">
            <asp:Literal id="ltlSingleSummary" runat="server" />
          </p>
        </div>
      </div>
    </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder id="phMultiple" runat="server">
    <div class="appmsg multi editing">
      <div id="js_appmsg_preview" class="appmsg_content">
        <div class="js_appmsg_item ">
          <div class="appmsg_info">
            <em class="appmsg_date">
            </em>
          </div>
          <div class="cover_appmsg_item">
            <h4 class="appmsg_title">
              <asp:Literal id="ltlMultipleTitle" runat="server" />
            </h4>
            <div class="appmsg_thumb_wrp">
              <asp:Literal id="ltlMultipleImageUrl" runat="server" />
            </div>
            <div class="appmsg_edit_mask">
              <asp:Literal id="ltlMultipleEditUrl" runat="server" />
            </div>
          </div>
        </div>

        <asp:Repeater ID="rptMultipleContents" runat="server">
          <itemtemplate>
            <div class="appmsg_item js_appmsg_item ">
              <asp:Literal id="ltlImageUrl" runat="server" />
              <h4 class="appmsg_title">
                <asp:Literal id="ltlTitle" runat="server" />
              </h4>
              <div class="appmsg_edit_mask">
                <asp:Literal id="ltlEditUrl" runat="server" />
                <asp:Literal id="ltlDeleteUrl" runat="server" />
              </div>
            </div>
          </itemtemplate>
        </asp:Repeater>

        <!-- start -->
        <div id="empty_item" style="display:none">
          <div class="appmsg_item js_appmsg_item">
            <img class="js_appmsg_thumb appmsg_thumb" src="">
            <i class="appmsg_thumb default">
              缩略图
            </i>
            <h4 class="appmsg_title">
              <a onclick="return false;" href="javascript:void(0);" target="_blank">
                标题
              </a>
            </h4>
            <div class="appmsg_edit_mask">
              <asp:Literal id="ltlItemEditUrl" runat="server" />
              <a class="icon18_common del_gray js_del" href="javascript:;" onclick="deleteItem(this)">&nbsp;&nbsp;</a>
            </div>
          </div>
        </div>
        <!-- end -->
      </div>
      <div class="appmsg_add">
        <a onclick="return false;" id="js_add_appmsg" href="javascript:void(0);">
          &nbsp;
          <i class="icon24_common add_gray" style="margin-top:25px;">
            增加一条
          </i>
        </a>
      </div>
    </div>
    </asp:PlaceHolder>

    <div class="tool_area">
      <div class="tool_bar">
        <input type="button" value="返 回" onclick="javascript:location.href='background_keywordNews.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>    '" class="btn">
        <!-- <span id="js_preview" class="btn btn_input btn_primary">
          <asp:Literal id="ltlPreview" runat="server" />
        </span> -->
      </div>
    </div>

  </div>
  <div id="js_appmsg_editor" class="media_edit_area">

    <asp:Literal id="ltlIFrame" runat="server" />

  </div>

</div>
      <!-- end -->
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->