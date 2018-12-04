CREATE PROC [dbo].[sp_Roles_Update] 
    @Id INT
    ,@Name NVARCHAR(50)
    --,@ModifiedDate DATETIME
AS
BEGIN
    IF EXISTS (
            SELECT TOP 1 1
            FROM [dbo].[Roles]
            WHERE [Name] = @Name
                AND Id <> @Id
            )
    BEGIN
        SELECT @Id = - 1
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Roles]
        SET [Name] = @Name
            ,[ModifiedDate] = GetDate()
        WHERE [Id] = @Id
    END
END