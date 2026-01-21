USE GregCustomersDb;
GO

-- CLIENTS
CREATE OR ALTER PROCEDURE dbo.sp_client_create
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @Email NVARCHAR(320)
    AS
BEGIN
    SET NOCOUNT ON;

INSERT INTO dbo.Clients (Id, Name, Email, CreatedAt, UpdatedAt)
VALUES (@Id, @Name, @Email, SYSUTCDATETIME(), SYSUTCDATETIME());
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_client_update
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @Email NVARCHAR(320)
    AS
BEGIN
    SET NOCOUNT ON;

UPDATE dbo.Clients
SET Name = @Name,
    Email = @Email,
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_client_delete
    @Id UNIQUEIDENTIFIER
    AS
BEGIN
    SET NOCOUNT ON;

DELETE FROM dbo.Clients WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_client_update_logo
    @ClientId UNIQUEIDENTIFIER,
    @Logo VARBINARY(MAX),
    @LogoContentType NVARCHAR(50),
    @LogoFileName NVARCHAR(255)
    AS
BEGIN
    SET NOCOUNT ON;

UPDATE dbo.Clients
SET Logo = @Logo,
    LogoContentType = @LogoContentType,
    LogoFileName = @LogoFileName,
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = @ClientId;
END
GO


-- ADDRESSES
CREATE OR ALTER PROCEDURE dbo.sp_address_create
    @Id UNIQUEIDENTIFIER,
    @ClientId UNIQUEIDENTIFIER,
    @Street NVARCHAR(300)
    AS
BEGIN
    SET NOCOUNT ON;

INSERT INTO dbo.Addresses (Id, ClientId, Street, CreatedAt, UpdatedAt)
VALUES (@Id, @ClientId, @Street, SYSUTCDATETIME(), SYSUTCDATETIME());
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_address_update
    @Id UNIQUEIDENTIFIER,
    @Street NVARCHAR(300)
    AS
BEGIN
    SET NOCOUNT ON;

UPDATE dbo.Addresses
SET Street = @Street,
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_address_delete
    @Id UNIQUEIDENTIFIER
    AS
BEGIN
    SET NOCOUNT ON;

DELETE FROM dbo.Addresses WHERE Id = @Id;
END
GO