ALTER TABLE bbs_Users ADD 
    LastPostDate       datetime         DEFAULT getdate() NOT NULL

GO

ALTER TABLE bbs_Post ADD 
    LastEditUserName nvarchar(50)    DEFAULT '' NOT NULL,
	LastEditDate	datetime	     DEFAULT getdate() NOT NULL

GO