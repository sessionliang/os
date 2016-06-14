ALTER TABLE {0} ADD [ReferenceID] [int] NOT NULL DEFAULT (0)

GO

UPDATE {0} SET [ReferenceID] = 0 WHERE [ReferenceID] IS NULL