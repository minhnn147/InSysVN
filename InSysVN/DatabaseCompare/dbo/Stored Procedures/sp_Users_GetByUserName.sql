--changngoktb
CREATE PROC [dbo].[sp_Users_GetByUserName]
@UserName varchar(max)
AS
SELECT * FROM dbo.Users WHERE UserName = @UserName AND isActive = 1