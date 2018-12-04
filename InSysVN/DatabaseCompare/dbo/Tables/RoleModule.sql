CREATE TABLE [dbo].[RoleModule] (
    [RoleId]   INT NOT NULL,
    [ModuleId] INT NOT NULL,
    [Add]      BIT NULL,
    [Edit]     BIT NULL,
    [View]     BIT NULL,
    [Delete]   BIT NULL,
    [Import]   BIT NULL,
    [Export]   BIT NULL,
    [Upload]   BIT NULL,
    [Publish]  BIT NULL,
    [Report]   BIT NULL,
    [Sync]     BIT NULL,
    [Accept]   BIT NULL,
    [Cancel]   BIT NULL,
    [Record]   BIT NULL,
    CONSTRAINT [PK_RoleModule] PRIMARY KEY CLUSTERED ([RoleId] ASC, [ModuleId] ASC)
);

