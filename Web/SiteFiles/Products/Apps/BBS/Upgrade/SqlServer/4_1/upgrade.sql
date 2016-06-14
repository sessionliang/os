/*
 * 3.6.x Éý¼¶ 4.1
 */

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bbs_Configuration') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bbs_Configuration(
    PublishmentSystemID    int      NOT NULL,
    SettingsXML            ntext    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Configuration PRIMARY KEY NONCLUSTERED (PublishmentSystemID)
)
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('bbs_Face') AND OBJECTPROPERTY(ID,'IsTable') = 1)
CREATE TABLE bbs_Face(
    ID                     int             IDENTITY(1,1),
    PublishmentSystemID    int             DEFAULT 0 NOT NULL,
    FaceName               varchar(50)     DEFAULT '' NOT NULL,
    Title                  nvarchar(50)    DEFAULT '' NOT NULL,
    Taxis                  int             DEFAULT 0 NOT NULL,
    IsEnabled              varchar(18)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bbs_Face PRIMARY KEY NONCLUSTERED (ID)
)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Ad') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Ad ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Announcement') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Announcement ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Attachment') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Attachment ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_AttachmentType') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_AttachmentType ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Configuration') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Configuration ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_CreditRule') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_CreditRule ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_CreditRuleLog') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_CreditRuleLog ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Face') AND NAME = 'ID')
ALTER TABLE bbs_Face ADD
    ID    int    IDENTITY(1,1),
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bbs_Face') AND NAME = 'PK_bbs_Face' AND XTYPE = 'PK')
ALTER TABLE bbs_Face DROP CONSTRAINT PK_bbs_Face   
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bbs_Face') AND NAME = 'PK_bbs_Face' AND XTYPE = 'PK')
ALTER TABLE bbs_Face ADD CONSTRAINT
	PK_bbs_Face   PRIMARY KEY (ID)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Forum') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Forum ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Icon') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Icon ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Identify') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Identify ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_KeywordsCategory') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_KeywordsCategory ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_KeywordsFilter') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_KeywordsFilter ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Link') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Link ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Navigation') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Navigation ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Online') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Online ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Permissions') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Permissions ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Poll') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Poll ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_PollItem') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_PollItem ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_PollUser') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_PollUser ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Post') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Post ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Report') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Report ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Thread') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_Thread ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_ThreadCategory') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_ThreadCategory ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_ThreadCategory') AND NAME = 'PublishmentSystemID')
ALTER TABLE bbs_ThreadCategory ADD
	PublishmentSystemID    int    DEFAULT 0 NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID = OBJECT_ID('bbs_Users') AND NAME = 'UserID')
ALTER TABLE bbs_Users ADD
    UserID                 int    IDENTITY(1,1),
	GroupSN                nvarchar(255)    DEFAULT '' NOT NULL
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bbs_Users') AND NAME = 'PK_bbs_Users' AND XTYPE = 'PK')
ALTER TABLE bbs_Users DROP CONSTRAINT PK_bbs_Users
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE PARENT_OBJ = OBJECT_ID('bbs_Users') AND NAME = 'PK_bbs_Users' AND XTYPE = 'PK')
ALTER TABLE bbs_Users ADD CONSTRAINT
	PK_bbs_Users PRIMARY KEY(UserID)
GO
