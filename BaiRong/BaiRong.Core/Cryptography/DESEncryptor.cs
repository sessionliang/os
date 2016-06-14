using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace BaiRong.Core.Cryptography
{
	/// <summary>
	/// Version: 2.0
	/// LastEditDate: 16:14 2005-10-6
	/// </summary>
	public class DESEncryptor
	{
		#region ˽�г�Ա
		/// <summary>
		/// �����ַ���
		/// </summary>
		private string inputString = null;
		/// <summary>
		/// ����ַ���
		/// </summary>
		private string outString = null;
		/// <summary>
		/// �����ļ�·��
		/// </summary>
		private string inputFilePath = null;
		/// <summary>
		/// ����ļ�·��
		/// </summary>
		private string outFilePath = null;
		/// <summary>
		/// ������Կ
		/// </summary>
		private string encryptKey = null;
		/// <summary>
		/// ������Կ
		/// </summary>
		private string decryptKey = null;
		/// <summary>
		/// ��ʾ��Ϣ
		/// </summary>
		private string noteMessage = null;
		#endregion
		#region ��������
		/// <summary>
		/// �����ַ���
		/// </summary>
		public string InputString
		{
			get { return inputString; }
			set { inputString = value; }
		}
		/// <summary>
		/// ����ַ���
		/// </summary>
		public string OutString
		{
			get { return outString; }
			set { outString = value; }
		}
		/// <summary>
		/// �����ļ�·��
		/// </summary>
		public string InputFilePath
		{
			get { return inputFilePath; }
			set { inputFilePath = value; }
		}
		/// <summary>
		/// ����ļ�·��
		/// </summary>
		public string OutFilePath
		{
			get { return outFilePath; }
			set { outFilePath = value; }
		}
		/// <summary>
		/// ������Կ
		/// </summary>
		public string EncryptKey
		{
			get { return encryptKey; }
			set { encryptKey = value; }
		}
		/// <summary>
		/// ������Կ
		/// </summary>
		public string DecryptKey
		{
			get { return decryptKey; }
			set { decryptKey = value; }
		}
		/// <summary>
		/// ������Ϣ
		/// </summary>
		public string NoteMessage
		{
			get { return noteMessage; }
			set { noteMessage = value; }
		}
		#endregion
		#region ���캯��
		public DESEncryptor()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#endregion
		#region DES�����ַ���
		/// <summary>
		/// �����ַ���
		/// ע��:��Կ����Ϊ��λ
		/// </summary>
		/// <param name="strText">�ַ���</param>
		/// <param name="encryptKey">��Կ</param>
		public void DesEncrypt()
		{
			byte[] byKey = null;
			byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			try
			{
                byKey = System.Text.Encoding.UTF8.GetBytes(this.encryptKey.Length > 8 ? this.encryptKey.Substring(0, 8) : this.encryptKey);
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				byte[] inputByteArray = Encoding.UTF8.GetBytes(this.inputString);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				this.outString = Convert.ToBase64String(ms.ToArray());
			}
			catch (System.Exception error)
			{
				this.noteMessage = error.Message;
			}
		}
		#endregion
		#region DES�����ַ���
		/// <summary>
		/// �����ַ���
		/// </summary>
		/// <param name="this.inputString">�����ܵ��ַ���</param>
		/// <param name="decryptKey">��Կ</param>
		public void DesDecrypt()
		{
			byte[] byKey = null;
			byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			byte[] inputByteArray = new Byte[this.inputString.Length];
			try
			{
				byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				inputByteArray = Convert.FromBase64String(this.inputString);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				System.Text.Encoding encoding = new System.Text.UTF8Encoding();
				this.outString = encoding.GetString(ms.ToArray());
			}
			catch (System.Exception error)
			{
				this.noteMessage = error.Message;
			}
		}
		#endregion
		#region DES�����ļ�
		/// <summary>
		/// DES�����ļ�
		/// </summary>
		/// <param name="this.inputFilePath">Դ�ļ�·��</param>
		/// <param name="this.outFilePath">����ļ�·��</param>
		/// <param name="encryptKey">��Կ</param>
		public void FileDesEncrypt()
		{
			byte[] byKey = null;
			byte[] IV = { 0x12, 0x44, 0x16, 0xEE, 0x88, 0x15, 0xDD, 0x41 };//�������з���һЩ�������
			try
			{
				byKey = System.Text.Encoding.UTF8.GetBytes(this.encryptKey.Substring(0, 8));
				FileStream fin = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
				FileStream fout = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fout.SetLength(0);
				//Create variables to help with read and write.
				byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
				long rdlen = 0;              //This is the total number of bytes written.
				long totlen = fin.Length;    //This is the total length of the input file.
				int len;                     //This is the number of bytes to be written at a time.
				DES des = new DESCryptoServiceProvider();
				CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);


				//Read from the input file, then encrypt and write to the output file.
				while (rdlen < totlen)
				{
					len = fin.Read(bin, 0, 100);
					encStream.Write(bin, 0, len);
					rdlen = rdlen + len;
				}

				encStream.Close();
				fout.Close();
				fin.Close();


			}
			catch (System.Exception error)
			{
				this.noteMessage = error.Message.ToString();

			}
		}
		#endregion
		#region DES�����ļ�
		/// <summary>
		/// �����ļ�
		/// </summary>
		/// <param name="this.inputFilePath">�����˵��ļ�·��</param>
		/// <param name="this.outFilePath">����ļ�·��</param>
		/// <param name="decryptKey">��Կ</param>
		public void FileDesDecrypt()
		{
			byte[] byKey = null;
			byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			try
			{
				byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
				FileStream fin = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
				FileStream fout = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fout.SetLength(0);
				//Create variables to help with read and write.
				byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
				long rdlen = 0;              //This is the total number of bytes written.
				long totlen = fin.Length;    //This is the total length of the input file.
				int len;                     //This is the number of bytes to be written at a time.
				DES des = new DESCryptoServiceProvider();
				CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);


				//Read from the input file, then encrypt and write to the output file.
				while (rdlen < totlen)
				{
					len = fin.Read(bin, 0, 100);
					encStream.Write(bin, 0, len);
					rdlen = rdlen + len;
				}

				encStream.Close();
				fout.Close();
				fin.Close();
			}
			catch (System.Exception error)
			{
				this.noteMessage = error.Message.ToString();
			}
		}
		#endregion
		#region MD5
		/// <summary>
		/// MD5 Encrypt
		/// </summary>
		/// <param name="strText">text</param>
		/// <returns>md5 Encrypt string</returns>
		public void MD5Encrypt()
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(this.inputString));
			this.outString = System.Text.Encoding.Default.GetString(result);
		}
		#endregion

	}
}

