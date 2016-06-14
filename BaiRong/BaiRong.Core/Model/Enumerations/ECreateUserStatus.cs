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
					message = "禁用该账号，请重新选择其他账号。";
					break;

				case ECreateUserStatus.DuplicateUsername:
					message = "对不起，您输入的账号已被注册。";
					break;

				case ECreateUserStatus.DuplicateEmailAddress:
					message = "对不起，您输入的电子邮箱地址已经被注册。";
					break;

				case ECreateUserStatus.InvalidPassword:
					message = "密码长度少于要求的最小密码长度，请重新输入。";
					break;

				case ECreateUserStatus.InvalidQuestionAnswer:
					message = "不正确的密码问题和答案。";
					break;

				case ECreateUserStatus.InvalidUserName:
					message = "不正确的用户名，请重新输入。";
					break;
					
				default:
					message = "注册新用户的时候产生了一个错误。";
					break;
			}

			return message;
		}
	}

}
