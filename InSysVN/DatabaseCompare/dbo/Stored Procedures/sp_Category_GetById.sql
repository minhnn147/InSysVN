CREATE PROC [dbo].[sp_Category_GetById]
@Id INT
as
SELECT *
FROM dbo.Category WHERE Id = @id