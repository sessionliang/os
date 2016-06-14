ALTER TABLE siteserver_GovPublicIdentifierRule ADD 
    SettingsXML            ntext            DEFAULT '' NOT NULL

go

CREATE TABLE siteserver_GovPublicIdentifierSeq(
    SeqID                  int    IDENTITY(1,1),
    PublishmentSystemID    int    DEFAULT 0 NOT NULL,
    NodeID                 int    DEFAULT 0 NOT NULL,
    DepartmentID           int    DEFAULT 0 NOT NULL,
    AddYear                int    DEFAULT 0 NOT NULL,
    Sequence               int    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_GovPublicIdentifierSeq PRIMARY KEY NONCLUSTERED (SeqID)
)
go