CREATE PROC [dbo].[sp_RoleModule_ByRoleId]
@RoleId BIGINT
AS
SELECT	[ModuleName] = m.Name
		,[ModuleDisplayName] = m.DisplayName
		,rm.*
FROM dbo.RoleModule rm
JOIN dbo.Module m ON rm.ModuleId = m.Id
WHERE RoleId = @RoleId
ORDER BY m.Sorting ASC