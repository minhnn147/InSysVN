
CREATE FUNCTION [dbo].[ConvertToMoney]
(
)
RETURNS nvarchar(200)
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