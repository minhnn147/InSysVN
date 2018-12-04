--changngoktb
CREATE PROCEDURE [dbo].[sp_Report_DetailGeneral]
	@txtSearch				NVARCHAR(255) = NULL,
	@opType					INT,
	@startDate				DATETIME,
	@endDate				DATETIME,
	@WarehouseId			INT,
	@pageNumber				INT,
	@pageSize				INT,
	@order					NVARCHAR(255) = NULL,
	@sort					NVARCHAR(255) = NULL,
	@CreatedSearchId		INT,
	@CustomerSearchId		INT,
	@totalRecord			INT OUT
AS
BEGIN

	DECLARE @query NVARCHAR(MAX);
	DECLARE @queryCountTotal NVARCHAR(max);
	DECLARE @queryGetData NVARCHAR(max);
	DECLARE @querysort NVARCHAR(MAX);

	IF ISNULL(@order,'')='' OR ISNULL(@sort,'')=''
	BEGIN
		SET @querysort = ' o.OrderDate DESC ';
	END
	ELSE
	BEGIN
		IF @sort = 'OrderDateTime' SET @sort = 'o.OrderDate';
		IF @sort = 'CustomerName' SET @sort = 'c.Name';
		IF @sort = 'CreatedByName' SET @sort = 'u.UserName';
		IF @sort = 'OrderCode' SET @sort = 'o.Code';
		IF @sort = 'CreatedBy_Name' SET @sort = 'u.FullName';
		IF @sort = 'CreatedBy_UserName' SET @sort = 'u.UserName';
		IF @sort = 'SellPrice' SET @sort = 'od.SellPrice';
		IF @sort = 'TotalPrice' SET @sort = '(od.Quantity * od.SellPrice)';
		SET @querysort = ' '+@sort+' '+@order+' ';
	END

	SET @query =	'
SELECT * 
FROM 
(
	(
		SELECT	
				OrderId					=	o.Id
				,OrderCode				=	o.OrderCode
				,OrderDate				=	o.OrderDate
				,CreatedBy_Name			=	u.FullName
				,CreatedBy_UserName		=	u.UserName
				,CustomerId				=	c.Id
				,CustomerName			=	c.Name
				,ProductName			=	p.ProductName
				,Barcode				=	p.Barcode
				,ComputeUnit			=	p.ComputeUnit
				,Quantity				=	od.Quantity
				,SellPrice				=	od.SellPrice
				,Discount_Product		=	od.Discount
				,ProductTotal			=	(od.Quantity * od.SellPrice - od.Quantity * od.SellPrice * od.Discount/100)
				,TotalPrice				=	o.ProductTotal
				,Discount				=	o.Discount
				,PointUsed				=	o.PointUsed
				,GrandTotal				=	o.GrandTotal
				,PaidGuests				=	ISNULL(o.PayCash,0) + ISNULL(o.PayByCard,0)
				,RefundMoney			=	o.RefundMoney
				,WarehouseId			=	o.WarehouseId
				,CreatedBy				=	o.CreatedBy
				,CardNumber				=	ca.CardNumberId
				,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querysort+')
		FROM dbo.OrderDetail od
		JOIN dbo.Products p ON od.ProductId = p.Id
		JOIN dbo.Orders o ON od.OrderId = o.Id
		JOIN dbo.Users u ON o.CreatedBy = u.Id
		LEFT JOIN dbo.Customer c ON o.CustomerId = c.Id
		LEFT JOIN dbo.CardNumbers ca ON ca.CustomerId = c.Id
		WHERE o.isActive = 1 AND o.Status = 1 AND p.WarehouseId = o.WarehouseId
	)
	UNION ALL

	(
		SELECT	
				OrderId					=	o.Id
				,OrderCode				=	o.OrderCode
				,OrderDate				=	o.OrderDate
				,CreatedBy_Name			=	u.FullName
				,CreatedBy_UserName		=	u.UserName
				,CustomerId				=	c.Id
				,CustomerName			=	c.Name
				,ProductName			=	p.ProductName
				,Barcode				=	p.Barcode
				,ComputeUnit			=	p.ComputeUnit
				,Quantity				=	op.Quantity
				,SellPrice				=	0
				,Discount_Product		=	0
				,ProductTotal			=	0
				,TotalPrice				=	o.ProductTotal
				,Discount				=	o.Discount
				,PointUsed				=	o.PointUsed
				,GrandTotal				=	o.GrandTotal
				,PaidGuests				=	ISNULL(o.PayCash,0) + ISNULL(o.PayByCard,0)
				,RefundMoney			=	o.RefundMoney
				,WarehouseId			=	o.WarehouseId
				,CreatedBy				=	o.CreatedBy
				,CardNumber				=	ca.CardNumberId
				,rownumber = ROW_NUMBER() OVER(ORDER BY '+ @querysort+' )
		FROM dbo.OrderPromotion op
		JOIN dbo.Products p ON op.ProductId = p.Id
		JOIN dbo.Orders o ON op.OrderId = o.Id
		JOIN dbo.Users u ON o.CreatedBy = u.Id
		LEFT JOIN dbo.Customer c ON o.CustomerId = c.Id
		LEFT JOIN dbo.CardNumbers ca ON ca.CustomerId = c.Id
		WHERE o.isActive = 1 AND o.Status = 1 AND p.WarehouseId = o.WarehouseId
	)
) AS a where 1=1 ';
	IF @WarehouseId IS NOT NULL
	BEGIN
		SET @query = @query + ' and a.WarehouseId = ' + CONVERT(NVARCHAR(MAX),@WarehouseId)+' ';
	END
	IF @opType = 0 --today
	BEGIN
		SET	 @query = @query + ' AND CONVERT(DATE,a.OrderDate) = CONVERT(DATE,GETDATE())';
	END 
	ELSE IF @opType = 1 -- this month
	BEGIN
		SET	 @query = @query + ' AND CONVERT(VARCHAR(7),a.OrderDate,126) = CONVERT(VARCHAR(7), GETDATE(), 126) ';
	END
	ELSE IF @opType = 2 -- this year
	BEGIN
		SET	 @query = @query + ' AND YEAR(CONVERT(DATE,a.OrderDate)) = YEAR(CONVERT(DATE,GETDATE())) ';
	END
	ELSE
	BEGIN
		IF @startDate IS NOT NULL
			BEGIN
				SET	 @query = @query + ' AND CONVERT(DATE,a.OrderDate) >= CONVERT(DATE, @startDate) '
			END

		IF @endDate IS NOT NULL
			BEGIN
				SET	 @query = @query + ' AND CONVERT(DATE,a.OrderDate) <= CONVERT(DATE, @endDate) '
			END
	END
	IF ISNULL(@txtSearch,'') <> ''
		BEGIN
			SET @query = @query + ' AND (a.OrderCode LIKE N''%' + @txtSearch + '%'' or a.CustomerName LIKE N''%' + @txtSearch + '%'' or a.CardNumber LIKE N''%'+@txtSearch+ '%'') ';
		END

	IF ISNULL(@CreatedSearchId,0)<>0
	BEGIN
		SET @query = @query + ' AND a.CreatedBy = '+CONVERT(NVARCHAR(MAX),@CreatedSearchId)+'';
	END
	IF @CustomerSearchId IS NOT NULL
	BEGIN
		IF @CustomerSearchId = -1
		BEGIN
			SET @query = @query + ' AND a.CustomerId IS NULL ';
		END
		ELSE
        BEGIN
			SET @query = @query + ' AND a.CustomerId = @CustomerSearchId ';
		END
	END
	SET @queryCountTotal = 'SELECT	@totalRecord = COUNT(*) FROM ('+@query+') as abc';
	SET @queryGetData = 'SELECT * FROM ('+@query+') as qr WHERE 1 = 1 ';

	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		 SET @queryGetData = @queryGetData+' and qr.rownumber > '
                        + CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50)) + 
                        ' AND qr.rownumber <= ' 
                        + CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END
	SET @queryGetData = @queryGetData + ' order by qr.OrderId desc';

	EXECUTE sp_executesql @queryCountTotal, N'@startDate DATETIME,@endDate DATETIME, @CustomerSearchId int, @totalRecord INT OUT',@startDate,@endDate,@CustomerSearchId,@totalRecord OUT;
	EXECUTE sp_executesql @queryGetData, N'@startDate DATETIME,@endDate DATETIME, @CustomerSearchId int',@startDate,@endDate,@CustomerSearchId;

 END