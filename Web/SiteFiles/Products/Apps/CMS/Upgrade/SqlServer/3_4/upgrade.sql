exec sp_rename 'siteserver_InputContent.AddUserID','UserName','column'

GO

exec sp_rename 'siteserver_Star.UserID','UserName','column'

GO

exec sp_rename 'siteserver_Tracking.UserID','UserName','column'

GO

ALTER TABLE siteserver_TemplateRule ADD 
    IsCreateChannels           varchar(18)     DEFAULT '' NOT NULL,
    IsCreateContents           varchar(18)     DEFAULT '' NOT NULL

GO