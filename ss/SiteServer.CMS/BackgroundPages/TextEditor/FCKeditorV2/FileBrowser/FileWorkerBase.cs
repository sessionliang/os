/*
 * FCKeditor - The text editor for Internet - http://www.fckeditor.net
 * Copyright (C) 2003-2007 Frederico Caldeira Knabben
 *
 * == BEGIN LICENSE ==
 *
 * Licensed under the terms of any of the following licenses at your
 * choice:
 *
 *  - GNU General Public License Version 2 or later (the "GPL")
 *    http://www.gnu.org/licenses/gpl.html
 *
 *  - GNU Lesser General Public License Version 2.1 or later (the "LGPL")
 *    http://www.gnu.org/licenses/lgpl.html
 *
 *  - Mozilla Public License Version 1.1 or later (the "MPL")
 *    http://www.mozilla.org/MPL/MPL-1.1.html
 *
 * == END LICENSE ==
 *
 * Base class used by the FileBrowserConnector and Uploader.
 */

using System;
using System.Web;
using System.Text.RegularExpressions;
using SiteServer.CMS.Core;
using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages.FredCK.FCKeditorV2.FileBrowser
{
	public abstract class FileWorkerBase : System.Web.UI.Page
	{
		public Config Config;

		protected void FileUpload( string resourceType, string currentFolder, bool isQuickUpload )
		{
			HttpPostedFile oFile = Request.Files[ "NewFile" ];

			if ( oFile == null )
			{
				this.SendFileUploadResponse( 202, isQuickUpload );
				return;
			}

			// Get the uploaded file name.
            //sFileName = System.IO.Path.GetFileName( oFile.FileName );
            //sFileName = this.SanitizeFileName( sFileName );

			string sExtension = System.IO.Path.GetExtension( oFile.FileName );
			sExtension = sExtension.TrimStart( '.' );

			if ( !this.Config.TypeConfig[ resourceType ].CheckIsAllowedExtension( sExtension ) )
			{
				this.SendFileUploadResponse( 202, isQuickUpload );
				return;
			}

			if ( this.Config.CheckIsNonHtmlExtension( sExtension ) && !this.CheckNonHtmlFile( oFile ) )
			{
				this.SendFileUploadResponse( 202, isQuickUpload );
				return;
			}

            //string sFilePath = string.Empty;

            //while ( true )
            //{
            //    sFilePath = System.IO.Path.Combine(sServerDir, sFileName);

            //    if ( System.IO.File.Exists( sFilePath ) )
            //    {
            //        iCounter++;
            //        sFileName =
            //            System.IO.Path.GetFileNameWithoutExtension( oFile.FileName ) +
            //            "(" + iCounter + ")." +
            //            sExtension;

            //        iErrorNumber = 201;
            //    }
            //    else
            //    {
            //        oFile.SaveAs( sFilePath );
            //        break;
            //    }
            //}

            //TypeConfig typeConfig = this.Config.TypeConfig[resourceType] ;

            ////string sFileUrl = isQuickUpload ? typeConfig.GetQuickUploadPath() : typeConfig.GetFilesPath() ;
            ////sFileUrl += sFileName;
            //string sFileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(Config.PublishmentSystemInfo, sFilePath);

            string filePath = oFile.FileName;
            string imageUrl = string.Empty;
            string fileName = string.Empty;
            try
            {
                string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                string localDirectoryPath = PathUtility.GetUploadDirectoryPath(Config.PublishmentSystemInfo, fileExtName);
                fileName = PathUtility.GetUploadFileName(Config.PublishmentSystemInfo, filePath);
                string localFilePath = PathUtils.Combine(localDirectoryPath, fileName);

                oFile.SaveAs(localFilePath);

                if (fileExtName == ".jpg" || fileExtName == ".jpeg" || fileExtName == ".gif" || fileExtName == ".bmp" || fileExtName == ".png" || fileExtName == ".tif" || fileExtName == ".swf")
                {
                    bool isImage = true;
                    if (fileExtName == ".swf")
                    {
                        isImage = false;
                    }

                    if (isImage)
                    {
                        FileUtility.AddWaterMark(Config.PublishmentSystemInfo, localFilePath);
                    }

                    imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(Config.PublishmentSystemInfo, localFilePath);
                }
            }
            catch { }

            this.SendFileUploadResponse(0, isQuickUpload, imageUrl, fileName);
		}

		private void SendFileUploadResponse( int errorNumber, bool isQuickUpload )
		{
			this.SendFileUploadResponse( errorNumber, isQuickUpload, "", "", "" );
		}

		private void SendFileUploadResponse( int errorNumber, bool isQuickUpload, string fileUrl, string fileName )
		{
			this.SendFileUploadResponse( errorNumber, isQuickUpload, fileUrl, fileName, "" );
		}

		protected void SendFileUploadResponse( int errorNumber, bool isQuickUpload, string fileUrl, string fileName, string customMsg )
		{
			Response.Clear();

			Response.Write( "<script type=\"text/javascript\">" );

			if ( isQuickUpload )
				Response.Write( "window.parent.OnUploadCompleted(" + errorNumber + ",'" + fileUrl.Replace( "'", "\\'" ) + "','" + fileName.Replace( "'", "\\'" ) + "','" + customMsg.Replace( "'", "\\'" ) + "') ;" );
			else
				Response.Write( "window.parent.frames['frmUpload'].OnUploadCompleted(" + errorNumber + ",'" + fileName.Replace( "'", "\\'" ) + "') ;" );

			Response.Write( "</script>" );

			Response.End();
		}


		// Do a cleanup of the folder name to avoid possible problems
		protected string SanitizeFolderName( string folderName )
		{
			// Remove . \ / | : ? * " < >
			return Regex.Replace( folderName, @"[.\\/|:?*""<>]", "_", RegexOptions.None );
		}

		// Do a cleanup of the file name to avoid possible problems
		private string SanitizeFileName( string fileName )
		{
			// Replace dots in the name with underscores (only one dot can be there... security issue).
			if ( Config.ForceSingleExtension )
				fileName = Regex.Replace( fileName, @"\.(?![^.]*$)", "_", RegexOptions.None );

			// Remove \ / | : ? * " < >
			return Regex.Replace( fileName, @"[\\/|:?*""<>]", "_", RegexOptions.None );
		}

		private bool CheckNonHtmlFile( HttpPostedFile file )
		{
			byte[] buffer = new byte[ 1024 ];
			file.InputStream.Read( buffer, 0, 1024 );

			string firstKB = System.Text.ASCIIEncoding.ASCII.GetString( buffer );

			if ( Regex.IsMatch( firstKB, @"<!DOCTYPE\W*X?HTML", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			if ( Regex.IsMatch( firstKB, @"<(?:body|head|html|img|pre|script|table|title)", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//type = javascript
			if ( Regex.IsMatch( firstKB, @"type\s*=\s*[\'""]?\s*(?:\w*/)?(?:ecma|java)", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//href = javascript
			//src = javascript
			//data = javascript
			if ( Regex.IsMatch( firstKB, @"(?:href|src|data)\s*=\s*[\'""]?\s*(?:ecma|java)script:", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//url(javascript
			if ( Regex.IsMatch( firstKB, @"url\s*\(\s*[\'""]?\s*(?:ecma|java)script:", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			return true;
		}
	}
}
