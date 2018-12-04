CREATE PROC sp_Category_Delete
@Id INT
AS
BEGIN
	UPDATE dbo.Category
	SET isActive = 0
	WHERE Id = @Id
END