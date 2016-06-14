CREATE TABLE bairong_Storage(
    StorageID      NUMBER(38, 0)     NOT NULL,
    StorageName    NVARCHAR2(50)     DEFAULT '',
    StorageUrl     VARCHAR2(200)     DEFAULT '',
    StorageType    VARCHAR2(50)      DEFAULT '',
    IsEnabled      VARCHAR2(18)      DEFAULT '',
    Description    NVARCHAR2(255)    DEFAULT '',
    AddDate        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_Storage PRIMARY KEY (StorageID)
)
go


CREATE TABLE bairong_LocalStorage(
    StorageID        NUMBER(38, 0)     NOT NULL,
    DirectoryPath    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_LocalStorage PRIMARY KEY (StorageID), 
    CONSTRAINT FK_bairong_Storage_Local FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
)
go


CREATE TABLE bairong_CloudStorage(
    StorageID       NUMBER(38, 0)    NOT NULL,
    ProviderType    VARCHAR2(50)     DEFAULT '',
    SettingsXML     NCLOB            DEFAULT '',
    CONSTRAINT PK_bairong_CloudStorage PRIMARY KEY (StorageID), 
    CONSTRAINT FK_bairong_Storage_Cloud FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
)
go


CREATE TABLE bairong_FTPStorage(
    StorageID    NUMBER(38, 0)    NOT NULL,
    Server       VARCHAR2(200)    DEFAULT '',
    Port         NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    UserName     VARCHAR2(200)    DEFAULT '',
    Password     VARCHAR2(200)    DEFAULT '',
    CONSTRAINT PK_bairong_FTPStorage PRIMARY KEY (StorageID), 
    CONSTRAINT FK_bairong_Storage_FTP FOREIGN KEY (StorageID)
    REFERENCES bairong_Storage(StorageID) ON DELETE CASCADE
)
go


CREATE TABLE bairong_Task(
    TaskID                  NUMBER(38, 0)     NOT NULL,
    TaskName                NVARCHAR2(50)     DEFAULT '',
    ProductID               VARCHAR2(50)      DEFAULT '',
    IsSystemTask            VARCHAR2(18)      DEFAULT '',
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
    CONSTRAINT PK_bairong_Task PRIMARY KEY (TaskID)
)
go


CREATE TABLE bairong_TaskLog(
    ID               NUMBER(38, 0)     NOT NULL,
    TaskID           NUMBER(38, 0)     NOT NULL,
    IsSuccess        VARCHAR2(18)      DEFAULT '',
    ErrorMessage     NVARCHAR2(255)    DEFAULT '',
    StackTrace       NCLOB             DEFAULT '',
    SubtaskErrors    NCLOB             DEFAULT '',
    AddDate          TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_TaskLog PRIMARY KEY (ID), 
    CONSTRAINT FK_bairong_Task_Log FOREIGN KEY (TaskID)
    REFERENCES bairong_Task(TaskID) ON DELETE CASCADE
)
go


CREATE SEQUENCE BAIRONG_STORAGE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE SEQUENCE BAIRONG_TASK_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE SEQUENCE BAIRONG_TASKLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go