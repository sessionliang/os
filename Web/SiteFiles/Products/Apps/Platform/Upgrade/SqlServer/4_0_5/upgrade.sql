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
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_ThirdLogin') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_ThirdLogin(
    ID                int              IDENTITY(1,1),
    ThirdLoginType    varchar(50)      DEFAULT 0 NOT NULL,
    ThirdLoginName    nvarchar(50)     DEFAULT '' NOT NULL,
    IsEnabled         varchar(18)      DEFAULT '' NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    Description       nvarchar(255)    DEFAULT '' NOT NULL,
    SettingsXML       ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ThirdLogin PRIMARY KEY NONCLUSTERED (ID)
)
GO

--第三方登录用户绑定
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

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserNoticeTemplate') AND NAME = 'RemoteTemplateID')
ALTER TABLE bairong_UserNoticeTemplate ADD
	RemoteTemplateID varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserNoticeTemplate') AND NAME = 'RemoteType')
ALTER TABLE bairong_UserNoticeTemplate ADD
	RemoteType varchar(50) DEFAULT '' NOT NULL
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
    CONSTRAINT PK_bairong_SMSServer PRIMARY KEY NONCLUSTERED (ID)
)
GO

--云存储空间
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_CloudStorage') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_CloudStorage(
    StorageID       int            NOT NULL,
    ProviderType    varchar(50)    DEFAULT '' NOT NULL,
    SettingsXML     ntext          DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_CloudStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
GO

--本地存储空间
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_LocalStorage') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_LocalStorage(
    StorageID        int              NOT NULL,
    DirectoryPath    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_LocalStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
GO

--用户联系
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserContact') AND NAME = 'Gender')
ALTER TABLE bairong_UserContact ADD
	Gender   varchar(18) DEFAULT '' NOT NULL
GO

--用户积分
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserCreditsLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserCreditsLog(
    ID             int              IDENTITY(1,1),
    UserName       nvarchar(255)    DEFAULT '' NOT NULL,
    ProductID      varchar(50)      DEFAULT '' NOT NULL,
    IsIncreased    varchar(18)      DEFAULT '' NOT NULL,
    Num            int              DEFAULT 0 NOT NULL,
    Action         nvarchar(255)    DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_UserCreditsLog PRIMARY KEY NONCLUSTERED (ID)
)
GO

--用户下载记录
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserDownload') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserDownload(
    ID                int              IDENTITY(1,1),
    CreateUserName    nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    Downloads         int              DEFAULT 0 NOT NULL,
    FileName          nvarchar(255)    DEFAULT '' NOT NULL,
    FileUrl           varchar(200)     DEFAULT '' NOT NULL,
    Summary           nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate           datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_UserDownload PRIMARY KEY NONCLUSTERED (ID)
)
GO

--用户分组
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserGroup') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserGroup(
    GroupID         int              IDENTITY(1,1),
    GroupSN         nvarchar(255)    DEFAULT '' NOT NULL,
    GroupName       nvarchar(50)     DEFAULT '' NOT NULL,
    GroupType       varchar(50)      DEFAULT '' NOT NULL,
    CreditsFrom     int              DEFAULT 0 NOT NULL,
    CreditsTo       int              DEFAULT 0 NOT NULL,
    Stars           int              DEFAULT 0 NOT NULL,
    Color           varchar(10)      DEFAULT '' NOT NULL,
    ExtendValues    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserGroup PRIMARY KEY NONCLUSTERED (GroupID)
)
GO

--用户等级
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserLevel') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserLevel(
    ID              int              IDENTITY(1,1),
    LevelSN         nvarchar(255)    DEFAULT '' NOT NULL,
    LevelName       nvarchar(50)     DEFAULT '' NOT NULL,
    LevelType       varchar(50)      DEFAULT '' NOT NULL,
    MinNum          int              DEFAULT 0 NOT NULL,
    MaxNum          int              DEFAULT 0 NOT NULL,
    Stars           int              DEFAULT 0 NOT NULL,
    Color           varchar(10)      DEFAULT '' NOT NULL,
    ExtendValues    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserLevel PRIMARY KEY NONCLUSTERED (ID)
)
GO

--用户消息
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserMessage') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserMessage(
    ID             int              IDENTITY(1,1),
    MessageFrom    nvarchar(255)    DEFAULT '' NOT NULL,
    MessageTo      nvarchar(255)    DEFAULT '' NOT NULL,
    MessageType    varchar(50)      DEFAULT '' NOT NULL,
    ParentID       int              DEFAULT 0 NOT NULL,
    IsViewed       varchar(18)      DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    Content        ntext            DEFAULT '' NOT NULL,
    LastAddDate    datetime         DEFAULT getdate() NOT NULL,
    LastContent    ntext            DEFAULT '' NOT NULL,
	Title          nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserMessage PRIMARY KEY NONCLUSTERED (ID)
)
GO

