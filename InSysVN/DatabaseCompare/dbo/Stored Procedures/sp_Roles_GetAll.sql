-- Author: <ChienHM>
-- Create date: <20161226>
-- Description: <Select all Roles>
CREATE PROC [dbo].[sp_Roles_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id
        ,[Name]
        ,[CreatedDate]
        ,[ModifiedDate]
    FROM [dbo].[Roles]
END