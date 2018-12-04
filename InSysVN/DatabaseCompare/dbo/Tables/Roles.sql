CREATE TABLE [dbo].[Roles] (
    [Id]           INT           NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [DisplayName]  NVARCHAR (50) NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_Roles_CreatedDate] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME      NULL,
    [Level]        INT           NULL,
    [isShow]       BIT           NULL,
    CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

