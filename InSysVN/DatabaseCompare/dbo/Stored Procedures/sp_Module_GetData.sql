CREATE PROC [dbo].[sp_Module_GetData]
AS
SELECT * FROM dbo.Module WHERE isShow = 1 ORDER BY Sorting ASC