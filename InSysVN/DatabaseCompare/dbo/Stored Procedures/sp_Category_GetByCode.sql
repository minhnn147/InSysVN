CREATE PROC [dbo].[sp_Category_GetByCode]
@Code NVARCHAR(MAX)
AS
BEGIN
	BEGIN
		SELECT Id ,
               Code ,
               Name ,
               ParentId ,
               CreatedDate ,
               ModifiedDate ,
               isActive 
		FROM dbo.Category WHERE Code = @Code AND isActive = 1	
	END
	
END