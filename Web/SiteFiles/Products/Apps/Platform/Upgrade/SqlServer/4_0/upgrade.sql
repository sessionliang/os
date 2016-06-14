ALTER TABLE bairong_Administrator ADD
LastProductID                    varchar(50)      DEFAULT '' NOT NULL,
PublishmentSystemIDCollection    varchar(50)      DEFAULT '' NOT NULL

GO

CREATE TABLE bairong_AjaxUrl(
    SN            varchar(50)     NOT NULL,
    AjaxUrl       varchar(200)    DEFAULT '' NOT NULL,
    Parameters    ntext           DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_AjaxUrl PRIMARY KEY NONCLUSTERED (SN)
)

GO