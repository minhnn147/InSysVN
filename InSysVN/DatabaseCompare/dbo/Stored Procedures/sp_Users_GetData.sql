
CREATE PROC [dbo].[sp_Users_GetData]
	@txtSearch				NVARCHAR(255) = NULL,
	@pageNumber				INT,
	@pageSize				INT,
	@order					NVARCHAR(255) = NULL,
	@sort					NVARCHAR(255) = NULL,
	@RoleId					INT,
	@RoleLevel				INT,
	@totalRecord			INT OUT
AS
BEGIN
	DECLARE @query NVARCHAR(MAX);
	DECLARE @queryCountTotal NVARCHAR(MAX);
    DECLARE @queryGetData NVARCHAR(MAX);
	DECLARE @querySort NVARCHAR(MAX);

	 IF		ISNULL(@order, '') <> '' AND ISNULL(@sort,'') <> ''
    BEGIN
		IF @sort = 'RoleDisplayName' SET @sort = 'r.DisplayName';
		IF @sort = 'ModifiedDate' SET @sort = 'u.ModifiedDate';
		IF @sort = 'Phone' SET @sort = 'u.Phone';
        SET @querySort = ' '+@sort+' '+@order+' ';
    END
	ELSE
	BEGIN
		SET @querySort = ' u.Id ASC ';
	END

	SET @query = '
					SELECT u.*
							,[RoleDisplayName] = r.DisplayName
							,rownumber = ROW_NUMBER() OVER(ORDER BY '+@querySort+')
					FROM dbo.Users u
					JOIN Roles r on u.RoleId = r.Id
					WHERE u.isActive = 1 and r.isShow = 1 AND r.[Level] > '+CONVERT(NVARCHAR(max),@RoleLevel)+'';

	IF ISNULL(@txtSearch,'') <> ''
		BEGIN
			SET @query = @query + ' AND (FullName LIKE N''%' + @txtSearch + '%''  OR FullName LIKE N''%' + @txtSearch + '%'' OR u.Email LIKE ''%'+@txtSearch+'%'')';
		END
	IF @RoleId IS NOT NULL
		BEGIN
			SET	@query = @query + ' and RoleId = '+CONVERT(NVARCHAR(MAX),@RoleId)+' ';
		END

	SET @queryCountTotal = 'SELECT	@totalRecord = COUNT(*) FROM ('+@query+') as qrall';

	SET @queryGetData = 'SELECT * FROM ('+@query+') as qr WHERE 1 = 1 ';
	IF ISNULL(@pageNumber,0) <> 0 AND ISNULL(@pageSize,0) <> 0
	BEGIN
		 SET @queryGetData = @queryGetData+' and qr.rownumber > '
	                    + CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(50)) + 
	                    ' AND qr.rownumber <= ' 
	                    + CAST((@pageNumber) * @pageSize AS NVARCHAR(50));
	END

EXECUTE sp_executesql @queryCountTotal, N'@totalRecord INT OUT',@totalRecord OUT;
EXECUTE sp_executesql @queryGetData;

END