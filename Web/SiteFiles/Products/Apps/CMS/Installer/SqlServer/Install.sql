/*
* ER/Studio 8.0 SQL Code Generation
* Company :      BaiRong Software
* Project :      SiteServer WCM
* Author :       BaiRong Software
*
* Date Created : Monday, July 28, 2014 09:55:07
* Target DBMS : Microsoft SQL Server 2000
*/

CREATE TABLE siteserver_AdArea(
AdAreaID               int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
AdAreaName             nvarchar(50)     DEFAULT '' NOT NULL,
Width                  int              DEFAULT 0 NOT NULL,
Height                 int              DEFAULT 0 NOT NULL,
Summary                nvarchar(255)    DEFAULT '' NOT NULL,
IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_AdArea PRIMARY KEY NONCLUSTERED (AdAreaID)
)
go



CREATE TABLE siteserver_AdMaterial(
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
CONSTRAINT PK_siteserver_AdMaterial PRIMARY KEY NONCLUSTERED (AdMaterialID)
)
go



CREATE TABLE siteserver_Adv(
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
NodeIDCollectionToChannel    nvarchar(4000)    DEFAULT '' NOT NULL,
NodeIDCollectionToContent    nvarchar(4000)    DEFAULT '' NOT NULL,
FileTemplateIDCollection     nvarchar(4000)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Adv PRIMARY KEY NONCLUSTERED (AdvID)
)
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



CREATE TABLE siteserver_Comment(
CommentID              int             IDENTITY(1,1),
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
NodeID                 int             DEFAULT 0 NOT NULL,
ContentID              int             DEFAULT 0 NOT NULL,
Good                   int             DEFAULT 0 NOT NULL,
UserName               nvarchar(50)    DEFAULT '' NOT NULL,
IPAddress              varchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
IsChecked              varchar(18)     DEFAULT '' NOT NULL,
IsRecommend            varchar(18)     DEFAULT '' NOT NULL,
Content                ntext           DEFAULT '' NOT NULL,
AdminName nvarchar(255) DEFAULT ''  NOT NULL,
CONSTRAINT PK_siteserver_Comment PRIMARY KEY NONCLUSTERED (CommentID)
)
go



CREATE TABLE siteserver_CommentContent(
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
CONSTRAINT PK_siteserver_CommentContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_ContentGroup(
ContentGroupName       nvarchar(255)    NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
Taxis                  int              DEFAULT 0 NOT NULL,
Description            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_ContentGroup PRIMARY KEY CLUSTERED (ContentGroupName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_GatherDatabaseRule(
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
IsAutoCreate           varchar(18)      DEFAULT '' NOT NULL,
IsOrderByDesc          varchar(18)      DEFAULT '' NOT NULL,
LastGatherDate         datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_GatherDR PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_GatherFileRule(
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
IsAutoCreate                     varchar(18)      DEFAULT '' NOT NULL,
ContentExclude                   ntext            DEFAULT '' NOT NULL,
ContentHtmlClearCollection       nvarchar(255)    DEFAULT '' NOT NULL,
ContentHtmlClearTagCollection    nvarchar(255)    DEFAULT '' NOT NULL,
ContentTitleStart                ntext            DEFAULT '' NOT NULL,
ContentTitleEnd                  ntext            DEFAULT '' NOT NULL,
ContentContentStart              ntext            DEFAULT '' NOT NULL,
ContentContentEnd                ntext            DEFAULT '' NOT NULL,
ContentAttributes                ntext            DEFAULT '' NOT NULL,
ContentAttributesXML             ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_GatherFileRule PRIMARY KEY NONCLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_GatherRule(
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
CONSTRAINT PK_siteserver_GatherRule PRIMARY KEY CLUSTERED (GatherRuleName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_InnerLink(
InnerLinkName          nvarchar(255)    NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
LinkUrl                varchar(200)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_InnerLink PRIMARY KEY NONCLUSTERED (InnerLinkName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_Input(
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
ClassifyID			   int             DEFAULT 0  NOT NULL,---- by 20151029 sofuny
CONSTRAINT PK_siteserver_Input PRIMARY KEY NONCLUSTERED (InputID)
)
go

---- by 20151029 sofuny  表单分类
CREATE TABLE siteserver_InputClassify(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_InputClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go



CREATE TABLE siteserver_InputContent(
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
CONSTRAINT PK_siteserver_InputContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_Keyword(
KeywordID      int             IDENTITY(1,1),
Keyword        nvarchar(50)    DEFAULT '' NOT NULL,
Alternative    nvarchar(50)    DEFAULT '' NOT NULL,
Grade          nvarchar(50)    DEFAULT '' NOT NULL,
ClassifyID     int             DEFAULT 0  NOT NULL,
CONSTRAINT PK_siteserver_Keyword PRIMARY KEY NONCLUSTERED (KeywordID)
)
go


CREATE TABLE siteserver_KeywordClassify(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_KeywordClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go





CREATE TABLE siteserver_Log(
ID                     int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
ChannelID              int              DEFAULT 0 NOT NULL,
ContentID              int              DEFAULT 0 NOT NULL,
UserName               varchar(50)      DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
Action                 nvarchar(255)    DEFAULT '' NOT NULL,
Summary                nvarchar(255)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Log PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_MailSendLog(
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
CONSTRAINT PK_siteserver_MailSendLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_MailSubscribe(
ID                     int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
Receiver               nvarchar(255)    DEFAULT '' NOT NULL,
Mail                   nvarchar(255)    DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_MailSubscribe PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_MenuDisplay(
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
CONSTRAINT PK_siteserver_MenuDisplay PRIMARY KEY CLUSTERED (MenuDisplayID)
)
go



CREATE TABLE siteserver_Node(
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
CONSTRAINT PK_siteserver_Node PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE siteserver_NodeGroup(
NodeGroupName          nvarchar(255)    NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
Taxis                  int              DEFAULT 0 NOT NULL,
Description            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_NodeGroup PRIMARY KEY CLUSTERED (NodeGroupName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_Photo(
ID                     int             IDENTITY(1,1),
PublishmentSystemID    int             NOT NULL,
ContentID              int             NOT NULL,
SmallUrl               varchar(200)    NOT NULL,
MiddleUrl              varchar(200)    NOT NULL,
LargeUrl               varchar(200)    NOT NULL,
Taxis                  int             NOT NULL,
Description            varchar(255)    NOT NULL,
CONSTRAINT PK_siteserver_Photo PRIMARY KEY NONCLUSTERED (ID)
)
go

CREATE TABLE siteserver_Teleplay(
ID                     int             IDENTITY(1,1),
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
ContentID              int             DEFAULT 0 NOT NULL,
StillUrl               varchar(500)    DEFAULT 0 NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
Description            ntext           DEFAULT '' NOT NULL,
Title                  nvarchar(255)   DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Teleplay PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_PublishmentSystem(
PublishmentSystemID             int              NOT NULL,
PublishmentSystemName           nvarchar(50)     DEFAULT '' NOT NULL,
PublishmentSystemType           varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForContent        varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForGoods          varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForBrand          varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForGovPublic      varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForGovInteract    varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForVote           varchar(50)      DEFAULT '' NOT NULL,
AuxiliaryTableForJob            varchar(50)      DEFAULT '' NOT NULL,
IsCheckContentUseLevel          varchar(18)      DEFAULT '' NOT NULL,
CheckContentLevel               int              DEFAULT 0 NOT NULL,
PublishmentSystemDir            varchar(50)      DEFAULT '' NOT NULL,
PublishmentSystemUrl            varchar(200)     DEFAULT '' NOT NULL,
IsHeadquarters                  varchar(18)      DEFAULT '' NOT NULL,
ParentPublishmentSystemID       int              DEFAULT 0 NOT NULL,
GroupSN                         nvarchar(255)    DEFAULT '' NOT NULL,
Taxis                           int              DEFAULT 0 NOT NULL,
SettingsXML                     ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_PS PRIMARY KEY CLUSTERED (PublishmentSystemID)
)
go



CREATE TABLE siteserver_RelatedField(
RelatedFieldID         int              IDENTITY(1,1),
RelatedFieldName       nvarchar(50)     DEFAULT '' NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
TotalLevel             int              DEFAULT 0 NOT NULL,
Prefixes               nvarchar(255)    DEFAULT '' NOT NULL,
Suffixes               nvarchar(255)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_RelatedField PRIMARY KEY NONCLUSTERED (RelatedFieldID)
)
go



CREATE TABLE siteserver_RelatedFieldItem(
ID                int              IDENTITY(1,1),
RelatedFieldID    int              NOT NULL,
ItemName          nvarchar(255)    DEFAULT '' NOT NULL,
ItemValue         nvarchar(255)    DEFAULT '' NOT NULL,
ParentID          int              DEFAULT 0 NOT NULL,
Taxis             int              DEFAULT 0 NOT NULL,
CONSTRAINT PK_siteserver_RelatedFieldItem PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_ResumeContent(
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
CONSTRAINT PK_siteserver_ResumeContent PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_SeoMeta(
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
CONSTRAINT PK_siteserver_SeoMeta PRIMARY KEY CLUSTERED (SeoMetaID)
)
go



CREATE TABLE siteserver_SeoMetasInNodes(
NodeID                 int            NOT NULL,
IsChannel              varchar(18)    DEFAULT '' NOT NULL,
SeoMetaID              int            NOT NULL,
PublishmentSystemID    int            NOT NULL,
CONSTRAINT PK_siteserver_SeoMetasInNodes PRIMARY KEY CLUSTERED (NodeID, IsChannel, SeoMetaID)
)
go



CREATE TABLE siteserver_SigninLog(
ID                     int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
ContentID              int              DEFAULT 0 NOT NULL,
UserName               nvarchar(255)    DEFAULT '' NOT NULL,
IsSignin               varchar(18)      DEFAULT '' NOT NULL,
SigninDate             datetime         DEFAULT getdate() NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SigninLog PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_SigninSetting(
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
CONSTRAINT PK_siteserver_SigninSetting PRIMARY KEY CLUSTERED (ID)
)
go



CREATE TABLE siteserver_SigninUserContentID(
ID                     int              IDENTITY(1,1),
IsGroup                varchar(18)      DEFAULT '' NOT NULL,
GroupID                int              DEFAULT 0 NOT NULL,
UserName               nvarchar(255)    DEFAULT '' NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL,
ContentIDCollection    varchar(500)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SigninUserContentID PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_Star(
StarID                 int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
ChannelID              int              DEFAULT 0 NOT NULL,
ContentID              int              DEFAULT 0 NOT NULL,
UserName               nvarchar(255)    DEFAULT '' NOT NULL,
Point                  int              DEFAULT 0 NOT NULL,
Message                nvarchar(255)    DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_Star PRIMARY KEY NONCLUSTERED (StarID)
)
go



CREATE TABLE siteserver_StarSetting(
StarSettingID          int               IDENTITY(1,1),
PublishmentSystemID    int               DEFAULT 0 NOT NULL,
ChannelID              int               DEFAULT 0 NOT NULL,
ContentID              int               DEFAULT 0 NOT NULL,
TotalCount             int               DEFAULT 0 NOT NULL,
PointAverage           decimal(18, 1)    DEFAULT 0 NOT NULL,
CONSTRAINT PK_siteserver_StarSetting PRIMARY KEY NONCLUSTERED (StarSettingID)
)
go



CREATE TABLE siteserver_StlTag(
TagName                nvarchar(50)     NOT NULL,
PublishmentSystemID    int              NOT NULL,
TagDescription         nvarchar(255)    DEFAULT '' NOT NULL,
TagContent             ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_StlTag PRIMARY KEY NONCLUSTERED (TagName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_SystemPermissions(
RoleName               nvarchar(255)    NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeIDCollection       text             DEFAULT '' NOT NULL,
ChannelPermissions     text             DEFAULT '' NOT NULL,
WebsitePermissions     text             DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SP PRIMARY KEY CLUSTERED (RoleName, PublishmentSystemID)
)
go



CREATE TABLE siteserver_TagStyle(
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
CONSTRAINT PK_siteserver_TagStyle PRIMARY KEY CLUSTERED (StyleID)
)
go



CREATE TABLE siteserver_Template(
TemplateID             int             IDENTITY(1,1),
PublishmentSystemID    int             NOT NULL,
TemplateName           nvarchar(50)    DEFAULT '' NOT NULL,
TemplateType           varchar(50)     DEFAULT '' NOT NULL,
RelatedFileName        nvarchar(50)    DEFAULT '' NOT NULL,
CreatedFileFullName    nvarchar(50)    DEFAULT '' NOT NULL,
CreatedFileExtName     varchar(50)     DEFAULT '' NOT NULL,
Charset                varchar(50)     DEFAULT '' NOT NULL,
IsDefault              varchar(18)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Template PRIMARY KEY CLUSTERED (TemplateID)
)
go



CREATE TABLE siteserver_TemplateLog(
ID                     int              IDENTITY(1,1),
TemplateID             int              NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
AddUserName            nvarchar(255)    DEFAULT '' NOT NULL,
ContentLength          int              DEFAULT 0 NOT NULL,
TemplateContent        ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_TemplateLog PRIMARY KEY NONCLUSTERED (ID)
)
go



CREATE TABLE siteserver_TemplateMatch(
NodeID                 int             NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
ChannelTemplateID      int             DEFAULT 0 NOT NULL,
ContentTemplateID      int             DEFAULT 0 NOT NULL,
FilePath               varchar(200)    DEFAULT '' NOT NULL,
ChannelFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
ContentFilePathRule    varchar(200)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_TemplateMatch PRIMARY KEY NONCLUSTERED (NodeID)
)
go



CREATE TABLE siteserver_Tracking(
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
CONSTRAINT PK_siteserver_Tracking PRIMARY KEY CLUSTERED (TrackingID)
)
go



CREATE TABLE siteserver_UserGroup(
GroupID            int             IDENTITY(1,1),
GroupName          nvarchar(50)    DEFAULT '' NOT NULL,
IsCredits          varchar(18)     DEFAULT '' NOT NULL,
CreditsFrom        int             DEFAULT 0 NOT NULL,
CreditsTo          int             DEFAULT 0 NOT NULL,
Stars              int             DEFAULT 0 NOT NULL,
Color              varchar(10)     DEFAULT '' NOT NULL,
Rank               int             DEFAULT 0 NOT NULL,
UserPermissions    ntext           DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_UserGroup PRIMARY KEY NONCLUSTERED (GroupID)
)
go



CREATE TABLE siteserver_Users(
UserName       nvarchar(255)    NOT NULL,
GroupID        int              DEFAULT 0 NOT NULL,
Credits        int              DEFAULT 0 NOT NULL,
SettingsXML    ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Users PRIMARY KEY NONCLUSTERED (UserName)
)
go



CREATE TABLE siteserver_VoteOperation(
OperationID            int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL,
ContentID              int              DEFAULT 0 NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
UserName               nvarchar(255)    DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_VoteOperation PRIMARY KEY NONCLUSTERED (OperationID)
)
go



CREATE TABLE siteserver_VoteOption(
OptionID               int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL,
ContentID              int              DEFAULT 0 NOT NULL,
Title                  nvarchar(255)    DEFAULT '' NOT NULL,
ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
NavigationUrl          varchar(200)     DEFAULT '' NOT NULL,
VoteNum                int              DEFAULT 0 NOT NULL,
CONSTRAINT PK_siteserver_VoteOption PRIMARY KEY NONCLUSTERED (OptionID)
)
go

CREATE TABLE siteserver_ThirdLogin(
ID                     int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
ThirdLoginType         varchar(50)      DEFAULT 0 NOT NULL,
ThirdLoginName         nvarchar(50)     DEFAULT '' NOT NULL,
IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
Taxis                  int              DEFAULT 0 NOT NULL,
Description            nvarchar(255)    DEFAULT '' NOT NULL,
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_ThirdLogin PRIMARY KEY NONCLUSTERED (ID)
)
go

--网站留言
CREATE TABLE siteserver_WebsiteMessage(
WebsiteMessageID         int             IDENTITY(1,1),
WebsiteMessageName       nvarchar(50)    DEFAULT '' NOT NULL,
PublishmentSystemID      int             DEFAULT 0 NOT NULL,
AddDate                  datetime        DEFAULT getdate() NOT NULL,
IsChecked                varchar(18)     DEFAULT '' NOT NULL,
IsReply                  varchar(18)     DEFAULT '' NOT NULL,
Taxis                    int             DEFAULT 0 NOT NULL,
IsTemplate               varchar(18)     DEFAULT '' NOT NULL,
StyleTemplate            ntext           DEFAULT '' NOT NULL,
ScriptTemplate           ntext           DEFAULT '' NOT NULL,
ContentTemplate          ntext           DEFAULT '' NOT NULL,
IsTemplateList           varchar(18)     DEFAULT '' NOT NULL,
StyleTemplateList        ntext           DEFAULT '' NOT NULL,
ScriptTemplateList       ntext           DEFAULT '' NOT NULL,
ContentTemplateList      ntext           DEFAULT '' NOT NULL,
IsTemplateDetail         varchar(18)     DEFAULT '' NOT NULL,
StyleTemplateDetail      ntext           DEFAULT '' NOT NULL,
ScriptTemplateDetail     ntext           DEFAULT '' NOT NULL,
ContentTemplateDetail    ntext           DEFAULT '' NOT NULL,
SettingsXML              ntext           DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_WebsiteMessage PRIMARY KEY NONCLUSTERED (WebsiteMessageID)
)
go

--网站留言分类
CREATE TABLE siteserver_WebsiteMessageClassify(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_WebsiteMessageClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go

--网站留言内容
CREATE TABLE siteserver_WebsiteMessageContent(
ID                  int              IDENTITY(1,1),
WebsiteMessageID    int              NOT NULL,
Taxis               int              DEFAULT 0 NOT NULL,
IsChecked           varchar(18)      DEFAULT '' NOT NULL,
UserName            nvarchar(255)    DEFAULT '' NOT NULL,
IPAddress           varchar(50)      DEFAULT '' NOT NULL,
Location            nvarchar(50)     DEFAULT '' NOT NULL,
AddDate             datetime         DEFAULT getdate() NOT NULL,
Reply               ntext            DEFAULT '' NOT NULL,
ClassifyID          int              DEFAULT 0 NOT NULL,
	Name                nvarchar(50)     DEFAULT '' NOT NULL,
	Phone               nvarchar(50)     DEFAULT '' NOT NULL,
	Email               nvarchar(50)     DEFAULT '' NOT NULL,
	Question            nvarchar(200)    DEFAULT '' NOT NULL,
	Description         ntext            DEFAULT '' NOT NULL,
	Ext1                nvarchar(50)     DEFAULT '' NOT NULL,
	Ext2                nvarchar(50)     DEFAULT '' NOT NULL,
	Ext3                nvarchar(50)     DEFAULT '' NOT NULL,
SettingsXML         ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_WebsiteMessageContent PRIMARY KEY NONCLUSTERED (ID)
)
go

--网站留言回复模板
CREATE TABLE siteserver_WebsiteMessageReplayTemplate(
ID                 int             IDENTITY(1,1),
TemplateTitle      nvarchar(50)    DEFAULT '' NOT NULL,
TemplateContent    ntext           DEFAULT '' NOT NULL,
IsEnabled          varchar(18)     DEFAULT '' NOT NULL,
ClassifyID         int             DEFAULT 0 NOT NULL,
AddDate         datetime     DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_WebsiteMessageReplayTemplate PRIMARY KEY NONCLUSTERED (ID)
)
go



---- by 20151110 sofuny  订阅信息数据表
-----订阅内容
CREATE TABLE siteserver_Subscribe(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
SubscribeValue         nvarchar(200)   DEFAULT '' NOT NULL,
ContentType	           nvarchar(200)   DEFAULT '' NOT NULL,
SubscribeNum           int             DEFAULT 0 NOT NULL,
IPAddress	           nvarchar(255)   DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
UserName	           nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL, 
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Subscribe PRIMARY KEY NONCLUSTERED (ItemID)
)
go
CREATE CLUSTERED INDEX IX_siteserver_Subscribe_Taxis ON siteserver_Subscribe(Taxis)
go


-----订阅内容设置
CREATE TABLE siteserver_SubscribeSet(
SubscribeSetID         int              IDENTITY(1,1),
EmailContentAddress    nvarchar(200)    DEFAULT '' NOT NULL,
MobileContentAddress   nvarchar(200)    DEFAULT '' NOT NULL,
PushType               nvarchar(50)     DEFAULT '' NOT NULL,
PushDate               int              DEFAULT 0 NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
StyleTemplate          ntext		DEFAULT '' NOT NULL,
ScriptTemplate         ntext		DEFAULT '' NOT NULL,
ContentTemplate        ntext		DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SubscribeSet PRIMARY KEY NONCLUSTERED (SubscribeSetID)
)
go

-----订阅会员
CREATE TABLE siteserver_SubscribeUser(
SubscribeUserID        int              IDENTITY(1,1),
Email                  nvarchar(200)    DEFAULT '' NOT NULL,
UserID                 int              DEFAULT 0 NOT NULL,
SubscribeName          nvarchar(200)    DEFAULT '' NOT NULL,
Mobile                 nvarchar(20)     DEFAULT '' NOT NULL,
IPAddress	           nvarchar(255)    DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
PushNum                int              DEFAULT 0 NOT NULL,
SubscribeStatu         varchar(18)      DEFAULT '' NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
CONSTRAINT PK_siteserver_SubscribeUser PRIMARY KEY NONCLUSTERED (SubscribeUserID)
)
go

-----订阅内容推送记录
CREATE TABLE siteserver_SubscribePushRecord(
RecordID               int              IDENTITY(1,1),
Email                  nvarchar(200)    DEFAULT '' NOT NULL,
UserID                 int              DEFAULT 0 NOT NULL,
Mobile                 nvarchar(20)     DEFAULT '' NOT NULL,
SubscribeName          nvarchar(200)    DEFAULT '' NOT NULL,
SubscriptionTemplate   nvarchar(200)    DEFAULT '' NOT NULL,
SubscribeSendRecordID  int              DEFAULT 0 NOT NULL,
PushType               varchar(18)      DEFAULT '' NOT NULL,
PushStatu              varchar(18)      DEFAULT '' NOT NULL,
TaskID                 int              DEFAULT 0 NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SubscribePushRecord PRIMARY KEY NONCLUSTERED (RecordID)
)
go


--搜索关键字
CREATE TABLE siteserver_Searchword(
    ID         int             IDENTITY(1,1),
	PublishmentSystemID       int               DEFAULT 0 NOT NULL,
    Searchword           nvarchar(50)    DEFAULT '' NOT NULL,
    SearchResultCount    int             DEFAULT 0 NOT NULL,
    SearchCount          int             DEFAULT 0 NOT NULL,
    AddDate              datetime        DEFAULT getdate() NOT NULL,
    IsEnabled            varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Searchword PRIMARY KEY NONCLUSTERED (ID)
)
go

--搜索关键字设置
CREATE TABLE siteserver_SearchwordSetting(
    ID                        int               IDENTITY(1,1),
    PublishmentSystemID       int               DEFAULT 0 NOT NULL,
	IsAllow						varchar(18)			DEFAULT '' NOT NULL,
    InNode                    nvarchar(4000)    DEFAULT '' NOT NULL,
    NotInNode                 nvarchar(4000)    DEFAULT '' NOT NULL,
    SearchResultCountLimit    int               DEFAULT 0 NOT NULL,
    SearchCountLimit          int               DEFAULT 0 NOT NULL,
    SearchOutputLimit         int               DEFAULT 0 NOT NULL,
    SearchSort                varchar(50)       DEFAULT '' NOT NULL,
	IsTemplate               varchar(18)         DEFAULT '' NOT NULL,
	StyleTemplate           ntext					DEFAULT '' NOT NULL,
	ScriptTemplate           ntext					DEFAULT '' NOT NULL,
	ContentTemplate           ntext					DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_SearchwordSetting PRIMARY KEY NONCLUSTERED (ID)
)
go

--专题分类
CREATE TABLE siteserver_SpecialClassify(
    ItemID                 int             IDENTITY(1,1),
    ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
    ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
    ParentID               int             DEFAULT 0 NOT NULL,
    ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
    ParentsCount           int             DEFAULT 0 NOT NULL,
    ChildrenCount          int             DEFAULT 0 NOT NULL,
    ContentNum             int             DEFAULT 0 NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Enabled                varchar(18)     DEFAULT '' NOT NULL,
    IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    AddDate                datetime        DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_SpecialClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go


--专题内容
CREATE TABLE siteserver_SpecialContent(
    ID              int              IDENTITY(1,1),
	PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Taxis           int              DEFAULT 0 NOT NULL,
    IsChecked       varchar(18)      DEFAULT '' NOT NULL,
    AddUser         nvarchar(50)     DEFAULT '' NOT NULL,
    AddDate         datetime         DEFAULT getdate() NOT NULL,
    LaseEditUser    nvarchar(50)     DEFAULT '' NOT NULL,
    LastEditDate    datetime         DEFAULT getdate() NOT NULL,
    SettingsXML     ntext            DEFAULT '' NOT NULL,
    SpecialName     nvarchar(50)     DEFAULT '' NOT NULL,
    Description     nvarchar(255)    DEFAULT '' NOT NULL,
    ClassifyID      int              DEFAULT 0 NOT NULL,
    SpecialPath     nvarchar(500)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_SpecialContent PRIMARY KEY NONCLUSTERED (ID)
)
go

--广告分类
CREATE TABLE siteserver_AdvImageClassify(
    ItemID                 int             IDENTITY(1,1),
    ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
    ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
    ParentID               int             DEFAULT 0 NOT NULL,
    ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
    ParentsCount           int             DEFAULT 0 NOT NULL,
    ChildrenCount          int             DEFAULT 0 NOT NULL,
    ContentNum             int             DEFAULT 0 NOT NULL,
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Enabled                varchar(18)     DEFAULT '' NOT NULL,
    IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    AddDate                datetime        DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_AdvImageClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go

--广告内容
CREATE TABLE siteserver_AdvImageContent(
    ID              int              IDENTITY(1,1),
	PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    Taxis           int              DEFAULT 0 NOT NULL,
    IsChecked       varchar(18)      DEFAULT '' NOT NULL,
    AddUser         nvarchar(50)     DEFAULT '' NOT NULL,
    AddDate         datetime         DEFAULT getdate() NOT NULL,
    LaseEditUser    nvarchar(50)     DEFAULT '' NOT NULL,
    LastEditDate    datetime         DEFAULT getdate() NOT NULL,
    SettingsXML     ntext            DEFAULT '' NOT NULL,
    AdvImageName     nvarchar(50)     DEFAULT '' NOT NULL,
    Description     nvarchar(255)    DEFAULT '' NOT NULL,
    ClassifyID      int              DEFAULT 0 NOT NULL,
    AdvImagePath     nvarchar(500)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_AdvImageContent PRIMARY KEY NONCLUSTERED (ID)
)
go
 

---by 20151124 智能推送
----会员浏览统计表
CREATE TABLE siteserver_ViewsStatistics(
ID                     int              IDENTITY(1,1),
UserID                 int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL,
StasCount              int              DEFAULT 0 NOT NULL,
StasYear               varchar(10)      DEFAULT 0 NOT NULL,
StasMonth              varchar(2)       DEFAULT 0 NOT NULL, 
PublishmentSystemID    int              DEFAULT 0 NOT NULL, 
AddDate                datetime         DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_ViewsStatistics PRIMARY KEY NONCLUSTERED (ID)
)
go 


----分支机构管理表 
-----分支机构分类
CREATE TABLE siteserver_OrganizationClassify(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_OrganizationClassify PRIMARY KEY NONCLUSTERED (ItemID)
)
go
CREATE CLUSTERED INDEX IX_siteserver_OrganizationClassify_Taxis ON siteserver_OrganizationClassify(Taxis)
go

-----分支机构区域
CREATE TABLE siteserver_OrganizationArea(
ItemID                 int             IDENTITY(1,1),
ItemName               nvarchar(50)    DEFAULT '' NOT NULL,
ItemIndexName          nvarchar(50)    DEFAULT '' NOT NULL,
ParentID               int             DEFAULT 0 NOT NULL,
ParentsPath            varchar(255)    DEFAULT '' NOT NULL,
ParentsCount           int             DEFAULT 0 NOT NULL,
ChildrenCount          int             DEFAULT 0 NOT NULL,
ContentNum             int             DEFAULT 0 NOT NULL,
ClassifyID     	       int             DEFAULT 0 NOT NULL,
PublishmentSystemID    int             DEFAULT 0 NOT NULL,
Enabled                varchar(18)     DEFAULT '' NOT NULL,
IsLastItem             varchar(18)     DEFAULT '' NOT NULL,
Taxis                  int             DEFAULT 0 NOT NULL,
AddDate                datetime        DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_OrganizationArea PRIMARY KEY NONCLUSTERED (ItemID)
)
go
CREATE CLUSTERED INDEX IX_siteserver_OrganizationArea_Taxis ON siteserver_OrganizationArea(Taxis)
go

----分支机构信息表
CREATE TABLE siteserver_OrganizationInfo(
ID                     int              IDENTITY(1,1),
ClassifyID             int              DEFAULT 0 NOT NULL,
OrganizationName       nvarchar(200)    DEFAULT '' NOT NULL,
AreaID                 int              DEFAULT 0 NOT NULL,
OrganizationAddress    nvarchar(255)    DEFAULT '' NOT NULL,
Explain                nvarchar(255)    DEFAULT '' NOT NULL,
Phone                  nvarchar(100)    DEFAULT '' NOT NULL,
Longitude              nvarchar(50)     DEFAULT '' NOT NULL,
Latitude               nvarchar(50)     DEFAULT '' NOT NULL,
LogoUrl                nvarchar(255)    DEFAULT '' NOT NULL,
ContentNum             int              DEFAULT 0 NOT NULL,
Enabled                varchar(18)      DEFAULT '' NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
Taxis                  int              DEFAULT 0 NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_OrganizationInfo PRIMARY KEY NONCLUSTERED (ID)
)
go
CREATE CLUSTERED INDEX IX_siteserver_OrganizationInfo_Taxis ON siteserver_OrganizationInfo(Taxis)
go



----评价管理-------
--评价内容
CREATE TABLE siteserver_EvaluationContent(
ECID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL, 
Taxis                  int              DEFAULT 0 NOT NULL,
IsChecked              varchar(18)      DEFAULT '' NOT NULL,
UserName               varchar(50)      DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
Location               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
Reply                  ntext            DEFAULT '' NOT NULL,
ClassifyID             int              DEFAULT 0 NOT NULL,
Description            nvarchar(500)    DEFAULT '' NOT NULL,
CompositeScore         int              DEFAULT 0 NOT NULL, 
Ext1                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext2                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext3                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext4                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext5                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext6                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext7                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext8                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext9                   nvarchar(50)     DEFAULT '' NOT NULL,
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_EvaluationContent PRIMARY KEY NONCLUSTERED (ECID)
)
go

CREATE CLUSTERED INDEX IX_siteserver_EvaluationContent_Taxis ON siteserver_EvaluationContent(Taxis)
go

--功能内容字段
CREATE TABLE siteserver_FunctionTableStyles(
FTID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL, 
TableStyleID           int              DEFAULT 0 NOT NULL,
TableStyle             varchar(18)      DEFAULT '' NOT NULL, 
Enabled                varchar(18)      DEFAULT '' NOT NULL, 
AddDate                datetime         DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_FunctionTableStyles PRIMARY KEY NONCLUSTERED (FTID)
)
go 

--功能操作记录
CREATE TABLE siteserver_ConsoleLog(
CLID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL,
TableName              nvarchar(50)     DEFAULT '' NOT NULL,
SourceID               int              DEFAULT 0 NOT NULL,  
TargetDesc             nvarchar(500)    DEFAULT '' NOT NULL,
ActionType             varchar(18)      DEFAULT '' NOT NULL, 
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
RedirectUrl            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_ConsoleLog PRIMARY KEY NONCLUSTERED (CLID)
)
go 


-----试用管理---
--试用申请
CREATE TABLE siteserver_TrialApply(
TAID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL, 
Taxis                  int              DEFAULT 0 NOT NULL,
ApplyStatus            varchar(18)      DEFAULT '' NOT NULL,
IsChecked              varchar(18)      DEFAULT '' NOT NULL,
CheckAdmin             nvarchar(50)     DEFAULT '' NOT NULL,
CheckDate              datetime         DEFAULT getdate() NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
Location               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
IsReport               varchar(18)      DEFAULT '' NOT NULL,  
IsEmail                varchar(18)      DEFAULT '' NOT NULL,  
IsMobile               varchar(18)      DEFAULT '' NOT NULL,  
Name                   nvarchar(50)     DEFAULT '' NOT NULL,
Phone                  nvarchar(50)     DEFAULT '' NOT NULL, 
Ext1                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext2                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext3                   nvarchar(50)     DEFAULT '' NOT NULL, 
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_TrialApply PRIMARY KEY NONCLUSTERED (TAID)
)
go

CREATE CLUSTERED INDEX IX_siteserver_TrialApply_Taxis ON siteserver_TrialApply(Taxis)
go

--试用报告
CREATE TABLE siteserver_TrialReport(
TRID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL,  
TAID                   int              DEFAULT 0 NOT NULL, 
Taxis                  int              DEFAULT 0 NOT NULL,
ReportStatus           varchar(18)      DEFAULT '' NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
AdminName              nvarchar(50)     DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
Location               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
Reply                  ntext            DEFAULT '' NOT NULL, 
Description            nvarchar(500)    DEFAULT '' NOT NULL,
CompositeScore         int              DEFAULT 0 NOT NULL, 
Ext1                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext2                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext3                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext4                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext5                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext6                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext7                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext8                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext9                   nvarchar(50)     DEFAULT '' NOT NULL,
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_TrialReport PRIMARY KEY NONCLUSTERED (TRID)
)
go

CREATE CLUSTERED INDEX IX_siteserver_TrialReport_Taxis ON siteserver_TrialReport(Taxis)
go


----调查问卷管理
--调查问卷
CREATE TABLE siteserver_SurveyQuestionnaire(
SQID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL,   
Taxis                  int              DEFAULT 0 NOT NULL,
SurveyStatus           varchar(18)      DEFAULT '' NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
AdminName              nvarchar(50)     DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
Location               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
Reply                  ntext            DEFAULT '' NOT NULL, 
ReplyAdmin             nvarchar(50)     DEFAULT '' NOT NULL,
IsEmail                varchar(18)      DEFAULT '' NOT NULL,   
IsMobile               varchar(18)      DEFAULT '' NOT NULL,  
Description            nvarchar(500)    DEFAULT '' NOT NULL,
CompositeScore         int              DEFAULT 0 NOT NULL, 
Ext1                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext2                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext3                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext4                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext5                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext6                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext7                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext8                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext9                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext10                  nvarchar(50)     DEFAULT '' NOT NULL,
Ext11                  nvarchar(50)     DEFAULT '' NOT NULL,
Ext12                  nvarchar(50)     DEFAULT '' NOT NULL,
Ext13                  nvarchar(50)     DEFAULT '' NOT NULL,
Ext14                  nvarchar(50)     DEFAULT '' NOT NULL,
Ext15                  nvarchar(50)     DEFAULT '' NOT NULL, 
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SurveyQuestionnaire PRIMARY KEY NONCLUSTERED (SQID)
)
go

CREATE CLUSTERED INDEX IX_siteserver_SurveyQuestionnaire_Taxis ON siteserver_SurveyQuestionnaire(Taxis)
go


----比较管理------- 
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_CompareContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE siteserver_CompareContent(
CCID                   int              IDENTITY(1,1),
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
NodeID                 int              DEFAULT 0 NOT NULL, 
ContentID              int              DEFAULT 0 NOT NULL, 
Taxis                  int              DEFAULT 0 NOT NULL,
CompareStatus          varchar(18)      DEFAULT '' NOT NULL,
AdminName              varchar(50)      DEFAULT '' NOT NULL,
UserName               varchar(50)      DEFAULT '' NOT NULL,
IPAddress              varchar(50)      DEFAULT '' NOT NULL,
Location               nvarchar(50)     DEFAULT '' NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL, 
Description            nvarchar(500)    DEFAULT '' NOT NULL,
CompositeScore1        int              DEFAULT 0 NOT NULL, 
CompositeScore2        int              DEFAULT 0 NOT NULL, 
Ext1                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext2                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext3                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext4                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext5                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext6                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext7                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext8                   nvarchar(50)     DEFAULT '' NOT NULL,
Ext9                   nvarchar(50)     DEFAULT '' NOT NULL,
SettingsXML            ntext            DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_CompareContent PRIMARY KEY NONCLUSTERED (CCID)
)
go

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_CompareContent_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_CompareContent_Taxis ON siteserver_CompareContent(Taxis)
go



CREATE INDEX IX_siteserver_MD_N ON siteserver_MenuDisplay(PublishmentSystemID)
go
CREATE CLUSTERED INDEX IX_siteserver_Node_Taxis ON siteserver_Node(Taxis)
go
CREATE INDEX IX_siteserver_Node_P ON siteserver_Node(PublishmentSystemID)
go
CREATE INDEX IX_siteserver_SM_N ON siteserver_SeoMeta(PublishmentSystemID)
go
CREATE INDEX IX_siteserver_Template_Node ON siteserver_Template(PublishmentSystemID)
go
CREATE INDEX IX_siteserver_Template_TT ON siteserver_Template(TemplateType)
go
CREATE INDEX IX_siteserver_Tracking_P ON siteserver_Tracking(PublishmentSystemID)
go
CREATE INDEX IX_siteserver_Tracking_Page ON siteserver_Tracking(PageNodeID, PageContentID)
go

CREATE CLUSTERED INDEX IX_siteserver_KeywordClassify_Taxis ON siteserver_KeywordClassify(Taxis)
go

CREATE CLUSTERED INDEX IX_siteserver_WebsiteMessageClassify_Taxis ON siteserver_WebsiteMessageClassify(Taxis)
go

CREATE INDEX IX_siteserver_WebsiteMessageClassify_P ON siteserver_WebsiteMessageClassify(PublishmentSystemID)
go

ALTER TABLE siteserver_AdMaterial ADD CONSTRAINT FK_siteserver_Adv_AdMaterial
FOREIGN KEY (AdvID)
REFERENCES siteserver_Adv(AdvID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_Adv ADD CONSTRAINT FK_siteserver_AdArea_Adv
FOREIGN KEY (AdAreaID)
REFERENCES siteserver_AdArea(AdAreaID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_Comment ADD CONSTRAINT FK_siteserver_Node_Comment
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_GatherDatabaseRule ADD CONSTRAINT FK_siteserver_GDR_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_GatherFileRule ADD CONSTRAINT FK_siteserver_GFR_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_GatherRule ADD CONSTRAINT FK_siteserver_GR_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_InputContent ADD CONSTRAINT FK_siteserver_IC_I
FOREIGN KEY (InputID)
REFERENCES siteserver_Input(InputID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_MenuDisplay ADD CONSTRAINT FK_siteserver_MD_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_PublishmentSystem ADD CONSTRAINT FK_siteserver_PS_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_RelatedFieldItem ADD CONSTRAINT FK_siteserver_RFI_RF
FOREIGN KEY (RelatedFieldID)
REFERENCES siteserver_RelatedField(RelatedFieldID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_SeoMeta ADD CONSTRAINT FK_siteserver_SM_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_SeoMetasInNodes ADD CONSTRAINT FK_siteserver_SMInN_SM
FOREIGN KEY (SeoMetaID)
REFERENCES siteserver_SeoMeta(SeoMetaID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_SystemPermissions ADD CONSTRAINT FK_siteserver_SP_N
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_Template ADD CONSTRAINT FK_siteserver_Template_Node
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_TemplateLog ADD CONSTRAINT FK_siteserver_Template_Log
FOREIGN KEY (TemplateID)
REFERENCES siteserver_Template(TemplateID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_TemplateMatch ADD CONSTRAINT FK_siteserver_TM_N
FOREIGN KEY (NodeID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE siteserver_Tracking ADD CONSTRAINT FK_siteserver_Tracking_Node
FOREIGN KEY (PublishmentSystemID)
REFERENCES siteserver_Node(NodeID) ON DELETE CASCADE ON UPDATE CASCADE
go

ALTER TABLE siteserver_WebsiteMessageContent ADD CONSTRAINT FK_siteserver_WC_W
FOREIGN KEY (WebsiteMessageID)
REFERENCES siteserver_WebsiteMessage(WebsiteMessageID) ON DELETE CASCADE ON UPDATE CASCADE
go


