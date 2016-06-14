CREATE TABLE bairong_Module(
    ModuleID       nvarchar(50)    DEFAULT '' NOT NULL,
    Version        nvarchar(50)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Module PRIMARY KEY NONCLUSTERED (ModuleID)
)