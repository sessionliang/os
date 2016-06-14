using System;
using System.Collections;

namespace BaiRong.Model
{
    public class UserAttribute
    {
        protected UserAttribute()
        {
        }

        public const string UserID = "UserID";
        public const string GroupSN = "GroupSN";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string PasswordFormat = "PasswordFormat";
        public const string PasswordSalt = "PasswordSalt";
        public const string GroupID = "GroupID";
        public const string Credits = "Credits";
        public const string LevelID = "LevelID";
        public const string Cash = "Cash";
        public const string CreateDate = "CreateDate";
        public const string CreateIPAddress = "CreateIPAddress";
        public const string LastActivityDate = "LastActivityDate";
        public const string IsChecked = "IsChecked";
        public const string IsLockedOut = "IsLockedOut";
        public const string IsTemporary = "IsTemporary";
        public const string DisplayName = "DisplayName";
        public const string Email = "Email";
        public const string Mobile = "Mobile";
        //settings
        public const string OnlineSeconds = "OnlineSeconds";
        public const string AvatarLarge = "AvatarLarge";
        public const string AvatarMiddle = "AvatarMiddle";
        public const string AvatarSmall = "AvatarSmall";
        public const string Signature = "Signature";
        public const string SettingsXML = "SettingsXML";

        public const string LoginNum = "LoginNum";
        //public const string SCQU = "SCQU";//�ܱ�����
        //public const string IsBindEmail = "IsBindEmail";//�Ƿ������
        //public const string IsBindPhone = "IsBindPhone";//�Ƿ���ֻ�
        //public const string IsSetSCQU = "IsSetSCQU";//�Ƿ������ܱ�
        //public const string LoginFailCounter = "LoginFailCounter";//��¼ʧ�ܴ���
        //public const string LockedTime = "LockedTime";//��������ʱ��
        //public const string Location = "Location";//���ڵ�

        //��Ҫȷ�ϵ��ֶ�
        public const string Birthday = "Birthday"; //����
        public const string BloodType = "BloodType"; //Ѫ��
        public const string Gender = "Gender";//�Ա�
        //public const string Height = "Height"; //���
        public const string MaritalStatus = "MaritalStatus";//����״��
        public const string Education = "Education";//�����̶�
        public const string Graduation = "Graduation";//��ҵԺУ
        //public const string BodyType = "BodyType";//����
        public const string Profession = "Profession";//��ҵ
        //public const string IncomeLevel = "IncomeLevel";//����ˮƽ
        public const string Address = "Address";//ͨѶ��ַ
        public const string QQ = "QQ";
        public const string WeiBo = "WeiBo";
        public const string WeiXin = "WeiXin";
        public const string Interests = "Interests";//��Ȥ
        public const string Organization = "Organization"; //��˾
        public const string Department = "Department"; //����
        public const string Position = "Position";//ְ��

        public const string NewGroupID = "NewGroupID";//by 20160119 �����µ��û��鹦��
        public const string MLibNum = "MLibNum";//by 20160119 �����µ�Ͷ��ϵͳ

        private static ArrayList userAttributes;
        public static ArrayList UserAttributes
        {
            get
            {
                if (userAttributes == null)
                {
                    userAttributes = new ArrayList();
                    userAttributes.AddRange(UserAttribute.BasicAttributes);
                    userAttributes.AddRange(UserAttribute.HiddenAttributes);
                }

                return userAttributes;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(UserID.ToLower());
                    hiddenAttributes.Add(GroupSN.ToLower());
                    hiddenAttributes.Add(Password.ToLower());
                    hiddenAttributes.Add(PasswordFormat.ToLower());
                    hiddenAttributes.Add(PasswordSalt.ToLower());
                    hiddenAttributes.Add(GroupID.ToLower());
                    hiddenAttributes.Add(Credits.ToLower());
                    hiddenAttributes.Add(LevelID.ToLower());
                    hiddenAttributes.Add(Cash.ToLower());
                    hiddenAttributes.Add(CreateDate.ToLower());
                    hiddenAttributes.Add(CreateIPAddress.ToLower());
                    hiddenAttributes.Add(LastActivityDate.ToLower());
                    hiddenAttributes.Add(IsChecked.ToLower());
                    hiddenAttributes.Add(IsLockedOut.ToLower());
                    hiddenAttributes.Add(IsTemporary.ToLower());
                    hiddenAttributes.Add(OnlineSeconds.ToLower());
                    hiddenAttributes.Add(SettingsXML.ToLower());

                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(DisplayName.ToLower());
                    hiddenAttributes.Add(AvatarLarge.ToLower());
                    hiddenAttributes.Add(AvatarMiddle.ToLower());
                    hiddenAttributes.Add(AvatarSmall.ToLower());
                    hiddenAttributes.Add(Signature.ToLower());
                    hiddenAttributes.Add(LoginNum.ToLower());
                    hiddenAttributes.Add(Email.ToLower());
                    hiddenAttributes.Add(Mobile.ToLower());

                    hiddenAttributes.Add(NewGroupID.ToLower());
                    hiddenAttributes.Add(MLibNum.ToLower());
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList basicAttributes;

        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    basicAttributes.Add(Birthday.ToLower());
                    basicAttributes.Add(BloodType.ToLower());
                    basicAttributes.Add(Gender.ToLower());
                    //basicAttributes.Add(Height.ToLower());
                    basicAttributes.Add(MaritalStatus.ToLower());
                    basicAttributes.Add(Education.ToLower());
                    basicAttributes.Add(Graduation.ToLower());
                    //basicAttributes.Add(BodyType.ToLower());
                    basicAttributes.Add(Profession.ToLower());
                    //basicAttributes.Add(IncomeLevel.ToLower());
                    basicAttributes.Add(Address.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                    basicAttributes.Add(WeiBo.ToLower());
                    basicAttributes.Add(WeiXin.ToLower());
                    basicAttributes.Add(Interests.ToLower());
                    basicAttributes.Add(Organization.ToLower());
                    basicAttributes.Add(Department.ToLower());
                    basicAttributes.Add(Position.ToLower());
                }
                return basicAttributes;
            }
        }

    }
}