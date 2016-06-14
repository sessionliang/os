using System;
using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages.TextEditorEWebEditor
{
	public class Upload : BackgroundBasePage
	{
		string sAction;
		string sType;
		string sStyleName;
		string sLanguage;
		string sAllowExt;
		int nAllowSize;
		string sSaveFileName;
		string sPathFileName;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        protected override bool IsAccessable
        {
            get
            {
                if (BaiRongDataProvider.UserDAO.IsAnonymous && !BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    return false;
                }
                return true;
            }
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
			
			InitUpload();

			if (Request.QueryString["action"] != null)
			{
				sAction = Request.QueryString["action"].Trim().ToUpper();
			}

			switch(sAction)
			{
				case "SAVE":
					ShowForm();
					DoSave();
					break;
				default:
					ShowForm();
					break;
			}
		}


		private void ShowForm()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat(@"
<HTML>
<HEAD>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
<script language=""javascript"" src=""../dialog/dialog.js""></script>
<link href='../language/{0}.css' type='text/css' rel='stylesheet'>
<link href='../dialog/dialog.css' type='text/css' rel='stylesheet'>
</head>
<body class=upload>
<form action='?PublishmentSystemID={1}&action=save&type={2}&style={3}&language={0}' method=post name=myform enctype='multipart/form-data'>
<input type=file name=uploadfile size=1 style='width:100%' >
</form>
<script language=javascript>

var sAllowExt = ""{4}"";

function CheckUploadForm() {{
	if (sAllowExt.length > 0){{
        if (!IsExt(document.myform.uploadfile.value,sAllowExt)){{
		    parent.UploadError('lang[""ErrUploadInvalidExt""]+"":'+sAllowExt+'""');
		    return false;
	    }}
    }}
	return true;
}}

var oForm = document.myform ;
oForm.attachEvent(""onsubmit"", CheckUploadForm) ;
if (! oForm.submitUpload) oForm.submitUpload = new Array() ;
oForm.submitUpload[oForm.submitUpload.length] = CheckUploadForm ;
if (! oForm.originalSubmit) {{
	oForm.originalSubmit = oForm.submit ;
	oForm.submit = function() {{
		if (this.submitUpload) {{
			for (var i = 0 ; i < this.submitUpload.length ; i++) {{
				this.submitUpload[i]() ;
			}}
		}}
		this.originalSubmit() ;
	}}
}}

try {{
	parent.UploadLoaded();
}}
catch(e){{
}}

</script>
</body></html>
				", sLanguage, base.PublishmentSystemID, sType, sStyleName, sAllowExt);
			Response.Write(builder.ToString());
		}

		private void DoSave()
		{

			DoUpload_ASPDotNet();

			string s_SmallImagePathFile = string.Empty;
			string s_SmallImageScript = string.Empty;

			OutScript("parent.UploadSaved('" + sPathFileName + "','" + s_SmallImagePathFile + "');var obj=parent.dialogArguments.dialogArguments;if (!obj) obj=parent.dialogArguments;try{obj.addUploadFile('', '" + sSaveFileName + "', '" + sPathFileName + "');} catch(e){} " + s_SmallImageScript);
		}


		private void InitUpload()
		{
			this.sType = Request.QueryString["type"].Trim().ToUpper();
			this.sStyleName = Request.QueryString["style"].Trim();
			this.sLanguage = Request.QueryString["language"].Trim();

            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                switch (this.sType)
                {
                    case "FILE":
                        this.sAllowExt = base.PublishmentSystemInfo.Additional.FileUploadTypeCollection;
                        this.nAllowSize = base.PublishmentSystemInfo.Additional.FileUploadTypeMaxSize;
                        break;
                    case "MEDIA":
                        this.sAllowExt = base.PublishmentSystemInfo.Additional.VideoUploadTypeCollection;
                        this.nAllowSize = base.PublishmentSystemInfo.Additional.VideoUploadTypeMaxSize;
                        break;
                    default:
                        this.sAllowExt = base.PublishmentSystemInfo.Additional.ImageUploadTypeCollection;
                        this.nAllowSize = base.PublishmentSystemInfo.Additional.ImageUploadTypeMaxSize;
                        break;
                }
            }
            else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                switch (this.sType)
                {
                    case "FILE":
                        this.sAllowExt = UserConfigManager.Additional.UploadFileTypeCollection;
                        this.nAllowSize = UserConfigManager.Additional.UploadFileTypeMaxSize;
                        break;
                    case "MEDIA":
                        this.sAllowExt = UserConfigManager.Additional.UploadMediaTypeCollection;
                        this.nAllowSize = UserConfigManager.Additional.UploadMediaTypeMaxSize;
                        break;
                    default:
                        this.sAllowExt = UserConfigManager.Additional.UploadImageTypeCollection;
                        this.nAllowSize = UserConfigManager.Additional.UploadImageTypeMaxSize;
                        break;
                }
            }
		}

		

		public void CheckValidExt(string sExt)
		{
            bool allow = true;
            if (!string.IsNullOrEmpty(this.sAllowExt))
            {
                allow = PathUtils.IsFileExtenstionAllowed(this.sAllowExt, sExt);
            }
             
			if (allow == false)
			{
				OutScript(string.Format(@"parent.UploadError('lang[""ErrUploadInvalidExt""]+"":{0}""')", this.sAllowExt));
			}
		}

		private void OutScript(string str)
		{
			Response.Write("<script language=javascript>" + str + ";history.back()</s" + "cript>");
			Response.End();
		}


	    private void DoUpload_ASPDotNet()
		{
			if (System.Web.HttpContext.Current.Request.Files != null)
			{
				System.Web.HttpPostedFile postedFile = System.Web.HttpContext.Current.Request.Files[0];
				string filePath = postedFile.FileName;
				string fileExtName = System.IO.Path.GetExtension(filePath);
				fileExtName = fileExtName.Substring(1, fileExtName.Length - 1);
				CheckValidExt(fileExtName);

                if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    this.sSaveFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                }
                else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    this.sSaveFileName = PathUtils.GetUserUploadFileName(filePath);
                }

				if (postedFile.ContentLength > this.nAllowSize*1024)
				{
					OutScript(@"parent.UploadError('lang[""ErrUploadSizeLimit""]+"":" + nAllowSize + @"KB""')");
				}

                string localDirectoryPath = string.Empty;
                if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                }
                else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(BaiRongDataProvider.UserDAO.CurrentUserName);
                }
 
				string localFileName = sSaveFileName;
                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

				postedFile.SaveAs(localFilePath);

                if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                    this.sPathFileName = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                }
                else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    this.sPathFileName = PageUtils.GetUserUploadFileUrl(BaiRongDataProvider.UserDAO.CurrentUserName, this.sSaveFileName);
                }
			}
		}


	}
}
