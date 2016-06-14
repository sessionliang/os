ALTER TABLE bairong_TableCollection ADD (
    ProductID                    VARCHAR2(50)    DEFAULT '',
    IsDefault                    VARCHAR2(18)    DEFAULT ''
)

GO

CREATE TABLE bairong_ContentModel(
    ModelID        VARCHAR2(50)      NOT NULL,
    ProductID      VARCHAR2(50)      DEFAULT '',
    SiteID         NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ModelName      NVARCHAR2(50)     DEFAULT '',
    IsSystem       VARCHAR2(18)      DEFAULT '',
    TableName      VARCHAR2(200)     DEFAULT '',
    TableType      VARCHAR2(50)      DEFAULT '',
    IconUrl        VARCHAR2(50)      DEFAULT '',
    Description    NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_bairong_ContentModel PRIMARY KEY (ModelID)
)

GO

CREATE TABLE bairong_Tags(
    TagID                  NUMBER(38, 0)     NOT NULL,
    ProductID              VARCHAR2(50)      DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentIDCollection    NVARCHAR2(255)    DEFAULT '',
    Tag                    NVARCHAR2(255)    DEFAULT '',
    UseNum                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Tags PRIMARY KEY (TagID)
)

GO

CREATE SEQUENCE BAIRONG_TAGS_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER

GO