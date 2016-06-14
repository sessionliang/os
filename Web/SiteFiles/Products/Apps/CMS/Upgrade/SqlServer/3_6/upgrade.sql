DROP TABLE siteserver_Comment
go



CREATE TABLE siteserver_Comment(
    CommentID              int              IDENTITY(1,1),
    CommentName            nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ApplyStyleID           int              DEFAULT 0 NOT NULL,
    QueryStyleID           int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Comment PRIMARY KEY NONCLUSTERED (CommentID), 
    CONSTRAINT FK_siteserver_Node_Comment FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
)
go



CREATE TABLE siteserver_CommentContent(
    ID                     int              IDENTITY(1,1),
    CommentID              int              DEFAULT 0 NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    ReferenceID            int              DEFAULT 0 NOT NULL,
    Good                   int              DEFAULT 0 NOT NULL,
    Bad                    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    Location               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    Attribute1             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute2             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute3             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute4             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute5             nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_CommentContent PRIMARY KEY NONCLUSTERED (ID)
)
go


ALTER TABLE siteserver_GatherFileRule ADD 
    ContentHtmlClearTagCollection    nvarchar(255)    DEFAULT '' NOT NULL
go


ALTER TABLE siteserver_GatherRule ADD 
    ContentHtmlClearTagCollection    nvarchar(255)    DEFAULT '' NOT NULL
go


CREATE TABLE siteserver_Keyword(
    KeywordID      int             IDENTITY(1,1),
    Keyword        nvarchar(50)    DEFAULT '' NOT NULL,
    Alternative    nvarchar(50)    DEFAULT '' NOT NULL,
    Grade          nvarchar(50)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Keyword PRIMARY KEY NONCLUSTERED (KeywordID)
)
go


ALTER TABLE siteserver_PublishmentSystem ADD 
    AuxiliaryTableForGovPublic      varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForGovInteract    varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForVote           varchar(50)     DEFAULT '' NOT NULL
go


UPDATE siteserver_PublishmentSystem SET AuxiliaryTableForVote = 'siteserver_ContentVote'
go


CREATE TABLE siteserver_SigninLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IsSignin               varchar(18)      DEFAULT '' NOT NULL,
    SigninDate             datetime         DEFAULT getdate() NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_SigninLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_SigninSetting(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IsGroup                varchar(18)      DEFAULT '' NOT NULL,
    UserGroupCollection    varchar(500)     DEFAULT '' NOT NULL,
    UserNameCollection     nvarchar(500)    DEFAULT '' NOT NULL,
    Priority               int              DEFAULT 0 NOT NULL,
    EndDate                varchar(50)      DEFAULT '' NOT NULL,
    IsSignin               varchar(18)      DEFAULT '' NOT NULL,
    SigninDate             datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_SigninSetting PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_SigninUserContentID(
    ID                     int              IDENTITY(1,1),
    IsGroup                varchar(18)      DEFAULT '' NOT NULL,
    GroupID                int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentIDCollection    varchar(500)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_SigninUserContentID PRIMARY KEY NONCLUSTERED (ID)
)
go


ALTER TABLE siteserver_TagStyle ADD 
    SuccessTemplate        ntext           DEFAULT '' NOT NULL,
    FailureTemplate        ntext           DEFAULT '' NOT NULL
go


CREATE TABLE siteserver_TouGaoSetting(
    SettingID                    int             IDENTITY(1,1),
    PublishmentSystemID          int             DEFAULT 0 NOT NULL,
    UserTypeID                   int             DEFAULT 0 NOT NULL,
    IsTouGaoAllowed              varchar(18)     DEFAULT '' NOT NULL,
    CheckLevel                   int             DEFAULT 0 NOT NULL,
    IsCheckAllowed               varchar(18)     DEFAULT '' NOT NULL,
    IsCheckAddedUsersOnly        varchar(18)     DEFAULT '' NOT NULL,
    CheckUserTypeIDCollection    varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TouGaoSetting PRIMARY KEY NONCLUSTERED (SettingID)
)
go


CREATE TABLE siteserver_VoteOperation(
    OperationID            int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_VoteOperation PRIMARY KEY NONCLUSTERED (OperationID)
)
go



CREATE TABLE siteserver_VoteOption(
    OptionID               int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    NavigationUrl          varchar(200)     DEFAULT '' NOT NULL,
    VoteNum                int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_VoteOption PRIMARY KEY NONCLUSTERED (OptionID)
)
go