CREATE PROC [dbo].[sp_Roles_GetById] @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id
        ,[Name]
        ,[CreatedDate]
        ,[ModifiedDate]
    FROM [dbo].[Roles]
    WHERE Id = @Id
END