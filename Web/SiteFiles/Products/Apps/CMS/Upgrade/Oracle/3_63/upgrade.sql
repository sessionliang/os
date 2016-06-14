ALTER TABLE siteserver_GovPublicIdentifierRule ADD 
	SettingsXML            NCLOB             DEFAULT ''
go

CREATE TABLE siteserver_GovPublicIdentifierSeq(
    SeqID                  NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    DepartmentID           NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    AddYear                NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    Sequence               NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_GovPublicIdentifierSeq PRIMARY KEY (SeqID)
)

go

CREATE SEQUENCE SITESERVER_GOVPUBLICIS_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER

go