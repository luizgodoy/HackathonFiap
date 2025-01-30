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
GO

IF SCHEMA_ID(N'Hackathon') IS NULL EXEC(N'CREATE SCHEMA [Hackathon];');
GO

CREATE TABLE [Hackathon].[Appointment] (
    [Id] uniqueidentifier NOT NULL,
    [Title] VARCHAR(100) NOT NULL,
    [Description] VARCHAR(100) NOT NULL,
    [StartAt] DATE NOT NULL,
    [FinishAt] DATE NOT NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NULL,
    CONSTRAINT [PK_Appointment] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Hackathon].[User] (
    [Id] uniqueidentifier NOT NULL,
    [Name] VARCHAR(100) NOT NULL,
    [CPF] VARCHAR(11) NOT NULL,
    [CRM] VARCHAR(10) NULL,
    [Email] VARCHAR(100) NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250130121833_Initial', N'8.0.4');
GO

COMMIT;
GO


