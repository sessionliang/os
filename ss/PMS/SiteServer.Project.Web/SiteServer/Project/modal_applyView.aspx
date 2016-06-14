<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplyView" Trace="false"%>

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
<bairong:alerts runat="server"></bairong:alerts>

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
  .RowattributeName { width: 80px; text-align: center; color: #333; font-size: 14px; }
  </style>

  <div class="popover popover-static">
    <h3 class="popover-title">办件信息(<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>)</h3>
    <div class="popover-content">

      <div class="applyTitle"><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></div>

      <table width="98%" class="center" border="0" cellspacing="0" cellpadding="0" style="border-left:1px solid silver;border-top:1px solid silver;">
      <tr>
        <td><table width="100%" border=0 cellspacing=0 cellpadding=0 style="width:100%;height:100%;">
            <tr>
                <td><table class="applyTable" border=0 cellspacing=0 cellpadding=0 style="width:100%;height:100%;table-layout:fixed">
                  <tbody>
                    <tr>
                      <td bgcolor="#f0f6fc" class="RowattributeName">办件编号</td>
                      <td>#
                        <asp:Literal ID="ltlApplyID" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="RowattributeName">状态</td>
                      <td><asp:Literal ID="ltlState" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="RowattributeName">优先级</td>
                      <td><asp:Literal ID="ltlPriority" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="RowattributeName">办件类型</td>
                      <td><asp:Literal ID="ltlTypeID" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="RowattributeName">发起人</td>
                      <td><asp:Literal ID="ltlAddUserName" runat="server"></asp:Literal></td>
                      <td bgcolor="#f0f6fc" class="RowattributeName">发起时间</td>
                      <td><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td bgcolor="#f0f6fc" class="RowattributeName">受理人</td>
                        <td><asp:Literal ID="ltlAcceptUserName" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="RowattributeName">受理时间</td>
                        <td><asp:Literal ID="ltlAcceptDate" runat="server"></asp:Literal></td>
                      </tr>
                    <tr>
                        <td bgcolor="#f0f6fc" class="RowattributeName">负责部门</td>
                        <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="RowattributeName">负责人</td>
                        <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td bgcolor="#f0f6fc" class="RowattributeName">审核人</td>
                        <td><asp:Literal ID="ltlCheckUserName" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="RowattributeName">审核时间</td>
                        <td><asp:Literal ID="ltlCheckDate" runat="server"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td bgcolor="#f0f6fc" class="RowattributeName">预计开始</td>
                        <td><asp:Literal ID="ltlExpectedDate" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="RowattributeName">实际开始</td>
                        <td><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td bgcolor="#f0f6fc" class="RowattributeName">截止日期</td>
                        <td><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></td>
                        <td bgcolor="#f0f6fc" class="RowattributeName">期限</td>
                        <td><asp:Literal ID="ltlDateLimit" runat="server"></asp:Literal></td>
                      </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="RowattributeName">备注</td>
                      <td colspan="3"><asp:Literal ID="ltlSummary" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                      <td bgcolor="#f0f6fc" class="RowattributeName">内容</td>
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
      <tr valign="top" style="background-color: #f0f6fc">
        <td colspan="2"></td>
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
              <td width="80" bgcolor="#f0f6fc" class="RowattributeName">办理人员</td>
              <td><asp:Literal ID="ltlDepartmentAndUserName" runat="server"></asp:Literal></td>
              </tr>
            <tr>
                <td align="right" bgcolor="#f0f6fc" class="RowattributeName">办理日期</td>
                <td class="tdBorder"><asp:Literal ID="ltlReplyAddDate" runat="server"></asp:Literal></td>
              </tr>
            <tr>
              <td align="right" bgcolor="#f0f6fc" class="RowattributeName">办理结果</td>
              <td class="tdBorder"><asp:Literal ID="ltlReply" runat="server"></asp:Literal></td>
              </tr>
            <tr>
              <td align="right" bgcolor="#f0f6fc" class="RowattributeName">附件</td>
              <td class="tdBorder"><asp:Literal ID="ltlReplyFileUrl" runat="server"></asp:Literal></td>
            </tr>
            </table>
        </td>
        </tr>
      </asp:PlaceHolder>
    </table>

  </div>
  </div>

</form>
</body>
</html>
