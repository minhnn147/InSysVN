CREATE PROC [dbo].[sp_User_ResetPassword] 
    @Email NVARCHAR(255)
    ,@Password NVARCHAR(255)
AS
UPDATE
	[dbo].[Users]
SET
	Password = @Password,
	ResetPassCode = ''
WHERE Email = @Email