CREATE PROC [dbo].[sp_ProductCate_Insert]
@ProductId BIGINT,
@CateId BIGINT

AS
BEGIN
	
	BEGIN
		INSERT INTO dbo.ProductCate
		        (
					ProductId
					,CateId
		        )
		VALUES  ( 
					@ProductId
					,@CateId
		        )
	END
	SELECT TOP 1 * FROM ProductCate WHERE ProductId=@ProductId
END