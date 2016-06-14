CREATE TABLE bairong_Card(
    CardID        int              IDENTITY(1,1),
    CardSN        varchar(50)      NOT NULL,
    Password      varchar(50)      DEFAULT '' NOT NULL,
    CardTypeID    int              DEFAULT 0 NOT NULL,
    CreateTime    datetime         DEFAULT getdate() NOT NULL,
    EndTime       datetime         DEFAULT getdate() NOT NULL,
    UseTime       varchar(50)      DEFAULT '' NOT NULL,
    Status        varchar(20)      NOT NULL,
    UserName      nvarchar(255)    NOT NULL,
    CONSTRAINT PK_bairong_Card PRIMARY KEY NONCLUSTERED (CardID)
)
go



CREATE TABLE bairong_CardType(
    CardTypeID     int               IDENTITY(1,1),
    NameType       nvarchar(50)      DEFAULT '' NOT NULL,
    CardCount      int               DEFAULT 0 NOT NULL,
    Price          decimal(20, 2)    DEFAULT 0 NOT NULL,
    Description    nvarchar(255)     DEFAULT '' NOT NULL,
    AddTime        datetime          DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_CardType PRIMARY KEY NONCLUSTERED (CardTypeID)
)
go

CREATE TABLE bairong_Payment(
    PaymentID      int              IDENTITY(1,1),
    PaymentType    varchar(50)      DEFAULT '' NOT NULL,
    PaymentName    nvarchar(50)     DEFAULT '' NOT NULL,
    Fee            int              DEFAULT 0 NOT NULL,
    IsEnabled      varchar(18)      DEFAULT '' NOT NULL,
    IsCOD          varchar(18)      DEFAULT '' NOT NULL,
    IsPayOnline    varchar(18)      DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis          int              DEFAULT 0 NOT NULL,
    SettingsXML    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Payment PRIMARY KEY NONCLUSTERED (PaymentID)
)
go



CREATE TABLE bairong_PayRecord(
    RecordID       int               IDENTITY(1,1),
    OrderSN        varchar(50)       DEFAULT '' NOT NULL,
    UserName       nvarchar(255)     DEFAULT '' NOT NULL,
    PayTime        datetime          DEFAULT getdate() NOT NULL,
    Price          decimal(20, 2)    DEFAULT 0 NOT NULL,
    IP             varchar(50)       DEFAULT '' NOT NULL,
    SettingsXML    ntext             DEFAULT '' NOT NULL,
    ApiType        varchar(50)       DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_PayRecord PRIMARY KEY NONCLUSTERED (RecordID)
)
go

CREATE TABLE bairong_SSOApp(
    AppID             int              IDENTITY(1,1),
    AppType           varchar(50)      DEFAULT '' NOT NULL,
    AppName           nvarchar(50)     DEFAULT '' NOT NULL,
    Url               varchar(200)     DEFAULT '' NOT NULL,
    AuthKey           varchar(200)     DEFAULT '' NOT NULL,
    IPAddress         varchar(50)      DEFAULT '' NOT NULL,
    AccessFileName    varchar(50)      DEFAULT '' NOT NULL,
    IsSyncLogin       varchar(18)      DEFAULT '' NOT NULL,
    AddDate           datetime         DEFAULT getdate() NOT NULL,
    Summary           nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_SSOApp PRIMARY KEY NONCLUSTERED (AppID)
)

GO

CREATE TABLE bairong_UserAddCard(
    CardID         int              IDENTITY(1,1),
    CardCount      int              DEFAULT 0 NOT NULL,
    BuyTime        datetime         DEFAULT getdate() NULL,
    IP             varchar(50)      DEFAULT '' NOT NULL,
    SettingsXML    ntext            DEFAULT '' NOT NULL,
    UserName       nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserAddCard PRIMARY KEY NONCLUSTERED (CardID)
)
go

CREATE TABLE bairong_UserBinding(
    UserName       nvarchar(255)    NOT NULL,
    BindingType    varchar(50)      DEFAULT '' NOT NULL,
    BindingID      int              DEFAULT 0 NOT NULL,
    BindingName    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserBinding PRIMARY KEY NONCLUSTERED (UserName)
)
go

CREATE TABLE bairong_UserConsume(
    ConsumeID      int              IDENTITY(1,1),
    Consumed       varchar(50)      DEFAULT '' NOT NULL,
    ConsumeTime    datetime         DEFAULT getdate() NOT NULL,
    IP             varchar(50)      DEFAULT '' NOT NULL,
    Description    varchar(255)     DEFAULT '' NOT NULL,
    UserName       nvarchar(255)    DEFAULT '' NOT NULL,
	ConsumedCount    int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_UserConsume PRIMARY KEY NONCLUSTERED (ConsumeID)
)
go



ALTER TABLE bairong_Users ADD 
	IsTemporary         varchar(18)      DEFAULT '' NOT NULL,
	PointCount          int              DEFAULT 0 NOT NULL,
	IP                  varchar(50)      DEFAULT '' NOT NULL
GO