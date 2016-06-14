<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.BackgroundForumEdit" %>

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
  
  <ul class="nav nav-pills">
    <li id="tab1" class="active"><a href="javascript:;" onClick="_toggleTab(1,5);">基本设置</a></li>
    <li id="tab2"><a href="javascript:;" onClick="_toggleTab(2,5);">高级设置</a></li>
    <li id="tab3"><a href="javascript:;" onClick="_toggleTab(3,5);">贴子选项</a></li>
    <li id="tab4"><a href="javascript:;" onClick="_toggleTab(4,5);">主题分类</a></li>
    <li id="tab5"><a href="javascript:;" onClick="_toggleTab(5,5);">权限相关</a></li>
  </ul>

  <bairong:alerts text="以下设置没有继承性，即仅对当前版块有效，不会对下级子版块产生影响。" runat="server"></bairong:alerts>

  <div id="column1" class="popover popover-static">
    <h3 class="popover-title">基本设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="160">版块名称：</td>
          <td>
            <asp:TextBox  Columns="45" MaxLength="255" id="txtForumName" runat="server"/>
            <asp:RequiredFieldValidator id="RequiredFieldValidator"
              ControlToValidate="txtForumName"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic"
              runat="server"/>
          </td>
        </tr>
        <tr>
          <td>版块索引：</td>
          <td>
            <asp:TextBox  Columns="45" MaxLength="255" id="txtIndexName" runat="server"/>
            <asp:RegularExpressionValidator
              runat="server"
              ControlToValidate="txtIndexName"
              ValidationExpression="[^']+"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic" />
          </td>
        </tr>
        <tr>
          <td>版块名称颜色：</td>
          <td>
            <asp:TextBox  Columns="45" MaxLength="200" id="txtColor" runat="server"/>
            <asp:RegularExpressionValidator
              runat="server"
              ControlToValidate="txtColor"
              ValidationExpression="[^']+"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic" />
          </td>
        </tr>
        <tr>
          <td>版块状态：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadState" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"> </asp:RadioButtonList>
            <br />
            <span class="gray">选择“不显示”将暂时将版块隐藏不显示，但版块内容仍将保留，且用户仍可通过直接提供带有 fid 的 URL 访问到此版块</span>
          </td>
        </tr>
        <tr>
          <td>版块图标：</td>
          <td>
            <asp:TextBox  ID="txtIconUrl"
              MaxLength="100"
              size="45"
              runat="server"/>
            <asp:Button ID="btnSelectImage" class="btn" text="选择" runat="server"></asp:Button>
            <asp:Button ID="btnUploadImage" class="btn" text="上传" runat="server"></asp:Button>
          </td>
        </tr>
        <tr>
          <td>版块简介：</td>
          <td>
            <asp:TextBox TextMode="MultiLine"  ID="txtSummary" MaxLength="200" Rows="5" Columns="45" runat="server"/>
            <br />
            <span class="gray">将显示于版块名称的下面，提供对本版块的简短描述</span>
          </td>
        </tr>
        <tr>
          <td>Keywords：</td>
          <td>
            <asp:TextBox TextMode="MultiLine"  ID="txtMetaKeywords" MaxLength="200" Rows="5" Columns="45" runat="server"/>
            <br />
            <span class="gray">用于搜索引擎优化，出现在 meta 的Keywords 标签中</span>
          </td>
        </tr>
        <tr>
          <td></td>
          <td colspan="2">
            <asp:TextBox TextMode="MultiLine"  ID="txtMetaDescription" MaxLength="200" Rows="5" Columns="45" runat="server"/>
            <br />
            <span class="gray">用于搜索引擎优化，出现在 meta 的 description  标签中</span>
          </td>
        </tr>
      </table>

    </div>
  </div>

  <div id="column2" style="display:none" class="popover popover-static">
    <h3 class="popover-title">高级设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr id="LinkURLRow" runat="server">
          <td width="160">版块转向URL：</td>
          <td>
            <asp:TextBox  Columns="45" MaxLength="200" id="txtLinkUrl" runat="server"/>
            <asp:RegularExpressionValidator
              runat="server"
              ControlToValidate="txtLinkUrl"
              ValidationExpression="[^']+"
              errorMessage=" *" foreColor="red" 
              Display="Dynamic" />
            <br />
            <span class="gray">如果设置转向 URL，用户点击本版块将进入转向中设置的 URL。</span>
          </td>
        </tr>
        <tr id="LinkTypeRow" runat="server">
          <td>下级子版块横排：</td>
          <td>
            <asp:DropDownList id="ddlColumns" runat="server">
              <asp:ListItem Text="0" Value="0" Selected="true"></asp:ListItem>
              <asp:ListItem Text="1" Value="1" ></asp:ListItem>
              <asp:ListItem Text="2" Value="2"></asp:ListItem>
              <asp:ListItem Text="3" Value="3"></asp:ListItem>
              <asp:ListItem Text="4" Value="4"></asp:ListItem>
              <asp:ListItem Text="5" Value="5"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <span class="gray">(设置下级子版块横排时每行版块数量，如果设置为 0，则按正常方式排列)</span>
          </td>
        </tr>
        <tr>
          <td>只显示下级子版块：</td>
          <td>
            <asp:RadioButtonList ID="rblIsOnlyDisplaySubForums" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将不显示本版块的主题列表、发帖按钮等等，类似于一个分类</span>
          </td>
        </tr>
        <tr>
          <td>显示边栏：</td>
          <td>
            <asp:RadioButtonList ID="rblIsDisplayForumInfo" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”版块首页侧边将显示聚合本版内容的信息</span>
          </td>
        </tr>
        <tr>
          <td>本版块在首页显示方式：</td>
          <td>
            <asp:RadioButtonList ID="rblForumSummaryType" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">选择“默认”，将使用全局设置</span>
          </td>
        </tr>
        <tr>
          <td>主题默认排序字段：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadOrderField" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">设置版块的主题列表默认按照哪个字段进行排序显示。默认为“回复时间”，除默认设置外其他排序方式会加重服务器负担</span>
          </td>
        </tr>
        <tr>
          <td>主题默认排序方式：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadOrderType" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">设置版块的主题列表默认排序的方式。默认为“按降序排列”，除默认设置外其他排序方式会加重服务器负担</span>
          </td>
        </tr>
        <tr>
          <td>是否显示置顶主题：</td>
          <td>
            <asp:RadioButtonList ID="rblIsDisplayTopThread" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">是否在本版显示全局置顶和分版置顶</span>
          </td>
        </tr>
        <tr style="display:none;">
          <td>本栏目的内容模版：</td>
          <td>
            <asp:DropDownList id="TemplateID" DataTextField="TemplateName" DataValueField="TemplateID" runat="server"></asp:DropDownList>
            <br />
            <span class="gray">此栏目内的内容所用到的内容模版</span>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

  <div id="column3" style="display:none" class="popover popover-static">
    <h3 class="popover-title">帖子选项</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="160">发帖审核：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadCheckType" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将使用户在本版发表的帖子待版主或管理员审查通过后才显示出来，打开此功能后，你可以在用户组中设定哪些组发帖可不经审核，也可以在管理组中设定哪些组可以审核别人的帖子</span>
          </td>
        </tr>
        <tr>
          <td>是否允许编辑帖子：</td>
          <td>
            <asp:RadioButtonList ID="rblIsEditThread" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
              <asp:ListItem Text="否" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将允许用户编辑本版发表的帖子</span>
          </td>
        </tr>
        <tr>
          <td>是否开启主题回收站：</td>
          <td>
            <asp:RadioButtonList ID="rblIsOpenRecycle" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">是否在本版启用回收站功能，打开此功能后，所有被删除主题将被放在回收站中，而不会被直接删除</span>
          </td>
        </tr>
        <tr>
          <td>是否允许使用HTML代码：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowHtml" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将不屏蔽帖子中的任何代码，将带来严重的安全隐患，请慎用</span>
          </td>
        </tr>
        <tr>
          <td>是否允许使用[img]代码：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowImg" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
              <asp:ListItem Text="否" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">注意: 允许 [img] 代码作者将可以在帖子插入其他网站的图片并显示</span>
          </td>
        </tr>
        <tr>
          <td>是否允许使多媒体代码：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowMultimedia" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">注意:允许 [audio] [video] [flash] 等多媒体代码后，作者将可以在帖子插入多媒体文件并显示</span>
          </td>
        </tr>
        <tr>
          <td>是否允许使用表情：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowEmotionSymbol" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
              <asp:ListItem Text="否" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">表情提供对表情符号，如“:)”的解析，使之作为图片显示</span>
          </td>
        </tr>
        <tr>
          <td>是否启用内容干扰码：</td>
          <td>
            <asp:RadioButtonList ID="rblIsOpenDisturbCode" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将在帖子内容中增加随机的干扰字串，使得访问者无法复制原始内容。注意: 本功能会轻微加重服务器负担</span>
          </td>
        </tr>
        <tr>
          <td>是否允许匿名发帖：</td>
          <td>
            <asp:RadioButtonList ID="rblIsAllowAnonymousThread" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">是否允许用户在本版匿名发表主题和回复，只要用户组或本版块允许，用户均可使用匿名发帖功能。匿名发帖不同于游客发帖，用户需要登录后才可使用，版主和管理员可以查看真实作者</span>
          </td>
        </tr>
        <tr>
          <td>图片附件是否加水印：</td>
          <td>
            <asp:RadioButtonList ID="rblIsOpenWatermark" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
              <asp:ListItem Text="否" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">选择“是”将不对本版块上传的图片附件自动添加水印，即便 全局设置中开启了此项功能。选择“否”为按照系统默认设置决定是否添加水印</span>
          </td>
        </tr>
        <tr>
          <td>允许附件类型：</td>
          <td>
            <asp:TextBox  Columns="40" id="txtAllowAccessoryType" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtAllowAccessoryType" ValidationExpression="[^']+" ErrorMessage=" *" foreColor="red" Display="Dynamic" />
            <br />
            <span class="gray">设置允许上传的附件扩展名，多个扩展名之间用半角逗号 ',' 分割。本设置的优先级高于用户组，留空为按照用户组允许的附件类型设定</span>
          </td>
        </tr>
        <tr>
          <td>主题自动关闭：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadAutoCloseType" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ThreadAutoCloseType_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true" runat="server"></asp:RadioButtonList>
          </td>
          <br />
          <span class="gray">设置主题是否在某时间后自动关闭，禁止普通用户回复</span>
        </tr>
        <asp:PlaceHolder ID="phThreadAutoCloseType"  Visible="false" runat="server">
          <tr>
            <td>自动关闭时间：</td>
            <td>
              <asp:TextBox  Columns="10" MaxLength="10" id="txtThreadAutoCloseWithDateNum" runat="server" />
              天
              <asp:RegularExpressionValidator runat="server" ControlToValidate="txtThreadAutoCloseWithDateNum" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
              <br />
              <span class="gray">本设定必须在“主题自动关闭”功能打开时才生效，主题依据自动关闭的设定: 在发表后若干天、或被最后回复后若干天被自动转入关闭状态，从而使普通用户无法回复</span>
            </td>
          </tr>
        </asp:PlaceHolder>
      </table>
  
    </div>
  </div>

  <div id="column4" style="display:none" class="popover popover-static">
    <h3 class="popover-title">主题分类</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="160">主题分类控制：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadCategoryType" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">是否强制用户发表新主题时必须选择分类</span>
          </td>
        </tr>
        <tr>
          <td>是否允许按类别浏览：</td>
          <td>
            <asp:RadioButtonList ID="rblIsReadByCategory" RepeatDirection="Horizontal" runat="server">
              <asp:ListItem Text="是" Value="True"></asp:ListItem>
              <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <span class="gray">用户是否可以按照主题分类筛选浏览内容</span>
          </td>
        </tr>
        <tr>
          <td>类别前缀：</td>
          <td>
            <asp:RadioButtonList ID="rblThreadCategoryDisplayType" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
            <br />
            <span class="gray">是否在主题前面显示分类的名称</span>
          </td>
        </tr>
        <tr class="summary-title" height="25">
          <td colspan="3">主题分类</td>
        </tr>
        <asp:Repeater ID="ChannelPermissionRepeater" runat="server">
            <itemtemplate>
              <tr ><td colspan="2" ><asp:CheckBox ID="Permission" runat="server"></asp:CheckBox></td>
            </itemtemplate>
          </asp:Repeater>
        <asp:Repeater ID="ChannelUserGroupRepeater" runat="server">
          <itemtemplate>
            <tr style="display:none;">
              <td><asp:CheckBox ID="UserGroup" runat="server"></asp:CheckBox></td>
              <asp:Literal ID="UserPermissions" runat="server"></asp:Literal>
            </tr>
          </itemtemplate>
        </asp:Repeater>
      </table>
  
    </div>
  </div>

  <div id="column5" style="display:none" class="popover popover-static">
    <h3 class="popover-title">权限相关</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="160">访问密码：</td>
          <td>
            <asp:TextBox  Columns="45" MaxLength="255" id="txtAccessPassword" runat="server"/>
            <br />
            <span class="gray">当你设置密码后，用户必须输入密码才可以访问到此版块</span>
          </td>
        </tr>
        <tr>
          <td>访问用户：</td>
          <td>
            <asp:TextBox TextMode="MultiLine"  ID="txtAccessUserNames" MaxLength="200" Rows="5" width="90%" runat="server"/>
            <br />
            <span class="gray">限定只有列表中的用户可以访问本版块，多个用户以“,”分隔</span>
          </td>
        </tr>
      </table>

      <table class="table table-bordered table-hover">
        <tr class="info thead">
          <td width="200">全选（会员组/被禁止的权限）</td>
          <asp:Repeater ID="PermissionRepeater" runat="server">
            <itemtemplate>
              <td><asp:Literal ID="ltlPermission" runat="server"></asp:Literal></td>
            </itemtemplate>
          </asp:Repeater>
        </tr>
        <tr class="success">
          <td colspan="20"><b>积分用户组</b></td>
        </tr>
        <asp:Repeater ID="UserGroupCreditsRepeater" runat="server">
          <itemtemplate>
            <tr>
              <td><asp:CheckBox ID="UserGroup" runat="server"></asp:CheckBox></td>
              <asp:Literal ID="UserPermissions" runat="server"></asp:Literal>
            </tr>
          </itemtemplate>
        </asp:Repeater>

        <asp:PlaceHolder ID="phUserGroupSpecials" Visible="false" runat="server">
        <tr class="summary-title" height="25">
          <td colspan="20"><b>特殊用户组</b></td>
        </tr>
        <asp:Repeater ID="UserGroupSpecialsRepeater" runat="server">
          <itemtemplate>
            <tr>
              <td><asp:CheckBox ID="UserGroup" runat="server"></asp:CheckBox></td>
              <asp:Literal ID="UserPermissions" runat="server"></asp:Literal>
            </tr>
          </itemtemplate>
        </asp:Repeater>
        </asp:PlaceHolder>
        <tr class="success">
          <td colspan="20"><b>系统用户组</b></td>
        </tr>
        <asp:Repeater ID="UserGroupSystemsRepeater" runat="server">
          <itemtemplate>
            <tr class="tdbg" height="25" onMouseOver="this.className='tdbg-dark';" onMouseOut="this.className='tdbg';">
              <td><asp:CheckBox ID="UserGroup" runat="server"></asp:CheckBox></td>
              <asp:Literal ID="UserPermissions" runat="server"></asp:Literal>
            </tr>
          </itemtemplate>
        </asp:Repeater>
      </table>

  
    </div>
  </div>

  <hr />
  <table class="table noborder">
    <tr>
      <td class="center">
        <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server"/>
        <input class="btn" type="button" onClick="location.href='background_forum.aspx';return false;" value="返 回" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->