CREATE TABLE bairong_CloudStorage(
    StorageID       int            NOT NULL,
    ProviderType    varchar(50)    DEFAULT '' NOT NULL,
    SettingsXML     ntext          DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_CloudStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go


CREATE TABLE bairong_FTPStorage(
    StorageID    int             NOT NULL,
    Server       varchar(200)    DEFAULT '' NOT NULL,
    Port         int             DEFAULT 0 NOT NULL,
    UserName     varchar(200)    DEFAULT '' NOT NULL,
    Password     varchar(200)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_FTPStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go


CREATE TABLE bairong_LocalStorage(
    StorageID        int              NOT NULL,
    DirectoryPath    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_LocalStorage PRIMARY KEY NONCLUSTERED (StorageID)
)
go


CREATE TABLE bairong_Storage(
    StorageID      int              IDENTITY(1,1),
    StorageName    nvarchar(50)     DEFAULT '' NOT NULL,
    StorageUrl     varchar(200)     DEFAULT '' NOT NULL,
    StorageType    varchar(50)      DEFAULT '' NOT NULL,
    IsEnabled      varchar(18)      DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate        datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_Storage PRIMARY KEY NONCLUSTERED (StorageID)
)
go


CREATE TABLE bairong_Task(
    TaskID                  int              IDENTITY(1,1),
    TaskName                nvarchar(50)     DEFAULT '' NOT NULL,
    ProductID               varchar(50)      DEFAULT '' NOT NULL,
    IsSystemTask            varchar(18)      DEFAULT '' NOT NULL,
    PublishmentSystemID     int              DEFAULT 0 NOT NULL,
    ServiceType             varchar(50)      DEFAULT '' NOT NULL,
    ServiceParameters       ntext            DEFAULT '' NOT NULL,
    FrequencyType           varchar(50)      DEFAULT '' NOT NULL,
    PeriodIntervalMinute    int              DEFAULT 0 NOT NULL,
    StartDay                int              DEFAULT 0 NOT NULL,
    StartWeekday            int              DEFAULT 0 NOT NULL,
    StartHour               int              DEFAULT 0 NOT NULL,
    IsEnabled               varchar(18)      DEFAULT '' NOT NULL,
    AddDate                 datetime         DEFAULT getdate() NOT NULL,
    LastExecuteDate         datetime         DEFAULT getdate() NOT NULL,
    Description             nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_Task PRIMARY KEY NONCLUSTERED (TaskID)
)
go



CREATE TABLE bairong_TaskLog(
    ID               int              IDENTITY(1,1),
    TaskID           int              NOT NULL,
    IsSuccess        varchar(18)      DEFAULT '' NOT NULL,
    ErrorMessage     nvarchar(255)    DEFAULT '' NOT NULL,
    StackTrace       ntext            DEFAULT '' NOT NULL,
    SubtaskErrors    ntext            DEFAULT '' NOT NULL,
    AddDate          datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_TaskLog PRIMARY KEY CLUSTERED (ID)
)
go


ALTER TABLE bairong_CloudStorage ADD CONSTRAINT FK_bairong_Storage_Cloud 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_FTPStorage ADD CONSTRAINT FK_bairong_Storage_FTP 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go


ALTER TABLE bairong_LocalStorage ADD CONSTRAINT FK_bairong_Storage_Local 
    FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE ON UPDATE CASCADE
go

ALTER TABLE bairong_TaskLog ADD CONSTRAINT FK_bairong_Task_Log 
    FOREIGN KEY (TaskID)
    REFERENCES bairong_Task(TaskID) ON DELETE CASCADE ON UPDATE CASCADE
go