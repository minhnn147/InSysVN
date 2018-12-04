CREATE PROC [dbo].[sp_Input_Return_InsertOrUpdate]
@UserId INT,
@xml xml
AS
BEGIN
--thêm mới phiếu nhập trả
	INSERT INTO dbo.Input_Return
	(
		ProductId
		,Quantity
		,Price
		,Note
		,WarehouseId
		,CreatedBy
		,CreatedDate
	)
	SELECT
		tblInsert.ProductId
		,tblInsert.Quantity
		,tblInsert.Price
		,tblInsert.Note
		,tblInsert.WarehouseId
		,@UserId
		,GETDATE()
	FROM 
	(
		SELECT
			Id			=	tblxml.value('(Id)[1]','INT'),
			ProductId	=	tblxml.value('(ProductId)[1]','BIGINT'),
			Quantity	=	tblxml.value('(Quantity)[1]','INT'),
			Price		=	tblxml.value('(Price)[1]','INT'),
			Note		=	tblxml.value('(Note)[1]','nvarchar(max)'),
			WarehouseId	=	tblxml.value('(WarehouseId)[1]','int')
		FROM @xml.nodes('/ArrayOfInput_ReturnEntity/Input_ReturnEntity') AS XD(tblxml)
	) AS tblInsert
	WHERE tblInsert.Id = 0
	--thêm số lượng vào phiếu khi nhập mới
	UPDATE dbo.Products 
	SET InventoryNumber = InventoryNumber + pins.Quantity
	FROM dbo.Products AS p
	JOIN (
			SELECT
				tblInsert.Quantity
				,tblInsert.ProductId
			FROM 
			(
				SELECT
					Id			=	tblxml.value('(Id)[1]','INT')
					,ProductId	=	tblxml.value('(ProductId)[1]','BIGINT')
					,Quantity	=	tblxml.value('(Quantity)[1]','INT')
				FROM @xml.nodes('/ArrayOfInput_ReturnEntity/Input_ReturnEntity') AS XD(tblxml)
			) AS tblInsert 
			WHERE tblInsert.Id = 0
		) AS pins ON p.Id = pins.ProductId

		--Trừ đi số lượng phiếu cũ
		UPDATE dbo.Products 
		SET InventoryNumber = InventoryNumber - pis.Quantity
		FROM dbo.Products AS p
		JOIN
		(
			SELECT
				ir.ProductId,
				ir.Quantity
			FROM
			(
				SELECT
					Id			=	tblxml.value('(Id)[1]','INT')
				FROM @xml.nodes('/ArrayOfInput_ReturnEntity/Input_ReturnEntity') AS XD(tblxml)
			) AS tblInsert
			JOIN dbo.Input_Return ir ON ir.Id = tblInsert.Id
			WHERE tblInsert.Id != 0
		) AS pis ON p.Id = pis.ProductId

			
--cập nhật lại phiếu nhập trả khi id != null
	UPDATE dbo.Input_Return
	SET
		ProductId		=	tblUpdate.ProductId
		,Quantity		=	tblUpdate.Quantity
		,Price			=	tblUpdate.Price
		,Note			=	tblUpdate.Note
		,WarehouseId	=	tblUpdate.WarehouseId
		,ModifiedBy		=	@UserId
		,ModifiedDate	=	GETDATE()
	FROM dbo.Input_Return as IR
	JOIN 
	(
		SELECT
			tblInsert.Id
			,tblInsert.ProductId
			,tblInsert.Quantity
			,tblInsert.Price
			,tblInsert.Note
			,tblInsert.WarehouseId
		FROM 
		(
			SELECT
				Id			=	tblxml.value('(Id)[1]','INT'),
				ProductId	=	tblxml.value('(ProductId)[1]','BIGINT'),
				Quantity	=	tblxml.value('(Quantity)[1]','INT'),
				Price		=	tblxml.value('(Price)[1]','INT'),
				Note		=	tblxml.value('(Note)[1]','nvarchar(max)'),
				WarehouseId	=	tblxml.value('(WarehouseId)[1]','int')
			FROM @xml.nodes('/ArrayOfInput_ReturnEntity/Input_ReturnEntity') AS XD(tblxml)
		) AS tblInsert
		WHERE tblInsert.Id != 0
	) AS tblUpdate ON IR.Id = tblUpdate.Id


	--cộng thêm số lượng phiếu mới

	UPDATE dbo.Products 
	SET InventoryNumber = InventoryNumber + pins.Quantity
	FROM dbo.Products AS p
	JOIN (
			SELECT
				tblInsert.Quantity
				,tblInsert.ProductId
			FROM 
			(
				SELECT
					Id			=	tblxml.value('(Id)[1]','INT')
					,ProductId	=	tblxml.value('(ProductId)[1]','BIGINT')
					,Quantity	=	tblxml.value('(Quantity)[1]','INT')
				FROM @xml.nodes('/ArrayOfInput_ReturnEntity/Input_ReturnEntity') AS XD(tblxml)
			) AS tblInsert 
			WHERE tblInsert.Id != 0
		) AS pins ON p.Id = pins.ProductId

END