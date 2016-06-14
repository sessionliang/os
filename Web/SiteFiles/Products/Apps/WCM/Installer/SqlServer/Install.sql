/*
 * ER/Studio 8.0 SQL Code Generation
 * Company :      BaiRong Software
 * Project :      SiteServer WCM
 * Author :       BaiRong Software
 *
 * Date Created : Wednesday, September 11, 2013 14:32:03
 * Target DBMS : Microsoft SQL Server 2000
 */

CREATE TABLE wcm_Ad(
    AdName                 nvarchar(50)     NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AdType                 varchar(50)      DEFAULT '' NOT NULL,
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
    IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
    IsDateLimited          varchar(18)      DEFAULT '' NOT NULL,
    StartDate              datetime         DEFAULT getdate() NOT NULL,
    EndDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_Ad PRIMARY KEY NONCLUSTERED (AdName, PublishmentSystemID)
)
go



CREATE TABLE wcm_Advertisement(
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
    CONSTRAINT PK_wcm_Advertisement PRIMARY KEY CLUSTERED (AdvertisementName, PublishmentSystemID)
)
go



CREATE TABLE wcm_Comment(
    CommentID              int              IDENTITY(1,1),
    CommentName            nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ApplyStyleID           int              DEFAULT 0 NOT NULL,
    QueryStyleID           int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Comment PRIMARY KEY NONCLUSTERED (CommentID)
)
go



CREATE TABLE wcm_CommentContent(
    ID                     int              IDENTITY(1,1),
    CommentID              int              DEFAULT 0 NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    ReferenceID            int              DEFAULT 0 NOT NULL,
    Good                   int              DEFAULT 0 NOT NULL,
    Bad                    int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    Location               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    Attribute1             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute2             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute3             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute4             nvarchar(255)    DEFAULT '' NOT NULL,
    Attribute5             nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_CommentContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_Configuration(
    SettingsXML    ntext    DEFAULT '' NOT NULL
)
go



CREATE TABLE wcm_ContentGroup(
    ContentGroupName       nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Description            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_ContentGroup PRIMARY KEY CLUSTERED (ContentGroupName, PublishmentSystemID)
)
go



CREATE TABLE wcm_GatherDatabaseRule(
    GatherRuleName         nvarchar(50)     NOT NULL,
    PublishmentSystemID    int              NOT NULL,
    DatabaseType           varchar(50)      DEFAULT '' NOT NULL,
    ConnectionString       varchar(255)     DEFAULT '' NOT NULL,
    RelatedTableName       varchar(255)     DEFAULT '' NOT NULL,
    RelatedIdentity        varchar(255)     DEFAULT '' NOT NULL,
    RelatedOrderBy         varchar(255)     DEFAULT '' NOT NULL,
    WhereString            nvarchar(255)    DEFAULT '' NOT NULL,
    TableMatchID           int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    GatherNum              int              DEFAULT 0 NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    IsOrderByDesc          varchar(18)      DEFAULT '' NOT NULL,
    LastGatherDate         datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_GatherDR PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE wcm_GatherFileRule(
    GatherRuleName                   nvarchar(50)     NOT NULL,
    PublishmentSystemID              int              NOT NULL,
    GatherUrl                        nvarchar(255)    DEFAULT '' NOT NULL,
    Charset                          varchar(50)      DEFAULT '' NOT NULL,
    LastGatherDate                   datetime         DEFAULT getdate() NOT NULL,
    IsToFile                         varchar(18)      DEFAULT '' NOT NULL,
    FilePath                         nvarchar(255)    DEFAULT '' NOT NULL,
    IsSaveRelatedFiles               varchar(18)      DEFAULT '' NOT NULL,
    IsRemoveScripts                  varchar(18)      DEFAULT '' NOT NULL,
    StyleDirectoryPath               nvarchar(255)    DEFAULT '' NOT NULL,
    ScriptDirectoryPath              nvarchar(255)    DEFAULT '' NOT NULL,
    ImageDirectoryPath               nvarchar(255)    DEFAULT '' NOT NULL,
    NodeID                           int              DEFAULT 0 NOT NULL,
    IsSaveImage                      varchar(18)      DEFAULT '' NOT NULL,
    IsChecked                        varchar(18)      DEFAULT '' NOT NULL,
    ContentExclude                   ntext            DEFAULT '' NOT NULL,
    ContentHtmlClearCollection       nvarchar(255)    DEFAULT '' NOT NULL,
    ContentHtmlClearTagCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    ContentTitleStart                ntext            DEFAULT '' NOT NULL,
    ContentTitleEnd                  ntext            DEFAULT '' NOT NULL,
    ContentContentStart              ntext            DEFAULT '' NOT NULL,
    ContentContentEnd                ntext            DEFAULT '' NOT NULL,
    ContentAttributes                ntext            DEFAULT '' NOT NULL,
    ContentAttributesXML             ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GatherFileRule PRIMARY KEY NONCLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE wcm_GatherRule(
    GatherRuleName                   nvarchar(50)     NOT NULL,
    PublishmentSystemID              int              NOT NULL,
    CookieString                     text             DEFAULT '' NOT NULL,
    GatherUrlIsCollection            varchar(18)      DEFAULT '' NOT NULL,
    GatherUrlCollection              text             DEFAULT '' NOT NULL,
    GatherUrlIsSerialize             varchar(18)      DEFAULT '' NOT NULL,
    GatherUrlSerialize               varchar(200)     DEFAULT '' NOT NULL,
    SerializeFrom                    int              DEFAULT 0 NOT NULL,
    SerializeTo                      int              DEFAULT 0 NOT NULL,
    SerializeInterval                int              DEFAULT 0 NOT NULL,
    SerializeIsOrderByDesc           varchar(18)      DEFAULT '' NOT NULL,
    SerializeIsAddZero               varchar(18)      DEFAULT '' NOT NULL,
    NodeID                           int              DEFAULT 0 NOT NULL,
    Charset                          varchar(50)      DEFAULT '' NOT NULL,
    UrlInclude                       varchar(200)     DEFAULT '' NOT NULL,
    TitleInclude                     nvarchar(255)    DEFAULT '' NOT NULL,
    ContentExclude                   ntext            DEFAULT '' NOT NULL,
    ContentHtmlClearCollection       nvarchar(255)    DEFAULT '' NOT NULL,
    ContentHtmlClearTagCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    LastGatherDate                   datetime         DEFAULT getdate() NOT NULL,
    ListAreaStart                    ntext            DEFAULT '' NOT NULL,
    ListAreaEnd                      ntext            DEFAULT '' NOT NULL,
    ContentChannelStart              ntext            DEFAULT '' NOT NULL,
    ContentChannelEnd                ntext            DEFAULT '' NOT NULL,
    ContentTitleStart                ntext            DEFAULT '' NOT NULL,
    ContentTitleEnd                  ntext            DEFAULT '' NOT NULL,
    ContentContentStart              ntext            DEFAULT '' NOT NULL,
    ContentContentEnd                ntext            DEFAULT '' NOT NULL,
    ContentNextPageStart             ntext            DEFAULT '' NOT NULL,
    ContentNextPageEnd               ntext            DEFAULT '' NOT NULL,
    ContentAttributes                ntext            DEFAULT '' NOT NULL,
    ContentAttributesXML             ntext            DEFAULT '' NOT NULL,
    ExtendValues                     ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GatherRule PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE wcm_GovInteractChannel(
    NodeID                    int              NOT NULL,
    PublishmentSystemID       int              DEFAULT 0 NOT NULL,
    ApplyStyleID              int              DEFAULT 0 NOT NULL,
    QueryStyleID              int              DEFAULT 0 NOT NULL,
    DepartmentIDCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    Summary                   nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovInteractChannel PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE wcm_GovInteractLog(
    LogID                  int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              NOT NULL,
    ContentID              int              NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    LogType                varchar(50)      DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovInteractLog PRIMARY KEY NONCLUSTERED (LogID)
)
go



CREATE TABLE wcm_GovInteractPermissions(
    UserName       nvarchar(50)     NOT NULL,
    NodeID         int              NOT NULL,
    Permissions    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovInteractAdministrator PRIMARY KEY NONCLUSTERED (UserName, NodeID)
)
go



CREATE TABLE wcm_GovInteractRemark(
    RemarkID               int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              NOT NULL,
    ContentID              int              NULL,
    RemarkType             varchar(50)      DEFAULT '' NOT NULL,
    Remark                 nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_GovInteractRemark PRIMARY KEY NONCLUSTERED (RemarkID)
)
go



CREATE TABLE wcm_GovInteractReply(
    ReplyID                int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              NOT NULL,
    ContentID              int              NULL,
    Reply                  ntext            NOT NULL,
    FileUrl                nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_GovInteractReply PRIMARY KEY NONCLUSTERED (ReplyID)
)
go



CREATE TABLE wcm_GovInteractType(
    TypeID                 int             IDENTITY(1,1),
    TypeName               nvarchar(50)    DEFAULT '' NOT NULL,
    NodeID                 int             DEFAULT 0 NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_GovInteractType PRIMARY KEY NONCLUSTERED (TypeID)
)
go



CREATE TABLE wcm_GovPublicApply(
    ID                     int              IDENTITY(1,1),
    StyleID                int              DEFAULT 0 NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    IsOrganization         varchar(18)      DEFAULT '' NOT NULL,
    CivicName              nvarchar(255)    DEFAULT '' NOT NULL,
    CivicOrganization      nvarchar(255)    DEFAULT '' NOT NULL,
    CivicCardType          nvarchar(255)    DEFAULT '' NOT NULL,
    CivicCardNo            nvarchar(255)    DEFAULT '' NOT NULL,
    CivicPhone             varchar(50)      DEFAULT '' NOT NULL,
    CivicPostCode          varchar(50)      DEFAULT '' NOT NULL,
    CivicAddress           nvarchar(255)    DEFAULT '' NOT NULL,
    CivicEmail             nvarchar(255)    DEFAULT '' NOT NULL,
    CivicFax               varchar(50)      DEFAULT '' NOT NULL,
    OrgName                nvarchar(255)    DEFAULT '' NOT NULL,
    OrgUnitCode            nvarchar(255)    DEFAULT '' NOT NULL,
    OrgLegalPerson         nvarchar(255)    DEFAULT '' NOT NULL,
    OrgLinkName            nvarchar(255)    DEFAULT '' NOT NULL,
    OrgPhone               varchar(50)      DEFAULT '' NOT NULL,
    OrgPostCode            varchar(50)      DEFAULT '' NOT NULL,
    OrgAddress             nvarchar(255)    DEFAULT '' NOT NULL,
    OrgEmail               nvarchar(255)    DEFAULT '' NOT NULL,
    OrgFax                 varchar(50)      DEFAULT '' NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Content                ntext            DEFAULT '' NOT NULL,
    Purpose                nvarchar(255)    DEFAULT '' NOT NULL,
    IsApplyFree            varchar(18)      DEFAULT '' NOT NULL,
    ProvideType            varchar(50)      DEFAULT '' NOT NULL,
    ObtainType             varchar(50)      DEFAULT '' NOT NULL,
    DepartmentName         nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    QueryCode              nvarchar(255)    DEFAULT '' NOT NULL,
    State                  varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovPublicApply PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_GovPublicApplyLog(
    LogID                  int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ApplyID                int              NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    LogType                varchar(50)      DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovPublicApplyLog PRIMARY KEY NONCLUSTERED (LogID)
)
go



CREATE TABLE wcm_GovPublicApplyRemark(
    RemarkID               int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ApplyID                int              NULL,
    RemarkType             varchar(50)      DEFAULT '' NOT NULL,
    Remark                 nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_GovPublicApplyRemark PRIMARY KEY NONCLUSTERED (RemarkID)
)
go



CREATE TABLE wcm_GovPublicApplyReply(
    ReplyID                int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ApplyID                int              NULL,
    Reply                  ntext            NOT NULL,
    FileUrl                nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID           int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_GovPublicApplyReply PRIMARY KEY NONCLUSTERED (ReplyID)
)
go



CREATE TABLE wcm_GovPublicCategory(
    CategoryID             int              IDENTITY(1,1),
    ClassCode              nvarchar(50)     DEFAULT '' NULL,
    PublishmentSystemID    int              DEFAULT 0 NULL,
    CategoryName           nvarchar(255)    DEFAULT '' NOT NULL,
    CategoryCode           varchar(50)      DEFAULT '' NOT NULL,
    ParentID               int              DEFAULT 0 NOT NULL,
    ParentsPath            nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount           int              DEFAULT 0 NOT NULL,
    ChildrenCount          int              DEFAULT 0 NOT NULL,
    IsLastNode             varchar(18)      DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    ContentNum             int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_GovPublicCategory PRIMARY KEY NONCLUSTERED (CategoryID)
)
go



CREATE TABLE wcm_GovPublicCategoryClass(
    ClassCode               nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    ClassName               nvarchar(255)    DEFAULT '' NOT NULL,
    IsSystem                varchar(18)      DEFAULT '' NOT NULL,
    IsEnabled               varchar(18)      DEFAULT '' NOT NULL,
    ContentAttributeName    varchar(50)      DEFAULT '' NOT NULL,
    Taxis                   int              DEFAULT 0 NOT NULL,
    Description             nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovPublicCategoryClass PRIMARY KEY NONCLUSTERED (ClassCode, PublishmentSystemID)
)
go



CREATE TABLE wcm_GovPublicChannel(
    NodeID                 int              NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Code                   nvarchar(50)     DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovPublicChannel PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE wcm_GovPublicIdentifierRule(
    RuleID                 int              IDENTITY(1,1),
    RuleName               nvarchar(255)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    IdentifierType         varchar(50)      DEFAULT '' NOT NULL,
    MinLength              int              DEFAULT 0 NOT NULL,
    Suffix                 varchar(50)      DEFAULT '' NOT NULL,
    FormatString           varchar(50)      DEFAULT '' NOT NULL,
    AttributeName          varchar(50)      DEFAULT '' NOT NULL,
    Sequence               int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    SettingsXML            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_GovPublicIdentifierRule PRIMARY KEY NONCLUSTERED (RuleID)
)
go



CREATE TABLE wcm_GovPublicIdentifierSeq(
    SeqID                  int    IDENTITY(1,1),
    PublishmentSystemID    int    DEFAULT 0 NOT NULL,
    NodeID                 int    DEFAULT 0 NOT NULL,
    DepartmentID           int    DEFAULT 0 NOT NULL,
    AddYear                int    DEFAULT 0 NOT NULL,
    Sequence               int    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_GovPublicIdentifierSeq PRIMARY KEY NONCLUSTERED (SeqID)
)
go



CREATE TABLE wcm_InnerLink(
    InnerLinkName          nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    LinkUrl                varchar(200)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_InnerLink PRIMARY KEY NONCLUSTERED (InnerLinkName, PublishmentSystemID)
)
go



CREATE TABLE wcm_Input(
    InputID                int             IDENTITY(1,1),
    InputName              nvarchar(50)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    AddDate                datetime        DEFAULT getdate() NOT NULL,
    IsChecked              varchar(18)     DEFAULT '' NOT NULL,
    IsReply                varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    IsTemplate             varchar(18)     DEFAULT '' NOT NULL,
    StyleTemplate          ntext           DEFAULT '' NOT NULL,
    ScriptTemplate         ntext           DEFAULT '' NOT NULL,
    ContentTemplate        ntext           DEFAULT '' NOT NULL,
    SettingsXML            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Input PRIMARY KEY NONCLUSTERED (InputID)
)
go



CREATE TABLE wcm_InputContent(
    ID             int              IDENTITY(1,1),
    InputID        int              NOT NULL,
    Taxis          int              DEFAULT 0 NOT NULL,
    IsChecked      varchar(18)      DEFAULT '' NOT NULL,
    UserName       nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress      varchar(50)      DEFAULT '' NOT NULL,
    Location       nvarchar(50)     DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    Reply          ntext            DEFAULT '' NOT NULL,
    SettingsXML    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_InputContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_Keyword(
    KeywordID      int             IDENTITY(1,1),
    Keyword        nvarchar(50)    DEFAULT '' NOT NULL,
    Alternative    nvarchar(50)    DEFAULT '' NOT NULL,
    Grade          nvarchar(50)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Keyword PRIMARY KEY NONCLUSTERED (KeywordID)
)
go



CREATE TABLE wcm_Log(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserName               varchar(50)      DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    Action                 nvarchar(255)    DEFAULT '' NOT NULL,
    Summary                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Log PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_MailSendLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    PageUrl                varchar(200)     DEFAULT '' NOT NULL,
    Receiver               nvarchar(255)    DEFAULT '' NOT NULL,
    Mail                   nvarchar(255)    DEFAULT '' NOT NULL,
    Sender                 nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_MailSendLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE wcm_MailSubscribe(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Receiver               nvarchar(255)    DEFAULT '' NOT NULL,
    Mail                   nvarchar(255)    DEFAULT '' NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_MailSubscribe PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE wcm_MenuDisplay(
    MenuDisplayID          int             IDENTITY(1,1),
    PublishmentSystemID    int             NOT NULL,
    MenuDisplayName        varchar(50)     DEFAULT '' NOT NULL,
    Vertical               varchar(50)     DEFAULT '' NOT NULL,
    FontFamily             varchar(200)    DEFAULT '' NOT NULL,
    FontSize               int             DEFAULT 0 NOT NULL,
    FontWeight             varchar(50)     DEFAULT '' NOT NULL,
    FontStyle              varchar(50)     DEFAULT '' NOT NULL,
    MenuItemHAlign         varchar(50)     DEFAULT '' NOT NULL,
    MenuItemVAlign         varchar(50)     DEFAULT '' NOT NULL,
    FontColor              varchar(50)     DEFAULT '' NOT NULL,
    MenuItemBgColor        varchar(50)     DEFAULT '' NOT NULL,
    FontColorHilite        varchar(50)     DEFAULT '' NOT NULL,
    MenuHiliteBgColor      varchar(50)     DEFAULT '' NOT NULL,
    XPosition              varchar(50)     DEFAULT '' NOT NULL,
    YPosition              varchar(50)     DEFAULT '' NOT NULL,
    HideOnMouseOut         varchar(50)     DEFAULT '' NOT NULL,
    MenuWidth              int             DEFAULT 0 NOT NULL,
    MenuItemHeight         int             DEFAULT 0 NOT NULL,
    MenuItemPadding        int             DEFAULT 0 NOT NULL,
    MenuItemSpacing        int             DEFAULT 0 NOT NULL,
    MenuItemIndent         int             DEFAULT 0 NOT NULL,
    HideTimeout            int             DEFAULT 0 NOT NULL,
    MenuBgOpaque           varchar(50)     DEFAULT '' NOT NULL,
    MenuBorder             int             DEFAULT 0 NOT NULL,
    BGColor                varchar(50)     DEFAULT '' NOT NULL,
    MenuBorderBgColor      varchar(50)     DEFAULT '' NOT NULL,
    MenuLiteBgColor        varchar(50)     DEFAULT '' NOT NULL,
    ChildMenuIcon          varchar(200)    DEFAULT '' NOT NULL,
    AddDate                datetime        DEFAULT getdate() NOT NULL,
    IsDefault              varchar(18)     DEFAULT '' NOT NULL,
    Description            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_MenuDisplay PRIMARY KEY CLUSTERED (MenuDisplayID)
)
go



CREATE TABLE wcm_Node(
    NodeID                     int              IDENTITY(1,1),
    NodeName                   nvarchar(255)    DEFAULT '' NOT NULL,
    NodeType                   varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID        int              DEFAULT 0 NOT NULL,
    ContentModelID             varchar(50)      DEFAULT '' NOT NULL,
    ParentID                   int              DEFAULT 0 NOT NULL,
    ParentsPath                nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount               int              DEFAULT 0 NOT NULL,
    ChildrenCount              int              DEFAULT 0 NOT NULL,
    IsLastNode                 varchar(18)      DEFAULT '' NOT NULL,
    NodeIndexName              nvarchar(255)    DEFAULT '' NOT NULL,
    NodeGroupNameCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis                      int              DEFAULT 0 NOT NULL,
    AddDate                    datetime         DEFAULT getdate() NOT NULL,
    ImageUrl                   varchar(200)     DEFAULT '' NOT NULL,
    Content                    ntext            DEFAULT '' NOT NULL,
    ContentNum                 int              DEFAULT 0 NOT NULL,
    CommentNum                 int              DEFAULT 0 NOT NULL,
    FilePath                   varchar(200)     DEFAULT '' NOT NULL,
    ChannelFilePathRule        varchar(200)     DEFAULT '' NOT NULL,
    ContentFilePathRule        varchar(200)     DEFAULT '' NOT NULL,
    LinkUrl                    varchar(200)     DEFAULT '' NOT NULL,
    LinkType                   varchar(200)     DEFAULT '' NOT NULL,
    ChannelTemplateID          int              DEFAULT 0 NOT NULL,
    ContentTemplateID          int              DEFAULT 0 NOT NULL,
    Keywords                   nvarchar(255)    DEFAULT '' NOT NULL,
    Description                nvarchar(255)    DEFAULT '' NOT NULL,
    ExtendValues               ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Node PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE wcm_NodeGroup(
    NodeGroupName          nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Description            ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_NodeGroup PRIMARY KEY CLUSTERED (NodeGroupName, PublishmentSystemID)
)
go



CREATE TABLE wcm_PhotoContent(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    PreviewUrl             varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Description            nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_PhotoContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_PublishmentSystem(
    PublishmentSystemID             int             NOT NULL,
    PublishmentSystemName           nvarchar(50)    DEFAULT '' NOT NULL,
    AuxiliaryTableForContent        varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForGovPublic      varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForGovInteract    varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForVote           varchar(50)     DEFAULT '' NOT NULL,
    AuxiliaryTableForJob            varchar(50)     DEFAULT '' NOT NULL,
    IsCheckContentUseLevel          varchar(18)     DEFAULT '' NOT NULL,
    CheckContentLevel               int             DEFAULT 0 NOT NULL,
    PublishmentSystemDir            varchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemUrl            varchar(200)    DEFAULT '' NOT NULL,
    IsHeadquarters                  varchar(18)     DEFAULT '' NOT NULL,
    ParentPublishmentSystemID       int             DEFAULT 0 NOT NULL,
    Taxis                           int             DEFAULT 0 NOT NULL,
    SettingsXML                     ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_PS PRIMARY KEY CLUSTERED (PublishmentSystemID)
)
go



CREATE TABLE wcm_RelatedField(
    RelatedFieldID         int              IDENTITY(1,1),
    RelatedFieldName       nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    TotalLevel             int              DEFAULT 0 NOT NULL,
    Prefixes               nvarchar(255)    DEFAULT '' NOT NULL,
    Suffixes               nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_RelatedField PRIMARY KEY NONCLUSTERED (RelatedFieldID)
)
go



CREATE TABLE wcm_RelatedFieldItem(
    ID                int              IDENTITY(1,1),
    RelatedFieldID    int              NOT NULL,
    ItemName          nvarchar(255)    DEFAULT '' NOT NULL,
    ItemValue         nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID          int              DEFAULT 0 NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_RelatedFieldItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_ResumeContent(
    ID                       int              IDENTITY(1,1),
    StyleID                  int              DEFAULT 0 NOT NULL,
    PublishmentSystemID      int              DEFAULT 0 NOT NULL,
    JobContentID             int              DEFAULT 0 NOT NULL,
    UserName                 nvarchar(255)    DEFAULT '' NOT NULL,
    IsView                   varchar(18)      DEFAULT '' NOT NULL,
    AddDate                  datetime         DEFAULT getdate() NOT NULL,
    RealName                 nvarchar(50)     DEFAULT '' NOT NULL,
    Nationality              nvarchar(50)     DEFAULT '' NOT NULL,
    Gender                   nvarchar(50)     DEFAULT '' NOT NULL,
    Email                    varchar(50)      DEFAULT '' NOT NULL,
    MobilePhone              varchar(50)      DEFAULT '' NOT NULL,
    HomePhone                varchar(50)      DEFAULT '' NOT NULL,
    LastSchoolName           nvarchar(50)     DEFAULT '' NOT NULL,
    Education                nvarchar(50)     DEFAULT '' NOT NULL,
    IDCardType               nvarchar(50)     DEFAULT '' NOT NULL,
    IDCardNo                 varchar(50)      DEFAULT '' NOT NULL,
    Birthday                 varchar(50)      DEFAULT '' NOT NULL,
    Marriage                 nvarchar(50)     DEFAULT '' NOT NULL,
    WorkYear                 nvarchar(50)     DEFAULT '' NOT NULL,
    Profession               nvarchar(50)     DEFAULT '' NOT NULL,
    ExpectSalary             nvarchar(50)     DEFAULT '' NOT NULL,
    AvailabelTime            nvarchar(50)     DEFAULT '' NOT NULL,
    Location                 nvarchar(50)     DEFAULT '' NOT NULL,
    ImageUrl                 varchar(200)     DEFAULT '' NOT NULL,
    Summary                  nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Count                int              DEFAULT 0 NOT NULL,
    Exp_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_EmployerName         nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Department           nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_EmployerPhone        nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_WorkPlace            nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_PositionTitle        nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Industry             nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Summary              ntext            DEFAULT '' NOT NULL,
    Exp_Score                ntext            DEFAULT '' NOT NULL,
    Pro_Count                int              DEFAULT 0 NOT NULL,
    Pro_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ProjectName          nvarchar(255)    DEFAULT '' NOT NULL,
    Pro_Summary              ntext            DEFAULT '' NOT NULL,
    Edu_Count                int              DEFAULT 0 NOT NULL,
    Edu_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_SchoolName           nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Education            nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Profession           nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Summary              ntext            DEFAULT '' NOT NULL,
    Tra_Count                int              DEFAULT 0 NOT NULL,
    Tra_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_TrainerName          nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_TrainerAddress       nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Lesson               nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Centification        nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Summary              nvarchar(255)    DEFAULT '' NOT NULL,
    Lan_Count                int              DEFAULT 0 NOT NULL,
    Lan_Language             nvarchar(255)    DEFAULT '' NOT NULL,
    Lan_Level                nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_Count                int              DEFAULT 0 NOT NULL,
    Ski_SkillName            nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_UsedTimes            nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_Ability              nvarchar(255)    DEFAULT '' NOT NULL,
    Cer_Count                int              DEFAULT 0 NOT NULL,
    Cer_CertificationName    nvarchar(255)    DEFAULT '' NOT NULL,
    Cer_EffectiveDate        nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_ResumeContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_SeoMeta(
    SeoMetaID              int              IDENTITY(1,1),
    PublishmentSystemID    int              NOT NULL,
    SeoMetaName            varchar(50)      DEFAULT '' NOT NULL,
    IsDefault              varchar(18)      DEFAULT '' NOT NULL,
    PageTitle              nvarchar(80)     DEFAULT '' NOT NULL,
    Keywords               nvarchar(100)    DEFAULT '' NOT NULL,
    Description            nvarchar(200)    DEFAULT '' NOT NULL,
    Copyright              nvarchar(255)    DEFAULT '' NOT NULL,
    Author                 nvarchar(50)     DEFAULT '' NOT NULL,
    Email                  nvarchar(50)     DEFAULT '' NOT NULL,
    Language               varchar(50)      DEFAULT '' NOT NULL,
    Charset                varchar(50)      DEFAULT '' NOT NULL,
    Distribution           varchar(50)      DEFAULT '' NOT NULL,
    Rating                 varchar(50)      DEFAULT '' NOT NULL,
    Robots                 varchar(50)      DEFAULT '' NOT NULL,
    RevisitAfter           varchar(50)      DEFAULT '' NOT NULL,
    Expires                varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_SeoMeta PRIMARY KEY CLUSTERED (SeoMetaID)
)
go



CREATE TABLE wcm_SeoMetasInNodes(
    NodeID                 int            NOT NULL,
    IsChannel              varchar(18)    DEFAULT '' NOT NULL,
    SeoMetaID              int            NOT NULL,
    PublishmentSystemID    int            NOT NULL,
    CONSTRAINT PK_wcm_SeoMetasInNodes PRIMARY KEY CLUSTERED (NodeID, IsChannel, SeoMetaID)
)
go



CREATE TABLE wcm_SigninLog(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IsSignin               varchar(18)      DEFAULT '' NOT NULL,
    SigninDate             datetime         DEFAULT getdate() NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_SigninLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE wcm_SigninSetting(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IsGroup                varchar(18)      DEFAULT '' NOT NULL,
    UserGroupCollection    varchar(500)     DEFAULT '' NOT NULL,
    UserNameCollection     nvarchar(500)    DEFAULT '' NOT NULL,
    Priority               int              DEFAULT 0 NOT NULL,
    EndDate                varchar(50)      DEFAULT '' NOT NULL,
    IsSignin               varchar(18)      DEFAULT '' NOT NULL,
    SigninDate             datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_SigninSetting PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE wcm_SigninUserContentID(
    ID                     int              IDENTITY(1,1),
    IsGroup                varchar(18)      DEFAULT '' NOT NULL,
    GroupID                int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentIDCollection    varchar(500)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_SigninUserContentID PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_Star(
    StarID                 int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    Point                  int              DEFAULT 0 NOT NULL,
    Message                nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_Star PRIMARY KEY NONCLUSTERED (StarID)
)
go



CREATE TABLE wcm_StarSetting(
    StarSettingID          int               IDENTITY(1,1),
    PublishmentSystemID    int               DEFAULT 0 NOT NULL,
    ChannelID              int               DEFAULT 0 NOT NULL,
    ContentID              int               DEFAULT 0 NOT NULL,
    TotalCount             int               DEFAULT 0 NOT NULL,
    PointAverage           decimal(18, 1)    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_StarSetting PRIMARY KEY NONCLUSTERED (StarSettingID)
)
go



CREATE TABLE wcm_StlTag(
    TagName                nvarchar(50)     NOT NULL,
    PublishmentSystemID    int              NOT NULL,
    TagDescription         nvarchar(255)    DEFAULT '' NOT NULL,
    TagContent             ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_StlTag PRIMARY KEY NONCLUSTERED (TagName, PublishmentSystemID)
)
go



CREATE TABLE wcm_SystemPermissions(
    RoleName               nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeIDCollection       text             DEFAULT '' NOT NULL,
    ChannelPermissions     text             DEFAULT '' NOT NULL,
    WebsitePermissions     text             DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_SP PRIMARY KEY CLUSTERED (RoleName, PublishmentSystemID)
)
go



CREATE TABLE wcm_TagStyle(
    StyleID                int             IDENTITY(1,1),
    StyleName              nvarchar(50)    DEFAULT '' NOT NULL,
    ElementName            varchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    IsTemplate             varchar(18)     DEFAULT '' NOT NULL,
    StyleTemplate          ntext           DEFAULT '' NOT NULL,
    ScriptTemplate         ntext           DEFAULT '' NOT NULL,
    ContentTemplate        ntext           DEFAULT '' NOT NULL,
    SuccessTemplate        ntext           DEFAULT '' NOT NULL,
    FailureTemplate        ntext           DEFAULT '' NOT NULL,
    SettingsXML            ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_TagStyle PRIMARY KEY CLUSTERED (StyleID)
)
go



CREATE TABLE wcm_Template(
    TemplateID             int             IDENTITY(1,1),
    PublishmentSystemID    int             NOT NULL,
    TemplateName           nvarchar(50)    DEFAULT '' NOT NULL,
    TemplateType           varchar(50)     DEFAULT '' NOT NULL,
    RelatedFileName        nvarchar(50)    DEFAULT '' NOT NULL,
    CreatedFileFullName    nvarchar(50)    DEFAULT '' NOT NULL,
    CreatedFileExtName     varchar(50)     DEFAULT '' NOT NULL,
    Charset                varchar(50)     DEFAULT '' NOT NULL,
    IsDefault              varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Template PRIMARY KEY CLUSTERED (TemplateID)
)
go



CREATE TABLE wcm_TemplateLog(
    ID                     int              IDENTITY(1,1),
    TemplateID             int              NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    AddUserName            nvarchar(255)    DEFAULT '' NOT NULL,
    ContentLength          int              DEFAULT 0 NOT NULL,
    TemplateContent        ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_TemplateLog PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE wcm_TemplateMatch(
    NodeID                 int             NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    ChannelTemplateID      int             DEFAULT 0 NOT NULL,
    ContentTemplateID      int             DEFAULT 0 NOT NULL,
    FilePath               varchar(200)    DEFAULT '' NOT NULL,
    ChannelFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    ContentFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_TemplateMatch PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE wcm_TouGaoSetting(
    SettingID                    int             IDENTITY(1,1),
    PublishmentSystemID          int             DEFAULT 0 NOT NULL,
    UserTypeID                   int             DEFAULT 0 NOT NULL,
    IsTouGaoAllowed              varchar(18)     DEFAULT '' NOT NULL,
    CheckLevel                   int             DEFAULT 0 NOT NULL,
    IsCheckAllowed               varchar(18)     DEFAULT '' NOT NULL,
    IsCheckAddedUsersOnly        varchar(18)     DEFAULT '' NOT NULL,
    CheckUserTypeIDCollection    varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_TouGaoSetting PRIMARY KEY NONCLUSTERED (SettingID)
)
go



CREATE TABLE wcm_Tracking(
    TrackingID             int              IDENTITY(1,1),
    PublishmentSystemID    int              NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    TrackerType            varchar(50)      DEFAULT '' NOT NULL,
    LastAccessDateTime     datetime         DEFAULT getdate() NOT NULL,
    PageUrl                varchar(200)     DEFAULT '' NOT NULL,
    PageNodeID             int              DEFAULT 0 NOT NULL,
    PageContentID          int              DEFAULT 0 NOT NULL,
    Referrer               varchar(200)     DEFAULT '' NOT NULL,
    IPAddress              varchar(200)     DEFAULT '' NOT NULL,
    OperatingSystem        varchar(200)     DEFAULT '' NOT NULL,
    Browser                varchar(200)     DEFAULT '' NOT NULL,
    AccessDateTime         datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_Tracking PRIMARY KEY CLUSTERED (TrackingID)
)
go



CREATE TABLE wcm_UserGroup(
    GroupID            int             IDENTITY(1,1),
    GroupName          nvarchar(50)    DEFAULT '' NOT NULL,
    IsCredits          varchar(18)     DEFAULT '' NOT NULL,
    CreditsFrom        int             DEFAULT 0 NOT NULL,
    CreditsTo          int             DEFAULT 0 NOT NULL,
    Stars              int             DEFAULT 0 NOT NULL,
    Color              varchar(10)     DEFAULT '' NOT NULL,
    Rank               int             DEFAULT 0 NOT NULL,
    UserPermissions    ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_UserGroup PRIMARY KEY NONCLUSTERED (GroupID)
)
go



CREATE TABLE wcm_Users(
    UserName       nvarchar(255)    NOT NULL,
    GroupID        int              DEFAULT 0 NOT NULL,
    Credits        int              DEFAULT 0 NOT NULL,
    SettingsXML    ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_wcm_Users PRIMARY KEY NONCLUSTERED (UserName)
)
go



CREATE TABLE wcm_VoteOperation(
    OperationID            int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IPAddress              varchar(50)      DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_wcm_VoteOperation PRIMARY KEY NONCLUSTERED (OperationID)
)
go



CREATE TABLE wcm_VoteOption(
    OptionID               int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    NavigationUrl          varchar(200)     DEFAULT '' NOT NULL,
    VoteNum                int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_VoteOption PRIMARY KEY NONCLUSTERED (OptionID)
)
go



CREATE INDEX IX_wcm_MD_N ON wcm_MenuDisplay(PublishmentSystemID)
go
CREATE CLUSTERED INDEX IX_wcm_Node_Taxis ON wcm_Node(Taxis)
go
CREATE INDEX IX_wcm_Node_P ON wcm_Node(PublishmentSystemID)
go
CREATE INDEX IX_wcm_SM_N ON wcm_SeoMeta(PublishmentSystemID)
go
CREATE INDEX IX_wcm_Template_Node ON wcm_Template(PublishmentSystemID)
go
CREATE INDEX IX_wcm_Template_TT ON wcm_Template(TemplateType)
go
CREATE INDEX IX_wcm_Tracking_P ON wcm_Tracking(PublishmentSystemID)
go
CREATE INDEX IX_wcm_Tracking_Page ON wcm_Tracking(PageNodeID, PageContentID)
go
ALTER TABLE wcm_Comment ADD CONSTRAINT FK_wcm_Node_Comment 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GatherDatabaseRule ADD CONSTRAINT FK_wcm_GDR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GatherFileRule ADD CONSTRAINT FK_wcm_GFR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GatherRule ADD CONSTRAINT FK_wcm_GR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractChannel ADD CONSTRAINT FK_wcm_GIC_N 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractLog ADD CONSTRAINT FK_RIC_RIL 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_GovInteractChannel(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractPermissions ADD CONSTRAINT FK_GIC_GIA 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_GovInteractChannel(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractRemark ADD CONSTRAINT FK_GIC_GIRe 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_GovInteractChannel(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractReply ADD CONSTRAINT FK_GIC_GIR 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_GovInteractChannel(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovInteractType ADD CONSTRAINT FK_GIC_GIT 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_GovInteractChannel(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovPublicApplyLog ADD CONSTRAINT FK_wcm_GPA_GPAL 
    FOREIGN KEY (ApplyID)
    REFERENCES wcm_GovPublicApply(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovPublicApplyRemark ADD CONSTRAINT FK_wcm_GPARemark_GPAL 
    FOREIGN KEY (ApplyID)
    REFERENCES wcm_GovPublicApply(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovPublicApplyReply ADD CONSTRAINT FK_wcm_GPAReply_GPAL 
    FOREIGN KEY (ApplyID)
    REFERENCES wcm_GovPublicApply(ID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovPublicCategory ADD CONSTRAINT FK_wcm_GPC_GPCC 
    FOREIGN KEY (ClassCode, PublishmentSystemID)
    REFERENCES wcm_GovPublicCategoryClass(ClassCode, PublishmentSystemID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_GovPublicChannel ADD CONSTRAINT FK_wcm_GPC_N 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_InputContent ADD CONSTRAINT FK_wcm_IC_I 
    FOREIGN KEY (InputID)
    REFERENCES wcm_Input(InputID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_MenuDisplay ADD CONSTRAINT FK_wcm_MD_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_PublishmentSystem ADD CONSTRAINT FK_wcm_PS_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_RelatedFieldItem ADD CONSTRAINT FK_wcm_RFI_RF 
    FOREIGN KEY (RelatedFieldID)
    REFERENCES wcm_RelatedField(RelatedFieldID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_SeoMeta ADD CONSTRAINT FK_wcm_SM_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_SeoMetasInNodes ADD CONSTRAINT FK_wcm_SMInN_SM 
    FOREIGN KEY (SeoMetaID)
    REFERENCES wcm_SeoMeta(SeoMetaID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_SystemPermissions ADD CONSTRAINT FK_wcm_SP_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_Template ADD CONSTRAINT FK_wcm_Template_Node 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_TemplateLog ADD CONSTRAINT FK_Template_Log 
    FOREIGN KEY (TemplateID)
    REFERENCES wcm_Template(TemplateID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_TemplateMatch ADD CONSTRAINT FK_wcm_TM_N 
    FOREIGN KEY (NodeID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE wcm_Tracking ADD CONSTRAINT FK_wcm_Tracking_Node 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


