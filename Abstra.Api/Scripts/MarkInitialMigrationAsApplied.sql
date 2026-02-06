-- Script para marcar a migration inicial como aplicada
-- Execute este script no banco de dados AbstraDb antes de aplicar as novas migrations

-- Verificar se a tabela __EFMigrationsHistory existe, se não, criar
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

-- Marcar a migration inicial como aplicada (se ainda não estiver marcada)
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260205183952_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260205183952_InitialCreate', '8.0.0');
END
