ALTER TABLE bairong_TableCollection ADD 
    ProductID                    varchar(50)    DEFAULT '' NOT NULL,
    IsDefault                    varchar(18)    DEFAULT '' NOT NULL

GO

CREATE TABLE bairong_ContentModel(
    ModelID        varchar(50)      NOT NULL,
    ProductID      varchar(50)      DEFAULT '' NOT NULL,
    SiteID         int              DEFAULT 0 NOT NULL,
    ModelName      nvarchar(50)     DEFAULT '' NOT NULL,
    IsSystem       varchar(18)      DEFAULT '' NOT NULL,
    TableName      varchar(200)     DEFAULT '' NOT NULL,
    TableType      varchar(50)      DEFAULT '' NOT NULL,
    IconUrl        varchar(50)      DEFAULT '' NOT NULL,
    Description    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ContentModel PRIMARY KEY NONCLUSTERED (ModelID)
)

GO

CREATE TABLE bairong_Tags(
    TagID                  int              IDENTITY(1,1),
    ProductID              varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentIDCollection    nvarchar(255)    DEFAULT '' NOT NULL,
    Tag                    nvarchar(255)    DEFAULT '' NOT NULL,
    UseNum                 int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Tags PRIMARY KEY NONCLUSTERED (TagID)
)

GO