
CREATE PROC [dbo].[sp_Users_GetDataAutoComplete]
@search NVARCHAR(500),
@page int,
@total int out
as
declare @query nvarchar(max);
declare @queryTotal nvarchar(max);
declare @queryPage nvarchar(max);



set @query ='	SELECT *
				FROM
				(
					SELECT	*,ROW_NUMBER() OVER(ORDER BY Id ASC) AS rownumber
					FROM	dbo.Users 
					WHERE	isActive = 1
							AND (	
									UserName LIKE ''%'+@search+'%''
									OR FullName LIKE N''%'+@search+'%''
									OR UserCode LIKE N''%'+@search+'%''
									OR Email LIKE N''%'+@search+'%''
								)
				) AS q
				WHERE 1 = 1 ';

set @queryTotal = 'select @total = COUNT(*) from ('+@query+') as tbltotal';

set @queryPage = @query+' and q.rownumber BETWEEN convert(int,(@page-1)*10) AND convert(int,@page*10)';

print @queryTotal;

execute sp_executesql @queryTotal,N'@total int out',@total out;
execute sp_executesql @queryPage,N'@page int',@page;