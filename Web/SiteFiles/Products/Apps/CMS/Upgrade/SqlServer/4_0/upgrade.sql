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

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('siteserver_Commnet') AND NAME = 'UserName')
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
