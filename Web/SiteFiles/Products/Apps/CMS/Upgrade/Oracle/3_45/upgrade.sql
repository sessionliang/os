DROP TABLE siteserver_GatherDatabaseRule

GO

CREATE TABLE siteserver_GatherDatabaseRule(
    GatherRuleName         NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    DatabaseType           VARCHAR2(50)      DEFAULT '',
    ConnectionString       VARCHAR2(255)     DEFAULT '',
    RelatedTableName       VARCHAR2(255)     DEFAULT '',
    RelatedIdentity        VARCHAR2(255)     DEFAULT '',
    RelatedOrderBy         VARCHAR2(255)     DEFAULT '',
    WhereString            NVARCHAR2(255)    DEFAULT '',
    TableMatchID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    GatherNum              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsChecked              VARCHAR2(18)      DEFAULT '',
    IsOrderByDesc          VARCHAR2(18)      DEFAULT '',
    LastGatherDate         TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_GatherDR PRIMARY KEY (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GDR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
)

GO

DROP TABLE siteserver_GatherFileRule

GO

CREATE TABLE siteserver_GatherFileRule(
    GatherRuleName                NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID           NUMBER(38, 0)     NOT NULL,
    GatherUrl                     NVARCHAR2(255)    DEFAULT '',
    Charset                       VARCHAR2(50)      DEFAULT '',
    LastGatherDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IsToFile                      VARCHAR2(18)      DEFAULT '',
    FilePath                      NVARCHAR2(255)    DEFAULT '',
    IsSaveRelatedFiles            VARCHAR2(18)      DEFAULT '',
    IsRemoveScripts               VARCHAR2(18)      DEFAULT '',
    StyleDirectoryPath            NVARCHAR2(255)    DEFAULT '',
    ScriptDirectoryPath           NVARCHAR2(255)    DEFAULT '',
    ImageDirectoryPath            NVARCHAR2(255)    DEFAULT '',
    NodeID                        NUMBER(38, 0)     NOT NULL,
    IsSaveImage                   VARCHAR2(18)      DEFAULT '',
    IsChecked                     VARCHAR2(18)      DEFAULT '',
    ContentExclude                NCLOB             DEFAULT '',
    ContentHtmlClearCollection    NVARCHAR2(255)    DEFAULT '',
    ContentTitleStart             NCLOB             DEFAULT '',
    ContentTitleEnd               NCLOB             DEFAULT '',
    ContentContentStart           NCLOB             DEFAULT '',
    ContentContentEnd             NCLOB             DEFAULT '',
    ContentAttributes             NCLOB             DEFAULT '',
    ContentAttributesXML          NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_GatherFileRule PRIMARY KEY (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GFR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
)

GO

DROP TABLE siteserver_GatherRule

GO

CREATE TABLE siteserver_GatherRule(
    GatherRuleName                NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID           NUMBER(38, 0)     NOT NULL,
    CookieString                  CLOB              DEFAULT '',
    GatherUrlIsCollection         VARCHAR2(18)      DEFAULT '',
    GatherUrlCollection           CLOB              DEFAULT '',
    GatherUrlIsSerialize          VARCHAR2(18)      DEFAULT '',
    GatherUrlSerialize            VARCHAR2(200)     DEFAULT '',
    SerializeFrom                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeTo                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeInterval             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeIsOrderByDesc        VARCHAR2(18)      DEFAULT '',
    SerializeIsAddZero            VARCHAR2(18)      DEFAULT '',
    NodeID                        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Charset                       VARCHAR2(50)      DEFAULT '',
    UrlInclude                    VARCHAR2(200)     DEFAULT '',
    TitleInclude                  NVARCHAR2(255)    DEFAULT '',
    ContentExclude                NCLOB             DEFAULT '',
    ContentHtmlClearCollection    NVARCHAR2(255)    DEFAULT '',
    LastGatherDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    ListAreaStart                 NCLOB             DEFAULT '',
    ListAreaEnd                   NCLOB             DEFAULT '',
    ContentChannelStart           NCLOB             DEFAULT '',
    ContentChannelEnd             NCLOB             DEFAULT '',
    ContentTitleStart             NCLOB             DEFAULT '',
    ContentTitleEnd               NCLOB             DEFAULT '',
    ContentContentStart           NCLOB             DEFAULT '',
    ContentContentEnd             NCLOB             DEFAULT '',
    ContentNextPageStart          NCLOB             DEFAULT '',
    ContentNextPageEnd            NCLOB             DEFAULT '',
    ContentAttributes             NCLOB             DEFAULT '',
    ContentAttributesXML          NCLOB             DEFAULT '',
    ExtendValues                  NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_GatherRule PRIMARY KEY (GatherRuleName, PublishmentSystemID), 
    CONSTRAINT FK_siteserver_GR_N FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
)

GO

CREATE TABLE siteserver_Comment(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ReferenceID            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Good                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Bad                    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    IPAddress              VARCHAR2(50)      DEFAULT '',
    Location               NVARCHAR2(255)    DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsChecked              VARCHAR2(18)      DEFAULT '',
    Title                  NVARCHAR2(255)    DEFAULT '',
    Content                NCLOB             DEFAULT '',
    SettingsXML            NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_Comment PRIMARY KEY (ID)
)

GO

CREATE SEQUENCE SITESERVER_COMMENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER

GO