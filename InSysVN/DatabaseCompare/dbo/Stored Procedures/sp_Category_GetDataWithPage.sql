--changngoktb
CREATE PROC [dbo].[sp_Category_GetDataWithPage]
@search NVARCHAR(500),
@page int,
@total int out
as
declare @query nvarchar(max);
declare @queryTotal nvarchar(max);
declare @queryPage nvarchar(max);

SET @query = '	SELECT	Id ,
						Code ,
						Name ,
						ParentId ,	
						CreatedDate ,
						ModifiedDate ,
						ROW_NUMBER() OVER(ORDER BY Name ASC) AS rownumber
				FROM dbo.Category
				WHERE isActive = 1 ';
IF ISNULL(@search,'') <> ''
BEGIN
	SET @query = @query + 'AND (Name LIKE ''%'+@search+'%'' OR Code LIKE ''%'+@search+'%'')';
END

SET @queryTotal = 'select @total = COUNT(*) from ('+@query+') as tbltotal';

SET @queryPage = '	SELECT *
					FROM
					('+@query+') AS q
					WHERE 1 = 1 and q.rownumber BETWEEN convert(int,(@page-1)*10) AND convert(int,@page*10)';

print @queryTotal;

execute sp_executesql @queryTotal,N'@total int out',@total out;
execute sp_executesql @queryPage,N'@page int',@page;