--
-- ER/Studio 8.0 SQL Code Generation
-- Company :      BaiRong Software
-- Project :      BaiRong SiteServer CMS
-- Author :       BaiRong Software
--
-- Date Created : Sunday, July 17, 2011 09:11:05
-- Target DBMS : Oracle 9i
--

CREATE SEQUENCE SITESERVER_INPUT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_INPUTCONTENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_LOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_MAILSENDLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_MAILSUBSCRIBE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_MENUDISPLAY_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_NODE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_PHOTOCONTENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_RELATEDFIELD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_RELATEDFIELDITE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_RESUMECONTENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_SEOMETA_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_STAR_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_STARSETTING_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TAG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TAGSTYLE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TASKLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TEMPLATE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TEMPLATERULE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_TRACKING_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE SITESERVER_USERGROUP_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE TABLE siteserver_Ad(
    AdName                 NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AdType                 VARCHAR2(50)      DEFAULT '',
    Code                   NCLOB             DEFAULT '',
    TextWord               NVARCHAR2(255)    DEFAULT '',
    TextLink               VARCHAR2(200)     DEFAULT '',
    TextColor              VARCHAR2(10)      DEFAULT '',
    TextFontSize           NUMBER            DEFAULT 0 NOT NULL,
    ImageUrl               VARCHAR2(200)     DEFAULT '',
    ImageLink              VARCHAR2(200)     DEFAULT '',
    ImageWidth             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ImageHeight            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ImageAlt               NVARCHAR2(50)     DEFAULT '',
    IsEnabled              VARCHAR2(18)      DEFAULT '',
    IsDateLimited          VARCHAR2(18)      DEFAULT '',
    StartDate              TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    EndDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_Ad PRIMARY KEY (AdName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_Advertisement(
    AdvertisementName            VARCHAR2(50)      NOT NULL,
    PublishmentSystemID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AdvertisementType            VARCHAR2(50)      DEFAULT '',
    NavigationUrl                VARCHAR2(200)     DEFAULT '',
    IsDateLimited                VARCHAR2(18)      DEFAULT '',
    StartDate                    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    EndDate                      TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    AddDate                      TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    NodeIDCollectionToChannel    NVARCHAR2(255)    DEFAULT '',
    NodeIDCollectionToContent    NVARCHAR2(255)    DEFAULT '',
    FileTemplateIDCollection     NVARCHAR2(255)    DEFAULT '',
    Settings                     NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_Advertisement PRIMARY KEY (AdvertisementName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_Configuration(
    SettingsXML    NCLOB    DEFAULT ''
)
GO



CREATE TABLE siteserver_ContentGroup(
    ContentGroupName       NVARCHAR2(255)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Description            NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_ContentGroup PRIMARY KEY (ContentGroupName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_ContentModel(
    ModelID                VARCHAR2(50)      NOT NULL,
    ModelName              NVARCHAR2(50)     DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    TableName              VARCHAR2(200)     DEFAULT '',
    ItemName               NVARCHAR2(50)     DEFAULT '',
    ItemIcon               VARCHAR2(50)      DEFAULT '',
    Description            NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_siteserver_ContentModel PRIMARY KEY (ModelID)
)
GO



CREATE TABLE siteserver_GatherDatabaseRule(
    GatherRuleName         VARCHAR2(50)      NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    DatabaseType           VARCHAR2(50)      DEFAULT '',
    ConnectionString       VARCHAR2(255)     DEFAULT '',
    RelatedTableName       VARCHAR2(255)     DEFAULT '',
    RelatedIdentity        VARCHAR2(255)     DEFAULT '',
    RelatedOrderBy         VARCHAR2(255)     DEFAULT '',
    WhereString            NVARCHAR2(255)    DEFAULT '',
    TableMatchID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    GatherNum              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsChecked              VARCHAR2(18)      DEFAULT '',
    IsOrderByDesc          VARCHAR2(18)      DEFAULT '',
    LastGatherDate         TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_GatherDR PRIMARY KEY (GatherRuleName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_GatherFileRule(
    GatherRuleName                VARCHAR2(50)      NOT NULL,
    PublishmentSystemID           NUMBER(38, 0)     NOT NULL,
    GatherUrl                     NVARCHAR2(255)    DEFAULT '',
    Charset                       VARCHAR2(50)      DEFAULT '',
    LastGatherDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IsToFile                      VARCHAR2(18)      DEFAULT '',
    FilePath                      NVARCHAR2(255)    DEFAULT '',
    IsSaveRelatedFiles            VARCHAR2(18)      DEFAULT '',
    IsRemoveScripts               VARCHAR2(18)      DEFAULT '',
    StyleDirectoryPath            NVARCHAR2(255)    DEFAULT '',
    ScriptDirectoryPath           NVARCHAR2(255)    DEFAULT '',
    ImageDirectoryPath            NVARCHAR2(255)    DEFAULT '',
    NodeID                        NUMBER(38, 0)     NOT NULL,
    IsSaveImage                   VARCHAR2(18)      DEFAULT '',
    IsChecked                     VARCHAR2(18)      DEFAULT '',
    ContentExclude                NCLOB             DEFAULT '',
    ContentHtmlClearCollection    VARCHAR2(255)     DEFAULT '',
    ContentTitleStart             NCLOB             DEFAULT '',
    ContentTitleEnd               NCLOB             DEFAULT '',
    ContentContentStart           NCLOB             DEFAULT '',
    ContentContentEnd             NCLOB             DEFAULT '',
    ContentAttributes             NCLOB             DEFAULT '',
    ContentAttributesXML          NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_GatherFileRule PRIMARY KEY (GatherRuleName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_GatherRule(
    GatherRuleName                VARCHAR2(50)      NOT NULL,
    PublishmentSystemID           NUMBER(38, 0)     NOT NULL,
    CookieString                  CLOB              DEFAULT '',
    GatherUrlIsCollection         VARCHAR2(18)      DEFAULT '',
    GatherUrlCollection           CLOB              DEFAULT '',
    GatherUrlIsSerialize          VARCHAR2(18)      DEFAULT '',
    GatherUrlSerialize            VARCHAR2(200)     DEFAULT '',
    SerializeFrom                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeTo                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeInterval             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SerializeIsOrderByDesc        VARCHAR2(18)      DEFAULT '',
    SerializeIsAddZero            VARCHAR2(18)      DEFAULT '',
    NodeID                        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Charset                       VARCHAR2(50)      DEFAULT '',
    UrlInclude                    VARCHAR2(200)     DEFAULT '',
    TitleInclude                  NVARCHAR2(255)    DEFAULT '',
    ContentExclude                NCLOB             DEFAULT '',
    ContentHtmlClearCollection    VARCHAR2(255)     DEFAULT '',
    LastGatherDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    ListAreaStart                 NCLOB             DEFAULT '',
    ListAreaEnd                   NCLOB             DEFAULT '',
    ContentChannelStart           NCLOB             DEFAULT '',
    ContentChannelEnd             NCLOB             DEFAULT '',
    ContentTitleStart             NCLOB             DEFAULT '',
    ContentTitleEnd               NCLOB             DEFAULT '',
    ContentContentStart           NCLOB             DEFAULT '',
    ContentContentEnd             NCLOB             DEFAULT '',
    ContentNextPageStart          NCLOB             DEFAULT '',
    ContentNextPageEnd            NCLOB             DEFAULT '',
    ContentAttributes             NCLOB             DEFAULT '',
    ContentAttributesXML          NCLOB             DEFAULT '',
    ExtendValues                  NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_GatherRule PRIMARY KEY (GatherRuleName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_InnerLink(
    InnerLinkName          NVARCHAR2(255)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    LinkUrl                VARCHAR2(200)     DEFAULT '',
    CONSTRAINT PK_siteserver_InnerLink PRIMARY KEY (InnerLinkName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_Input(
    InputID                NUMBER(38, 0)    NOT NULL,
    InputName              NVARCHAR2(50)    DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    AddDate                TIMESTAMP(6)     DEFAULT sysdate NOT NULL,
    IsChecked              VARCHAR2(18)     DEFAULT '',
    IsReply                VARCHAR2(18)     DEFAULT '',
    Taxis                  NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsTemplate             VARCHAR2(18)     DEFAULT '',
    StyleTemplate          NCLOB            DEFAULT '',
    ScriptTemplate         NCLOB            DEFAULT '',
    ContentTemplate        NCLOB            DEFAULT '',
    SettingsXML            NCLOB            DEFAULT '',
    CONSTRAINT PK_siteserver_Input PRIMARY KEY (InputID)
)
GO



CREATE TABLE siteserver_InputContent(
    ID             NUMBER(38, 0)     NOT NULL,
    InputID        NUMBER(38, 0)     NOT NULL,
    Taxis          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsChecked      VARCHAR2(18)      DEFAULT '',
    UserName       NVARCHAR2(255)    DEFAULT '',
    IPAddress      VARCHAR2(50)      DEFAULT '',
    Location       NVARCHAR2(50)     DEFAULT '',
    AddDate        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Reply          NCLOB             DEFAULT '',
    SettingsXML    NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_InputContent PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_Log(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChannelID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               VARCHAR2(50)      DEFAULT '' NOT NULL,
    IPAddress              VARCHAR2(50)      DEFAULT '' NOT NULL,
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Action                 NVARCHAR2(255)    DEFAULT '' NOT NULL,
    Summary                NVARCHAR2(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Log PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_Machine(
    MachineName             NVARCHAR2(50)    DEFAULT '' NOT NULL,
    PublishmentSystemID     NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ConnectionType          VARCHAR2(50)     DEFAULT '',
    ServiceType             VARCHAR2(50)     DEFAULT '',
    IsEnabled               VARCHAR2(18)     DEFAULT '',
    FtpServer               VARCHAR2(200)    DEFAULT '',
    FtpPort                 NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    FtpUserName             VARCHAR2(200)    DEFAULT '',
    FtpPassword             VARCHAR2(200)    DEFAULT '',
    FtpHomeDirectory        VARCHAR2(200)    DEFAULT '',
    NetworkDirectoryPath    VARCHAR2(200)    DEFAULT '',
    LocalDirectoryPath      VARCHAR2(200)    DEFAULT '',
    CONSTRAINT PK_siteserver_Machine PRIMARY KEY (MachineName)
)
GO



CREATE TABLE siteserver_MailSendLog(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChannelID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Title                  NVARCHAR2(255)    DEFAULT '',
    PageUrl                VARCHAR2(200)     DEFAULT '',
    Receiver               NVARCHAR2(255)    DEFAULT '',
    Mail                   NVARCHAR2(255)    DEFAULT '',
    Sender                 NVARCHAR2(255)    DEFAULT '',
    IPAddress              VARCHAR2(50)      DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_MailSendLog PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_MailSubscribe(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Receiver               NVARCHAR2(255)    DEFAULT '',
    Mail                   NVARCHAR2(255)    DEFAULT '',
    IPAddress              VARCHAR2(50)      DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_MailSubscribe PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_MenuDisplay(
    MenuDisplayID          NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    NOT NULL,
    MenuDisplayName        VARCHAR2(50)     DEFAULT '',
    Vertical               VARCHAR2(50)     DEFAULT '',
    FontFamily             VARCHAR2(200)    DEFAULT '',
    FontSize               NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    FontWeight             VARCHAR2(50)     DEFAULT '',
    FontStyle              VARCHAR2(50)     DEFAULT '',
    MenuItemHAlign         VARCHAR2(50)     DEFAULT '',
    MenuItemVAlign         VARCHAR2(50)     DEFAULT '',
    FontColor              VARCHAR2(50)     DEFAULT '',
    MenuItemBgColor        VARCHAR2(50)     DEFAULT '',
    FontColorHilite        VARCHAR2(50)     DEFAULT '',
    MenuHiliteBgColor      VARCHAR2(50)     DEFAULT '',
    XPosition              VARCHAR2(50)     DEFAULT '',
    YPosition              VARCHAR2(50)     DEFAULT '',
    HideOnMouseOut         VARCHAR2(50)     DEFAULT '',
    MenuWidth              NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    MenuItemHeight         NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    MenuItemPadding        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    MenuItemSpacing        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    MenuItemIndent         NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    HideTimeout            NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    MenuBgOpaque           VARCHAR2(50)     DEFAULT '',
    MenuBorder             NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    BGColor                VARCHAR2(50)     DEFAULT '',
    MenuBorderBgColor      VARCHAR2(50)     DEFAULT '',
    MenuLiteBgColor        VARCHAR2(50)     DEFAULT '',
    ChildMenuIcon          VARCHAR2(200)    DEFAULT '',
    AddDate                TIMESTAMP(6)     DEFAULT sysdate NOT NULL,
    IsDefault              VARCHAR2(18)     DEFAULT '',
    Description            NCLOB            DEFAULT '',
    CONSTRAINT PK_siteserver_MenuDisplay PRIMARY KEY (MenuDisplayID)
)
GO



CREATE TABLE siteserver_Node(
    NodeID                     NUMBER(38, 0)     NOT NULL,
    NodeName                   NVARCHAR2(255)    DEFAULT '',
    NodeType                   VARCHAR2(50)      DEFAULT '',
    PublishmentSystemID        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentModelID             VARCHAR2(50)      DEFAULT '',
    ParentID                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ParentsPath                NVARCHAR2(255)    DEFAULT '',
    ParentsCount               NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChildrenCount              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsLastNode                 VARCHAR2(18)      DEFAULT '',
    NodeIndexName              NVARCHAR2(255)    DEFAULT '',
    NodeGroupNameCollection    NVARCHAR2(255)    DEFAULT '',
    Taxis                      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AddUserName                NVARCHAR2(255)    DEFAULT '',
    AddDate                    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    ImageUrl                   VARCHAR2(200)     DEFAULT '',
    Content                    NCLOB             DEFAULT '',
    ContentNum                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    FilePath                   VARCHAR2(200)     DEFAULT '',
    ChannelFilePathRule        VARCHAR2(200)     DEFAULT '',
    ContentFilePathRule        VARCHAR2(200)     DEFAULT '',
    LinkUrl                    VARCHAR2(200)     DEFAULT '',
    LinkType                   VARCHAR2(200)     DEFAULT '',
    ChannelTemplateID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentTemplateID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Keywords                   NVARCHAR2(255)    DEFAULT '',
    Description                NVARCHAR2(255)    DEFAULT '',
    ExtendValues               NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_Node PRIMARY KEY (NodeID)
)
GO



CREATE TABLE siteserver_NodeGroup(
    NodeGroupName          NVARCHAR2(255)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Description            NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_NodeGroup PRIMARY KEY (NodeGroupName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_PagePermissions(
    UserGroupID               NUMBER(38, 0)    NOT NULL,
    NodeID                    NUMBER(38, 0)    NOT NULL,
    ChannelPagePermissions    CLOB             DEFAULT '',
    ContentPagePermissions    CLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_PagePermissions PRIMARY KEY (NodeID, UserGroupID)
)
GO



CREATE TABLE siteserver_PhotoContent(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    PreviewUrl             VARCHAR2(200)     DEFAULT '',
    ImageUrl               VARCHAR2(200)     DEFAULT '',
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Title                  NVARCHAR2(255)    DEFAULT '',
    Description            VARCHAR2(255)     DEFAULT '',
    CONSTRAINT PK_siteserver_PhotoContent PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_PublishmentSystem(
    PublishmentSystemID          NUMBER(38, 0)    NOT NULL,
    PublishmentSystemName        NVARCHAR2(50)    DEFAULT '',
    AuxiliaryTableForChannel     VARCHAR2(50)     DEFAULT '',
    AuxiliaryTableForContent     VARCHAR2(50)     DEFAULT '',
    AuxiliaryTableForJob         VARCHAR2(50)     DEFAULT '',
    AuxiliaryTableForComment     VARCHAR2(50)     DEFAULT '',
    IsCheckContentUseLevel       VARCHAR2(18)     DEFAULT '',
    CheckContentLevel            NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    PublishmentSystemDir         VARCHAR2(50)     DEFAULT '',
    PublishmentSystemUrl         VARCHAR2(200)    DEFAULT '',
    IsHeadquarters               VARCHAR2(18)     DEFAULT '',
    ParentPublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Taxis                        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsRelatedUrl                 VARCHAR2(18)     DEFAULT '',
    SettingsXML                  NCLOB            DEFAULT '',
    CONSTRAINT PK_siteserver_PS PRIMARY KEY (PublishmentSystemID)
)
GO



CREATE TABLE siteserver_RelatedField(
    RelatedFieldID         NUMBER(38, 0)     NOT NULL,
    RelatedFieldName       NVARCHAR2(50)     DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    TotalLevel             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Prefixes               NVARCHAR2(255)    DEFAULT '',
    Suffixes               NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_siteserver_RelatedField PRIMARY KEY (RelatedFieldID)
)
GO



CREATE TABLE siteserver_RelatedFieldItem(
    ID                NUMBER(38, 0)     NOT NULL,
    RelatedFieldID    NUMBER(38, 0)     NOT NULL,
    ItemName          NVARCHAR2(255)    DEFAULT '',
    ItemValue         NVARCHAR2(255)    DEFAULT '',
    ParentID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Taxis             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_RelatedFieldItem PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_ResumeContent(
    ID                       NUMBER(38, 0)     NOT NULL,
    StyleID                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    PublishmentSystemID      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    JobContentID             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName                 NVARCHAR2(255)    DEFAULT '',
    IsView                   VARCHAR2(18)      DEFAULT '',
    AddDate                  TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    RealName                 NVARCHAR2(50)     DEFAULT '',
    Nationality              NVARCHAR2(50)     DEFAULT '',
    Gender                   NVARCHAR2(50)     DEFAULT '',
    Email                    VARCHAR2(50)      DEFAULT '',
    MobilePhone              VARCHAR2(50)      DEFAULT '',
    HomePhone                VARCHAR2(50)      DEFAULT '',
    LastSchoolName           NVARCHAR2(50)     DEFAULT '',
    Education                NVARCHAR2(50)     DEFAULT '',
    IDCardType               NVARCHAR2(50)     DEFAULT '',
    IDCardNo                 VARCHAR2(50)      DEFAULT '',
    Birthday                 VARCHAR2(50)      DEFAULT '',
    Marriage                 NVARCHAR2(50)     DEFAULT '',
    WorkYear                 NVARCHAR2(50)     DEFAULT '',
    Profession               NVARCHAR2(50)     DEFAULT '',
    ExpectSalary             NVARCHAR2(50)     DEFAULT '',
    AvailabelTime            NVARCHAR2(50)     DEFAULT '',
    Location                 NVARCHAR2(50)     DEFAULT '',
    ImageUrl                 VARCHAR2(200)     DEFAULT '',
    Summary                  NVARCHAR2(255)    DEFAULT '',
    Exp_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Exp_FromYear             NVARCHAR2(50)     DEFAULT '',
    Exp_FromMonth            NVARCHAR2(50)     DEFAULT '',
    Exp_ToYear               NVARCHAR2(50)     DEFAULT '',
    Exp_ToMonth              NVARCHAR2(50)     DEFAULT '',
    Exp_EmployerName         NVARCHAR2(255)    DEFAULT '',
    Exp_Department           NVARCHAR2(255)    DEFAULT '',
    Exp_EmployerPhone        NVARCHAR2(255)    DEFAULT '',
    Exp_WorkPlace            NVARCHAR2(255)    DEFAULT '',
    Exp_PositionTitle        NVARCHAR2(255)    DEFAULT '',
    Exp_Industry             NVARCHAR2(255)    DEFAULT '',
    Exp_Summary              NCLOB             DEFAULT '',
    Exp_Score                NCLOB             DEFAULT '',
    Pro_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Pro_FromYear             NVARCHAR2(50)     DEFAULT '',
    Pro_FromMonth            NVARCHAR2(50)     DEFAULT '',
    Pro_ToYear               NVARCHAR2(50)     DEFAULT '',
    Pro_ToMonth              NVARCHAR2(50)     DEFAULT '',
    Pro_ProjectName          NVARCHAR2(255)    DEFAULT '',
    Pro_Summary              NCLOB             DEFAULT '',
    Edu_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Edu_FromYear             NVARCHAR2(50)     DEFAULT '',
    Edu_FromMonth            NVARCHAR2(50)     DEFAULT '',
    Edu_ToYear               NVARCHAR2(50)     DEFAULT '',
    Edu_ToMonth              NVARCHAR2(50)     DEFAULT '',
    Edu_SchoolName           NVARCHAR2(255)    DEFAULT '',
    Edu_Education            NVARCHAR2(255)    DEFAULT '',
    Edu_Profession           NVARCHAR2(255)    DEFAULT '',
    Edu_Summary              NCLOB             DEFAULT '',
    Tra_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Tra_FromYear             NVARCHAR2(50)     DEFAULT '',
    Tra_FromMonth            NVARCHAR2(50)     DEFAULT '',
    Tra_ToYear               NVARCHAR2(50)     DEFAULT '',
    Tra_ToMonth              NVARCHAR2(50)     DEFAULT '',
    Tra_TrainerName          NVARCHAR2(255)    DEFAULT '',
    Tra_TrainerAddress       NVARCHAR2(255)    DEFAULT '',
    Tra_Lesson               NVARCHAR2(255)    DEFAULT '',
    Tra_Centification        NVARCHAR2(255)    DEFAULT '',
    Tra_Summary              NVARCHAR2(255)    DEFAULT '',
    Lan_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Lan_Language             NVARCHAR2(255)    DEFAULT '',
    Lan_Level                NVARCHAR2(255)    DEFAULT '',
    Ski_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Ski_SkillName            NVARCHAR2(255)    DEFAULT '',
    Ski_UsedTimes            NVARCHAR2(255)    DEFAULT '',
    Ski_Ability              NVARCHAR2(255)    DEFAULT '',
    Cer_Count                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Cer_CertificationName    NVARCHAR2(255)    DEFAULT '',
    Cer_EffectiveDate        NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_siteserver_ResumeContent PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_SeoMeta(
    SeoMetaID              NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    SeoMetaName            VARCHAR2(50)      DEFAULT '',
    IsDefault              VARCHAR2(18)      DEFAULT '',
    PageTitle              NVARCHAR2(80)     DEFAULT '',
    Keywords               NVARCHAR2(100)    DEFAULT '',
    Description            NVARCHAR2(200)    DEFAULT '',
    Copyright              NVARCHAR2(255)    DEFAULT '',
    Author                 NVARCHAR2(50)     DEFAULT '',
    Email                  NVARCHAR2(50)     DEFAULT '',
    Language               VARCHAR2(50)      DEFAULT '',
    Charset                VARCHAR2(50)      DEFAULT '',
    Distribution           VARCHAR2(50)      DEFAULT '',
    Rating                 VARCHAR2(50)      DEFAULT '',
    Robots                 VARCHAR2(50)      DEFAULT '',
    RevisitAfter           VARCHAR2(50)      DEFAULT '',
    Expires                VARCHAR2(50)      DEFAULT '',
    CONSTRAINT PK_siteserver_SeoMeta PRIMARY KEY (SeoMetaID)
)
GO



CREATE TABLE siteserver_SeoMetasInNodes(
    NodeID                 NUMBER(38, 0)    NOT NULL,
    IsChannel              VARCHAR2(18)     DEFAULT '' NOT NULL,
    SeoMetaID              NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    NOT NULL,
    CONSTRAINT PK_siteserver_SeoMetasInNodes PRIMARY KEY (NodeID, IsChannel, SeoMetaID)
)
GO



CREATE TABLE siteserver_Star(
    StarID                 NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChannelID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    Point                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Message                NVARCHAR2(255)    DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_Star PRIMARY KEY (StarID)
)
GO



CREATE TABLE siteserver_StarSetting(
    StarSettingID          NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ChannelID              NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    TotalCount             NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    PointAverage           NUMBER(18, 1)    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_StarSetting PRIMARY KEY (StarSettingID)
)
GO



CREATE TABLE siteserver_StlTag(
    TagName                NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    TagDescription         NVARCHAR2(255)    DEFAULT '',
    TagContent             NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_StlTag PRIMARY KEY (TagName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_SystemPermissions(
    RoleName               NVARCHAR2(255)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    NodeIDCollection       CLOB              DEFAULT '',
    ChannelPermissions     CLOB              DEFAULT '',
    WebsitePermissions     CLOB              DEFAULT '',
    CONSTRAINT PK_siteserver_SP PRIMARY KEY (RoleName, PublishmentSystemID)
)
GO



CREATE TABLE siteserver_Tag(
    TagID                  NUMBER(38, 0)     NOT NULL,
    TagName                NVARCHAR2(255)    DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_Tag PRIMARY KEY (TagID)
)
GO



CREATE TABLE siteserver_TagStyle(
    StyleID                NUMBER(38, 0)    NOT NULL,
    StyleName              NVARCHAR2(50)    DEFAULT '' NOT NULL,
    ElementName            VARCHAR2(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsTemplate             VARCHAR2(18)     DEFAULT '' NOT NULL,
    StyleTemplate          NCLOB            DEFAULT '',
    ScriptTemplate         NCLOB            DEFAULT '',
    ContentTemplate        NCLOB            DEFAULT '',
    SettingsXML            NCLOB            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TagStyle PRIMARY KEY (StyleID)
)
GO



CREATE TABLE siteserver_Task(
    TaskName                NVARCHAR2(50)     NOT NULL,
    PublishmentSystemID     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ServiceType             VARCHAR2(50)      DEFAULT '',
    ServiceParameters       NCLOB             DEFAULT '',
    FrequencyType           VARCHAR2(50)      DEFAULT '',
    PeriodIntervalMinute    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartDay                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartWeekday            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    StartHour               NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsEnabled               VARCHAR2(18)      DEFAULT '',
    AddDate                 TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    LastExecuteDate         TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Description             NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_siteserver_Task PRIMARY KEY (TaskName)
)
GO



CREATE TABLE siteserver_TaskLog(
    ID                     NUMBER(38, 0)     NOT NULL,
    TaskName               NVARCHAR2(50)     DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ServiceType            VARCHAR2(50)      DEFAULT '',
    IsSuccess              VARCHAR2(18)      DEFAULT '',
    Action                 NVARCHAR2(50)     DEFAULT '',
    Message                NVARCHAR2(255)    DEFAULT '',
    StackTrace             NCLOB             DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_TaskLog PRIMARY KEY (ID)
)
GO



CREATE TABLE siteserver_Template(
    TemplateID             NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    NOT NULL,
    TemplateName           VARCHAR2(50)     DEFAULT '',
    TemplateType           VARCHAR2(50)     DEFAULT '',
    RelatedFileName        VARCHAR2(50)     DEFAULT '',
    CreatedFileFullName    VARCHAR2(50)     DEFAULT '',
    CreatedFileExtName     VARCHAR2(50)     DEFAULT '',
    RuleID                 NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Charset                VARCHAR2(50)     DEFAULT '',
    IsDefault              VARCHAR2(18)     DEFAULT '',
    CONSTRAINT PK_siteserver_Template PRIMARY KEY (TemplateID)
)
GO



CREATE TABLE siteserver_TemplateMatch(
    NodeID                 NUMBER(38, 0)    NOT NULL,
    RuleID                 NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ChannelTemplateID      NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ContentTemplateID      NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    FilePath               VARCHAR2(200)    DEFAULT '',
    ChannelFilePathRule    VARCHAR2(200)    DEFAULT '',
    ContentFilePathRule    VARCHAR2(200)    DEFAULT '',
    CONSTRAINT PK_siteserver_TemplateMatch PRIMARY KEY (RuleID, NodeID)
)
GO



CREATE TABLE siteserver_TemplateRule(
    RuleID                 NUMBER(38, 0)    NOT NULL,
    RuleName               NVARCHAR2(50)    DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsCreateChannels       VARCHAR2(18)     DEFAULT '',
    IsCreateContents       VARCHAR2(18)     DEFAULT '',
    IndexTemplateID        NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ChannelTemplateID      NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ContentTemplateID      NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    ChannelFilePathRule    VARCHAR2(200)    DEFAULT '',
    ContentFilePathRule    VARCHAR2(200)    DEFAULT '',
    CONSTRAINT PK_siteserver_TemplateRule PRIMARY KEY (RuleID)
)
GO



CREATE TABLE siteserver_Tracking(
    TrackingID             NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    TrackerType            VARCHAR2(50)      DEFAULT '',
    LastAccessDateTime     TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    PageUrl                VARCHAR2(200)     DEFAULT '',
    PageNodeID             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    PageContentID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Referrer               VARCHAR2(200)     DEFAULT '',
    IPAddress              VARCHAR2(200)     DEFAULT '',
    OperatingSystem        VARCHAR2(200)     DEFAULT '',
    Browser                VARCHAR2(200)     DEFAULT '',
    AccessDateTime         TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_siteserver_Tracking PRIMARY KEY (TrackingID)
)
GO



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



CREATE TABLE siteserver_Users(
    UserName       NVARCHAR2(255)    NOT NULL,
    GroupID        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Credits        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SettingsXML    NCLOB             DEFAULT '',
    CONSTRAINT PK_siteserver_Users PRIMARY KEY (UserName)
)
GO



CREATE INDEX IX_siteserver_MD_Node ON siteserver_MenuDisplay(PublishmentSystemID)
GO
CREATE INDEX IX_siteserver_Node_P ON siteserver_Node(PublishmentSystemID)
GO
CREATE INDEX IX_siteserver_Node_Taxis ON siteserver_Node(Taxis)
GO
CREATE INDEX IX_siteserver_SM_N ON siteserver_SeoMeta(PublishmentSystemID)
GO
CREATE INDEX IX_siteserver_Template_Node ON siteserver_Template(PublishmentSystemID)
GO
CREATE INDEX IX_siteserver_Template_TT ON siteserver_Template(TemplateType)
GO
CREATE INDEX IX_siteserver_Tracking_P ON siteserver_Tracking(PublishmentSystemID)
GO
CREATE INDEX IX_siteserver_Tracking_Page ON siteserver_Tracking(PageNodeID, PageContentID)
GO
ALTER TABLE siteserver_GatherDatabaseRule ADD CONSTRAINT FK_siteserver_GDR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_GatherFileRule ADD CONSTRAINT FK_siteserver_GFR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_GatherRule ADD CONSTRAINT FK_siteserver_GR_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_InputContent ADD CONSTRAINT FK_siteserver_IC_I 
    FOREIGN KEY (InputID)
    REFERENCES siteserver_Input(InputID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_MenuDisplay ADD CONSTRAINT FK_siteserver_MD_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_PagePermissions ADD CONSTRAINT FK_siteserver_PP_N 
    FOREIGN KEY (NodeID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_PublishmentSystem ADD CONSTRAINT FK_siteserver_PS_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_RelatedFieldItem ADD CONSTRAINT FK_siteserver_RFI_RF 
    FOREIGN KEY (RelatedFieldID)
    REFERENCES siteserver_RelatedField(RelatedFieldID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_SeoMeta ADD CONSTRAINT FK_siteserver_SM_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_SeoMetasInNodes ADD CONSTRAINT FK_siteserver_SMInN_SM 
    FOREIGN KEY (SeoMetaID)
    REFERENCES siteserver_SeoMeta(SeoMetaID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_SystemPermissions ADD CONSTRAINT FK_siteserver_SP_N 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_Template ADD CONSTRAINT FK_siteserver_Template_Node 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_TemplateMatch ADD CONSTRAINT FK_siteserver_TM_N 
    FOREIGN KEY (NodeID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO

ALTER TABLE siteserver_TemplateMatch ADD CONSTRAINT FK_siteserver_TM_TR 
    FOREIGN KEY (RuleID)
    REFERENCES siteserver_TemplateRule(RuleID) ON DELETE CASCADE
GO


ALTER TABLE siteserver_Tracking ADD CONSTRAINT FK_siteserver_Tracking_Node 
    FOREIGN KEY (PublishmentSystemID)
    REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE
GO


