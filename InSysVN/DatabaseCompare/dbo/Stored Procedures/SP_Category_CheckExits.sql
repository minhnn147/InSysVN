CREATE PROC [dbo].[SP_Category_CheckExits]
@check NVARCHAR(max)
AS
BEGIN
	SELECT * FROM dbo.Category WHERE Name = @check
END