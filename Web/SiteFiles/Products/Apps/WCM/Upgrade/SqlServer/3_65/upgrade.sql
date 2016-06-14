CREATE TABLE wcm_TemplateLog(
    ID                     int              IDENTITY(1,1),
    TemplateID             int              NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    AddUserName            nvarchar(255)    DEFAULT '' NOT NULL,
    ContentLength          int              DEFAULT 0 NOT NULL,
    TemplateContent        ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_TemplateLog PRIMARY KEY NONCLUSTERED (ID), 
    CONSTRAINT FK_Template_Log FOREIGN KEY (TemplateID)
    REFERENCES wcm_Template(TemplateID) ON DELETE CASCADE ON UPDATE CASCADE
)
go

CREATE TABLE wcm_AdArea(
    AdAreaID               int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AdAreaName             nvarchar(50)     DEFAULT '' NOT NULL,
    Width                  int              DEFAULT 0 NOT NULL,
    Height                 int              DEFAULT 0 NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_AdArea PRIMARY KEY NONCLUSTERED (AdAreaID)
)
go

CREATE TABLE wcm_Adv(
    AdvID                        int              IDENTITY(1,1),
    PublishmentSystemID          int              DEFAULT 0 NOT NULL,
    AdAreaID                     int              DEFAULT 0 NOT NULL,
    AdvName                      nvarchar(50)     DEFAULT '' NOT NULL,
    Summary                      nvarchar(255)    DEFAULT '' NOT NULL,
    IsEnabled                    varchar(18)      DEFAULT '' NOT NULL,
    IsDateLimited                varchar(18)      DEFAULT '' NOT NULL,
    StartDate                    datetime         DEFAULT getdate() NOT NULL,
    EndDate                      datetime         DEFAULT getdate() NOT NULL,
    LevelType                    nvarchar(50)     DEFAULT '' NOT NULL,
    Level                        int              DEFAULT 0 NOT NULL,
    IsWeight                     varchar(18)      DEFAULT '' NOT NULL,
    Weight                       int              DEFAULT 0 NOT NULL,
    RotateType                   nvarchar(50)     DEFAULT '' NOT NULL,
    RotateInterval               int              DEFAULT 0 NOT NULL,
    NodeIDCollectionToChannel    nvarchar(255)    DEFAULT '' NOT NULL,
    NodeIDCollectionToContent    nvarchar(255)    DEFAULT '' NOT NULL,
    FileTemplateIDCollection     nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Adv PRIMARY KEY NONCLUSTERED (AdvID), 
    CONSTRAINT FK_wcm_AdArea_Adv FOREIGN KEY (AdAreaID)
    REFERENCES wcm_AdArea(AdAreaID) ON DELETE CASCADE ON UPDATE CASCADE
)
go

CREATE TABLE wcm_AdMaterial(
    AdMaterialID           int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AdvID                  int              DEFAULT 0 NOT NULL,
    AdvertID               int              DEFAULT 0 NOT NULL,
    AdMaterialName         nvarchar(50)     DEFAULT '' NOT NULL,
    AdMaterialType         varchar(50)      DEFAULT '' NOT NULL,
    Code                   ntext            DEFAULT '' NOT NULL,
    TextWord               nvarchar(255)    DEFAULT '' NOT NULL,
    TextLink               varchar(200)     DEFAULT '' NOT NULL,
    TextColor              varchar(10)      DEFAULT '' NOT NULL,
    TextFontSize           int              DEFAULT 0 NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    ImageLink              varchar(200)     DEFAULT '' NOT NULL,
    ImageWidth             int              DEFAULT 0 NOT NULL,
    ImageHeight            int              DEFAULT 0 NOT NULL,
    ImageAlt               nvarchar(50)     DEFAULT '' NOT NULL,
    Weight                 int              DEFAULT 0 NOT NULL,
    IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_AdMaterial PRIMARY KEY NONCLUSTERED (AdMaterialID), 
    CONSTRAINT FK_wcm_Adv_AdMaterial FOREIGN KEY (AdvID)
    REFERENCES wcm_Adv(AdvID) ON DELETE CASCADE ON UPDATE CASCADE
)
go