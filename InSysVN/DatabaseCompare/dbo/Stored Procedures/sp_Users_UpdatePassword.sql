--changngoktb
CREATE PROC [dbo].[sp_Users_UpdatePassword]
@UserId int,
@Password nvarchar(max)
AS
UPDATE dbo.Users 
SET [Password] = @Password 
WHERE Id = @UserId