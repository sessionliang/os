ALTER TABLE bairong_Users ADD (
    OnlineSeconds       NUMBER(38, 0)     DEFAULT 0 NOT NULL,
	AvatarLarge            VARCHAR2(200)      DEFAULT '',
    AvatarMiddle           VARCHAR2(200)      DEFAULT '',
    AvatarSmall            VARCHAR2(200)      DEFAULT ''
)

GO