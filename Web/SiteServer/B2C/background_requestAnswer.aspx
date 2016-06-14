<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundRequestAnswer"%>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls"%>
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

    <div class="well well-small">
      <h5>
        主题：<asp:Literal id="ltlPageTitle" runat="server" />
      </h5>
    </div>
    <style type="text/css">
      .q_detail { clear: both; overflow: hidden; padding: 10px; position: relative;
      margin-top:15px; } .q_detail ul{ overflow: hidden; padding: 5px 0; } .q_detail
      ul li { clear: both; color: #999999; overflow: hidden; padding: 5px 10px;
      vertical-align: middle; } .q_content { background-color: #FFFFFF; background-image:
      -moz-linear-gradient(center bottom , #F0F0F0, #FFFFFF); border: 1px solid
      #E0E0E0; border-radius: 6px 6px 6px 6px; box-shadow: 0 1px 2px #E0E0E0;
      position: relative; } .q_detail .q_content.left { background-color: #F3FBFF;
      background-image: -moz-linear-gradient(center bottom , #E6F7FF, #F3FBFF);
      border: 1px solid #D8E1EA; float: left; } .arrow_blue, .arrow_gray { background:
      url("../pic/bubble_arrow.gif") no-repeat scroll 0 0 transparent; height:
      14px; width: 8px; } .arrow_blue{ left: -8px; position: absolute; top: 10px;
      } .arrow_gray { background-position: 0 -50px; position: absolute; right:
      -8px; top: 10px; } .q_detail li .content_tips { padding: 10px 10px 0;color:#999999;
      } .left_float {float: left;} .right {float: right;} .q_content p { height:
      auto; max-width: 335px; width:350px; padding: 8px 10px; margin:0; color:#676767;
      text-align: left; } .continue_ask{ margin-top:15px; }
    </style>

    <table class="table table-bordered">
      <tr>
        <td width="65%">

          <div class="q_detail">
            <ul>

              <asp:Repeater ID="rptContents" runat="server">
                <itemtemplate>
                  <li><asp:Literal id="ltlContent" runat="server" /></li>
                </itemtemplate>
              </asp:Repeater>

              <bairong:sqlPager id="spContents" runat="server" class="table table-pager"
              />

            </ul>
          </div>

        </td>
        <td>
          <table class="table table-bordered table-striped">
            <tr class="thead info">
              <td colspan="2">
                工单详情
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  编号：
                </div>
              </td>
              <td>
                <%=GetValue( "SN")%>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  状态：
                </div>
              </td>
              <td>
                <code><asp:Literal id="ltlStatus" runat="server" /></code>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  主题：
                </div>
              </td>
              <td>
                <%=GetValue( "Subject")%>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  问题类型：
                </div>
              </td>
              <td>
                <%=GetValue( "RequestType")%>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  网址：
                </div>
              </td>
              <td>
                <asp:Literal id="ltlWebsite" runat="server" />
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  邮箱：
                </div>
              </td>
              <td>
                <asp:Literal id="ltlEmail" runat="server" />
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  手机：
                </div>
              </td>
              <td>
                <%=GetValue( "Mobile")%>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  QQ：
                </div>
              </td>
              <td>
                <%=GetValue( "QQ")%>
              </td>
            </tr>
            <tr>
              <td>
                <div class="pull-right">
                  提交时间：
                </div>
              </td>
              <td>
                <%=GetValue( "AddDate")%>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

    <asp:PlaceHolder id="phAnswer" runat="server">

    <hr />

    <div class="popover popover-static">
      <h3 class="popover-title">回复</h3>
      <div class="popover-content">

        <table class="table noborder table-noborder">
          <tr>
            <td><bairong:BREditor id="Content" runat="server"></bairong:BREditor></td>
          </tr>
          <tr>
            <td>
                <div style="position:relative;overflow:hidden;">
                    <a class="btn" style="margin:0px 15px 0 0;" href="javascript:;">添加附件</a> 附件大小不超过2M！
                    <input type="file" name="attachment" style="position:absolute;left:-160px;cursor:pointer;top:0px;z-index:999;opacity:0;filter:alpha(opacity=0);"/>
                    <span></span>
                </div>
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

    </asp:PlaceHolder>

  </form>
</body>

</html>