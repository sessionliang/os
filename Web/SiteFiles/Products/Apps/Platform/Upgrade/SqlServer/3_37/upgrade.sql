CREATE TABLE bairong_GeneralPermissionsInRoles(
    RoleName              nvarchar(255)    NOT NULL,
    GeneralPermissions    text             DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_GeneralPermissionsInRoles PRIMARY KEY CLUSTERED (RoleName)
)

GO

CREATE TABLE bairong_Log(
    LogID          int            IDENTITY(1,1),
    LogUserName    varchar(50)    DEFAULT '' NOT NULL,
    LogIP          varchar(50)    DEFAULT '' NOT NULL,
    OS             varchar(50)    DEFAULT '' NOT NULL,
    IsSuccessed    varchar(18)    DEFAULT '' NOT NULL,
    LoginDate      datetime       DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_Log PRIMARY KEY CLUSTERED (LogID)
)

GO

CREATE TABLE bairong_UserConfig(
    CenterName      nvarchar(50)     DEFAULT '' NOT NULL,
    Copyright       nvarchar(255)    DEFAULT '' NOT NULL,
    DefaultTheme    varchar(50)      DEFAULT '' NOT NULL,
    SettingsXML     ntext            DEFAULT '' NOT NULL
)

GO

ALTER TABLE bairong_UserGroups ADD 
    UserPermissions    ntext           DEFAULT '' NOT NULL

GO