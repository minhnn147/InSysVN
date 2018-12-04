CREATE TABLE [dbo].[Users] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [UserCode]      NVARCHAR (50)  NULL,
    [FullName]      NVARCHAR (255) NOT NULL,
    [UserName]      NVARCHAR (50)  NOT NULL,
    [Password]      NVARCHAR (255) NOT NULL,
    [Gender]        BIT            NULL,
    [Address]       NVARCHAR (500) NULL,
    [Phone]         NVARCHAR (50)  NULL,
    [Birthday]      DATETIME       NULL,
    [CreatedDate]   DATETIME       CONSTRAINT [DF_Users_CreatedDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]  DATETIME       NULL,
    [Email]         NVARCHAR (255) NULL,
    [AvatarImg]     NVARCHAR (500) NULL,
    [ResetPassCode] NVARCHAR (500) NULL,
    [RoleId]        INT            NULL,
    [isActive]      BIT            CONSTRAINT [DF_Users_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

