-- Script SQL para crear la base de datos Lab10
-- Ejecuta este script en tu servidor SQL Server

USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Lab10DB')
BEGIN
    CREATE DATABASE Lab10DB;
END
GO

USE Lab10DB;
GO

-- Tabla Users
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'User',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );

    CREATE INDEX IX_Users_Username ON Users(Username);
    CREATE INDEX IX_Users_Email ON Users(Email);
END
GO

-- Tabla Products
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );
END
GO

-- Insertar datos de prueba
-- Usuario de prueba (password: test123)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt, IsActive)
    VALUES ('admin', 'admin@lab10.com', 'dGVzdDEyM1NBTFQ=', 'Admin', GETUTCDATE(), 1);
END
GO

IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'testuser')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt, IsActive)
    VALUES ('testuser', 'test@lab10.com', 'dGVzdDEyM1NBTFQ=', 'User', GETUTCDATE(), 1);
END
GO

-- Productos de prueba
IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Laptop HP')
BEGIN
    INSERT INTO Products (Name, Description, Price, Stock, CreatedAt, IsActive)
    VALUES
    ('Laptop HP', 'Laptop HP Core i7 16GB RAM 512GB SSD', 1200.00, 15, GETUTCDATE(), 1),
    ('Mouse Logitech', 'Mouse inalámbrico Logitech MX Master 3', 99.99, 50, GETUTCDATE(), 1),
    ('Teclado Mecánico', 'Teclado mecánico RGB switches blue', 149.99, 30, GETUTCDATE(), 1),
    ('Monitor Dell', 'Monitor Dell 27 pulgadas 4K', 450.00, 20, GETUTCDATE(), 1),
    ('Webcam Logitech', 'Webcam Logitech C920 HD', 79.99, 40, GETUTCDATE(), 1);
END
GO

PRINT 'Base de datos Lab10DB creada exitosamente con datos de prueba';
PRINT 'Usuario de prueba: admin / password: test123';
PRINT 'Usuario de prueba: testuser / password: test123';
GO
