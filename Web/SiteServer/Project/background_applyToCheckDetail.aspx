<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundApplyToCheckDetail" %>

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

  <style>
  .applyTitle {margin:5px; padding:5px; font-size:18px;text-align:center}
  .applyTable td { text-align: left; border-bottom: 1px solid silver; border-right: 1px solid silver; }
  .tableBorder td { text-align: left; padding: 2px; margin: 2px; padding-left: 8px; border-bottom: 1px solid silver; border-right: 1px solid silver; }
  .tdBorder, .applyTable td { border-bottom: 1px solid silver; border-right: 1px solid silver; }
  .applyTable td { height: 30px; padding: 2px; margin: 2px; padding-left: 5px; }
  .applyTable .attributeName { text-align: right; padding-right: 8px; width: 100px; background: F8F8F8; }
  .applyTable .normalText, .applyTable select { margin-left: 8px; width: 200px; height: 24px; line-height: 20px; border: 1px solid #9AABBB; }
  .applyTable textarea { margin-left: 8px; width: 700px; height: 50px; border: 1px solid #9AABBB; line-height: 20px; }
  table { font-size: 14px; }
  .disableTable .normalText { }
  .requireStyle { margin-left: 4px; color: red; }
  .RowattributeName { width: 30px; text-align: center; color: #333; font-size: 14px; }
  </style>
  <script>
  function showAction(divID){
    $('.action').hide();$('#' + divID).show();$('html,body').animate({scrollTop: $('#' + divID).offset().top}, 1000);
  }
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">待审核办件信息(<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>)</h3>
    <div class="popover-content">
    
      <div class="applyTitle"><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></div>

      <table width="98%" class="center" border=0 cellspacing=0 cellpadding=0 style="border-left:1px solid silver;border-top:1px solid silver;">
      <tr>
        <td><table width="100%" border=0 cellspacing=0 cellpadding=0 style="width:100%;height:100%;">
            <tr>
                <td><table class="applyTable" border=0 cellspacing=0 cellpadding=0 style="width:100%;height:100%;table-layout:fixed">
                  <tbody>
                    <tr>
                      <td bgcolor="#f0f6fc" class="attributeName">办件编号</td>
                      <td>#
                        <asp:Literal ID="ltlApplyID" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="attributeName">状态</td>
                      <td><asp:Literal ID="ltlState" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="attributeName">优先级</td>
                      <td><asp:Literal ID="ltlPriority" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="attributeName">办件类型</td>
                      <td><asp:Literal ID="ltlTypeID" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="attributeName">发起人</td>
                      <td><asp:Literal ID="ltlAddUserName" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="attributeName">发起时间</td>
                      <td><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td bgcolor="#f0f6fc" class="attributeName">受理人</td>
                        <td><asp:Literal ID="ltlAcceptUserName" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="attributeName">受理时间</td>
                        <td><asp:Literal ID="ltlAcceptDate" runat="server"></asp:Literal></td>
                      </tr>
                    <tr>
                        <td bgcolor="#f0f6fc" class="attributeName">负责部门</td>
                        <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="attributeName">负责人</td>
                        <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td bgcolor="#f0f6fc" class="attributeName">预计开始</td>
                        <td><asp:Literal ID="ltlExpectedDate" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="attributeName">实际开始</td>
                        <td><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td bgcolor="#f0f6fc" class="attributeName">截止日期</td>
                        <td><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="attributeName">期限</td>
                        <td><asp:Literal ID="ltlDateLimit" runat="server"></asp:Literal></td>
                      </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="attributeName">备注</td>
                      <td colspan="3"><asp:Literal ID="ltlSummary" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="attributeName">内容</td>
                      <td colspan="3"><asp:Literal ID="ltlContent" runat="server"></asp:Literal></td>
                    </tr>
                  </tbody>
                </table></td>
            </tr>
          </table></td>
        </tr>
      </table>
      <br />
      <table width="100%" border="0" class="center" cellpadding="8" cellspacing="0">
        <tr>
          <td colspan="2"><hr /></td>
        </tr>
        <asp:PlaceHolder ID="phRemarks" Visible="false" runat="server">
        <tr>
        <td width="80" class="center">意见：</td>
          <td>
        <table border=0 cellspacing=0 cellpadding=0 class="applyTable" style="width:100%; border:1px solid silver">
          <tr>
            <td bgcolor="#f0f6fc" width="60">类型</td>
            <td bgcolor="#f0f6fc" width="100">日期</td>
            <td bgcolor="#f0f6fc" width="150">人员</td>
            <td bgcolor="#f0f6fc">意见</td>
          </tr>
          <asp:Repeater ID="rptRemarks" runat="server">
            <itemtemplate>
              <tr>
                <td class="tdBorder"><asp:Literal ID="ltlRemarkType" runat="server"></asp:Literal></td>
                <td class="tdBorder"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
                <td class="tdBorder"><asp:Literal ID="ltlDepartmentAndUserName" runat="server"></asp:Literal></td>
                <td class="tdBorder"><asp:Literal ID="ltlRemark" runat="server"></asp:Literal></td>
                </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>
          </td>
          </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phReply" Visible="false" runat="server">
          <tr>
          <td width="80" class="center">办理情况：</td>
          <td>
              <table border=0 cellspacing=0 cellpadding=0 class="applyTable" style="width:100%; border:1px solid silver">
              <tr>
                <td width="80" bgcolor="#f0f6fc" class="attributeName">办理人员</td>
                <td><asp:Literal ID="ltlDepartmentAndUserName" runat="server"></asp:Literal></td>
                </tr>
              <tr>
                  <td align="right" bgcolor="#f0f6fc" class="attributeName">办理日期</td>
                  <td class="tdBorder"><asp:Literal ID="ltlReplyAddDate" runat="server"></asp:Literal></td>
                </tr>
              <tr>
                <td align="right" bgcolor="#f0f6fc" class="attributeName">办理结果</td>
                <td class="tdBorder"><asp:Literal ID="ltlReply" runat="server"></asp:Literal></td>
                </tr>
              <tr>
                <td align="right" bgcolor="#f0f6fc" class="attributeName">附件</td>
                <td class="tdBorder"><asp:Literal ID="ltlReplyFileUrl" runat="server"></asp:Literal></td>
              </tr>
              </table>
          </td>
          </tr>
        </asp:PlaceHolder>
        <tr>
          <td width="80" class="center">操作：</td>
          <td>
            <ul class="breadcrumb breadcrumb-button" style="text-align:left">
              <input type="button" value="审 核" onClick="showAction('divCheck');return false;" class="btn btn-primary" />
              <input type="button" value="要求返工" onClick="showAction('divRedo');return false;" class="btn" />
              <input type="button" value="批 注" onClick="showAction('divComment');return false;" class="btn" />
              <asp:Button ID="btnSetting" runat="server" Text="设置属性" class="btn"></asp:Button>
              <asp:Button ID="btnTranslate" runat="server" Text="转 移" class="btn"></asp:Button>
            </ul>
          </td>
        </tr>
      </table>

      <table id="divCheck" class="table table-bordered action" style="display:none">
        <tr class="info thead">
          <td colspan="2"><strong>审核办件</strong></td>
        </tr>
        <tr>
          <td>
            <table class="table table-noborder">
              <tr>
                <td colspan="2"><div class="tips">审核办件后信息将变为已审核状态</div></td>
              </tr>
              <tr>
                <td class="center" width="120">审核部门：</td>
                <td><%=MyDepartment%></td>
              </tr>
              <tr>
                <td class="center">审核人：</td>
                <td><%=MyDisplayName%></td>
              </tr>
              <tr>
                <td class="center">&nbsp;</td>
                <td>
                  <asp:Button class="btn btn-primary" OnClick="Check_OnClick" Text="提 交" runat="server"></asp:Button>
                  <input type="button" value="取 消" onClick="$('#divCheck').hide();" class="btn" />
                </td>
              </tr>
            </table>
          </td>
        </tr>        
      </table>
       
      <table id="divSwitchTo" class="table table-bordered action" style="display:none">
        <tr class="info thead">
          <td colspan="2"><strong>转办办件</strong></td>
        </tr>
        <tr>
          <td>
            <table class="table table-noborder">
              <tr>
                <td colspan="2"><div class="tips">受理办件后信息将变为待办理状态</div></td>
              </tr>
              <tr>
                <td class="center" width="120">转办到：</td>
                <td>
                <div class="fill_box" id="switchToDepartmentContainer" style="display:none">
                    <div class="addr_base addr_normal"> <b id="switchToDepartmentName"></b> <a class="addr_del" href="javascript:;" onClick="showswitchToDepartment('', '0')"></a>
                      <input id="switchToDepartmentID" name="switchToDepartmentID" value="0" type="hidden">
                    </div>
                  </div>
                  <div ID="divAddDepartment" class="btn_pencil" runat="server"><span class="pencil"></span>　选择</div>
                  <script language="javascript">
                function showCategoryDepartment(departmentName, departmentID){
                    $('#switchToDepartmentName').html(departmentName);
                    $('#switchToDepartmentID').val(departmentID);
                    if (departmentID == '0'){
                      $('#switchToDepartmentContainer').hide();
                    }else{
                        $('#switchToDepartmentContainer').show();
                    }
                }
                </script>
                <asp:Literal ID="ltlScript" runat="server"></asp:Literal>
                </td>
              </tr>
                <tr>
                  <td class="center">意见：</td>
                  <td><asp:TextBox ID="tbSwitchToRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
                </tr>
                <tr>
                  <td class="center">操作部门：</td>
                  <td><%=MyDepartment%></td>
                </tr>
                <tr>
                  <td class="center">操作人：</td>
                  <td><%=MyDisplayName%></td>
                </tr>
                <tr>
                  <td class="center">&nbsp;</td>
                  <td><asp:Button class="btn btn-primary" OnClick="SwitchTo_OnClick" Text="提 交" runat="server"></asp:Button>
                    <input type="button" value="取 消" onClick="$('#divSwitchTo').hide();" class="btn" /></td>
                </tr>
            </table>
          </td>
        </tr>        
      </table>
      
      <table id="divComment" class="table table-bordered action" style="display:none">
        <tr class="info thead">
          <td colspan="2"><strong>批注办件</strong></td>
        </tr>
        <tr>
          <td>
            <table class="table table-noborder">
              <tr>
                <td class="center" width="120">批注意见：</td>
                <td><asp:TextBox ID="tbCommentRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
              </tr>
              <tr>
                <td class="center">批注部门：</td>
                <td><%=MyDepartment%></td>
              </tr>
              <tr>
                <td class="center">批注人：</td>
                <td><%=MyDisplayName%></td>
              </tr>
              <tr>
                <td class="center">&nbsp;</td>
                <td>
                  <asp:Button class="btn btn-primary" OnClick="Comment_OnClick" Text="提 交" runat="server"></asp:Button>
                  <input type="button" value="取 消" onClick="$('#divComment').hide();" class="btn" />
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      
      <table id="divRedo" class="table table-bordered action" style="display:none">
        <tr class="info thead">
          <td colspan="2"><strong>要求返工</strong></td>
        </tr>
        <tr>
          <td>
            <table class="table table-noborder">
              <tr>
                <td class="center" width="120">返工意见：</td>
                <td><asp:TextBox ID="tbRedoRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
              </tr>
              <tr>
                <td class="center">办理部门：</td>
                <td><%=MyDepartment%></td>
              </tr>
              <tr>
                <td class="center">办理人：</td>
                <td><%=MyDisplayName%></td>
              </tr>
              <tr>
                <td class="center">&nbsp;</td>
                <td>
                  <asp:Button class="btn btn-primary" OnClick="Redo_OnClick" Text="提 交" runat="server"></asp:Button>
                  <input type="button" value="取 消" onClick="$('#divRedo').hide();" class="btn" /></td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">流动轨迹（操作日志）</h3>
    <div class="popover-content">

      <table class="table table-bordered table-hover">
        <tr class="info thead">
          <td>操作部门</td>
          <td>操作人</td>
          <td width="120">操作时间</td>
          <td>操作内容</td>
        </tr>
        <asp:Repeater ID="rptLogs" runat="server">
          <itemtemplate>
            <tr>
              <td class="center"><asp:Literal ID="ltlDepartment" runat="server"></asp:Literal></td>
              <td class="center"><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
              <td class="center" style="width:120px;"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
              <td>
                <asp:Literal ID="ltlSummary" runat="server"></asp:Literal></td>
            </tr>
          </itemtemplate>
        </asp:Repeater>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
