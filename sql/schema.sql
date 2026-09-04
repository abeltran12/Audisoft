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
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    CREATE TABLE [Estudiantes] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Status] nvarchar(max) NOT NULL DEFAULT N'Activo',
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Estudiantes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    CREATE TABLE [Profesores] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Status] nvarchar(max) NOT NULL DEFAULT N'Activo',
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Profesores] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    CREATE TABLE [Notas] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Valor] int NOT NULL,
        [Fecha] date NOT NULL,
        [Materia] nvarchar(50) NOT NULL,
        [EstudianteId] int NOT NULL,
        [ProfesorId] int NOT NULL,
        [Status] nvarchar(max) NOT NULL DEFAULT N'Activo',
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Notas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notas_Estudiantes_EstudianteId] FOREIGN KEY ([EstudianteId]) REFERENCES [Estudiantes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notas_Profesores_ProfesorId] FOREIGN KEY ([ProfesorId]) REFERENCES [Profesores] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notas_EstudianteId] ON [Notas] ([EstudianteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Notas_ProfesorId] ON [Notas] ([ProfesorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260903215843_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260903215843_InitialCreate', N'10.0.11');
END;

COMMIT;
GO

