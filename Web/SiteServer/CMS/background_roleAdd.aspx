<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundRoleAdd" %>

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
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">后台角色</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="160">角色名称：</td>
                        <td>
                            <asp:TextBox Columns="25" MaxLength="50" ID="tbRoleName" runat="server" />
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbRoleName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbRoleName"
                                ValidationExpression="[^',]+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                            <span>唯一标识此角色的字符串</span>
                        </td>
                    </tr>
                    <tr>
                        <td>备注：</td>
                        <td>
                            <asp:TextBox Columns="50" TextMode="MultiLine" ID="tbDescription" runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbDescription"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <asp:Repeater ID="rptProducts" runat="server">
            <ItemTemplate>

                <div class="popover popover-static">
                    <h3 class="popover-title">
                        <asp:Literal ID="ltlModuleName" runat="server"></asp:Literal>
                        权限设置
                    </h3>
                    <div class="popover-content">

                        <asp:Literal ID="ltlModulePermissions" runat="server"></asp:Literal>

                        <asp:PlaceHolder ID="phPublishmentSystemPermissions" runat="server">
                            <hr />
                            <table class="table noborder">
                                <tr class="info">
                                    <td>点击下列链接，进入对应站点的管理权限设置界面</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltlPublishmentSystems" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>注：点击网站选择需要管理的栏目&nbsp;&nbsp;<img src="../pic/canedit.gif" align="absmiddle" />有权管理此网站&nbsp;
                                            <img src="../pic/cantedit.gif" align="absmiddle" />无权管理此网站</span>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                            <table class="table noborder">
                                <tr class="info">
                                    <td>点击下列链接，进入【用户中心】的管理权限设置界面</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltlUserCenter" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>注：点击用户中心选择需要管理的栏目&nbsp;&nbsp;<img src="../pic/canedit.gif" align="absmiddle" />有权管理用户中心&nbsp;
                                            <img src="../pic/cantedit.gif" align="absmiddle" />无权管理用户中心</span>
                                    </td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>

                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>

        <hr />
        <table class="table noborder">
            <tr>
                <td class="center">
                    <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                    <%if (Request.QueryString["RoleName"] != null)
                        {%>
                    <input type="button" class="btn" value="返 回" onclick="javascript:location.href='background_role.aspx?module=<%= Request.QueryString["module"]%>    ';" />
                    <%}%>
                </td>
            </tr>
        </table>

        <script language="javascript" type="text/javascript">
            function loadEvent(){
    
                var els = document.getElementsByTagName("input");
                var inputs = new Array(els.length + 1);
                for(var i=0;i<els.length;i++){
                    inputs[i] = els[i];
                }

                inputs[els.length] = document.getElementById("tbDescription");
    
                for(var i=0;i<inputs.length;i++){
                    inputs[i].onchange = function()
                    {
                        var ss_role = _getCookie("background_roleAdd_aspx");
                        var theV = (this.type == "checkbox") ? this.checked : this.value;
                        if (!ss_role){
                            ss_role = this.id + ":" + theV;
                        }else{
                            ss_role += "," + this.id + ":" + theV;
                        }
                        _setCookie("background_roleAdd_aspx", ss_role);
                    };
                }
    
  <%if (base.Request.QueryString["Return"] == null)
            {%>
      _setCookie("background_roleAdd_aspx", "");
  <%}
            else
            {%>
      var ss_role = _getCookie("background_roleAdd_aspx");
      if (ss_role){
          var strs=ss_role.split(",");
          for (i=0;i<strs.length ;i++ )
          {
              var el = document.getElementById(strs[i].split(":")[0]);
              if (el.type == "checkbox"){
                  el.checked = (strs[i].split(":")[1] == "true");
              }else{
                  el.value = strs[i].split(":")[1];
              }
          }
      }
  <%}%>

  }
            loadEvent();
        </script>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
