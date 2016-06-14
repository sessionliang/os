ALTER TABLE siteserver_PublishmentSystem ADD 
	AuxiliaryTableForJob         varchar(50)     DEFAULT '' NOT NULL

GO

ALTER TABLE siteserver_Node ADD 
    ContentModelID             varchar(50)      DEFAULT '' NOT NULL,
	Keywords                   nvarchar(255)    DEFAULT '' NOT NULL,
    [Description]              nvarchar(255)    DEFAULT '' NOT NULL

GO

CREATE TABLE siteserver_ResumeContent(
    ID                       int              IDENTITY(1,1),
    StyleID                  int              DEFAULT 0 NOT NULL,
    PublishmentSystemID      int              DEFAULT 0 NOT NULL,
    JobContentID             int              DEFAULT 0 NOT NULL,
    UserName                 nvarchar(255)    DEFAULT '' NOT NULL,
    IsView                   varchar(18)      DEFAULT '' NOT NULL,
    AddDate                  datetime         DEFAULT getdate() NOT NULL,
    RealName                 nvarchar(50)     DEFAULT '' NOT NULL,
    Nationality              nvarchar(50)     DEFAULT '' NOT NULL,
    Gender                   nvarchar(50)     DEFAULT '' NOT NULL,
    Email                    varchar(50)      DEFAULT '' NOT NULL,
    MobilePhone              varchar(50)      DEFAULT '' NOT NULL,
    HomePhone                varchar(50)      DEFAULT '' NOT NULL,
    LastSchoolName           nvarchar(50)     DEFAULT '' NOT NULL,
    Education                nvarchar(50)     DEFAULT '' NOT NULL,
    IDCardType               nvarchar(50)     DEFAULT '' NOT NULL,
    IDCardNo                 varchar(50)      DEFAULT '' NOT NULL,
    Birthday                 varchar(50)      DEFAULT '' NOT NULL,
    Marriage                 nvarchar(50)     DEFAULT '' NOT NULL,
    WorkYear                 nvarchar(50)     DEFAULT '' NOT NULL,
    Profession               nvarchar(50)     DEFAULT '' NOT NULL,
    ExpectSalary             nvarchar(50)     DEFAULT '' NOT NULL,
    AvailabelTime            nvarchar(50)     DEFAULT '' NOT NULL,
    Location                 nvarchar(50)     DEFAULT '' NOT NULL,
    ImageUrl                 varchar(200)     DEFAULT '' NOT NULL,
    Summary                  nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Count                int              DEFAULT 0 NOT NULL,
    Exp_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Exp_EmployerName         nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Department           nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_EmployerPhone        nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_WorkPlace            nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_PositionTitle        nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Industry             nvarchar(255)    DEFAULT '' NOT NULL,
    Exp_Summary              ntext            DEFAULT '' NOT NULL,
    Exp_Score                ntext            DEFAULT '' NOT NULL,
    Pro_Count                int              DEFAULT 0 NOT NULL,
    Pro_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Pro_ProjectName          nvarchar(255)    DEFAULT '' NOT NULL,
    Pro_Summary              ntext            DEFAULT '' NOT NULL,
    Edu_Count                int              DEFAULT 0 NOT NULL,
    Edu_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Edu_SchoolName           nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Education            nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Profession           nvarchar(255)    DEFAULT '' NOT NULL,
    Edu_Summary              ntext            DEFAULT '' NOT NULL,
    Tra_Count                int              DEFAULT 0 NOT NULL,
    Tra_FromYear             nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_FromMonth            nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_ToYear               nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_ToMonth              nvarchar(50)     DEFAULT '' NOT NULL,
    Tra_TrainerName          nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_TrainerAddress       nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Lesson               nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Centification        nvarchar(255)    DEFAULT '' NOT NULL,
    Tra_Summary              nvarchar(255)    DEFAULT '' NOT NULL,
    Lan_Count                int              DEFAULT 0 NOT NULL,
    Lan_Language             nvarchar(255)    DEFAULT '' NOT NULL,
    Lan_Level                nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_Count                int              DEFAULT 0 NOT NULL,
    Ski_SkillName            nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_UsedTimes            nvarchar(255)    DEFAULT '' NOT NULL,
    Ski_Ability              nvarchar(255)    DEFAULT '' NOT NULL,
    Cer_Count                int              DEFAULT 0 NOT NULL,
    Cer_CertificationName    nvarchar(255)    DEFAULT '' NOT NULL,
    Cer_EffectiveDate        nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_ResumeContent PRIMARY KEY NONCLUSTERED (ID)
)

GO

CREATE TABLE siteserver_PhotoContent(
    ID                     int              IDENTITY(1,1),
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    PreviewUrl             varchar(200)     DEFAULT '' NOT NULL,
    ImageUrl               varchar(200)     DEFAULT '' NOT NULL,
    Taxis                  int              DEFAULT 0 NOT NULL,
    Title                  nvarchar(255)    DEFAULT '' NOT NULL,
    Description            varchar(255)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_PhotoContent PRIMARY KEY NONCLUSTERED (ID)
)

GO

CREATE TABLE siteserver_RelatedField(
    RelatedFieldID         int              IDENTITY(1,1),
    RelatedFieldName       nvarchar(50)     DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    TotalLevel             int              DEFAULT 0 NOT NULL,
    Prefixes               nvarchar(255)    DEFAULT '' NOT NULL,
    Suffixes               nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_siteserver_RelatedField PRIMARY KEY NONCLUSTERED (RelatedFieldID)
)

GO

CREATE TABLE siteserver_RelatedFieldItem(
    ID                int              IDENTITY(1,1),
    RelatedFieldID    int              NOT NULL,
    ItemName          nvarchar(255)    DEFAULT '' NOT NULL,
    ItemValue         nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID          int              DEFAULT 0 NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_siteserver_RelatedFieldItem PRIMARY KEY NONCLUSTERED (ID), 
    CONSTRAINT FK_siteserver_RFI_RF FOREIGN KEY (RelatedFieldID)
    REFERENCES siteserver_RelatedField(RelatedFieldID) ON DELETE CASCADE ON UPDATE CASCADE
)

GO