<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.BackgroundPermission" %>

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

  <ul class="nav nav-pills">
    <li id="tab1" class="active"><a href="javascript:;" onClick="_toggleTab(1,3);">基本设置</a></li>
    <li id="tab2"><a href="javascript:;" onClick="_toggleTab(2,3);">帖子权限</a></li>
    <li id="tab3"><a href="javascript:;" onClick="_toggleTab(3,3);">附件权限</a></li>
  </ul>

  <div id="column1" class="popover popover-static">
    <h3 class="popover-title">基本设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr class="success">
          <td colspan="2"></td>
          <td class="center" width="70">横向扩展</td>
        </tr>
        <tr>
          <td width="150">访问论坛：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowVisit" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>关闭后，用户将不能访问站点的任何页面</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowVisit" /></td>
        </tr>
        <tr>
          <td>隐身登录：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowHide" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，用户可以隐身登陆站点</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowHide" /></td>
        </tr>
        <tr>
          <td>个性签名：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowSignature" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，此用户组的用户可以使用个性签名功能</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowSignature" /></td>
        </tr>
        <tr>
          <td>搜索控制：</td>
          <td>
            <asp:RadioButtonList ID="rblSearchType" runat="server"></asp:RadioButtonList>
            <span>设置用户组的搜索权限</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="SearchType" /></td>
        </tr>
        <tr>
          <td>两次搜索时间间隔：</td>
          <td>
            <asp:TextBox ID="tbSearchInterval"  Width="120" runat="server"></asp:TextBox> 秒
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="SearchInterval" /></td>
        </tr>
      </table>
  
    </div>
  </div>

  <div id="column2" style="display:none" class="popover popover-static">
    <h3 class="popover-title">帖子权限</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr class="success">
          <td colspan="2">&nbsp;</td>
          <td class="center" width="70">横向扩展</td>
        </tr>
        <tr>
          <td width="150">浏览帖子：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowRead" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，此用户组的用户可以浏览帖子</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowRead" /></td>
        </tr>
        <tr>
          <td>发表主题：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowPost" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，此用户组的用户可以发表主题</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowPost" /></td>
        </tr>
        <tr>
          <td>回复主题：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowReply" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，此用户组的用户可以回复主题</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowReply" /></td>
        </tr>
        <tr>
          <td>发起投票：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowPoll" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <span>开启后，此用户组的用户可以发起投票</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="IsAllowPoll" /></td>
        </tr>
        <tr>
          <td>每天最多发表帖子数：</td>
          <td>
            <asp:TextBox ID="tbMaxPostPerDay"  Width="120" runat="server"></asp:TextBox>
            篇
            <br />
            <span>0或留空表示不限制</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="MaxPostPerDay" /></td>
        </tr>
        <tr>
          <td>发帖间隔：</td>
          <td>
            <asp:TextBox ID="tbPostInterval"  Width="120" runat="server"></asp:TextBox>
          秒
          <br />
          <span>多少秒间隔内不能发帖，0或留空表示不限制</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="PostInterval" /></td>
        </tr>
      </table>
  
    </div>
  </div>

  <div id="column3" style="display:none" class="popover popover-static">
    <h3 class="popover-title">附件权限</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr class="success">
          <td colspan="2">&nbsp;</td>
          <td class="center" width="70">横向扩展</td>
        </tr>
        <tr>
          <td width="150">上传附件权限：</td>
          <td>
            <asp:RadioButtonList ID="rblUploadType" runat="server"></asp:RadioButtonList>
            <span>可在版块设置处设置上传附件&ldquo;奖励或扣除&rdquo;积分</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="UploadType" /></td>
        </tr>
        <tr>
          <td>下载附件权限：</td>
          <td>
            <asp:RadioButtonList ID="rblDownloadType" runat="server"></asp:RadioButtonList>
            <span>可在版块设置处设置下载附件&ldquo;奖励或扣除&rdquo;积分</span>
          </td>
          <td class="center"><input type="checkbox" name="horizontal" value="DownloadType" /></td>
        </tr>
        <tr>
        <td>是否允许设置附件权限：</td>
        <td>
          <asp:RadioButtonList ID="rblIsAllowSetAttachmentPermissions" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="允许" Value="True" ></asp:ListItem>
              <asp:ListItem Text="禁止" Value="False"></asp:ListItem>
          </asp:RadioButtonList>
          <span>0或留空表示不限制</span>
        </td>
        <td class="center"><input type="checkbox" name="horizontal" value="IsAllowSetAttachmentPermissions" /></td>
      </tr>
      <tr>
        <td>允许上传的最大附件大小：</td>
        <td>
          <asp:TextBox  Width="60" id="tbMaxSize" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="tbMaxSize"
            errorMessage=" *" foreColor="red" 
            Display="Dynamic"
            runat="server"/>
          <br />
          <span>单位K，0或留空表示不限制</span>
        </td>
        <td class="center"><input type="checkbox" name="horizontal" value="MaxSize" /></td>
      </tr>
      <tr>
        <td>允许上传的附件总大小：</td>
        <td>
          <asp:TextBox  Width="60" id="tbMaxSizePerDay" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="tbMaxSizePerDay"
            errorMessage=" *" foreColor="red" 
            Display="Dynamic"
            runat="server"/>
          <br />
          <span>每日，单位K，0或留空表示不限制</span>
        </td>
        <td class="center"><input type="checkbox" name="horizontal" value="MaxSizePerDay" /></td>
      </tr>
      <tr>
        <td>允许上传的附件数量：</td>
        <td>
          <asp:TextBox  Width="60" id="tbMaxNumPerDay" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="tbMaxNumPerDay"
            errorMessage=" *" foreColor="red" 
            Display="Dynamic"
            runat="server"/>
          个
          <br />
          <span>每日，0或留空表示不限制</span>
        </td>
        <td class="center"><input type="checkbox" name="horizontal" value="MaxNumPerDay" /></td>
      </tr>
      <tr>
        <td width="160">允许上传的附件类型：</td>
        <td>
          <asp:TextBox  Width="200" id="tbAttachmentExtensions" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="tbAttachmentExtensions"
            errorMessage=" *" foreColor="red" 
            Display="Dynamic"
            runat="server"/>
          <br />
          <span>设置允许上传的附件扩展名，多个扩展名之间用半角逗号 "," 分割，留空为使用系统设置</span>
        </td>
        <td class="center"><input type="checkbox" name="horizontal" value="AttachmentExtensions" /></td>
      </tr>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">横向扩展</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="150">用户组权限横向设置：</td>
          <td>
          <asp:ListBox ID="lbHorizontalGroup" Rows="10" runat="server"></asp:ListBox>
          </td>
          <td class="gray">利用此功能可将一个或多个设置同时应用到其它用户组中，使操作更加简便<br>
    操作说明:<br>
    选中需要进行横向操作的权限设置后面的复选框<br>
    在左边的复选框中选择需要进行横向操作的用户组<br>
    然后提交即可完成横向操作<br>
    注:<br>
    选择用户组时可使用 &lsquo;Ctrl&rsquo; 键进行多选，也可使用 &lsquo;Shift&rsquo; 键或拖动鼠标连续选择多个版块<br>
    不使用此功能，请不要选中权限设置后面的复选框和左边的复选框中版块</td>
        </tr>
      </table>
  
    </div>
  </div>

  <hr />
  <table class="table noborder">
    <tr>
      <td class="center">
        <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server"/>
        <input class="btn" type="button" onClick="location.href = 'background_userGroup.aspx'; return false;" value="返 回" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->