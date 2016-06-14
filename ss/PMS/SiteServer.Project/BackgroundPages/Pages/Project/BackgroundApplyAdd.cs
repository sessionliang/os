using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;



namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundApplyAdd : BackgroundBasePage
    {
        public TextBox tbTitle;
        public DropDownList ddlPriority;
        public DateTimeTextBox tbExpectedDate;
        public DropDownList ddlIsSelf;
        public DropDownList ddlDepartmentID;
        public DropDownList ddlTypeID;
        public Literal ltlUserName;
        public DateTimeTextBox tbEndDate;
        public TextBox tbSummary;
        public BREditor breContent;

        private int contentID;
        private string returnUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundApplyToWork.GetRedirectUrl(base.ProjectID);
            }

            string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Image");
            string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Scrawl");
            string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "File");
            string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "ImageManager");
            string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "GetMovie");
            //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.ddlIsSelf, "自己负责", "指定负责部门");
                this.ddlIsSelf.Attributes.Add("onchange", "if ($(this).val() == 'False'){$('#spanOthers').show();}else{$('#spanOthers').hide();}");
                ArrayList departmentIDArrayList = ProjectManager.GetFirstDepartmentIDArrayList();
                foreach (int departmentID in departmentIDArrayList)
                {
                    ListItem listItem = new ListItem(DepartmentManager.GetDepartmentName(departmentID), departmentID.ToString());
                    if (departmentID == AdminManager.Current.DepartmentID)
                    {
                        listItem.Selected = true;
                    }
                    this.ddlDepartmentID.Items.Add(listItem);
                }

                ArrayList typeInfoArrayList = DataProvider.TypeDAO.GetTypeInfoArrayList(base.ProjectID);
                this.ddlTypeID.Items.Add(new ListItem("<<不设置>>", "0"));
                foreach (TypeInfo typeInfo in typeInfoArrayList)
                {
                    this.ddlTypeID.Items.Add(new ListItem(typeInfo.TypeName, typeInfo.TypeID.ToString()));
                }

                this.ltlUserName.Text = AdminManager.GetDisplayName(AdminManager.Current.UserName, true);

                this.tbEndDate.DateTime = DateTime.Now.AddDays(15);
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                ApplyInfo applyInfo = new ApplyInfo(0, base.ProjectID, TranslateUtils.ToInt(this.ddlPriority.SelectedValue), TranslateUtils.ToInt(this.ddlTypeID.SelectedValue), AdminManager.Current.UserName, DateTime.Now, string.Empty, DateTime.Now, string.Empty, DateTime.Now, 0, string.Empty, 0, EApplyState.New, this.tbTitle.Text);
                applyInfo.ExpectedDate = this.tbExpectedDate.DateTime;
                applyInfo.EndDate = this.tbEndDate.DateTime;
                applyInfo.Summary = this.tbSummary.Text;
                applyInfo.Content = this.breContent.Text;

                if (TranslateUtils.ToBool(this.ddlIsSelf.SelectedValue))
                {
                    applyInfo.AcceptUserName = AdminManager.Current.UserName;
                    applyInfo.AcceptDate = DateTime.Now;
                    applyInfo.DepartmentID = AdminManager.Current.DepartmentID;
                    applyInfo.UserName = AdminManager.Current.UserName;
                    applyInfo.State = EApplyState.Accepted;
                    applyInfo.StartDate = DateTime.Now;
                }
                else
                {
                    applyInfo.DepartmentID = TranslateUtils.ToInt(this.ddlDepartmentID.SelectedValue);
                }

                applyInfo.ID = DataProvider.ApplyDAO.Insert(applyInfo);

                if (TranslateUtils.ToBool(this.ddlIsSelf.SelectedValue))
                {
                    ApplyManager.Log(applyInfo.ID, EProjectLogType.Accept);
                }

                base.SuccessMessage("办件添加成功");
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
