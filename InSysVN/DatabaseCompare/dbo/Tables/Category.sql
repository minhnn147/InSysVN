CREATE TABLE [dbo].[Category] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (250) NOT NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF_Category_CreatedDate] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME       NULL,
    [isActive]     BIT            CONSTRAINT [DF_Category_isActive] DEFAULT ((1)) NULL,
    [ImagePath]    NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

