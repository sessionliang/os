DROP TABLE wcm_Comment
go



CREATE TABLE wcm_Comment(
    CommentID              counter NOT NULL,
    CommentName            text(50)    DEFAULT "" NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    ApplyStyleID           integer      DEFAULT 0 NOT NULL,
    QueryStyleID           integer      DEFAULT 0 NOT NULL,
    Taxis                  integer      DEFAULT 0 NOT NULL,
    Summary                text(255)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_Comment PRIMARY KEY (CommentID)
)
go



CREATE TABLE wcm_CommentContent(
    ID                     counter NOT NULL,
    CommentID              integer      DEFAULT 0 NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    NodeID                 integer      DEFAULT 0 NOT NULL,
    ContentID              integer      DEFAULT 0 NOT NULL,
    ReferenceID            integer      DEFAULT 0 NOT NULL,
    Good                   integer      DEFAULT 0 NOT NULL,
    Bad                    integer      DEFAULT 0 NOT NULL,
    UserName               text(255)    DEFAULT "" NOT NULL,
    IPAddress              text(50)    DEFAULT "" NOT NULL,
    Location               text(255)    DEFAULT "" NOT NULL,
    AddDate                time         DEFAULT Now() NOT NULL,
    Taxis                  integer      DEFAULT 0 NOT NULL,
    IsChecked              text(18)    DEFAULT "" NOT NULL,
    Attribute1             text(255)    DEFAULT "" NOT NULL,
    Attribute2             text(255)    DEFAULT "" NOT NULL,
    Attribute3             text(255)    DEFAULT "" NOT NULL,
    Attribute4             text(255)    DEFAULT "" NOT NULL,
    Attribute5             text(255)    DEFAULT "" NOT NULL,
    Content                memo         DEFAULT "" NOT NULL,
    SettingsXML            memo         DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_CommentContent PRIMARY KEY (ID)
)
go


ALTER TABLE wcm_GatherFileRule ADD 
    ContentHtmlClearTagCollection    text(255)    DEFAULT "" NOT NULL
go


ALTER TABLE wcm_GatherRule ADD 
    ContentHtmlClearTagCollection    text(255)    DEFAULT "" NOT NULL
go


CREATE TABLE wcm_Keyword(
    KeywordID      counter NOT NULL,
    Keyword        text(50)    DEFAULT "" NOT NULL,
    Alternative    text(50)    DEFAULT "" NOT NULL,
    Grade          text(50)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_Keyword PRIMARY KEY (KeywordID)
)
go


ALTER TABLE wcm_PublishmentSystem ADD 
    AuxiliaryTableForGovPublic      text(50)    DEFAULT "" NOT NULL,
    AuxiliaryTableForGovInteract    text(50)    DEFAULT "" NOT NULL,
    AuxiliaryTableForVote           text(50)    DEFAULT "" NOT NULL
go


UPDATE wcm_PublishmentSystem SET AuxiliaryTableForVote = 'siteserver_ContentVote'
go


CREATE TABLE wcm_SigninLog(
    ID                     counter NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    ContentID              integer      DEFAULT 0 NOT NULL,
    UserName               text(255)    DEFAULT "" NOT NULL,
    IsSignin               text(18)    DEFAULT "" NOT NULL,
    SigninDate             time         DEFAULT Now() NOT NULL,
    IPAddress              text(50)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_SigninLog PRIMARY KEY (ID)
)
go



CREATE TABLE wcm_SigninSetting(
    ID                     counter NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    NodeID                 integer      DEFAULT 0 NOT NULL,
    ContentID              integer      DEFAULT 0 NOT NULL,
    IsGroup                text(18)    DEFAULT "" NOT NULL,
    UserGroupCollection    text(255)    DEFAULT "" NOT NULL,
    UserNameCollection     text(255)    DEFAULT "" NOT NULL,
    Priority               integer      DEFAULT 0 NOT NULL,
    EndDate                text(50)    DEFAULT "" NOT NULL,
    IsSignin               text(18)    DEFAULT "" NOT NULL,
    SigninDate             time         DEFAULT Now() NOT NULL,
    CONSTRAINT PK_wcm_SigninSetting PRIMARY KEY (ID)
)
go



CREATE TABLE wcm_SigninUserContentID(
    ID                     counter NOT NULL,
    IsGroup                text(18)    DEFAULT "" NOT NULL,
    GroupID                integer      DEFAULT 0 NOT NULL,
    UserName               text(255)    DEFAULT "" NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    NodeID                 integer      DEFAULT 0 NOT NULL,
    ContentIDCollection    text(255)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_SigninUserContentID PRIMARY KEY (ID)
)
go


ALTER TABLE wcm_TagStyle ADD 
    SuccessTemplate        memo         DEFAULT "" NOT NULL,
    FailureTemplate        memo         DEFAULT "" NOT NULL
go


CREATE TABLE wcm_TouGaoSetting(
    SettingID                    counter NOT NULL,
    PublishmentSystemID          int             DEFAULT 0 NOT NULL,
    UserTypeID                   int             DEFAULT 0 NOT NULL,
    IsTouGaoAllowed              text(18)    DEFAULT "" NOT NULL,
    CheckLevel                   int             DEFAULT 0 NOT NULL,
    IsCheckAllowed               text(18)    DEFAULT "" NOT NULL,
    IsCheckAddedUsersOnly        text(18)    DEFAULT "" NOT NULL,
    CheckUserTypeIDCollection    text(200)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_wcm_TouGaoSetting PRIMARY KEY (SettingID)
)
go


CREATE TABLE wcm_VoteOperation(
    OperationID            counter NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    NodeID                 integer      DEFAULT 0 NOT NULL,
    ContentID              integer      DEFAULT 0 NOT NULL,
    IPAddress              text(50)    DEFAULT "" NOT NULL,
    UserName               text(255)    DEFAULT "" NOT NULL,
    AddDate                time         DEFAULT Now() NOT NULL,
    CONSTRAINT PK_wcm_VoteOperation PRIMARY KEY (OperationID)
)
go



CREATE TABLE wcm_VoteOption(
    OptionID               counter NOT NULL,
    PublishmentSystemID    integer      DEFAULT 0 NOT NULL,
    NodeID                 integer      DEFAULT 0 NOT NULL,
    ContentID              integer      DEFAULT 0 NOT NULL,
    Title                  text(255)    DEFAULT "" NOT NULL,
    ImageUrl               text(200)    DEFAULT "" NOT NULL,
    NavigationUrl          text(200)    DEFAULT "" NOT NULL,
    VoteNum                integer      DEFAULT 0 NOT NULL,
    CONSTRAINT PK_wcm_VoteOption PRIMARY KEY (OptionID)
)
go