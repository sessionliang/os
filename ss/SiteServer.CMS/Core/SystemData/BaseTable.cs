using System;
using System.Collections;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Core
{
	/// <summary>
	/// AuxiliaryTable ��ժҪ˵����
	/// </summary>
	public class BaseTable
	{
		private BaseTable()
		{
		}

        //public static string GetSqlScript(string fileName)
        //{
        //    string filePath = PathUtils.PhysicalSiteServerPath + string.Format("\\inc\\SqlScript\\{0}.sql", fileName);
        //    string CMD = FileUtils.ReadText(filePath, ECharset.utf_8);
        //    return CMD;
        //}

		public static ArrayList GetDefaultMenuDisplayArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();

			MenuDisplayInfo menuDisplayInfo = new MenuDisplayInfo(0, publishmentSystemID, "ϵͳ�˵���ʾ��ʽ", "true", "", 12, "plain", "", "center", "middle", "#000000", "#F2F2F2", "#FFFFFF", "#CCCCCC", "-10", "20", "true", 115, 24, 0, 0, 0, 500, "true", 1, "#A8A8A8", "", "#A8A8A8", "~/sitefiles/bairong/Icons/arrows.gif", DateTime.Now, true, "ϵͳ�˵���ʾ��ʽ");
			arraylist.Add(menuDisplayInfo);

			return arraylist;
		}

        public static PublishmentSystemInfo GetDefaultPublishmentSystemInfo(string publishmentSystemName, EPublishmentSystemType publishmentSystemType, string auxiliaryTableForContent, string auxiliaryTableForGoods, string auxiliaryTableForBrand, string auxiliaryTableForGovPublic, string auxiliaryTableForGovInteract, string auxiliaryTableForVote, string auxiliaryTableForJob, string publishmentSystemDir, string publishmentSystemUrl, int parentPublishmentSystemID, string groupSN)
		{
            PublishmentSystemInfo psInfo = new PublishmentSystemInfo(0, publishmentSystemName, publishmentSystemType, auxiliaryTableForContent, auxiliaryTableForGoods, auxiliaryTableForBrand, auxiliaryTableForGovPublic, auxiliaryTableForGovInteract, auxiliaryTableForVote, auxiliaryTableForJob, false, 0, publishmentSystemDir, publishmentSystemUrl, false, parentPublishmentSystemID, groupSN, 0, string.Empty);
			return psInfo;
		}


		/// <summary>
		/// Ĭ�ϵķ���ϵͳ������˴���
		/// </summary>
		public static int DefaultContentCheckNum
		{
			get
			{
				return 1;
			}
		}


		/// <summary>
		/// Ĭ�ϵķ���ϵͳ��Ŀ��˴���
		/// </summary>
		public static int DefaultChannelCheckNum
		{
			get
			{
				return 1;
			}
		}
	}
}
