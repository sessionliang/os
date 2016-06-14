<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.SpecSelect" Trace="false"%>

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
<bairong:alerts text="请点击对应规格，添加本商品需要的规格" runat="server"></bairong:alerts>

	<script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/scripts/independent/validate.js"></script>
	<style>
	.photos img{width:22px; height:22px; margin:2px;}
	</style>
	<script>
	var trCount = <%=GetSelectedCount()%>;
	$(document).ready(function(){
		
		$('.specItem div').click(function(){
			var obj = $(this);
			var trID = 'specItem_' + obj.attr("itemID");
			if(obj.attr('isSelected') == 'true'){
				obj.attr('isSelected', 'false');
				obj.removeClass("cur");
				if ($('#'+trID).length > 0){
					$('#'+trID).insertBefore($("#specHeaderRow"));
					$("input[name='selectedIsHide']", '#'+trID).val("True");
					$('#'+trID).hide();
					trCount--;
				}
			}else{
				obj.attr('isSelected', 'true');
				obj.addClass("cur");
				if ($('#'+trID).length == 0){
					var itemID = obj.attr("itemID");
					var itemTitle = obj.attr("itemTitle");
					var iconUrl = '';
					var iconClickString = obj.attr("iconClickString");
					var selectPhotosClickString = obj.attr("selectPhotosClickString");
					var imageUrl = $("img", obj).attr('src');
					var html = '<tr id="' + trID + '" class="tdbg"><td class="center">';
					html += '<input type="hidden" name="selectedItemID" value="' + itemID + '" />';
					html += '<input type="hidden" name="selectedIconUrl" id="selectedIconUrl_' + itemID + '" value="' + iconUrl + '" />';
					html += '<input type="hidden" name="selectedPhotoIDCollection" id="selectedPhotoIDCollection_' + itemID + '" value="" />';
					html += '<input type="hidden" name="selectedIsHide" value="False" />';
					if (<%=GetIsIcon()%>){
						html += '<img src="' + imageUrl + '" alt="' + itemTitle + '" /></td>';
					}else{
						html += itemTitle + '</td>';
					}
					html += '<td><input type="text" name="selectedTitle" value="' + itemTitle + '" size="20" /></td>';
					if (<%=GetIsIcon()%>){
						html += '<td class="center"><img id="preview_' + itemID + '" style="display:none" align="absmiddle" />&nbsp;&nbsp;<a href="javascript:;" onClick="' + iconClickString + '">设 置</a>';
					}
					html += '<td class="center"><span id="urls_' + itemID + '"></span><a href="javascript:;" onclick="' + selectPhotosClickString + '">选 择</a></td>';
					html += '<td class="center"><a href="javascript:;" onclick="taxis(this, true);return false;"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></a></td>';
					html += '<td class="center"><a href="javascript:;" onclick="taxis(this, false);return false;"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></a></td>';
					html += '</tr>';
					$("#specHeaderRow").before(html);
				}else{
					$('#'+trID).insertBefore($("#specHeaderRow"));
					$("input[name='selectedIsHide']", '#'+trID).val("False");
					$('#'+trID).show();
				}
				trCount++;
			}
		});
	});

	function taxis(a, isUp){
		if (isUp){
			var tr = $(a).parent().parent();
			if (tr.index() != 1) {
				tr.insertBefore(tr.prev());
			}
		}else{
			var tr = $(a).parent().parent();
			if (tr.index() != trCount) {
				tr.insertAfter(tr.next());
			}
		}
	}
	</script>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
	      <td align="right" width="100"><%=GetSpecName()%>：</td>
	      <td><div class="specItem">
	          <asp:Literal ID="ltlSpecItems" runat="server"></asp:Literal>
	        </div></td>
	    </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="60">系统<%=GetSpecName()%></td>
      <td>自定义<%=GetSpecName()%>值</td>
      <%if (GetIsIcon() == "true"){%>
      <td width="120">自定义<%=GetSpecName()%>图片</td>
	  	<%}%>
      <td width="120">关联商品相册图片</td>
      <td class="center" width="30">&nbsp;</td>
      <td class="center" width="30">&nbsp;</td>
    </tr>
    <asp:Repeater ID="rptSelected" runat="server">
      <itemtemplate>
        <tr id="specItem_<%#DataBinder.Eval(Container.DataItem,"ItemID")%>">
          <td class="center">
            <input name="selectedItemID" type="hidden" value="<%#DataBinder.Eval(Container.DataItem,"ItemID")%>" />
            <input name="selectedIconUrl" id="selectedIconUrl_<%#DataBinder.Eval(Container.DataItem,"ItemID")%>" type="hidden" value="<%#GetSystemSpecItemIconUrl((string)DataBinder.Eval(Container.DataItem,"IconUrl"), (int)DataBinder.Eval(Container.DataItem,"ItemID"))%>" />
            <input name="selectedPhotoIDCollection" id="selectedPhotoIDCollection_<%#DataBinder.Eval(Container.DataItem,"ItemID")%>" type="hidden" value="<%#ParsePhotoIDCollection((string)DataBinder.Eval(Container.DataItem,"PhotoIDCollection"))%>" />
            <input name="selectedIsHide" type="hidden" value="False" />
            <%#GetSystemSpecItemHTML((int)DataBinder.Eval(Container.DataItem,"ItemID"))%>
          </td>
          <td><input name="selectedTitle" type="text" size="20" value="<%#DataBinder.Eval(Container.DataItem,"Title")%>" /></td>
          <%if (GetIsIcon() == "true"){%>
          <td class="center"><img id="preview_<%#DataBinder.Eval(Container.DataItem,"ItemID")%>" align="absMiddle" src="<%#ParseIconUrl((string)DataBinder.Eval(Container.DataItem,"IconUrl"))%>" style="display:<%#string.IsNullOrEmpty((string)DataBinder.Eval(Container.DataItem,"IconUrl")) ? "none" : ""%>" />&nbsp;&nbsp;<a href="javascript:;" onClick="<%#GetIconClickString("selectedIconUrl_" + (int)DataBinder.Eval(Container.DataItem,"ItemID"), "preview_" + (int)DataBinder.Eval(Container.DataItem,"ItemID"), (string)DataBinder.Eval(Container.DataItem,"IconUrl"))%>">设 置</a></td>
		  		<%}%>
          <td class="center"><span id="urls_<%#DataBinder.Eval(Container.DataItem,"ItemID")%>" class="photos"><%#ParsePhotoIDCollectionHTML((string)DataBinder.Eval(Container.DataItem,"PhotoIDCollection"))%></span><a href="javascript:;" onClick="<%#GetSelectPhotosClickString("selectedPhotoIDCollection_" + (int)DataBinder.Eval(Container.DataItem,"ItemID"), "urls_" + (int)DataBinder.Eval(Container.DataItem,"ItemID"))%>">选 择</a></td>
          <td class="center"><a onClick="taxis(this, true);return false;" href="javascript:;"><img alt="上升" src="../Pic/icon/up.gif" border="0" /></a></td>
          <td class="center"><a onClick="taxis(this, false);return false;" href="javascript:;"><img alt="下降" src="../Pic/icon/down.gif" border="0" /></a></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
    <tr id="specHeaderRow" style="display:none"></tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->