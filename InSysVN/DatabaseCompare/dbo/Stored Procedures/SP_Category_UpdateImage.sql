CREATE PROC [dbo].[SP_Category_UpdateImage]
@id BIGINT,
@pathimage NVARCHAR(max)
AS
BEGIN
	UPDATE dbo.Category
	SET ImagePath = @pathimage 
	WHERE Id = @id
END