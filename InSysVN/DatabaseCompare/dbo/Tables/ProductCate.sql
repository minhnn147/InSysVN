CREATE TABLE [dbo].[ProductCate] (
    [ProductId] BIGINT NOT NULL,
    [CateId]    INT    NOT NULL,
    CONSTRAINT [PK_ProductCate] PRIMARY KEY CLUSTERED ([ProductId] ASC, [CateId] ASC)
);

