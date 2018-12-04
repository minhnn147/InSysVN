-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spOrderDetail_Save]
	-- Add the parameters for the stored procedure here
	@xml				XML,
	@OrderId			BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

    -- Insert statements for procedure here
	DELETE OD
	FROM [dbo].[OrderDetail] OD	(NOLOCK)
	WHERE 
		NOT EXISTS(
			SELECT
				1
			FROM @xml.nodes('/OrderSubmitEntity/OrderDetailEntities/OrderDetailEntity') AS XD(xmlTable)
			WHERE 
				xmlTable.value('(OrderId)[1]', 'BIGINT')	=	OD.OrderId
				AND	xmlTable.value('(ProductId)[1]', 'BIGINT')	=	OD.ProductId
		)	
		AND	OD.OrderId			=	@OrderId

	UPDATE OD
		SET
			Quantity			=	X.[Quantity]
			,[SellPrice]		=	X.SellPrice
			,[Discount]			=	X.[Discount]
	FROM [dbo].[OrderDetail] OD	(NOLOCK)

	INNER JOIN	(
		SELECT
		   [OrderId]			=	xmlTable.value('(OrderId)[1]', 'BIGINT')
		  ,[ProductId]			=	xmlTable.value('(ProductId)[1]', 'BIGINT')
		  ,[Name]				=	xmlTable.value('(Name)[1]', 'NVARCHAR(255)')
		  ,[Barcode]			=	xmlTable.value('(Barcode)[1]', 'VARCHAR(20)')
		  ,[Quantity]			=	xmlTable.value('(Quantity)[1]', 'INT')
		  ,[ParentId]			=	xmlTable.value('(ParentId)[1]', 'BIGINT')
		  ,[Price]				=	xmlTable.value('(Price)[1]', 'DECIMAL(18,2)')
		  ,[SellPrice]			=	xmlTable.value('(SellPrice)[1]', 'DECIMAL(18,2)')
		  ,[Discount]			=	xmlTable.value('(Discount)[1]', 'DECIMAL(18,2)')
		  ,[Coupon]				=	xmlTable.value('(Coupon)[1]', 'DECIMAL(18,2)')
		FROM @xml.nodes('/OrderSubmitEntity/OrderDetailEntities/OrderDetailEntity') AS XD(xmlTable)
	)	X
		ON	X.OrderId			=	OD.OrderId
		AND	X.ProductId			=	OD.ProductId

	INSERT INTO [dbo].[OrderDetail] (
	   [OrderId]
      ,[ProductId]
      ,ProductName
      ,[Barcode]
      ,[Quantity]
      ,[Price]
      ,[SellPrice]
      ,[Discount]
	)
	SELECT
	   [OrderId]			=	@OrderId
      ,[ProductId]			=	X.[ProductId]
      ,[ProductName]		=	X.[Name]
      ,[Barcode]			=	X.[Barcode]
      ,[Quantity]			=	X.[Quantity]
      ,[Price]				=	X.[Price]
      ,[SellPrice]			=	X.[SellPrice]
      ,[Discount]			=	X.[Discount]
	FROM (
		SELECT
		   [OrderId]			=	xmlTable.value('(OrderId)[1]', 'BIGINT')
		  ,[ProductId]			=	xmlTable.value('(ProductId)[1]', 'BIGINT')
		  ,[Name]				=	xmlTable.value('(Name)[1]', 'NVARCHAR(255)')
		  ,[Barcode]			=	xmlTable.value('(Barcode)[1]', 'VARCHAR(20)')
		  ,[Quantity]			=	xmlTable.value('(Quantity)[1]', 'INT')
		  ,[ParentId]			=	xmlTable.value('(ParentId)[1]', 'BIGINT')
		  ,[Price]				=	xmlTable.value('(Price)[1]', 'DECIMAL(18,2)')
		  ,[SellPrice]			=	xmlTable.value('(SellPrice)[1]', 'DECIMAL(18,2)')
		  ,[Discount]			=	xmlTable.value('(Discount)[1]', 'DECIMAL(18,2)')
		  ,[Coupon]				=	xmlTable.value('(Coupon)[1]', 'DECIMAL(18,2)')
		FROM @xml.nodes('/OrderSubmitEntity/OrderDetailEntities/OrderDetailEntity') AS XD(xmlTable)
	)	X

	WHERE 
		NOT EXISTS(
			SELECT	
				1
			FROM	[dbo].[OrderDetail]	OD	(NOLOCK)
			WHERE
				OD.OrderId		=	@OrderId
				AND	OD.ProductId	=	X.ProductId
		)
END