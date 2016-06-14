using System;

namespace BaiRong.Model
{
	/// <summary>
	/// Indicates the return status for creating a new user.
	/// </summary>
	public enum ECreateUserStatus 
	{ 
		/// <summary>
		/// The user was not created for some unknown reason.
		/// </summary>
		UnknownFailure, 
        
		/// <summary>
		/// The user's account was successfully created.
		/// </summary>
		Created, 
        
		/// <summary>
		/// The user's account was not created because the user's desired username is already being used.
		/// </summary>
		//DuplicateUsername, 
        
		/// <summary>
		/// The user's account was not created because the user's email address is already being used.
		/// </summary>
		DuplicateEmailAddress, 
        
		/// <summary>
		/// The user's account was not created because the user's desired username did not being with an
		/// alphabetic character.
		/// </summary>
		InvalidFirstCharacter,

		/// <summary>
		/// The username has been previously disallowed.
		/// </summary>
		DisallowedUsername,

		/// <summary>
		/// The user record has been updated
		/// </summary>
		Updated,

		/// <summary>
		/// The user record has been deleted
		/// </summary>
		Deleted,

		InvalidQuestionAnswer,

		InvalidPassword,

		/// <summary>
		/// The user's account was not created because the user's email was invalid.
		/// </summary>
		InvalidEmail,

		/// <summary>
		/// The user's account was not created because the user's username was invalid.
		/// </summary>
		InvalidUserName,

		DuplicateUsername
	}

	public class ECreateUserStatusUtils
	{
		public static string GetMessage(ECreateUserStatus status)
		{
			string message = status.ToString();

			switch (status) 
			{
				case ECreateUserStatus.DisallowedUsername:
					message = "���ø��˺ţ�������ѡ�������˺š�";
					break;

				case ECreateUserStatus.DuplicateUsername:
					message = "�Բ�����������˺��ѱ�ע�ᡣ";
					break;

				case ECreateUserStatus.DuplicateEmailAddress:
					message = "�Բ���������ĵ��������ַ�Ѿ���ע�ᡣ";
					break;

				case ECreateUserStatus.InvalidPassword:
					message = "���볤������Ҫ�����С���볤�ȣ����������롣";
					break;

				case ECreateUserStatus.InvalidQuestionAnswer:
					message = "����ȷ����������ʹ𰸡�";
					break;

				case ECreateUserStatus.InvalidUserName:
					message = "����ȷ���û��������������롣";
					break;
					
				default:
					message = "ע�����û���ʱ�������һ������";
					break;
			}

			return message;
		}
	}

}
