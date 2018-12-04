CREATE PROC [dbo].[sp_Users_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id
        ,[FullName]
        ,[UserName]
        ,[Password]
        ,[Gender]
        ,[Address]
        ,[Phone]
        ,[Birthday]
        ,[CreatedDate]
        ,[ModifiedDate]
        ,Email
		,AvatarImg
    FROM [dbo].[Users]
	WHERE isActive = 1
END