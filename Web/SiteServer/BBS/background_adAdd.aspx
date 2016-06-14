<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundAdAdd" %>

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

  <div class="popover popover-static">
  <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="184"><bairong:help HelpText="广告名称" Text="广告名称：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox Columns="45" MaxLength="50" id="AdName" runat="server"/>
          <asp:RequiredFieldValidator
                  ControlToValidate="AdName"
                  errorMessage=" *" foreColor="red" 
                  Display="Dynamic"
                  runat="server"/>
        </td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="展现方式" Text="展现方式：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:DropDownList ID="AdType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ReFresh"></asp:DropDownList></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="是否生效" Text="是否生效：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:RadioButtonList ID="IsEnabled" RepeatDirection="Horizontal" runat="server"> </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="此广告是否存在期限" Text="是否存在时间限制：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:CheckBox id="IsDateLimited" AutoPostBack="true" OnCheckedChanged="ReFresh" Text="存在时间限制" runat="server"> </asp:CheckBox>
        </td>
      </tr>
      <tr id="StartDateRow" runat="server">
        <td width="184"><bairong:help HelpText="显示此广告的开始时间" Text="开始时间：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><bairong:DateTimeTextBox id="StartDate" Columns="30" runat="server" />
        </td>
      </tr>
      <tr id="EndDateRow" runat="server">
        <td width="184"><bairong:help HelpText="取消此广告的结束时间" Text="结束时间：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><bairong:DateTimeTextBox id="EndDate" Columns="30" runat="server" />
        </td>
      </tr>
      <asp:PlaceHolder ID="phCode" runat="server">
      <tr>
        <td><bairong:help HelpText="广告内容" Text="广告内容：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox style="height:150px; width:70%" TextMode="MultiLine" id="Code" runat="server" Wrap="false" />
        <asp:RequiredFieldValidator
                  ControlToValidate="Code"
                  errorMessage=" *" foreColor="red" 
                  Display="Dynamic"
                  runat="server"/></td>
      </tr>
      </asp:PlaceHolder>
      <asp:PlaceHolder ID="phText" runat="server" Visible="false">
      <tr>
        <td width="184"><bairong:help HelpText="文字内容" Text="文字内容：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox Columns="65" MaxLength="50" id="TextWord" runat="server"/>
          <asp:RequiredFieldValidator
                  ControlToValidate="TextWord"
                  errorMessage=" *" foreColor="red" 
                  Display="Dynamic"
                  runat="server"/>
        </td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="文字链接" Text="文字链接：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox Columns="65" MaxLength="50" id="TextLink" runat="server"/>
          <asp:RequiredFieldValidator
                  ControlToValidate="TextLink"
                  errorMessage=" *" foreColor="red" 
                  Display="Dynamic"
                  runat="server"/>
        </td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="文字颜色" Text="文字颜色：" runat="server" ></bairong:help>
        </td>
        <td width="274"><asp:TextBox Columns="25" MaxLength="50" id="TextColor" runat="server"/></td>
        <td width="114"><bairong:help HelpText="字体大小" Text="字体大小：" runat="server" ></bairong:help>
        </td>
        <td width="654"><asp:TextBox Columns="25" MaxLength="50" id="TextFontSize" runat="server"/>
        <asp:RegularExpressionValidator
          ControlToValidate="TextFontSize"
          ValidationExpression="\d+"
          Display="Dynamic"
          ErrorMessage="字体大小必须为数字"
          runat="server"/>
        </td>
      </tr>
      </asp:PlaceHolder>
      <asp:PlaceHolder ID="phImage" runat="server" Visible="false">
      <tr>
        <td width="184"><bairong:help HelpText="图片地址" Text="图片地址：" runat="server" ></bairong:help>
        </td>
        <td colspan="3">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="400"><input id="ImageUrlUploader" style="display:none; width:345px;" type="file" runat="server" />
      <asp:TextBox Columns="65" MaxLength="50" id="ImageUrl" runat="server" style="display:" /></td>
    <td rowspan="2" style="padding-left:10px;"><IMG class="preview" src='<%=GetPreviewImageSrc("Image")%>' alt=图片预览 align="absmiddle" name="preview_ImageUrl" width=88 height=70 id="preview_ImageUrl"></td>
  </tr>
  <tr>
<td valign="top"><a id="imageUrlLink1" style="" href="javascript:;" onClick="document.getElementById('imageUrlLink2').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('ImageUrlUploader').style.display = '';document.getElementById('ImageUrl').style.display = 'none'">上传图片</a>&nbsp;
      <a id="imageUrlLink2" style="font-weight:bold" href="javascript:;" onClick="document.getElementById('imageUrlLink1').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('ImageUrlUploader').style.display = 'none';document.getElementById('ImageUrl').style.display = ''">输入 URL</a></td>
    </tr>
</table>

        </td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="广告链接" Text="广告链接：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox Columns="65" MaxLength="50" id="ImageLink" runat="server"/>
          <asp:RequiredFieldValidator
                  ControlToValidate="ImageLink"
                  errorMessage=" *" foreColor="red" 
                  Display="Dynamic"
                  runat="server"/>
        </td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="图片尺寸" Text="图片尺寸：" runat="server" ></bairong:help>
        </td>
        <td colspan="3">宽度：
          <asp:TextBox Columns="10" MaxLength="50" id="ImageWidth" runat="server"/>
          <asp:RegularExpressionValidator
          ControlToValidate="ImageWidth"
          ValidationExpression="\d+"
          Display="Dynamic"
          ErrorMessage="宽度必须为数字"
          runat="server"/>
          高度：
          <asp:TextBox Columns="10" MaxLength="50" id="ImageHeight" runat="server"/>        
          <asp:RegularExpressionValidator
          ControlToValidate="ImageHeight"
          ValidationExpression="\d+"
          Display="Dynamic"
          ErrorMessage="高度必须为数字"
          runat="server"/>（0代表不限制尺寸）       </td>
        </tr>
      <tr>
        <td width="184"><bairong:help HelpText="图片替换文字" Text="图片替换文字：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><asp:TextBox Columns="65" MaxLength="50" id="ImageAlt" runat="server"/></td>
      </tr>
      </asp:PlaceHolder>
      <asp:PlaceHolder ID="phFlash" runat="server" Visible="false">
      <tr>
        <td width="184"><bairong:help HelpText="Flash地址" Text="Flash地址：" runat="server" ></bairong:help>
        </td>
        <td colspan="3"><table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="400">
      <input id="FlashUrlUploader" style="display:none; width:345px;" type="file" runat="server" />
      <asp:TextBox Columns="65" MaxLength="50" id="FlashUrl" runat="server" style="display:" />
    </td>
    <td rowspan="2" style="padding-left:10px;">
      <IMG class="preview" src='<%=GetPreviewImageSrc("Flash")%>' alt=图片预览 align="absmiddle" name="preview_FlashUrl" width=88 height=70 id="preview_FlashUrl">
    </td>
  </tr>
  <tr>
    <td valign="top">
      <a id="flashUrlLink1" style="" href="javascript:;" onClick="$('flashUrlLink2').style.fontWeight = '';this.style.fontWeight = 'bold';$('FlashUrlUploader').style.display = '';$('FlashUrl').style.display = 'none'">上传Flash</a>&nbsp;
      <a id="flashUrlLink2" style="font-weight:bold" href="javascript:;" onClick="$('flashUrlLink1').style.fontWeight = '';this.style.fontWeight = 'bold';$('FlashUrlUploader').style.display = 'none';$('FlashUrl').style.display = ''">输入 URL</a></td>
    </tr>
</table></td>
      </tr>
      <tr>
        <td width="184"><bairong:help HelpText="宽度" Text="宽度：" runat="server" ></bairong:help>
        </td>
        <td colspan="3">宽度：
          <asp:TextBox Columns="10" MaxLength="50" id="FlashWidth" runat="server"/>
          <asp:RegularExpressionValidator
          ControlToValidate="FlashWidth"
          ValidationExpression="\d+"
          Display="Dynamic"
          ErrorMessage="宽度必须为数字"
          runat="server"/>
          高度：
          <asp:TextBox Columns="10" MaxLength="50" id="FlashHeight" runat="server"/>        
          <asp:RegularExpressionValidator
          ControlToValidate="FlashHeight"
          ValidationExpression="\d+"
          Display="Dynamic"
          ErrorMessage="高度必须为数字"
          runat="server"/>（0代表不限制尺寸）       </td>
        </tr>
      </asp:PlaceHolder>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="提 交" onclick="Submit_OnClick" runat="server" />
          <input class="btn" type="button" onClick="location.href='background_ad.aspx?publishmentSystemID=<%=PublishmentSystemID%>&adLocation=<%=Request.QueryString["adLocation"]%>';return false;" value="返 回" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->