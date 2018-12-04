-- Author: <ChienHM>
-- Create date: <20161229>
-- Description: <Remove Users roles>
CREATE PROC [dbo].[sp_User_RemoveFromRoles] 
	@UserId INT
    ,@RoleXml XML
AS
BEGIN		
    --SELECT us.RoleId
	DELETE UserInRole
    FROM [dbo].[UserInRole] us
    INNER JOIN (
        SELECT Id = xmlTable.value('(Id)[1]', 'int')
        FROM @RoleXml.nodes('/Roles/Role') AS XD(xmlTable)
        ) AS del ON us.RoleId = del.Id
    WHERE [UserId] = @UserId
END