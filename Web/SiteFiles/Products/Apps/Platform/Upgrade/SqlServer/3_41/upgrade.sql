sp_rename   'bairong_AuxiliaryTableCollection','bairong_TableCollection'

GO

sp_rename   'bairong_AuxiliaryTableMetadata','bairong_TableMetadata'

GO

sp_rename   'bairong_AuxiliaryTableStyle','bairong_TableStyle'

GO

sp_rename   'bairong_AuxiliaryTableStyleItem','bairong_TableStyleItem'

GO

sp_rename   'bairong_GeneralPermissionsInRoles','bairong_PermissionsInRoles'

GO

DROP TABLE bairong_Log

GO

CREATE TABLE bairong_Log(
    ID           int              IDENTITY(1,1),
    UserName     varchar(50)      DEFAULT '' NOT NULL,
    IPAddress    varchar(50)      DEFAULT '' NOT NULL,
    AddDate      datetime         DEFAULT getdate() NOT NULL,
    Action       nvarchar(255)    DEFAULT '' NOT NULL,
    Summary      nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Log PRIMARY KEY CLUSTERED (ID)
)

GO