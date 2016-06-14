<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.BackgroundMobanAdd" %>

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
          <td align="right">模板编号：</td>
          <td>
            <input name="SN" type="text" id="SN" value="<%=GetValue("SN")%>" />
          </td>
          <td align="right">组别：</td>
          <td colspan="3">
            <select name="Category" id="Category">
              <option <%=GetSelected("Category", "W1", true)%> value="W1">2013内部制作（W1）</option>
              <option <%=GetSelected("Category", "V4")%> value="V4">2013模板大赛（V4）</option>
              <option <%=GetSelected("Category", "V3")%> value="V3">2012模板大赛（V3）</option>
              <option <%=GetSelected("Category", "V2")%> value="V2">2011模板大赛（V2）</option>
              <option <%=GetSelected("Category", "V1")%> value="V1">2010模板大赛（V1）</option>
              <option <%=GetSelected("Category", "V0")%> value="V0">2010之前模板（V0）</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">所属行业：</td>
          <td>
            <select name="Industry" id="Industry">
              <option <%=GetSelected("Industry", "地产/金融/投资", true)%> value="地产/金融/投资">地产/金融/投资</option>
              <option <%=GetSelected("Industry", "科技/IT电子")%> value="科技/IT电子">科技/IT电子</option>
              <option <%=GetSelected("Industry", "医药/健康产业")%> value="医药/健康产业">医药/健康产业</option>
              <option <%=GetSelected("Industry", "制造/工业产品")%> value="制造/工业产品">制造/工业产品</option>
              <option <%=GetSelected("Industry", "传媒/广告/公关策划")%> value="传媒/广告/公关策划">传媒/广告/公关策划</option>
              <option <%=GetSelected("Industry", "时尚/生活/娱乐")%> value="时尚/生活/娱乐">时尚/生活/娱乐</option>

              <option <%=GetSelected("Industry", "商业贸易/物流")%> value="商业贸易/物流">商业贸易/物流</option>
              <option <%=GetSelected("Industry", "教育/培训/咨询")%> value="教育/培训/咨询">教育/培训/咨询</option>
              <option <%=GetSelected("Industry", "服饰/化妆/美容")%> value="服饰/化妆/美容">服饰/化妆/美容</option>
              <option <%=GetSelected("Industry", "食品/餐饮/酒类")%> value="食品/餐饮/酒类">食品/餐饮/酒类</option>
              <option <%=GetSelected("Industry", "服务行业")%> value="服务行业">服务行业</option>
              <option <%=GetSelected("Industry", "装饰/设计")%> value="装饰/设计">装饰/设计</option>
              <option <%=GetSelected("Industry", "事业单位/组织机构")%> value="事业单位/组织机构">事业单位/组织机构</option>
              <option <%=GetSelected("Industry", "其它")%> value="其它">其它</option>
            </select>
          </td>
          <td align="right">颜色：</td>
          <td>
            <select name="Color" id="Color">
              <option <%=GetSelected("Color", "灰色系", true)%> value="灰色系">灰色系</option>
              <option <%=GetSelected("Color", "蓝色系")%> value="蓝色系">蓝色系</option>
              <option <%=GetSelected("Color", "绿色系")%> value="绿色系">绿色系</option>
              <option <%=GetSelected("Color", "黄色系")%> value="黄色系">黄色系</option>
              <option <%=GetSelected("Color", "红色系")%> value="红色系">红色系</option>
              <option <%=GetSelected("Color", "紫色系")%> value="紫色系">紫色系</option>
            </select>
          </td>
        </tr>
        <tr>
          <td align="right">上线时间：</td>
          <td colspan="3">
            <input name="AddDate" type="text" class="input-large" id="AddDate" value="<%=GetValue("AddDate")%>" />
          </td>
        </tr>
        
        <tr>
          <td align="right">备注：</td>
          <td colspan="3">
          <textarea name="Summary" style="width:90%;height:60px;" id="Summary" ><%=GetValue("Summary")%></textarea>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" OnClick="Submit_OnClick" runat="server" />
            <asp:Button class="btn" id="Return" text="返 回" OnClick="Return_OnClick" CausesValidation="false" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->