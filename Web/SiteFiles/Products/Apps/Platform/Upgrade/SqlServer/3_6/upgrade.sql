ALTER TABLE bairong_Administrator ADD 
    DepartmentID           int              DEFAULT 0 NOT NULL,
    AreaID                 int              DEFAULT 0 NOT NULL,
    Mobile                 varchar(20)      DEFAULT '' NOT NULL
go


CREATE TABLE bairong_Area(
    AreaID           int              IDENTITY(1,1),
    AreaName         nvarchar(255)    DEFAULT '' NOT NULL,
    ParentID         int              DEFAULT 0 NOT NULL,
    ParentsPath      nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount     int              DEFAULT 0 NOT NULL,
    ChildrenCount    int              DEFAULT 0 NOT NULL,
    IsLastNode       varchar(18)      DEFAULT '' NOT NULL,
    Taxis            int              DEFAULT 0 NOT NULL,
    CountOfAdmin     int              DEFAULT 0 NOT NULL,
    CountOfUser      int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Area PRIMARY KEY NONCLUSTERED (AreaID)
)
go


CREATE TABLE bairong_ContentCheck(
    CheckID                int              IDENTITY(1,1),
    TableName              varchar(50)      DEFAULT '' NOT NULL,
    PublishmentSystemID    int              DEFAULT 0 NOT NULL,
    NodeID                 int              DEFAULT 0 NOT NULL,
    ContentID              int              DEFAULT 0 NOT NULL,
    IsAdmin                varchar(18)      DEFAULT '' NOT NULL,
    UserName               nvarchar(255)    DEFAULT '' NOT NULL,
    IsChecked              varchar(18)      DEFAULT '' NOT NULL,
    CheckedLevel           int              DEFAULT 0 NOT NULL,
    CheckDate              datetime         DEFAULT getdate() NOT NULL,
    Reasons                nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_ContentCheck PRIMARY KEY NONCLUSTERED (CheckID)
)
go


CREATE TABLE bairong_Department(
    DepartmentID      int              IDENTITY(1,1),
    DepartmentName    nvarchar(255)    DEFAULT '' NOT NULL,
    Code              varchar(50)      DEFAULT '' NOT NULL,
    ParentID          int              DEFAULT 0 NOT NULL,
    ParentsPath       nvarchar(255)    DEFAULT '' NOT NULL,
    ParentsCount      int              DEFAULT 0 NOT NULL,
    ChildrenCount     int              DEFAULT 0 NOT NULL,
    IsLastNode        varchar(18)      DEFAULT '' NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    AddDate           datetime         DEFAULT getdate() NOT NULL,
    Summary           nvarchar(255)    DEFAULT '' NOT NULL,
    CountOfAdmin      int              DEFAULT 0 NOT NULL,
    CountOfUser       int              DEFAULT 0 NOT NULL,
    CONSTRAINT PK_bairong_Department PRIMARY KEY NONCLUSTERED (DepartmentID)
)
go


CREATE TABLE bairong_SMSConfig(
    ID           int              IDENTITY(1,1),
    UserName     nvarchar(255)    DEFAULT '' NOT NULL,
    PassWord     nvarchar(255)    DEFAULT '' NOT NULL,
    MD5String    nvarchar(255)    DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_SMSConfig PRIMARY KEY NONCLUSTERED (ID)
)
go


CREATE TABLE bairong_SMSMessages(
    ID             int               IDENTITY(1,1),
    MobilesList    ntext             DEFAULT '' NOT NULL,
    SMSContent     nvarchar(1000)    DEFAULT '' NOT NULL,
    SendDate       datetime          DEFAULT getdate() NOT NULL,
    SMSUserName    nvarchar(255)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_SMSMessages PRIMARY KEY NONCLUSTERED (ID)
)
go


ALTER TABLE bairong_TableStyle ADD 
    IsSingleLine       varchar(18)      DEFAULT '' NOT NULL
go


CREATE TABLE bairong_UserContact(
    ID                 int              IDENTITY(1,1),
    RelatedUserName    nvarchar(255)    DEFAULT '' NOT NULL,
    CreateUserName     nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis              int              DEFAULT 0 NOT NULL,
    FullName           nvarchar(50)     DEFAULT '' NOT NULL,
    AvatarUrl          varchar(200)     DEFAULT '' NOT NULL,
    Summary            nvarchar(255)    DEFAULT '' NOT NULL,
    Tel                varchar(50)      DEFAULT '' NOT NULL,
    Mobile             varchar(200)     DEFAULT '' NOT NULL,
    Email              varchar(200)     DEFAULT '' NOT NULL,
    QQ                 varchar(50)      DEFAULT '' NOT NULL,
    Birthday           varchar(50)      DEFAULT '' NOT NULL,
    Organization       nvarchar(50)     DEFAULT '' NOT NULL,
    Department         nvarchar(50)     DEFAULT '' NOT NULL,
    Position           nvarchar(50)     DEFAULT '' NOT NULL,
    Address            nvarchar(255)    DEFAULT '' NOT NULL,
    Website            varchar(200)     DEFAULT '' NOT NULL,
    CONSTRAINT PK_bairong_UserContact PRIMARY KEY NONCLUSTERED (ID)
)
go


CREATE TABLE bairong_UserDownload(
    ID                int              IDENTITY(1,1),
    CreateUserName    nvarchar(255)    DEFAULT '' NOT NULL,
    Taxis             int              DEFAULT 0 NOT NULL,
    Downloads         int              DEFAULT 0 NOT NULL,
    FileName          nvarchar(255)    DEFAULT '' NOT NULL,
    FileUrl           varchar(200)     DEFAULT '' NOT NULL,
    Summary           nvarchar(255)    DEFAULT '' NOT NULL,
    AddDate           datetime         DEFAULT getdate() NOT NULL,
    CONSTRAINT PK_bairong_UserDownload PRIMARY KEY NONCLUSTERED (ID)
)
go


ALTER TABLE bairong_Users ADD
    CreateDate          datetime         DEFAULT getdate() NOT NULL,
    CreateIPAddress     varchar(50)      DEFAULT '' NOT NULL,
    CreateUserName      nvarchar(255)    DEFAULT '' NOT NULL,
    DepartmentID        int              DEFAULT 0 NOT NULL,
    AreaID              int              DEFAULT 0 NOT NULL
go


ALTER TABLE bairong_UserType ADD
    IsDefault        varchar(18)     DEFAULT '' NOT NULL,
    IsPermissions    varchar(18)     DEFAULT '' NOT NULL,
    Permissions      text            DEFAULT '' NOT NULL
go