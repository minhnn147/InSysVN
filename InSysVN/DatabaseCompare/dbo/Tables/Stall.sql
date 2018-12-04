CREATE TABLE [dbo].[Stall] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [IpAddress] VARCHAR (25)  NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Stall] PRIMARY KEY CLUSTERED ([Id] ASC, [IpAddress] ASC, [Name] ASC)
);

