-- =============================================
-- Author:		Dangdai
-- Create date: 16/08/2018
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Products_GetById]
	@Id INT
AS
BEGIN
	SELECT *
	FROM Products
	WHERE Id=@Id
END