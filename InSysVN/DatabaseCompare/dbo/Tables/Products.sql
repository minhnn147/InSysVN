CREATE TABLE [dbo].[Products] (
    [Id]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProductCategory] NVARCHAR (255)  NULL,
    [ProductName]     NVARCHAR (255)  NOT NULL,
    [Barcode]         NVARCHAR (50)   NOT NULL,
    [MainImage]       NVARCHAR (MAX)  NULL,
    [Image]           NVARCHAR (500)  NULL,
    [Description]     NVARCHAR (1000) NULL,
    [Status]          BIT             NOT NULL,
    [CreatedDate]     DATETIME        NULL,
    [ModifiedDate]    DATETIME        NULL,
    [Quantity]        INT             NOT NULL,
    [ComputeUnit]     NVARCHAR (50)   NULL,
    [Price]           DECIMAL (18, 2) NULL
);

