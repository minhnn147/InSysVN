-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spProducts_Report]
	-- Add the parameters for the stored procedure here
	@textSearch				NVARCHAR(255) = NULL,
	@startDate				NVARCHAR(255) = NULL,
	@endDate				NVARCHAR(255) = NULL,
	@shopId					INT,
	@page					INT,
	@resultsPerPage			INT,
	@sort					NVARCHAR(255) = NULL,
	@totalRecord			INT OUT
AS
BEGIN
	DECLARE @pagingQuery NVARCHAR(MAX);
    DECLARE @totalQuery NVARCHAR(MAX);
	DECLARE @conditionDate NVARCHAR(MAX) = '';
	DECLARE @keywordQuery NVARCHAR(MAX) = ''
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED --WITH (NOLOCK)
    IF ISNULL(@sort, '') = ''
    BEGIN
        SET @sort = 'P.[Id]';
    END


	IF ISNULL(@startDate,'') <> ''
	BEGIN
		SET	 @conditionDate = @conditionDate + ' AND CONVERT(DATE,O.OrderDate) >= CONVERT(DATE,''' + @startDate + ''',103)'
	END
	IF ISNULL(@endDate,'') <> ''
	BEGIN
		SET	 @conditionDate = @conditionDate + ' AND CONVERT(DATE,O.OrderDate) <= CONVERT(DATE,''' + @endDate + ''',103)'
	END

	 IF ISNULL(@textSearch,'') <> ''
    BEGIN
        SET @keywordQuery = @keywordQuery + ' AND (ProductName LIKE N''%' + @textSearch + '%'''+                                              
                                              ' OR Barcode LIKE N''%' + @textSearch + '%'')';
    END
    -- get total record
    SET @totalQuery = 'SELECT 
							@totalRecordOut = COUNT(*) 
						FROM 
						(
								SELECT 
									RowIndex		=	ROW_NUMBER() OVER (ORDER BY ' + @sort + ')
									,ProductId		=	OD.ProductId
									,Quantity		=	OD.Quantity
									,GrandTotal		=	OD.GrandTotal
									,Barcode		=	P.Barcode
									,Name			=	P.ProductName
									,Category		=	P.ProductCategory
								FROM 
								(
									SELECT
										 ProductId		=	OD.ProductId
										,Quantity		=	SUM(OD.Quantity)
										,GrandTotal		=	SUM(OD.Quantity * OD.SellPrice)	 
										,ShopId			=	O.ShopId
									FROM	[dbo].[OrderDetail] OD	(NOLOCK)

									INNER JOIN	[dbo].[Orders] O	(NOLOCK)
										ON	O.Orderid	=	OD.OrderId	
									WHERE
										(1=1)
										AND	O.ShopId	=	'+ CONVERT(VARCHAR,@shopId)+'
									'+@conditionDate+'
									GROUP BY 
										OD.ProductId
										,O.ShopId
								)	OD
								INNER JOIN [dbo].[Products]	P	(NOLOCK)
									ON	P.Id			=	OD.ProductId
									AND	P.ShopId		=	OD.ShopId

								WHERE
									(1 = 1)
									'+ @keywordQuery +'
						) AS TMP';

    --get data by paging
    SET @pagingQuery = 'SELECT 
							TMP.* 
						FROM 
						(
								SELECT 
									RowIndex		=	ROW_NUMBER() OVER (ORDER BY ' + @sort + ')
									,ProductId		=	OD.ProductId
									,Quantity		=	OD.Quantity
									,GrandTotal		=	OD.GrandTotal
									,Barcode		=	P.Barcode
									,Name			=	P.ProductName
									,Category		=	P.ProductCategory
								FROM 
								(
									SELECT
										 ProductId		=	OD.ProductId
										,Quantity		=	SUM(OD.Quantity)
										,GrandTotal		=	SUM(OD.Quantity * OD.SellPrice)	 
										,ShopId			=	O.ShopId
									FROM	[dbo].[OrderDetail] OD	(NOLOCK)

									INNER JOIN	[dbo].[Orders] O	(NOLOCK)
										ON	O.Orderid	=	OD.OrderId	
									WHERE
										(1=1)
										AND	O.ShopId	=	'+ CONVERT(VARCHAR,@shopId)+'
									'+@conditionDate+'
									GROUP BY 
										OD.ProductId
										,O.ShopId
								)	OD
								INNER JOIN [dbo].[Products]	P	(NOLOCK)
									ON	P.Id			=	OD.ProductId
									AND	P.ShopId		=	OD.ShopId

								WHERE
									(1 = 1)
									'+ @keywordQuery +'
						) AS TMP
                        '        
						
	IF ISNULL(@page,0) <> 0 AND ISNULL(@resultsPerPage,0) <> 0
	BEGIN
		 SET @pagingQuery += '	WHERE RowIndex > '
                        + CAST((@page - 1) * @resultsPerPage AS NVARCHAR(50)) + 
                        ' AND RowIndex <= ' 
                        + CAST((@page) * @resultsPerPage AS NVARCHAR(50));
	END                   
   
    PRINT @totalQuery
    --GO
    PRINT @pagingQuery
    EXECUTE sp_executesql @totalQuery
        ,N'@totalRecordOut int OUTPUT'
        ,@totalRecordOut = @totalRecord OUTPUT;

    EXECUTE (@pagingQuery);
    select @pagingQuery
END