-- =============================================
-- Author:		changngoktb
-- =============================================
CREATE PROCEDURE [dbo].[sp_Category_GetDataPaging]
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
		SET @querySort = ' '+@sort+' '+@order+' ';
	END

	SET @query = '	SELECT	Id
							,Code
							,Name
							,ParentId
							,TitleSeo
							,KeywordSeo
							,[Description]
							,Sort
							,IsDel
							,PositionMenu
							,Published
							,PictureId
							,IsHot
							,CreatedDate
							,ModifiedDate
							,IsHomePage
							,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querySort+') 
					FROM dbo.Category
					WHERE 1	= 1 ';
	IF ISNULL(@txtSearch,'') <> ''
    BEGIN
        SET @query = @query + ' AND	(Name LIKE N''%' + @txtSearch + '%''
								OR Code LIKE  N''%' + @txtSearch + '%'')';
	END
	
	SET @queryCountTotal = 'select @totalRecord = count(*) from ('+@query+') as qcount';

	SET @queryData = 'select * from ('+@query+') as qr where 1 = 1 ';

	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		SET @queryData = @queryData+ ' and qr.rownumber > '+CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50))
																+' and qr.rownumber <= '+CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END

	
	EXECUTE sp_executesql @queryCountTotal,N'@totalRecord int out',@totalRecord OUT;
	EXECUTE sp_executesql @queryData;
END