CREATE TABLE siteserver_UserGroup(
    GroupID            NUMBER(38, 0)    NOT NULL,
    GroupName          NVARCHAR2(50)    DEFAULT '',
    IsCredits          VARCHAR2(18)     DEFAULT '',
    CreditsFrom        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    CreditsTo          NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Stars              NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Color              VARCHAR2(10)     DEFAULT '',
    Rank               NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    UserPermissions    NCLOB            DEFAULT '',
    CONSTRAINT PK_siteserver_UserGroup PRIMARY KEY (GroupID)
)

GO

CREATE SEQUENCE SITESERVER_USERGROUP_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER

GO

CREATE TABLE siteserver_Users(
    UserName       NVARCHAR2(255)    NOT NULL,
    GroupID        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Credits        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SettingsXML    NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_Users PRIMARY KEY (UserName)
)

GO