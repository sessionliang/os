ALTER TABLE {0} ADD 
[HitsByDay] [int] NOT NULL DEFAULT (0) ,
[HitsByWeek] [int] NOT NULL DEFAULT (0) ,
[HitsByMonth] [int] NOT NULL DEFAULT (0) ,
[LastHitsDate] [datetime] NOT NULL DEFAULT (getdate())

GO