/* =====================================================================
   OCN_WebApp_DB · Creación de la base de datos (Actividad 8)
   Generado con: dotnet ef migrations script --idempotent
   Es el equivalente en SQL de la migración InitialCreate.
   "Idempotente" = se puede ejecutar varias veces sin duplicar nada.
   ===================================================================== */
IF DB_ID(N'OCN_WebApp_DB') IS NULL
    CREATE DATABASE OCN_WebApp_DB;
GO
USE OCN_WebApp_DB;
GO
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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    CREATE TABLE [Problematicas] (
        [ProblematicaId] int NOT NULL IDENTITY,
        [NombreProblematica] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Problematicas] PRIMARY KEY ([ProblematicaId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    CREATE TABLE [Zonas] (
        [ZonaId] int NOT NULL IDENTITY,
        [NombreZona] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Zonas] PRIMARY KEY ([ZonaId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    CREATE TABLE [RegistrosOCN] (
        [RegistroOCNId] int NOT NULL IDENTITY,
        [ZonaId] int NOT NULL,
        [ProblematicaId] int NOT NULL,
        [Intensidad] int NOT NULL,
        [Frecuencia] int NOT NULL,
        CONSTRAINT [PK_RegistrosOCN] PRIMARY KEY ([RegistroOCNId]),
        CONSTRAINT [FK_RegistrosOCN_Problematicas_ProblematicaId] FOREIGN KEY ([ProblematicaId]) REFERENCES [Problematicas] ([ProblematicaId]) ON DELETE CASCADE,
        CONSTRAINT [FK_RegistrosOCN_Zonas_ZonaId] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([ZonaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RegistrosOCN_ProblematicaId] ON [RegistrosOCN] ([ProblematicaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RegistrosOCN_ZonaId] ON [RegistrosOCN] ([ZonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261117170356_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261117170356_InitialCreate', N'11.0.0');
END;

COMMIT;
GO


