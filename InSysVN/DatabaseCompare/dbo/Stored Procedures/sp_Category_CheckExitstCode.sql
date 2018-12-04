CREATE PROC [dbo].[sp_Category_CheckExitstCode]
@Code NVARCHAR(MAX),
@Id INT
AS
BEGIN
	IF @Id IS NULL
	BEGIN
		SELECT COUNT(*) FROM dbo.Category WHERE Code = @Code AND isActive = 1	
	END
	ELSE
	BEGIN
		SELECT COUNT(*) FROM dbo.Category WHERE Code = @Code AND Id != @Id AND isActive = 1
	END
END