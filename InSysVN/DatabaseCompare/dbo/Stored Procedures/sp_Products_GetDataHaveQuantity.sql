
CREATE PROCEDURE [dbo].[sp_Products_GetDataHaveQuantity]
	-- Add the parameters for the stored procedure here
	@txtSearch				NVARCHAR(255) = NULL,
	@WarehouseId			INT,
	@pageNumber				INT,
	@pageSize				INT,
	@order					NVARCHAR(255) = NULL,
	@sort					NVARCHAR(255) = NULL,
	@totalRecord			INT OUT
AS
BEGIN
	DECLARE @query NVARCHAR(MAX);
	DECLARE @queryCountTotal NVARCHAR(MAX);
    DECLARE @queryData NVARCHAR(MAX);
	DECLARE @querySort NVARCHAR(MAX);

    IF		ISNULL(@order, '') = '' OR ISNULL(@sort,'')=''
    BEGIN
        SET @querySort = ' Id ASC ';
    END
	ELSE
	BEGIN
		SET @querySort = ' '+@sort+' '+@order+' ';
	END

	SET @query = '	SELECT	Id,
							ProductCategory,
							ProductName,
							ProductCode,
							Barcode,
							Image,
							Description,
							Status,
							CreatedDate,
							ModifiedDate,
							InventoryNumber = 	(SELECT SUM(Quantity) FROM dbo.ProductBarcode 
														 where WarehouseId = '+CONVERT(NVARCHAR(max),@WarehouseId)+' and Barcode = p.Barcode
														GROUP BY Barcode),
							ComputeUnit,
							Price,
							SellPrice,
							SellPriceShop,
							ExpiredDate,
							Allowcated,
							WarehouseId
							,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querySort+' ) 
					FROM dbo.Products p
					WHERE 1	= 1 and SellPrice Is not Null' --and InventoryNumber > 0
	
	IF @WarehouseId IS NOT NULL
    BEGIN
		SET @query = @query + ' AND p.WarehouseId = '+CAST(@WarehouseId AS NVARCHAR(max))+'';
	END

	IF ISNULL(@txtSearch,'') <> ''
    BEGIN
        SET @query = @query + ' AND	(p.ProductName LIKE N''%' + @txtSearch + '%''
								OR p.Barcode LIKE  N''%' + @txtSearch + '%'')';
	END
	
	SET @queryCountTotal = 'select @totalRecord = count(*) from ('+@query+') as qcount';

	PRINT @queryCountTotal
	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		SET @queryData = 'select * from ('+@query+') as qr where qr.rownumber > '+CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50))
																+' and qr.rownumber <= '+CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END

	
	EXECUTE sp_executesql @queryCountTotal,N'@totalRecord int out',@totalRecord OUT;
	EXECUTE sp_executesql @queryData;
END