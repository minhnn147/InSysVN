
CREATE PROCEDURE [dbo].[sp_Products_Report]
	@txtSearch				NVARCHAR(255) = NULL,
	@startDate				DATETIME = NULL,
	@endDate				DATETIME = NULL,
	@WarehouseId			INT,
	@pageNumber				INT,
	@pageSize				INT,
	@order					NVARCHAR(255) = NULL,
	@sort					NVARCHAR(255) = NULL,
	@totalRecord			INT OUT
AS
BEGIN

	DECLARE @query NVARCHAR(MAX);
	DECLARE @queryCountTotal NVARCHAR(max);
	DECLARE @queryGetData NVARCHAR(max);
	DECLARE @querysort NVARCHAR(MAX);
	DECLARE @query2Table NVARCHAR(MAX);
	DECLARE @keySearch NVARCHAR(MAX);
	IF ISNULL(@order,'')='' OR ISNULL(@sort,'')=''
	BEGIN
		SET @querysort = ' sell.QuantitySell DESC ';
	END
	ELSE 
	BEGIN
		IF @sort = 'QuantityInventory' SET @sort = 'p.InventoryNumber';
		IF @sort = 'Barcode' SET @sort = 'sell.Barcode';
		SET @querysort = ' '+@sort+' '+@order+' ';
	END
	

	DECLARE @qrWhereSell NVARCHAR(MAX) = '';
	DECLARE @qrWhereReturn NVARCHAR(MAX) = '';
	DECLARE @qrWherePro NVARCHAR(MAX) = '';
	
	IF @startDate IS NOT NULL
	BEGIN
		SET	@qrWhereSell = @qrWhereSell + ' AND CONVERT(DATE,o.OrderDate) >= CONVERT(DATE,@startDate) ';
		SET @qrWhereReturn = @qrWhereReturn + ' AND (
														CASE 
															WHEN rt.ModifiedDate IS NULL
																THEN CONVERT(DATE,rt.CreatedDate)
															ELSE rt.ModifiedDate
														END
													) >= CONVERT(DATE,@startDate) ';
		SET @qrWherePro = @qrWherePro + ' AND CONVERT(DATE,o.PromotedDate) >= CONVERT(DATE,@startDate) ';
	END
	IF @endDate IS NOT NULL
	BEGIN
		SET	@qrWhereSell = @qrWhereSell + ' AND CONVERT(DATE,o.OrderDate) <= CONVERT(DATE,@endDate) ';
		SET @qrWhereReturn = @qrWhereReturn + ' AND (
														CASE 
															WHEN rt.ModifiedDate IS NULL
																THEN CONVERT(DATE,rt.CreatedDate)
															ELSE rt.ModifiedDate
														END
													) <= CONVERT(DATE,@endDate) ';
		SET	@qrWherePro = @qrWherePro + ' AND CONVERT(DATE,o.PromotedDate) <= CONVERT(DATE,@endDate) ';
	END
	Set @keySearch = ' where  1 = 1 '
	IF @WarehouseId IS NOT NULL
	BEGIN
		SET @qrWhereSell = @qrWhereSell + ' AND o.WarehouseId = @WarehouseId ';
		SET @qrWhereReturn = @qrWhereReturn + ' AND rt.WarehouseId = @WarehouseId ';
		SET @qrWherePro = @qrWherePro + ' AND o.WarehouseId = @WarehouseId ';
		Set @keySearch = @keySearch + ' AND p.WarehouseId = @WarehouseId  '
	END
	
	IF ISNULL(@txtSearch,'') <> ''
	BEGIN
		SET @qrWhereSell = @qrWhereSell + ' AND (od.ProductName LIKE N''%'+@txtSearch+'%''OR od.Barcode LIKE N''%'+@txtSearch+'%'') ';
		Set @keySearch = @keySearch + ' AND (p.ProductName LIKE N''%'+@txtSearch+'%''OR p.Barcode LIKE N''%'+@txtSearch+'%'') ';
	END
	 
	SET @query2Table = 'Select ProductId,
		Barcode,
		QuantitySell = sum(QuantitySell),
		TotalPriceSell = sum(TotalPriceSell),
		QuantityPromotion = sum(QuantityPromotion),
		QuantityReturn = sum(QuantityReturn),
		TotalPriceReturn = sum(TotalPriceReturn)
		From
		(
			SELECT							od.ProductId
											,od.Barcode
											,QuantitySell = ISNULL(SUM(od.Quantity),0)
											,TotalPriceSell = ISNULL(SUM(od.SellPrice*od.Quantity - od.SellPrice*od.Quantity*od.Discount/100),0)
											,QuantityPromotion = 0
											,QuantityReturn = 0
											,TotalPriceReturn = 0
									FROM dbo.OrderDetail od
									JOIN dbo.Orders o ON od.OrderId = o.Id	
									JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
									WHERE o.isActive = 1 AND o.Status = 1 AND w.isActive = 1
									'+@qrWhereSell+'						
									GROUP BY od.ProductId, od.Barcode
			
								
		union all

			SELECT						op.ProductId
										,p.Barcode
										,QuantitySell = 0
										,TotalPriceSell = 0
										,QuantityPromotion = SUM(op.Quantity)
										,QuantityReturn = 0
										,TotalPriceReturn = 0
									FROM dbo.OrderPromotion op
									JOIN dbo.Products p ON op.ProductId = p.Id
									JOIN dbo.Orders o ON op.OrderId = o.Id
									JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
									WHERE o.isActive = 1 AND o.Status = 1 AND w.isActive = 1
									'+@qrWherePro+'
									GROUP BY op.ProductId, p.Barcode
			
		
		union all

			SELECT						rtd.ProductId
										,p.Barcode
										,QuantitySell = 0
										,TotalPriceSell = 0
										,QuantityPromotion = 0
										,QuantityReturn = ISNULL(SUM(rtd.QuantityReturn),0)
										,TotalPriceReturn = ISNULL(SUM(rtd.QuantityReturn*rtd.PriceReturn),0)
									FROM dbo.[Return] rt
									JOIN dbo.ReturnDetail rtd on rt.id = rtd.ReturnId
									JOIN dbo.Orders o ON rt.OrderId = o.Id
									JOIN dbo.OrderDetail od on od.OrderId = o.Id
									JOIN dbo.Products p ON od.ProductId = p.Id
									JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
									WHERE o.isActive = 1 AND o.Status = 1 AND w.isActive  = 1 And o.PromotedDate is not Null
									'+@qrWhereReturn+'
									GROUP BY rtd.ProductId, p.Barcode
			
		) as ProductALL
		group by ProductALL.Barcode , ProductALL.ProductId
						
						
	';
	Set @query = 'SELECT	
							ProductId=p.Id
							,p.ProductName
							,ProductCategory = 
								CASE WHEN 
								(
									SELECT TOP 1 c.Name FROM dbo.ProductCate pc
									JOIN dbo.Category c ON pc.CateId = c.Id
									WHERE pc.ProductId = sell.ProductId
								) IS NULL
									THEN (SELECT ProductCategory FROM Products where Barcode = p.Barcode and WarehouseId = p.WarehouseId)
								ELSE 
									(
										SELECT TOP 1 c.Name FROM dbo.ProductCate pc
										JOIN dbo.Category c ON pc.CateId = c.Id
										WHERE pc.ProductId = sell.ProductId
									)
								END
							,p.Barcode
							,QuantitySell		= isnull(sell.QuantitySell,0)
							,TotalPriceSell		=isnull(sell.TotalPriceSell,0)
							,QuantityReturn		=isnull(sell.QuantityReturn,0)
							,TotalPriceReturn	=isnull(sell.TotalPriceReturn,0)
							,QuantityPromotion	=isnull(sell.QuantityPromotion,0)
							,QuantityInventory	= (select Quantity = TotalPb.Quantity + isNull(TotalTemp.QuantityPromotion,0)+Isnull(TotalTemp.QuantitySell,0) from
										(SELECT Quantity = SUM(Quantity),pb.Barcode,pb.WarehouseId FROM dbo.ProductBarcode pb
														 where pb.WarehouseId = p.WarehouseId and pb.Barcode = p.BarCode
														GROUP BY pb.Barcode,pb.WarehouseId) as TotalPb
									left join

									(SELECT	o.WarehouseId
											,od.Barcode
											,QuantitySell = ISNULL(SUM(od.Quantity),0)											
											,QuantityPromotion = 0
									FROM dbo.OrderDetail od
									JOIN dbo.Orders o ON od.OrderId = o.Id	
									JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
									WHERE o.isActive = 1 AND o.Status = 0 AND w.isActive = 1					
									GROUP BY o.WarehouseId, od.Barcode
			
								
						union all

							SELECT						o.WarehouseId
										,p.Barcode
										,QuantitySell = 0
										,QuantityPromotion = SUM(op.Quantity)
									FROM dbo.OrderPromotion op
									JOIN dbo.Products p ON op.ProductId = p.Id
									JOIN dbo.Orders o ON op.OrderId = o.Id
									JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
									WHERE o.isActive = 1 AND o.Status = 0 AND w.isActive = 1 And o.PromotedDate is Null
									GROUP BY o.WarehouseId, p.Barcode
							) as TotalTemp on TotalPb.Barcode = TotalTemp.Barcode and TotalPb.WarehouseId = TotalTemp.WarehouseId)

							,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querysort+')
			From('+@query2Table+') as sell
			right join dbo.Products p ON sell.ProductId = p.Id AND sell.Barcode = p.Barcode 
			JOIN dbo.Warehouses w ON p.WarehouseId = w.Id 
			'+@keySearch+'
			';
	SET @queryCountTotal = 'SELECT	@totalRecord = COUNT(*) FROM ('+@query+') as abc';
	SET @queryGetData = 'SELECT * FROM ('+@query+') as qr WHERE 1 = 1' ;

	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		 SET @queryGetData = @queryGetData+' and qr.rownumber > '
                        + CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50)) + 
                        ' AND qr.rownumber <= ' 
                        + CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END

	--***********************
print @queryGetData

	EXECUTE sp_executesql @queryCountTotal, N'@startDate DATETIME, @endDate DATETIME, @WarehouseId INT, @txtSearch NVARCHAR(255), @totalRecord INT OUT', @startDate, @endDate, @WarehouseId, @txtSearch, @totalRecord OUT;
	EXECUTE sp_executesql @queryGetData,N'@startDate DATETIME, @endDate DATETIME,@WarehouseId INT, @txtSearch NVARCHAR(255)',@startDate,@endDate, @WarehouseId, @txtSearch;

 END