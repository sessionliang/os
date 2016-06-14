ALTER TABLE siteserver_Input ADD 
	StyleTemplate          ntext           DEFAULT '' NOT NULL,
    ScriptTemplate         ntext           DEFAULT '' NOT NULL,
    ContentTemplate        ntext           DEFAULT '' NOT NULL

GO

ALTER TABLE siteserver_InputContent ADD
    IPAddress      varchar(50)      DEFAULT '' NOT NULL,
    Location       nvarchar(50)     DEFAULT '' NOT NULL
	
GO

CREATE TABLE siteserver_MailSendLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    PageUrl                varchar(200)     DEFAULT '' NOT NULL,
    Receiver               nvarchar(255)    DEFAULT '' NOT NULL,
    Mail                   nvarchar(255)    DEFAULT '' NOT NULL,
    Sender                 nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_MailSendLog PRIMARY KEY CLUSTERED (ID)
)

GO

CREATE TABLE siteserver_MailSubscribe(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Receiver               nvarchar(255)    DEFAULT '' NOT NULL,
    Mail                   nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_MailSubscribe PRIMARY KEY CLUSTERED (ID)
)

GO

ALTER TABLE siteserver_TagStyle ADD
    StyleTemplate          ntext           DEFAULT '' NOT NULL,
    ScriptTemplate         ntext           DEFAULT '' NOT NULL,
    ContentTemplate        ntext           DEFAULT '' NOT NULL
	
GO

CREATE TABLE siteserver_TaskLog(
    ID                     int              IDENTITY(1,1),
    TaskName               nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ServiceType            varchar(50)      DEFAULT '' NOT NULL,
    IsSuccess              varchar(18)      DEFAULT '' NOT NULL,
    Action                 nvarchar(50)     DEFAULT '' NOT NULL,
    Message                nvarchar(255)    DEFAULT '' NOT NULL,
    StackTrace             ntext            DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_TaskLog PRIMARY KEY CLUSTERED (ID)
)

GO