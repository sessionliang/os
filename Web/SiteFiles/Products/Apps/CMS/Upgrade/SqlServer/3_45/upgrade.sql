sp_rename   'siteserver_GatherDatabaseRule','siteserver_GatherDatabaseRule_Backup'

GO

ALTER TABLE siteserver_GatherDatabaseRule_Backup DROP Constraint PK_siteserver_GatherDR

GO

ALTER TABLE siteserver_GatherDatabaseRule_Backup DROP Constraint FK_siteserver_GDR_N

GO

CREATE TABLE siteserver_GatherDatabaseRule(
    GatherRuleName         nvarchar(50)     NOT NULL,
    PublishmentSystemID    int              NOT NULL,
    DatabaseType           varchar(50)      DEFAULT '' NOT NULL,
    ConnectionString       varchar(255)     DEFAULT '' NOT NULL,
    RelatedTableName       varchar(255)     DEFAULT '' NOT NULL,
    RelatedIdentity        varchar(255)     DEFAULT '' NOT NULL,
    RelatedOrderBy         varchar(255)     DEFAULT '' NOT NULL,
    WhereString            nvarchar(255)    DEFAULT '' NOT NULL,
    TableMatchID           int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    GatherNum              int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    IsOrderByDesc          varchar(18)      DEFAULT '' NOT NULL,
    LastGatherDate         datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_GatherDR PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GDR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
)

GO

INSERT INTO siteserver_GatherDatabaseRule SELECT * FROM siteserver_GatherDatabaseRule_Backup

GO

sp_rename   'siteserver_GatherFileRule','siteserver_GatherFileRule_Backup'

GO

ALTER TABLE siteserver_GatherFileRule_Backup DROP Constraint PK_siteserver_GatherFileRule

GO

ALTER TABLE siteserver_GatherFileRule_Backup DROP Constraint FK_siteserver_GFR_N

GO

CREATE TABLE siteserver_GatherFileRule(
    GatherRuleName                nvarchar(50)     NOT NULL,
    PublishmentSystemID           int              NOT NULL,
    GatherUrl                     nvarchar(255)    DEFAULT '' NOT NULL,
    Charset                       varchar(50)      DEFAULT '' NOT NULL,
    LastGatherDate                datetime         DEFAULT getdate() NOT NULL,
    IsToFile                      varchar(18)      DEFAULT '' NOT NULL,
    FilePath                      nvarchar(255)    DEFAULT '' NOT NULL,
    IsSaveRelatedFiles            varchar(18)      DEFAULT '' NOT NULL,
    IsRemoveScripts               varchar(18)      DEFAULT '' NOT NULL,
    StyleDirectoryPath            nvarchar(255)    DEFAULT '' NOT NULL,
    ScriptDirectoryPath           nvarchar(255)    DEFAULT '' NOT NULL,
    ImageDirectoryPath            nvarchar(255)    DEFAULT '' NOT NULL,
    NodeID                        int              DEFAULT 0 NOT NULL,
    IsSaveImage                   varchar(18)      DEFAULT '' NOT NULL,
    IsChecked                     varchar(18)      DEFAULT '' NOT NULL,
    ContentExclude                ntext            DEFAULT '' NOT NULL,
    ContentHtmlClearCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    ContentTitleStart             ntext            DEFAULT '' NOT NULL,
    ContentTitleEnd               ntext            DEFAULT '' NOT NULL,
    ContentContentStart           ntext            DEFAULT '' NOT NULL,
    ContentContentEnd             ntext            DEFAULT '' NOT NULL,
    ContentAttributes             ntext            DEFAULT '' NOT NULL,
    ContentAttributesXML          ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_GatherFileRule PRIMARY KEY NONCLUSTERED (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GFR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
)

GO

INSERT INTO siteserver_GatherFileRule SELECT * FROM siteserver_GatherFileRule_Backup

GO

sp_rename   'siteserver_GatherRule','siteserver_GatherRule_Backup'

GO

ALTER TABLE siteserver_GatherRule_Backup DROP Constraint PK_siteserver_GatherRule

GO

ALTER TABLE siteserver_GatherRule_Backup DROP Constraint FK_siteserver_GR_N

GO

CREATE TABLE siteserver_GatherRule(
    GatherRuleName                nvarchar(50)     NOT NULL,
    PublishmentSystemID           int              NOT NULL,
    CookieString                  text             DEFAULT '' NOT NULL,
    GatherUrlIsCollection         varchar(18)      DEFAULT '' NOT NULL,
    GatherUrlCollection           text             DEFAULT '' NOT NULL,
    GatherUrlIsSerialize          varchar(18)      DEFAULT '' NOT NULL,
    GatherUrlSerialize            varchar(200)     DEFAULT '' NOT NULL,
    SerializeFrom                 int              DEFAULT 0 NOT NULL,
    SerializeTo                   int              DEFAULT 0 NOT NULL,
    SerializeInterval             int              DEFAULT 0 NOT NULL,
    SerializeIsOrderByDesc        varchar(18)      DEFAULT '' NOT NULL,
    SerializeIsAddZero            varchar(18)      DEFAULT '' NOT NULL,
    NodeID                        int              DEFAULT 0 NOT NULL,
    Charset                       varchar(50)      DEFAULT '' NOT NULL,
    UrlInclude                    varchar(200)     DEFAULT '' NOT NULL,
    TitleInclude                  nvarchar(255)    DEFAULT '' NOT NULL,
    ContentExclude                ntext            DEFAULT '' NOT NULL,
    ContentHtmlClearCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    LastGatherDate                datetime         DEFAULT getdate() NOT NULL,
    ListAreaStart                 ntext            DEFAULT '' NOT NULL,
    ListAreaEnd                   ntext            DEFAULT '' NOT NULL,
    ContentChannelStart           ntext            DEFAULT '' NOT NULL,
    ContentChannelEnd             ntext            DEFAULT '' NOT NULL,
    ContentTitleStart             ntext            DEFAULT '' NOT NULL,
    ContentTitleEnd               ntext            DEFAULT '' NOT NULL,
    ContentContentStart           ntext            DEFAULT '' NOT NULL,
    ContentContentEnd             ntext            DEFAULT '' NOT NULL,
    ContentNextPageStart          ntext            DEFAULT '' NOT NULL,
    ContentNextPageEnd            ntext            DEFAULT '' NOT NULL,
    ContentAttributes             ntext            DEFAULT '' NOT NULL,
    ContentAttributesXML          ntext            DEFAULT '' NOT NULL,
    ExtendValues                  ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_GatherRule PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
)

GO

INSERT INTO siteserver_GatherRule SELECT * FROM siteserver_GatherRule_Backup

GO

CREATE TABLE siteserver_Comment(
    ID                     int              IDENTITY(1,1),
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
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Comment PRIMARY KEY NONCLUSTERED (ID)
)

GO