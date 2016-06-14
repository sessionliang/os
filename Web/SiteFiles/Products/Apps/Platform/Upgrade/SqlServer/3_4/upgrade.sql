ALTER TABLE bairong_Administrator ADD 
    LastModuleID           varchar(50)      DEFAULT '' NOT NULL

GO

CREATE TABLE bairong_Config(
    IsInitialized           varchar(18)      DEFAULT '' NOT NULL,
    DatabaseVersion         varchar(50)      DEFAULT '' NOT NULL,
    RestrictionBlackList    nvarchar(255)    DEFAULT '' NOT NULL,
    RestrictionWhiteList    nvarchar(255)    DEFAULT '' NOT NULL,
    IsRelatedUrl            varchar(18)      DEFAULT '' NOT NULL,
    RootUrl                 varchar(200)     DEFAULT '' NOT NULL,
    UpdateDate              datetime         DEFAULT getdate() NOT NULL,
    SettingsXML             ntext            DEFAULT '' NOT NULL
)

GO

INSERT INTO bairong_Config (IsInitialized, DatabaseVersion, RestrictionBlackList, RestrictionWhiteList, IsRelatedUrl, RootUrl, UpdateDate, SettingsXML) VALUES ('True', '3.4', '', '', 'True', '', getdate(), '')

GO

ALTER TABLE bairong_Module ADD 
    DirectoryName           varchar(50)      DEFAULT '' NOT NULL

GO

ALTER TABLE bairong_Roles ADD 
    Modules            varchar(200)     DEFAULT '' NOT NULL

GO

CREATE PROCEDURE ss_deletecolumn( 
@tableName varchar(50),
@columnName varchar(50)
)
AS

begin

declare @name varchar(50)
select @name = b.name from syscolumns a,sysobjects b where a.id=object_id('' + @tableName + '') and b.id=a.cdefault and a.name='' + @columnName + ''  and b.name like 'DF%'

exec('alter table ' + @tableName + ' drop constraint  '+ @name)


end

GO

exec ss_deletecolumn 'bairong_Users','UserName'

GO

ALTER TABLE bairong_Users DROP COLUMN UserName

GO

exec sp_rename 'bairong_Users.UserID','UserName','column'

GO

ALTER TABLE bairong_Users ADD [Mobile] varchar(20) DEFAULT '' NOT NULL

GO

exec sp_rename 'bairong_UserSettings.UserID','UserName','column'

GO

exec sp_rename 'bairong_VoteIPAddress.AddUserID','UserName','column'

GO
