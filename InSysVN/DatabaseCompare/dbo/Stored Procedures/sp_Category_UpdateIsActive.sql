CREATE PROC dbo.sp_Category_UpdateIsActive
@CateId INT
AS 
BEGIN
	DECLARE @res BIT
	SET @res = (SELECT IsActive FROM dbo.Category WHERE Id=@CateId)
	IF(@res = 1)

		UPDATE dbo.Category
		SET isActive = 0
		WHERE Id = @CateId

	ELSE
		UPDATE dbo.Category
		SET isActive = 1
		WHERE Id = @CateId
END