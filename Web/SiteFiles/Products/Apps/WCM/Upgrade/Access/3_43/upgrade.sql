ALTER TABLE siteserver_Input ADD 
    StyleTemplate          memo           DEFAULT "" NOT NULL,
    ScriptTemplate         memo           DEFAULT "" NOT NULL,
    ContentTemplate        memo           DEFAULT "" NOT NULL
	
GO

ALTER TABLE siteserver_PublishmentSystem ADD 
	AuxiliaryTableForJob         text(50)     DEFAULT "" NOT NULL

GO

ALTER TABLE siteserver_Node ADD 
    ContentModelID             text(50)      DEFAULT "" NOT NULL,
	Keywords                   text(255)    DEFAULT "" NOT NULL,
    [Description]              text(255)    DEFAULT "" NOT NULL

GO

CREATE TABLE siteserver_ResumeContent(
    ID             counter NOT NULL,
    StyleID                  integer              DEFAULT 0 NOT NULL,
    PublishmentSystemID      integer              DEFAULT 0 NOT NULL,
    JobContentID             integer              DEFAULT 0 NOT NULL,
    UserName                 text(255)    DEFAULT "" NOT NULL,
    IsView                   text(18)      DEFAULT "" NOT NULL,
    AddDate                  time         DEFAULT Now() NOT NULL,
    RealName                 text(50)     DEFAULT "" NOT NULL,
    Nationality              text(50)     DEFAULT "" NOT NULL,
    Gender                   text(50)     DEFAULT "" NOT NULL,
    Email                    text(50)      DEFAULT "" NOT NULL,
    MobilePhone              text(50)      DEFAULT "" NOT NULL,
    HomePhone                text(50)      DEFAULT "" NOT NULL,
    LastSchoolName           text(50)     DEFAULT "" NOT NULL,
    Education                text(50)     DEFAULT "" NOT NULL,
    IDCardType               text(50)     DEFAULT "" NOT NULL,
    IDCardNo                 text(50)      DEFAULT "" NOT NULL,
    Birthday                 text(50)      DEFAULT "" NOT NULL,
    Marriage                 text(50)     DEFAULT "" NOT NULL,
    WorkYear                 text(50)     DEFAULT "" NOT NULL,
    Profession               text(50)     DEFAULT "" NOT NULL,
    ExpectSalary             text(50)     DEFAULT "" NOT NULL,
    AvailabelTime            text(50)     DEFAULT "" NOT NULL,
    Location                 text(50)     DEFAULT "" NOT NULL,
    ImageUrl                 text(200)     DEFAULT "" NOT NULL,
    Summary                  text(255)    DEFAULT "" NOT NULL,
    Exp_Count                integer              DEFAULT 0 NOT NULL,
    Exp_FromYear             text(50)     DEFAULT "" NOT NULL,
    Exp_FromMonth            text(50)     DEFAULT "" NOT NULL,
    Exp_ToYear               text(50)     DEFAULT "" NOT NULL,
    Exp_ToMonth              text(50)     DEFAULT "" NOT NULL,
    Exp_EmployerName         text(255)    DEFAULT "" NOT NULL,
    Exp_Department           text(255)    DEFAULT "" NOT NULL,
    Exp_EmployerPhone        text(255)    DEFAULT "" NOT NULL,
    Exp_WorkPlace            text(255)    DEFAULT "" NOT NULL,
    Exp_PositionTitle        text(255)    DEFAULT "" NOT NULL,
    Exp_Industry             text(255)    DEFAULT "" NOT NULL,
    Exp_Summary              memo            DEFAULT "" NOT NULL,
    Exp_Score                memo            DEFAULT "" NOT NULL,
    Pro_Count                integer              DEFAULT 0 NOT NULL,
    Pro_FromYear             text(50)     DEFAULT "" NOT NULL,
    Pro_FromMonth            text(50)     DEFAULT "" NOT NULL,
    Pro_ToYear               text(50)     DEFAULT "" NOT NULL,
    Pro_ToMonth              text(50)     DEFAULT "" NOT NULL,
    Pro_ProjectName          text(255)    DEFAULT "" NOT NULL,
    Pro_Summary              memo            DEFAULT "" NOT NULL,
    Edu_Count                integer              DEFAULT 0 NOT NULL,
    Edu_FromYear             text(50)     DEFAULT "" NOT NULL,
    Edu_FromMonth            text(50)     DEFAULT "" NOT NULL,
    Edu_ToYear               text(50)     DEFAULT "" NOT NULL,
    Edu_ToMonth              text(50)     DEFAULT "" NOT NULL,
    Edu_SchoolName           text(255)    DEFAULT "" NOT NULL,
    Edu_Education            text(255)    DEFAULT "" NOT NULL,
    Edu_Profession           text(255)    DEFAULT "" NOT NULL,
    Edu_Summary              memo            DEFAULT "" NOT NULL,
    Tra_Count                integer              DEFAULT 0 NOT NULL,
    Tra_FromYear             text(50)     DEFAULT "" NOT NULL,
    Tra_FromMonth            text(50)     DEFAULT "" NOT NULL,
    Tra_ToYear               text(50)     DEFAULT "" NOT NULL,
    Tra_ToMonth              text(50)     DEFAULT "" NOT NULL,
    Tra_TrainerName          text(255)    DEFAULT "" NOT NULL,
    Tra_TrainerAddress       text(255)    DEFAULT "" NOT NULL,
    Tra_Lesson               text(255)    DEFAULT "" NOT NULL,
    Tra_Centification        text(255)    DEFAULT "" NOT NULL,
    Tra_Summary              text(255)    DEFAULT "" NOT NULL,
    Lan_Count                integer              DEFAULT 0 NOT NULL,
    Lan_Language             text(255)    DEFAULT "" NOT NULL,
    Lan_Level                text(255)    DEFAULT "" NOT NULL,
    Ski_Count                integer              DEFAULT 0 NOT NULL,
    Ski_SkillName            text(255)    DEFAULT "" NOT NULL,
    Ski_UsedTimes            text(255)    DEFAULT "" NOT NULL,
    Ski_Ability              text(255)    DEFAULT "" NOT NULL,
    Cer_Count                integer              DEFAULT 0 NOT NULL,
    Cer_CertificationName    text(255)    DEFAULT "" NOT NULL,
    Cer_EffectiveDate        text(255)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_siteserver_ResumeContent PRIMARY KEY (ID)
)

GO

CREATE TABLE siteserver_PhotoContent(
    ID             counter NOT NULL,
    PublishmentSystemID    integer              DEFAULT 0 NOT NULL,
    ContentID              integer              DEFAULT 0 NOT NULL,
    PreviewUrl             text(200)     DEFAULT "" NOT NULL,
    ImageUrl               text(200)     DEFAULT "" NOT NULL,
    Taxis                  integer              DEFAULT 0 NOT NULL,
    Title                  text(255)    DEFAULT "" NOT NULL,
    Description            text(255)     DEFAULT "" NOT NULL,
    CONSTRAINT PK_siteserver_PhotoContent PRIMARY KEY (ID)
)

GO

CREATE TABLE siteserver_RelatedField(
	RelatedFieldID             counter NOT NULL,
    RelatedFieldName       text(50)     DEFAULT "" NOT NULL,
    PublishmentSystemID    integer              DEFAULT 0 NOT NULL,
    TotalLevel             integer              DEFAULT 0 NOT NULL,
    Prefixes               text(255)    DEFAULT "" NOT NULL,
    Suffixes               text(255)    DEFAULT "" NOT NULL,
    CONSTRAINT PK_siteserver_RelatedField PRIMARY KEY (RelatedFieldID)
)

GO

CREATE TABLE siteserver_RelatedFieldItem(
    ID             counter NOT NULL,
    RelatedFieldID    integer              NOT NULL,
    ItemName          text(255)    DEFAULT "" NOT NULL,
    ItemValue         text(255)    DEFAULT "" NOT NULL,
    ParentID          integer              DEFAULT 0 NOT NULL,
    Taxis             integer              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_RelatedFieldItem PRIMARY KEY (ID)
)

GO