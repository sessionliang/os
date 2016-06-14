--
-- ER/Studio 8.0 SQL Code Generation
-- Company :      BaiRong Software
-- Project :      BaiRong Software Fundation Tables
-- Author :       BaiRong Software
--
-- Date Created : Saturday, August 03, 2013 13:30:46
-- Target DBMS : Oracle 9i
--

CREATE SEQUENCE BAIRONG_AREA_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_CONTENTCHECK_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_COUNT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_DEPARTMENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_DIGG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_IP2CITY_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_LOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_SMSMESSAGES_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_SSOAPP_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_STORAGE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TABLEMATCH_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TABLEMETADATA_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TABLESTYLE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TABLESTYLEITEM_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TAGS_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TASK_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_TASKLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERCONTACT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERCREDITSLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERDOWNLOAD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERMESSAGE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERTYPE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE TABLE bairong_Administrator(
    UserName               NVARCHAR2(255)    NOT NULL,
    Password               NVARCHAR2(255)    DEFAULT '',
    PasswordFormat         VARCHAR2(50)      DEFAULT '',
    PasswordSalt           NVARCHAR2(128)    DEFAULT '',
    CreationDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    LastActivityDate       TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    LastModuleID           VARCHAR2(50)      DEFAULT '',
    CountOfLogin           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CreatorUserName        NVARCHAR2(255)    DEFAULT '',
    IsChecked              VARCHAR2(18)      DEFAULT '',
    IsLockedOut            VARCHAR2(18)      DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    DepartmentID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AreaID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    DisplayName            NVARCHAR2(255)    DEFAULT '',
    Question               NVARCHAR2(255)    DEFAULT '',
    Answer                 NVARCHAR2(255)    DEFAULT '',
    Email                  NVARCHAR2(255)    DEFAULT '',
    Mobile                 VARCHAR2(20)      DEFAULT '',
    Theme                  VARCHAR2(50)      DEFAULT '',
    Language               VARCHAR2(50)      DEFAULT '',
    CONSTRAINT PK_bairong_Administrator PRIMARY KEY (UserName)
)
GO



CREATE TABLE bairong_AdministratorsInRoles(
    RoleName    NVARCHAR2(255)    NOT NULL,
    UserName    NVARCHAR2(255)    NOT NULL,
    CONSTRAINT PK_bairong_AInR PRIMARY KEY (RoleName, UserName)
)
GO



CREATE TABLE bairong_Area(
    AreaID           NUMBER(38, 0)     NOT NULL,
    AreaName         NVARCHAR2(255)    DEFAULT '',
    ParentID         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ParentsPath      NVARCHAR2(255)    DEFAULT '',
    ParentsCount     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChildrenCount    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsLastNode       VARCHAR2(18)      DEFAULT '',
    Taxis            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfAdmin     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfUser      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Area PRIMARY KEY (AreaID)
)
GO



CREATE TABLE bairong_Cache(
    CacheKey      VARCHAR2(200)    NOT NULL,
    CacheValue    NCLOB            DEFAULT '',
    CONSTRAINT PK_bairong_Cache PRIMARY KEY (CacheKey)
)
GO



CREATE TABLE bairong_CloudStorage(
    StorageID       NUMBER(38, 0)    NOT NULL,
    ProviderType    VARCHAR2(50)     DEFAULT '',
    SettingsXML     NCLOB            DEFAULT '',
    CONSTRAINT PK_bairong_CloudStorage PRIMARY KEY (StorageID)
)
GO



CREATE TABLE bairong_Config(
    IsInitialized           VARCHAR2(18)      DEFAULT '',
    DatabaseVersion         VARCHAR2(50)      DEFAULT '',
    RestrictionBlackList    NVARCHAR2(255)    DEFAULT '',
    RestrictionWhiteList    NVARCHAR2(255)    DEFAULT '',
    IsRelatedUrl            VARCHAR2(18)      DEFAULT '',
    RootUrl                 VARCHAR2(200)     DEFAULT '',
    UpdateDate              TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    SettingsXML             NCLOB             DEFAULT ''
)
GO



CREATE TABLE bairong_ContentCheck(
    CheckID                NUMBER(38, 0)     NOT NULL,
    TableName              VARCHAR2(50)      DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsAdmin                VARCHAR2(18)      DEFAULT '',
    UserName               NVARCHAR2(255)    DEFAULT '',
    IsChecked              VARCHAR2(18)      DEFAULT '',
    CheckedLevel           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CheckDate              TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Reasons                NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_ContentCheck PRIMARY KEY (CheckID)
)
GO



CREATE TABLE bairong_ContentModel(
    ModelID        VARCHAR2(50)      NOT NULL,
    ProductID      VARCHAR2(50)      DEFAULT '',
    SiteID         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ModelName      NVARCHAR2(50)     DEFAULT '',
    IsSystem       VARCHAR2(18)      DEFAULT '',
    TableName      VARCHAR2(200)     DEFAULT '',
    TableType      VARCHAR2(50)      DEFAULT '',
    IconUrl        VARCHAR2(50)      DEFAULT '',
    Description    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_ContentModel PRIMARY KEY (ModelID)
)
GO



CREATE TABLE bairong_Count(
    CountID             NUMBER(38, 0)     NOT NULL,
    ApplicationName     VARCHAR2(50)      DEFAULT '',
    RelatedTableName    NVARCHAR2(255)    DEFAULT '',
    RelatedIdentity     NVARCHAR2(255)    DEFAULT '',
    CountType           VARCHAR2(50)      DEFAULT '',
    CountNum            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Count PRIMARY KEY (CountID)
)
GO



CREATE TABLE bairong_Department(
    DepartmentID      NUMBER(38, 0)     NOT NULL,
    DepartmentName    NVARCHAR2(255)    DEFAULT '',
    Code              VARCHAR2(50)      DEFAULT '',
    ParentID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ParentsPath       NVARCHAR2(255)    DEFAULT '',
    ParentsCount      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChildrenCount     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsLastNode        VARCHAR2(18)      DEFAULT '',
    Taxis             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AddDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Summary           NVARCHAR2(255)    DEFAULT '',
    CountOfAdmin      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfUser       NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Department PRIMARY KEY (DepartmentID)
)
GO



CREATE TABLE bairong_Digg(
    DiggID                 NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    RelatedIdentity        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Good                   NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Bad                    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Digg PRIMARY KEY (DiggID)
)
GO



CREATE TABLE bairong_FTPStorage(
    StorageID        NUMBER(38, 0)    NOT NULL,
    Server           VARCHAR2(200)    DEFAULT '',
    Port             NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    UserName         VARCHAR2(200)    DEFAULT '',
    Password         VARCHAR2(200)    DEFAULT '',
    IsPassiveMode    VARCHAR2(18)     DEFAULT '',
    CONSTRAINT PK_bairong_FTPStorage PRIMARY KEY (StorageID)
)
GO



CREATE TABLE bairong_IP2City(
    ID          NUMBER(38, 0)       NOT NULL,
    StartNum    DOUBLE PRECISION    DEFAULT 0 NOT NULL,
    EndNum      DOUBLE PRECISION    DEFAULT 0 NOT NULL,
    Province    NVARCHAR2(50)       DEFAULT '',
    City        NVARCHAR2(50)       DEFAULT '',
    CONSTRAINT PK_bairong_IP2City PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_LocalStorage(
    StorageID        NUMBER(38, 0)     NOT NULL,
    DirectoryPath    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_LocalStorage PRIMARY KEY (StorageID)
)
GO



CREATE TABLE bairong_Log(
    ID           NUMBER(38, 0)     NOT NULL,
    UserName     VARCHAR2(50)      DEFAULT '',
    IPAddress    VARCHAR2(50)      DEFAULT '',
    AddDate      TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Action       NVARCHAR2(255)    DEFAULT '',
    Summary      NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_Log PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_Module(
    ModuleID         NVARCHAR2(50)    DEFAULT '' NOT NULL,
    DirectoryName    VARCHAR2(50)     DEFAULT '',
    IsRoot           VARCHAR2(18)     DEFAULT '',
    CONSTRAINT PK_bairong_Module PRIMARY KEY (ModuleID)
)
GO



CREATE TABLE bairong_PermissionsInRoles(
    RoleName              NVARCHAR2(255)    NOT NULL,
    GeneralPermissions    CLOB              DEFAULT '',
    CONSTRAINT PK_bairong_GPInR PRIMARY KEY (RoleName)
)
GO



CREATE TABLE bairong_Roles(
    RoleName           NVARCHAR2(255)    NOT NULL,
    Modules            VARCHAR2(200)     DEFAULT '',
    CreatorUserName    NVARCHAR2(255)    DEFAULT '',
    Description        NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_Roles PRIMARY KEY (RoleName)
)
GO



CREATE TABLE bairong_SMSMessages(
    ID             NUMBER(38, 0)      NOT NULL,
    MobilesList    NCLOB              DEFAULT '',
    SMSContent     NVARCHAR2(1000)    DEFAULT '',
    SendDate       TIMESTAMP(6)       DEFAULT sysdate NOT NULL,
    SMSUserName    NVARCHAR2(255)     DEFAULT '',
    CONSTRAINT PK_bairong_SMSMessages PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_SSOApp(
    AppID             NUMBER(38, 0)     NOT NULL,
    AppType           VARCHAR2(50)      DEFAULT '',
    AppName           NVARCHAR2(50)     DEFAULT '',
    Url               VARCHAR2(200)     DEFAULT '',
    AuthKey           VARCHAR2(200)     DEFAULT '',
    IPAddress         VARCHAR2(50)      DEFAULT '',
    AccessFileName    VARCHAR2(50)      DEFAULT '',
    IsSyncLogin       VARCHAR2(18)      DEFAULT '',
    AddDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Summary           NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_SSOApp PRIMARY KEY (AppID)
)
GO



CREATE TABLE bairong_Storage(
    StorageID      NUMBER(38, 0)     NOT NULL,
    StorageName    NVARCHAR2(50)     DEFAULT '',
    StorageUrl     VARCHAR2(200)     DEFAULT '',
    StorageType    VARCHAR2(50)      DEFAULT '',
    IsEnabled      VARCHAR2(18)      DEFAULT '',
    Description    NVARCHAR2(255)    DEFAULT '',
    AddDate        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_Storage PRIMARY KEY (StorageID)
)
GO



CREATE TABLE bairong_TableCollection(
    TableENName                  VARCHAR2(50)     NOT NULL,
    TableCNName                  VARCHAR2(50)     DEFAULT '',
    AttributeNum                 NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    AuxiliaryTableType           VARCHAR2(50)     DEFAULT '',
    IsCreatedInDB                VARCHAR2(18)     DEFAULT '',
    IsChangedAfterCreatedInDB    VARCHAR2(18)     DEFAULT '',
    ProductID                    VARCHAR2(50)     DEFAULT '',
    IsDefault                    VARCHAR2(18)     DEFAULT '',
    Description                  NCLOB            DEFAULT '',
    CONSTRAINT PK_bairong_AT PRIMARY KEY (TableENName)
)
GO



CREATE TABLE bairong_TableMatch(
    TableMatchID               NUMBER(38, 0)    NOT NULL,
    ConnectionString           VARCHAR2(200)    DEFAULT '',
    TableName                  VARCHAR2(200)    DEFAULT '',
    ConnectionStringToMatch    VARCHAR2(200)    DEFAULT '',
    TableNameToMatch           VARCHAR2(200)    DEFAULT '',
    ColumnsMap                 NCLOB            DEFAULT '',
    CONSTRAINT PK_bairong_TableMatch PRIMARY KEY (TableMatchID)
)
GO



CREATE TABLE bairong_TableMetadata(
    TableMetadataID         NUMBER(38, 0)     NOT NULL,
    AuxiliaryTableENName    VARCHAR2(50)      NOT NULL,
    AttributeName           VARCHAR2(50)      DEFAULT '',
    DataType                VARCHAR2(50)      DEFAULT '',
    DataLength              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CanBeNull               VARCHAR2(18)      DEFAULT '',
    DBDefaultValue          NVARCHAR2(255)    DEFAULT '',
    Taxis                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsSystem                VARCHAR2(18)      DEFAULT '',
    CONSTRAINT PK_bairong_ATM PRIMARY KEY (TableMetadataID)
)
GO



CREATE TABLE bairong_TableStyle(
    TableStyleID       NUMBER(38, 0)     NOT NULL,
    RelatedIdentity    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    TableName          VARCHAR2(50)      DEFAULT '',
    AttributeName      VARCHAR2(50)      DEFAULT '',
    Taxis              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    DisplayName        NVARCHAR2(255)    DEFAULT '',
    HelpText           VARCHAR2(255)     DEFAULT '',
    IsVisible          VARCHAR2(18)      DEFAULT '',
    IsVisibleInList    VARCHAR2(18)      DEFAULT '',
    IsSingleLine       VARCHAR2(18)      DEFAULT '',
    InputType          VARCHAR2(50)      DEFAULT '',
    IsRequired         VARCHAR2(18)      DEFAULT '',
    DefaultValue       VARCHAR2(255)     DEFAULT '',
    IsHorizontal       VARCHAR2(18)      DEFAULT '',
    ExtendValues       NCLOB             DEFAULT '',
    CONSTRAINT PK_bairong_ATS PRIMARY KEY (TableStyleID)
)
GO



CREATE TABLE bairong_TableStyleItem(
    TableStyleItemID    NUMBER(38, 0)    NOT NULL,
    TableStyleID        NUMBER(38, 0)    NOT NULL,
    ItemTitle           VARCHAR2(255)    DEFAULT '',
    ItemValue           VARCHAR2(255)    DEFAULT '',
    IsSelected          VARCHAR2(18)     DEFAULT '',
    CONSTRAINT PK_bairong_STSI PRIMARY KEY (TableStyleItemID)
)
GO



CREATE TABLE bairong_Tags(
    TagID                  NUMBER(38, 0)     NOT NULL,
    ProductID              VARCHAR2(50)      DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentIDCollection    NVARCHAR2(255)    DEFAULT '',
    Tag                    NVARCHAR2(255)    DEFAULT '',
    UseNum                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Tags PRIMARY KEY (TagID)
)
GO



CREATE TABLE bairong_Task(
    TaskID                  NUMBER(38, 0)     NOT NULL,
    TaskName                NVARCHAR2(50)     DEFAULT '',
    ProductID               VARCHAR2(50)      DEFAULT '',
    IsSystemTask            VARCHAR2(18)      DEFAULT '',
    PublishmentSystemID     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ServiceType             VARCHAR2(50)      DEFAULT '',
    ServiceParameters       NCLOB             DEFAULT '',
    FrequencyType           VARCHAR2(50)      DEFAULT '',
    PeriodIntervalMinute    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartDay                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartWeekday            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartHour               NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsEnabled               VARCHAR2(18)      DEFAULT '',
    AddDate                 TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    LastExecuteDate         TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Description             NVARCHAR2(255)    DEFAULT '',
	OnlyOnceDate            TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_Task PRIMARY KEY (TaskID)
)
GO



CREATE TABLE bairong_TaskLog(
    ID               NUMBER(38, 0)     NOT NULL,
    TaskID           NUMBER(38, 0)     NOT NULL,
    IsSuccess        VARCHAR2(18)      DEFAULT '',
    ErrorMessage     NVARCHAR2(255)    DEFAULT '',
    StackTrace       NCLOB             DEFAULT '',
    SubtaskErrors    NCLOB             DEFAULT '',
    AddDate          TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_TaskLog PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_UserBinding(
    UserName       NVARCHAR2(255)    DEFAULT '' NOT NULL,
    BindingType    VARCHAR2(50)      DEFAULT '',
    BindingID      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    BindingName    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK325 PRIMARY KEY (UserName)
)
GO



CREATE TABLE bairong_UserConfig(
    SettingsXML    NCLOB    DEFAULT ''
)
GO



CREATE TABLE bairong_UserContact(
    ID                 NUMBER(38, 0)     NOT NULL,
    RelatedUserName    NVARCHAR2(255)    DEFAULT '',
    CreateUserName     NVARCHAR2(255)    DEFAULT '',
    Taxis              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    FullName           NVARCHAR2(50)     DEFAULT '',
    AvatarUrl          VARCHAR2(200)     DEFAULT '',
    Summary            NVARCHAR2(255)    DEFAULT '',
    Tel                VARCHAR2(50)      DEFAULT '',
    Mobile             VARCHAR2(200)     DEFAULT '',
    Email              VARCHAR2(200)     DEFAULT '',
    QQ                 VARCHAR2(50)      DEFAULT '',
    Birthday           VARCHAR2(50)      DEFAULT '',
    Organization       NVARCHAR2(50)     DEFAULT '',
    Department         NVARCHAR2(50)     DEFAULT '',
    Position           NVARCHAR2(50)     DEFAULT '',
    Address            NVARCHAR2(255)    DEFAULT '',
    Website            VARCHAR2(200)     DEFAULT '',
    CONSTRAINT PK_bairong_UserContact PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_UserCreditsLog(
    ID             NUMBER(38, 0)     NOT NULL,
    UserName       NVARCHAR2(255)    DEFAULT '',
    ProductID      VARCHAR2(50)      DEFAULT '',
    IsIncreased    VARCHAR2(18)      DEFAULT '',
    Num            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Action         NVARCHAR2(255)    DEFAULT '',
    Description    NVARCHAR2(255)    DEFAULT '',
    AddDate        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_UserCreditsLog PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_UserDownload(
    ID                NUMBER(38, 0)     NOT NULL,
    CreateUserName    NVARCHAR2(255)    DEFAULT '',
    Taxis             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Downloads         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    FileName          NVARCHAR2(255)    DEFAULT '',
    FileUrl           VARCHAR2(200)     DEFAULT '',
    Summary           NVARCHAR2(255)    DEFAULT '',
    AddDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_UserDownload PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_UserMessage(
    ID             NUMBER(38, 0)     DEFAULT '' NOT NULL,
    MessageFrom    NVARCHAR2(255)    DEFAULT '',
    MessageTo      NVARCHAR2(255)    DEFAULT '',
    MessageType    VARCHAR2(50)      DEFAULT '',
    ParentID       NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsViewed       VARCHAR2(18)      DEFAULT '',
    AddDate        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Content        NCLOB             DEFAULT '',
    LastAddDate    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    LastContent    NCLOB             DEFAULT '',
    CONSTRAINT PK_bairong_UserMessage PRIMARY KEY (ID)
)
GO



CREATE TABLE bairong_Users(
    UserName            NVARCHAR2(255)    NOT NULL,
    Password            NVARCHAR2(255)    DEFAULT '',
    PasswordFormat      VARCHAR2(50)      DEFAULT '',
    PasswordSalt        NVARCHAR2(128)    DEFAULT '',
    CreateDate          TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CreateIPAddress     VARCHAR2(50)      DEFAULT '',
    CreateUserName      NVARCHAR2(255)    DEFAULT '',
    PointCount          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    LastActivityDate    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IsChecked           VARCHAR2(18)      DEFAULT '',
    IsLockedOut         VARCHAR2(18)      DEFAULT '',
    IsTemporary         VARCHAR2(18)      DEFAULT '',
    DisplayName         NVARCHAR2(255)    DEFAULT '',
    Email               NVARCHAR2(255)    DEFAULT '',
    Mobile              VARCHAR2(20)      DEFAULT '',
    TypeID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    DepartmentID        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AreaID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    OnlineSeconds       NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AvatarLarge         VARCHAR2(200)     DEFAULT '',
    AvatarMiddle        VARCHAR2(200)     DEFAULT '',
    AvatarSmall         VARCHAR2(200)     DEFAULT '',
    Signature           NVARCHAR2(255)    DEFAULT '',
    SettingsXML         NCLOB             DEFAULT '',
    CONSTRAINT PK_bairong_Users PRIMARY KEY (UserName)
)
GO



CREATE TABLE bairong_UserType(
    TypeID           NUMBER(38, 0)    NOT NULL,
    TypeName         NVARCHAR2(50)    DEFAULT '',
    IsDefault        VARCHAR2(18)     DEFAULT '',
    IsPermissions    VARCHAR2(18)     DEFAULT '',
    Permissions      CLOB             DEFAULT '',
    CONSTRAINT PK_bairong_UserType PRIMARY KEY (TypeID)
)
GO



CREATE INDEX IX_bairong_TM_ATE ON bairong_TableMetadata(AuxiliaryTableENName)
GO
CREATE INDEX IX_bairong_TSI_TSI ON bairong_TableStyleItem(TableStyleID)
GO
ALTER TABLE bairong_AdministratorsInRoles ADD CONSTRAINT FK_bairong_AInR_A 
    FOREIGN KEY (UserName)
    REFERENCES bairong_Administrator(UserName) ON DELETE CASCADE
GO

ALTER TABLE bairong_AdministratorsInRoles ADD CONSTRAINT FK_bairong_AInR_R 
    FOREIGN KEY (RoleName)
    REFERENCES bairong_Roles(RoleName) ON DELETE CASCADE
GO


ALTER TABLE bairong_CloudStorage ADD CONSTRAINT FK_bairong_Storage_Cloud 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
GO


ALTER TABLE bairong_FTPStorage ADD CONSTRAINT FK_bairong_Storage_FTP 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
GO


ALTER TABLE bairong_LocalStorage ADD CONSTRAINT FK_bairong_Storage_Local 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
GO


ALTER TABLE bairong_TableMetadata ADD CONSTRAINT FK_bairong_ATM_AT 
    FOREIGN KEY (AuxiliaryTableENName)
    REFERENCES bairong_TableCollection(TableENName) ON DELETE CASCADE
GO


ALTER TABLE bairong_TableStyleItem ADD CONSTRAINT FK_bairong_ATSI_ATS 
    FOREIGN KEY (TableStyleID)
    REFERENCES bairong_TableStyle(TableStyleID) ON DELETE CASCADE
GO


ALTER TABLE bairong_TaskLog ADD CONSTRAINT FK_bairong_Task_Log 
    FOREIGN KEY (TaskID)
    REFERENCES bairong_Task(TaskID) ON DELETE CASCADE
GO


