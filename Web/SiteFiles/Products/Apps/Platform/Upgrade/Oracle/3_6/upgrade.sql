ALTER TABLE bairong_Administrator ADD (
    DepartmentID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AreaID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Mobile                 VARCHAR2(20)      DEFAULT ''
)
go


CREATE TABLE bairong_Area(
    AreaID           NUMBER(38, 0)     NOT NULL,
    AreaName         NVARCHAR2(255)    DEFAULT '',
    ParentID         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ParentsPath      NVARCHAR2(255)    DEFAULT '',
    ParentsCount     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChildrenCount    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsLastNode       VARCHAR2(18)      DEFAULT '',
    Taxis            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfAdmin     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfUser      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Area PRIMARY KEY (AreaID)
)
go

CREATE SEQUENCE BAIRONG_AREA_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE bairong_ContentCheck(
    CheckID                NUMBER(38, 0)     NOT NULL,
    TableName              VARCHAR2(50)      DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsAdmin                VARCHAR2(18)      DEFAULT '',
    UserName               NVARCHAR2(255)    DEFAULT '',
    IsChecked              VARCHAR2(18)      DEFAULT '',
    CheckedLevel           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CheckDate              TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Reasons                NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_ContentCheck PRIMARY KEY (CheckID)
)
go


CREATE SEQUENCE BAIRONG_CONTENTCHECK_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE bairong_Department(
    DepartmentID      NUMBER(38, 0)     NOT NULL,
    DepartmentName    NVARCHAR2(255)    DEFAULT '',
    Code              VARCHAR2(50)      DEFAULT '',
    ParentID          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ParentsPath       NVARCHAR2(255)    DEFAULT '',
    ParentsCount      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ChildrenCount     NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsLastNode        VARCHAR2(18)      DEFAULT '',
    Taxis             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AddDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Summary           NVARCHAR2(255)    DEFAULT '',
    CountOfAdmin      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CountOfUser       NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Department PRIMARY KEY (DepartmentID)
)
go


CREATE SEQUENCE BAIRONG_DEPARTMENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE bairong_SMSConfig(
    ID           NUMBER(38, 0)     NOT NULL,
    UserName     NVARCHAR2(255)    DEFAULT '',
    PassWord     NVARCHAR2(255)    DEFAULT '',
    MD5String    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_SMSConfig PRIMARY KEY (ID)
)
go


CREATE SEQUENCE BAIRONG_SMSCONFIG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE bairong_SMSMessages(
    ID             NUMBER(38, 0)      NOT NULL,
    MobilesList    NCLOB              DEFAULT '',
    SMSContent     NVARCHAR2(1000)    DEFAULT '',
    SendDate       TIMESTAMP(6)       DEFAULT sysdate NOT NULL,
    SMSUserName    NVARCHAR2(255)     DEFAULT '',
    CONSTRAINT PK_bairong_SMSMessages PRIMARY KEY (ID)
)
go


CREATE SEQUENCE BAIRONG_SMSMESSAGES_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


ALTER TABLE bairong_TableStyle ADD (
    IsSingleLine       VARCHAR2(18)      DEFAULT ''
)
go


CREATE TABLE bairong_UserContact(
    ID                 NUMBER(38, 0)     NOT NULL,
    RelatedUserName    NVARCHAR2(255)    DEFAULT '',
    CreateUserName     NVARCHAR2(255)    DEFAULT '',
    Taxis              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    FullName           NVARCHAR2(50)     DEFAULT '',
    AvatarUrl          VARCHAR2(200)     DEFAULT '',
    Summary            NVARCHAR2(255)    DEFAULT '',
    Tel                VARCHAR2(50)      DEFAULT '',
    Mobile             VARCHAR2(200)     DEFAULT '',
    Email              VARCHAR2(200)     DEFAULT '',
    QQ                 VARCHAR2(50)      DEFAULT '',
    Birthday           VARCHAR2(50)      DEFAULT '',
    Organization       NVARCHAR2(50)     DEFAULT '',
    Department         NVARCHAR2(50)     DEFAULT '',
    Position           NVARCHAR2(50)     DEFAULT '',
    Address            NVARCHAR2(255)    DEFAULT '',
    Website            VARCHAR2(200)     DEFAULT '',
    CONSTRAINT PK_bairong_UserContact PRIMARY KEY (ID)
)
go


CREATE SEQUENCE BAIRONG_USERCONTACT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE bairong_UserDownload(
    ID                NUMBER(38, 0)     NOT NULL,
    CreateUserName    NVARCHAR2(255)    DEFAULT '',
    Taxis             NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Downloads         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    FileName          NVARCHAR2(255)    DEFAULT '',
    FileUrl           VARCHAR2(200)     DEFAULT '',
    Summary           NVARCHAR2(255)    DEFAULT '',
    AddDate           TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_UserDownload PRIMARY KEY (ID)
)
go


CREATE SEQUENCE BAIRONG_USERDOWNLOAD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


ALTER TABLE bairong_Users ADD (
    CreateDate          TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CreateIPAddress     VARCHAR2(50)      DEFAULT '',
    CreateUserName      NVARCHAR2(255)    DEFAULT '',
    DepartmentID        NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    AreaID              NUMBER(38, 0)     DEFAULT 0 NOT NULL
)
go


ALTER TABLE bairong_UserType ADD (
    IsDefault        VARCHAR2(18)     DEFAULT '',
    IsPermissions    VARCHAR2(18)     DEFAULT '',
    Permissions      CLOB             DEFAULT ''
)
go