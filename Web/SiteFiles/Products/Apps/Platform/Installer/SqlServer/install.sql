/*
 * ER/Studio 8.0 SQL Code Generation
 * Company :      BaiRong Software
 * Project :      BaiRong Software Fundation Tables
 * Author :       BaiRong Software
 *
 * Date Created : Monday, July 28, 2014 09:53:47
 * Target DBMS : Microsoft SQL Server 2000
 */

CREATE TABLE bairong_Administrator(
    UserName                         nvarchar(255)    NOT NULL,
    Password                         nvarchar(255)    DEFAULT '' NOT NULL,
    PasswordFormat                   varchar(50)      DEFAULT '' NOT NULL,
    PasswordSalt                     nvarchar(128)    DEFAULT '' NOT NULL,
    CreationDate                     datetime         DEFAULT getdate() NOT NULL,
    LastActivityDate                 datetime         DEFAULT getdate() NOT NULL,
    LastProductID                    varchar(50)      DEFAULT '' NOT NULL,
    CountOfLogin                     int              DEFAULT 0 NOT NULL,
    CreatorUserName                  nvarchar(255)    DEFAULT '' NOT NULL,
    IsLockedOut                      varchar(18)      DEFAULT '' NOT NULL,
    PublishmentSystemIDCollection    varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID              int              DEFAULT 0 NOT NULL,
    DepartmentID                     int              DEFAULT 0 NOT NULL,
    AreaID                           int              DEFAULT 0 NOT NULL,
    DisplayName                      nvarchar(255)    DEFAULT '' NOT NULL,
    Question                         nvarchar(255)    DEFAULT '' NOT NULL,
    Answer                           nvarchar(255)    DEFAULT '' NOT NULL,
    Email                            nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile                           varchar(20)      DEFAULT '' NOT NULL,
    Theme                            varchar(50)      DEFAULT '' NOT NULL,
    Language                         varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Administrator PRIMARY KEY NONCLUSTERED (UserName)
)
go



CREATE TABLE bairong_AdministratorsInRoles(
    RoleName    nvarchar(255)    NOT NULL,
    UserName    nvarchar(255)    NOT NULL,
    CONSTRAINT PK_bairong_AInR PRIMARY KEY NONCLUSTERED (RoleName, UserName)
)
go



CREATE TABLE bairong_AjaxUrl(
    SN            varchar(50)     NOT NULL,
    AjaxUrl       varchar(500)    DEFAULT '' NOT NULL,
    Parameters    ntext           DEFAULT '' NOT NULL,
	ContentID              int             DEFAULT 0 NOT NULL,
	NodeID              int             DEFAULT 0 NOT NULL,
	TemplateID              int             DEFAULT 0 NOT NULL,
	PublishmentSystemID              int             DEFAULT 0 NOT NULL,
	CreateTaskID              int             DEFAULT 0 NOT NULL,
	ActionType	varchar(50)		DEFAULT ('') NOT NULL 
    CONSTRAINT PK_bairong_AjaxUrl PRIMARY KEY NONCLUSTERED (SN)
)
go



CREATE TABLE bairong_Area(
    AreaID           int              IDENTITY(1,1),
    AreaName         nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID         int              DEFAULT 0 NOT NULL,
    ParentsPath      nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount     int              DEFAULT 0 NOT NULL,
    ChildrenCount    int              DEFAULT 0 NOT NULL,
    IsLastNode       varchar(18)      DEFAULT '' NOT NULL,
    Taxis            int              DEFAULT 0 NOT NULL,
    CountOfAdmin     int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Area PRIMARY KEY NONCLUSTERED (AreaID)
)
go



CREATE TABLE bairong_Cache(
    CacheKey      varchar(200)    NOT NULL,
    CacheValue    ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Cache PRIMARY KEY NONCLUSTERED (CacheKey)
)
go



CREATE TABLE bairong_CloudStorage(
    StorageID       int            NOT NULL,
    ProviderType    varchar(50)    DEFAULT '' NOT NULL,
    SettingsXML     ntext          DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_CloudStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go



CREATE TABLE bairong_Config(
    IsInitialized           varchar(18)      DEFAULT '' NOT NULL,
    DatabaseVersion         varchar(50)      DEFAULT '' NOT NULL,
    RestrictionBlackList    nvarchar(255)    DEFAULT '' NOT NULL,
    RestrictionWhiteList    nvarchar(255)    DEFAULT '' NOT NULL,
    IsRelatedUrl            varchar(18)      DEFAULT '' NOT NULL,
    RootUrl                 varchar(200)     DEFAULT '' NOT NULL,
    UpdateDate              datetime         DEFAULT getdate() NOT NULL,
    SettingsXML             ntext            DEFAULT '' NOT NULL
)
go



CREATE TABLE bairong_ContentCheck(
    CheckID                int              IDENTITY(1,1),
    TableName              varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IsAdmin                varchar(18)      DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    CheckedLevel           int              DEFAULT 0 NOT NULL,
    CheckDate              datetime         DEFAULT getdate() NOT NULL,
    Reasons                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ContentCheck PRIMARY KEY NONCLUSTERED (CheckID)
)
go



CREATE TABLE bairong_ContentModel(
    ModelID        varchar(50)      NOT NULL,
    ProductID      varchar(50)      DEFAULT '' NOT NULL,
    SiteID         int              DEFAULT 0 NOT NULL,
    ModelName      nvarchar(50)     DEFAULT '' NOT NULL,
    IsSystem       varchar(18)      DEFAULT '' NOT NULL,
    TableName      varchar(200)     DEFAULT '' NOT NULL,
    TableType      varchar(50)      DEFAULT '' NOT NULL,
    IconUrl        varchar(50)      DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ContentModel PRIMARY KEY NONCLUSTERED (ModelID, SiteID)
)
go



CREATE TABLE bairong_Count(
    CountID             int              IDENTITY(1,1),
    ApplicationName     varchar(50)      DEFAULT '' NOT NULL,
    RelatedTableName    nvarchar(255)    DEFAULT '' NOT NULL,
    RelatedIdentity     nvarchar(255)    DEFAULT '' NOT NULL,
    CountType           varchar(50)      DEFAULT '' NOT NULL,
    CountNum            int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Count PRIMARY KEY CLUSTERED (CountID)
)
go



CREATE TABLE bairong_Department(
    DepartmentID      int              IDENTITY(1,1),
    DepartmentName    nvarchar(255)    DEFAULT '' NOT NULL,
    Code              varchar(50)      DEFAULT '' NOT NULL,
    ParentID          int              DEFAULT 0 NOT NULL,
    ParentsPath       nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount      int              DEFAULT 0 NOT NULL,
    ChildrenCount     int              DEFAULT 0 NOT NULL,
    IsLastNode        varchar(18)      DEFAULT '' NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    AddDate           datetime         DEFAULT getdate() NOT NULL,
    Summary           nvarchar(255)    DEFAULT '' NOT NULL,
    CountOfAdmin      int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Department PRIMARY KEY NONCLUSTERED (DepartmentID)
)
go



CREATE TABLE bairong_Digg(
    DiggID                 int    IDENTITY(1,1),
    PublishmentSystemID    int    DEFAULT 0 NOT NULL,
    RelatedIdentity        int    DEFAULT 0 NOT NULL,
    Good                   int    DEFAULT 0 NOT NULL,
    Bad                    int    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Digg PRIMARY KEY NONCLUSTERED (DiggID)
)
go



CREATE TABLE bairong_ErrorLog(
    ID            int              IDENTITY(1,1),
    AddDate       datetime         DEFAULT getdate() NOT NULL,
    Message       nvarchar(255)    DEFAULT '' NOT NULL,
    Stacktrace    ntext            DEFAULT '' NOT NULL,
    Summary       ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ErrorLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE bairong_FTPStorage(
    StorageID        int             NOT NULL,
    Server           varchar(200)    DEFAULT '' NOT NULL,
    Port             int             DEFAULT 0 NOT NULL,
    UserName         varchar(200)    DEFAULT '' NOT NULL,
    Password         varchar(200)    DEFAULT '' NOT NULL,
    IsPassiveMode    varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_FTPStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go



CREATE TABLE bairong_IP2City(
    ID          int             IDENTITY(1,1),
    StartNum    float           DEFAULT 0 NOT NULL,
    EndNum      float           DEFAULT 0 NOT NULL,
    Province    nvarchar(50)    DEFAULT '' NOT NULL,
    City        nvarchar(50)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_IP2City PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bairong_LocalStorage(
    StorageID        int              NOT NULL,
    DirectoryPath    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_LocalStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go


CREATE TABLE bairong_Log(
    ID           int              IDENTITY(1,1),
    UserName     varchar(50)      DEFAULT '' NOT NULL,
    IPAddress    varchar(50)      DEFAULT '' NOT NULL,
    AddDate      datetime         DEFAULT getdate() NOT NULL,
    Action       nvarchar(255)    DEFAULT '' NOT NULL,
    Summary      nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Log PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE bairong_PermissionsInRoles(
    RoleName              nvarchar(255)    NOT NULL,
    GeneralPermissions    text             DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_GPInR PRIMARY KEY CLUSTERED (RoleName)
)
go



CREATE TABLE bairong_Roles(
    RoleName               nvarchar(255)    NOT NULL,
    ProductIDCollection    varchar(200)     DEFAULT '' NOT NULL,
    CreatorUserName        nvarchar(255)    DEFAULT '' NOT NULL,
    Description            nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Roles PRIMARY KEY NONCLUSTERED (RoleName)
)
go



CREATE TABLE bairong_SMSMessages(
    ID             int               IDENTITY(1,1),
    MobilesList    ntext             DEFAULT '' NOT NULL,
    SMSContent     nvarchar(1000)    DEFAULT '' NOT NULL,
    SendDate       datetime          DEFAULT getdate() NOT NULL,
    SMSUserName    nvarchar(255)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_SMSMessages PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bairong_Storage(
    StorageID      int              IDENTITY(1,1),
    StorageName    nvarchar(50)     DEFAULT '' NOT NULL,
    StorageUrl     varchar(200)     DEFAULT '' NOT NULL,
    StorageType    varchar(50)      DEFAULT '' NOT NULL,
    IsEnabled      varchar(18)      DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_Storage PRIMARY KEY NONCLUSTERED (StorageID)
)
go



CREATE TABLE bairong_TableCollection(
    TableENName                  varchar(50)     NOT NULL,
    TableCNName                  nvarchar(50)    DEFAULT '' NOT NULL,
    AttributeNum                 int             DEFAULT 0 NOT NULL,
    AuxiliaryTableType           varchar(50)     DEFAULT '' NOT NULL,
    IsCreatedInDB                varchar(18)     DEFAULT '' NOT NULL,
    IsChangedAfterCreatedInDB    varchar(18)     DEFAULT '' NOT NULL,
    IsDefault                    varchar(18)     DEFAULT '' NOT NULL,
    Description                  ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_AT PRIMARY KEY CLUSTERED (TableENName)
)
go



CREATE TABLE bairong_TableMatch(
    TableMatchID               int             IDENTITY(1,1),
    ConnectionString           varchar(200)    DEFAULT '' NOT NULL,
    TableName                  varchar(200)    DEFAULT '' NOT NULL,
    ConnectionStringToMatch    varchar(200)    DEFAULT '' NOT NULL,
    TableNameToMatch           varchar(200)    DEFAULT '' NOT NULL,
    ColumnsMap                 ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_TableMatch PRIMARY KEY CLUSTERED (TableMatchID)
)
go



CREATE TABLE bairong_TableMetadata(
    TableMetadataID         int              IDENTITY(1,1),
    AuxiliaryTableENName    varchar(50)      NOT NULL,
    AttributeName           varchar(50)      DEFAULT '' NOT NULL,
    DataType                varchar(50)      DEFAULT '' NOT NULL,
    DataLength              int              DEFAULT 0 NOT NULL,
    CanBeNull               varchar(18)      DEFAULT '' NOT NULL,
    DBDefaultValue          nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis                   int              DEFAULT 0 NOT NULL,
    IsSystem                varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ATM PRIMARY KEY CLUSTERED (TableMetadataID)
)
go



CREATE TABLE bairong_TableStyle(
    TableStyleID       int              IDENTITY(1,1),
    RelatedIdentity    int              DEFAULT 0 NOT NULL,
    TableName          varchar(50)      DEFAULT '' NOT NULL,
    AttributeName      varchar(50)      DEFAULT '' NOT NULL,
    Taxis              int              DEFAULT 0 NOT NULL,
    DisplayName        nvarchar(255)    DEFAULT '' NOT NULL,
    HelpText           varchar(255)     DEFAULT '' NOT NULL,
    IsVisible          varchar(18)      DEFAULT '' NOT NULL,
    IsVisibleInList    varchar(18)      DEFAULT '' NOT NULL,
    IsSingleLine       varchar(18)      DEFAULT '' NOT NULL,
    InputType          varchar(50)      DEFAULT '' NOT NULL,
    IsRequired         varchar(18)      DEFAULT '' NOT NULL,
    DefaultValue       varchar(255)     DEFAULT '' NOT NULL,
    IsHorizontal       varchar(18)      DEFAULT '' NOT NULL,
    ExtendValues       ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ATS PRIMARY KEY NONCLUSTERED (TableStyleID)
)
go



CREATE TABLE bairong_TableStyleItem(
    TableStyleItemID    int             IDENTITY(1,1),
    TableStyleID        int             NOT NULL,
    ItemTitle           varchar(255)    DEFAULT '' NOT NULL,
    ItemValue           varchar(255)    DEFAULT '' NOT NULL,
    IsSelected          varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_STSI PRIMARY KEY NONCLUSTERED (TableStyleItemID)
)
go



CREATE TABLE bairong_Tags(
    TagID                  int              IDENTITY(1,1),
    ProductID              varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentIDCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    Tag                    nvarchar(255)    DEFAULT '' NOT NULL,
    UseNum                 int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Tags PRIMARY KEY NONCLUSTERED (TagID)
)
go



CREATE TABLE bairong_Task(
    TaskID                  int              IDENTITY(1,1),
    TaskName                nvarchar(50)     DEFAULT '' NOT NULL,
    ProductID               varchar(50)      DEFAULT '' NOT NULL,
    IsSystemTask            varchar(18)      DEFAULT '' NOT NULL,
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    ServiceType             varchar(50)      DEFAULT '' NOT NULL,
    ServiceParameters       ntext            DEFAULT '' NOT NULL,
    FrequencyType           varchar(50)      DEFAULT '' NOT NULL,
    PeriodIntervalMinute    int              DEFAULT 0 NOT NULL,
    StartDay                int              DEFAULT 0 NOT NULL,
    StartWeekday            int              DEFAULT 0 NOT NULL,
    StartHour               int              DEFAULT 0 NOT NULL,
    IsEnabled               varchar(18)      DEFAULT '' NOT NULL,
    AddDate                 datetime         DEFAULT getdate() NOT NULL,
    LastExecuteDate         datetime         DEFAULT getdate() NOT NULL,
    Description             nvarchar(255)    DEFAULT '' NOT NULL,
	OnlyOnceDate            datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_Task PRIMARY KEY NONCLUSTERED (TaskID)
)
go



CREATE TABLE bairong_TaskLog(
    ID               int              IDENTITY(1,1),
    TaskID           int              NOT NULL,
    IsSuccess        varchar(18)      DEFAULT '' NOT NULL,
    ErrorMessage     nvarchar(255)    DEFAULT '' NOT NULL,
    StackTrace       ntext            DEFAULT '' NOT NULL,
    SubtaskErrors    ntext            DEFAULT '' NOT NULL,
    AddDate          datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_TaskLog PRIMARY KEY CLUSTERED (ID)
)
go



--CREATE TABLE bairong_UserBinding(
    --UserName       nvarchar(255)    NOT NULL,
    --BindingType    varchar(50)      DEFAULT '' NOT NULL,
    --BindingID      int              DEFAULT 0 NOT NULL,
    --BindingName    nvarchar(255)    DEFAULT '' NOT NULL,
    --CONSTRAINT PK_bairong_UserBinding PRIMARY KEY NONCLUSTERED (UserName)
--)
--go

CREATE TABLE bairong_UserBinding(
    UserID       nvarchar(255)    NOT NULL,
    ThirdLoginType    varchar(50)      DEFAULT '' NOT NULL,
    ThirdLoginUserID    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserBinding PRIMARY KEY NONCLUSTERED (UserID)
)
go



CREATE TABLE bairong_UserConfig(
    SettingsXML    ntext    DEFAULT '' NOT NULL
)
go



CREATE TABLE bairong_UserContact(
    ID                 int              IDENTITY(1,1),
    RelatedUserName    nvarchar(255)    DEFAULT '' NOT NULL,
    CreateUserName     nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis              int              DEFAULT 0 NOT NULL,
    FullName           nvarchar(50)     DEFAULT '' NOT NULL,
    AvatarUrl          varchar(200)     DEFAULT '' NOT NULL,
    Summary            nvarchar(255)    DEFAULT '' NOT NULL,
    Tel                varchar(50)      DEFAULT '' NOT NULL,
    Mobile             varchar(200)     DEFAULT '' NOT NULL,
    Email              varchar(200)     DEFAULT '' NOT NULL,
    QQ                 varchar(50)      DEFAULT '' NOT NULL,
    Birthday           varchar(50)      DEFAULT '' NOT NULL,
    Organization       nvarchar(50)     DEFAULT '' NOT NULL,
    Department         nvarchar(50)     DEFAULT '' NOT NULL,
    Position           nvarchar(50)     DEFAULT '' NOT NULL,
    Address            nvarchar(255)    DEFAULT '' NOT NULL,
    Website            varchar(200)     DEFAULT '' NOT NULL,
	Gender             varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserContact PRIMARY KEY NONCLUSTERED (ID)
)
go



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
go



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
go



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
go



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
go



CREATE TABLE bairong_Users(
    UserID              int              IDENTITY(1,1),
    GroupSN             nvarchar(255)    DEFAULT '' NOT NULL,
    UserName            nvarchar(255)    DEFAULT '' NOT NULL,
    Password            nvarchar(255)    DEFAULT '' NOT NULL,
    PasswordFormat      varchar(50)      DEFAULT '' NOT NULL,
    PasswordSalt        nvarchar(128)    DEFAULT '' NOT NULL,
    GroupID             int              DEFAULT 0 NOT NULL,
    Credits             int              DEFAULT 0 NOT NULL,
    CreateDate          datetime         DEFAULT getdate() NOT NULL,
    CreateIPAddress     varchar(50)      DEFAULT '' NOT NULL,
    CreateUserName      nvarchar(255)    DEFAULT '' NOT NULL,
    PointCount          int              DEFAULT 0 NOT NULL,
    LastActivityDate    datetime         DEFAULT getdate() NOT NULL,
    IsChecked           varchar(18)      DEFAULT '' NOT NULL,
    IsLockedOut         varchar(18)      DEFAULT '' NOT NULL,
    IsTemporary         varchar(18)      DEFAULT '' NOT NULL,
    DisplayName         nvarchar(255)    DEFAULT '' NOT NULL,
    Email               nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile              varchar(20)      DEFAULT '' NOT NULL,
	TypeID              int              DEFAULT 0 NOT NULL,
    DepartmentID        int              DEFAULT 0 NOT NULL,
    AreaID              int              DEFAULT 0 NOT NULL,
    OnlineSeconds       int              DEFAULT 0 NOT NULL,
    AvatarLarge         varchar(200)     DEFAULT '' NOT NULL,
    AvatarMiddle        varchar(200)     DEFAULT '' NOT NULL,
    AvatarSmall         varchar(200)     DEFAULT '' NOT NULL,
    Signature           nvarchar(255)    DEFAULT '' NOT NULL,
    SettingsXML         ntext            DEFAULT '' NOT NULL,
	LoginNum            int              DEFAULT 0 NOT NULL,
    Birthday            varchar(50)      DEFAULT getdate() NOT NULL,
    BloodType           varchar(50)      DEFAULT '' NOT NULL,
    Gender              nvarchar(50)     DEFAULT '' NOT NULL,
    Education           nvarchar(255)    DEFAULT '' NOT NULL,
    MaritalStatus       nvarchar(50)     DEFAULT '' NOT NULL,
    Graduation          nvarchar(255)    DEFAULT '' NOT NULL,
    Profession          nvarchar(255)    DEFAULT '' NOT NULL,
    QQ                  varchar(50)      DEFAULT '' NOT NULL,
    WeiBo               nvarchar(255)    DEFAULT '' NOT NULL,
    WeiXin              nvarchar(255)    DEFAULT '' NOT NULL,
    Interests           ntext            DEFAULT '' NOT NULL,
    Organization        nvarchar(50)     DEFAULT '' NOT NULL,
    Department          nvarchar(50)     DEFAULT '' NOT NULL,
    Position            nvarchar(50)     DEFAULT '' NOT NULL,
    Address             nvarchar(255)    DEFAULT '' NOT NULL,
    NewGroupID          int              DEFAULT 0 NOT NULL,
    MLibNum             int              DEFAULT 0 NOT NULL,
    MLibValidityDate    datetime         DEFAULT '1754-1-1 0:0:0:0' NOT NULL,
    CONSTRAINT PK_bairong_Users PRIMARY KEY NONCLUSTERED (UserID)
)
go


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
go


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
go


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
go


CREATE TABLE bairong_UserSecurityQuestion(
    ID          int              IDENTITY(1,1),
    Question    nvarchar(255)    DEFAULT '' NOT NULL,
    IsEnable    varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK355 PRIMARY KEY NONCLUSTERED (ID)
)
go


CREATE TABLE bairong_UserLog(
    ID           int              IDENTITY(1,1),
    UserName     varchar(50)      DEFAULT '' NOT NULL,
    IPAddress    varchar(50)      DEFAULT '' NOT NULL,
    AddDate      datetime         DEFAULT getdate() NOT NULL,
    Action       nvarchar(255)    DEFAULT '' NOT NULL,
    Summary      nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserLog PRIMARY KEY CLUSTERED (ID)
)
go


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





CREATE INDEX IX_bairong_TM_ATE ON bairong_TableMetadata(AuxiliaryTableENName)
go
CREATE CLUSTERED INDEX IX_bairong_TSI_TSI ON bairong_TableStyleItem(TableStyleID)
go
ALTER TABLE bairong_AdministratorsInRoles ADD CONSTRAINT FK_bairong_AInR_A 
    FOREIGN KEY (UserName)
    REFERENCES bairong_Administrator(UserName) ON DELETE CASCADE ON UPDATE CASCADE
go

ALTER TABLE bairong_AdministratorsInRoles ADD CONSTRAINT FK_bairong_AInR_R 
    FOREIGN KEY (RoleName)
    REFERENCES bairong_Roles(RoleName) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_CloudStorage ADD CONSTRAINT FK_bairong_Storage_Cloud 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_FTPStorage ADD CONSTRAINT FK_bairong_Storage_FTP 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_LocalStorage ADD CONSTRAINT FK_bairong_Storage_Local 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_TableMetadata ADD CONSTRAINT FK_bairong_ATM_AT 
    FOREIGN KEY (AuxiliaryTableENName)
    REFERENCES bairong_TableCollection(TableENName) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_TableStyleItem ADD CONSTRAINT FK_bairong_ATSI_ATS 
    FOREIGN KEY (TableStyleID)
    REFERENCES bairong_TableStyle(TableStyleID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_TaskLog ADD CONSTRAINT FK_bairong_Task_Log 
    FOREIGN KEY (TaskID)
    REFERENCES bairong_Task(TaskID) ON DELETE CASCADE ON UPDATE CASCADE
go



--稿件系统（临时）
CREATE TABLE [dbo].[ml_Config](
	[AttrName] [varchar](200) NOT NULL,
	[Attrkey] [varchar](200) NOT NULL,
	[value] [varchar](200) NOT NULL,
	[discription] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ml_Config] ADD  DEFAULT ('') FOR [AttrName]
GO

ALTER TABLE [dbo].[ml_Config] ADD  DEFAULT ('') FOR [Attrkey]
GO

ALTER TABLE [dbo].[ml_Config] ADD  DEFAULT ('') FOR [value]
GO


CREATE TABLE [dbo].[ml_Content](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NodeID] [int] NOT NULL,
	[PublishmentSystemID] [int] NOT NULL,
	[AddUserName] [nvarchar](255) NOT NULL,
	[LastEditUserName] [nvarchar](255) NOT NULL,
	[LastEditDate] [datetime] NOT NULL,
	[Taxis] [int] NOT NULL,
	[ContentGroupNameCollection] [nvarchar](255) NOT NULL,
	[Tags] [nvarchar](255) NOT NULL,
	[SourceID] [int] NOT NULL,
	[ReferenceID] [int] NOT NULL,
	[IsChecked] [varchar](18) NOT NULL,
	[CheckedLevel] [int] NOT NULL,
	[Comments] [int] NOT NULL,
	[Photos] [int] NOT NULL,
	[Teleplays] [int] NOT NULL,
	[Hits] [int] NOT NULL,
	[HitsByDay] [int] NOT NULL,
	[HitsByWeek] [int] NOT NULL,
	[HitsByMonth] [int] NOT NULL,
	[LastHitsDate] [datetime] NOT NULL,
	[SettingsXML] [ntext] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[IsTop] [varchar](18) NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[SubTitle] [nvarchar](200) NOT NULL,
	[ImageUrl] [nvarchar](200) NOT NULL,
	[VideoUrl] [nvarchar](200) NOT NULL,
	[FileUrl] [nvarchar](200) NOT NULL,
	[LinkUrl] [nvarchar](200) NOT NULL,
	[Content] [ntext] NOT NULL,
	[Summary] [ntext] NOT NULL,
	[Author] [nvarchar](200) NOT NULL,
	[Source] [nvarchar](200) NOT NULL,
	[IsRecommend] [nvarchar](18) NOT NULL,
	[IsHot] [nvarchar](18) NOT NULL,
	[IsColor] [nvarchar](18) NOT NULL,
 CONSTRAINT [PK_ml_Content] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [NodeID]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [PublishmentSystemID]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [AddUserName]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [LastEditUserName]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT (getdate()) FOR [LastEditDate]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [Taxis]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [ContentGroupNameCollection]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Tags]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [SourceID]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [ReferenceID]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [IsChecked]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [CheckedLevel]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [Comments]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [Photos]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [Teleplays]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [Hits]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [HitsByDay]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [HitsByWeek]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ((0)) FOR [HitsByMonth]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT (getdate()) FOR [LastHitsDate]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [SettingsXML]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Title]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [IsTop]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT (getdate()) FOR [AddDate]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [SubTitle]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [ImageUrl]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [VideoUrl]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [FileUrl]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [LinkUrl]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Content]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Summary]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Author]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [Source]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [IsRecommend]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [IsHot]
GO

ALTER TABLE [dbo].[ml_Content] ADD  DEFAULT ('') FOR [IsColor]
GO




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




CREATE TABLE [dbo].[ml_ReferenceLogs](
	[RLID] [int] IDENTITY(1,1) NOT NULL,
	[RTID] [int] NOT NULL,
	[PublishmentSystemID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[ToContentID] [int] NOT NULL,
	[Operator] [varchar](200) NOT NULL,
	[OperateDate] [datetime] NULL,
	[SubmissionID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RLID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ((0)) FOR [RTID]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ((0)) FOR [PublishmentSystemID]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ((0)) FOR [NodeID]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ((0)) FOR [ToContentID]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ('') FOR [Operator]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT (getdate()) FOR [OperateDate]
GO

ALTER TABLE [dbo].[ml_ReferenceLogs] ADD  DEFAULT ((0)) FOR [SubmissionID]
GO



CREATE TABLE [dbo].[ml_ReferenceType](
	[RTID] [int] IDENTITY(1,1) NOT NULL,
	[RTName] [varchar](200) NULL,
	[AddDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RTID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ml_ReferenceType] ADD  CONSTRAINT [DF_ml_ReferenceType_AddDate]  DEFAULT (getdate()) FOR [AddDate]
GO



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

SET ANSI_PADDING OFF
GO



CREATE TABLE [dbo].[ml_Submission](
	[SubmissionID] [int] IDENTITY(1,1) NOT NULL,
	[AddUserName] [varchar](200) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[AddDate] [datetime] NULL,
	[IsChecked] [varchar](18) NULL,
	[CheckedLevel] [int] NOT NULL,
	[PassDate] [datetime] NULL,
	[ReferenceTimes] [int] NOT NULL,
 CONSTRAINT [PK__ml_Submi__449EE1050702DA78] PRIMARY KEY CLUSTERED 
(
	[SubmissionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ml_Submission] ADD  CONSTRAINT [DF__ml_Submis__AddUs__09DF4723]  DEFAULT ('') FOR [AddUserName]
GO

ALTER TABLE [dbo].[ml_Submission] ADD  CONSTRAINT [DF__ml_Submis__AddDa__0AD36B5C]  DEFAULT (getdate()) FOR [AddDate]
GO

ALTER TABLE [dbo].[ml_Submission] ADD  CONSTRAINT [DF_ml_Submission_IsChecked]  DEFAULT ('false') FOR [IsChecked]
GO

ALTER TABLE [dbo].[ml_Submission] ADD  CONSTRAINT [DF__ml_Submis__Statu__0BC78F95]  DEFAULT ((0)) FOR [CheckedLevel]
GO

ALTER TABLE [dbo].[ml_Submission] ADD  CONSTRAINT [DF__ml_Submis__Refer__0CBBB3CE]  DEFAULT ((0)) FOR [ReferenceTimes]
GO


INSERT INTO [dbo].[bairong_TableCollection]([TableENName],[TableCNName],[AttributeNum],[AuxiliaryTableType],[IsCreatedInDB],[IsChangedAfterCreatedInDB],[IsDefault],[Description])
     VALUES ('ml_Content','稿件池',15,'ManuscriptContent','True','False','False','投稿系统专用模型')
GO


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
     
GO



----新用户组表
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

----投稿范围表
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

