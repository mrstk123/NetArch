-- Sample table backing the Dapper command/query sample (Infrastructure/Queries, Infrastructure/Commands).
IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        IsActive BIT NOT NULL DEFAULT(1),
        CreatedAt DATETIME2 NOT NULL DEFAULT(SYSUTCDATETIME()),
        UpdatedAt DATETIME2 NOT NULL DEFAULT(SYSUTCDATETIME())
    );
END
GO
