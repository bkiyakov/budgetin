IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Logos] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(32) NULL,
    [IconUrl] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_Logos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(32) NOT NULL,
    [LogoId] int NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_Logos_LogoId] FOREIGN KEY ([LogoId]) REFERENCES [Logos] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Expenses] (
    [Id] int NOT NULL IDENTITY,
    [Sum] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [CategoryId] int NOT NULL,
    [Note] nvarchar(128) NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Expenses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Categories_LogoId] ON [Categories] ([LogoId]);

GO

CREATE INDEX [IX_Expenses_CategoryId] ON [Expenses] ([CategoryId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200816181343_Initial', N'3.1.6');

GO

-- Вручную добавил добавление системных данных для иконок и категорий

INSERT INTO [Logos] ([Name], [IconUrl])
VALUES (N'Grossary', N'https://cdn3.iconfinder.com/data/icons/shopping-icons-14/128/03_Trolley-512.png');

GO

INSERT INTO [Categories] ([Name], [LogoId], [UserId])
VALUES (N'Grossary', (SELECT Id FROM Logos WHERE Name = 'Grossary'), N'system_user' );

GO

