-- =============================================
CREATE PROCEDURE [dbo].[sp_Users_GetByPaging]
    @pageIndex INT = 1
    ,@pageSize INT = 10
    ,@filter NVARCHAR(MAX) = NULL
    ,@sort NVARCHAR(255) = NULL
    ,@keyword NVARCHAR(500) = NULL
    ,@totalRecord INT OUT
AS
BEGIN
    DECLARE @pagingQuery NVARCHAR(MAX);
    DECLARE @totalQuery NVARCHAR(MAX);
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED --WITH (NOLOCK)
    IF ISNULL(@sort, '') = ''
    BEGIN
        SET @sort = 'a.[Id]';
    END

    -- get total record
    SET @totalQuery = 'SELECT @totalRecordOut = COUNT(a.Id) FROM [dbo].[Users] a WHERE (1=1) ';

    --get data by paging
    SET @pagingQuery = 'SELECT tmp.* FROM '+ 
                            '(SELECT RowIndex = ROW_NUMBER() OVER (ORDER BY ' + @sort + 
                            '), *  FROM [dbo].[Users] a WHERE (1=1) ';
                            
    IF ISNULL(@filter,'') <> ''
    BEGIN
        DECLARE @filterQuery NVARCHAR(MAX) = ' AND (' + @filter + ')';
        SET @totalQuery += @filterQuery;
        SET @pagingQuery += @filterQuery;
    END

    IF ISNULL(@keyword,'') <> ''
    BEGIN
        DECLARE @keywordQuery NVARCHAR(MAX) = ' AND (a.FullName LIKE N''%' + @keyword + '%'' OR a.UserName LIKE N''%' + @keyword + '%'')';
        SET @totalQuery += @keywordQuery;
        SET @pagingQuery += @keywordQuery;
    END

    SET @pagingQuery += ') AS tmp WHERE RowIndex > '
                        + CAST((@pageIndex - 1) * @pageSize AS NVARCHAR(50)) + 
                        ' AND RowIndex <= ' 
                        + CAST((@pageIndex) * @pageSize AS NVARCHAR(50));
    PRINT @totalQuery
    --GO
    PRINT @pagingQuery
    EXECUTE sp_executesql @totalQuery
        ,N'@totalRecordOut int OUTPUT'
        ,@totalRecordOut = @totalRecord OUTPUT;

    EXECUTE (@pagingQuery);
END