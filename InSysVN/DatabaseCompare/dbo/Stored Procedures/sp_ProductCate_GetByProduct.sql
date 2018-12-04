-- =============================================
-- Author:		Dangdai
-- Create date: 16/08/2018
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_ProductCate_GetByProduct]
	@ProductId INT
AS
BEGIN
	SELECT *
	FROM ProductCate
	WHERE ProductId=@ProductId
END