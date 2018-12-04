CREATE PROC [dbo].[sp_Roles_Insert] 
    @Id INT OUT
    ,@Name NVARCHAR(50)
    --,@CreatedDate DATETIME
    --,@ModifiedDate DATETIME
AS
BEGIN
    IF EXISTS (
            SELECT TOP 1 1
            FROM [dbo].[Roles]
            WHERE [Name] = @Name
            )
    BEGIN
        SELECT @Id = - 1
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[Roles] (
            [Name]
            ,[CreatedDate]
            ,[ModifiedDate]
            )
        VALUES (
            @Name
            ,GetDate()
            ,GetDate()
            )

        SELECT @Id = @@IDENTITY
    END
END