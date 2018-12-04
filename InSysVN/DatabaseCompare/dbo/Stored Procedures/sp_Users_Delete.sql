CREATE PROC [dbo].[sp_Users_Delete] 
@Id INT
AS
BEGIN
	UPDATE dbo.Users
	SET isActive = 0 WHERE Id = @Id
END