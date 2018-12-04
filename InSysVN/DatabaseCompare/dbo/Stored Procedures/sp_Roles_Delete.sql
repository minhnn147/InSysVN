-- Author: <ChienHM>
-- Create date: <20161226>
-- Description: <Delete Roles item>
CREATE PROC [dbo].[sp_Roles_Delete] @Id INT
AS
BEGIN
    DELETE
    FROM [dbo].[Roles]
    WHERE [Id] = @Id
END