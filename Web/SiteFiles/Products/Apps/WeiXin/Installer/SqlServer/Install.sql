/*
 * ER/Studio 8.0 SQL Code Generation
 * Project :      SQLQuery2.DM1
 *
 * Date Created : Monday, July 28, 2014 09:55:29
 * Target DBMS : Microsoft SQL Server 2008
 */

CREATE TABLE wx_Account(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Token                  varchar(200)     DEFAULT '' NOT NULL,
    IsBinding              varchar(18)      DEFAULT '' NOT NULL,
    AccountType            varchar(50)      DEFAULT '' NOT NULL,
    WeChatID               nvarchar(255)    DEFAULT '' NOT NULL,
    SourceID               varchar(200)     DEFAULT '' NOT NULL,
    ThumbUrl               varchar(200)     DEFAULT '' NOT NULL,
    AppID                  varchar(200)     DEFAULT '' NOT NULL,
    AppSecret              varchar(200)     DEFAULT '' NOT NULL,
    IsWelcome              varchar(18)      DEFAULT '' NOT NULL,
    WelcomeKeyword         nvarchar(50)     DEFAULT '' NOT NULL,
    IsDefaultReply         varchar(18)      DEFAULT '' NOT NULL,
    DefaultReplyKeyword    nvarchar(50)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Account PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Album(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Album PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_AlbumContent(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AlbumID                int              DEFAULT 0 NOT NULL,
    ParentID               int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    LargeImageUrl          varchar(200)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_AlbumContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Appointment(
    ID                          int              IDENTITY(1,1),
    PublishmentSystemID         int              DEFAULT 0 NOT NULL,
    KeywordID                   int              DEFAULT 0 NOT NULL,
    UserCount                   int              DEFAULT 0 NOT NULL,
    PVCount                     int              DEFAULT 0 NOT NULL,
    StartDate                   datetime         DEFAULT getdate() NOT NULL,
    EndDate                     datetime         DEFAULT getdate() NOT NULL,
    IsDisabled                  varchar(18)      DEFAULT '' NOT NULL,
    Title                       nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                    varchar(200)     DEFAULT '' NOT NULL,
    Summary                     nvarchar(255)    DEFAULT '' NOT NULL,
    ContentIsSingle             varchar(18)      DEFAULT '' NOT NULL,
    ContentImageUrl             varchar(200)     DEFAULT '' NOT NULL,
    ContentDescription          nvarchar(255)    DEFAULT '' NOT NULL,
    ContentResultTopImageUrl    varchar(200)     DEFAULT '' NOT NULL,
    EndTitle                    nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl                 varchar(200)     DEFAULT '' NOT NULL,
    EndSummary                  nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Appointment PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_AppointmentContent(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AppointmentID          int              DEFAULT 0 NOT NULL,
    AppointmentItemID      int              DEFAULT 0 NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    RealName               nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile                 varchar(50)      DEFAULT '' NOT NULL,
    Email                  varchar(200)     DEFAULT '' NOT NULL,
    Status                 varchar(50)      DEFAULT '' NOT NULL,
    Message                nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_AppointmentContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_AppointmentItem(
    ID                         int              IDENTITY(1,1),
    PublishmentSystemID        int              DEFAULT 0 NOT NULL,
    AppointmentID              int              DEFAULT 0 NOT NULL,
    UserCount                  int              DEFAULT 0 NOT NULL,
    Title                      nvarchar(255)    DEFAULT '' NOT NULL,
    TopImageUrl                varchar(200)     DEFAULT '' NOT NULL,
    IsDescription              varchar(18)      DEFAULT '' NOT NULL,
    DescriptionTitle           nvarchar(255)    DEFAULT '' NOT NULL,
    Description                nvarchar(255)    DEFAULT '' NOT NULL,
    IsImageUrl                 varchar(18)      DEFAULT '' NOT NULL,
    ImageUrlTitle              nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                   varchar(200)     DEFAULT '' NOT NULL,
    IsVideoUrl                 varchar(18)      DEFAULT '' NOT NULL,
    VideoUrlTitle              nvarchar(255)    DEFAULT '' NOT NULL,
    VideoUrl                   varchar(200)     DEFAULT '' NOT NULL,
    IsImageUrlCollection       varchar(18)      DEFAULT '' NOT NULL,
    ImageUrlCollectionTitle    nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrlCollection         ntext            DEFAULT '' NOT NULL,
    LargeImageUrlCollection    ntext            DEFAULT '' NOT NULL,
    IsMap                      varchar(18)      DEFAULT '' NOT NULL,
    MapTitle                   nvarchar(255)    DEFAULT '' NOT NULL,
    MapAddress                 nvarchar(255)    DEFAULT '' NOT NULL,
    IsTel                      varchar(18)      DEFAULT '' NOT NULL,
    TelTitle                   nvarchar(255)    DEFAULT '' NOT NULL,
    Tel                        varchar(20)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_AppointmentItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Conference(
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
    BackgroundImageUrl     varchar(200)     DEFAULT '' NOT NULL,
    ConferenceName         nvarchar(255)    DEFAULT '' NOT NULL,
    Address                nvarchar(255)    DEFAULT '' NOT NULL,
    Duration               nvarchar(255)    DEFAULT '' NOT NULL,
    Introduction           ntext            DEFAULT '' NOT NULL,
    IsAgenda               varchar(18)      DEFAULT '' NOT NULL,
    AgendaTitle            nvarchar(255)    DEFAULT '' NOT NULL,
    AgendaList             ntext            DEFAULT '' NOT NULL,
    IsGuest                varchar(18)      DEFAULT '' NOT NULL,
    GuestTitle             nvarchar(255)    DEFAULT '' NOT NULL,
    GuestList              ntext            DEFAULT '' NOT NULL,
    EndTitle               nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl            varchar(200)     DEFAULT '' NOT NULL,
    EndSummary             nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Conference PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_ConferenceContent(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ConferenceID           int              DEFAULT 0 NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    RealName               nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile                 varchar(50)      DEFAULT '' NOT NULL,
    Email                  varchar(200)     DEFAULT '' NOT NULL,
    Company                nvarchar(255)    DEFAULT '' NOT NULL,
    Position               nvarchar(255)    DEFAULT '' NOT NULL,
    Note                   nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_MessageContent_1 PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Count(
    CountID                int            IDENTITY(1,1),
    PublishmentSystemID    int            DEFAULT 0 NOT NULL,
    CountYear              int            DEFAULT 0 NOT NULL,
    CountMonth             int            DEFAULT 0 NOT NULL,
    CountDay               int            DEFAULT 0 NOT NULL,
    CountType              varchar(50)    DEFAULT '' NOT NULL,
    Count                  int            DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_Count PRIMARY KEY NONCLUSTERED (CountID)
)
go



CREATE TABLE wx_Coupon(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ActID                  int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    TotalNum               int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_Coupon PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_CouponAct(
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
    ContentUsage           ntext            DEFAULT '' NOT NULL,
    ContentDescription     ntext            DEFAULT '' NOT NULL,
    IsFormRealName         varchar(18)      DEFAULT '' NOT NULL,
    FormRealNameTitle      nvarchar(255)    DEFAULT '' NOT NULL,
    IsFormMobile           varchar(18)      DEFAULT '' NOT NULL,
    FormMobileTitle        nvarchar(255)    DEFAULT '' NOT NULL,
    IsFormEmail            varchar(18)      DEFAULT '' NOT NULL,
    FormEmailTitle         nvarchar(255)    DEFAULT '' NOT NULL,
    IsFormAddress          varchar(18)      DEFAULT '' NOT NULL,
    FormAddressTitle       nvarchar(255)    DEFAULT '' NOT NULL,
    EndTitle               nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl            varchar(200)     DEFAULT '' NOT NULL,
    EndSummary             nvarchar(255)    DEFAULT '' NOT NULL,
	AwardCode              nvarchar(50)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_CouponAct PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_CouponSN(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    CouponID               int              NOT NULL,
    SN                     varchar(200)     DEFAULT '' NOT NULL,
    Status                 varchar(50)      DEFAULT '' NOT NULL,
    HoldDate               datetime         DEFAULT getdate() NOT NULL,
    HoldRealName           nvarchar(255)    DEFAULT '' NOT NULL,
    HoldMobile             varchar(200)     DEFAULT '' NOT NULL,
    HoldEmail              varchar(200)     DEFAULT '' NOT NULL,
    HoldAddress            nvarchar(255)    DEFAULT '' NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    CashDate               datetime         DEFAULT getdate() NOT NULL,
    CashUserName           nvarchar(50)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_CouponSN PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Keyword(
    KeywordID              int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Keywords               nvarchar(255)    DEFAULT '' NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    KeywordType            varchar(50)      DEFAULT '' NOT NULL,
    MatchType              varchar(50)      DEFAULT '' NOT NULL,
    Reply                  ntext            DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_Keyword PRIMARY KEY NONCLUSTERED (KeywordID)
)
go



CREATE TABLE wx_KeywordGroup(
    GroupID                int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    GroupName              nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_KeywordGroup PRIMARY KEY NONCLUSTERED (GroupID)
)
go



CREATE TABLE wx_KeywordMatch(
    MatchID                int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Keyword                nvarchar(255)    DEFAULT '' NOT NULL,
    KeywordID              int              NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    KeywordType            varchar(50)      DEFAULT '' NOT NULL,
    MatchType              varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_KeywordMatch PRIMARY KEY NONCLUSTERED (MatchID)
)
go



CREATE TABLE wx_KeywordResource(
    ResourceID             int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ResourceType           varchar(50)      DEFAULT '' NOT NULL,
    IsShowCoverPic         varchar(18)      DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    NavigationUrl          varchar(200)     DEFAULT '' NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_KeywordResource PRIMARY KEY NONCLUSTERED (ResourceID)
)
go



CREATE TABLE wx_Lottery(
    ID                      int              IDENTITY(1,1),
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    LotteryType             varchar(50)      DEFAULT '' NOT NULL,
    KeywordID               int              DEFAULT 0 NOT NULL,
    IsDisabled              varchar(18)      DEFAULT '' NOT NULL,
    UserCount               int              DEFAULT 0 NOT NULL,
    PVCount                 int              DEFAULT 0 NOT NULL,
    StartDate               datetime         DEFAULT getdate() NOT NULL,
    EndDate                 datetime         DEFAULT getdate() NOT NULL,
    Title                   nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                varchar(200)     DEFAULT '' NOT NULL,
    Summary                 nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl         varchar(200)     DEFAULT '' NOT NULL,
    ContentAwardImageUrl    varchar(200)     DEFAULT '' NOT NULL,
    ContentUsage            ntext            DEFAULT '' NOT NULL,
    AwardImageUrl           varchar(200)     DEFAULT '' NOT NULL,
    AwardUsage              ntext            DEFAULT '' NOT NULL,
    IsAwardTotalNum         varchar(10)      DEFAULT '' NOT NULL,
    AwardMaxCount           int              DEFAULT 0 NOT NULL,
    AwardMaxDailyCount      int              DEFAULT 0 NOT NULL,
    AwardCode               nvarchar(50)     DEFAULT '' NOT NULL,
    IsFormRealName          varchar(18)      DEFAULT '' NOT NULL,
    FormRealNameTitle       nvarchar(50)     DEFAULT '' NOT NULL,
    IsFormMobile            varchar(18)      DEFAULT '' NOT NULL,
    FormMobileTitle         nvarchar(50)     DEFAULT '' NOT NULL,
    IsFormEmail             varchar(18)      DEFAULT '' NOT NULL,
    FormEmailTitle          nvarchar(50)     DEFAULT '' NOT NULL,
    IsFormAddress           varchar(18)      DEFAULT '' NOT NULL,
    FormAddressTitle        nvarchar(50)     DEFAULT '' NOT NULL,
    EndTitle                nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl             varchar(200)     DEFAULT '' NOT NULL,
    EndSummary              nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Lottery PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_LotteryAward(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    LotteryID              int               DEFAULT 0 NOT NULL,
    AwardName              nvarchar(255)     DEFAULT '' NOT NULL,
    Title                  nvarchar(255)     DEFAULT '' NOT NULL,
    TotalNum               int               DEFAULT 0 NOT NULL,
    Probability            decimal(18, 2)    DEFAULT 0 NOT NULL,
    WonNum                 int               DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_LotteryAward PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_LotteryLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    LotteryID              int              NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    LotteryCount           int              DEFAULT '' NOT NULL,
    LotteryDailyCount      int              DEFAULT 0 NOT NULL,
    LastLotteryDate        datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_LotteryLog PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_LotteryWinner(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    LotteryType            varchar(50)      DEFAULT '' NOT NULL,
    LotteryID              int              DEFAULT 0 NOT NULL,
    AwardID                int              NOT NULL,
    Status                 varchar(50)      DEFAULT '' NOT NULL,
    CookieSN               varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(50)     DEFAULT '' NOT NULL,
    RealName               nvarchar(255)    DEFAULT '' NOT NULL,
    Mobile                 varchar(200)     DEFAULT '' NOT NULL,
    Email                  varchar(200)     DEFAULT '' NOT NULL,
    Address                nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CashSN                 varchar(200)     DEFAULT '' NOT NULL,
    CashDate               datetime         DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_LotteryWinner PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Map(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    MapWD                  nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Map PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Menu(
    MenuID                 int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    MenuName               nvarchar(50)    DEFAULT '' NOT NULL,
    MenuType               varchar(50)     DEFAULT '' NOT NULL,
    Keyword                nvarchar(50)    DEFAULT '' NOT NULL,
    Url                    varchar(200)    DEFAULT '' NOT NULL,
    ChannelID              int             DEFAULT 0 NOT NULL,
    ContentID              int             DEFAULT 0 NOT NULL,
    ParentID               int             DEFAULT 0 NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_Menu PRIMARY KEY NONCLUSTERED (MenuID)
)
go



CREATE TABLE wx_Message(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    UserCount              int              DEFAULT 0 NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl        varchar(200)     DEFAULT '' NOT NULL,
    ContentDescription     ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Message PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_MessageContent(
    ID                        int              IDENTITY(1,1),
    PublishmentSystemID       int              DEFAULT 0 NOT NULL,
    MessageID                 int              DEFAULT 0 NOT NULL,
    IPAddress                 varchar(50)      DEFAULT '' NOT NULL,
    CookieSN                  varchar(50)      DEFAULT '' NOT NULL,
    WXOpenID                  varchar(200)     DEFAULT '' NOT NULL,
    UserName                  nvarchar(255)    DEFAULT '' NOT NULL,
    ReplyCount                int              DEFAULT 0 NOT NULL,
    LikeCount                 int              DEFAULT 0 NOT NULL,
    LikeCookieSNCollection    ntext            DEFAULT '' NOT NULL,
    IsReply                   varchar(18)      DEFAULT '' NOT NULL,
    ReplyID                   int              DEFAULT 0 NOT NULL,
    DisplayName               nvarchar(50)     DEFAULT '' NOT NULL,
    Color                     varchar(50)      DEFAULT '' NOT NULL,
    Content                   nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                   datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_MessageContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Search(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl        varchar(200)     DEFAULT '' NOT NULL,
    IsOutsiteSearch        varchar(18)      DEFAULT '' NOT NULL,
    IsNavigation           varchar(18)      DEFAULT '' NOT NULL,
    NavTitleColor          varchar(50)      DEFAULT '' NOT NULL,
    NavImageColor          varchar(50)      DEFAULT '' NOT NULL,
    ImageAreaTitle         nvarchar(50)     DEFAULT '' NOT NULL,
    ImageAreaChannelID     int              DEFAULT 0 NOT NULL,
    TextAreaTitle          nvarchar(50)     DEFAULT '' NOT NULL,
    TextAreaChannelID      int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_Search PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_SearchNavigation(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    SearchID               int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Url                    varchar(200)     DEFAULT '' NOT NULL,
    ImageCssClass          varchar(200)     DEFAULT '' NOT NULL,
    NavigationType         varchar(50)      DEFAULT '' NOT NULL,
    KeywordType            varchar(50)      DEFAULT '' NOT NULL,
    FunctionID             int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_SearchNavigation PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Store(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Store PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_StoreCategory(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NULL,
    CategoryName           nvarchar(50)    DEFAULT '' NOT NULL,
    ParentID               int             DEFAULT 0 NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    ChildCount             int             DEFAULT 0 NOT NULL,
    ParentsCount           int             DEFAULT 0 NOT NULL,
    ParentsPath            varchar(100)    DEFAULT '' NOT NULL,
    StoreCount             int             DEFAULT 0 NOT NULL,
    IsLastNode             varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_StoreCategory PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_StoreItem(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    StoreID                int              DEFAULT 0 NOT NULL,
    CategoryID             int              DEFAULT 0 NOT NULL,
    StoreName              nvarchar(255)    DEFAULT '' NOT NULL,
    Tel                    varchar(50)      DEFAULT '' NOT NULL,
    Mobile                 nvarchar(11)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Address                nvarchar(255)    DEFAULT '' NOT NULL,
    Longitude              varchar(100)     DEFAULT '' NOT NULL,
    Latitude               varchar(100)     DEFAULT '' NOT NULL,
    Summary                ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_StoreItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_View360(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl1       varchar(200)     DEFAULT '' NOT NULL,
    ContentImageUrl2       varchar(200)     DEFAULT '' NOT NULL,
    ContentImageUrl3       varchar(200)     DEFAULT '' NOT NULL,
    ContentImageUrl4       varchar(200)     DEFAULT '' NOT NULL,
    ContentImageUrl5       varchar(200)     DEFAULT '' NOT NULL,
    ContentImageUrl6       varchar(200)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_View360 PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Vote(
    ID                      int              IDENTITY(1,1),
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    KeywordID               int              DEFAULT 0 NOT NULL,
    IsDisabled              varchar(18)      DEFAULT '' NOT NULL,
    UserCount               int              DEFAULT 0 NOT NULL,
    PVCount                 int              DEFAULT 0 NOT NULL,
    StartDate               datetime         DEFAULT getdate() NOT NULL,
    EndDate                 datetime         DEFAULT getdate() NOT NULL,
    Title                   nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl                varchar(200)     DEFAULT '' NOT NULL,
    Summary                 nvarchar(255)    DEFAULT '' NOT NULL,
    ContentImageUrl         varchar(200)     DEFAULT '' NOT NULL,
    ContentDescription      ntext            DEFAULT '' NOT NULL,
    ContentIsImageOption    varchar(18)      DEFAULT '' NOT NULL,
    ContentIsCheckBox       varchar(18)      DEFAULT '' NOT NULL,
    ContentResultVisible    varchar(50)      DEFAULT '' NOT NULL,
    EndTitle                nvarchar(255)    DEFAULT '' NOT NULL,
    EndImageUrl             varchar(200)     DEFAULT '' NOT NULL,
    EndSummary              nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Vote PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_VoteItem(
    ID                     int              IDENTITY(1,1),
    VoteID                 int              NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    NavigationUrl          varchar(200)     DEFAULT '' NOT NULL,
    VoteNum                int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_VoteItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_VoteLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    VoteID                 int              DEFAULT 0 NOT NULL,
    ItemIDCollection       varchar(200)     DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    CookieSN               varchar(10)      DEFAULT '' NOT NULL,
    WXOpenID               varchar(200)     DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_VoteLog PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_WebMenu(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    MenuName               nvarchar(50)    DEFAULT '' NOT NULL,
    IconUrl                varchar(200)    DEFAULT '' NOT NULL,
    IconCssClass           varchar(50)     DEFAULT '' NOT NULL,
    NavigationType         varchar(50)     DEFAULT '' NOT NULL,
    Url                    varchar(200)    DEFAULT '' NOT NULL,
    ChannelID              int             DEFAULT 0 NOT NULL,
    ContentID              int             DEFAULT 0 NOT NULL,
    KeywordType            varchar(50)     DEFAULT '' NOT NULL,
    FunctionID             int             DEFAULT 0 NOT NULL,
    ParentID               int             DEFAULT 0 NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wx_WebMenu PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_Wifi(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    KeywordID              int              DEFAULT 0 NOT NULL,
    PVCount                int              DEFAULT 0 NOT NULL,
    IsDisabled             varchar(18)      DEFAULT '' NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    BusinessID             varchar(100)     DEFAULT '' NOT NULL,
    CallBackString         ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_Wifi PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wx_WifiNode(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    BusinessID             varchar(100)    DEFAULT '' NOT NULL,
    NodeID                 varchar(100)    DEFAULT '' NOT NULL,
    CallBackString         ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_WifiNode PRIMARY KEY NONCLUSTERED (ID)
)
go


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

CREATE TABLE wx_CardCashLog(
    ID                     int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)     DEFAULT '' NOT NULL,
    CardID                 int               DEFAULT 0 NOT NULL,
    CardSNID               int               DEFAULT 0 NOT NULL,
    CashType               nvarchar(50)      DEFAULT '' NOT NULL,
    Amount                 decimal(20, 2)    DEFAULT 0 NOT NULL,
    CurAmount              decimal(20, 2)    DEFAULT 0 NOT NULL,
    ConsumeType            nvarchar(50)      DEFAULT '' NOT NULL,
    Operator               nvarchar(255)     DEFAULT '' NOT NULL,
    Description            nvarchar(255)     DEFAULT '' NOT NULL,
    AddDate                datetime          DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_CardCashLog PRIMARY KEY NONCLUSTERED (ID)
)
go

CREATE TABLE wx_ConfigExtend(
    ID                     int            IDENTITY(1,1),
    PublishmentSystemID    int            DEFAULT 0 NOT NULL,
    KeywordType            varchar(50)    DEFAULT '' NOT NULL,
    FunctionID             int            DEFAULT 0 NOT NULL,
    AttributeName          varchar(50)    DEFAULT '' NOT NULL,
    IsVisible              varchar(18)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wx_ConfigExtend PRIMARY KEY NONCLUSTERED (ID)
)
go


CREATE TABLE wx_CardSignLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    SignDate               datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wx_CardSignLog PRIMARY KEY NONCLUSTERED (ID)
)
go






CREATE INDEX IX_wx_KeywordMatch_K ON wx_KeywordMatch(Keyword)
go
ALTER TABLE wx_CouponSN ADD CONSTRAINT FK_wx_Coupon_SN 
    FOREIGN KEY (CouponID)
    REFERENCES wx_Coupon(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wx_KeywordMatch ADD CONSTRAINT FK_wx_Keyword_KeywordMatch 
    FOREIGN KEY (KeywordID)
    REFERENCES wx_Keyword(KeywordID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wx_KeywordResource ADD CONSTRAINT FK_wx_Keyword_KeywordResource 
    FOREIGN KEY (KeywordID)
    REFERENCES wx_Keyword(KeywordID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wx_LotteryLog ADD CONSTRAINT FK_wx_Lottery_Log 
    FOREIGN KEY (LotteryID)
    REFERENCES wx_Lottery(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wx_LotteryWinner ADD CONSTRAINT FK_wx_LotteryAward_Winner 
    FOREIGN KEY (AwardID)
    REFERENCES wx_LotteryAward(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


