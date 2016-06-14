ALTER TABLE siteserver_Tracking ADD 
    UserID          nvarchar(255)    DEFAULT '' NOT NULL
go

CREATE TABLE siteserver_Users(
    UserID                 nvarchar(255)    NOT NULL,
    PublishmentSystemID    int              NOT NULL,
    Theme                  varchar(50)      DEFAULT '' NOT NULL,
    ExtendValues           ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_Users PRIMARY KEY NONCLUSTERED (UserID)
)
go