IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [OvenLogs] (
    [Id] int NOT NULL IDENTITY,
    [OvenNumber] int NOT NULL,
    [CycleStep] nvarchar(max) NOT NULL,
    [Temperature] float NOT NULL,
    [Timestamp] datetime2 NOT NULL DEFAULT (GETDATE()),
    [LogType] int NOT NULL,
    CONSTRAINT [PK_OvenLogs] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250915144709_InitialCreate', N'9.0.9');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250915185411_secondCreate', N'9.0.9');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OvenLogs]') AND [c].[name] = N'CycleStep');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [OvenLogs] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [OvenLogs] ALTER COLUMN [CycleStep] nvarchar(25) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250916125718_CycleStepColumnLength', N'9.0.9');

COMMIT;
GO

