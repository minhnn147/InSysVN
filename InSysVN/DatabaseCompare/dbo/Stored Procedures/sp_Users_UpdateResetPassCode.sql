create PROC [dbo].[sp_Users_UpdateResetPassCode] 
    @UserName NVARCHAR(50)
    ,@Email NVARCHAR(255)
    ,@Code NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

	update
		a
	set
		a.ResetPassCode = @Code
    FROM [dbo].[Users] a
    WHERE [UserName] = @UserName
        AND Email = @Email

    SELECT TOP 1
        *
    FROM [dbo].[Users]
    WHERE [UserName] = @UserName
        AND Email = @Email
END