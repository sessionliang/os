CREATE TABLE siteserver_TemplateLog(
    ID                     int              IDENTITY(1,1),
    TemplateID             int              NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AddDate                datetime         DEFAULT getdate() NOT NULL,
    AddUserName            nvarchar(255)    DEFAULT '' NOT NULL,
    ContentLength          int              DEFAULT 0 NOT NULL,
    TemplateContent        ntext            DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_TemplateLog PRIMARY KEY NONCLUSTERED (ID), 
    CONSTRAINT FK_Template_Log FOREIGN KEY (TemplateID)
    REFERENCES siteserver_Template(TemplateID) ON DELETE CASCADE ON UPDATE CASCADE
)
go