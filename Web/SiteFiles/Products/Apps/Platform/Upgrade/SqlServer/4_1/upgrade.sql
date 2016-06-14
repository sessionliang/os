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
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserBinding') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserBinding(
    UserID       nvarchar(255)    NOT NULL,
    ThirdLoginType    varchar(50)      DEFAULT '' NOT NULL,
    ThirdLoginUserID    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserBinding PRIMARY KEY NONCLUSTERED (UserID)
)
GO


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
    CONSTRAINT PK_bairong_UserSecurityQuestion PRIMARY KEY NONCLUSTERED (ID)
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
    CONSTRAINT PK_bairong_UserNoticeTemplate PRIMARY KEY NONCLUSTERED (ID)
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
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'GroupID')
ALTER TABLE bairong_Users ADD
	GroupID int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'Credits')
ALTER TABLE bairong_Users ADD
	Credits int DEFAULT 0 NOT NULL
GO

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
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserContact') AND NAME = 'Gender')
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

--用户等级规则
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_LevelRule') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_LevelRule(
    ID             int            IDENTITY(1,1),
    RuleType       varchar(50)    DEFAULT '' NOT NULL,
    PeriodType     varchar(50)    DEFAULT '' NOT NULL,
    PeriodCount    int            DEFAULT 0 NOT NULL,
    MaxNum         int            DEFAULT 0 NOT NULL,
    CreditNum      int            DEFAULT 0 NOT NULL,
    CashNum        int            DEFAULT 0 NOT NULL,
    AppID          varchar(50)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_LevelRule PRIMARY KEY NONCLUSTERED (ID)
)
go

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

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_UserMessage') AND NAME = 'Title')
ALTER TABLE bairong_UserMessage ADD
	Title          nvarchar(255)    DEFAULT '' NOT NULL
GO


--3.6.x升级
--bairong_Administrator
IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Administrator') AND NAME = 'LastModuleID')
EXEC SP_RENAME 'bairong_Administrator.LastModuleID','LastProductID','column'
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Administrator') AND NAME = 'PublishmentSystemIDCollection')
ALTER TABLE bairong_Administrator ADD
	PublishmentSystemIDCollection varchar(50) DEFAULT '' NOT NULL
GO

--bairong_AjaxUrl
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_AjaxUrl(
    SN            varchar(50)     NOT NULL,
    AjaxUrl       varchar(500)    DEFAULT '' NOT NULL,
    Parameters    ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_AjaxUrl PRIMARY KEY NONCLUSTERED (SN)
)
GO

--bairong_ErrorLog
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_ErrorLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_ErrorLog(
    ID            int              IDENTITY(1,1),
    AddDate       datetime         DEFAULT getdate() NOT NULL,
    Message       nvarchar(255)    DEFAULT '' NOT NULL,
    Stacktrace    ntext            DEFAULT '' NOT NULL,
    Summary       ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ErrorLog PRIMARY KEY CLUSTERED (ID)
)
GO

--bairong_Roles
IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Roles') AND NAME = 'Modules')
EXEC SP_RENAME 'bairong_Roles.Modules','ProductIDCollection','column'
GO

--bairong_Users
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'UserID')
ALTER TABLE bairong_Users ADD
	UserID              int              IDENTITY(1,1),
	GroupSN             nvarchar(255)    DEFAULT '' NOT NULL;
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bairong_Users') AND NAME = 'PK_bairong_Users' AND XTYPE = 'PK')
ALTER TABLE bairong_Users DROP CONSTRAINT PK_bairong_Users
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bairong_Users') AND NAME = 'PK_bairong_Users' AND XTYPE = 'PK')
ALTER TABLE bairong_Users ADD CONSTRAINT PK_bairong_Users PRIMARY KEY (UserID)
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bairong_ContentModel') AND NAME = 'PK_bairong_ContentModel' AND XTYPE = 'PK')
ALTER TABLE bairong_ContentModel DROP CONSTRAINT PK_bairong_ContentModel
ALTER TABLE bairong_ContentModel ADD CONSTRAINT PK_bairong_ContentModel PRIMARY KEY (ModelID, SiteID)
GO

--bairong_CreateTask
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_CreateTask') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_CreateTask(
    ID            int             IDENTITY(1,1),
    UserName      nvarchar(50)    DEFAULT '' NOT NULL,
    TotalCount    int             DEFAULT 0 NOT NULL,
    ErrorCount    int             DEFAULT 0 NOT NULL,
    Summary       ntext           DEFAULT '' NOT NULL,
    State         nvarchar(50)    DEFAULT '' NOT NULL,
    AddDate       datetime        DEFAULT getdate() NOT NULL,
    StartTime     datetime        DEFAULT getdate() NOT NULL,
    EndTime       datetime        DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_CreateTask PRIMARY KEY NONCLUSTERED (ID)
)
GO

--bairong_CreateTask
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_CreateTask') AND NAME = 'StartTime')
ALTER TABLE bairong_CreateTask ADD
    StartTime     datetime        DEFAULT getdate() NOT NULL,
    EndTime       datetime        DEFAULT getdate() NOT NULL
GO

--bairong_AjaxUrl
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'ContentID')
ALTER TABLE bairong_AjaxUrl ADD
	ContentID              int             DEFAULT 0 NOT NULL,
	NodeID              int             DEFAULT 0 NOT NULL,
	TemplateID              int             DEFAULT 0 NOT NULL,
	PublishmentSystemID              int             DEFAULT 0 NOT NULL,
	CreateTaskID              int             DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'ActionType')
ALTER TABLE bairong_AjaxUrl ADD
	ActionType	varchar(50)		DEFAULT ('') NOT NULL 
GO



--稿件系统（临时）
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_Config') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_Config](
	[AttrName] [varchar](200) NOT NULL DEFAULT (''),
	[Attrkey] [varchar](200) NOT NULL DEFAULT (''),
	[value] [varchar](200) NOT NULL DEFAULT (''),
	[discription] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_Content') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_Content](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NodeID] [int] NOT NULL DEFAULT ((0)),
	[PublishmentSystemID] [int] NOT NULL DEFAULT ((0)),
	[AddUserName] [nvarchar](255) NOT NULL DEFAULT (''),
	[LastEditUserName] [nvarchar](255) NOT NULL DEFAULT (''),
	[LastEditDate] [datetime] NOT NULL DEFAULT (getdate()),
	[Taxis] [int] NOT NULL DEFAULT ((0)),
	[ContentGroupNameCollection] [nvarchar](255) NOT NULL DEFAULT (''),
	[Tags] [nvarchar](255) NOT NULL DEFAULT (''),
	[SourceID] [int] NOT NULL DEFAULT ((0)),
	[ReferenceID] [int] NOT NULL DEFAULT ((0)),
	[IsChecked] [varchar](18) NOT NULL DEFAULT (''),
	[CheckedLevel] [int] NOT NULL DEFAULT ((0)),
	[Comments] [int] NOT NULL DEFAULT ((0)),
	[Photos] [int] NOT NULL DEFAULT ((0)),
	[Teleplays] [int] NOT NULL DEFAULT ((0)),
	[Hits] [int] NOT NULL DEFAULT ((0)),
	[HitsByDay] [int] NOT NULL DEFAULT ((0)),
	[HitsByWeek] [int] NOT NULL DEFAULT ((0)),
	[HitsByMonth] [int] NOT NULL DEFAULT ((0)),
	[LastHitsDate] [datetime] NOT NULL DEFAULT (getdate()),
	[SettingsXML] [ntext] NOT NULL DEFAULT (''),
	[Title] [nvarchar](255) NOT NULL DEFAULT (''),
	[IsTop] [varchar](18) NOT NULL DEFAULT (''),
	[AddDate] [datetime] NOT NULL DEFAULT (getdate()),
	[SubTitle] [nvarchar](200) NOT NULL DEFAULT (''),
	[ImageUrl] [nvarchar](200) NOT NULL DEFAULT (''),
	[VideoUrl] [nvarchar](200) NOT NULL DEFAULT (''),
	[FileUrl] [nvarchar](200) NOT NULL DEFAULT (''),
	[LinkUrl] [nvarchar](200) NOT NULL DEFAULT (''),
	[Content] [ntext] NOT NULL DEFAULT (''),
	[Summary] [ntext] NOT NULL DEFAULT (''),
	[Author] [nvarchar](200) NOT NULL DEFAULT (''),
	[Source] [nvarchar](200) NOT NULL DEFAULT (''),
	[IsRecommend] [nvarchar](18) NOT NULL DEFAULT (''),
	[IsHot] [nvarchar](18) NOT NULL DEFAULT (''),
	[IsColor] [nvarchar](18) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_ml_Content] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_RCNode') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_RCNode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RCID] [int] NULL,
	[NodeID] [int] NULL,
 CONSTRAINT [PK_ml_RCNode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_ReferenceLogs') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_ReferenceLogs](
	[RLID] [int] IDENTITY(1,1) NOT NULL,
	[RTID] [int] NOT NULL DEFAULT ((0)),
	[PublishmentSystemID] [int] NOT NULL DEFAULT ((0)),
	[NodeID] [int] NOT NULL DEFAULT ((0)),
	[ToContentID] [int] NOT NULL DEFAULT ((0)),
	[Operator] [varchar](200) NOT NULL DEFAULT (''),
	[OperateDate] [datetime] NULL DEFAULT (getdate()),
	[SubmissionID] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[RLID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_ReferenceType') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_ReferenceType](
	[RTID] [int] IDENTITY(1,1) NOT NULL,
	[RTName] [varchar](200) NULL,
	[AddDate] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[RTID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_RoleCheckLevel') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_RoleCheckLevel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NULL,
	[CheckLevel] [int] NULL,
 CONSTRAINT [PK_ml_RoleCheckLevel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('ml_Submission') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE [dbo].[ml_Submission](
	[SubmissionID] [int] IDENTITY(1,1) NOT NULL,
	[AddUserName] [varchar](200) NOT NULL DEFAULT (''),
	[Title] [nvarchar](255) NULL,
	[AddDate] [datetime] NULL DEFAULT (getdate()),
	[IsChecked] [varchar](18) NULL DEFAULT ('false'),
	[CheckedLevel] [int] NOT NULL DEFAULT ((0)),
	[PassDate] [datetime] NULL,
	[ReferenceTimes] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK__ml_Submi__449EE1050702DA78] PRIMARY KEY CLUSTERED 
(
	[SubmissionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



--ajaxUrl
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'ContentID')
ALTER TABLE bairong_AjaxUrl ADD
	ContentID    int   DEFAULT 0 NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'NodeID')
ALTER TABLE bairong_AjaxUrl ADD
	NodeID    int   DEFAULT 0 NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'TemplateID')
ALTER TABLE bairong_AjaxUrl ADD
	TemplateID    int   DEFAULT 0 NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'PublishmentSystemID')
ALTER TABLE bairong_AjaxUrl ADD
	PublishmentSystemID    int   DEFAULT 0 NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'CreateTaskID')
ALTER TABLE bairong_AjaxUrl ADD
	CreateTaskID    int   DEFAULT 0 NOT NULL
GO
IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_AjaxUrl') AND NAME = 'AjaxUrl')
ALTER TABLE bairong_AjaxUrl ALTER COLUMN
	AjaxUrl    varchar(500)
GO

IF NOT EXISTS(SELECT * FROM bairong_TableCollection WHERE TableENName = 'ml_Content')
INSERT INTO [dbo].[bairong_TableCollection]([TableENName],[TableCNName],[AttributeNum],[AuxiliaryTableType],[IsCreatedInDB],[IsChangedAfterCreatedInDB],[IsDefault],[Description])
     VALUES ('ml_Content','稿件池',15,'ManuscriptContent','True','False','False','投稿系统专用模型')
GO

IF NOT EXISTS(SELECT * FROM bairong_TableMetadata WHERE AuxiliaryTableENName = 'ml_Content')
BEGIN
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','Title','NVarChar',255,'False','''''',1,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','SubTitle','NVarChar',200,'False','''''',2,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','ImageUrl','NVarChar',200,'False','''''',3,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','VideoUrl','NVarChar',200,'False','''''',4,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','FileUrl','NVarChar',200,'False','''''',5,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','LinkUrl','NVarChar',200,'False','''''',6,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','Content','NText',16,'False','''''',7,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','Summary','NText',16,'False','''''',8,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','Author','NVarChar',200,'False','''''',9,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','Source','NVarChar',200,'False','''''',10,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','IsRecommend','NVarChar',18,'False','''''',11,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','IsHot','NVarChar',18,'False','''''',12,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','IsColor','NVarChar',18,'False','''''',13,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','IsTop','NVarChar',18,'False','''''',14,'True')
INSERT INTO [dbo].[bairong_TableMetadata]([AuxiliaryTableENName],[AttributeName],[DataType],[DataLength],[CanBeNull],[DBDefaultValue],[Taxis],[IsSystem])
     VALUES('ml_Content','AddDate','DateTime',8,'False','getdate()',15,'True')    
END     
GO

----投稿管理------

----投稿范围表
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_MLibScope') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_MLibScope(
    PublishmentSystemID       int             DEFAULT 0 NOT NULL,
    NodeID                    int             DEFAULT 0 NOT NULL,
    ContentNum                int             DEFAULT 0 NOT NULL,
    IsChecked                 varchar(18)     DEFAULT '' NOT NULL,
    Field                     varchar(500)    DEFAULT '' NOT NULL,
    AddDate                   datetime        DEFAULT getdate() NOT NULL,
    UserName                  varchar(500)    DEFAULT '' NOT NULL,
    SetXML                    ntext           DEFAULT '' NOT NULL  
)
GO 




----投稿草稿表 
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_MLibDraftContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE  bairong_MLibDraftContent(
	 ID                          int            IDENTITY(1,1),
	 NodeID                      int            DEFAULT 0 NOT NULL,
	 PublishmentSystemID         int            DEFAULT 0 NOT NULL,
	 AddUserName                 nvarchar(255)  DEFAULT '' NOT NULL,
	 LastEditUserName            nvarchar(255)  DEFAULT '' NOT NULL,
	 LastEditDate                datetime       DEFAULT getdate() NOT NULL,
	 Taxis                       int            DEFAULT 0 NOT NULL,
	 ContentGroupNameCollection  nvarchar(255)  DEFAULT '' NOT NULL,
	 Tags                        nvarchar(255)  DEFAULT '' NOT NULL,
	 SourceID                    int            DEFAULT 0 NOT NULL,
	 ReferenceID                 int            DEFAULT 0 NOT NULL,
	 IsChecked                   varchar(18)    DEFAULT '' NOT NULL,
	 CheckedLevel                int            DEFAULT 0 NOT NULL,
	 Comments                    int            DEFAULT 0 NOT NULL,
	 Photos                      int            DEFAULT 0 NOT NULL,
	 Teleplays                   int            DEFAULT 0 NOT NULL,
	 Hits                        int            DEFAULT 0 NOT NULL,
	 HitsByDay                   int            DEFAULT 0 NOT NULL,
	 HitsByWeek                  int            DEFAULT 0 NOT NULL,
	 HitsByMonth                 int            DEFAULT 0 NOT NULL,
	 LastHitsDate                datetime       DEFAULT getdate() NOT NULL,
	 SettingsXML                 ntext          DEFAULT '' NOT NULL,
	 Title                       nvarchar(255)  DEFAULT '' NOT NULL,
	 SubTitle                    nvarchar(255)  DEFAULT '' NOT NULL,
	 ImageUrl                    varchar(200)   DEFAULT '' NOT NULL,
	 VideoUrl                    varchar(200)   DEFAULT '' NOT NULL,
	 FileUrl                     varchar(200)   DEFAULT '' NOT NULL,
	 LinkUrl                     nvarchar(200)  DEFAULT '' NOT NULL,
	 Content                     ntext          DEFAULT '' NOT NULL,
	 Summary                     ntext          DEFAULT '' NOT NULL,
	 Author                      nvarchar(255)  DEFAULT '' NOT NULL,
	 Source                      nvarchar(255)  DEFAULT '' NOT NULL,
	 IsRecommend                 varchar(18)    DEFAULT '' NOT NULL,
	 IsHot                       varchar(18)    DEFAULT '' NOT NULL,
	 IsColor                     varchar(18)    DEFAULT '' NOT NULL,
	 IsTop                       varchar(18)    DEFAULT '' NOT NULL,
	 AddDate                     datetime       DEFAULT getdate() NOT NULL,
	 CheckTaskDate               datetime       DEFAULT getdate() NULL,
	 UnCheckTaskDate             datetime       DEFAULT getdate() NULL,
	 MemberName                  nvarchar(50)   DEFAULT '' NOT NULL,
CONSTRAINT PK_bairong_MLibDraftContent PRIMARY KEY NONCLUSTERED (ID)
)
GO 



----新用户组表
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bairong_UserNewGroup') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bairong_UserNewGroup(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
ClassifyID             int             DEFAULT 0 NOT NULL, 
GroupType              varchar(18)     DEFAULT '' NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)    DEFAULT '' NOT NULL,
Description            nvarchar(500)   DEFAULT '' NOT NULL,
SetXML                 ntext           DEFAULT '' NOT NULL,
CONSTRAINT PK_bairong_UserNewGroup PRIMARY KEY NONCLUSTERED (ItemID)
)
GO
IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_bairong_UserNewGroup_Taxis')
CREATE CLUSTERED INDEX IX_bairong_UserNewGroup_Taxis ON bairong_UserNewGroup(Taxis)
GO


--会员表增加字段，新用户组ID
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'NewGroupID')
ALTER TABLE bairong_Users ADD
    NewGroupID             int              DEFAULT 0 NOT NULL 
GO
--会员表增加字段，投稿数量
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'MLibNum')
ALTER TABLE bairong_Users ADD 
    MLibNum            int              DEFAULT 0 NOT NULL
GO
--会员表增加字段，投稿有效期
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bairong_Users') AND NAME = 'MLibValidityDate')
ALTER TABLE bairong_Users ADD 
MLibValidityDate       datetime        DEFAULT '1754-1-1 0:0:0:0' NOT NULL
GO 


----投稿管理--  END----










