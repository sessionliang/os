CREATE TABLE siteserver_Comment(
    ID                     counter NOT NULL,
    PublishmentSystemID    integer              DEFAULT 0 NOT NULL,
    NodeID                 integer              DEFAULT 0 NOT NULL,
    ContentID              integer              DEFAULT 0 NOT NULL,
    ReferenceID            integer              DEFAULT 0 NOT NULL,
    Good                   integer              DEFAULT 0 NOT NULL,
    Bad                    integer              DEFAULT 0 NOT NULL,
    UserName               text(255)    DEFAULT "" NOT NULL,
    IPAddress              text(50)      DEFAULT "" NOT NULL,
    Location               text(255)    DEFAULT "" NOT NULL,
    AddDate                time         DEFAULT Now() NOT NULL,
    Taxis                  integer              DEFAULT 0 NOT NULL,
    IsChecked              text(18)      DEFAULT "" NOT NULL,
    Title                  text(255)    DEFAULT "" NOT NULL,
    Content                memo            DEFAULT "" NOT NULL,
    SettingsXML            memo            DEFAULT "" NOT NULL,
    CONSTRAINT PK_siteserver_Comment PRIMARY KEY (ID)
)

GO