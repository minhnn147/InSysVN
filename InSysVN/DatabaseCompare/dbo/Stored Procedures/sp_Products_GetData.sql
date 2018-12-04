-- =============================================
-- Author:		changngoktb
-- Create date: 24/05/2018
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Products_GetData]
	-- Add the parameters for the stored procedure here
	@txtSearch				NVARCHAR(MAX) = NULL,
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

    IF @order IS NULL OR @sort IS NULL
    BEGIN
        SET @querySort = ' CreatedDate DESC ';
    END
	ELSE
	BEGIN
		IF @sort = 'ProductCategory'
		BEGIN
			SET @sort = ' CASE 
							WHEN pc.ProductCategory IS NULL
								THEN p.ProductCategory
							ELSE pc.ProductCategory
						END ';
		END
		SET @querySort = ' '+@sort+' '+@order+' ';
	END

	SET @query = '	SELECT	
							p.Id
							,p.ProductCategory
							,p.ProductName
							,p.ProductCode
							,p.Barcode
							,p.MainImage
							,p.Image
							,p.Description
							,p.Status
							,p.CreatedDate
							,p.ModifiedDate
							,InventoryNumber = 0
							,p.ComputeUnit
							,p.Price
							,p.SellPrice
							,p.SellPriceShop
							,p.ExpiredDate
							,p.Allowcated
							,DateSync
							,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querySort+') 
					FROM dbo.Products p
					
					WHERE 1	= 1 ';


	IF ISNULL(@txtSearch,'') <> ''
    BEGIN
        SET @query = @query + ' AND	(p.ProductName LIKE N''%' + @txtSearch + '%''
								OR p.Barcode LIKE  N''%' + @txtSearch + '%'')';
	END
	
	SET @queryCountTotal = 'select @totalRecord = count(*) from ('+@query+') as qcount';

	SET @queryData = 'select * from ('+@query+') as qr where 1 = 1 ';

	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		SET @queryData = @queryData+ ' and qr.rownumber > '+CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50))
																+' and qr.rownumber <= '+CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END
	PRINT @queryData
	
	EXECUTE sp_executesql @queryCountTotal,N'@totalRecord int out',@totalRecord OUT;
	EXECUTE sp_executesql @queryData;
END