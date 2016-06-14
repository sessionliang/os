ALTER TABLE bairong_Administrator ADD 
    CountOfLogin           int              DEFAULT 0 NOT NULL
go

CREATE TABLE bairong_Digg(
    DiggID                 int    IDENTITY(1,1),
    PublishmentSystemID    int    DEFAULT 0 NOT NULL,
    RelatedIdentity        int    DEFAULT 0 NOT NULL,
    Good                   int    DEFAULT 0 NOT NULL,
    Bad                    int    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Digg PRIMARY KEY NONCLUSTERED (DiggID)
)
go

CREATE TABLE bairong_Star(
    StarID                 int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    RelatedIdentity        int              DEFAULT 0 NOT NULL,
    UserID                 nvarchar(255)    DEFAULT '' NOT NULL,
    Point                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Star PRIMARY KEY NONCLUSTERED (StarID)
)
go

DROP TABLE siteserver_AdvertisementFloat
go

DROP TABLE siteserver_AdvertisementNewWindow
go

DROP TABLE siteserver_AdvertisementGroupsInTemplates
go

DROP TABLE siteserver_AdvertisementGroup
go

DROP TABLE siteserver_Advertisement
go

DROP TABLE siteserver_AdvertisementsInGroups
go

CREATE TABLE siteserver_Advertisement(
    AdvertisementName            varchar(50)      NOT NULL,
    PublishmentSystemID          int              DEFAULT 0 NOT NULL,
    AdvertisementType            varchar(50)      DEFAULT '' NOT NULL,
    NavigationUrl                varchar(200)     DEFAULT '' NOT NULL,
    IsDateLimited                varchar(18)      DEFAULT '' NOT NULL,
    StartDate                    datetime         DEFAULT getdate() NOT NULL,
    EndDate                      datetime         DEFAULT getdate() NOT NULL,
    AddDate                      datetime         DEFAULT getdate() NOT NULL,
    NodeIDCollectionToChannel    nvarchar(255)    DEFAULT '' NOT NULL,
    NodeIDCollectionToContent    nvarchar(255)    DEFAULT '' NOT NULL,
    FileTemplateIDCollection     nvarchar(255)    DEFAULT '' NOT NULL,
    Settings                     ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Advertisement PRIMARY KEY CLUSTERED (AdvertisementName, PublishmentSystemID)
)
go

DROP TABLE siteserver_Category
go

ALTER TABLE siteserver_GatherRule ADD 
    ExtendValues                  ntext            DEFAULT '' NOT NULL
go

CREATE TABLE siteserver_Tag(
    TagID                  int              IDENTITY(1,1),
    TagName                nvarchar(255)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_Tag PRIMARY KEY NONCLUSTERED (TagID)
)
go