IF DB_ID('GregCustomersDb') IS NULL
BEGIN
    CREATE DATABASE GregCustomersDb;
END
GO

USE GregCustomersDb;
GO

IF OBJECT_ID('dbo.Addresses', 'U') IS NOT NULL DROP TABLE dbo.Addresses;
IF OBJECT_ID('dbo.Clients', 'U') IS NOT NULL DROP TABLE dbo.Clients;
GO

CREATE TABLE dbo.Clients
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Clients PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Email NVARCHAR(320) NOT NULL,
    Logo VARBINARY(MAX) NULL,
    LogoContentType NVARCHAR(50) NULL,
    LogoFileName NVARCHAR(255) NULL,
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Clients_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Clients_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

-- UNIQUE no email (se quiser garantir case-insensitive, use collation CI)
CREATE UNIQUE INDEX UX_Clients_Email ON dbo.Clients (Email);
GO

CREATE TABLE dbo.Addresses
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Addresses PRIMARY KEY,
    ClientId UNIQUEIDENTIFIER NOT NULL,
    Street NVARCHAR(300) NOT NULL,
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Addresses_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Addresses_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Addresses_Clients FOREIGN KEY (ClientId) REFERENCES dbo.Clients (Id) ON DELETE CASCADE
);
GO

CREATE INDEX IX_Addresses_ClientId ON dbo.Addresses (ClientId);
GO