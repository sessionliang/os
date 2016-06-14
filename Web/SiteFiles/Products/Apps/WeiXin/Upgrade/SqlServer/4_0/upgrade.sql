
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_Card') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_Card(
    ID                      int              IDENTITY(1,1),
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    KeywordID               int              DEFAULT 0 NOT NULL,
    IsDisabled              varchar(18)      DEFAULT '' NOT NULL,
    UserCount               int              DEFAULT 0 NOT NULL,
    PVCount                 int              DEFAULT 0 NOT NULL,
    Title                   nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                varchar(200)     DEFAULT '' NOT NULL,
    Summary                 nvarchar(255)    DEFAULT '' NOT NULL,
    CardTitle               nvarchar(255)    DEFAULT '' NOT NULL,
    CardTitleColor          nvarchar(50)     DEFAULT '' NOT NULL,
    CardNoColor             varchar(50)      DEFAULT '' NOT NULL,
    ContentFrontImageUrl    varchar(200)     DEFAULT '' NOT NULL,
    ContentBackImageUrl     varchar(200)     DEFAULT '' NOT NULL,
    ShopName                nvarchar(255)    DEFAULT '' NOT NULL,
    ShopAddress             nvarchar(255)    DEFAULT '' NOT NULL,
    ShopTel                 nvarchar(255)    DEFAULT '' NOT NULL,
    ShopPosition            varchar(200)     DEFAULT '' NOT NULL,
    ShopPassword            nvarchar(200)    DEFAULT '' NOT NULL,
    ShopOperatorList        ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Card PRIMARY KEY NONCLUSTERED (ID)
)
go


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_CardSN') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_CardSN(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    CardID                 int               DEFAULT 0 NOT NULL,
    SN                     varchar(200)      DEFAULT '' NOT NULL,
    Amount                 decimal(20, 2)    DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)       DEFAULT '' NOT NULL,
    UserName               nvarchar(255)     DEFAULT '' NOT NULL,
    AddDate                datetime          DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_CardSN PRIMARY KEY NONCLUSTERED (ID)
)
go


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_Collect') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_Collect(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    UserCount              int              DEFAULT 0 NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    StartDate              datetime         DEFAULT getdate() NOT NULL,
    EndDate                datetime         DEFAULT getdate() NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl        varchar(200)     DEFAULT '' NOT NULL,
    ContentDescription     ntext            DEFAULT '' NOT NULL,
    ContentMaxVote         int              DEFAULT 0 NOT NULL,
    ContentIsCheck         varchar(18)      DEFAULT '' NOT NULL,
    EndTitle               nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl            varchar(200)     DEFAULT '' NOT NULL,
    EndSummary             nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Collect PRIMARY KEY NONCLUSTERED (ID)
)
go

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_CollectItem') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_CollectItem(
    ID                     int              IDENTITY(1,1),
    CollectID              int              DEFAULT 0 NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    SmallUrl               varchar(200)     DEFAULT '' NOT NULL,
    LargeUrl               varchar(200)     DEFAULT '' NOT NULL,
    Description            nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile                 varchar(200)     DEFAULT '' NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    VoteNum                int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_CollectItem PRIMARY KEY NONCLUSTERED (ID)
)
go


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_CollectLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_CollectLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    CollectID              int              DEFAULT 0 NOT NULL,
    ItemID                 int              DEFAULT 0 NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_CollectLog PRIMARY KEY NONCLUSTERED (ID)
)
go

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('wx_CardEntitySN') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE wx_CardEntitySN(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    CardID                 int               DEFAULT 0 NOT NULL,
    SN                     varchar(200)      DEFAULT '' NOT NULL,
    UserName               nvarchar(255)     DEFAULT '' NOT NULL,
    Mobile                 varchar(50)       DEFAULT '' NOT NULL,
    Amount                 decimal(20, 2)    DEFAULT 0 NOT NULL,
    Credits                int               DEFAULT 0 NOT NULL,
    Email                  nvarchar(255)     DEFAULT '' NOT NULL,
    Address                nvarchar(255)     DEFAULT '' NOT NULL,
    IsBinding              varchar(18)       DEFAULT '' NOT NULL,
    AddDate                datetime          DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_CardEntitySN PRIMARY KEY NONCLUSTERED (ID)
)
go