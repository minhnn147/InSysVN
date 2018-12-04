CREATE PROC [dbo].[sp_Input_Return_GetData]
	@txtSearch				NVARCHAR(255) = NULL,
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

    IF(ISNULL(@order, '') = '' OR ISNULL(@sort,'')='')
		BEGIN
			SET @querySort = ' ir.CreatedDate DESC ';
		END
	ELSE
		BEGIN
			IF @sort = 'CreatedDate' SET @sort = 'ir.CreatedDate'
			IF @sort = 'Price' SET @sort = 'ir.Price'
			IF @sort = 'TotalPrice' SET @sort = '(ir.Price * ir.Quantity)'
			IF @sort = 'CreatedByName' SET @sort = 'u.UserName'
			IF @sort = 'ModifiedDate' SET @sort = 'ir.ModifiedDate'
			IF @sort = 'ModifiedByName' SET @sort = 'u1.UserName'
			SET @querySort =  ' '+@sort+' '+@order+' ';
		END
	print @querySort;
    SET @query = '	SELECT	ir.*
							,p.ProductName
							,p.Barcode
							,CreatedByName = u.UserName
							,ModifiedByName = u1.UserName
							,rownumber		=		ROW_NUMBER() OVER(ORDER BY '+@querySort+') 
					FROM dbo.Input_Return AS ir
					JOIN dbo.Products AS p ON ir.ProductId = p.Id
					LEFT JOIN dbo.Users AS u ON ir.CreatedBy = u.Id
					LEFT JOIN dbo.Users AS u1 ON ir.ModifiedBy = u1.Id
					where ir.isActive = 1 
				';

	IF ISNULL(@txtSearch,'') <> ''
    BEGIN
        SET @query = @query + ' AND	p.ProductName LIKE N''%' + @txtSearch + '%''';
	END

	SET @queryCountTotal = 'select @totalRecord = count(*) from ('+@query+') as qcount';

	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		SET @queryData = 'select * from ('+@query+') as qr where qr.rownumber > '+CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50))
																+' and qr.rownumber <= '+CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END

	EXECUTE sp_executesql @queryCountTotal,N'@totalRecord int out',@totalRecord OUT;
	EXECUTE sp_executesql @queryData;
END