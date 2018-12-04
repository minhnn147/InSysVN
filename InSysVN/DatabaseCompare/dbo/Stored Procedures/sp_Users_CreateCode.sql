CREATE PROC [dbo].[sp_Users_CreateCode]
@Prefix NVARCHAR(max)
AS
BEGIN
SELECT	CASE
			WHEN (SELECT TOP 1 1 FROM dbo.Users WHERE UserCode LIKE ''+@Prefix+'%'+'') IS NULL
			THEN
				@Prefix + '000000001'
			ELSE @Prefix + FORMAT(MAX(CONVERT(BIGINT,REPLACE(UserCode,@Prefix,'')))+1,'000000000')
		END
FROM dbo.Users
WHERE UserCode LIKE ''+@Prefix+'%'+''
END