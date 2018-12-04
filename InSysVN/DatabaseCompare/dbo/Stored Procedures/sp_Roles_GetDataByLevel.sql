CREATE PROC [dbo].[sp_Roles_GetDataByLevel]
@Level INT
as
SELECT * 
FROM dbo.Roles 
WHERE isShow = 1 AND [Level] > @Level
ORDER BY Level ASC