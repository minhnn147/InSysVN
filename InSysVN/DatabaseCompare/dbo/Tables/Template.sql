CREATE TABLE [dbo].[Template] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Url]  NVARCHAR (500) NOT NULL,
    [Type] INT            CONSTRAINT [DF_Template_Type] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1: excel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Template', @level2type = N'COLUMN', @level2name = N'Type';

