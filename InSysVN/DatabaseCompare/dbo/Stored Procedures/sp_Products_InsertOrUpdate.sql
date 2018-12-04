CREATE PROC [dbo].[sp_Products_InsertOrUpdate]
@Id BIGINT,
@ProductCategory NVARCHAR(255),
@ProductName NVARCHAR(255),
@Barcode NVARCHAR(50),
@InventoryNumber INT,
@ComputeUnit NVARCHAR(50),
@Price DECIMAL(18,2),
@SellPrice DECIMAL(18,2),
@SellPriceShop DECIMAL(18,2),
@Description NVARCHAR(255),
@ExpiredDate datetime
AS
BEGIN
	IF(@Id IS NULL)
	BEGIN
		INSERT INTO dbo.Products
		        (
					ProductCategory
					,ProductName
					,Barcode
					,InventoryNumber
					,ComputeUnit
					,Price
					,SellPrice
					,CreatedDate
					,Status
					,Description
					,ExpiredDate
					,SellPriceShop
		        )
		VALUES  ( 
					@ProductCategory
					,@ProductName
					,@Barcode
					,@InventoryNumber
					,@ComputeUnit
					,@Price
					,@SellPrice
					,GETDATE()
					,0
					,@Description
					,@ExpiredDate
					,@SellPriceShop
		        )
		SET @Id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE dbo.Products
		SET 
			ProductName = @ProductName
			,Barcode = @Barcode
			,InventoryNumber = @InventoryNumber
			,ComputeUnit = @ComputeUnit
			,Price = @Price
			,SellPrice = @SellPrice
			,SellPriceShop = @SellPriceShop
			,ModifiedDate = GETDATE()
			,Description=@Description
			,ExpiredDate=@ExpiredDate
			,Status = 0
		WHERE Id = @Id
	END
	SELECT TOP 1 * FROM dbo.Products WHERE Id = @Id
END