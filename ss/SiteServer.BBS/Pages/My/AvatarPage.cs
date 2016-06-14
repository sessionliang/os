using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.IO;
using BaiRong.Model;
using BaiRong.Core.Drawing;

namespace SiteServer.BBS.Pages
{
    public class AvatarPage : BasePage
    {
        public HtmlInputFile hifFile;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (this.hifFile.PostedFile != null && "" != this.hifFile.PostedFile.FileName)
                {
                    string filePath = this.hifFile.PostedFile.FileName;
                    FileInfo fileInfo = new FileInfo(filePath);
                    string fileExtName = filePath.ToLower().Substring(filePath.LastIndexOf(".") + 1);
                    EImageType imageType = EImageTypeUtils.GetEnumType(fileExtName);
                    if (imageType != EImageType.Unknown)
                    {
                        string temporaryImagePath = PathUtils.GetTemporaryFilesPath(string.Format("image.{0}", EImageTypeUtils.GetValue(imageType)));
                        this.hifFile.PostedFile.SaveAs(temporaryImagePath);
                        this.hifFile.Dispose();

                        string imageLargeName = string.Format("large_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));
                        string imageMiddleName = string.Format("middle_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));
                        string imageSmallName = string.Format("small_{0}.{1}", StringUtils.GetRandomInt(1, 1000), EImageTypeUtils.GetValue(imageType));

                        UserInfo userInfo = UserManager.Current;

                        string imagePath = PathUtils.GetUserFilesPath(userInfo.UserName, imageLargeName);

                        if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 180, 180, false))
                        {
                            string imageUrl = userInfo.AvatarLarge;
                            FileUtils.DeleteFileIfExists(PathUtils.GetUserFilesPath(userInfo.UserName, imageUrl));
                            userInfo.AvatarLarge = imageLargeName;
                        }
                        imagePath = PathUtils.GetUserFilesPath(userInfo.UserName, imageMiddleName);

                        if (imageType != EImageType.Gif)
                        {
                            if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 120, 120, false))
                            {
                                string imageUrl = userInfo.AvatarMiddle;
                                FileUtils.DeleteFileIfExists(PathUtils.GetUserFilesPath(userInfo.UserName, imageUrl));
                                userInfo.AvatarMiddle = imageMiddleName;
                            }
                        }
                        else
                        {
                            if (FileUtils.CopyFile(temporaryImagePath, imagePath))
                            {
                                string imageUrl = userInfo.AvatarMiddle;
                                FileUtils.DeleteFileIfExists(PathUtils.GetUserFilesPath(userInfo.UserName, imageUrl));
                                userInfo.AvatarMiddle = imageMiddleName;
                            }
                        }

                        imagePath = PathUtils.GetUserFilesPath(userInfo.UserName, imageSmallName);
                        if (ImageUtils.MakeThumbnail(temporaryImagePath, imagePath, 48, 48, false))
                        {
                            string imageUrl = userInfo.AvatarSmall;
                            FileUtils.DeleteFileIfExists(PathUtils.GetUserFilesPath(userInfo.UserName, imageUrl));
                            userInfo.AvatarSmall = imageSmallName;
                        }

                        //UserExtendManager.AddCredits(userInfo.UserName, ECreditRule.SetAvatar);

                        BaiRongDataProvider.UserDAO.Update(userInfo);

                        FileUtils.DeleteFileIfExists(temporaryImagePath);
                        string redirectUrl = "avatar.aspx?isRight=True";
                        PageUtils.Redirect(redirectUrl);
                    }
                }
            }
        }
    }
}
