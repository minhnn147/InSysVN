CREATE PROC [dbo].[sp_Modules_GetByRoleId]
@RoleId INT
as
SELECT m.*
FROM dbo.RoleModule rm
JOIN dbo.Module m ON rm.ModuleId = m.Id
WHERE RoleId = @RoleId