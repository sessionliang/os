CREATE TABLE siteserver_TemplateRule(
    RuleID                 int             IDENTITY(1,1),
    RuleName               nvarchar(50)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    IndexTemplateID        int             DEFAULT 0 NOT NULL,
    ChannelTemplateID      int             DEFAULT 0 NOT NULL,
    ContentTemplateID      int             DEFAULT 0 NOT NULL,
    ChannelFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    ContentFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TemplateRule PRIMARY KEY NONCLUSTERED (RuleID), 
    CONSTRAINT FK_siteserver_TemplateRule_Node FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
)

GO

CREATE TABLE siteserver_TemplateMatch(
    NodeID                 int             NOT NULL,
    RuleID                 int             NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    ChannelTemplateID      int             DEFAULT 0 NOT NULL,
    ContentTemplateID      int             DEFAULT 0 NOT NULL,
    FilePath               varchar(200)    DEFAULT '' NOT NULL,
    ChannelFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    ContentFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TemplateMatch PRIMARY KEY NONCLUSTERED (RuleID, NodeID), 
    CONSTRAINT FK_siteserver_TemplateMatch_TemplateRule FOREIGN KEY (RuleID)
    REFERENCES siteserver_TemplateRule(RuleID) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_siteserver_TemplateMatch_Node FOREIGN KEY (NodeID)
    REFERENCES siteserver_Node(NodeID)
)

GO

ALTER TABLE siteserver_Template ADD 
    RuleID                 int            DEFAULT 0 NOT NULL

GO

UPDATE siteserver_Template SET RuleID = 0 WHERE RuleID IS NULL

GO

CREATE TABLE bairong_UserSettings(
    UserID                 nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              NOT NULL,
    TypeID                 int              DEFAULT 0 NOT NULL,
    Theme                  varchar(50)      DEFAULT '' NOT NULL,
    IconUrl                varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Signature              nvarchar(255)    DEFAULT '' NOT NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserSettings PRIMARY KEY NONCLUSTERED (UserID)
)

GO
