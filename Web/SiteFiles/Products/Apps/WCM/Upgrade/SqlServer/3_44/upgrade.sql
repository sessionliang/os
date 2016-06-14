CREATE TABLE siteserver_UserGroup(
    GroupID            int             IDENTITY(1,1),
    GroupName          nvarchar(50)    DEFAULT '' NOT NULL,
    IsCredits          varchar(18)     DEFAULT '' NOT NULL,
    CreditsFrom        int             DEFAULT 0 NOT NULL,
    CreditsTo          int             DEFAULT 0 NOT NULL,
    Stars              int             DEFAULT 0 NOT NULL,
    Color              varchar(10)     DEFAULT '' NOT NULL,
    Rank               int             DEFAULT 0 NOT NULL,
    UserPermissions    ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_UserGroup PRIMARY KEY NONCLUSTERED (GroupID)
)

GO

CREATE TABLE siteserver_Users(
    UserName       nvarchar(255)    NOT NULL,
    GroupID        int              DEFAULT 0 NOT NULL,
    Credits        int              DEFAULT 0 NOT NULL,
    SettingsXML    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Users PRIMARY KEY NONCLUSTERED (UserName)
)

GO