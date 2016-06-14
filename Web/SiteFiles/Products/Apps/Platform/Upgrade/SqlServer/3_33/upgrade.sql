ALTER TABLE siteserver_Configuration ADD 
    UpdateDate              datetime         DEFAULT getdate() NOT NULL
go

DROP TABLE bairong_Star
go

CREATE TABLE siteserver_Star(
    StarID                 int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ChannelID              int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    UserID                 nvarchar(255)    DEFAULT '' NOT NULL,
    Point                  int              DEFAULT 0 NOT NULL,
    Message                ntext            DEFAULT '' NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_Star PRIMARY KEY NONCLUSTERED (StarID)
)