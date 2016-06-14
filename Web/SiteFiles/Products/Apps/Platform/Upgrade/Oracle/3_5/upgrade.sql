CREATE SEQUENCE BAIRONG_CARD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_CARDTYPE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_SSOAPP_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_PAYMENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_PAYRECORD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERADDCARD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE SEQUENCE BAIRONG_USERCONSUME_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
GO

CREATE TABLE bairong_Card(
    CardID        NUMBER(38, 0)     NOT NULL,
    CardSN        VARCHAR2(50)      DEFAULT '',
    Password      VARCHAR2(50)      DEFAULT '',
    CardTypeID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CreateTime    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    EndTime       TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    UseTime       VARCHAR2(50)      DEFAULT '' NOT NULL,
    Status        VARCHAR2(20)      DEFAULT '',
    UserName      NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_Card PRIMARY KEY (CardID)
)
GO



CREATE TABLE bairong_CardType(
    CardTypeID     NUMBER(38, 0)    NOT NULL,
    NameType       NVARCHAR2(50)    DEFAULT '',
    CardCount      NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Price          FLOAT(20)        DEFAULT 0 NOT NULL,
    Description    VARCHAR2(255)    DEFAULT '',
    AddTime        TIMESTAMP(6)     DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_bairong_CardType PRIMARY KEY (CardTypeID)
)
GO

CREATE TABLE bairong_SSOApp(
    AppID             NUMBER(38, 0)    NOT NULL,
    AppType           VARCHAR2(50)     DEFAULT '' NOT NULL,
    AppName           NVARCHAR2(50)    DEFAULT '' NOT NULL,
    Url               VARCHAR2(200)    DEFAULT '' NOT NULL,
    AuthKey           VARCHAR2(200)    DEFAULT '' NOT NULL,
    IPAddress         VARCHAR2(50)     DEFAULT '' NOT NULL,
    AccessFileName    VARCHAR2(50)     DEFAULT '' NOT NULL,
    IsSyncLogin       VARCHAR2(18)     DEFAULT '' NOT NULL,
    AddDate           TIMESTAMP(6)     DEFAULT sysdate NOT NULL,
    Summary           NVARCHAR2(255)    DEFAULT '' NOT NULL
)
GO

CREATE TABLE bairong_Payment(
    PaymentID      NUMBER(38, 0)     NOT NULL,
    PaymentType    VARCHAR2(50)      DEFAULT '' NOT NULL,
    PaymentName    NVARCHAR2(50)     DEFAULT '' NOT NULL,
    Fee            NUMBER            DEFAULT 0 NOT NULL,
    IsEnabled      VARCHAR2(18)      DEFAULT '' NOT NULL,
    IsCOD          VARCHAR2(18)      DEFAULT '' NOT NULL,
    IsPayOnline    VARCHAR2(18)      DEFAULT '' NOT NULL,
    Description    NVARCHAR2(255)    DEFAULT '' NOT NULL,
    Taxis          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    SettingsXML    NCLOB             DEFAULT '' NOT NULL
)
GO

CREATE TABLE bairong_PayRecord(
    RecordID       NUMBER(38, 0)     NOT NULL,
    OrderSN        VARCHAR2(50)      DEFAULT '',
    UserName       NVARCHAR2(255)    DEFAULT '',
    PayTime        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Price          FLOAT(20)         DEFAULT 0 NOT NULL,
    IP             VARCHAR2(50)      DEFAULT '',
    SettingsXML    NCLOB             DEFAULT '',
    ApiType        VARCHAR2(50)      DEFAULT '',
    CONSTRAINT PK_bairong_PayRecord PRIMARY KEY (RecordID)
)
GO

CREATE TABLE bairong_UserAddCard(
    CardID         NUMBER(38, 0)     NOT NULL,
    CardCount      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    BuyTime        TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IP             VARCHAR2(50)      DEFAULT '',
    SettingsXML    NCLOB             DEFAULT '',
    UserName       NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_UserAddCard PRIMARY KEY (CardID)
)
GO



CREATE TABLE bairong_UserBinding(
    UserName       NVARCHAR2(255)    DEFAULT '' NOT NULL,
    BindingType    VARCHAR2(50)      DEFAULT '',
    BindingID      NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    BindingName    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_UserBinding PRIMARY KEY (UserName)
)
GO

CREATE TABLE bairong_UserConsume(
    ConsumeID      NUMBER(38, 0)     NOT NULL,
    Consumed       VARCHAR2(50)      DEFAULT '',
    ConsumeTime    TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IP             VARCHAR2(50)      DEFAULT '',
    Description    VARCHAR2(255)     DEFAULT '',
    UserName       NVARCHAR2(255)    DEFAULT '',
	ConsumedCount    int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_UserConsume PRIMARY KEY (ConsumeID)
)
GO



ALTER TABLE bairong_Users ADD 
	IsTemporary         VARCHAR2(18)      DEFAULT '',
	PointCount          NUMBER(38, 0)     DEFAULT 0 NOT NULL,
	IP                  VARCHAR2(50)      DEFAULT ''

GO