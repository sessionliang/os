using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EFileSystemType
	{
		Htm,
		Html,
        SHtml,
        Asp,
        Aspx,
        Php,
        Jsp,
		Txt,
		Xml,
        Json,
		Js,
		Ascx,
		Css,
		Jpg,
		Jpeg,
		Gif,
		Png,
		Bmp,
		Swf,
        Flv,
		Doc,
		Xls,
		Ppt,
        Pdf,
		Mdb,
		Rm,
        Rmb,
        Rmvb,
		Mp3,
		Wav,
		Mid,
		Midi,
		Avi,
		Mpg,
		MPeg,
		Asf,
		Asx,
		Wma,
        Wmv,
        Smi,
		Rar,
		Zip,
		Dll,
        Lic,
        Image,
        Video,
		Directory,
		Unknown
	}

	public class EFileSystemTypeUtils
	{
		public static string GetValue(EFileSystemType type)
		{
			if (type == EFileSystemType.Htm)
			{
				return ".htm";
			}
			else if (type == EFileSystemType.Html)
			{
				return ".html";
            }
            else if (type == EFileSystemType.SHtml)
            {
                return ".shtml";
            }
            else if (type == EFileSystemType.Asp)
            {
                return ".asp";
            }
            else if (type == EFileSystemType.Aspx)
            {
                return ".aspx";
            }
            else if (type == EFileSystemType.Php)
            {
                return ".php";
            }
            else if (type == EFileSystemType.Jsp)
            {
                return ".jsp";
            }
			else if (type == EFileSystemType.Txt)
			{
				return ".txt";
			}
			else if (type == EFileSystemType.Xml)
			{
				return ".xml";
            }
            else if (type == EFileSystemType.Json)
            {
                return ".json";
            }
			else if (type == EFileSystemType.Js)
			{
				return ".js";
			}
			else if (type == EFileSystemType.Ascx)
			{
				return ".ascx";
			}
			else if (type == EFileSystemType.Css)
			{
				return ".css";
			}
			else if (type == EFileSystemType.Jpg)
			{
				return ".jpg";
			}
			else if (type == EFileSystemType.Jpeg)
			{
				return ".jpeg";
			}
			else if (type == EFileSystemType.Gif)
			{
				return ".gif";
			}
			else if (type == EFileSystemType.Png)
			{
				return ".png";
			}
			else if (type == EFileSystemType.Bmp)
			{
				return ".bmp";
			}
			else if (type == EFileSystemType.Swf)
			{
				return ".swf";
            }
            else if (type == EFileSystemType.Flv)
            {
                return ".flv";
            }
			else if (type == EFileSystemType.Doc)
			{
				return ".doc";
			}
			else if (type == EFileSystemType.Xls)
			{
				return ".xls";
			}
			else if (type == EFileSystemType.Ppt)
			{
				return ".ppt";
            }
            else if (type == EFileSystemType.Pdf)
            {
                return ".pdf";
            }
			else if (type == EFileSystemType.Mdb)
			{
				return ".mdb";
			}
			else if (type == EFileSystemType.Rm)
			{
				return ".rm";
            }
            else if (type == EFileSystemType.Rmb)
            {
                return ".rmb";
            }
            else if (type == EFileSystemType.Rmvb)
            {
                return ".rmvb";
            }
			else if (type == EFileSystemType.Mp3)
			{
				return ".mp3";
			}
			else if (type == EFileSystemType.Wav)
			{
				return ".wav";
			}
			else if (type == EFileSystemType.Mid)
			{
				return ".mid";
			}
			else if (type == EFileSystemType.Midi)
			{
				return ".midi";
			}
			else if (type == EFileSystemType.Avi)
			{
				return ".avi";
			}
			else if (type == EFileSystemType.Mpg)
			{
				return ".mpg";
			}
			else if (type == EFileSystemType.MPeg)
			{
				return ".mpeg";
			}
			else if (type == EFileSystemType.Asf)
			{
				return ".asf";
			}
			else if (type == EFileSystemType.Asx)
			{
				return ".asx";
			}
			else if (type == EFileSystemType.Wma)
			{
				return ".wma";
            }
            else if (type == EFileSystemType.Wmv)
            {
                return ".wmv";
            }
            else if (type == EFileSystemType.Smi)
            {
                return ".smi";
            }
			else if (type == EFileSystemType.Rar)
			{
				return ".rar";
			}
			else if (type == EFileSystemType.Zip)
			{
				return ".zip";
			}
			else if (type == EFileSystemType.Dll)
			{
				return ".dll";
            }
            else if (type == EFileSystemType.Lic)
            {
                return ".lic";
            }
            else if (type == EFileSystemType.Image)
            {
                return ".image";
            }
            else if (type == EFileSystemType.Video)
            {
                return ".video";
            }
			else if (type == EFileSystemType.Directory)
			{
				return string.Empty;
			}
			else if (type == EFileSystemType.Unknown)
			{
				return ".unknown";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EFileSystemType type)
		{
			if (type == EFileSystemType.Htm)
			{
				return "HTML Document";
			}
			else if (type == EFileSystemType.Html)
			{
				return "HTML Document";
            }
            else if (type == EFileSystemType.SHtml)
            {
                return "HTML Document";
            }
            else if (type == EFileSystemType.Asp)
            {
                return "Active Server Page";
            }
            else if (type == EFileSystemType.Aspx)
            {
                return "Web Form";
            }
            else if (type == EFileSystemType.Php)
            {
                return "PHP Server Page";
            }
            else if (type == EFileSystemType.Jsp)
            {
                return "Java Server Page";
            }
			else if (type == EFileSystemType.Txt)
			{
				return "文本文档";
			}
			else if (type == EFileSystemType.Xml)
			{
				return "Xml 文档";
            }
            else if (type == EFileSystemType.Json)
            {
                return "Json 文档";
            }
			else if (type == EFileSystemType.Js)
			{
				return "JScript Script File";
            }
			else if (type == EFileSystemType.Ascx)
			{
				return "Web User Control";
			}
			else if (type == EFileSystemType.Css)
			{
				return "Cascading Style Sheet Document";
			}
			else if (type == EFileSystemType.Jpg)
			{
				return "JPEG 图像";
			}
			else if (type == EFileSystemType.Jpeg)
			{
				return "JPEG 图像";
			}
			else if (type == EFileSystemType.Gif)
			{
				return "GIF 图像";
			}
			else if (type == EFileSystemType.Png)
			{
				return "PNG 图像";
			}
			else if (type == EFileSystemType.Bmp)
			{
				return "BMP 图像";
			}
			else if (type == EFileSystemType.Swf)
			{
				return "SWF 文件";
            }
            else if (type == EFileSystemType.Flv)
            {
                return "Flash 歌曲文件";
            }
			else if (type == EFileSystemType.Doc)
			{
				return "Word 文档";
			}
			else if (type == EFileSystemType.Xls)
			{
				return "Excel 工作表";
			}
			else if (type == EFileSystemType.Ppt)
			{
				return "PowerPoint 演示文稿";
            }
            else if (type == EFileSystemType.Pdf)
            {
                return "PDF 文件";
            }
			else if (type == EFileSystemType.Mdb)
			{
				return "Access 应用程序";
			}
			else if (type == EFileSystemType.Rm)
			{
				return "RealPlay 格式声音";
            }
            else if (type == EFileSystemType.Rmb)
            {
                return "RealPlay 格式声音";
            }
            else if (type == EFileSystemType.Rmvb)
            {
                return "RealPlay 格式声音";
            }
			else if (type == EFileSystemType.Mp3)
			{
				return "MP3 格式声音";
			}
			else if (type == EFileSystemType.Wav)
			{
				return "波形声音";
			}
			else if (type == EFileSystemType.Mid)
			{
				return "MIDI 序列";
			}
			else if (type == EFileSystemType.Midi)
			{
				return "MIDI 序列";
			}
			else if (type == EFileSystemType.Avi)
			{
				return "歌曲剪辑";
			}
			else if (type == EFileSystemType.Mpg)
			{
				return "电影剪辑";
			}
			else if (type == EFileSystemType.MPeg)
			{
				return "电影剪辑";
			}
			else if (type == EFileSystemType.Asf)
			{
				return "Windows Media 音频/歌曲文件";
			}
			else if (type == EFileSystemType.Asx)
			{
				return "Windows Media 音频/歌曲播放列表";
			}
			else if (type == EFileSystemType.Wma)
			{
				return "Windows Media 音频文件";
            }
            else if (type == EFileSystemType.Wmv)
            {
                return "Windows Media 视频文件";
            }
            else if (type == EFileSystemType.Smi)
            {
                return "Smi 音频文件";
            }
			else if (type == EFileSystemType.Rar)
			{
				return "RAR 压缩文件";
			}
			else if (type == EFileSystemType.Zip)
			{
				return "ZIP 压缩文件";
			}
			else if (type == EFileSystemType.Dll)
			{
				return "Application Extension";
            }
            else if (type == EFileSystemType.Lic)
            {
                return "License 文件";
            }
            else if (type == EFileSystemType.Image)
            {
                return "图片文件";
            }
            else if (type == EFileSystemType.Video)
            {
                return "视频文件";
            }
			else if (type == EFileSystemType.Directory)
			{
				return "文件夹";
			}
			else if (type == EFileSystemType.Unknown)
			{
				return string.Empty;
			}
			else
			{
				throw new Exception();
			}
		}

		public static EFileSystemType GetEnumType(string typeStr)
		{
			EFileSystemType retval = EFileSystemType.Unknown;

			if (Equals(EFileSystemType.Htm, typeStr))
			{
				retval = EFileSystemType.Htm;
			}
			else if (Equals(EFileSystemType.Html, typeStr))
			{
				retval = EFileSystemType.Html;
            }
            else if (Equals(EFileSystemType.SHtml, typeStr))
            {
                retval = EFileSystemType.SHtml;
            }
            else if (Equals(EFileSystemType.Asp, typeStr))
            {
                retval = EFileSystemType.Asp;
            }
            else if (Equals(EFileSystemType.Aspx, typeStr))
            {
                retval = EFileSystemType.Aspx;
            }
            else if (Equals(EFileSystemType.Php, typeStr))
            {
                retval = EFileSystemType.Php;
            }
            else if (Equals(EFileSystemType.Jsp, typeStr))
            {
                retval = EFileSystemType.Jsp;
            }
			else if (Equals(EFileSystemType.Txt, typeStr))
			{
				retval = EFileSystemType.Txt;
			}
			else if (Equals(EFileSystemType.Xml, typeStr))
			{
				retval = EFileSystemType.Xml;
            }
            else if (Equals(EFileSystemType.Json, typeStr))
            {
                retval = EFileSystemType.Json;
            }
			else if (Equals(EFileSystemType.Js, typeStr))
			{
				retval = EFileSystemType.Js;
            }
			else if (Equals(EFileSystemType.Ascx, typeStr))
			{
				retval = EFileSystemType.Ascx;
			}
			else if (Equals(EFileSystemType.Css, typeStr))
			{
				retval = EFileSystemType.Css;
			}
			else if (Equals(EFileSystemType.Jpg, typeStr))
			{
				retval = EFileSystemType.Jpg;
			}
			else if (Equals(EFileSystemType.Jpeg, typeStr))
			{
				retval = EFileSystemType.Jpeg;
			}
			else if (Equals(EFileSystemType.Gif, typeStr))
			{
				retval = EFileSystemType.Gif;
			}
			else if (Equals(EFileSystemType.Png, typeStr))
			{
				retval = EFileSystemType.Png;
			}
			else if (Equals(EFileSystemType.Bmp, typeStr))
			{
				retval = EFileSystemType.Bmp;
			}
			else if (Equals(EFileSystemType.Swf, typeStr))
			{
				retval = EFileSystemType.Swf;
            }
            else if (Equals(EFileSystemType.Flv, typeStr))
            {
                retval = EFileSystemType.Flv;
            }
			else if (Equals(EFileSystemType.Doc, typeStr))
			{
				retval = EFileSystemType.Doc;
			}
			else if (Equals(EFileSystemType.Xls, typeStr))
			{
				retval = EFileSystemType.Xls;
			}
			else if (Equals(EFileSystemType.Ppt, typeStr))
			{
				retval = EFileSystemType.Ppt;
            }
            else if (Equals(EFileSystemType.Pdf, typeStr))
            {
                retval = EFileSystemType.Pdf;
            }
			else if (Equals(EFileSystemType.Mdb, typeStr))
			{
				retval = EFileSystemType.Mdb;
			}
			else if (Equals(EFileSystemType.Rm, typeStr))
			{
				retval = EFileSystemType.Rm;
            }
            else if (Equals(EFileSystemType.Rmb, typeStr))
            {
                retval = EFileSystemType.Rmb;
            }
            else if (Equals(EFileSystemType.Rmvb, typeStr))
            {
                retval = EFileSystemType.Rmvb;
            }
			else if (Equals(EFileSystemType.Mp3, typeStr))
			{
				retval = EFileSystemType.Mp3;
			}
			else if (Equals(EFileSystemType.Wav, typeStr))
			{
				retval = EFileSystemType.Wav;
			}
			else if (Equals(EFileSystemType.Mid, typeStr))
			{
				retval = EFileSystemType.Mid;
			}
			else if (Equals(EFileSystemType.Midi, typeStr))
			{
				retval = EFileSystemType.Midi;
			}
			else if (Equals(EFileSystemType.Avi, typeStr))
			{
				retval = EFileSystemType.Avi;
			}
			else if (Equals(EFileSystemType.Mpg, typeStr))
			{
				retval = EFileSystemType.Mpg;
			}
			else if (Equals(EFileSystemType.MPeg, typeStr))
			{
				retval = EFileSystemType.MPeg;
			}
			else if (Equals(EFileSystemType.Asf, typeStr))
			{
				retval = EFileSystemType.Asf;
			}
			else if (Equals(EFileSystemType.Asx, typeStr))
			{
				retval = EFileSystemType.Asx;
			}
			else if (Equals(EFileSystemType.Wma, typeStr))
			{
				retval = EFileSystemType.Wma;
            }
            else if (Equals(EFileSystemType.Wmv, typeStr))
            {
                retval = EFileSystemType.Wmv;
            }
            else if (Equals(EFileSystemType.Smi, typeStr))
            {
                retval = EFileSystemType.Smi;
            }
			else if (Equals(EFileSystemType.Rar, typeStr))
			{
				retval = EFileSystemType.Rar;
			}
			else if (Equals(EFileSystemType.Zip, typeStr))
			{
				retval = EFileSystemType.Zip;
			}
			else if (Equals(EFileSystemType.Dll, typeStr))
			{
				retval = EFileSystemType.Dll;
            }
            else if (Equals(EFileSystemType.Lic, typeStr))
            {
                retval = EFileSystemType.Lic;
            }
            else if (Equals(EFileSystemType.Image, typeStr))
            {
                retval = EFileSystemType.Image;
            }
            else if (Equals(EFileSystemType.Video, typeStr))
            {
                retval = EFileSystemType.Video;
            }
			else if (Equals(EFileSystemType.Directory, typeStr))
			{
				retval = EFileSystemType.Directory;
			}

			return retval;
		}

        public static ListItem GetListItem(EFileSystemType type, bool selected)
        {
            ListItem item = new ListItem(GetValue(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddWebPageListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EFileSystemType.Html, false));
                listControl.Items.Add(GetListItem(EFileSystemType.Htm, false));
                listControl.Items.Add(GetListItem(EFileSystemType.SHtml, false));
                listControl.Items.Add(GetListItem(EFileSystemType.Xml, false));
                listControl.Items.Add(GetListItem(EFileSystemType.Json, false));
            }
        }

        /*
         * $mimetypes = array(
    'ez'        => 'application/andrew-inset',
    'hqx'        => 'application/mac-binhex40',
    'cpt'        => 'application/mac-compactpro',
    'doc'        => 'application/msword',
    'bin'        => 'application/octet-stream',
    'dms'        => 'application/octet-stream',
    'lha'        => 'application/octet-stream',
    'lzh'        => 'application/octet-stream',
    'exe'        => 'application/octet-stream',
    'class'        => 'application/octet-stream',
    'so'        => 'application/octet-stream',
    'dll'        => 'application/octet-stream',
    'oda'        => 'application/oda',
    'pdf'        => 'application/pdf',
    'ai'        => 'application/postscript',
    'eps'        => 'application/postscript',
    'ps'        => 'application/postscript',
    'smi'        => 'application/smil',
    'smil'        => 'application/smil',
    'mif'        => 'application/vnd.mif',
    'xls'        => 'application/vnd.ms-excel',
    'ppt'        => 'application/vnd.ms-powerpoint',
    'wbxml'        => 'application/vnd.wap.wbxml',
    'wmlc'        => 'application/vnd.wap.wmlc',
    'wmlsc'        => 'application/vnd.wap.wmlscriptc',
    'bcpio'        => 'application/x-bcpio',
    'vcd'        => 'application/x-cdlink',
    'pgn'        => 'application/x-chess-pgn',
    'cpio'        => 'application/x-cpio',
    'csh'        => 'application/x-csh',
    'dcr'        => 'application/x-director',
    'dir'        => 'application/x-director',
    'dxr'        => 'application/x-director',
    'dvi'        => 'application/x-dvi',
    'spl'        => 'application/x-futuresplash',
    'gtar'        => 'application/x-gtar',
    'hdf'        => 'application/x-hdf',
    'js'        => 'application/x-javascript',
    'skp'        => 'application/x-koan',
    'skd'        => 'application/x-koan',
    'skt'        => 'application/x-koan',
    'skm'        => 'application/x-koan',
    'latex'        => 'application/x-latex',
    'nc'        => 'application/x-netcdf',
    'cdf'        => 'application/x-netcdf',
    'sh'        => 'application/x-sh',
    'shar'        => 'application/x-shar',
    'swf'        => 'application/x-shockwave-flash',
    'sit'        => 'application/x-stuffit',
    'sv4cpio'    => 'application/x-sv4cpio',
    'sv4crc'    => 'application/x-sv4crc',
    'tar'        => 'application/x-tar',
    'tcl'        => 'application/x-tcl',
    'tex'        => 'application/x-tex',
    'texinfo'    => 'application/x-texinfo',
    'texi'        => 'application/x-texinfo',
    't'            => 'application/x-troff',
    'tr'        => 'application/x-troff',
    'roff'        => 'application/x-troff',
    'man'        => 'application/x-troff-man',
    'me'        => 'application/x-troff-me',
    'ms'        => 'application/x-troff-ms',
    'ustar'        => 'application/x-ustar',
    'src'        => 'application/x-wais-source',
    'xhtml'        => 'application/xhtml+xml',
    'xht'        => 'application/xhtml+xml',
    'zip'        => 'application/zip',
    'au'        => 'audio/basic',
    'snd'        => 'audio/basic',
    'mid'        => 'audio/midi',
    'midi'        => 'audio/midi',
    'kar'        => 'audio/midi',
    'mpga'        => 'audio/mpeg',
    'mp2'        => 'audio/mpeg',
    'mp3'        => 'audio/mpeg',
    'aif'        => 'audio/x-aiff',
    'aiff'        => 'audio/x-aiff',
    'aifc'        => 'audio/x-aiff',
    'm3u'        => 'audio/x-mpegurl',
    'ram'        => 'audio/x-pn-realaudio',
    'rm'        => 'audio/x-pn-realaudio',
    'rpm'        => 'audio/x-pn-realaudio-plugin',
    'ra'        => 'audio/x-realaudio',
    'wav'        => 'audio/x-wav',
    'pdb'        => 'chemical/x-pdb',
    'xyz'        => 'chemical/x-xyz',
    'bmp'        => 'image/bmp',
    'gif'        => 'image/gif',
    'ief'        => 'image/ief',
    'jpeg'        => 'image/jpeg',
    'jpg'        => 'image/jpeg',
    'jpe'        => 'image/jpeg',
    'png'        => 'image/png',
    'tiff'        => 'image/tiff',
    'tif'        => 'image/tiff',
    'djvu'        => 'image/vnd.djvu',
    'djv'        => 'image/vnd.djvu',
    'wbmp'        => 'image/vnd.wap.wbmp',
    'ras'        => 'image/x-cmu-raster',
    'pnm'        => 'image/x-portable-anymap',
    'pbm'        => 'image/x-portable-bitmap',
    'pgm'        => 'image/x-portable-graymap',
    'ppm'        => 'image/x-portable-pixmap',
    'rgb'        => 'image/x-rgb',
    'xbm'        => 'image/x-xbitmap',
    'xpm'        => 'image/x-xpixmap',
    'xwd'        => 'image/x-xwindowdump',
    'igs'        => 'model/iges',
    'iges'        => 'model/iges',
    'msh'        => 'model/mesh',
    'mesh'        => 'model/mesh',
    'silo'        => 'model/mesh',
    'wrl'        => 'model/vrml',
    'vrml'        => 'model/vrml',
    'css'        => 'text/css',
    'html'        => 'text/html',
    'htm'        => 'text/html',
    'asc'        => 'text/plain',
    'txt'        => 'text/plain',
    'rtx'        => 'text/richtext',
    'rtf'        => 'text/rtf',
    'sgml'        => 'text/sgml',
    'sgm'        => 'text/sgml',
    'tsv'        => 'text/tab-separated-values',
    'wml'        => 'text/vnd.wap.wml',
    'wmls'        => 'text/vnd.wap.wmlscript',
    'etx'        => 'text/x-setext',
    'xsl'        => 'text/xml',
    'xml'        => 'text/xml',
    'mpeg'        => 'video/mpeg',
    'mpg'        => 'video/mpeg',
    'mpe'        => 'video/mpeg',
    'qt'        => 'video/quicktime',
    'mov'        => 'video/quicktime',
    'mxu'        => 'video/vnd.mpegurl',
    'avi'        => 'video/x-msvideo',
    'movie'        => 'video/x-sgi-movie',
    'ice'        => 'x-conference/x-cooltalk',
);
         * */

        public static string GetResponseContentType(EFileSystemType type)
        {
            if (type == EFileSystemType.Htm)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Html)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.SHtml)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Asp)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Aspx)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Php)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Jsp)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Txt)
            {
                return "text/plain";
			}
			else if (type == EFileSystemType.Xml)
			{
				return "text/plain";
			}
            else if (type == EFileSystemType.Json)
            {
                return "text/plain";
            }
            else if (type == EFileSystemType.Js)
            {
                return "application/x-javascript";
            }
            else if (type == EFileSystemType.Ascx)
            {
                return "text/html";
            }
            else if (type == EFileSystemType.Css)
            {
                return "text/css";
            }
            else if (type == EFileSystemType.Jpg)
            {
                return "image/jpeg";
            }
            else if (type == EFileSystemType.Jpeg)
            {
                return "image/jpeg";
            }
            else if (type == EFileSystemType.Gif)
            {
                return "image/gif";
            }
            else if (type == EFileSystemType.Png)
            {
                return "image/png";
            }
            else if (type == EFileSystemType.Bmp)
            {
                return "image/bmp";
            }
            else if (type == EFileSystemType.Swf)
            {
                return "application/x-shockwave-flash";
            }
            else if (type == EFileSystemType.Flv)
            {
                return "application/x-shockwave-flash";
            }
            else if (type == EFileSystemType.Doc)
            {
                return "application/msword";
            }
            else if (type == EFileSystemType.Xls)
            {
                return "application/vnd.ms-excel";
            }
            else if (type == EFileSystemType.Ppt)
            {
                return "application/vnd.ms-powerpoint";
            }
            else if (type == EFileSystemType.Pdf)
            {
                return "";
            }
            else if (type == EFileSystemType.Mdb)
            {
                return "";
            }
            else if (type == EFileSystemType.Rm)
            {
                return "audio/x-pn-realaudio";
            }
            else if (type == EFileSystemType.Rmb)
            {
                return "audio/x-pn-realaudio";
            }
            else if (type == EFileSystemType.Rmvb)
            {
                return "audio/x-pn-realaudio";
            }
            else if (type == EFileSystemType.Mp3)
            {
                return "audio/mpeg";
            }
            else if (type == EFileSystemType.Wav)
            {
                return "audio/x-wav";
            }
            else if (type == EFileSystemType.Mid)
            {
                return "audio/midi";
            }
            else if (type == EFileSystemType.Midi)
            {
                return "audio/midi";
            }
            else if (type == EFileSystemType.Avi)
            {
                return "video/x-msvideo";
            }
            else if (type == EFileSystemType.Mpg)
            {
                return "video/mpeg";
            }
            else if (type == EFileSystemType.MPeg)
            {
                return "video/mpeg";
            }
            else if (type == EFileSystemType.Asf)
            {
                return "";
            }
            else if (type == EFileSystemType.Asx)
            {
                return "";
            }
            else if (type == EFileSystemType.Wma)
            {
                return "";
            }
            else if (type == EFileSystemType.Wmv)
            {
                return "";
            }
            else if (type == EFileSystemType.Smi)
            {
                return "";
            }
            else if (type == EFileSystemType.Rar)
            {
                return "application/zip";
            }
            else if (type == EFileSystemType.Zip)
            {
                return "application/zip";
            }
            else if (type == EFileSystemType.Dll)
            {
                return "application/octet-stream";
            }
            else if (type == EFileSystemType.Lic)
            {
                return "text/plain";
            }
            else if (type == EFileSystemType.Image)
            {
                return "";
            }
            else if (type == EFileSystemType.Video)
            {
                return "";
            }
            else if (type == EFileSystemType.Directory)
            {
                return "";
            }
            else if (type == EFileSystemType.Unknown)
            {
                return string.Empty;
            }
            else
            {
                throw new Exception();
            }
        }

		public static bool Equals(EFileSystemType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EFileSystemType type)
        {
            return Equals(type, typeStr);
        }

        public static bool IsTextEditable(EFileSystemType type)
        {
            bool editable = false;
            if (type == EFileSystemType.Ascx || type == EFileSystemType.Asp || type == EFileSystemType.Aspx || type == EFileSystemType.Css || type == EFileSystemType.Htm || type == EFileSystemType.Html || type == EFileSystemType.Js || type == EFileSystemType.Jsp || type == EFileSystemType.Php || type == EFileSystemType.SHtml || type == EFileSystemType.Txt || type == EFileSystemType.Xml || type == EFileSystemType.Json || type == EFileSystemType.Lic)
            {
                editable = true;
            }
            return editable;
        }

        public static bool IsHtml(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".asp" || fileExtName == ".aspx" || fileExtName == ".htm" || fileExtName == ".html" || fileExtName == ".jsp" || fileExtName == ".php" || fileExtName == ".shtml")
                {
                    retval = true;
                }
            }
            return retval;
        }

        /// <summary>
        /// 判断是否是样式文件
        /// add by sessionliang at 20151223
        /// </summary>
        /// <param name="fileExtName"></param>
        /// <returns></returns>
        public static bool IsCSS(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".css")
                {
                    retval = true;
                }
            }
            return retval;
        }

        /// <summary>
        /// 判断是否是脚本文件
        /// add by sessionliang at 20151223
        /// </summary>
        /// <param name="fileExtName"></param>
        /// <returns></returns>
        public static bool IsJS(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".js")
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static bool IsImage(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".bmp" || fileExtName == ".gif" || fileExtName == ".jpg" || fileExtName == ".jpeg" || fileExtName == ".png" || fileExtName == ".pneg")
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static bool IsCompressionFile(string typeStr)
        {
            bool retval = false;
            if (EFileSystemTypeUtils.Equals(EFileSystemType.Rar, typeStr) || EFileSystemTypeUtils.Equals(EFileSystemType.Zip, typeStr))
            {
                retval = true;
            }
            return retval;
        }

        public static bool IsFlash(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".swf")
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static bool IsPlayer(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".flv" || fileExtName == ".avi" || fileExtName == ".mpg" || fileExtName == ".mpeg" || fileExtName == ".smi" || fileExtName == ".mp3" || fileExtName == ".mid" || fileExtName == ".midi" || fileExtName == ".rm" || fileExtName == ".rmb" || fileExtName == ".rmvb" || fileExtName == ".wmv" || fileExtName == ".wma" || fileExtName == ".asf" || fileExtName == ".mov" || fileExtName == ".mp4")
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static bool IsImageOrFlashOrPlayer(string fileExtName)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(fileExtName))
            {
                fileExtName = fileExtName.ToLower().Trim();
                if (!fileExtName.StartsWith("."))
                {
                    fileExtName = "." + fileExtName;
                }
                if (fileExtName == ".bmp" || fileExtName == ".gif" || fileExtName == ".jpeg" || fileExtName == ".jpg" || fileExtName == ".png" || fileExtName == ".pneg" || fileExtName == ".swf")
                {
                    retval = true;
                }
                if (retval == false) retval = IsPlayer(fileExtName);
            }
            return retval;
        }

        public static bool IsDownload(EFileSystemType type)
        {
            bool download = false;
            if (EFileSystemTypeUtils.IsTextEditable(type) || EFileSystemTypeUtils.IsImageOrFlashOrPlayer(EFileSystemTypeUtils.GetValue(type)))
            {
                download = true;
            }
            else if (type == EFileSystemType.Pdf || type == EFileSystemType.Doc || type == EFileSystemType.Ppt || type == EFileSystemType.Xls || type == EFileSystemType.Mdb)
            {
                download = true;
            }
            return download;
        }
	}
}
