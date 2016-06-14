CREATE TABLE siteserver_StarSetting(
    StarSettingID          int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    ChannelID              int               DEFAULT 0 NOT NULL,
    ContentID              int               DEFAULT 0 NOT NULL,
    TotalCount             int               DEFAULT 0 NOT NULL,
    PointAverage           decimal(18, 1)    DEFAULT 0 NULL,
    CONSTRAINT PK_bairong_StarSetting PRIMARY KEY NONCLUSTERED (StarSettingID)
)

GO

CREATE TABLE bairong_UserGroups(
    GroupID        int             IDENTITY(1,1),
    GroupName      nvarchar(50)    DEFAULT '' NOT NULL,
    IsCredits      varchar(18)     DEFAULT '' NOT NULL,
    CreditsFrom    int             DEFAULT 0 NOT NULL,
    CreditsTo      int             DEFAULT 0 NOT NULL,
    Stars          int             DEFAULT 0 NOT NULL,
    Color          varchar(10)     DEFAULT '' NOT NULL,
    Rank           int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_UserGroups PRIMARY KEY NONCLUSTERED (GroupID)
)

GO

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('新手上路', 'True', 0, 50, 1, '', 10)

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('注册会员', 'True', 50, 200, 2, '', 20)

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('中级会员', 'True', 200, 500, 3, '', 30)

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('高级会员', 'True', 500, 1000, 4, '', 50)

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('金牌会员', 'True', 1000, 3000, 5, '', 70)

INSERT INTO bairong_UserGroups (GroupName, IsCredits, CreditsFrom, CreditsTo, Stars, Color, Rank) VALUES ('元老会员', 'True', 3000, 9999999, 6, '', 100)

GO

CREATE TABLE bairong_UserType(
    TypeID      int             IDENTITY(1,1),
    TypeName    nvarchar(50)    DEFAULT '' NOT NULL,
    GroupID     int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_UserType PRIMARY KEY NONCLUSTERED (TypeID)
)

GO

ALTER TABLE bairong_Users ADD 
    TypeID              int              DEFAULT 0 NOT NULL,
    GroupID             int              DEFAULT 0 NOT NULL,
    Credits             int              DEFAULT 0 NOT NULL,
    IconUrl             varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl            varchar(200)     DEFAULT '' NOT NULL,
    Signature           nvarchar(255)    DEFAULT '' NOT NULL