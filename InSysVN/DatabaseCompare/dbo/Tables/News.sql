CREATE TABLE [dbo].[News] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (MAX) NULL,
    [Content]    NVARCHAR (MAX) NULL,
    [CreateDate] DATETIME       NULL,
    [IsActive]   BIT            NULL,
    [ImageTitle] NVARCHAR (MAX) NULL,
    [CreateBy]   INT            NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([ID] ASC)
);

