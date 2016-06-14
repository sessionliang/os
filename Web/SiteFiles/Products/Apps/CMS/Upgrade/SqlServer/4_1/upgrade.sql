IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_PublishmentSystem') AND NAME = 'PublishmentSystemType')
ALTER TABLE siteserver_PublishmentSystem ADD
PublishmentSystemType    varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_PublishmentSystem') AND NAME = 'AuxiliaryTableForGoods')
ALTER TABLE siteserver_PublishmentSystem ADD
AuxiliaryTableForGoods    varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_PublishmentSystem') AND NAME = 'AuxiliaryTableForBrand')
ALTER TABLE siteserver_PublishmentSystem ADD
AuxiliaryTableForBrand    varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_PublishmentSystem') AND NAME = 'GroupSN')
ALTER TABLE siteserver_PublishmentSystem ADD
GroupSN    nvarchar(255) DEFAULT '' NOT NULL
GO

--内容评论
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'NodeID')
ALTER TABLE siteserver_Comment ADD
NodeID    int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'ContentID')
ALTER TABLE siteserver_Comment ADD
ContentID    int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'Good')
ALTER TABLE siteserver_Comment ADD
Good   int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'UserName')
ALTER TABLE siteserver_Comment ADD
UserName   nvarchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'IPAddress')
ALTER TABLE siteserver_Comment ADD
IPAddress   varchar(50) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'AddDate')
ALTER TABLE siteserver_Comment ADD
AddDate   datetime DEFAULT getdate() NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'IsChecked')
ALTER TABLE siteserver_Comment ADD
IsChecked   varchar(18) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'IsRecommend')
ALTER TABLE siteserver_Comment ADD
IsRecommend   varchar(18) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'Content')
ALTER TABLE siteserver_Comment ADD
Content   ntext DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'PublishmentSystemID')
ALTER TABLE siteserver_Comment ADD
PublishmentSystemID   int DEFAULT 0 NOT NULL
GO


--第三方登录表（为兼容以前版本）
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_ThirdLogin') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

--内容定时审核时间
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('model_Content') AND NAME = 'CheckTaskDate')
ALTER TABLE model_Content ADD
CheckTaskDate    DateTime   DEFAULT getdate() NOT NULL
GO

--内容定时下架时间
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('model_Content') AND NAME = 'UnCheckTaskDate')
ALTER TABLE model_Content ADD
UnCheckTaskDate    DateTime   DEFAULT getdate() NOT NULL
GO

--电视剧模型
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Teleplay') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO


/*
* 3.6.x 升级 4.1
*/

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdArea') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Adv') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
NodeIDCollectionToChannel    nvarchar(255)    DEFAULT '' NOT NULL,
NodeIDCollectionToContent    nvarchar(255)    DEFAULT '' NOT NULL,
FileTemplateIDCollection     nvarchar(255)    DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_Adv PRIMARY KEY NONCLUSTERED (AdvID)
)
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdMaterial') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('siteserver_AdMaterial') AND NAME = 'FK_siteserver_Adv_AdMaterial' AND XTYPE = 'F')
ALTER TABLE siteserver_AdMaterial ADD CONSTRAINT FK_siteserver_Adv_AdMaterial
FOREIGN KEY(AdvID)
REFERENCES siteserver_Adv(AdvID) ON DELETE CASCADE ON UPDATE CASCADE
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('siteserver_Adv') AND NAME = 'FK_siteserver_AdArea_Adv' AND XTYPE = 'F')
ALTER TABLE siteserver_Adv ADD CONSTRAINT FK_siteserver_AdArea_Adv
FOREIGN KEY(AdAreaID)
REFERENCES siteserver_AdArea(AdAreaID) ON DELETE CASCADE ON UPDATE CASCADE
GO

/************************************
* action : 固定广告数据迁移
* when   : 4.0之前版本向4.1版本升级
************************************/
--如果siteserver_Ad存在，并且存有数据，那么进行数据转移
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Ad') AND OBJECTPROPERTY(ID,'ISTABLE')=1)
BEGIN
--判断是否存有数据
DECLARE @adCount int;
SELECT @adCount = COUNT(1) FROM siteserver_Ad;
IF @adCount = 0
BEGIN
PRINT 'Count of siteserver_Ad is zero';
RETURN;
END

--开始同步数据
PRINT 'Sync siteserver_Ad Starting ...';
DECLARE @isCursorExists int;
SELECT @isCursorExists = CURSOR_STATUS('LOCAL','cursor_Ergodic');
IF (@isCursorExists = -3)
BEGIN
DECLARE cursor_Ergodic CURSOR LOCAL FOR SELECT AdName, PublishmentSystemID FROM siteserver_Ad;
END
DECLARE @adAreaID int;
DECLARE @advID int;
DECLARE @adMaterialID int;
DECLARE @count int;
DECLARE @adName nvarchar(50);
DECLARE @publishmentSystemID int;

OPEN cursor_Ergodic;
FETCH NEXT FROM	cursor_Ergodic into @adName, @publishmentSystemID;
WHILE(@@FETCH_STATUS=0)
BEGIN
--STEP 1, 创建siteserver_AdArea(广告位)
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdArea') AND OBJECTPROPERTY(ID,'ISTABLE') = 1)
BEGIN
INSERT INTO siteserver_AdArea(PublishmentSystemID, AdAreaName, Width, Height, Summary, IsEnabled, AddDate)
SELECT PublishmentSystemID, AdName, 0, 0, '', IsEnabled, GETDATE() FROM siteserver_Ad WHERE AdName = @adName AND PublishmentSystemID = @publishmentSystemID;
SELECT @adAreaID = @@IDENTITY;
END
ELSE
BEGIN
PRINT 'Siteserver_AdArea is not exists';
CLOSE cursor_Ergodic;
DEALLOCATE cursor_Ergodic;
RETURN;
END
--STEP 2, 同步广告siteserver_Adv(广告)
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Adv') AND OBJECTPROPERTY(ID,'ISTABLE') = 1)
BEGIN
INSERT INTO siteserver_Adv(PublishmentSystemID, AdAreaID, AdvName, Summary, IsEnabled, IsDateLimited, StartDate, EndDate, LevelType, Level, IsWeight, Weight, RotateType, RotateInterval, NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection)
SELECT PublishmentSystemID, @adAreaID, AdName, '', IsEnabled, IsDateLimited, StartDate, EndDate, 'Hold', 0, 'False', 0, 'Equality', 0, PublishmentSystemID, '', '' FROM siteserver_Ad WHERE AdName = @adName AND PublishmentSystemID = @publishmentSystemID;
SELECT @advID = @@IDENTITY;
END
ELSE
BEGIN
PRINT 'siteserver_Adv is not exists';
CLOSE cursor_Ergodic;
DEALLOCATE cursor_Ergodic;
RETURN;
END
--STEP 3, 同步广告物料siteserver_AdMaterial(广告物料)
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdMaterial') AND OBJECTPROPERTY(ID,'ISTABLE') = 1)
BEGIN
INSERT INTO siteserver_AdMaterial(PublishmentSystemID, AdvID, AdvertID, AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, Weight, IsEnabled)
SELECT PublishmentSystemID, @advID, 0, AdName, AdType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, 0, IsEnabled FROM siteserver_Ad WHERE AdName = @adName AND PublishmentSystemID = @publishmentSystemID;
SELECT @adMaterialID = @@IDENTITY;
END
ELSE
BEGIN
PRINT 'siteserver_AdMeterial is not exists';
CLOSE cursor_Ergodic;
DEALLOCATE cursor_Ergodic;
RETURN;
END

SET @count = @count + 1;
PRINT 'Have sync ' + CONVERT(varchar(50), @count) + ' AD';

--获取下一条数据
FETCH NEXT FROM cursor_Ergodic into @adName, @publishmentSystemID;
END
CLOSE cursor_Ergodic;
DEALLOCATE cursor_Ergodic;
END
GO


IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Photo') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_TemplateLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO


--广告位修改
IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_adv') AND NAME = 'NodeIDCollectionToChannel')
ALTER TABLE siteserver_adv ALTER
COLUMN NodeIDCollectionToChannel nvarchar(4000)
GO

IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_adv') AND NAME = 'NodeIDCollectionToContent')
ALTER TABLE siteserver_adv ALTER
COLUMN NodeIDCollectionToContent nvarchar(4000)
GO

IF EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_adv') AND NAME = 'FileTemplateIDCollection')
ALTER TABLE siteserver_adv ALTER
COLUMN FileTemplateIDCollection nvarchar(4000)
GO

--采集（自动开关）
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_GatherDatabaseRule') AND NAME = 'IsAutoCreate')
ALTER TABLE siteserver_GatherDatabaseRule ADD
IsAutoCreate varchar(18) DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_GatherFileRule') AND NAME = 'IsAutoCreate')
ALTER TABLE siteserver_GatherFileRule ADD
IsAutoCreate varchar(18) DEFAULT '' NOT NULL
GO

--关键字分类
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Keyword') AND NAME = 'ClassifyID')
ALTER TABLE siteserver_Keyword ADD
ClassifyID int DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_KeywordClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
BEGIN
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
END
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_KeywordClassify_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_KeywordClassify_Taxis ON siteserver_KeywordClassify(Taxis)
GO

--网站留言
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_WebsiteMessage') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

--网站留言分类
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_WebsiteMessageClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
BEGIN
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
END
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_WebsiteMessageClassify_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_WebsiteMessageClassify_Taxis ON siteserver_WebsiteMessageClassify(Taxis)
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_WebsiteMessageClassify_P')
CREATE INDEX IX_siteserver_WebsiteMessageClassify_P ON siteserver_WebsiteMessageClassify(PublishmentSystemID)
GO

--网站留言内容
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_WebsiteMessageContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
BEGIN
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

ALTER TABLE siteserver_WebsiteMessageContent ADD CONSTRAINT FK_siteserver_WC_W
FOREIGN KEY (WebsiteMessageID)
REFERENCES siteserver_WebsiteMessage(WebsiteMessageID) ON DELETE CASCADE ON UPDATE CASCADE
END
GO

--网站留言回复模板
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_WebsiteMessageReplayTemplate') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE siteserver_WebsiteMessageReplayTemplate(
ID                 int             IDENTITY(1,1),
TemplateTitle      nvarchar(50)    DEFAULT '' NOT NULL,
TemplateContent    ntext           DEFAULT '' NOT NULL,
IsEnabled          varchar(18)     DEFAULT '' NOT NULL,
ClassifyID         int             DEFAULT 0 NOT NULL,
AddDate         datetime     DEFAULT getdate() NOT NULL,
CONSTRAINT PK_siteserver_WebsiteMessageReplayTemplate PRIMARY KEY NONCLUSTERED (ID)
)
GO

--搜索关键字
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Searchword') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

--搜索关键字设置
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SearchwordSetting') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SearchwordSetting') AND NAME = 'IsTemplate')
ALTER TABLE siteserver_SearchwordSetting ADD
	IsTemplate               varchar(18)         DEFAULT '' NOT NULL,
	StyleTemplate           ntext					DEFAULT '' NOT NULL,
	ScriptTemplate           ntext					DEFAULT '' NOT NULL,
	ContentTemplate           ntext					DEFAULT '' NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SearchwordSetting') AND NAME = 'IsTemplate')
ALTER TABLE siteserver_SearchwordSetting ADD
	IsTemplate               varchar(18)         DEFAULT '' NOT NULL,
	StyleTemplate           ntext					DEFAULT '' NOT NULL,
	ScriptTemplate           ntext					DEFAULT '' NOT NULL,
	ContentTemplate           ntext					DEFAULT '' NOT NULL
GO


--专题分类
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SpecialClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
    CONSTRAINT PK_siteserver_Special PRIMARY KEY NONCLUSTERED (ItemID)
)
GO

--专题内容
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SpecialContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO


--广告分类
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdvImageClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

--广告内容
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_AdvImageContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_InputClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_InputClassify_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_InputClassify_Taxis ON siteserver_InputClassify(Taxis)
GO
IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_InputClassify_P')
CREATE INDEX IX_siteserver_InputClassify_P ON siteserver_InputClassify(PublishmentSystemID)
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Input') AND NAME = 'ClassifyID')
ALTER TABLE siteserver_Input ADD
ClassifyID			   int             DEFAULT 0  NOT NULL
GO


---- by 20151110 sofuny  订阅信息数据表
-----订阅内容
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_Subscribe') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_Subscribe_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_Subscribe_Taxis ON siteserver_Subscribe(Taxis)
GO


-----订阅内容设置
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SubscribeSet') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE siteserver_SubscribeSet(
SubscribeSetID         int              IDENTITY(1,1),
EmailContentAddress    nvarchar(200)    DEFAULT '' NOT NULL,
MobileContentAddress   nvarchar(200)    DEFAULT '' NOT NULL,
PushType               nvarchar(50)     DEFAULT '' NOT NULL,
PushDate               int              DEFAULT 0 NOT NULL,
AddDate                datetime         DEFAULT getdate() NOT NULL,
PublishmentSystemID    int              DEFAULT 0 NOT NULL,
UserName               nvarchar(50)     DEFAULT '' NOT NULL,
IsTemplate             varchar(18)      DEFAULT '' NOT NULL,
StyleTemplate          ntext		DEFAULT '' NOT NULL,
ScriptTemplate         ntext		DEFAULT '' NOT NULL,
ContentTemplate        ntext		DEFAULT '' NOT NULL,
CONSTRAINT PK_siteserver_SubscribeSet PRIMARY KEY NONCLUSTERED (SubscribeSetID)
)
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SubscribeSet') AND NAME = 'IsTemplate')
ALTER TABLE siteserver_SubscribeSet ADD
	IsTemplate               varchar(18)         DEFAULT '' NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SubscribeSet') AND NAME = 'StyleTemplate')
ALTER TABLE siteserver_SubscribeSet ADD
	StyleTemplate            ntext		     DEFAULT '' NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SubscribeSet') AND NAME = 'ScriptTemplate')
ALTER TABLE siteserver_SubscribeSet ADD
	ScriptTemplate           ntext		     DEFAULT '' NOT NULL
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_SubscribeSet') AND NAME = 'ContentTemplate')
ALTER TABLE siteserver_SubscribeSet ADD
	ContentTemplate          ntext		     DEFAULT '' NOT NULL
GO

-----订阅会员
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SubscribeUser') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO

-----订阅内容推送记录
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SubscribePushRecord') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO
  

---by 20151124 智能推送
----会员浏览统计表
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_ViewsStatistics') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
GO 

--评论增加字段，管理员名称
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Comment') AND NAME = 'AdminName')
ALTER TABLE siteserver_Comment ADD
	AdminName nvarchar(255) DEFAULT ''  NOT NULL
GO



----分支机构管理表 
-----分支机构分类
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_OrganizationClassify') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_OrganizationClassify_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_OrganizationClassify_Taxis ON siteserver_OrganizationClassify(Taxis)
go

-----分支机构区域
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_OrganizationArea') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_OrganizationArea_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_OrganizationArea_Taxis ON siteserver_OrganizationArea(Taxis)
go

----分支机构信息表
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_OrganizationInfo') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_OrganizationInfo_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_OrganizationInfo_Taxis ON siteserver_OrganizationInfo(Taxis)
go



--内容前台会员  新的投稿管理功能增加
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('model_Content') AND NAME = 'MemberName')
ALTER TABLE model_Content ADD
MemberName          nvarchar(50)     DEFAULT '' NOT NULL
GO


----评价管理-------
--评价内容
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_EvaluationContent') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_EvaluationContent_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_EvaluationContent_Taxis ON siteserver_EvaluationContent(Taxis)
go

--功能内容字段
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_FunctionTableStyles') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_ConsoleLog') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_TrialApply') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_TrialApply_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_TrialApply_Taxis ON siteserver_TrialApply(Taxis)
go

--试用报告
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_TrialReport') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_TrialReport_Taxis')
CREATE CLUSTERED INDEX IX_siteserver_TrialReport_Taxis ON siteserver_TrialReport(Taxis)
go


----调查问卷管理
--调查问卷
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('siteserver_SurveyQuestionnaire') AND OBJECTPROPERTY(ID,'IsTable') = 1)
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

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = 'IX_siteserver_SurveyQuestionnaire_Taxis')
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
