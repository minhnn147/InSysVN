CREATE PROC [dbo].[sp_Category_GetAllData]
as
SELECT *
FROM dbo.Category
--WHERE isActive = 1
ORDER BY Name ASC