
CREATE FUNCTION [dbo].[ConvertToMoney]
(
	@Input			money=0
)
RETURNS nvarchar(200)
AS
BEGIN
declare @Output				nvarchar(200)
SELECT

   @Output=  REPLACE(FORMAT(ISNULL(@Input,0), N'N', C.culture),',00','')

FROM
    (
        VALUES
           ('el-GR')
    ) C (culture);
return @Output	
END