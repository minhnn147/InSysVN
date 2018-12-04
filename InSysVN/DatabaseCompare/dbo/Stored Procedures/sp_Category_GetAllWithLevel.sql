CREATE PROC [dbo].[sp_Category_GetAllWithLevel]
@txtSearch NVARCHAR(MAX)
AS
BEGIN
	SELECT * FROM dbo.Category WHERE Name LIKE '%'+@txtSearch+'%'
END