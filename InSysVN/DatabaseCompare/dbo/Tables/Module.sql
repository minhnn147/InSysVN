CREATE TABLE [dbo].[Module] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [DisplayName]  NVARCHAR (255) NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF_Module_CreatedDate] DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME       NULL,
    [Parent]       INT            NULL,
    [Sorting]      INT            NULL,
    [isShow]       BIT            CONSTRAINT [DF_Module_isShow] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED ([Id] ASC)
);

