sp_rename   'siteserver_PagePermissionsInUserGroups','siteserver_PagePermissions'

GO

sp_rename   'siteserver_PublishmentSystemPermissionsInRoles','siteserver_SystemPermissions'

GO

ALTER TABLE siteserver_Input ADD 
IsTemplate             varchar(18)     DEFAULT '' NOT NULL

GO

CREATE TABLE siteserver_Log(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserName               varchar(50)      DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Action                 nvarchar(255)    DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Event PRIMARY KEY NONCLUSTERED (ID)
)

GO

CREATE TABLE siteserver_TagStyle(
    StyleID                int             IDENTITY(1,1),
    StyleName              nvarchar(50)    DEFAULT '' NOT NULL,
    ElementName            varchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    IsTemplate             varchar(18)     DEFAULT '' NOT NULL,
    SettingsXML            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TagStyle PRIMARY KEY CLUSTERED (StyleID)
)

GO