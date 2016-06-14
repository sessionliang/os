<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.UserLevelAdd" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <asp:PlaceHolder ID="phCreditsAddTips" runat="server">
            <div class="tips">
                增加用户等级时最低积分和最高积分必须在已有的某个用户等级积分范围之内.<br>
                例如: 注册的积分范围是50 - 200 ,那么要添加的用户等级的积分上下限必须在(200≥ 积分范围 >50) 或 (200> 积分范围 ≥50)之间. 如果积分范围跨越多个用户等级积分范围区间的用户等级时, 系统将视为无效
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phCreditsEditTips" runat="server">
            <div class="tips">
                编辑用户等级时最低积分和最高积分必须在当前用户等级积分范围之内.
                <br>
                例如: 注册的积分范围是50 - 200 ,那么编辑该用户等级的积分必须在50 - 200 之间. 如果积分范围跨越多个用户等级积分上下限区间时, 系统将视为无效.
                <br>
                如果想要扩展积分的范围时可通过增加最低积分或缩小最高积分来进行调整. 
      例如: 注册的积分范围分别是50 - 200 , 如果想把最高积分扩展为300 ,只需调整相邻的"中级"等级(范围为200和500) 的最低积分修改为300即可. 调整最低积分的方法与调整最高积分的方法类似
            </div>
        </asp:PlaceHolder>

        <table class="table table-noborder table-hover">
            <tr>
                <td width="160">等级名称：</td>
                <td>
                    <asp:TextBox ID="LevelName" MaxLength="50" Size="30" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="LevelName"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="phCredits" runat="server">
                <tr>
                    <td>最小值：</td>
                    <td>
                        <asp:TextBox ID="MinNum" Columns="10" runat="server" />
                        <asp:RequiredFieldValidator
                            ControlToValidate="MinNum"
                            ErrorMessage=" *" ForeColor="red"
                            Display="Dynamic"
                            runat="server" />
                        <asp:RegularExpressionValidator
                            ControlToValidate="MinNum"
                            ValidationExpression="[0-9]+"
                            Display="Dynamic"
                            ErrorMessage=" *" ForeColor="red"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>最大值：</td>
                    <td>
                        <asp:TextBox ID="MaxNum" MaxLength="50" Columns="10" runat="server" />
                        <asp:RequiredFieldValidator
                            ControlToValidate="MaxNum"
                            ErrorMessage=" *" ForeColor="red"
                            Display="Dynamic"
                            runat="server" />
                        <asp:RegularExpressionValidator
                            ControlToValidate="MaxNum"
                            ValidationExpression="[0-9]+"
                            Display="Dynamic"
                            ErrorMessage=" *" ForeColor="red"
                            runat="server" />
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td>星星数：</td>
                <td>
                    <asp:TextBox ID="Stars" MaxLength="50" Columns="10" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="Stars"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                    <asp:RegularExpressionValidator
                        ControlToValidate="Stars"
                        ValidationExpression="[0-9]+"
                        Display="Dynamic"
                        ErrorMessage=" *" ForeColor="red"
                        runat="server" />
                    (0 - 10)
                </td>
            </tr>
            <tr>
                <td>等级名称颜色：</td>
                <td>
                    <asp:TextBox ID="Color" MaxLength="50" Columns="15" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
