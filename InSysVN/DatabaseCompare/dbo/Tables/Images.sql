CREATE TABLE [dbo].[Images] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ImagePath] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC)
);

