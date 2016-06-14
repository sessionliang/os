CREATE TABLE siteserver_Ad(
    AdName                 nvarchar(50)     NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    AdType                 varchar(50)      DEFAULT '' NOT NULL,
    Code                   ntext            DEFAULT '' NOT NULL,
    TextWord               nvarchar(255)    DEFAULT '' NOT NULL,
    TextLink               varchar(200)     DEFAULT '' NOT NULL,
    TextColor              varchar(10)      DEFAULT '' NOT NULL,
    TextFontSize           int              DEFAULT 0 NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    ImageLink              varchar(200)     DEFAULT '' NOT NULL,
    ImageWidth             int              DEFAULT 0 NOT NULL,
    ImageHeight            int              DEFAULT 0 NOT NULL,
    ImageAlt               nvarchar(50)     DEFAULT '' NOT NULL,
    IsEnabled              varchar(18)      DEFAULT '' NOT NULL,
    IsDateLimited          varchar(18)      DEFAULT '' NOT NULL,
    StartDate              datetime         DEFAULT getdate() NOT NULL,
    EndDate                datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_siteserver_Ad PRIMARY KEY NONCLUSTERED (AdName, PublishmentSystemID)
)

GO

ALTER TABLE siteserver_Node ADD 
    LinkUrl                    varchar(200)     DEFAULT '' NOT NULL,
    LinkType                   varchar(200)     DEFAULT '' NOT NULL,
    ChannelTemplateID          int              DEFAULT 0 NOT NULL,
    ContentTemplateID          int              DEFAULT 0 NOT NULL

GO

UPDATE siteserver_Node SET LinkUrl = siteserver_BackgroundNode.LinkUrl FROM siteserver_BackgroundNode WHERE siteserver_Node.NodeID = siteserver_BackgroundNode.NodeID

GO

UPDATE siteserver_Node SET LinkType = siteserver_BackgroundNode.LinkType FROM siteserver_BackgroundNode WHERE siteserver_Node.NodeID = siteserver_BackgroundNode.NodeID

GO

UPDATE siteserver_Node SET ChannelTemplateID = siteserver_BackgroundNode.ChannelTemplateID FROM siteserver_BackgroundNode WHERE siteserver_Node.NodeID = siteserver_BackgroundNode.NodeID

GO

UPDATE siteserver_Node SET ContentTemplateID = siteserver_BackgroundNode.ContentTemplateID FROM siteserver_BackgroundNode WHERE siteserver_Node.NodeID = siteserver_BackgroundNode.NodeID

GO