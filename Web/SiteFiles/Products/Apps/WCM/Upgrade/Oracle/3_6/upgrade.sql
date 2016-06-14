DROP TABLE wcm_Comment
go



CREATE TABLE wcm_Comment(
    CommentID              NUMBER(38, 0)     NOT NULL,
    CommentName            NVARCHAR2(50)     DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ApplyStyleID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    QueryStyleID           NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Summary                NVARCHAR2(255)    DEFAULT '',
    CONSTRAINT PK_wcm_Comment PRIMARY KEY (CommentID), 
    CONSTRAINT FK_wcm_Node_Comment FOREIGN KEY (PublishmentSystemID)
    REFERENCES wcm_Node(NodeID) ON DELETE CASCADE
)
go


CREATE SEQUENCE WCM_COMMENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go



CREATE TABLE wcm_CommentContent(
    ID                     NUMBER(38, 0)     NOT NULL,
    CommentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ReferenceID            NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Good                   NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Bad                    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    IPAddress              VARCHAR2(50)      DEFAULT '',
    Location               NVARCHAR2(255)    DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    Taxis                  NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsChecked              VARCHAR2(18)      DEFAULT '',
    Attribute1             NVARCHAR2(255)    DEFAULT '',
    Attribute2             NVARCHAR2(255)    DEFAULT '',
    Attribute3             NVARCHAR2(255)    DEFAULT '',
    Attribute4             NVARCHAR2(255)    DEFAULT '',
    Attribute5             NVARCHAR2(255)    DEFAULT '',
    Content                NCLOB             DEFAULT '',
    SettingsXML            NCLOB             DEFAULT '',
    CONSTRAINT PK_wcm_CommentContent PRIMARY KEY (ID)
)
go


CREATE SEQUENCE WCM_COMMENTCONTENT_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


ALTER TABLE wcm_GatherFileRule ADD 
    ContentHtmlClearTagCollection    NVARCHAR2(255)    DEFAULT ''
go


ALTER TABLE wcm_GatherRule ADD 
    ContentHtmlClearTagCollection    NVARCHAR2(255)    DEFAULT ''
go


CREATE TABLE wcm_Keyword(
    KeywordID      NUMBER(38, 0)    NOT NULL,
    Keyword        NVARCHAR2(50)    DEFAULT '',
    Alternative    NVARCHAR2(50)    DEFAULT '',
    Grade          NVARCHAR2(50)    DEFAULT '',
    CONSTRAINT PK_wcm_Keyword PRIMARY KEY (KeywordID)
)
go


CREATE SEQUENCE WCM_KEYWORD_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


ALTER TABLE wcm_PublishmentSystem ADD 
    AuxiliaryTableForGovPublic      VARCHAR2(50)     DEFAULT '',
    AuxiliaryTableForGovInteract    VARCHAR2(50)     DEFAULT '',
    AuxiliaryTableForVote           VARCHAR2(50)     DEFAULT ''
go


UPDATE wcm_PublishmentSystem SET AuxiliaryTableForVote = 'siteserver_ContentVote'
go


CREATE TABLE wcm_SigninLog(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    IsSignin               VARCHAR2(18)      DEFAULT '',
    SigninDate             TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    IPAddress              VARCHAR2(50)      DEFAULT '',
    CONSTRAINT PK_wcm_SigninLog PRIMARY KEY (ID)
)
go


CREATE SEQUENCE WCM_SIGNINLOG_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go



CREATE TABLE wcm_SigninSetting(
    ID                     NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IsGroup                VARCHAR2(18)      DEFAULT '',
    UserGroupCollection    VARCHAR2(500)     DEFAULT '',
    UserNameCollection     NVARCHAR2(500)    DEFAULT '',
    Priority               NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    EndDate                VARCHAR2(50)      DEFAULT '',
    IsSignin               VARCHAR2(18)      DEFAULT '',
    SigninDate             TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_wcm_SigninSetting PRIMARY KEY (ID)
)
go



CREATE SEQUENCE WCM_SIGNINSETTING_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go



CREATE TABLE wcm_SigninUserContentID(
    ID                     NUMBER(38, 0)     NOT NULL,
    IsGroup                VARCHAR2(18)      DEFAULT '',
    GroupID                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    UserName               NVARCHAR2(255)    DEFAULT '',
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentIDCollection    VARCHAR2(500)     DEFAULT '',
    CONSTRAINT PK_wcm_SigninUserContentID PRIMARY KEY (ID)
)
go


CREATE SEQUENCE WCM_SIGNINUSERCONTENTID_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


ALTER TABLE wcm_TagStyle ADD 
    SuccessTemplate        NCLOB            DEFAULT '',
    FailureTemplate        NCLOB            DEFAULT ''
go


CREATE TABLE wcm_TouGaoSetting(
    SettingID                    NUMBER(38, 0)    NOT NULL,
    PublishmentSystemID          NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    UserTypeID                   NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsTouGaoAllowed              VARCHAR2(18)     DEFAULT '',
    CheckLevel                   NUMBER(38, 0)    DEFAULT 0 NOT NULL,
    IsCheckAllowed               VARCHAR2(18)     DEFAULT '',
    IsCheckAddedUsersOnly        VARCHAR2(18)     DEFAULT '',
    CheckUserTypeIDCollection    VARCHAR2(200)    DEFAULT '',
    CONSTRAINT PK_wcm_TouGaoSetting PRIMARY KEY (SettingID)
)
go


CREATE SEQUENCE WCM_TOUGAOSETTING_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go


CREATE TABLE wcm_VoteOperation(
    OperationID            NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    IPAddress              VARCHAR2(50)      DEFAULT '',
    UserName               NVARCHAR2(255)    DEFAULT '',
    AddDate                TIMESTAMP(6)      DEFAULT sysdate NOT NULL,
    CONSTRAINT PK_wcm_VoteOperation PRIMARY KEY (OperationID)
)
go


CREATE SEQUENCE WCM_VOTEOPERATION_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go



CREATE TABLE wcm_VoteOption(
    OptionID               NUMBER(38, 0)     NOT NULL,
    PublishmentSystemID    NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    NodeID                 NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    ContentID              NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    Title                  NVARCHAR2(255)    DEFAULT '',
    ImageUrl               VARCHAR2(200)     DEFAULT '',
    NavigationUrl          VARCHAR2(200)     DEFAULT '',
    VoteNum                NUMBER(38, 0)     DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_VoteOption PRIMARY KEY (OptionID)
)
go


CREATE SEQUENCE WCM_VOTEOPTION_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go