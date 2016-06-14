using System;
using System.Net;
using System.Management;

using BaiRong.Core;
using System.Web;

namespace BaiRong.Core.Diagnostics
{
	/// <summary>
	/// ��ȡ���г���ļ������Ϣ����
	/// </summary>
	public sealed class ComputerUtils
	{
		private ComputerUtils()
		{
		}


		/// <summary>
		/// ��ȡ�������������ַ������������ַ�еķָ�����:����ȥ
		/// </summary>
		/// <returns>�������������ַ</returns>
		public static string GetMacAddress()
		{
			string macAddress = CacheUtils.Get("ComputerUtils_MacAddress") as String;
			if (string.IsNullOrEmpty(macAddress))
			{
				macAddress = string.Empty;
				try
				{
					ManagementClass mcMAC = new ManagementClass("Win32_NetworkAdapterConfiguration");
					ManagementObjectCollection mocMAC = mcMAC.GetInstances();
					foreach(ManagementObject m in mocMAC)
					{
						if((bool)m["IPEnabled"])
						{
							macAddress = m["MacAddress"].ToString();
							break;
						}
					}
				}
				catch{}
				macAddress = macAddress.Replace(":", "");
				CacheUtils.Max("ComputerUtils_MacAddress", macAddress);
			}
			return macAddress;
		}


		/// <summary>
		/// ��ȡ�������CPU��ʶ
		/// </summary>
		/// <returns>�������CPU��ʶ</returns>
		public static string GetProcessorId()
		{
			string processorId = CacheUtils.Get("ComputerUtils_ProcessorId") as String;
			if (string.IsNullOrEmpty(processorId))
			{
				processorId = string.Empty;
				try
				{
					ManagementClass mcCpu = new ManagementClass("win32_Processor");
					ManagementObjectCollection mocCpu = mcCpu.GetInstances();
					foreach(ManagementObject m in mocCpu)
					{
						processorId = m["ProcessorId"].ToString();
					}
				}
				catch{}
				processorId = processorId.Replace(":", "");
				CacheUtils.Max("ComputerUtils_ProcessorId", processorId);
			}
			return processorId;
		}


		/// <summary>
		/// ��ȡ�������Ӳ�����к�
		/// </summary>
		/// <returns>�������Ӳ�����к�</returns>
		public static string GetColumnSerialNumber()
		{
			string columnSerialNumber = CacheUtils.Get("ComputerUtils_ColumnSerialNumber") as String;
			if (string.IsNullOrEmpty(columnSerialNumber))
			{
				columnSerialNumber = string.Empty;
				try
				{
					ManagementClass mcHD = new ManagementClass("win32_logicaldisk");
					ManagementObjectCollection mocHD = mcHD.GetInstances();
					foreach(ManagementObject m in mocHD)
					{
						if(m["DeviceID"].ToString() == "C:")
						{
							columnSerialNumber = m["VolumeSerialNumber"].ToString();
							break;
						}
					}
				}
				catch{}
				columnSerialNumber = columnSerialNumber.Replace(":", "");
				CacheUtils.Max("ComputerUtils_ColumnSerialNumber", columnSerialNumber);
			}
			return columnSerialNumber;
		}


		/// <summary>
		/// ��ȡ������ı�ʶ����ʶ��������ַ��CPU��ʶ�Լ�Ӳ�����к���ɣ��м��á�:���ָ�
		/// </summary>
		/// <returns></returns>
		public static string GetComputerID()
		{
			return string.Format("{0}:{1}:{2}", ComputerUtils.GetMacAddress(), ComputerUtils.GetProcessorId(), ComputerUtils.GetColumnSerialNumber());
		}


		/// <summary>
		/// �õ���������
		/// </summary>
		/// <returns></returns>
		public static string GetHostName()
		{
			return Dns.GetHostName().ToUpper();
		}

        //public static string GetIP()
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
        //        {
        //            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
        //        }
        //        else
        //        {
        //            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        }
        //    }
        //    return string.Empty;
        //}
	}
}
