/*
 * ER/Studio 8.0 SQL Code Generation
 * Company :      bairongsoft
 * Project :      bbs.dm1
 * Author :       ThinkPad
 *
 * Date Created : Monday, July 28, 2014 09:54:38
 * Target DBMS : Microsoft SQL Server 2000
 */

CREATE TABLE bbs_Ad(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AdName                 nvarchar(50)     DEFAULT '' NOT NULL,
    AdType                 varchar(50)      DEFAULT '' NOT NULL,
    AdLocation             varchar(50)      DEFAULT '' NOT NULL,
    Code                   ntext            DEFAULT '' NOT NULL,
    TextWord               nvarchar(255)    DEFAULT '' NOT NULL,
    TextLink               varchar(200)     DEFAULT '' NOT NULL,
    TextColor              varchar(10)      DEFAULT '' NOT NULL,
    TextFontSize           int              DEFAULT 0 NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    ImageLink              varchar(200)     DEFAULT '' NOT NULL,
    ImageWidth             int              DEFAULT 0 NOT NULL,
    ImageHeight            int              DEFAULT 0 NOT NULL,
    ImageAlt               nvarchar(50)     DEFAULT '' NOT NULL,
    IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
    IsDateLimited          varchar(18)      DEFAULT '' NOT NULL,
    StartDate              datetime         DEFAULT getdate() NOT NULL,
    EndDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bbs_Ad PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Announcement(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    FormatString           varchar(50)      DEFAULT '' NOT NULL,
    LinkUrl                varchar(200)     DEFAULT '' NOT NULL,
    IsBlank                varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Announcement PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Attachment(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ThreadID               int              DEFAULT 0 NOT NULL,
    PostID                 int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(50)     DEFAULT '' NOT NULL,
    IsInContent            varchar(18)      DEFAULT '' NOT NULL,
    IsImage                varchar(18)      DEFAULT '' NOT NULL,
    Price                  int              DEFAULT 0 NOT NULL,
    FileName               nvarchar(50)     DEFAULT '' NOT NULL,
    FileType               varchar(50)      DEFAULT '' NOT NULL,
    FileSize               int              DEFAULT 0 NOT NULL,
    AttachmentUrl          varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Downloads              int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Description            nvarchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Attachment PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_AttachmentType(
    ID                     int            IDENTITY(1,1),
    PublishmentSystemID    int            DEFAULT 0 NOT NULL,
    FileExtName            varchar(10)    DEFAULT '' NOT NULL,
    MaxSize                int            DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_AttachmentType PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Configuration(
    PublishmentSystemID    int      NOT NULL,
    SettingsXML            ntext    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Configuration PRIMARY KEY NONCLUSTERED (PublishmentSystemID)
)
go



CREATE TABLE bbs_CreditRule(
    ID                     int            IDENTITY(1,1),
    PublishmentSystemID    int            DEFAULT 0 NOT NULL,
    RuleType               varchar(50)    DEFAULT '' NOT NULL,
    ForumID                int            DEFAULT 0 NOT NULL,
    PeriodType             varchar(50)    DEFAULT '' NOT NULL,
    PeriodCount            int            DEFAULT 0 NOT NULL,
    MaxNum                 int            DEFAULT 0 NOT NULL,
    Prestige               int            DEFAULT 0 NOT NULL,
    Contribution           int            DEFAULT 0 NOT NULL,
    Currency               int            DEFAULT 0 NOT NULL,
    ExtCredit1             int            DEFAULT 0 NOT NULL,
    ExtCredit2             int            DEFAULT 0 NOT NULL,
    ExtCredit3             int            DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_CreditRule PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_CreditRuleLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    RuleType               varchar(50)      DEFAULT '' NOT NULL,
    TotalCount             int              DEFAULT 0 NOT NULL,
    PeriodCount            int              DEFAULT 0 NOT NULL,
    Prestige               int              DEFAULT 0 NOT NULL,
    Contribution           int              DEFAULT 0 NOT NULL,
    Currency               int              DEFAULT 0 NOT NULL,
    ExtCredit1             int              DEFAULT 0 NOT NULL,
    ExtCredit2             int              DEFAULT 0 NOT NULL,
    ExtCredit3             int              DEFAULT 0 NOT NULL,
    LastDate               datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bbs_CreditRuleLog PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Face(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    FaceName               varchar(50)     DEFAULT '' NOT NULL,
    Title                  nvarchar(50)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    IsEnabled              varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Face PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Forum(
    ForumID                int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ForumName              nvarchar(255)    DEFAULT '' NOT NULL,
    IndexName              nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID               int              DEFAULT 0 NOT NULL,
    ParentsPath            nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount           int              DEFAULT 0 NOT NULL,
    ChildrenCount          int              DEFAULT 0 NOT NULL,
    IsLastNode             varchar(18)      DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    IconUrl                varchar(200)     DEFAULT '' NOT NULL,
    Color                  varchar(50)      DEFAULT '' NOT NULL,
    Columns                int              DEFAULT 0 NOT NULL,
    MetaKeywords           nvarchar(255)    DEFAULT '' NOT NULL,
    MetaDescription        nvarchar(255)    DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    FilePath               varchar(200)     DEFAULT '' NOT NULL,
    FilePathRule           varchar(200)     DEFAULT '' NOT NULL,
    TemplateID             int              DEFAULT 0 NOT NULL,
    LinkUrl                varchar(200)     DEFAULT '' NOT NULL,
    ThreadCount            int              DEFAULT 0 NOT NULL,
    TodayThreadCount       int              DEFAULT 0 NOT NULL,
    PostCount              int              DEFAULT 0 NOT NULL,
    TodayPostCount         int              DEFAULT 0 NOT NULL,
    LastThreadID           int              DEFAULT 0 NOT NULL,
    LastPostID             int              DEFAULT 0 NOT NULL,
    LastTitle              nvarchar(255)    DEFAULT '' NOT NULL,
    LastUserName           nvarchar(255)    DEFAULT '' NOT NULL,
    LastDate               datetime         DEFAULT getdate() NOT NULL,
    State                  varchar(50)      DEFAULT '' NOT NULL,
    ExtendValues           ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Forum PRIMARY KEY NONCLUSTERED (ForumID)
)
go



CREATE TABLE bbs_Icon(
    ID                     char(10)    NOT NULL,
    PublishmentSystemID    int         DEFAULT 0 NOT NULL,
    IconUrl                char(10)    NULL,
    Taxis                  char(10)    NULL,
    CONSTRAINT PK_bbs_Icon PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Identify(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Title                  nvarchar(50)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    StampUrl               varchar(200)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_Identify PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_KeywordsCategory(
    CategoryID             int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    CategoryName           nvarchar(50)    DEFAULT '' NOT NULL,
    IsOpen                 varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_KeywordsCategory PRIMARY KEY NONCLUSTERED (CategoryID)
)
go



CREATE TABLE bbs_KeywordsFilter(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    CategoryID             int             DEFAULT 0 NOT NULL,
    Grade                  int             DEFAULT 0 NOT NULL,
    Name                   nvarchar(50)    DEFAULT '' NOT NULL,
    Replacement            nvarchar(50)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_KeywordsFilter PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Link(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    LinkName               nvarchar(50)    DEFAULT '' NOT NULL,
    LinkUrl                varchar(200)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_Links PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Navigation(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    NavType                varchar(50)      DEFAULT '' NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    FormatString           varchar(50)      DEFAULT '' NOT NULL,
    LinkUrl                varchar(200)     DEFAULT '' NOT NULL,
    IsBlank                varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Navigation PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Online(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    UserName               nvarchar(50)    DEFAULT '' NOT NULL,
    SessionID              varchar(50)     DEFAULT '' NOT NULL,
    ActiveTime             datetime        DEFAULT getdate() NOT NULL,
    IPAddress              varchar(50)     DEFAULT '' NOT NULL,
    IsHide                 varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Online PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Permissions(
    UserGroupID            int     NOT NULL,
    PublishmentSystemID    int     DEFAULT 0 NOT NULL,
    ForumID                int     NOT NULL,
    Forbidden              text    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Permissions PRIMARY KEY CLUSTERED (ForumID, UserGroupID)
)
go



CREATE TABLE bbs_Poll(
    ID                     int            IDENTITY(1,1),
    PublishmentSystemID    int            DEFAULT 0 NOT NULL,
    ThreadID               int            DEFAULT 0 NOT NULL,
    IsVoteFirst            varchar(18)    DEFAULT '' NOT NULL,
    MaxNum                 int            DEFAULT 0 NOT NULL,
    RestrictType           varchar(50)    DEFAULT '' NOT NULL,
    AddDate                datetime       DEFAULT getdate() NOT NULL,
    Deadline               datetime       DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bbs_Poll PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE bbs_PollItem(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    PollID                 int              NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Num                    int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_PollItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_PollUser(
    ID                      int              IDENTITY(1,1),
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    PollID                  int              NOT NULL,
    PollItemIDCollection    varchar(200)     DEFAULT '' NOT NULL,
    IPAddress               varchar(50)      DEFAULT '' NOT NULL,
    UserName                nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                 datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bbs_PollUser PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Post(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ThreadID               int              DEFAULT 0 NOT NULL,
    ForumID                int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(50)     DEFAULT '' NOT NULL,
    LastEditUserName       nvarchar(50)     DEFAULT '' NOT NULL,
    LastEditDate           datetime         DEFAULT getdate() NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    Assessor               nvarchar(50)     DEFAULT '' NOT NULL,
    CheckDate              datetime         DEFAULT getdate() NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    IsThread               varchar(18)      DEFAULT '' NOT NULL,
    IsBanned               varchar(18)      DEFAULT '' NOT NULL,
    IsAnonymous            varchar(18)      DEFAULT '' NOT NULL,
    IsHtml                 varchar(18)      DEFAULT '' NOT NULL,
    IsBBCodeOff            varchar(18)      DEFAULT '' NOT NULL,
    IsSmileyOff            varchar(18)      DEFAULT '' NOT NULL,
    IsUrlOff               varchar(18)      DEFAULT '' NOT NULL,
    IsSignature            varchar(18)      DEFAULT '' NOT NULL,
    IsAttachment           varchar(18)      DEFAULT '' NOT NULL,
    IsHandled              varchar(18)      DEFAULT '' NOT NULL,
    State                  varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Post PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Report(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ForumID                int              DEFAULT 0 NOT NULL,
    ThreadID               int              DEFAULT 0 NOT NULL,
    PostID                 int              DEFAULT 0 NOT NULL,
    UserName               varchar(50)      DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Content                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Report PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_Thread(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AreaID                 int              DEFAULT 0 NOT NULL,
    ForumID                int              DEFAULT 0 NOT NULL,
    IconID                 int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(50)     DEFAULT '' NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    LastDate               datetime         DEFAULT getdate() NOT NULL,
    LastPostID             int              DEFAULT 0 NOT NULL,
    LastUserName           nvarchar(50)     DEFAULT '' NOT NULL,
    Hits                   int              DEFAULT 0 NOT NULL,
    Replies                int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    IsLocked               varchar(18)      DEFAULT '' NOT NULL,
    IsImage                varchar(18)      DEFAULT '' NOT NULL,
    IsAttachment           varchar(18)      DEFAULT '' NOT NULL,
    CategoryID             int              DEFAULT 0 NOT NULL,
    TopLevel               int              DEFAULT 0 NOT NULL,
    TopLevelDate           datetime         DEFAULT getdate() NOT NULL,
    DigestLevel            int              DEFAULT 0 NOT NULL,
    DigestDate             datetime         DEFAULT getdate() NOT NULL,
    Highlight              varchar(50)      DEFAULT '' NOT NULL,
    HighlightDate          datetime         DEFAULT getdate() NOT NULL,
    IdentifyID             int              DEFAULT 0 NOT NULL,
    ThreadType             varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Thread PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE bbs_ThreadCategory(
    CategoryID             int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ForumID                int              DEFAULT 0 NOT NULL,
    CategoryName           nvarchar(50)     DEFAULT '' NOT NULL,
    Summary                nvarchar(500)    DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bbs_ThreadCategory PRIMARY KEY NONCLUSTERED (CategoryID)
)
go



CREATE TABLE bbs_Users(
    UserID             int              NOT NULL,
    GroupSN            nvarchar(255)    DEFAULT '' NOT NULL,
    UserName           nvarchar(255)    DEFAULT '' NOT NULL,
    PostCount          int              DEFAULT 0 NOT NULL,
    PostDigestCount    int              DEFAULT 0 NOT NULL,
    Prestige           int              DEFAULT 0 NOT NULL,
    Contribution       int              DEFAULT 0 NOT NULL,
    Currency           int              DEFAULT 0 NOT NULL,
    ExtCredit1         int              DEFAULT 0 NOT NULL,
    ExtCredit2         int              DEFAULT 0 NOT NULL,
    ExtCredit3         int              DEFAULT 0 NOT NULL,
    LastPostDate       datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bbs_Users PRIMARY KEY NONCLUSTERED (UserID)
)
go



ALTER TABLE bbs_PollItem ADD CONSTRAINT FK_bbs_PollItem_Poll 
    FOREIGN KEY (PollID)
    REFERENCES bbs_Poll(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bbs_PollUser ADD CONSTRAINT FK_bbs_PollUser_Poll 
    FOREIGN KEY (PollID)
    REFERENCES bbs_Poll(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


