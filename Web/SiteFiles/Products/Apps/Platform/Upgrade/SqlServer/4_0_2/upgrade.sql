--用户日志
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserLog(
    ID           int              IDENTITY(1,1),
    UserName     varchar(50)      DEFAULT '' NOT NULL,
    IPAddress    varchar(50)      DEFAULT '' NOT NULL,
    AddDate      datetime         DEFAULT getdate() NOT NULL,
    Action       nvarchar(255)    DEFAULT '' NOT NULL,
    Summary      nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserLog PRIMARY KEY CLUSTERED (ID)
)
GO

--定时审核
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Task') AND NAME = 'OnlyOnceDate')
ALTER TABLE bairong_Task ADD
	OnlyOnceDate    DateTime   DEFAULT getdate() NOT NULL
GO

--第三方登录
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserBinding') AND NAME = 'UserID')
ALTER TABLE bairong_UserBinding ADD
    UserID   int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserBinding') AND NAME = 'ThirdLoginType')
ALTER TABLE bairong_UserBinding ADD
	ThirdLoginType  varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserBinding') AND NAME = 'ThirdLoginUserID')
ALTER TABLE bairong_UserBinding ADD
	ThirdLoginUserID nvarchar(255) DEFAULT '' NOT NULL
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bairong_UserBinding') AND NAME = 'PK_bairong_UserBinding' AND XTYPE = 'PK')
ALTER TABLE bairong_UserBinding DROP CONSTRAINT PK_bairong_UserBinding
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bairong_UserBinding') AND NAME = 'PK_bairong_UserBinding' AND XTYPE = 'PK')
ALTER TABLE bairong_UserBinding ADD CONSTRAINT PK_bairong_UserBinding PRIMARY KEY (UserID)
GO

--用户密保问题
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserSecurityQuestion') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserSecurityQuestion(
    ID          int              IDENTITY(1,1),
    Question    nvarchar(255)    DEFAULT '' NOT NULL,
    IsEnable    varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK355 PRIMARY KEY NONCLUSTERED (ID)
)
GO

--用户提醒信息模板
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserNoticeTemplate') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserNoticeTemplate(
    ID                 int             IDENTITY(1,1),
    RelatedIdentity    varchar(50)     DEFAULT '' NOT NULL,
    Name               nvarchar(50)    DEFAULT '' NOT NULL,
    Title              nvarchar(50)    DEFAULT '' NOT NULL,
    Content            ntext           DEFAULT '' NOT NULL,
    Type               varchar(50)     DEFAULT '' NOT NULL,
    IsSystem           varchar(18)     DEFAULT '' NOT NULL,
    IsEnable           varchar(18)     DEFAULT '' NOT NULL,
	RemoteTemplateID   varchar(50)     DEFAULT '' NOT NULL,
    RemoteType         varchar(50)     DEFAULT '' NOT NULL,
    CONSTRAINT PK354 PRIMARY KEY NONCLUSTERED (ID)
)
GO

--用户
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'LoginNum')
ALTER TABLE bairong_Users ADD
	LoginNum int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Birthday')
ALTER TABLE bairong_Users ADD
	Birthday varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'BloodType')
ALTER TABLE bairong_Users ADD
	BloodType varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Gender')
ALTER TABLE bairong_Users ADD
	Gender nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Education')
ALTER TABLE bairong_Users ADD
	Education nvarchar(255) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'MaritalStatus')
ALTER TABLE bairong_Users ADD
	MaritalStatus nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Graduation')
ALTER TABLE bairong_Users ADD
	Graduation nvarchar(255) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Profession')
ALTER TABLE bairong_Users ADD
	Profession nvarchar(255) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'QQ')
ALTER TABLE bairong_Users ADD
	QQ varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'WeiBo')
ALTER TABLE bairong_Users ADD
	WeiBo nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'WeiXin')
ALTER TABLE bairong_Users ADD
	WeiXin nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Interests')
ALTER TABLE bairong_Users ADD
	Interests ntext DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Organization')
ALTER TABLE bairong_Users ADD
	Organization nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Department')
ALTER TABLE bairong_Users ADD
	Department nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Position')
ALTER TABLE bairong_Users ADD
	Position nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Address')
ALTER TABLE bairong_Users ADD
	Address nvarchar(255) DEFAULT '' NOT NULL
GO

--短信服务商
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_SMSServer') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_SMSServer(
    ID                 int             IDENTITY(1,1),
    SmsServerName      nvarchar(50)    DEFAULT '' NOT NULL,
    SmsServerEName     varchar(100)    DEFAULT '' NOT NULL,
    ParamCollection    ntext           DEFAULT '' NOT NULL,
    IsEnable           varchar(18)     DEFAULT '' NOT NULL,
    ExtendValues       ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK357 PRIMARY KEY NONCLUSTERED (ID)
)
GO