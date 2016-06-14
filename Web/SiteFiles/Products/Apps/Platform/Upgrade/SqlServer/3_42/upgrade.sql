CREATE TABLE bairong_UserCreditsLog(
    ID             int              IDENTITY(1,1),
    UserName       nvarchar(255)    DEFAULT '' NOT NULL,
    ProductID      varchar(50)      DEFAULT '' NOT NULL,
    IsIncreased    varchar(18)      DEFAULT '' NOT NULL,
    Num            int              DEFAULT 0 NOT NULL,
    Action         nvarchar(255)    DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_UserCreditsLog PRIMARY KEY NONCLUSTERED (ID)
)

GO

CREATE TABLE bairong_UserMessage(
    ID             int              IDENTITY(1,1),
    MessageFrom    nvarchar(255)    DEFAULT '' NOT NULL,
    MessageTo      nvarchar(255)    DEFAULT '' NOT NULL,
    MessageType    varchar(50)      DEFAULT '' NOT NULL,
    ParentID       int              DEFAULT 0 NOT NULL,
    IsViewed       varchar(18)      DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    Content        ntext            DEFAULT '' NOT NULL,
    LastAddDate    datetime         DEFAULT getdate() NOT NULL,
    LastContent    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserMessage PRIMARY KEY NONCLUSTERED (ID)
)

GO

ALTER TABLE bairong_Users ADD 
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    TypeID                 int              DEFAULT 0 NOT NULL,
    Theme                  varchar(50)      DEFAULT '' NOT NULL,
    IconUrl                varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NULL,
    Signature              nvarchar(255)    DEFAULT '' NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL

GO

UPDATE bairong_Users SET PublishmentSystemID = bairong_UserSettings.PublishmentSystemID, TypeID = bairong_UserSettings.TypeID, Theme = bairong_UserSettings.Theme, IconUrl = bairong_UserSettings.IconUrl, ImageUrl = bairong_UserSettings.ImageUrl, Signature = bairong_UserSettings.Signature, SettingsXML = bairong_UserSettings.SettingsXML FROM bairong_UserSettings WHERE bairong_Users.UserName = bairong_UserSettings.UserName

GO

ALTER TABLE bairong_Module ADD IsRoot varchar(18) DEFAULT '' NOT NULL

GO