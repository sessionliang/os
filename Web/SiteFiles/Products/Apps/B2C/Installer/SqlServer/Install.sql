/*
 * ER/Studio 8.0 SQL Code Generation
 * Project :      SQLQuery2.DM1
 *
 * Date Created : Friday, August 08, 2014 09:25:16
 * Target DBMS : Microsoft SQL Server 2008
 */

CREATE TABLE b2c_Brand(
    BrandID                int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    BrandName              nvarchar(50)    DEFAULT '' NOT NULL,
    BrandUrl               varchar(200)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    Content                ntext           DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_Brand PRIMARY KEY NONCLUSTERED (BrandID)
)
go



CREATE TABLE b2c_BrandContent(
    ID                            int              IDENTITY(1,1),
    NodeID                        int              DEFAULT 0 NOT NULL,
    PublishmentSystemID           int              DEFAULT 0 NOT NULL,
    AddUserName                   nvarchar(255)    DEFAULT '' NOT NULL,
    LastEditUserName              nvarchar(255)    DEFAULT '' NOT NULL,
    LastEditDate                  datetime         DEFAULT getdate() NOT NULL,
    Taxis                         int              DEFAULT 0 NOT NULL,
    ContentGroupNameCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    Tags                          nvarchar(255)    DEFAULT '' NOT NULL,
    SourceID                      int              DEFAULT 0 NOT NULL,
    ReferenceID                   int              DEFAULT 0 NOT NULL,
    IsChecked                     varchar(18)      DEFAULT '' NOT NULL,
    CheckedLevel                  int              DEFAULT 0 NOT NULL,
    Comments                      int              DEFAULT 0 NOT NULL,
    Hits                          int              DEFAULT 0 NOT NULL,
    HitsByDay                     int              DEFAULT 0 NOT NULL,
    HitsByWeek                    int              DEFAULT 0 NOT NULL,
    HitsByMonth                   int              DEFAULT 0 NOT NULL,
    LastHitsDate                  datetime         DEFAULT getdate() NOT NULL,
    SettingsXML                   ntext            DEFAULT '' NOT NULL,
    GoodsCount                    int              DEFAULT 0 NOT NULL,
    Title                         nvarchar(255)    DEFAULT '' NOT NULL,
    BrandUrl                      nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                      varchar(200)     DEFAULT '' NOT NULL,
    LinkUrl                       nvarchar(200)    DEFAULT '' NOT NULL,
    Content                       ntext            DEFAULT '' NOT NULL,
    IsRecommend                   varchar(18)      DEFAULT '' NOT NULL,
    IsTop                         varchar(18)      DEFAULT '' NOT NULL,
    AddDate                       datetime         DEFAULT getdate() NOT NULL,
    F1                            nvarchar(255)    DEFAULT '' NOT NULL,
    F2                            nvarchar(255)    DEFAULT '' NOT NULL,
    F3                            nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_BrandContent PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE b2c_Cart(
    CartID                 int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    SessionID              nvarchar(255)    DEFAULT '' NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    GoodsID                int              DEFAULT 0 NOT NULL,
    PurchaseNum            int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_b2c_Cart PRIMARY KEY NONCLUSTERED (CartID)
)
go



CREATE TABLE b2c_Configuration(
    NodeID            int            NOT NULL,
    IsVirtualGoods    varchar(18)    DEFAULT '' NOT NULL,
    SettingsXML       ntext          DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Configuration PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE b2c_Consignee(
    ID           int              IDENTITY(1,1),
    GroupSN      nvarchar(255)    DEFAULT '' NOT NULL,
    UserName     nvarchar(255)    DEFAULT '' NOT NULL,
    IsOrder      varchar(18)      DEFAULT '' NOT NULL,
    IPAddress    varchar(50)      DEFAULT '' NOT NULL,
    IsDefault    varchar(18)      DEFAULT '' NOT NULL,
    Consignee    nvarchar(50)     DEFAULT '' NOT NULL,
    Country      nvarchar(50)     DEFAULT '' NOT NULL,
    Province     nvarchar(50)     DEFAULT '' NOT NULL,
    City         nvarchar(50)     DEFAULT '' NOT NULL,
    Area         nvarchar(50)     DEFAULT '' NOT NULL,
    Address      nvarchar(100)    DEFAULT '' NOT NULL,
    Zipcode      varchar(20)      DEFAULT '' NOT NULL,
    Mobile       varchar(50)      DEFAULT '' NOT NULL,
    Tel          varchar(50)      DEFAULT '' NOT NULL,
    Email        varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Consignee PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Filter(
    FilterID               int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    NodeID                 int             DEFAULT 0 NOT NULL,
    AttributeName          varchar(200)    DEFAULT '' NOT NULL,
    FilterName             nvarchar(50)    DEFAULT '' NOT NULL,
    IsDefaultValues        varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_Filter PRIMARY KEY CLUSTERED (FilterID)
)
go



CREATE TABLE b2c_FilterItem(
    ItemID      int              IDENTITY(1,1),
    FilterID    int              NOT NULL,
    Title       nvarchar(255)    DEFAULT '' NOT NULL,
    Value       nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis       int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_FilterItem PRIMARY KEY NONCLUSTERED (ItemID)
)
go



CREATE TABLE b2c_Follow(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_b2c_Follow PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Goods(
    GoodsID                 int               IDENTITY(1,1),
    PublishmentSystemID     int               DEFAULT 0 NOT NULL,
    ContentID               int               DEFAULT 0 NOT NULL,
    ComboIDCollection       varchar(200)      DEFAULT '' NOT NULL,
    SpecIDCollection        varchar(200)      DEFAULT '' NOT NULL,
    SpecItemIDCollection    varchar(200)      DEFAULT '' NOT NULL,
    GoodsSN                 varchar(50)       DEFAULT '' NOT NULL,
    Stock                   int               DEFAULT 0 NOT NULL,
    PriceMarket             decimal(18, 2)    DEFAULT 0 NOT NULL,
    PriceSale               decimal(18, 2)    DEFAULT 0 NOT NULL,
    IsOnSale                varchar(18)       DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Goods PRIMARY KEY NONCLUSTERED (GoodsID)
)
go



CREATE TABLE b2c_Invoice(
    ID                  int              IDENTITY(1,1),
    GroupSN             nvarchar(255)    DEFAULT '' NOT NULL,
    UserName            nvarchar(255)    DEFAULT '' NOT NULL,
    IsOrder             varchar(18)      DEFAULT '' NOT NULL,
    IsDefault           varchar(18)      DEFAULT '' NOT NULL,
    IsVat               varchar(18)      DEFAULT '' NOT NULL,
    IsCompany           varchar(18)      DEFAULT '' NOT NULL,
    CompanyName         nvarchar(255)    DEFAULT '' NOT NULL,
    VatCompanyName      nvarchar(255)    DEFAULT '' NOT NULL,
    VatCode             nvarchar(255)    DEFAULT '' NOT NULL,
    VatAddress          nvarchar(255)    DEFAULT '' NOT NULL,
    VatPhone            nvarchar(255)    DEFAULT '' NOT NULL,
    VatBankName         nvarchar(255)    DEFAULT '' NOT NULL,
    VatBankAccount      nvarchar(255)    DEFAULT '' NOT NULL,
    ConsigneeName       nvarchar(255)    DEFAULT '' NOT NULL,
    ConsigneeMobile     nvarchar(255)    DEFAULT '' NOT NULL,
    ConsigneeAddress    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Invoice PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Location(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    LocationName           nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID               int              DEFAULT 0 NOT NULL,
    ParentsPath            nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount           int              DEFAULT 0 NOT NULL,
    ChildrenCount          int              DEFAULT 0 NOT NULL,
    IsLastNode             varchar(18)      DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_Location PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Order(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    OrderSN                varchar(50)       DEFAULT '' NOT NULL,
    UserName               nvarchar(255)     DEFAULT '' NOT NULL,
    IPAddress              varchar(50)       DEFAULT '' NOT NULL,
    OrderStatus            varchar(50)       DEFAULT '' NOT NULL,
    PaymentStatus          varchar(50)       DEFAULT '' NOT NULL,
    ShipmentStatus         varchar(50)       DEFAULT '' NOT NULL,
    TimeOrder              datetime          DEFAULT getdate() NOT NULL,
    TimePayment            datetime          DEFAULT getdate() NOT NULL,
    TimeShipment           datetime          DEFAULT getdate() NOT NULL,
    ConsigneeID            int               DEFAULT 0 NOT NULL,
    PaymentID              int               DEFAULT 0 NOT NULL,
    ShipmentID             int               DEFAULT 0 NOT NULL,
    InvoiceID              int               DEFAULT 0 NOT NULL,
    PriceTotal             decimal(20, 2)    DEFAULT 0 NOT NULL,
    PriceShipment          decimal(20, 2)    DEFAULT 0 NOT NULL,
    PriceReturn            decimal(20, 2)    DEFAULT 0 NOT NULL,
    PriceActual            decimal(20, 2)    DEFAULT 0 NOT NULL,
    Summary                nvarchar(255)     DEFAULT '' NOT NULL,
	Extended                ntext             DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Order PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_OrderItem(
    ID             int               IDENTITY(1,1),
    OrderID        int               NOT NULL,
    ChannelID      int               DEFAULT 0 NOT NULL,
    ContentID      int               DEFAULT 0 NOT NULL,
    GoodsID        int               DEFAULT 0 NOT NULL,
    GoodsSN        varchar(50)       DEFAULT '' NOT NULL,
    Title          nvarchar(255)     DEFAULT '' NOT NULL,
    ThumbUrl       varchar(200)      DEFAULT '' NOT NULL,
    PriceSale      decimal(20, 2)    DEFAULT 0 NOT NULL,
    PurchaseNum    int               DEFAULT 0 NOT NULL,
    IsShipment     varchar(18)       DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_OrderItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Payment(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    PaymentType            varchar(50)     DEFAULT '' NOT NULL,
    PaymentName            nvarchar(50)    DEFAULT '' NOT NULL,
    IsEnabled              varchar(18)     DEFAULT '' NOT NULL,
    IsOnline               varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    Description            ntext           DEFAULT '' NOT NULL,
    SettingsXML            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Payment PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Photo(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    ContentID              int             DEFAULT 0 NOT NULL,
    SmallUrl               varchar(200)    DEFAULT '' NOT NULL,
    MiddleUrl              varchar(200)    DEFAULT '' NOT NULL,
    LargeUrl               varchar(200)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    Description            varchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Photo PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Promotion(
    ID                            int               IDENTITY(1,1),
    PublishmentSystemID           int               DEFAULT 0 NOT NULL,
    PromotionName                 nvarchar(50)      DEFAULT '' NOT NULL,
    StartDate                     datetime          DEFAULT getdate() NOT NULL,
    EndDate                       datetime          DEFAULT getdate() NOT NULL,
    Tags                          nvarchar(50)      DEFAULT '' NOT NULL,
    Target                        varchar(50)       DEFAULT '' NOT NULL,
    ChannelIDCollection           nvarchar(255)     DEFAULT '' NOT NULL,
    IDsCollection                 nvarchar(255)     DEFAULT '' NOT NULL,
    ExcludeChannelIDCollection    nvarchar(255)     DEFAULT '' NOT NULL,
    ExcludeIDsCollection          nvarchar(255)     DEFAULT '' NOT NULL,
    IfAmount                      decimal(18, 2)    DEFAULT 0 NOT NULL,
    IfCount                       int               DEFAULT 0 NOT NULL,
    Discount                      decimal(18, 2)    DEFAULT 0 NOT NULL,
    ReturnAmount                  decimal(18, 2)    DEFAULT 0 NOT NULL,
    IsReturnMultiply              varchar(18)       DEFAULT '' NOT NULL,
    IsShipmentFree                varchar(18)       DEFAULT '' NOT NULL,
    IsGift                        varchar(18)       DEFAULT '' NOT NULL,
    GiftName                      nvarchar(50)      DEFAULT '' NOT NULL,
    GiftUrl                       varchar(200)      DEFAULT '' NOT NULL,
    IsEnabled                     varchar(18)       DEFAULT '' NOT NULL,
    Taxis                         int               DEFAULT 0 NOT NULL,
    AddDate                       datetime          DEFAULT getdate() NOT NULL,
    Description                   nvarchar(255)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Promotion PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Request(
    ID                 int              IDENTITY(1,1),
    SN                 varchar(50)      DEFAULT '' NOT NULL,
    Status             varchar(50)      DEFAULT '' NOT NULL,
    UserName           nvarchar(255)    DEFAULT '' NOT NULL,
    AdminUserName      nvarchar(50)     DEFAULT '' NOT NULL,
    AddDate            datetime         DEFAULT getdate() NOT NULL,
    RequestType        nvarchar(50)     DEFAULT '' NOT NULL,
    Subject            nvarchar(255)    DEFAULT '' NOT NULL,
    Website            varchar(200)     DEFAULT '' NOT NULL,
    Email              varchar(200)     DEFAULT '' NOT NULL,
    Mobile             varchar(50)      DEFAULT '' NOT NULL,
    QQ                 varchar(50)      DEFAULT '' NOT NULL,
    IsEstimate         varchar(18)      DEFAULT '' NOT NULL,
    EstimateValue      varchar(50)      DEFAULT '' NOT NULL,
    EstimateComment    nvarchar(255)    DEFAULT '' NOT NULL,
    EstimateDate       datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_b2c_Request PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_RequestAnswer(
    ID           int             IDENTITY(1,1),
    RequestID    int             NOT NULL,
    IsAnswer     varchar(18)     DEFAULT '' NOT NULL,
    Content      ntext           DEFAULT '' NOT NULL,
    FileUrl      varchar(200)    DEFAULT '' NOT NULL,
    AddDate      datetime        DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_b2c_RequestAnswer PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Shipment(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    ShipmentName           nvarchar(50)    DEFAULT '' NOT NULL,
    ShipmentPeriod         varchar(50)     DEFAULT '' NOT NULL,
    IsEnabled              varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    Description            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Shipment PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE b2c_Spec(
    SpecID                 int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    SpecName               nvarchar(50)     DEFAULT '' NOT NULL,
    IsIcon                 varchar(18)      DEFAULT '' NOT NULL,
    IsMultiple             varchar(18)      DEFAULT '' NOT NULL,
    IsRequired             varchar(18)      DEFAULT '' NOT NULL,
    Description            nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_Spec PRIMARY KEY NONCLUSTERED (SpecID)
)
go



CREATE TABLE b2c_SpecCombo(
    ComboID                int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    ContentID              int             DEFAULT 0 NOT NULL,
    SpecID                 int             DEFAULT 0 NOT NULL,
    ItemID                 int             DEFAULT 0 NOT NULL,
    Title                  nvarchar(50)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    PhotoIDCollection      varchar(200)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_SpecCombo PRIMARY KEY NONCLUSTERED (ComboID)
)
go



CREATE TABLE b2c_SpecItem(
    ItemID                 int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    SpecID                 int             DEFAULT 0 NOT NULL,
    Title                  nvarchar(50)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    IsDefault              varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_b2c_SpecItem PRIMARY KEY NONCLUSTERED (ItemID)
)
go



CREATE TABLE b2c_UserSetting(
    ID           int              IDENTITY(1,1),
    UserName     nvarchar(50)     DEFAULT '' NOT NULL,
    SessionID    nvarchar(255)    DEFAULT '' NOT NULL,
    Province     nvarchar(50)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_UserSetting PRIMARY KEY NONCLUSTERED (ID)
)
go

--¶©µ¥ÆÀ¼Û
CREATE TABLE b2c_OrderItemComment(
    ID             int              IDENTITY(1,1),
    OrderItemID    int              NOT NULL,
    Star           int              DEFAULT 0 NOT NULL,
    Tags           nvarchar(200)    DEFAULT '' NOT NULL,
    Comment        nvarchar(200)    DEFAULT '' NOT NULL,
    IsAnonymous    varchar(18)      DEFAULT '' NOT NULL,
    OrderUrl       nvarchar(500)    DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    AddUser        nvarchar(50)     DEFAULT '' NOT NULL,
    GoodCount      int              DEFAULT 0 NOT NULL,
    ImageUrl       nvarchar(500)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_OrderItemComment PRIMARY KEY NONCLUSTERED (ID)
)
go


--¹ºÂò×ÉÑ¯
CREATE TABLE b2c_Consultation(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(200)    DEFAULT '' NOT NULL,
    ThumbUrl               nvarchar(200)    DEFAULT '' NOT NULL,
    Question               ntext            DEFAULT '' NOT NULL,
    Answer                 ntext            DEFAULT '' NOT NULL,
    Type                   nvarchar(50)     DEFAULT '' NOT NULL,
    AddUser                nvarchar(50)     DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    ReplyDate              datetime         DEFAULT getdate() NOT NULL,
    IsReply                varchar(18)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_b2c_Consultation PRIMARY KEY NONCLUSTERED (ID)
)
go

--ä¯ÀÀÀúÊ·
CREATE TABLE b2c_History(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_b2c_History PRIMARY KEY NONCLUSTERED (ID)
)
go

--¶©µ¥ÍË»õ
CREATE TABLE b2c_OrderItemReturn(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    OrderItemID            int               DEFAULT 0 NOT NULL,
    Status                 varchar(50)       DEFAULT '' NOT NULL,
    Title                  nvarchar(100)     DEFAULT '' NOT NULL,
    GoodsSN                nvarchar(100)     DEFAULT '' NOT NULL,
    ApplyDate              datetime          DEFAULT getdate() NOT NULL,
    ApplyUser              nvarchar(100)     DEFAULT '' NOT NULL,
    AuditStatus            varchar(50)       DEFAULT '' NOT NULL,
    AuditUser              nvarchar(100)     DEFAULT '' NOT NULL,
    AuditDate              datetime          DEFAULT getdate() NOT NULL,
    ReturnOrderStatus      varchar(50)       DEFAULT '' NOT NULL,
    ReturnOrderUser        nvarchar(100)     DEFAULT '' NOT NULL,
    ReturnOrderDate        datetime          DEFAULT getdate() NOT NULL,
    ReturnMoneyStatus      varchar(50)       DEFAULT '' NOT NULL,
    ReturnMoneyUser        nvarchar(100)     DEFAULT '' NOT NULL,
    ReturnMoneyDate        datetime          DEFAULT getdate() NOT NULL,
    Type                   varchar(50)       DEFAULT '' NOT NULL,
    Description            ntext             DEFAULT '' NOT NULL,
    ImageUrl               nvarchar(1000)    DEFAULT '' NOT NULL,
    ReturnCount            int               DEFAULT 0 NOT NULL,
    InspectReport          varchar(18)       DEFAULT '' NOT NULL,
    ReturnMode             varchar(50)       DEFAULT '' NOT NULL,
    Contact                nvarchar(100)     DEFAULT '' NOT NULL,
    ContactPhone           varchar(50)       DEFAULT '' NOT NULL,
    SettingXml             ntext             DEFAULT '' NOT NULL,
    CONSTRAINT PK77 PRIMARY KEY NONCLUSTERED (ID)
)
go





ALTER TABLE b2c_FilterItem ADD CONSTRAINT FK_b2c_Filter_FilterItem 
    FOREIGN KEY (FilterID)
    REFERENCES b2c_Filter(FilterID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE b2c_OrderItem ADD CONSTRAINT FK_b2c_Order_OrderItem 
    FOREIGN KEY (OrderID)
    REFERENCES b2c_Order(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE b2c_RequestAnswer ADD CONSTRAINT FK_b2c_Request_Answer 
    FOREIGN KEY (RequestID)
    REFERENCES b2c_Request(ID) ON DELETE CASCADE ON UPDATE CASCADE
go

ALTER TABLE b2c_OrderItemComment ADD CONSTRAINT FK_b2c_OrderItem_OrderItemComment 
    FOREIGN KEY (OrderItemID)
    REFERENCES b2c_OrderItem(ID)
go


