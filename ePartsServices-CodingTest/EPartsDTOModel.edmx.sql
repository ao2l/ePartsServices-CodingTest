
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/18/2020 11:19:54
-- Generated from EDMX file: C:\Users\Ashley\source\repos\ePartsServices-CodingTest\ePartsServices-CodingTest\EPartsDTOModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ePartsCodeTest];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ProductTables'
CREATE TABLE [dbo].[ProductTables] (
    [Product_ID] int IDENTITY(1,1) NOT NULL,
    [Product_Active] bit  NOT NULL,
    [Product_Number_Custom] nvarchar(max)  NOT NULL,
    [Cost_Price] decimal(18,0)  NOT NULL,
    [Product_Description] nvarchar(max)  NOT NULL,
    [Manufacturer_ID_Manufacturer_ID] int  NOT NULL
);
GO

-- Creating table 'ProductOptionsTables'
CREATE TABLE [dbo].[ProductOptionsTables] (
    [Option_Number] int  NOT NULL,
    [Option_Text] nvarchar(max)  NOT NULL,
    [ProductOption_ID] int  NOT NULL,
    [Product_ID_Product_ID] int  NOT NULL
);
GO

-- Creating table 'CustomerTables'
CREATE TABLE [dbo].[CustomerTables] (
    [Cust_ID] int IDENTITY(1,1) NOT NULL,
    [Cust_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ManufacturerTables'
CREATE TABLE [dbo].[ManufacturerTables] (
    [Manufacturer_ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ManufacturerCustomerTables'
CREATE TABLE [dbo].[ManufacturerCustomerTables] (
    [ManufacturerCustomer_ID] int  NOT NULL,
    [Cust_ID_Cust_ID] int  NOT NULL,
    [Manufacturer_ID_Manufacturer_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Product_ID] in table 'ProductTables'
ALTER TABLE [dbo].[ProductTables]
ADD CONSTRAINT [PK_ProductTables]
    PRIMARY KEY CLUSTERED ([Product_ID] ASC);
GO

-- Creating primary key on [ProductOption_ID] in table 'ProductOptionsTables'
ALTER TABLE [dbo].[ProductOptionsTables]
ADD CONSTRAINT [PK_ProductOptionsTables]
    PRIMARY KEY CLUSTERED ([ProductOption_ID] ASC);
GO

-- Creating primary key on [Cust_ID] in table 'CustomerTables'
ALTER TABLE [dbo].[CustomerTables]
ADD CONSTRAINT [PK_CustomerTables]
    PRIMARY KEY CLUSTERED ([Cust_ID] ASC);
GO

-- Creating primary key on [Manufacturer_ID] in table 'ManufacturerTables'
ALTER TABLE [dbo].[ManufacturerTables]
ADD CONSTRAINT [PK_ManufacturerTables]
    PRIMARY KEY CLUSTERED ([Manufacturer_ID] ASC);
GO

-- Creating primary key on [ManufacturerCustomer_ID] in table 'ManufacturerCustomerTables'
ALTER TABLE [dbo].[ManufacturerCustomerTables]
ADD CONSTRAINT [PK_ManufacturerCustomerTables]
    PRIMARY KEY CLUSTERED ([ManufacturerCustomer_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Cust_ID_Cust_ID] in table 'ManufacturerCustomerTables'
ALTER TABLE [dbo].[ManufacturerCustomerTables]
ADD CONSTRAINT [FK_CustomerTableManufacturerCustomerTable]
    FOREIGN KEY ([Cust_ID_Cust_ID])
    REFERENCES [dbo].[CustomerTables]
        ([Cust_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerTableManufacturerCustomerTable'
CREATE INDEX [IX_FK_CustomerTableManufacturerCustomerTable]
ON [dbo].[ManufacturerCustomerTables]
    ([Cust_ID_Cust_ID]);
GO

-- Creating foreign key on [Manufacturer_ID_Manufacturer_ID] in table 'ManufacturerCustomerTables'
ALTER TABLE [dbo].[ManufacturerCustomerTables]
ADD CONSTRAINT [FK_ManufacturerCustomerTableManufacturerTable]
    FOREIGN KEY ([Manufacturer_ID_Manufacturer_ID])
    REFERENCES [dbo].[ManufacturerTables]
        ([Manufacturer_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ManufacturerCustomerTableManufacturerTable'
CREATE INDEX [IX_FK_ManufacturerCustomerTableManufacturerTable]
ON [dbo].[ManufacturerCustomerTables]
    ([Manufacturer_ID_Manufacturer_ID]);
GO

-- Creating foreign key on [Manufacturer_ID_Manufacturer_ID] in table 'ProductTables'
ALTER TABLE [dbo].[ProductTables]
ADD CONSTRAINT [FK_ProductTableManufacturerTable]
    FOREIGN KEY ([Manufacturer_ID_Manufacturer_ID])
    REFERENCES [dbo].[ManufacturerTables]
        ([Manufacturer_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTableManufacturerTable'
CREATE INDEX [IX_FK_ProductTableManufacturerTable]
ON [dbo].[ProductTables]
    ([Manufacturer_ID_Manufacturer_ID]);
GO

-- Creating foreign key on [Product_ID_Product_ID] in table 'ProductOptionsTables'
ALTER TABLE [dbo].[ProductOptionsTables]
ADD CONSTRAINT [FK_ProductTableProductOptionsTable]
    FOREIGN KEY ([Product_ID_Product_ID])
    REFERENCES [dbo].[ProductTables]
        ([Product_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTableProductOptionsTable'
CREATE INDEX [IX_FK_ProductTableProductOptionsTable]
ON [dbo].[ProductOptionsTables]
    ([Product_ID_Product_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

-- ---------------------------------------------------
-- Inserts
-- ---------------------------------------------------

--Product
USE [ePartsCodeTest]
GO

INSERT INTO [dbo].[ProductTables]
           ([Product_Active]
           ,[Product_Number_Custom]
           ,[Cost_Price]
           ,[Product_Description]
           ,[Manufacturer_ID_Manufacturer_ID])
     VALUES
           (--this would be ID 4 in my test. 
		   1
           ,'a'
           ,1
           ,'test'
           ,1),
		              (--this would be ID 5 in my test. 
		   1
           ,'b'
           ,1
           ,'test2'
           ,1),
		              (--this would be ID 6 in my test. 
		   0
           ,'c'
           ,2
           ,'test'
           ,1),
		              (--this would be ID 7 in my test. 
		   1
           ,'d'
           ,2
           ,'test'
           ,1)
GO

--Manufacturer
USE [ePartsCodeTest]
GO

INSERT INTO [dbo].[ManufacturerTables]
           ([Name])
     VALUES
	 --Id would be 1
           ('test')
GO



--Manu Customer
USE [ePartsCodeTest]
GO

INSERT INTO [dbo].[ManufacturerCustomerTables]
           ([ManufacturerCustomer_ID]
           ,[Cust_ID_Cust_ID]
           ,[Manufacturer_ID_Manufacturer_ID])
     VALUES
           (1
           ,1
           ,1)
GO


--Product Options
USE [ePartsCodeTest]
GO

INSERT INTO [dbo].[ProductOptionsTables]
           ([Option_Number]
           ,[Option_Text]
           ,[ProductOption_ID]
           ,[Product_ID_Product_ID])
     VALUES
           (1
           ,'test'
           ,1
           ,4),
		   (2
           ,'test'
           ,2
           ,4),
		   (1
           ,'test'
           ,3
           ,6),
		  (2
           ,'test'
           ,4
           ,6),
		   (1
           ,'test'
           ,5
           ,7),
		   (2
           ,'test'
           ,6
           ,7),
GO


--Customer
USE [ePartsCodeTest]
GO

INSERT INTO [dbo].[CustomerTables]
           ([Cust_Name])
     VALUES --Id is 1
           ('test')
GO



