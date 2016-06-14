CREATE TABLE siteserver_UserGroup(
    GroupID            counter NOT NULL,
    GroupName          text(50)    DEFAULT "" NOT NULL,
    IsCredits          text(18)    DEFAULT "" NOT NULL,
    CreditsFrom        integer              DEFAULT 0 NOT NULL,
    CreditsTo          integer              DEFAULT 0 NOT NULL,
    Stars              integer              DEFAULT 0 NOT NULL,
    Color              text(10)    DEFAULT "" NOT NULL,
    Rank               integer              DEFAULT 0 NOT NULL,
    UserPermissions    memo           DEFAULT "" NOT NULL,
	CONSTRAINT PK_siteserver_UserGroup PRIMARY KEY (GroupID)
)

GO

CREATE TABLE siteserver_Users(
    UserName       text(255)    DEFAULT "" NOT NULL,
    GroupID        integer              DEFAULT 0 NOT NULL,
    Credits        integer              DEFAULT 0 NOT NULL,
    SettingsXML    memo           DEFAULT "" NOT NULL,
	CONSTRAINT PK_siteserver_Users PRIMARY KEY (UserName)
)

GO