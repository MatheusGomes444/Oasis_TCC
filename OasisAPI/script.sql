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

CREATE TABLE [Alojamentos] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NULL,
    [Equipe] nvarchar(100) NOT NULL,
    [Telefone] int NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [CapacidadeMaxima] int NOT NULL,
    [Pet] nvarchar(max) NULL,
    [Sexo] nvarchar(max) NULL,
    [Pertences] nvarchar(max) NULL,
    [Refeicoes] nvarchar(max) NULL,
    CONSTRAINT [PK_Alojamentos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Moradores] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NULL,
    [CPF] nvarchar(11) NULL,
    [RG] nvarchar(9) NULL,
    [Telefone] int NOT NULL,
    [Endereco] nvarchar(200) NULL,
    [Idade] int NOT NULL,
    [Datanascimento] int NOT NULL,
    [Nacionalidade] nvarchar(max) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [Ativo] bit NOT NULL,
    [AlojamentoId] int NOT NULL,
    CONSTRAINT [PK_Moradores] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Moradores_Alojamentos_AlojamentoId] FOREIGN KEY ([AlojamentoId]) REFERENCES [Alojamentos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [FilasDeEspera] (
    [Id] int NOT NULL IDENTITY,
    [MoradorId] int NOT NULL,
    [AlojamentoId] int NOT NULL,
    [DataEntrada] datetime2 NOT NULL,
    CONSTRAINT [PK_FilasDeEspera] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FilasDeEspera_Alojamentos_AlojamentoId] FOREIGN KEY ([AlojamentoId]) REFERENCES [Alojamentos] ([Id]),
    CONSTRAINT [FK_FilasDeEspera_Moradores_MoradorId] FOREIGN KEY ([MoradorId]) REFERENCES [Moradores] ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CapacidadeMaxima', N'Email', N'Equipe', N'Nome', N'Pertences', N'Pet', N'Refeicoes', N'Sexo', N'Telefone') AND [object_id] = OBJECT_ID(N'[Alojamentos]'))
    SET IDENTITY_INSERT [Alojamentos] ON;
INSERT INTO [Alojamentos] ([Id], [CapacidadeMaxima], [Email], [Equipe], [Nome], [Pertences], [Pet], [Refeicoes], [Sexo], [Telefone])
VALUES (1, 10, N'exemplo1@dominio.com', N'Equipe A', N'Albergue Vila Maria', N'Roupas e sapatos', N'Apenas cães', N'Café e janta', N'Masculino', 123456789);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CapacidadeMaxima', N'Email', N'Equipe', N'Nome', N'Pertences', N'Pet', N'Refeicoes', N'Sexo', N'Telefone') AND [object_id] = OBJECT_ID(N'[Alojamentos]'))
    SET IDENTITY_INSERT [Alojamentos] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AlojamentoId', N'Ativo', N'CPF', N'Datanascimento', N'Endereco', N'Idade', N'Nacionalidade', N'Nome', N'Observacoes', N'RG', N'Telefone') AND [object_id] = OBJECT_ID(N'[Moradores]'))
    SET IDENTITY_INSERT [Moradores] ON;
INSERT INTO [Moradores] ([Id], [AlojamentoId], [Ativo], [CPF], [Datanascimento], [Endereco], [Idade], [Nacionalidade], [Nome], [Observacoes], [RG], [Telefone])
VALUES (1, 1, CAST(1 AS bit), N'12345678900', 0, N'Rua Alcântara, 113', 18, N'Brasileiro', N'Pedro', N'Sem observações', N'603456789', 912345678);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AlojamentoId', N'Ativo', N'CPF', N'Datanascimento', N'Endereco', N'Idade', N'Nacionalidade', N'Nome', N'Observacoes', N'RG', N'Telefone') AND [object_id] = OBJECT_ID(N'[Moradores]'))
    SET IDENTITY_INSERT [Moradores] OFF;
GO

CREATE INDEX [IX_FilasDeEspera_AlojamentoId] ON [FilasDeEspera] ([AlojamentoId]);
GO

CREATE INDEX [IX_FilasDeEspera_MoradorId] ON [FilasDeEspera] ([MoradorId]);
GO

CREATE INDEX [IX_Moradores_AlojamentoId] ON [Moradores] ([AlojamentoId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241205012210_Create', N'8.0.8');
GO

COMMIT;
GO

