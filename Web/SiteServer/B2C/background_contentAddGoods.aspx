<%@ Page Language="C#" ValidateRequest="false" EnableEventValidation="false" Inherits="SiteServer.B2C.BackgroundPages.BackgroundContentAddGoods" %>

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
    <form id="myForm" class="form-inline" enctype="multipart/form-data" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/scripts/independent/validate.js"></script>
        <script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/jquery/jquery.form.js"></script>
        <script src="../../sitefiles/bairong/jquery/jscolor/jscolor.js"></script>
        <script type="text/javascript" charset="utf-8" src="../cms/js/contentAdd.js"></script>

        <script language="javascript" type="text/javascript">
            function getBrands(){
                var nodeID = $('option:selected', $('#BrandNodeID')).attr('value');
                if (nodeID){
                    var url = '../../SiteFiles/Services/B2C/PageService.aspx?type=GetBrands&publishmentSystemID=<%=base.PublishmentSystemID%>&nodeID=' + nodeID;
          $.get(url, '', function(data){
              data = eval('(' + data + ')');
              var $sel = $('#BrandContentID');
              $('option', $sel).each(function(){
                  $(this).remove();
              })
              $sel.append('<option value=""><<请选择>></option>');
              var show = false;
              $.each(data, function(i, item){
                  show = true;
                  $opt = $('<option value="' + item.value + '">' + item.text + '</option>');
                  $opt.appendTo($sel);
              });
          });
      }else{
          var $sel = $('#BrandContentID');
          $('option', $sel).each(function(){
              $(this).remove();
          })
          $sel.append('<option value=""><<请选择>></option>');
      }
  }
        </script>

        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">

                <table id="column1" class="table table-fixed noborder" style="position: relative; top: -30px;">
                    <tr>
                        <td width="100">&nbsp;</td>
                        <td></td>
                        <td width="100"></td>
                        <td></td>
                    </tr>

                    <site:AuxiliaryControl ID="acAttributes" runat="server" />

                    <asp:PlaceHolder ID="phBrand" runat="server">
                        <tr>
                            <td>所属品牌：</td>
                            <td colspan="3">栏目：<asp:DropDownList ID="BrandNodeID" runat="server"></asp:DropDownList>&nbsp;
          品牌：<asp:DropDownList ID="BrandContentID" runat="server"></asp:DropDownList></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phBrandFilter" runat="server">
                        <tr>
                            <td>所属品牌：</td>
                            <td colspan="3"><asp:DropDownList ID="BrandContent" runat="server"></asp:DropDownList></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phContentAttributes" runat="server">
                        <tr>
                            <td>内容属性：</td>
                            <td colspan="3">
                                <asp:CheckBoxList CssClass="checkboxlist" ID="ContentAttributes" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server" /></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phContentGroup" runat="server">
                        <tr>
                            <td>所属内容组：</td>
                            <td colspan="3">
                                <asp:CheckBoxList CssClass="checkboxlist" ID="ContentGroupNameCollection" RepeatDirection="Horizontal" class="noborder" RepeatColumns="5" runat="server" /></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phTags" runat="server">
                        <tr>
                            <td>内容标签：</td>
                            <td colspan="3">
                                <asp:TextBox ID="Tags" MaxLength="50" Width="380" runat="server" />
                                &nbsp;<span>请用空格或英文逗号分隔</span>
                                <asp:Literal ID="ltlTags" runat="server"></asp:Literal></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phStatus" runat="server">
                        <tr>
                            <td>状态：</td>
                            <td colspan="3">
                                <asp:RadioButtonList CssClass="radiobuttonlist" ID="ContentLevel" RepeatDirection="Horizontal" class="noborder" runat="server" /></td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phTranslate" runat="server">
                        <tr>
                            <td>转移到：</td>
                            <td colspan="3">
                                <div class="fill_box" id="translateContainer" style="display: "></div>
                                <input id="translateCollection" name="translateCollection" value="" type="hidden">
                                <div id="divTranslateAdd" class="btn_pencil" runat="server"><span class="pencil"></span>选择</div>
                                <span id="translateType" style="padding-left: 5px; display: none">
                                    <asp:DropDownList ID="ddlTranslateType" class="input-small" runat="server"></asp:DropDownList>
                                </span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>

                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" itemIndex="1" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <input class="btn btn-info" type="button" onclick="submitPreview();" value="预 览" />
                            <%if (!string.IsNullOrEmpty(ReturnUrl))
                                {%>
                            <input class="btn" type="button" onclick="location.href='<%=ReturnUrl%>    ';return false;" value="返 回" />
                            <%}%>
                            <br>
                            <span class="gray">提示：按CTRL+回车可以快速提交</span>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
